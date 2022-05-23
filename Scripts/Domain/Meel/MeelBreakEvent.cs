using UnityEngine;

namespace Unity1week202205.Domain.Meel
{
    /// <summary>
    /// ミールが消えるときのイベント
    /// </summary>
    public class MeelBreakEvent
    {
        public Vector3 Position { get; }
        public int TypeId { get; }
        public MeelSize Size { get; }

        public MeelBreakEvent(int typeId, Vector3 position, MeelSize size)
        {
            TypeId = typeId;
            Position = position;
            Size = size;
        }
    }
}
