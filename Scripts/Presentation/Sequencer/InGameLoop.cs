using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.AudioPlayer;
using MessagePipe;
using UniRx;
using Unity1week202205.Data;
using Unity1week202205.Domain;
using Unity1week202205.Domain.Meel;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// インゲームループ
    /// </summary>
    public class InGameLoop : IInitializable, IStartable, IAsyncStartable, IDisposable
    {
        private readonly MeelSettings _settings;
        private readonly IMeelSpawner _meelSpawner;
        private readonly GameTimer _gameTimer;
        private readonly IGameReadyPerformer _gameReadyPerformer;
        private readonly InGameStartSequencer _gameStartSequencer;
        private readonly InGameEndSequencer _gameEndSequencer;
        private readonly InGameService _inGameService;
        private readonly ISubscriber<InGameRetryEvent> _inGameRetryEvent;
        private readonly IInputHandler _inputHandler;
        private readonly SpinService _spinService;
        private readonly IDoctorView _doctorView;
        private readonly GameStateModel _gameStateModel;

        private CancellationTokenSource _cancellationTokenSource = new();
        private CompositeDisposable _disposable = new();

        public InGameLoop(
            MeelSettings settings,
            IMeelSpawner meelSpawner,
            GameTimer gameTimer,
            InGameService inGameService,
            IGameReadyPerformer gameReadyPerformer,
            InGameStartSequencer gameStartSequencer,
            InGameEndSequencer gameEndSequencer,
            ISubscriber<InGameRetryEvent> inGameRetryEvent,
            IInputHandler inputHandler,
            SpinService spinService,
            IDoctorView doctorView,
            GameStateModel gameStateModel)
        {
            _settings = settings;
            _meelSpawner = meelSpawner;
            _gameTimer = gameTimer;
            _inGameService = inGameService;
            _gameReadyPerformer = gameReadyPerformer;
            _gameStartSequencer = gameStartSequencer;
            _gameEndSequencer = gameEndSequencer;
            _inGameRetryEvent = inGameRetryEvent;
            _inputHandler = inputHandler;
            _spinService = spinService;
            _doctorView = doctorView;
            _gameStateModel = gameStateModel;
        }

        public void Initialize()
        {
            _gameStateModel.State
                .SkipLatestValueOnSubscribe()
                .Where(state => state == GameState.Ready)
                .Subscribe(_ => UniTask.Void(async () =>
                {
                    _inGameService.DeselectAll();
                    await StartWaitTap(_cancellationTokenSource.Token);
                }))
                .AddTo(_disposable);
        }

        public void Start()
        {
            _inGameRetryEvent
                .Subscribe(_ => UniTask.Void(() => Retry(_cancellationTokenSource.Token)))
                .AddTo(_disposable);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            _gameStateModel.SetState(GameState.Ready);

            _doctorView.Play(DoctorAnimationType.Ready);
            AudioPlayer.Instance.Bgm.Play(0);
            _spinService.SetActive(true);

            // 最初の生成
            for (int i = 0; i < 50; i++)
            {
                _meelSpawner.RandomSpawn();
                await UniTask.Delay(TimeSpan.FromSeconds(0.02f), cancellationToken: cancellation);
            }

            await StartWaitTap(cancellation);
        }

        /// <summary>
        /// はじめるが押されるまで待つ
        /// </summary>
        /// <param name="cancellation"></param>
        public async UniTask StartWaitTap(CancellationToken cancellation)
        {
            await _gameReadyPerformer.Show(cancellation);
            await _gameReadyPerformer.OnClickStart.ToUniTask(true, cancellation);
            await _gameReadyPerformer.Hide(cancellation);

            await GameStart(cancellation);
        }

        /// <summary>
        /// ゲーム開始
        /// </summary>
        /// <param name="cancellation"></param>
        public async UniTask GameStart(CancellationToken cancellation)
        {
            // ゲームの状態更新
            _gameStateModel.SetState(GameState.Playing);
            // 博士アニメ
            _doctorView.Play(DoctorAnimationType.Ready);

            // ゲームの準備
            await Prepare(cancellation);

            // 開始演出
            await _gameStartSequencer.Start(cancellation);

            // 入力可能に変更
            _inputHandler.SetActive(true);
            _spinService.SetActive(true);

            // 博士ちゃんのアニメ
            _doctorView.Play(DoctorAnimationType.Normal);

            // ゲームスタート
            _gameTimer.Start();

            // 終了待つ
            await _gameTimer.OnTimeUp.ToUniTask(true, cancellationToken: cancellation);

            await OnGameEnd(cancellation);
        }

        private async UniTask OnGameEnd(CancellationToken cancellation)
        {
            // ゲーム状態更新
            _gameStateModel.SetState(GameState.Result);

            // 入力をオフ
            _inputHandler.SetActive(false);
            _spinService.SetActive(false);

            // 選択状態を全解除
            _inGameService.DeselectAll();

            _doctorView.Play(DoctorAnimationType.Result);

            // 終了演出
            await _gameEndSequencer.Start(cancellation);
        }

        /// <summary>
        /// ゲームの準備
        /// </summary>
        private async UniTask Prepare(CancellationToken cancellation)
        {
            // 場にあるミールを削除
            await _inGameService.MeelDeleteAll();

            // リセット
            _inGameService.Reset();

            // 最初の生成
            for (int i = 0; i < 50; i++)
            {
                _meelSpawner.RandomSpawn();
                await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: cancellation);
            }
        }

        /// <summary>
        /// リトライ
        /// </summary>
        /// <param name="cancellationToken"></param>
        private async UniTaskVoid Retry(CancellationToken cancellationToken)
        {
            await GameStart(cancellationToken);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
            _disposable?.Dispose();
        }
    }
}
