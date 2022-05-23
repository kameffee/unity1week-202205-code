using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Unity1week202205.Presentation.Performer
{
    /// <summary>
    /// ゲーム開始時演出
    /// </summary>
    public class GameStartPerformer : MonoBehaviour, IGameStartPerformer
    {
        [Header("Ready")]
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [Header("Start")]
        [SerializeField]
        private CanvasGroup _startCanvas;

        [SerializeField]
        private RectTransform _startRoot;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _startCanvas.alpha = 0;
        }

        public async UniTask Play(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(1, 0.2f)
                .WithCancellation(cancellationToken);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);

            _canvasGroup.DOFade(0, 0f)
                .WithCancellation(cancellationToken);

            _startRoot.localScale = Vector3.one * 0.2f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_startCanvas.DOFade(1, 0.2f).SetEase(Ease.Linear));
            sequence.Append(_startRoot.DOScale(1, 0.2f).SetEase(Ease.OutBack));
            sequence.AppendInterval(0.5f);
            sequence.Append(_startCanvas.DOFade(0, 0.2f));
            sequence.WithCancellation(cancellationToken);
            await sequence.Play();
        }
    }
}
