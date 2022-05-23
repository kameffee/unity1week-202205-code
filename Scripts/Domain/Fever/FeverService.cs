using System;
using UniRx;
using UnityEngine;
using IInitializable = VContainer.Unity.IInitializable;

namespace Unity1week202205.Domain.Fever
{
    /// <summary>
    /// フィーバーサービス
    /// </summary>
    public class FeverService : IInitializable, IDisposable
    {
        /// フィーバー中か
        /// <summary>
        /// </summary>
        public bool IsFevering => _feverTimer.IsFeverTime.Value;

        /// <summary>
        /// フィーバー開始
        /// </summary>
        public IObservable<Unit> OnStartFever => _feverTimer.OnStartFever;

        /// <summary>
        /// フィーバー終了
        /// </summary>
        public IObservable<Unit> OnFinishFever => _feverTimer.OnEndFever;

        private readonly FeverGaugeModel _feverGaugeModel;
        private readonly FeverTimer _feverTimer;

        private CompositeDisposable _disposable = new();

        public FeverService(FeverGaugeModel feverGaugeModel, FeverTimer feverTimer)
        {
            _feverGaugeModel = feverGaugeModel;
            _feverTimer = feverTimer;
        }

        public void Initialize()
        {
            // 自動減少
            Observable.EveryUpdate()
                .Subscribe(_ => AutoDecrease())
                .AddTo(_disposable);

            _feverTimer.OnEndFever
                .Subscribe(_ => _feverGaugeModel.Reset())
                .AddTo(_disposable);
        }

        public void AddFeverGauge(float value)
        {
            if (_feverTimer.IsFeverTime.Value)
            {
                // フィーバー中
                return;
            }

            _feverGaugeModel.Add(value);
            if (_feverGaugeModel.IsMax)
            {
                _feverTimer.Start(10);
            }
        }

        private void AutoDecrease()
        {
            if (_feverTimer.IsFeverTime.Value)
            {
                // フェーバータイム中
                return;
            }

            _feverGaugeModel.Add(-10f * Time.deltaTime);
        }

        public void Pause()
        {
            if (_feverTimer.IsFeverTime.Value)
            {
                _feverTimer.Pause();
            }
        }

        public void UnPause()
        {
            if (_feverTimer.IsFeverTime.Value)
            {
                _feverTimer.UnPause();
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void Reset()
        {
            _feverGaugeModel.Reset();
            _feverTimer.Stop();
        }
    }
}
