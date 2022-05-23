using System;
using MessagePipe;
using UniRx;
using Unity1week202205.Domain;
using Unity1week202205.Domain.Score;
using VContainer.Unity;

namespace Unity1week202205.Presentation.Score
{
    /// <summary>
    /// スコアプレゼンター
    /// </summary>
    public class ScorePresenter : IStartable, IDisposable
    {
        private readonly ISubscriber<ChangedScore> _changeScoreSubscriber;
        private readonly ScoreModel _scoreModel;
        private readonly IScoreView _scoreView;

        private CompositeDisposable _disposable = new();

        public ScorePresenter(ISubscriber<ChangedScore> changeScoreSubscriber, ScoreModel scoreModel, IScoreView scoreView)
        {
            _changeScoreSubscriber = changeScoreSubscriber;
            _scoreModel = scoreModel;
            _scoreView = scoreView;
        }

        public void Start()
        {
            _scoreView.Render(new ChangedScore(0, _scoreModel.Score.Value));

            var d = DisposableBag.CreateBuilder();
            _changeScoreSubscriber
                .Subscribe(score => _scoreView.Render(score))
                .AddTo(d);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
