using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202205.Presentation.Performer
{
    /// <summary>
    /// ゲーム開始時の演出
    /// </summary>
    public interface IGameStartPerformer
    {
        UniTask Play(CancellationToken cancellationToken);
    }
}
