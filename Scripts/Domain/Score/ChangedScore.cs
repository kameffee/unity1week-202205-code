namespace Unity1week202205.Domain.Score
{
    /// <summary>
    /// スコア変動
    /// </summary>
    public readonly struct ChangedScore
    {
        public long Before => _before;
        public long After => _before + _addValue;
        public long AddValue => _addValue;

        private readonly long _before;
        private readonly long _addValue;

        public ChangedScore(long before, long addValue)
        {
            _before = before;
            _addValue = addValue;
        }

        public override string ToString()
        {
            return $"{nameof(Before)}: {Before}, {nameof(After)}: {After}, {nameof(AddValue)}: {AddValue}";
        }
    }
}
