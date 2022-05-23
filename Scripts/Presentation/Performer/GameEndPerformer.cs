using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Unity1week202205.Presentation.Performer
{
    /// <summary>
    /// ゲームの終了時演出
    /// </summary>
    public class GameEndPerformer : MonoBehaviour, IGameEndPerformer
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
        }

        public async UniTask Play(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(1, 0.5f)
                .WithCancellation(cancellationToken);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);

            await _canvasGroup.DOFade(0, 0.2f)
                .WithCancellation(cancellationToken);
        }
    }
}
