using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Unity1week202205.Presentation.Tutorial
{
    /// <summary>
    /// チュートリアル表示
    /// </summary>
    public class TutorialPerformer : MonoBehaviour, ITutorialPerformer
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private List<TutorialPageView> _pages;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
        }

        public async UniTask Open(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(1, 0.2f);

            foreach (var tutorialPageView in _pages)
            {
                await tutorialPageView.Open(cancellationToken);
                await tutorialPageView.Close(cancellationToken);
            }

            await _canvasGroup.DOFade(0, 0.2f);
        }
    }
}
