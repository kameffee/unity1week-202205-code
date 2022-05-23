namespace Unity1week202205.Domain
{
    /// <summary>
    /// ゲームの結果データ
    /// </summary>
    public class ResultData
    {
        public long Score => _score;

        public int MaxComboCount => _maxComboCount; 
        
        private readonly long _score;
        private readonly int _maxComboCount;

        public ResultData(long score, int maxComboCount)
        {
            _score = score;
            _maxComboCount = maxComboCount;
        }
    }
}
