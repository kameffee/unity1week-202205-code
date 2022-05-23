using System;
using MessagePipe;
using UniRx;
using Unity1week202205.Domain.Meel;
using VContainer.Unity;

namespace Unity1week202205.Presentation.Meel
{
    /// <summary>
    /// ミールが弾けるときのエフェクト
    /// </summary>
    public class MeelBreakEffectPresenter : IStartable, IDisposable
    {
        private readonly ISubscriber<MeelBreakEvent> _meelBreakEvent;
        private readonly IMeelSplashEffect _splashEffect;
        private readonly CompositeDisposable _disposable = new();

            public MeelBreakEffectPresenter(ISubscriber<MeelBreakEvent> meelBreakEvent, IMeelSplashEffect splashEffect)
            {
                _meelBreakEvent = meelBreakEvent;
                _splashEffect = splashEffect;
            }

        public void Start()
        {
            var disposable = DisposableBag.CreateBuilder();
            _meelBreakEvent
                    .Subscribe(breakEvent =>
                    {
                        _splashEffect.Play(breakEvent.Position);
                    })
                    .AddTo(disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
