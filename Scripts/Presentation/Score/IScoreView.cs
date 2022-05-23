using Unity1week202205.Domain;
using Unity1week202205.Domain.Score;

namespace Unity1week202205.Presentation.Score
{
    /// <summary>
    /// スコア表示
    /// </summary>
    public interface IScoreView
    {
        void Render(ChangedScore changedScore);
    }
}
