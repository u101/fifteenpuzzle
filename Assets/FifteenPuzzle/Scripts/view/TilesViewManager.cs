using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FifteenPuzzle.events;
using UnityEngine;

namespace FifteenPuzzle.view
{
    public class TilesViewManager
    {
        private readonly TilesViewInstanceProvider _tilesViewInstanceProvider;
        private readonly TileClickEvent _tileClickEvent;
        private readonly float _tileViewSize;
        private readonly Camera _camera;

        private readonly List<TileComponent> _tileComponents = new List<TileComponent>();

        public TilesViewManager(
            TilesViewInstanceProvider tilesViewInstanceProvider,
            TileClickEvent tileClickEvent,
            float tileViewSize,
            Camera camera)
        {
            _tilesViewInstanceProvider = tilesViewInstanceProvider;
            _tileClickEvent = tileClickEvent;
            _tileViewSize = tileViewSize;
            _camera = camera;
        }

        public void SetupGameField(int[] tiles, int gridWidth)
        {
            Clear();

            for (var i = 0; i < tiles.Length; i++)
            {
                var tileValue = tiles[i];
                
                if (tileValue == 0) continue;

                var tileComp = _tilesViewInstanceProvider.GetOrCreateTileView();
                
                _tileComponents.Add(tileComp);
                
                tileComp.Setup(_tileClickEvent);
                tileComp.SetTileValue(tileValue);

                var x = (i % gridWidth) * _tileViewSize;
                // ReSharper disable once PossibleLossOfFraction
                var y = -(i / gridWidth) * _tileViewSize;

                tileComp.transform.localPosition = new Vector3(x, y);
            }
            
            _camera.transform.position = new Vector3(gridWidth * 0.5f, -gridWidth  * 0.5f, -10f);
            _camera.orthographicSize = (gridWidth * _tileViewSize) + 1f;
            
        }

        public IEnumerator MoveTile(int tileValue, Vector2Int toPosition, float moveSpeed)
        {
            var tileComponent = _tileComponents
                .FirstOrDefault(tileComp => tileComp.GetTileValue() == tileValue);
            
            if (tileComponent == null) yield break;

            var transform = tileComponent.transform;

            var targetPos = new Vector3(toPosition.x * _tileViewSize, -toPosition.y * _tileViewSize);

            const float minDistanceOffset = 0.01f * 0.01f;

            while((targetPos - transform.position).sqrMagnitude > minDistanceOffset)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, targetPos, Time.deltaTime * moveSpeed);
                yield return null;
            }
            transform.position = targetPos;
        }

        public void Clear()
        {
            foreach (var tileComponent in _tileComponents)
            {
                _tilesViewInstanceProvider.ReleaseTileView(tileComponent);
            }
            _tileComponents.Clear();
        }
        
    }
}