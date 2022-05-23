using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.UI;
using VContainer.Unity;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// 設定
    /// </summary>
    public class OptionPresenter : IStartable, IDisposable
    {
        private readonly OptionView _view;
        private readonly Button _optionButton;
        private readonly CompositeDisposable _disposable = new();

        public OptionPresenter(OptionView view, Button optionButton)
        {
            _view = view;
            _optionButton = optionButton;
        }

        public void Start()
        {
            _optionButton.OnClickAsObservable()
                .Subscribe(async _ => await _view.Open())
                .AddTo(_disposable);
            
            _view.OnClickClose
                .Subscribe(async _ => await _view.Close())
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
