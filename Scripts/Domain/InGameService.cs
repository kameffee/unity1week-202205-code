using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Kameffee.AudioPlayer;
using UniRx;
using Unity1week202205.Domain.Combo;
using Unity1week202205.Domain.Fever;
using Unity1week202205.Domain.Meel;
using Unity1week202205.Domain.Score;
using Unity1week202205.Presentation;
using Unity1week202205.Presentation.Meel;
using UnityEngine;

namespace Unity1week202205.Domain
{
    /// <summary>
    /// インゲームサービス
    /// </summary>
    public class InGameService : IDisposable
    {
        private readonly MeelSelecter _meelSelecter;
        private readonly IMeelRemover _meelRemover;
        private readonly IMeelSpawner _meelSpawner;
        private readonly BeakerModel _beakerModel;
        private readonly ComboModel _comboModel;
        private readonly ComboResetTimer _comboResetTimer;
        private readonly ScoreModel _scoreModel;
        private readonly ScoreCalculator _scoreCalculator;
        private readonly FeverService _feverService;
        private readonly Camera _camera;
        private readonly GameTimer _gameTimer;
        private readonly IDoctorView _doctorView;

        private readonly RaycastHit2D[] _results = new RaycastHit2D[128];
        private readonly CompositeDisposable _disposable = new();

        // ブレイク中のシーケンス数
        private int _breakingCount;

        public InGameService(Camera camera,
            MeelSelecter meelSelecter,
            IMeelRemover meelRemover,
            IMeelSpawner meelSpawner,
            BeakerModel beakerModel,
            ComboModel comboModel,
            ComboResetTimer comboResetTimer,
            ScoreModel scoreModel,
            ScoreCalculator scoreCalculator,
            FeverService feverService,
            GameTimer gameTimer,
            IDoctorView doctorView)
        {
            _camera = camera;
            _meelSelecter = meelSelecter;
            _meelRemover = meelRemover;
            _meelSpawner = meelSpawner;
            _beakerModel = beakerModel;
            _comboModel = comboModel;
            _comboResetTimer = comboResetTimer;
            _scoreModel = scoreModel;
            _scoreCalculator = scoreCalculator;
            _feverService = feverService;
            _gameTimer = gameTimer;
            _doctorView = doctorView;

            _comboResetTimer.OnReset
                .Subscribe(_ => _comboModel.Reset())
                .AddTo(_disposable);

            // フィーバー開始
            _feverService.OnStartFever
                .Subscribe(_ => OnStartFever())
                .AddTo(_disposable);

            // フィーバー終了
            _feverService.OnFinishFever
                .Subscribe(_ => OnFinishFever())
                .AddTo(_disposable);
        }

        public void Tapping(Vector3 screenPosition)
        {
            ValidateChain();

            var ray = _camera.ScreenPointToRay(screenPosition);
            var raycast = Physics2D.Raycast(ray.origin, ray.direction, 20);
            if (raycast.collider)
            {
                if (raycast.transform.TryGetComponent<MeelView>(out var meel))
                {
                    var hitMeel = _beakerModel.GetMeel(meel.Id);
                    if (!hitMeel.Selected.Value)
                    {
                        if (_meelSelecter.IsEmpty())
                        {
                            // そのまま選択状態へ
                            _meelSelecter.Register(hitMeel);
                        }
                        else if (_meelSelecter.First.TypeId == hitMeel.TypeId)
                        {
                            // 最初に選んだミールと同じタイプなら

                            // Raycastで判定
                            var lastMeel = _meelSelecter.Last;
                            var direction = hitMeel.Position - lastMeel.Position;
                            var size = Physics2D.CircleCastNonAlloc(lastMeel.Position, 0.1f, direction, _results);
                            var isChain = _results
                                .FirstOrDefault(hit2D => hit2D.distance > 0)
                                .transform.GetComponent<MeelView>().Id == hitMeel.Id;

                            if (isChain)
                            {
                                _meelSelecter.Register(hitMeel);
                            }
                        }
                    }
                }
            }
        }

        public void TapUp()
        {
            // 3つ以上選択していないと消せない
            if (_meelSelecter.Count >= 3)
            {
                Break().Forget();
            }
            else
            {
                // 全選択解除
                _meelSelecter.Reset();
            }
        }

