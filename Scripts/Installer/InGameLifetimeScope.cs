using System;
using MessagePipe;
using Unity1week202205.Data;
using Unity1week202205.Domain;
using Unity1week202205.Domain.Combo;
using Unity1week202205.Domain.Fever;
using Unity1week202205.Domain.Meel;
using Unity1week202205.Domain.Score;
using Unity1week202205.Presentation;
using Unity1week202205.Presentation.Combo;
using Unity1week202205.Presentation.Fever;
using Unity1week202205.Presentation.Meel;
using Unity1week202205.Presentation.MeelChain;
using Unity1week202205.Presentation.Performer;
using Unity1week202205.Presentation.Score;
using Unity1week202205.Presentation.Tutorial;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Unity1week202205.Installer
{
    public class InGameLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private MeelSettings _meelSettings;

        [SerializeField]
        private MeelGenerateData _meelGenerateData;

        [SerializeField]
        private ResultView _resultViewPrefab;

        [SerializeField]
        private Transform _performCanvas;

        [SerializeField]
        private OptionView _optionViewPrefab;

        [SerializeField]
        private Button _optionButton;

        [SerializeField]
        private CanvasGroup _readyCanvas;

        [Header("Tutorial")]
        [SerializeField]
        private Button _tutorialButton;

        [SerializeField]
        private TutorialPerformer _tutorialPerformerPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            var option = builder.RegisterMessagePipe();

            builder.Register<GameStateModel>(Lifetime.Scoped);
            builder.RegisterEntryPoint<GameUISwichPresenter>()
                .WithParameter("readyCanvas", _readyCanvas);

            // 博士
            builder.RegisterComponentInHierarchy<DoctorView>().AsImplementedInterfaces();

            // 入力
            builder.Register<PlayerInput>(Lifetime.Scoped).As<IInputHandler>();

            // ミール
            builder.RegisterInstance(_meelGenerateData);
            builder.Register<MeelGenerator>(Lifetime.Scoped).As<IMeelInstantiater>();
            builder.RegisterInstance<MeelSettings>(_meelSettings);
            builder.Register<MeelSpawner>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<BeakerModel>(Lifetime.Scoped);
            builder.RegisterInstance<Camera>(Camera.main);
            builder.Register<MeelHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MeelSelecter>(Lifetime.Scoped);
            builder.Register<InGameService>(Lifetime.Scoped);

            builder.RegisterFactory<MeelModel, MeelView, MeelPresenter>(
                resolver => (model, view) =>
                {
                    var publisher = resolver.Resolve<IPublisher<MeelBreakEvent>>();
                    return new MeelPresenter(model, view, publisher);
                }, Lifetime.Scoped);

            builder.RegisterMessageBroker<MeelBreakEvent>(option);

            // コンボ
            builder.Register<ComboModel>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<ComboView>().As<IComboView>();
            builder.RegisterEntryPoint<ComboPresenter>();
            builder.Register<ComboResetTimer>(Lifetime.Scoped).WithParameter(3f);

            // スコア
            builder.Register<ScoreCalculator>(Lifetime.Scoped);
            builder.RegisterMessageBroker<ChangedScore>(option);
            builder.Register<ScoreModel>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<ScoreView>().As<IScoreView>();
            builder.RegisterEntryPoint<ScorePresenter>();

            // フィーバー
            builder.Register<FeverGaugeModel>(Lifetime.Scoped).WithParameter("maxValue", 1000f);
            builder.Register<FeverTimer>(Lifetime.Scoped);
            builder.Register<FeverService>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.RegisterComponentInHierarchy<FeverGaugeView>().As<IFeverView>();
            builder.RegisterEntryPoint<FeverGaugePresenter>();

            // エフェクト
            builder.RegisterComponentInHierarchy<MeelSplashEffect>().As<IMeelSplashEffect>();
            builder.RegisterEntryPoint<MeelBreakEffectPresenter>();

            // タイマー
            builder.Register<GameTimer>(Lifetime.Scoped).WithParameter(60f);
            builder.RegisterComponentInHierarchy<RemainigTimeView>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<GameStatusPresenter>(Lifetime.Scoped);

            // タイム追加
            builder.RegisterComponentInHierarchy<AddTimePerformer>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<AddTimePresenter>();

            // 準備
            builder.RegisterComponentInHierarchy<GameReadyPerformer>().AsImplementedInterfaces();

            // 開始演出
            builder.RegisterComponentInHierarchy<GameStartPerformer>().AsImplementedInterfaces();
            builder.Register<InGameStartSequencer>(Lifetime.Scoped);

            // 終了演出
            builder.RegisterComponentInHierarchy<GameEndPerformer>().AsImplementedInterfaces();
            builder.Register<InGameEndSequencer>(Lifetime.Scoped);

            // 結果表示
            builder.RegisterMessageBroker<ResultData>(option);
            builder.RegisterComponentInNewPrefab<ResultView>(_resultViewPrefab, Lifetime.Scoped)
                .UnderTransform(_performCanvas)
                .AsImplementedInterfaces();
            builder.RegisterEntryPoint<ResultPresenter>();

            // スピン
            builder.Register<SpinService>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<SpinButton>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<SpinPresenter>();

            // リトライ
            builder.RegisterMessageBroker<InGameRetryEvent>(option);

            builder.RegisterComponentInHierarchy<MeelChainView>().AsImplementedInterfaces();
            builder.RegisterEntryPoint<MeelChainPresenter>();

            // 設定
            builder.RegisterComponentInNewPrefab<OptionView>(_optionViewPrefab, Lifetime.Scoped)
                .UnderTransform(_performCanvas)
                .AsSelf();
            builder.RegisterEntryPoint<OptionPresenter>().WithParameter("optionButton", _optionButton);

            // チュートリアル
            builder.RegisterComponentInNewPrefab<TutorialPerformer>(_tutorialPerformerPrefab, Lifetime.Scoped)
                .UnderTransform(_performCanvas)
                .AsImplementedInterfaces();
            builder.RegisterEntryPoint<TutorialPresenter>()
                .WithParameter("button", _tutorialButton);

            builder.RegisterEntryPoint<PlayerInput>();
            builder.RegisterEntryPoint<InGameLoop>();
        }
    }
}
