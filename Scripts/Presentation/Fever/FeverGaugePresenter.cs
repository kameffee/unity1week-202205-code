using System;
using Cysharp.Threading.Tasks;
using Unity1week202205.Domain.Fever;
using VContainer.Unity;
using UniRx;

namespace Unity1week202205.Presentation.Fever
{
    public class FeverGaugePresenter : IStartable, IDisposable
    {
        private readonly FeverGaugeModel _feverGaugeModel;
        private readonly IFeverView _view;
        private readonly FeverTimer _feverTimer;

        private readonly CompositeDisposable _disposable = new();

        public FeverGaugePresenter(FeverGaugeModel feverGaugeModel, IFeverView view, FeverTimer feverTimer)
        {
            _feverGaugeModel = feverGaugeModel;
            _view = view;
            _feverTimer = feverTimer;
        }

        public void Start()
        {
            // 通常時のゲージ変動
            _feverGaugeModel.Normalize
                .Where(_ => !_feverTimer.IsFeverTime.Value)
                .Subscribe(percent => _view.Render(percent))
                .AddTo(_disposable);

            // フィーバー中のゲージ変動
            _feverTimer.RemainingTimeNormalize
                .Where(_ => _feverTimer.IsFeverTime.Value)
                .Subscribe(normalize => _view.Render(normalize))
                .AddTo(_disposable);

            _feverTimer.OnStartFever.Subscribe(_ => _view.FeverStart()).AddTo(_disposable);
            _feverTimer.OnEndFever.Subscribe(_ => _view.FeverStop()).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
