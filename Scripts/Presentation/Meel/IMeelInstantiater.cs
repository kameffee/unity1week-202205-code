using Unity1week202205.Domain.Meel;
using UnityEngine;

namespace Unity1week202205.Presentation.Meel
{
    /// <summary>
    /// ミールオブジェクトの生成
    /// </summary>
    public interface IMeelInstantiater
    {
        void Create(MeelModel meelModel);

        void Create(MeelModel meelModel, Vector3 position);
    }
}
