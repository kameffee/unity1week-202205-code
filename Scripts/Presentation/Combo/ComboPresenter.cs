using UniRx;
using Unity1week202205.Domain;
using Unity1week202205.Domain.Combo;
using VContainer.Unity;

namespace Unity1week202205.Presentation.Combo
{
    public class ComboPresenter : IStartable
    {
        private readonly ComboModel _model;
        private readonly ComboResetTimer _comboResetTimer;
        private readonly IComboView _view;

        private readonly CompositeDisposable _disposable = new();

        public ComboPresenter(ComboModel model, ComboResetTimer comboResetTimer, IComboView view)
        {
            _model = model;
            _comboResetTimer = comboResetTimer;
            _view = view;
        }

        public void Start()
        {
            // コンボ数
            _model.CurrentCombo
                .Where(combo => combo > 0)
                .Subscribe(combo => _view.Render(combo))
                .AddTo(_disposable);

            // コンボが切れるまでの残り時間ゲージ更新
            _comboResetTimer.RemainingNormalizeTime
                .Subscribe(normalize => _view.RenderGauge(normalize))
                .AddTo(_disposable);
        }
    }
}
