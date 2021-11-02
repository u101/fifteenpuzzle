using System;
using System.Collections.Generic;
using FifteenPuzzle.model;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

namespace FifteenPuzzle.utils
{
    public static class TilesGridUtils
    {

        public static bool AreEqual(
            [NotNull] TilesGridModel tilesGridModelA, 
            [NotNull] TilesGridModel tilesGridModelB)
        {
            if (tilesGridModelA.GridWidth != tilesGridModelB.GridWidth) return false;
            
            if (tilesGridModelA.GridWidth == 0) return true;

            var valuesA = tilesGridModelA.Values;
            var valuesB = tilesGridModelB.Values;

            for (var i = 0; i < valuesA.Length; i++)
            {
                if (valuesA[i] != valuesB[i]) return false;
            }

            return true;
        }
        
        /// <summary>
        /// Shuffles TilesGridModel values,
        /// returns TilesGridShuffleResult which contains a shuffled copy of input tiles gird model and
        /// ShuffleOrder list which contains tile values in order to swap with empty tile to get shuffled grid model 
        /// </summary>
        /// <param name="tilesGridModel"></param>
        /// <param name="shuffleSteps"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        
        public static TilesGridShuffleResult ShuffleTiles(TilesGridModel tilesGridModel, int shuffleSteps)
        {
            // create a cope of tiles grid model
            var newTilesGrid = tilesGridModel.Copy();

            // hash set to store visited states hashes to prevent loops
            var visitedStatesSet = new HashSet<string>();
            
            // store tile values (not indices) to be swapped with empty tile
            var shuffleOrder = new List<int>();
            
            // find empty tile index
            var emptyTileIndex = Array.IndexOf(tilesGridModel.Values, 0);
            // convert empty tile index to grid coordinates
            var emptyTileX = emptyTileIndex % tilesGridModel.GridWidth;
            var emptyTileY = emptyTileIndex / tilesGridModel.GridWidth;

            if (Shuffle(newTilesGrid, shuffleSteps, 
                emptyTileX, emptyTileY, visitedStatesSet, shuffleOrder, new Random()))
            {
                return new TilesGridShuffleResult(newTilesGrid, shuffleOrder);
            }

            throw new Exception($"Failed to shuffle tiles grid of size {tilesGridModel.GridWidth}");

        }
        
        public readonly struct TilesGridShuffleResult
        {
            public TilesGridModel TilesGridModel { get; }
            
            /// <summary>
            /// list of tile values that were swapped with empty tile sequentially
            /// </summary>
            public List<int> ShuffleOrder { get; }

            public TilesGridShuffleResult(TilesGridModel tilesGridModel, List<int> shuffleOrder)
            {
                TilesGridModel = tilesGridModel;
                ShuffleOrder = shuffleOrder;
            }
        }

        private static bool Shuffle(
            TilesGridModel tilesGridModel, 
            int shuffleSteps,
            int emptyTileX,
            int emptyTileY,
            HashSet<string> visitedStatesSet,
            List<int> shuffleOrder,
            Random random)
        {
            var stateHash = GetStateHash(tilesGridModel);
            
            if (visitedStatesSet.Contains(stateHash)) return false;
            
            // if we exceeded max shuffle steps and we are in legal grid state - success
            if (shuffleSteps == 0) return true;
            
            // store current grid state hash
            visitedStatesSet.Add(stateHash);
            
            var gridWidth = tilesGridModel.GridWidth;

            // get randomly ordered array of next tile-lookup direction
            var tilesLookupDirections = GetRandomTilesLookupDirections(random);

            foreach (var lookupDir in tilesLookupDirections)
            {
                var lookupX = emptyTileX + lookupDir.x;
                var lookupY = emptyTileY + lookupDir.y;
                
                // check if adjacent tile is within grid
                
                if (lookupX < 0 || lookupX >= gridWidth ||
                    lookupY < 0 || lookupY >= gridWidth) continue;
                
                // store adjacent tile value in shuffle order list
                shuffleOrder.Add(tilesGridModel.GetValueAt(lookupX, lookupY));
                
                // swap adjacent tile with empty tile
                tilesGridModel.SwapTiles(lookupX, lookupY, emptyTileX, emptyTileY);
                
                // try to shuffle next iteration
                if (Shuffle(tilesGridModel, shuffleSteps - 1, 
                    lookupX, lookupY, visitedStatesSet, shuffleOrder, random))
                {
                    // success
                    return true;
                }
                else
                {
                    // revert tiles grid model state (swap back)
                    tilesGridModel.SwapTiles(lookupX, lookupY, emptyTileX, emptyTileY);
                    // revert shuffle order list state
                    shuffleOrder.RemoveAt(shuffleOrder.Count - 1);
                }
            }

            return false;
        }

        private static Vector2Int[] GetRandomTilesLookupDirections(Random random)
        {
            var lookup = new Vector2Int[]
            {
                new Vector2Int(-1,0),
                new Vector2Int(1,0),
                new Vector2Int(0,-1),
                new Vector2Int(0,1)
                
            };
            
            var n = lookup.Length;
            while (n > 1) 
            {
                var k = random.Next(n--);
                (lookup[n], lookup[k]) = (lookup[k], lookup[n]);
            }

            return lookup;
        }
        
        private static string GetStateHash(TilesGridModel tilesGridModel)
        {
            return string.Join(",", tilesGridModel.Values);
        }
        
        
        
        

    }
}