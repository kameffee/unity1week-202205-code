using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202205.Presentation.Tutorial
{
    /// <summary>
    /// チュートリアル演出
    /// </summary>
    public interface ITutorialPerformer
    {
        UniTask Open(CancellationToken cancellationToken);
    }
}
