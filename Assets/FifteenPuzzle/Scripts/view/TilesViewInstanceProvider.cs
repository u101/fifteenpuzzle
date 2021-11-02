using System;
using System.Collections.Generic;
using UnityEngine;

namespace FifteenPuzzle.view
{
    public class TilesViewInstanceProvider
    {
        private readonly GameObject _tilePrefab;
        private readonly List<TileComponent> _tileComponentsPool = new List<TileComponent>();

        public TilesViewInstanceProvider(GameObject tilePrefab)
        {
            _tilePrefab = tilePrefab;
        }
        
        public TileComponent GetOrCreateTileView()
        {
            var tileComponent = (TileComponent)null;
            
            if (_tileComponentsPool.Count == 0)
            {
                var gameObject = UnityEngine.Object.Instantiate(_tilePrefab);
                
                tileComponent = gameObject.GetComponent<TileComponent>();
                
                if (tileComponent == null)
                {
                    throw new Exception("tile prefab does not contain TileComponent");
                }

                return tileComponent;
            }

            tileComponent = _tileComponentsPool[_tileComponentsPool.Count - 1];
            
            _tileComponentsPool.RemoveAt(_tileComponentsPool.Count - 1);

            tileComponent.gameObject.SetActive(true);
            
            return tileComponent;
        }

        public void ReleaseTileView(TileComponent tileComponent)
        {
            tileComponent.gameObject.SetActive(false);
            
            _tileComponentsPool.Add(tileComponent);
        }
        
    }
}