using System;
using UniRx;
using UnityEngine;

namespace Unity1week202205.Domain.Fever
{
    /// <summary>
    /// フェーバータイム管理
    /// </summary>
    public class FeverTimer : IDisposable
    {
        /// <summary>
        /// フィーバー中か
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsFeverTime => _isFeverTime;

        /// <summary>
        /// フィーバー開始時
        /// </summary>
        public IObservable<Unit> OnStartFever => _onStartFever;

        /// <summary>
        /// フィーバー残り時間
        /// </summary>
        public IReadOnlyReactiveProperty<float> RemainingTimeNormalize => _remainingTimeNormalize;

        /// <summary>
        /// フィーバー終了時
        /// </summary>
        public IObservable<Unit> OnEndFever => _onEndFever;

        /// <summary>
        /// ポーズ中
        /// </summary>
        public bool IsPause => _isPause;

        private readonly ReactiveProperty<bool> _isFeverTime = new();
        private readonly Subject<Unit> _onStartFever = new();
        private readonly Subject<Unit> _onEndFever = new();
        private readonly ReactiveProperty<float> _remainingTimeNormalize = new();

        private readonly CompositeDisposable _disposable = new();

        // フィーバー時間
        private float _feverTime;

        // 経過時間
        private float _elpsedTime;

        // ポーズ状態か
        private bool _isPause;

        private IDisposable _timerDisposable;

        /// <summary>
        /// フィーバー開始
        /// </summary>
        /// <param name="feverTime"></param>
        public void Start(float feverTime)
        {
            _feverTime = feverTime;
            _isFeverTime.Value = true;
            _onStartFever.OnNext(Unit.Default);
            _elpsedTime = 0;
            _isPause = false;

            _timerDisposable = Observable.EveryUpdate()
                .Where(_ => !_isPause)
                .Subscribe(_ => UpdateTime())
                .AddTo(_disposable);
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _onEndFever.OnNext(Unit.Default);
            _timerDisposable?.Dispose();
            _isFeverTime.Value = false;
        }

        private void UpdateTime()
        {
            _elpsedTime += Time.deltaTime;
            _remainingTimeNormalize.Value = Mathf.Clamp01(1f - _elpsedTime / _feverTime);
            if (_elpsedTime >= _feverTime)
            {
                Stop();
            }
        }

        /// <summary>
        /// 一時停止
        /// </summary>
        public void Pause()
        {
            _isPause = true;
        }

        /// <summary>
        /// 一時停止を解除
        /// </summary>
        public void UnPause()
        {
            _isPause = false;
        }

        public void Dispose()
        {
            _timerDisposable?.Dispose();
            _onStartFever?.Dispose();
            _onEndFever?.Dispose();
            _isFeverTime?.Dispose();
            _disposable?.Dispose();
        }
    }
}
