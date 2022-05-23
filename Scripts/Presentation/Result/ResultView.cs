using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using Unity1week202205.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// ゲームの結果表示
    /// </summary>
    public class ResultView : MonoBehaviour, IResultView
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private RectTransform _root;

        [SerializeField]
        private TextMeshProUGUI _scoreText;

        [SerializeField]
        private TextMeshProUGUI _comboText;

        [SerializeField]
        private Button _retryButton;

        [SerializeField]
        private Button _rankingButton;

        [SerializeField]
        private Button _closeButton;

        public IObservable<Unit> OnClickRetry => _retryButton.OnClickAsObservable();

        public IObservable<Unit> OnClickRanking => _rankingButton.OnClickAsObservable();

        public IObservable<Unit> OnClose => _closeButton.OnClickAsObservable();

        private ResultData _resultData;
        private long currentScore = 0;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _retryButton.interactable = false;
            _rankingButton.interactable = false;
        }

        public void SetData(ResultData resultData)
        {
            _resultData = resultData;
        }

        public async UniTask Open(CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            _scoreText.SetText("0");

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1, 0.3f));
            sequence.Join(_root.DOAnchorPosY(-50f, 0.3f).From(true));
            sequence.WithCancellation(cancellationToken);
            await sequence.Play();

            var tasks = new List<UniTask>();
            tasks.Add(_comboText.DOCounter(0, _resultData.MaxComboCount, 1f)
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: cancellationToken)
            );

            tasks.Add(DOTween.To(
                    () => currentScore,
                    value =>
                    {
                        currentScore = value;
                        _scoreText.SetText(value.ToString("N0"));
                    },
                    _resultData.Score,
                    1f)
                .SetEase(Ease.Linear)
                .WithCancellation(cancellationToken)
            );

            await UniTask.WhenAll(tasks);

            _retryButton.interactable = true;
            _rankingButton.interactable = true;
        }

        public async UniTask Close(CancellationToken cancellationToken)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            await _canvasGroup.DOFade(0, 0)
                .WithCancellation(cancellationToken);
        }
    }
}
