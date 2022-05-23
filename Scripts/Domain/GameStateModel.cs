using UniRx;
using Unity1week202205.Presentation;

namespace Unity1week202205.Domain
{
    /// <summary>
    /// ゲーム状態
    /// </summary>
    public class GameStateModel
    {
        public IReadOnlyReactiveProperty<GameState> State => _state;
        private readonly ReactiveProperty<GameState> _state = new(GameState.Ready);

        public void SetState(GameState state)
        {
            _state.Value = state;
        }
    }
}
