using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// 時間追加表示
    /// </summary>
    public class AddTimeView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _root;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TextMeshProUGUI _addTimeText;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
        }

        public void Play(TimeSpan time)
        {
            _addTimeText.SetText($"+{time.TotalSeconds:N0}");

            _root.localScale = Vector3.one * 0.5f;

            // アニメーション
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1, 0.1f));
            sequence.Join(_root.DOScale(1f, 0.3f)
                .SetEase(Ease.OutBack));

            sequence.AppendInterval(2f);

            sequence.Append(_canvasGroup.DOFade(0, 1f));
            sequence.SetLink(gameObject);
            sequence.Play();
        }
    }
}
