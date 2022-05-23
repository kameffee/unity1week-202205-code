using System;
using UniRx;
using Unity1week202205.Presentation;
using UnityEngine;
using Unit = UniRx.Unit;

namespace Unity1week202205.Domain
{
    /// <summary>
    /// ゲーム内タイマー
    /// </summary>
    public class GameTimer : IDisposable
    {
        /// <summary>
        /// 経過時間
        /// </summary>
        public IReadOnlyReactiveProperty<float> ElpsedTime => _elpsedTime;

        /// <summary>
        /// 残り時間
        /// </summary>
        public IReadOnlyReactiveProperty<RemainingTime> RemainingTime => _remainingTime;

        /// <summary>
        /// タイムアップ時
        /// </summary>
        public IObservable<Unit> OnTimeUp => _onTimeUp;

        /// <summary>
        /// 時間が追加通知
        /// </summary>
        /// <returns></returns>
        public IObservable<TimeSpan> OnAddTime => _onAddTime;
        
        private readonly ReactiveProperty<float> _elpsedTime = new();
        private readonly ReactiveProperty<RemainingTime> _remainingTime = new();
        private readonly Subject<Unit> _onTimeUp = new();
        private readonly Subject<TimeSpan> _onAddTime = new();
        private readonly CompositeDisposable _disposable = new();

        private float _initialTime;

        public GameTimer(float initialTime)
        {
            _initialTime = initialTime;
            _remainingTime.Value = new RemainingTime(initialTime, 0);
        }

        /// <summary>
        /// カウントの開始
        /// </summary>
        public void Start()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => Update())
                .AddTo(_disposable);
        }

        private void Update()
        {
            // 前回の時間からの差分
            _elpsedTime.Value += Time.deltaTime;
            _remainingTime.Value = new RemainingTime(_initialTime, _elpsedTime.Value);

            // 残り時間が0以下になったら
            if (_remainingTime.Value.Value.TotalSeconds < 0.0f)
            {
                _remainingTime.Value = new RemainingTime(_initialTime, _initialTime);
                _onTimeUp.OnNext(Unit.Default);
                Stop();
            }
        }

        /// <summary>
        /// タイマーカウントを停止
        /// </summary>
        public void Stop()
        {
            _disposable?.Clear();
        }

        public void Reset()
        {
            _elpsedTime.Value = 0f;
            _remainingTime.Value = new RemainingTime(_initialTime, 0);
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause()
        {
            _disposable?.Clear();
        }

        /// <summary>
        /// 一時停止を解除
        /// </summary>
        public void UnPause()
        {
            _disposable.Clear();
            Observable.EveryUpdate()
                .Subscribe(_ => Update())
                .AddTo(_disposable);
        }

        /// <summary>
        /// 残り時間を追加
        /// </summary>
        /// <param name="time"></param>
        public void AddTime(TimeSpan time)
        {
            _elpsedTime.Value = Mathf.Max(0, _elpsedTime.Value - (float)time.TotalSeconds);
            _onAddTime.OnNext(time);
        }

        public void Dispose()
        {
            _elpsedTime?.Dispose();
            _remainingTime?.Dispose();
            _onTimeUp?.Dispose();
            _onAddTime?.Dispose();
            _disposable?.Dispose();
        }
    }
}
