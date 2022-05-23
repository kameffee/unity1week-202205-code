using System.Collections.Generic;
using UnityEngine;

namespace Unity1week202205.Data
{
    /// <summary>
    /// ミール設定
    /// </summary>
    [CreateAssetMenu(fileName = "MeelSettings", menuName = "InGame/MeelSettings", order = 0)]
    public class MeelSettings : ScriptableObject
    {
        [SerializeField]
        private List<MeelProfile> _profiles;

        public IEnumerable<MeelProfile> All() => _profiles;
    }
}
