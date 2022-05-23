using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using Unity1week202205.Domain.Combo;
using Unity1week202205.Domain.Score;
using Unity1week202205.Presentation.Performer;

namespace Unity1week202205.Domain
{
    public class InGameEndSequencer
    {
        private readonly IGameEndPerformer _performer;
        private readonly ScoreModel _scoreModel;
        private readonly ComboModel _comboModel;
        private readonly IPublisher<ResultData> _resultPublisher;

        public InGameEndSequencer(
            IGameEndPerformer performer,
            ScoreModel scoreModel,
            ComboModel comboModel,
            IPublisher<ResultData> resultPublisher)
        {
            _performer = performer;
            _scoreModel = scoreModel;
            _comboModel = comboModel;
            _resultPublisher = resultPublisher;
        }

        public async UniTask Start(CancellationToken cancellationToken)
        {
            // 終了演出再生
            await _performer.Play(cancellationToken);

            // スコア表示
            _resultPublisher.Publish(new ResultData(_scoreModel.Score.Value, _comboModel.MaxCombo));
        }
    }
}