        private async UniTaskVoid Break()
        {
            if (_comboResetTimer.Starting)
            {
                _comboResetTimer.Pause();
            }

            // コンボをカウントアップ
            _comboModel.Add();

            if (_breakingCount <= 0)
            {
                // 重力停止
                _beakerModel.Freeze();

                // フィーバー一時停止
                _feverService.Pause();
            }

            // カウントアップ
            _breakingCount++;

            // 消した数
            var chainCount = _meelSelecter.Count;

            var bigMeelCount = _meelSelecter.BigMeelCount();

            // スコア算出
            var addScore = _scoreCalculator.Calculate
            (chainCount,
                bigMeelCount,
                _comboModel.CurrentCombo.Value,
                _feverService.IsFevering);

            _scoreModel.Add(addScore);

            var comboBonusRatio = 1 + 0.15f * (_comboModel.CurrentCombo.Value - 1);
            var chainBonusRatio = 1 + 0.25f * (chainCount - 3);
            var addFever = 20 * comboBonusRatio + 100 * chainBonusRatio;
            _feverService.AddFeverGauge(addFever);

            var selectedMeels = _meelSelecter.All().Reverse().ToArray();
            foreach (var selectedMeel in selectedMeels)
            {
                selectedMeel.SetIsLastSelected(false);
            }

            _meelSelecter.Clear();

            await _meelRemover.Remove(selectedMeels);

            var last = selectedMeels.LastOrDefault();
            var isLongChain = chainCount >= 7;

            _meelSpawner.RandomSpawn(chainCount - (isLongChain ? 1 : 0));

            if (isLongChain)
            {
                _meelSpawner.Spawn(last.Position, last.TypeId, MeelSize.Big);
            }

            // リセット時間更新
            _comboResetTimer.SetResetTime(GetResetTime(_comboModel.CurrentCombo.Value));

            _breakingCount--;
            if (_breakingCount <= 0)
            {
                // 重力停止解除
                _beakerModel.UnFreeze();
                // フィーバータイムのタイマーを再開
                _feverService.UnPause();

                // フィーバーではない
                if (!_feverService.IsFevering)
                {
                    // コンボタイマー開始
                    _comboResetTimer.Start();
                }
            }
        }

        /// <summary>
        /// フィーバー開始
        /// </summary>
        private void OnStartFever()
        {
            // 時間を追加
            _gameTimer.AddTime(TimeSpan.FromSeconds(5f));

            _doctorView.Play(DoctorAnimationType.Fever);

            // フィーバーBGMに切り替える
            AudioPlayer.Instance.Bgm.CrossFade(1, 0.5f);

            // コンボタイマー停止
            _comboResetTimer.Pause();
        }

        /// <summary>
        /// フィーバー終了時
        /// </summary>
        private void OnFinishFever()
        {
            _doctorView.Play(DoctorAnimationType.Normal);
            // 元に戻す
            AudioPlayer.Instance.Bgm.CrossFade(0, 1f);

            // ブレイク中でなければ
            if (_breakingCount <= 0)
            {
                _comboResetTimer.SetResetTime(GetResetTime(_comboModel.CurrentCombo.Value));
                _comboResetTimer.Start();
            }
        }

        private float GetResetTime(int comboCount)
        {
            return comboCount switch
            {
                <= 50 => 3f,
                <= 100 => 2f,
                <= 500 => 1f,
                > 500 => 0.5f,
            };
        }

        /// <summary>
        /// 選択中のミール達が繋がっているかチェック
        /// 動いたりして繋がらなくなった場合は解除する
        /// </summary>
        private void ValidateChain()
        {
            if (_meelSelecter.Count > 1)
            {
                var meelArray = _meelSelecter.All().ToArray();
                for (var i = 1; i < meelArray.Length; i++)
                {
                    var prev = meelArray[i - 1];
                    var current = meelArray[i];
                    var castAll = Physics2D.CircleCastAll(prev.Position, 0.1f, current.Position - prev.Position);
                    var isChain = castAll
                        .FirstOrDefault(hit2D => hit2D.distance > 0)
                        .transform.GetComponent<MeelView>().Id == current.Id;

                    if (isChain)
                    {
                        continue;
                    }

                    _meelSelecter.UnRegister(current);
                    break;
                }
            }
        }

        /// <summary>
        /// 選択状態を全解除
        /// </summary>
        public void DeselectAll()
        {
            _meelSelecter.Reset();
        }

        /// <summary>
        /// ミール全削除
        /// </summary>
        public async UniTask MeelDeleteAll()
        {
            await _meelRemover.RemoveAll();
        }

        public void Dispose()
        {
            _comboModel?.Dispose();
            _disposable?.Dispose();
        }

        public void Reset()
        {
            _gameTimer.Reset();
            _feverService.Reset();
            _comboModel.Reset();
            _scoreModel.Reset();
        }
    }
}
