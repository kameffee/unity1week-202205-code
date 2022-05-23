using System.Linq;
using Unity1week202205.Data;
using UnityEngine;

namespace Unity1week202205.Domain.Meel
{
    /// <summary>
    /// ミールの生成
    /// </summary>
    public class MeelSpawner : IMeelSpawner
    {
        private readonly IMeelGenerator _meelGenerator;
        private readonly MeelSettings _meelSettings;

        private MeelProfile[] _profiles;

        public MeelSpawner(
            IMeelGenerator meelGenerator,
            MeelSettings meelSettings)
        {
            _meelGenerator = meelGenerator;
            _meelSettings = meelSettings;
            _profiles = _meelSettings.All().ToArray();
        }

        public void RandomSpawn(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                var addMeel = _profiles[Random.Range(0, _profiles.Length)];
                var model = new MeelModel(_meelGenerator.NextCreateId(), addMeel, MeelSize.Normal);
                _meelGenerator.Create(model);
            }
        }

        public void Spawn(Vector3 position, int typeId, MeelSize size)
        {
            var meelProfile = _profiles.FirstOrDefault(profile => profile.TypeId == typeId);
            var model = new MeelModel(_meelGenerator.NextCreateId(), meelProfile, MeelSize.Big);
            _meelGenerator.Create(model, position);
        }
    }
}
