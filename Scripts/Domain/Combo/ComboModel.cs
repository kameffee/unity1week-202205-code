using System;
using UniRx;

namespace Unity1week202205.Domain.Combo
{
    /// <summary>
    /// コンボ Model
    /// </summary>
    public class ComboModel : IDisposable
    {
        public IReadOnlyReactiveProperty<int> CurrentCombo => _currentCombo;
        private readonly ReactiveProperty<int> _currentCombo = new();

        public int MaxCombo => _maxCombo;
        private int _maxCombo;

        public void Add()
        {
            _currentCombo.Value++;
            
            // 最大コンボ数を更新
            if (_currentCombo.Value > _maxCombo)
            {
                _maxCombo = _currentCombo.Value;
            }
        }

        public void Reset()
        {
            _maxCombo = 0;
            _currentCombo.Value = 0;
        }

        /// <summary>
        /// 最大コンボ数なども初期化する
        /// </summary>
        public void Clear()
        {
            _maxCombo = 0;
            _currentCombo.Value = 0;
        }

        public void Dispose()
        {
            _currentCombo?.Dispose();
        }
    }
}
