using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// 設定画面ビュー
    /// </summary>
    public class OptionView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private RectTransform _windowRoot;

        [SerializeField]
        private Button _closeButton;

        public IObservable<Unit> OnClickClose => _closeButton.OnClickAsObservable();

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public async UniTask Open(CancellationToken cancellationToken = default)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1, 0.2f).SetEase(Ease.Linear));
            sequence.Join(_windowRoot.DOAnchorPosY(-30f, 0.2f).From(true).SetEase(Ease.OutCubic));
            sequence.SetLink(gameObject);
            sequence.WithCancellation(cancellationToken);
            await sequence.Play();
        }

        public async UniTask Close(CancellationToken cancellationToken = default)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0, 0f).SetEase(Ease.Linear));
            sequence.SetLink(gameObject);
            sequence.WithCancellation(cancellationToken);
            await sequence.Play();
        }
    }
}
