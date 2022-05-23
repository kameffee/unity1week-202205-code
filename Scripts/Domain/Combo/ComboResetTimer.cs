using System;
using UniRx;
using UnityEngine;

namespace Unity1week202205.Domain.Combo
{
    /// <summary>
    /// コンボのリセットタイマー
    /// </summary>
    public class ComboResetTimer
    {
        /// <summary>
        /// コンボがリセットされるまでの残り時間
        /// </summary>
        public IReadOnlyReactiveProperty<float> RemainingNormalizeTime => _remainingNormalizeTime;

        private readonly ReactiveProperty<float> _remainingNormalizeTime = new();

        /// <summary>
        /// 経過時間
        /// </summary>
        public IReadOnlyReactiveProperty<float> ElpsedTime => _elpsedTime;

        private readonly ReactiveProperty<float> _elpsedTime = new();

        public IObservable<Unit> OnReset => _onReset;
        private readonly Subject<Unit> _onReset = new();

        /// <summary>
        /// タイマーが動いているか
        /// </summary>
        public bool Starting => isStarting;

        /// <summary>
        /// 停止しているか
        /// </summary>
        public bool Stoping => !isStarting;

        // リセットされるまでの時間
        private float _resetTime;

        private bool isStarting = false;
        private IDisposable _timerDisposable;

        /// <param name="resetTime">リセットされるまでの時間</param>
        public ComboResetTimer(float resetTime)
        {
            
            _resetTime = resetTime;
        }

        public void Start()
        {
            isStarting = true;
            _elpsedTime.Value = 0;

            _timerDisposable?.Dispose();
            _timerDisposable = Observable.EveryUpdate()
                .Subscribe(_ => Update());
        }

        public void SetResetTime(float time)
        {
            _resetTime = time;
        }

        private void Update()
        {
            // 経過時間更新
            _elpsedTime.Value = Mathf.Min(_resetTime, _elpsedTime.Value + Time.deltaTime);
            // 消えるまでの到達率
            _remainingNormalizeTime.Value = Mathf.Clamp01(1f - (_elpsedTime.Value / _resetTime));

            if (Math.Abs(_elpsedTime.Value - _resetTime) < 0.0001f)
            {
                _onReset.OnNext(Unit.Default);
                Stop();
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            isStarting = false;
            _timerDisposable?.Dispose();
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause()
        {
            _timerDisposable?.Dispose();
        }

        /// <summary>
        /// 一時停止を解除
        /// </summary>
        public void Resume()
        {
            _timerDisposable?.Dispose();
            _timerDisposable = Observable.EveryUpdate()
                .Subscribe(_ => Update());
        }
    }
}
