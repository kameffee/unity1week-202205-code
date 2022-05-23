using System;
using System.Collections.Generic;
using System.Linq;
using Unity1week202205.Domain.Meel;

namespace Unity1week202205.Domain
{
    /// <summary>
    /// ビーカー. ミールの管理
    /// </summary>
    public sealed class BeakerModel
    {
        private readonly List<MeelModel> _container = new();

        /// <summary>
        /// ビーカー内のミールの数
        /// </summary>
        public int MeelCount => _container.Count;

        public BeakerModel()
        {
        }

        public IEnumerable<MeelModel> All() => _container;

        public void Add(MeelModel model)
        {
            _container.Add(model);
        }

        public void Remove(MeelModel model)
        {
            _container.Remove(model);
        }

        public MeelModel GetMeel(int id)
        {
            return _container.FirstOrDefault(model => model.Id == id);
        }

        public void Freeze()
        {
            foreach (var meelModel in _container)
            {
                meelModel.Freeze();
            }
        }

        public void UnFreeze()
        {
            foreach (var meelModel in _container)
            {
                meelModel.UnFreeze();
            }
        }
    }
}
