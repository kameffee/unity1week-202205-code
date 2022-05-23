using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202205.Presentation.Performer
{
    /// <summary>
    /// ゲーム終了時の演出
    /// </summary>
    public interface IGameEndPerformer
    {
        UniTask Play(CancellationToken cancellationToken);
    }
}
