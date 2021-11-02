using FifteenPuzzle.control;
using FifteenPuzzle.model;
using FifteenPuzzle.ui;
using FifteenPuzzle.view;
using UnityEngine;

namespace FifteenPuzzle.gameStates
{
    public class WinGameState : GameState
    {
        private readonly GameStateModel _gameStateModel;
        private readonly IGameStatesSwitch _gameStatesSwitch;
        private readonly IGameSaver _gameSaver;
        private readonly TilesViewManager _tilesViewManager;
        private readonly Transform _uiRoot;
        private readonly GameObject _popupPrefab;
        public override GameStateId GameStateId => GameStateId.Win;
        
        private WinGamePopupComponent _popupComponent;

        public WinGameState(
            GameStateModel gameStateModel, IGameStatesSwitch gameStatesSwitch, 
            IGameSaver gameSaver, TilesViewManager tilesViewManager,
            Transform uiRoot, GameObject popupPrefab)
        {
            _gameStateModel = gameStateModel;
            _gameStatesSwitch = gameStatesSwitch;
            _gameSaver = gameSaver;
            _tilesViewManager = tilesViewManager;
            _uiRoot = uiRoot;
            _popupPrefab = popupPrefab;
        }
        
        public override void Activate()
        {
            var gameObject = UnityEngine.Object.Instantiate(_popupPrefab, _uiRoot);

            _popupComponent = gameObject.GetComponent<WinGamePopupComponent>();
            
            _popupComponent.OkButton.onClick.AddListener(OnOkButtonClick);
        }

        private void OnOkButtonClick()
        {
            _gameStateModel.GameStateId = GameStateId.SelectGameMode;
            _gameStateModel.TilesGridModel = TilesGridModelFactory.CreateEmpty();
            _gameSaver.SaveGame(_gameStateModel);
            
            _tilesViewManager.Clear();
            
            _gameStatesSwitch.SetState(GameStateId.SelectGameMode);
        }

        public override void Deactivate()
        {
            _popupComponent.OkButton.onClick.RemoveListener(OnOkButtonClick);
            
            UnityEngine.Object.Destroy(_popupComponent.gameObject);

            _popupComponent = null;
        }
    }
}