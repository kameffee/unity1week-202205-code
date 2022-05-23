using Cysharp.Threading.Tasks;
using DG.Tweening;
using Kameffee.AudioPlayer;
using Spine.Unity;
using UniRx;
using Unity1week202205.Data;
using Unity1week202205.Domain.Meel;
using UnityEngine;

namespace Unity1week202205.Presentation.Meel
{
    /// <summary>
    /// ミールの表示
    /// </summary>
    public class MeelView : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation _spineAnimation;

        [SerializeField]
        private Transform _rendererRoot;

        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        [SerializeField]
        private MeelSelectCountView _countView;

        [Header("Spine Animations")]
        [SpineAnimation]
        [SerializeField]
        private string _selectedState;

        [SpineAnimation]
        [SerializeField]
        private string _breakState;

        public int Id { get; private set; }

        public IReadOnlyReactiveProperty<Vector3> Position => _position;
        private readonly ReactiveProperty<Vector3> _position = new();

        private MeelPooler _pooler;
        private Color _defaultColor;

        public void Initialize(int id, MeelProfile meelProfile)
        {
            Id = id;
            _renderer.color = meelProfile.Color;
            _defaultColor = meelProfile.Color;
            // スキンを適応
            _spineAnimation.skeleton.SetSkin(meelProfile.SkinName);
            _spineAnimation.state.SetEmptyAnimation(1, 0.0f);
            _spineAnimation.state.SetEmptyAnimation(2, 0.0f);
        }

        public void SetMeelPooler(MeelPooler meelPooler)
        {
            _pooler = meelPooler;
        }

        /// <summary>
        /// 膨れ上がる演出
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="toSize"></param>
        public void Expansion(float startSize, float toSize)
        {
            transform.localScale = Vector3.one * startSize;
            transform.DOScale(toSize, 0.3f).SetLink(gameObject);
        }

        private void Update()
        {
            _position.Value = transform.position;
        }

        public async UniTask Delete()
        {
            var trackEntry = _spineAnimation.state.SetAnimation(2, _breakState, false);
            await transform.DOScale(0, 0.2f).SetLink(gameObject).SetEase(Ease.InBack);
            AudioPlayer.Instance.Se.Play("meel_splash");
            _pooler.Return(this);
        }

        public void SetFreeze(bool isFreeze)
        {
            _rigidbody.constraints = isFreeze ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
        }

        public void SetSelected(bool isSelected, MeelSelectedNumber count)
        {
            if (isSelected)
            {
                _countView.RenderNumber(count.Value);

                AudioPlayer.Instance.Se.Play("meel_select");

                // 手前に表示
                _spineAnimation.GetComponent<MeshRenderer>().sortingOrder = 1;

                // ちょっと明るくする
                var color = _defaultColor;
                color.r = Mathf.Clamp01(color.r + 0.1f);
                color.g = Mathf.Clamp01(color.g + 0.1f);
                color.b = Mathf.Clamp01(color.b + 0.1f);
                _renderer.color = color;
                _spineAnimation.state.SetAnimation(1, _selectedState, true);
            }
            else
            {
                _countView.Hide();

                // 元に戻す
                _spineAnimation.GetComponent<MeshRenderer>().sortingOrder = 0;
                _renderer.color = _defaultColor;
                _spineAnimation.state.SetEmptyAnimation(1, 0.2f);
            }
        }

        public void SetLastSelected(bool isLast, MeelSelectedNumber count)
        {
            if (isLast)
            {
                _countView.RenderNumber(count.Value);
            }
            else
            {
                _countView.Hide();
            }

            _renderer.sortingOrder = isLast ? 1 : 0;
            _rendererRoot.DOScale(isLast ? 1.25f : 1, 0.2f);
        }

        public void Spine()
        {
            _rigidbody.AddTorque(Random.Range(1f, 3f), ForceMode2D.Impulse);
            _rigidbody.AddForce(new Vector2(Random.Range(-5f, 5f), Random.Range(1f, 5f)), ForceMode2D.Impulse);
        }
    }
}
