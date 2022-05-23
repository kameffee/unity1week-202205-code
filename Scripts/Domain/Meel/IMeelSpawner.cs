using UnityEngine;

namespace Unity1week202205.Domain.Meel
{
    /// <summary>
    /// ミール生成
    /// </summary>
    public interface IMeelSpawner
    {
        void RandomSpawn(int count = 1);

        void Spawn(Vector3 position, int typeId, MeelSize size);
    }
}
