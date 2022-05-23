using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Unity1week202205.Presentation.MeelChain
{
    /// <summary>
    /// ミールを繋ぐ線
    /// </summary>
    public class MeelChainView : MonoBehaviour, IMeelChainView
    {
        [SerializeField]
        private LineRenderer _lineRenderer;

        [SerializeField]
        private Transform _holder;
        
        [SerializeField]
        private CornerPointView _cornerPointPrefab;

        private CornerPointPooler _cornerPointPooler;

        private List<CornerPointView> _cache = new();

        private void Awake()
        {
            _cornerPointPooler = new CornerPointPooler(_cornerPointPrefab, _holder);
            _cornerPointPooler.PreloadAsync(10, 1).Subscribe().AddTo(this);
        }

        public void Render(Vector3[] points)
        {
            // ラインを引く
            _lineRenderer.positionCount = points.Length;
            _lineRenderer.SetPositions(points);

            if (_cache.Count < points.Length)
            {
                // 足りない分を足す
                var num = points.Length - _cache.Count;
                for (int i = 0; i < num; i++)
                {
                    _cache.Add(_cornerPointPooler.Rent());
                }
            }

            if (_cache.Count > points.Length)
            {
                // 余計な分を削除
                var num = _cache.Count - points.Length;
                for (var i = num - 1; i >= 0; i--)
                {
                    var view = _cache[i];
                    _cornerPointPooler.Return(view);
                    _cache.Remove(view);
                }
            }

            if (points.Length == 0)
            {
                // 全消し
                for (var i = _cache.Count - 1; i >= 0; i--)
                {
                    var view = _cache[i];
                    _cornerPointPooler.Return(view);
                    _cache.Remove(view);
                }
            }
            
            for (var i = 0; i < points.Length; i++)
            {
                _cache[i].transform.position = points[i];
            }
        }
    }
}
