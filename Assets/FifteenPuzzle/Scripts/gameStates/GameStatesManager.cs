using System;
using System.Collections.Generic;
using FifteenPuzzle.model;

namespace FifteenPuzzle.gameStates
{
    public class GameStatesManager : IGameStatesSwitch, IDisposable
    {

        private readonly Dictionary<GameStateId, GameState> _controls =
            new Dictionary<GameStateId, GameState>();
        
        private GameState _currentState;

        public GameStatesManager() {}

        public void AddState(GameStateId gameStateId, GameState gameState)
        {
            _controls[gameStateId] = gameState;
        }

        public void SetState(GameStateId gameStateId)
        {
            if (_currentState != null && _currentState.GameStateId == gameStateId)
            {
                return;
            }
            
            _currentState?.Deactivate();
            _currentState = _controls[gameStateId];
            _currentState.Activate();
        }

        public void Dispose()
        {
            _currentState?.Deactivate();
            _currentState = null;

        }
    }
}