using System;
using UniRx;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// シャッフル
    /// </summary>
    public interface ISpinView
    {
        IObservable<Unit> OnClickSpin { get; }

        void SetActive(bool isActive);

        void Spin();
    }
}
