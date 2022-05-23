using System;
using UniRx;
using UnityEngine;

namespace Unity1week202205.Domain.Fever
{
    /// <summary>
    /// フィーバーゲージ Model
    /// </summary>
    public class FeverGaugeModel : IDisposable
    {
        public IReadOnlyReactiveProperty<float> Normalize => _normalize;
        private readonly ReactiveProperty<float> _normalize = new();

        public IReadOnlyReactiveProperty<float> Fever => _fever;
        private readonly ReactiveProperty<float> _fever = new();

        public IObservable<Unit> OnMax => _onMax;
        private readonly Subject<Unit> _onMax = new();

        public bool IsMax => Math.Abs(_fever.Value - _maxValue) < 0.0001;

        private float _maxValue;

        private IDisposable _timerDisposable;
        private readonly CompositeDisposable _disposable = new();

        public FeverGaugeModel(float maxValue)
        {
            _maxValue = maxValue;
        }

        public void Add(float value)
        {
            _fever.Value = Mathf.Min(_fever.Value + value, _maxValue);
            _normalize.Value = Mathf.Clamp01(_fever.Value / _maxValue);
            if (Math.Abs(_normalize.Value - 1f) < 0.0001f)
            {
                _onMax.OnNext(Unit.Default);
            }
        }

        public void Reset()
        {
            _fever.Value = 0;
            _normalize.Value = 0;
        }

        public void Dispose()
        {
            _normalize?.Dispose();
            _fever?.Dispose();
            _onMax?.Dispose();
            _timerDisposable?.Dispose();
            _disposable?.Dispose();
        }
    }
}
