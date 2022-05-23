using UniRx.Toolkit;
using UnityEngine;

namespace Unity1week202205.Presentation.MeelChain
{
    /// <summary>
    /// チェイン角のプール
    /// </summary>
    public class CornerPointPooler : ObjectPool<CornerPointView>
    {
        private readonly CornerPointView _prefab;
        private readonly Transform _parent;

        public CornerPointPooler(CornerPointView prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        protected override CornerPointView CreateInstance()
        {
            return Object.Instantiate(_prefab, _parent);
        }
    }
}
