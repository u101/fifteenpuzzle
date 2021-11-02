using FifteenPuzzle.model;

namespace FifteenPuzzle.gameStates
{
    public interface IGameStatesSwitch
    {
        void SetState(GameStateId gameStateId);
    }
}