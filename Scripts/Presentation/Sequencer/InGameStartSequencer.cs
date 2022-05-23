using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202205.Presentation.Performer;

namespace Unity1week202205.Domain
{
    public class InGameStartSequencer
    {
        private readonly IGameStartPerformer _performer;

        public InGameStartSequencer(IGameStartPerformer performer)
        {
            _performer = performer;
        }

        public async UniTask Start(CancellationToken cancellationToken)
        {
            // TODO: カウント
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);

            await _performer.Play(cancellationToken);
        }
    }
}
