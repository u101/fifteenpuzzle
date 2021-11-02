using FifteenPuzzle.control;
using FifteenPuzzle.events;
using FifteenPuzzle.model;
using FifteenPuzzle.view;
using UnityEngine;

namespace FifteenPuzzle.gameStates
{
    public class PlayGameState : GameState
    {
        private readonly GameStateModel _gameStateModel;
        private readonly IGameStatesSwitch _gameStatesSwitch;
        private readonly TilesViewManager _tilesViewManager;
        private readonly TileClickEvent _tileClickEvent;
        private readonly IGameSaver _gameSaver;
        private readonly MonoBehaviour _coroutineContainer;

        public override GameStateId GameStateId => GameStateId.Play;

        public PlayGameState(
            GameStateModel gameStateModel, IGameStatesSwitch gameStatesSwitch,
            TilesViewManager tilesViewManager, TileClickEvent tileClickEvent,
            IGameSaver gameSaver,
            MonoBehaviour coroutineContainer)
        {
            _gameStateModel = gameStateModel;
            _gameStatesSwitch = gameStatesSwitch;
            _tilesViewManager = tilesViewManager;
            _tileClickEvent = tileClickEvent;
            _gameSaver = gameSaver;
            _coroutineContainer = coroutineContainer;
        }
        
        public override void Activate()
        {
            _tileClickEvent.AddListener(OnTileClick);
        }

        private void OnTileClick(int tileValue)
        {
            var tilesGridModel = _gameStateModel.TilesGridModel;
            
            var valuePosition = tilesGridModel.GetTileValuePosition(tileValue);
            var emptyTilePosition = tilesGridModel.GetTileValuePosition(0);

            var dx = Mathf.Abs(emptyTilePosition.x - valuePosition.x);
            var dy = Mathf.Abs(emptyTilePosition.y - valuePosition.y);
            
            // check if it is not empty tile click and it is not diagonal tile click
            if ((dx == 0) == (dy == 0) ||
                dx > 1 || dy > 1) return;
            
            tilesGridModel.SwapTiles(0, tileValue);

            _coroutineContainer.StartCoroutine(
                _tilesViewManager.MoveTile(tileValue, emptyTilePosition, 30f));

            if (tilesGridModel.IsInSolvedState)
            {
                _gameStateModel.GameStateId = GameStateId.Win;
                _gameStatesSwitch.SetState(GameStateId.Win);
            }
            
            _gameSaver.SaveGame(_gameStateModel);
        }

        public override void Deactivate()
        {
            _tileClickEvent.RemoveListener(OnTileClick);
        }
    }
}