using System;
using System.Linq;
using FifteenPuzzle.model;
using UnityEngine;

namespace FifteenPuzzle.control
{
    public class GameSaverUnityPrefs : IGameSaver
    {
        private const string GameStateKey = "GameState";
        private const string GridWidthKey = "GridWidth";
        private const string TilesKey = "Tiles";

        public bool HasSavedGame => PlayerPrefs.HasKey(GameStateKey);

        public GameStateModel LoadSavedGame()
        {
            if (!HasSavedGame)
                throw new Exception("saved game state not found");

            var gameState = 
                (GameStateId)Enum.Parse(typeof(GameStateId), PlayerPrefs.GetString(GameStateKey));
            
            var gridWidth = PlayerPrefs.GetInt(GridWidthKey, 0);

            var tilesString = PlayerPrefs.GetString(TilesKey);

            var tiles = !string.IsNullOrEmpty(tilesString) ? 
                tilesString
                    .Split(',').Select(int.Parse).ToArray() : Array.Empty<int>();

            var tilesGridModel = new TilesGridModel(tiles, gridWidth);

            return new GameStateModel()
            {
                GameStateId = gameState,
                TilesGridModel = tilesGridModel
            };
        }

        public void SaveGame(GameStateModel gameStateModel)
        {
            var gameStateId = gameStateModel.GameStateId;
            var tilesGridModel = gameStateModel.TilesGridModel;

            PlayerPrefs.SetString(GameStateKey, gameStateId.ToString());
            PlayerPrefs.SetInt(GridWidthKey, tilesGridModel.GridWidth);
            PlayerPrefs.SetString(TilesKey, string.Join(",", tilesGridModel.Values));
        }
    }
}