using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity1week202205.Domain.Fever;
using Unity1week202205.Presentation;
using Unity1week202205.Presentation.Meel;
using UnityEngine;

namespace Unity1week202205.Domain.Meel
{
    /// <summary>
    /// Meelの操作
    /// </summary>
    public class MeelHandler : IMeelRemover, IMeelGenerator
    {
        private BeakerModel _beakerModel;
        private readonly IMeelInstantiater _instantiater;
        private readonly FeverService _feverService;

        public MeelHandler(
            BeakerModel beakerModel,
            IMeelInstantiater instantiater,
            FeverService feverService)
        {
            _beakerModel = beakerModel;
            _instantiater = instantiater;
            _feverService = feverService;
        }

        /// <summary>
        /// ミールの削除
        /// </summary>
        /// <param name="meelModel"></param>
        /// <param name="delay"></param>
        public async UniTask Remove(MeelModel meelModel, float delay = 0)
        {
            // ミールの削除
            await meelModel.Break(delay);
            meelModel.Dispose();
            _beakerModel.Remove(meelModel);
        }

        public async UniTask Remove(MeelModel[] meelModels)
        {
            List<UniTask> tasks = new();

            // フィーバー中は消えるまでが早い
            var interval = _feverService.IsFevering ? 0.05f : 0.2f;

            foreach (var tuple in meelModels.Select((model, i) => (Model: model, Index: i)))
            {
                tasks.Add(Remove(tuple.Model, interval * tuple.Index));
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask RemoveAll()
        {
            List<UniTask> tasks = new List<UniTask>();
            foreach (var tuple in _beakerModel.All().ToArray().Select((model, i) => (model: model, index: i)))
            {
                tasks.Add(Remove(tuple.model, tuple.index * 0.02f));
            }

            await UniTask.WhenAll(tasks);

            Debug.Assert(_beakerModel.All().Count() == 0, $"Count: {_beakerModel.All().Count()}");
        }

        /// <summary>
        /// ミールの生成
        /// </summary>
        /// <param name="meelModel"></param>
        public void Create(MeelModel meelModel)
        {
            _beakerModel.Add(meelModel);
            _instantiater.Create(meelModel);
        }

        public void Create(MeelModel meelModel, Vector3 position)
        {
            _beakerModel.Add(meelModel);
            _instantiater.Create(meelModel, position);
        }

        public int NextCreateId()
        {
            var last = _beakerModel.All().LastOrDefault();
            if (last == null)
            {
                return 0;
            }

            return last.Id + 1;
        }
    }
}
