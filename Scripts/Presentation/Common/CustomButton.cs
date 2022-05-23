using System;
using DG.Tweening;
using Kameffee.AudioPlayer;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity1week202205.Presentation.Common
{
    /// <summary>
    /// ボタンのアニメーションなど
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var rectTransform = _button.transform as RectTransform;
            rectTransform.DOScale(0.95f, 0.15f)
                .SetEase(Ease.OutCirc)
                .SetLink(gameObject);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var rectTransform = _button.transform as RectTransform;
            rectTransform.DOScale(1f, 0.25f)
                .SetEase(Ease.OutBack)
                .SetLink(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioPlayer.Instance.Se.Play("button_click");
        }
    }
}
