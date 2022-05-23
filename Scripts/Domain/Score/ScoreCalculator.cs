using Debug = UnityEngine.Debug;

namespace Unity1week202205.Domain.Score
{
    /// <summary>
    /// スコア計算
    /// </summary>
    public class ScoreCalculator
    {
        public long Calculate(int meelCount, int bigMeelCount, int comboCount, bool isFever)
        {
            long chainScore = CalculateChainBonus(meelCount);
            long baseScore = (100 * meelCount) + chainScore + (200 * bigMeelCount);

            var addScore = baseScore + baseScore * ((10 + comboCount) / 100f);
            addScore *= isFever ? 1 : 1.1f;
            Debug.Log($"chainScore: {meelCount}, comboCount: {comboCount}\n{baseScore} + ({baseScore} x (10 + {comboCount}) / 100f ) = {addScore}");
            return (long)addScore;
        }

        private long CalculateChainBonus(long chainCount)
        {
            switch (chainCount)
            {
                case 3:
                    return 300;
                case <= 7:
                    return 200 * chainCount;
                case <= 10:
                    return 1500;
                case <= 14:
                    return 2000;
                case <= 19:
                    return 2500;
                case < 30:
                    return 3000;
                case >= 30:
                    return 3500;
            }
        }
    }
}
