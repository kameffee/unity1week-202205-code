using System;
using DG.Tweening;
using MPUIKIT;
using TMPro;
using UnityEngine;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// 残り時間表示
    /// </summary>
    public class RemainigTimeView : MonoBehaviour, IRemainingTime
    {
        [SerializeField]
        private TextMeshProUGUI _remainingTime;

        [SerializeField]
        private MPImage _gauge;

        private RemainingTime _last;

        public void Render(RemainingTime remainingTime)
        {
            // ゲージは常に更新
            _gauge.fillAmount = remainingTime.Normalize;

            // 小数点の変化は無視
            var last = (int)_last.Value.TotalSeconds;
            var current = (int)remainingTime.Value.TotalSeconds;
            if (last != current)
            {
                // 秒が変わったときに更新
                _remainingTime.SetText("{0:0}", (int)remainingTime.Value.TotalSeconds);

                // アニメーション
                var text = _remainingTime.transform;
                Sequence sequence = DOTween.Sequence();
                sequence.Join(text.DOScaleY(1.2f, 0.2f));
                sequence.Join(text.DOScaleX(0.9f, 0.2f));
                sequence.Append(text.DOScale(1, 0.3f).SetEase(Ease.OutBack));
                sequence.SetLink(gameObject);
                sequence.Play();

                _last = remainingTime;
            }
        }
    }
}
