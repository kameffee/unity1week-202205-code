using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// シャッフルボタン
    /// </summary>
    public class SpinButton : MonoBehaviour, ISpinView
    {
        [SerializeField]
        private Button _spineButton;

        [SerializeField]
        private Image _spineIcon;

        public IObservable<Unit> OnClickSpin => _spineButton.OnClickAsObservable();

        public void SetActive(bool isActive)
        {
            _spineButton.interactable = isActive;
        }

        public void Spin()
        {
            _spineIcon.rectTransform.DORotate(Vector3.forward * 360f * 2, 1, RotateMode.LocalAxisAdd);
        }
    }
}
