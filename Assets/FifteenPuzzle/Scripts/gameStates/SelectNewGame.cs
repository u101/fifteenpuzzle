using FifteenPuzzle.model;
using FifteenPuzzle.ui;
using UnityEngine;

namespace FifteenPuzzle.gameStates
{
    public class SelectNewGame : GameState
    {
        private readonly GameStateModel _gameStateModel;
        private readonly IGameStatesSwitch _gameStatesSwitch;
        private readonly Transform _uiRoot;
        private readonly GameObject _popupPrefab;
        public override GameStateId GameStateId => GameStateId.SelectGameMode;

        private StartNewGamePopupComponent _popupComponent;

        public SelectNewGame(
            GameStateModel gameStateModel, IGameStatesSwitch gameStatesSwitch,
            Transform uiRoot, GameObject popupPrefab)
        {
            _gameStateModel = gameStateModel;
            _gameStatesSwitch = gameStatesSwitch;
            _uiRoot = uiRoot;
            _popupPrefab = popupPrefab;
        }
        
        public override void Activate()
        {
            var gameObject = UnityEngine.Object.Instantiate(_popupPrefab, _uiRoot);

            _popupComponent = gameObject.GetComponent<StartNewGamePopupComponent>();
            
            _popupComponent.Button3x3.onClick.AddListener(OnClickButton3x3);
            _popupComponent.Button4x4.onClick.AddListener(OnClickButton4x4);
            _popupComponent.Button5x5.onClick.AddListener(OnClickButton5x5);
        }

        private void OnClickButton3x3()
        {
            StartNewGame(3);
        }

        private void OnClickButton4x4()
        {
            StartNewGame(4);
        }

        private void OnClickButton5x5()
        {
            StartNewGame(5);
        }

        private void StartNewGame(int gridSize)
        {
            _gameStateModel.TilesGridModel = TilesGridModelFactory.Create(gridSize);
            _gameStatesSwitch.SetState(GameStateId.Shuffle);
        }
        

        public override void Deactivate()
        {
            _popupComponent.Button3x3.onClick.RemoveListener(OnClickButton3x3);
            _popupComponent.Button4x4.onClick.RemoveListener(OnClickButton4x4);
            _popupComponent.Button5x5.onClick.RemoveListener(OnClickButton5x5);
            
            UnityEngine.Object.Destroy(_popupComponent.gameObject);

            _popupComponent = null;
        }
    }
}