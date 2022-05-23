using System.Linq;
using Unity1week202205.Domain;
using Unity1week202205.Domain.Meel;
using VContainer.Unity;

namespace Unity1week202205.Presentation.MeelChain
{
    /// <summary>
    /// 選択したミールを繋ぐやつ
    /// </summary>
    public class MeelChainPresenter : ITickable
    {
        private readonly MeelSelecter _meelSelecter;
        private readonly IMeelChainView _chainView;

        public MeelChainPresenter(MeelSelecter meelSelecter, IMeelChainView chainView)
        {
            _meelSelecter = meelSelecter;
            _chainView = chainView;
        }

        public void Tick()
        {
            _chainView.Render(_meelSelecter.All().Select(model => model.Position).ToArray());
        }
    }
}
