using UnityEngine;

namespace Unity1week202205.Presentation.Meel
{
    /// <summary>
    /// ミールが弾けるときのエフェクト
    /// </summary>
    public interface IMeelSplashEffect
    {
        void Play(Vector3 position);
    }
}
