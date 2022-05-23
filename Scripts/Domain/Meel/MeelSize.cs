namespace Unity1week202205.Domain.Meel
{
    /// <summary>
    /// ミールの大きさ
    /// </summary>
    public readonly struct MeelSize
    {
        public int Value => _value;

        private readonly int _value;

        public MeelSize(int value) : this()
        {
            _value = value;
        }

        public static MeelSize Normal => new MeelSize(1);
        public static MeelSize Big => new MeelSize(2);
    }

    public static class MeelSizeEx
    {
        public static bool IsNormal(this MeelSize meelSize) => meelSize.Value == 1;
        public static bool IsBig(this MeelSize meelSize) => meelSize.Value == 2;
    }
}
