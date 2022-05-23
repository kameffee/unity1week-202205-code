using System;
using System.Threading;
using UniRx;
using UnityEngine.UI;
using VContainer.Unity;

namespace Unity1week202205.Presentation.Tutorial
{
    /// <summary>
    /// チュートリアルプレゼンター
    /// </summary>
    public class TutorialPresenter : IStartable, IDisposable
    {
        private readonly ITutorialPerformer _performer;
        private readonly IGameReadyPerformer _gameReadyPerformer;
        private readonly Button _button;
        private readonly CompositeDisposable _disposable = new();
        private CancellationTokenSource _tokenSource;

        public TutorialPresenter(
            ITutorialPerformer performer,
            IGameReadyPerformer gameReadyPerformer,
            Button button)
        {
            _performer = performer;
            _gameReadyPerformer = gameReadyPerformer;
            _button = button;
        }

        public void Start()
        {
            _button.OnClickAsObservable()
                .Subscribe(async _ =>
                {
                    _tokenSource = new CancellationTokenSource();

                    _gameReadyPerformer.Hide(_tokenSource.Token);

                    await _performer.Open(_tokenSource.Token);

                    _gameReadyPerformer.Show(_tokenSource.Token);

                    _button.gameObject.SetActive(true);
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _tokenSource?.Dispose();
            _disposable?.Dispose();
        }
    }
}
