using System;
using FifteenPuzzle.control;
using FifteenPuzzle.events;
using FifteenPuzzle.gameStates;
using FifteenPuzzle.model;
using FifteenPuzzle.view;
using UnityEngine;

namespace FifteenPuzzle
{
    public class GameLauncher : MonoBehaviour
    {
        [SerializeField] public Camera Camera;
        [SerializeField] public GameObject TilePrefab;
        [SerializeField] public Transform UiRoot;
        [SerializeField] public GameObject StartNewGamePopupPrefab;
        [SerializeField] public GameObject WinGamePopupPrefab;
        [SerializeField] public float TileSize = 1.3f;

        private void Start()
        {
            var gameSaver = GameSaverFactory.CreateGameSaver();

            var tileClickEvent = new TileClickEvent();

            var gameStateModel = gameSaver.HasSavedGame ?
                gameSaver.LoadSavedGame() : new GameStateModel()
                {
                    GameStateId = GameStateId.SelectGameMode,
                    TilesGridModel = TilesGridModelFactory.CreateEmpty()
                };

            var tilesViewInstanceProvider = new TilesViewInstanceProvider(TilePrefab);

            var tilesViewManager = new TilesViewManager(
                tilesViewInstanceProvider, tileClickEvent, 
                TileSize, Camera);

            var tilesGridModel = gameStateModel.TilesGridModel;

            if (tilesGridModel.GridWidth != 0)
            {
                tilesViewManager.SetupGameField(tilesGridModel.Values, tilesGridModel.GridWidth);
            }

            if (gameStateModel.GameStateId == GameStateId.Play && 
                gameStateModel.TilesGridModel.IsInSolvedState)
            {
                gameStateModel.GameStateId = GameStateId.Win;
            }

            var gameStatesManager = new GameStatesManager();
            
            gameStatesManager.AddState(GameStateId.SelectGameMode, 
                new SelectNewGame(gameStateModel, gameStatesManager, UiRoot, StartNewGamePopupPrefab));
            
            gameStatesManager.AddState(GameStateId.Shuffle, 
                new ShuffleTilesState(
                    gameStateModel, tilesViewManager, gameSaver, this, gameStatesManager));
            
            gameStatesManager.AddState(GameStateId.Play,
                new PlayGameState(
                    gameStateModel, gameStatesManager, tilesViewManager, tileClickEvent, gameSaver, this));
            
            gameStatesManager.AddState(GameStateId.Win, 
                new WinGameState(gameStateModel, gameStatesManager, gameSaver, 
                    tilesViewManager, UiRoot, WinGamePopupPrefab));

            gameStatesManager.SetState(gameStateModel.GameStateId);
        }
    }
}