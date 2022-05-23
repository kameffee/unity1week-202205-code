using System;
using UniRx;
using Unity1week202205.Domain;
using VContainer.Unity;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// ステータス
    /// </summary>
    public class GameStatusPresenter : IStartable, IDisposable
    {
        private readonly GameTimer _gameTimer;
        private readonly IRemainingTime _remainigTimeView;
        private readonly CompositeDisposable _disposable = new();

        public GameStatusPresenter(
            GameTimer gameTimer,
            IRemainingTime remainigTimeView)
        {
            _gameTimer = gameTimer;
            _remainigTimeView = remainigTimeView;
        }

        public void Start()
        {
            _gameTimer.RemainingTime
                .Subscribe(time => _remainigTimeView.Render(time))
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
