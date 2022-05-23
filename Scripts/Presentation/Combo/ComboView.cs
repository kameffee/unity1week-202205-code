using DG.Tweening;
using MPUIKIT;
using TMPro;
using UnityEngine;

namespace Unity1week202205.Presentation.Combo
{
    /// <summary>
    /// コンボ数表示
    /// </summary>
    public class ComboView : MonoBehaviour, IComboView
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TextMeshProUGUI _comboValue;

        [SerializeField]
        private MPImage _gauge;

        private void Awake()
        {
            _comboValue.SetText("0");
        }

        public void Render(int combo)
        {
            _canvasGroup.DOFade(1, 0f);

            _comboValue.SetText("{0}", combo);

            _gauge.DOFillAmount(1, 0.2f).SetEase(Ease.OutSine);

            // アニメーション
            var text = _comboValue.transform;
            Sequence sequence = DOTween.Sequence();
            sequence.Join(text.DOScaleY(1.2f, 0.2f));
            sequence.Join(text.DOScaleX(0.9f, 0.2f));
            sequence.Append(text.DOScale(1, 0.3f).SetEase(Ease.OutBack));
            sequence.SetLink(gameObject);
            sequence.Play();
        }

        public void RenderGauge(float normalize)
        {
            _gauge.fillAmount = normalize;
        }
    }
}
