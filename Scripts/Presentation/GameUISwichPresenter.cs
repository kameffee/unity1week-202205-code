using System;
using DG.Tweening;
using UniRx;
using Unity1week202205.Domain;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// UIの表示切り替え
    /// </summary>
    public class GameUISwichPresenter : IStartable, IDisposable
    {
        private readonly GameStateModel _gameState;
        private readonly CanvasGroup _readyCanvas;
        private readonly CompositeDisposable _disposable = new();

        public GameUISwichPresenter(GameStateModel gameState, CanvasGroup readyCanvas)
        {
            _gameState = gameState;
            _readyCanvas = readyCanvas;
        }

        public void Start()
        {
            _gameState.State
                .Subscribe(state =>
                {
                    _readyCanvas.DOFade(state == GameState.Ready ? 1 : 0, 0.2f);
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
