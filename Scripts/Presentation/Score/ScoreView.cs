using DG.Tweening;
using TMPro;
using Unity1week202205.Domain;
using Unity1week202205.Domain.Score;
using UnityEngine;

namespace Unity1week202205.Presentation.Score
{
    /// <summary>
    /// スコア表示
    /// </summary>
    public class ScoreView : MonoBehaviour, IScoreView
    {
        [SerializeField]
        private TextMeshProUGUI _score;

        private Tween _tween;

        private long currentScore;

        public void Render(ChangedScore changedScore)
        {
            _tween.Kill();

            // カウント表示
            _tween = DOTween.To(
                () => currentScore,
                value =>
                {
                    currentScore = value;
                    _score.SetText(value.ToString("N0"));
                },
                changedScore.After,
                0.5f);
        }
    }
}
