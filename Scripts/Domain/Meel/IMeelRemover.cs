using Cysharp.Threading.Tasks;

namespace Unity1week202205.Domain.Meel
{
    /// <summary>
    /// ミール削除者
    /// </summary>
    public interface IMeelRemover
    {
        UniTask Remove(MeelModel meelModel, float delay = 0);

        UniTask Remove(MeelModel[] meelModels);

        UniTask RemoveAll();
    }
}
