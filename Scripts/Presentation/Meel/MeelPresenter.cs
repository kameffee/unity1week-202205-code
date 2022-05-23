using System;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UniRx;
using Unity1week202205.Domain.Meel;

namespace Unity1week202205.Presentation.Meel
{
    /// <summary>
    /// ミールプレゼンター
    /// </summary>
    public class MeelPresenter : IDisposable
    {
        private readonly MeelModel _model;
        private readonly MeelView _meelView;
        private readonly IPublisher<MeelBreakEvent> _breakPublisher;

        private readonly CompositeDisposable _disposable = new();

        public MeelPresenter(
            MeelModel model,
            MeelView meelView,
            IPublisher<MeelBreakEvent> meelBreakEventPublisher)
        {
            _model = model;
            _meelView = meelView;
            _breakPublisher = meelBreakEventPublisher;
        }

        public void Initialzie()
        {
            _meelView.Initialize(_model.Id, _model.MeelProfile);

            _meelView.Position
                .Subscribe(pos => _model.SetPosition(pos))
                .AddTo(_disposable);

            _model.IsFreeze
                .Subscribe(isFreeze => _meelView.SetFreeze(isFreeze))
                .AddTo(_disposable);

            _model.OnBreak
                .Subscribe(async delay =>
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delay));
                    await _meelView.Delete();
                    _breakPublisher.Publish(new MeelBreakEvent(_model.TypeId, _model.Position, _model.Size));
                    _model.OnCompleteBreak();
                    Dispose();
                })
                .AddTo(_disposable);

            _model.Selected
                .Subscribe(isSelected => _meelView.SetSelected(isSelected, _model.SelectedNumber))
                .AddTo(_disposable);

            _model.IsLastSelected
                .Subscribe(isNow => _meelView.SetLastSelected(isNow, _model.SelectedNumber))
                .AddTo(_disposable);

            _model.OnSpine
                .Subscribe(_ => _meelView.Spine())
                .AddTo(_disposable);

            if (_model.Size.IsBig())
            {
                _meelView.Expansion(0.5f, 1f);
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
