using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202205.Presentation.Fever
{
    public class FeverGaugeView : MonoBehaviour, IFeverView
    {
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private Image _gauge;

        [SerializeField]
        private Color _defaultColor;
        
        [SerializeField]
        private Color _feverColor;

        public void Render(float value)
        {
            _slider.DOValue(Mathf.Clamp01(value), 0.2f);
        }

        public void FeverStart()
        {
            _gauge.DOColor(_feverColor, 0.2f);
        }

        public void FeverStop()
        {
            _gauge.DOColor(_defaultColor, 0.2f);
        }
    }
}
