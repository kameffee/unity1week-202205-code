using UnityEngine;

namespace Unity1week202205.Presentation.Meel
{
    /// <summary>
    /// ミールが消えるときのエフェクト
    /// </summary>
    public class MeelSplashEffect : MonoBehaviour, IMeelSplashEffect
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        public void Play(Vector3 position)
        {
            ParticleSystem.EmitParams param = new();
            param.position = position;
            param.applyShapeToPosition = true;
            _particleSystem.Emit(param, 10);
        }

        private void OnParticleSystemStopped()
        {
        }
    }
}
