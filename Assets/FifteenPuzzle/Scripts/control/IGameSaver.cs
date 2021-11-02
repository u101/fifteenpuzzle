using FifteenPuzzle.model;

namespace FifteenPuzzle.control
{
    public interface IGameSaver
    {
        bool HasSavedGame { get; }

        GameStateModel LoadSavedGame();

        void SaveGame(GameStateModel gameStateModel);

    }
}