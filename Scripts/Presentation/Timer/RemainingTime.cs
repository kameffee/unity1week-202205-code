using System;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// 残り時間
    /// </summary>
    public readonly struct RemainingTime
    {
        /// <summary>
        /// 経過時間
        /// </summary>
        public float ElpsedTime => _elpsedTime;
        
        /// <summary>
        /// 最大時間
        /// </summary>
        public float MaxTime => _maxTime;

        /// <summary>
        /// 正規化した値 (0.0 ~ 1.0)
        /// </summary>
        public float Normalize => (_maxTime - _elpsedTime) / _maxTime;
        
        /// <summary>
        /// 残り時間
        /// </summary>
        public TimeSpan Value => _remainingValue;

        private readonly float _maxTime;
        private readonly float _elpsedTime;
        private readonly TimeSpan _remainingValue;

        /// <param name="maxTime">最大時間</param>
        /// <param name="elpsedTime">経過時間</param>
        public RemainingTime(float maxTime, float elpsedTime = 0) : this()
        {
            _maxTime = maxTime;
            _elpsedTime = elpsedTime;
            _remainingValue = TimeSpan.FromSeconds(_maxTime) - TimeSpan.FromSeconds(_elpsedTime);
        }
    }
}
