using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// ゲームの開始前演出
    /// </summary>
    public interface IGameReadyPerformer
    {
        IObservable<Unit> OnClickStart { get; }
        UniTask Show(CancellationToken cancellationToken);
        UniTask Hide(CancellationToken cancellationToken);
    }
}
