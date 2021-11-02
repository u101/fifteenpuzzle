using System;

namespace FifteenPuzzle.model
{
    public static class TilesGridModelFactory
    {

        public static TilesGridModel CreateEmpty()
        {
            return new TilesGridModel(Array.Empty<int>(), 0);
        }

        /// <summary>
        /// Creates sorted tiles grid model 1..(gridWidth * gridWidth)-1
        /// empty tile value is 0
        /// </summary>
        public static TilesGridModel Create(int gridWidth)
        {
            var gridWidthSquared = gridWidth * gridWidth;
            var tilesValues = new int[gridWidthSquared];

            for (var i = 0; i < gridWidthSquared - 1; i++)
            {
                tilesValues[i] = i + 1;
            }

            return new TilesGridModel(tilesValues, gridWidth);
        }
        
    }
}