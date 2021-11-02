using FifteenPuzzle.model;

namespace FifteenPuzzle.gameStates
{
    public abstract class GameState
    {
        public abstract GameStateId GameStateId { get; }
        
        public abstract void Activate();
        public abstract void Deactivate();

    }
}