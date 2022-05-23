using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202205.Presentation.Tutorial
{
    /// <summary>
    /// チュートリアルのページ
    /// </summary>
    public class TutorialPageView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Button _button;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public async UniTask Open(CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            await _canvasGroup.DOFade(1, 0.2f);

            await _button.OnClickAsObservable().ToUniTask(true, cancellationToken);
        }

        public async UniTask Close(CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            await _canvasGroup.DOFade(0, 0.2f);
        }
    }
}
