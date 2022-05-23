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
    /// ゲーム開始前演出
    /// </summary>
    public class GameReadyPerformer : MonoBehaviour, IGameReadyPerformer
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        [SerializeField]
        private Button _startButton;

        public IObservable<Unit> OnClickStart => _startButton.OnClickAsObservable();

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public async UniTask Show(CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            await _canvasGroup.DOFade(1, 0.5f).WithCancellation(cancellationToken);
        }
        
        public async UniTask Hide(CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            await _canvasGroup.DOFade(0, 0.1f).WithCancellation(cancellationToken);
        }
    }
}
