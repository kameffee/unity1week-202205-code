namespace Unity1week202205.Domain.Meel
{
    /// <summary>
    /// 選択された順番、番号
    /// </summary>
    public readonly struct MeelSelectedNumber
    {
        public int Value => _value;

        private readonly int _value;

        public MeelSelectedNumber(int value)
        {
            _value = value;
        }

        public static MeelSelectedNumber None => new(-1);
    }
}
