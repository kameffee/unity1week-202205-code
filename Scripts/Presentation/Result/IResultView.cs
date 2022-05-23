using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1week202205.Domain;

namespace Unity1week202205.Presentation
{
    /// <summary>
    /// 結果表示
    /// </summary>
    public interface IResultView
    {
        IObservable<Unit> OnClickRetry { get; }
        
        IObservable<Unit> OnClickRanking { get; }
        
        IObservable<Unit> OnClose { get; }

        void SetData(ResultData resultData);
        
        UniTask Open(CancellationToken cancellationToken);

        UniTask Close(CancellationToken cancellationToken);
    }
}
