using System;
using UniRx;

namespace Unity1week202205.Domain
{
    /// <summary>
    /// ミールをランダムに動かすサービス
    /// 詰まり対策用
    /// </summary>
    public class SpinService
    {
        public IObservable<Unit> OnSpine => _onSpine;
        private readonly Subject<Unit> _onSpine = new();

        public IReadOnlyReactiveProperty<bool> Spinable => _spinable;
        private readonly BoolReactiveProperty _spinable = new();

        private readonly BeakerModel _beakerModel;

        public SpinService(BeakerModel beakerModel)
        {
            _beakerModel = beakerModel;
        }

        /// <summary>
        /// シャッフルの実行
        /// </summary>
        public void Spin()
        {
            foreach (var meelModel in _beakerModel.All())
            {
                meelModel.Spine();
            }

            _onSpine.OnNext(Unit.Default);
        }

        /// <summary>
        /// 有効状態の切り替え
        /// </summary>
        /// <param name="isActive"></param>
        public void SetActive(bool isActive)
        {
            _spinable.Value = isActive;
        }
    }
}
