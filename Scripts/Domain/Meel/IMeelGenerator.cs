using UnityEngine;

namespace Unity1week202205.Domain.Meel
{
    /// <summary>
    /// ミール生成
    /// </summary>
    public interface IMeelGenerator
    {
        void Create(MeelModel meelModel);

        void Create(MeelModel meelModel, Vector3 position);

        int NextCreateId();
    }
}
