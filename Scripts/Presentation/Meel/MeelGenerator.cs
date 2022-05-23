using System;
using UniRx;
using Unity1week202205.Domain.Meel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity1week202205.Presentation.Meel
{
    /// <summary>
    /// ミール生成
    /// </summary>
    public class MeelGenerator : IMeelInstantiater, IDisposable
    {
        private readonly Func<MeelModel, MeelView, MeelPresenter> _meelFactory;
        private readonly Transform _generatePoint;
        private readonly Transform _holder;
        private readonly MeelView _meelPrefab;
        private readonly MeelView _bigMeelPrefab;

        private readonly MeelPooler _normalMeelPooler;
        private readonly MeelPooler _bigMeelPooler;

        private readonly CompositeDisposable _disposable = new();

        public MeelGenerator(
            Func<MeelModel, MeelView, MeelPresenter> meelFactory,
            MeelGenerateData meelGenerateData)
        {
            _meelFactory = meelFactory;
            _generatePoint = meelGenerateData.GeneratePoint;
            _holder = meelGenerateData.Holder;
            _meelPrefab = meelGenerateData.MeelPrefab;
            _bigMeelPrefab = meelGenerateData.BigMeelPrefab;

            // 通常ミール
            _normalMeelPooler = new MeelPooler(_meelPrefab, _holder);
            _normalMeelPooler.PreloadAsync(60, 2)
                .Subscribe()
                .AddTo(_disposable);

            // ビッグミール
            _bigMeelPooler = new MeelPooler(_bigMeelPrefab, _holder);
            _bigMeelPooler.PreloadAsync(5, 1)
                .Subscribe()
                .AddTo(_disposable);
        }

        public void Create(MeelModel meelModel)
        {
            var position = _generatePoint.position;
            position.x += Random.Range(-1.0f, 1.0f);
            var meelView = meelModel.Size.IsBig() ? _bigMeelPooler.Rent() : _normalMeelPooler.Rent();
            meelView.transform.position = position;

            var presenter = _meelFactory.Invoke(meelModel, meelView);
            presenter.Initialzie();
        }

        public void Create(MeelModel meelModel, Vector3 position)
        {
            position.x += Random.Range(-1.0f, 1.0f);
            var meelView = meelModel.Size.IsBig() ? _bigMeelPooler.Rent() : _normalMeelPooler.Rent();
            meelView.transform.position = position;

            var presenter = _meelFactory.Invoke(meelModel, meelView);
            presenter.Initialzie();
        }

        public void Dispose()
        {
            _normalMeelPooler?.Dispose();
            _bigMeelPooler?.Dispose();
            _disposable?.Dispose();
        }
    }
}
