using System.Collections;
using System.Collections.Generic;
using FifteenPuzzle.control;
using FifteenPuzzle.model;
using FifteenPuzzle.utils;
using FifteenPuzzle.view;
using UnityEngine;

namespace FifteenPuzzle.gameStates
{
    public class ShuffleTilesState : GameState
    {
        private readonly GameStateModel _gameStateModel;
        private readonly TilesViewManager _tilesViewManager;
        private readonly IGameSaver _gameSaver;
        private readonly MonoBehaviour _coroutineContainer;
        private readonly IGameStatesSwitch _gameStatesSwitch;
        public override GameStateId GameStateId => GameStateId.Shuffle;

        private Coroutine _coroutine;

        public ShuffleTilesState(
            GameStateModel gameStateModel,
            TilesViewManager tilesViewManager,
            IGameSaver gameSaver,
            MonoBehaviour coroutineContainer,
            IGameStatesSwitch gameStatesSwitch)
        {
            _gameStateModel = gameStateModel;
            _tilesViewManager = tilesViewManager;
            _gameSaver = gameSaver;
            _coroutineContainer = coroutineContainer;
            _gameStatesSwitch = gameStatesSwitch;
        }
        
        public override void Activate()
        {
            var originalGridModel = _gameStateModel.TilesGridModel;
            var gridWidth = originalGridModel.GridWidth;

            var gridShuffleResult = TilesGridUtils.ShuffleTiles(
                originalGridModel, gridWidth * gridWidth * gridWidth);

            _gameStateModel.GameStateId = GameStateId.Play;
            _gameStateModel.TilesGridModel = gridShuffleResult.TilesGridModel;
            
            _gameSaver.SaveGame(_gameStateModel);
            
            _tilesViewManager.SetupGameField(
                originalGridModel.Values, originalGridModel.GridWidth);

            _coroutine = _coroutineContainer.StartCoroutine(
                DoShuffle(originalGridModel.Copy(), gridShuffleResult.ShuffleOrder));
        }

        private IEnumerator DoShuffle(
            TilesGridModel tilesGrid, List<int> shuffleOrder)
        {
            foreach (var tileValue in shuffleOrder)
            {
                var endPosition = tilesGrid.GetTileValuePosition(0);

                yield return _tilesViewManager.MoveTile(tileValue, endPosition, 50f);
                
                tilesGrid.SwapTiles(0, tileValue);
            }
            
            _gameStatesSwitch.SetState(GameStateId.Play);
        }

        public override void Deactivate()
        {
            _coroutineContainer.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}