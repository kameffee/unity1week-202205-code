using System;
using UniRx;
using Unity1week202205.Domain;
using VContainer.Unity;

namespace Unity1week202205.Presentation
{
    public class AddTimePresenter : IStartable, IDisposable
    {
        private readonly GameTimer _gameTimer;
        private readonly IAddTimePerformer _performer;
        private readonly CompositeDisposable _disposable = new();

        public AddTimePresenter(GameTimer gameTimer, IAddTimePerformer performer)
        {
            _gameTimer = gameTimer;
            _performer = performer;
        }

        public void Start()
        {
            // 時間が追加されたとき
            _gameTimer.OnAddTime
                .Subscribe(time => _performer.Play(time))
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
