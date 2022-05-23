using UniRx.Toolkit;
using Unity1week202205.Presentation.Meel;
using UnityEngine;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// ミールのプール
    /// </summary>
    public class MeelPooler : ObjectPool<MeelView>
    {
        private readonly MeelView _prefab;
        private Transform _parent;
            
        public MeelPooler(MeelView prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        protected override MeelView CreateInstance()
        {
            var instance =  Object.Instantiate(_prefab, _parent);
            instance.SetMeelPooler(this);
            return instance;
        }

        protected override void OnBeforeReturn(MeelView instance)
        {
            base.OnBeforeReturn(instance);
            instance.transform.localScale = Vector3.one;
        }
    }
}
