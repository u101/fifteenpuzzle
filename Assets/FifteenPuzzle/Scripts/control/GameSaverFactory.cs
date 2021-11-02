namespace FifteenPuzzle.control
{
    public static class GameSaverFactory
    {

        public static IGameSaver CreateGameSaver()
        {
            return new GameSaverUnityPrefs();
        }
        
    }
}