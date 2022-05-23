using UnityEngine;

namespace Unity1week202205.Presentation.MeelChain
{
    /// <summary>
    /// ミールを繋ぐ線
    /// </summary>
    public interface IMeelChainView
    {
        void Render(Vector3[] points);
    }
}
