using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using naichilab;
using UniRx;
using Unity1week202205.Domain;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// ゲームの結果プレゼンター
    /// </summary>
    public class ResultPresenter : IStartable, IDisposable
    {
        private readonly GameStateModel _gameStateModel;
        private readonly IResultView _resultView;
        private readonly ISubscriber<ResultData> _resultSubscriber;
        private readonly IPublisher<InGameRetryEvent> _retryPublisher;

        private CancellationTokenSource _tokenSource;
        private readonly CompositeDisposable _disposable = new();

        private ResultData _resultData;

        public ResultPresenter(
            GameStateModel gameStateModel,
            IResultView resultView,
            ISubscriber<ResultData> resultSubscriber,
            IPublisher<InGameRetryEvent> retryPublisher)
        {
            _gameStateModel = gameStateModel;
            _resultView = resultView;
            _resultSubscriber = resultSubscriber;
            _retryPublisher = retryPublisher;
            _tokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _resultSubscriber
                .Subscribe(resultData => UniTask.Void(async () =>
                {
                    _resultData = resultData;
                    _resultView.SetData(resultData);
                    await _resultView.Open(_tokenSource.Token);
                }))
                .AddTo(_disposable);

            // リトライ
            _resultView.OnClickRetry
                .Subscribe(_ => UniTask.Void(async () =>
                {
                    await _resultView.Close(_tokenSource.Token);
                    _retryPublisher.Publish(new InGameRetryEvent());
                }))
                .AddTo(_disposable);

            // ランキング表示
            _resultView.OnClickRanking
                .Subscribe(_ => { RankingLoader.Instance.SendScoreAndShowRanking(_resultData.Score); })
                .AddTo(_disposable);

            // 閉じたとき
            _resultView.OnClose
                .Subscribe(async _ =>
                {
                    await _resultView.Close(_tokenSource.Token);
                    _gameStateModel.SetState(GameState.Ready);
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _tokenSource?.Dispose();
            _disposable?.Dispose();
        }
    }
}
