using System;
using UniRx;
using Unity1week202205.Domain;
using VContainer.Unity;

namespace Unity1week202205.Presentation
{
    public class SpinPresenter : IStartable, IDisposable
    {
        private readonly ISpinView _view;
        private readonly SpinService _spinService;
        private readonly CompositeDisposable _disposable = new();

        public SpinPresenter(ISpinView view, SpinService spinService)
        {
            _view = view;
            _spinService = spinService;
        }

        public void Start()
        {
            _spinService.Spinable
                .Subscribe(spinable => _view.SetActive(spinable))
                .AddTo(_disposable);

            _spinService.OnSpine
                .Subscribe(_ => _view.Spin())
                .AddTo(_disposable);

            _view.OnClickSpin
                .Subscribe(_ => _spinService.Spin())
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
