using System;
using UnityEngine;

namespace FifteenPuzzle.model
{
    public class TilesGridModel
    {
        public int[] Values { get; }
        public int GridWidth { get; }

        public TilesGridModel(int[] values, int gridWidth)
        {
            if (values.Length != gridWidth * gridWidth)
            {
                throw new Exception(
                    $"values.Length ({values.Length}) should be gridWidth ({gridWidth}) squared");
            }
            
            Values = values;
            GridWidth = gridWidth;
        }

        /// <summary>
        /// returns true if tiles in array are sorted according to expected order
        /// </summary>
        public bool IsInSolvedState
        {
            get
            {
                for (var i = 0; i < Values.Length - 1; i++)
                {
                    if (Values[i] != i + 1) return false;
                }

                return true;
            }
        }

        public Vector2Int GetTileValuePosition(int tileValue)
        {
            var index = Array.IndexOf(Values, tileValue);
            
            if (index == -1)
            {
                throw new Exception($"tile value not found {index}");
            }

            return new Vector2Int(index % GridWidth, index / GridWidth);
        }

        public int GetValueAt(Vector2Int position)
        {
            return GetValueAt(position.x, position.y);
        }

        public int GetValueAt(int x, int y)
        {
            var index = x + y * GridWidth;

            if (index < 0 || index >= Values.Length)
            {
                throw new IndexOutOfRangeException(
                    $"index {index} (x:{x},y:{y}) out of range [0..{Values.Length}]");
            }

            return Values[index];
        }

        public void SetValueAt(int x, int y, int value)
        {
            var index = x + y * GridWidth;

            if (index < 0 || index >= Values.Length)
            {
                throw new IndexOutOfRangeException(
                    $"index {index} (x:{x}:{y}) out of range [0..{Values.Length}]");
            }

            Values[index] = value;
        }

        public void SwapTiles(int x0, int y0, int x1, int y1)
        {
            var index0 = x0 + y0 * GridWidth;
            var index1 = x1 + y1 * GridWidth;

            if (index0 < 0 || index0 >= Values.Length)
            {
                throw new IndexOutOfRangeException(
                    $"index0 {index0} (x:{x0},y:{y0}) out of range [0..{Values.Length}]");
            }
            if (index1 < 0 || index1 >= Values.Length)
            {
                throw new IndexOutOfRangeException(
                    $"index1 {index1} (x:{x1},y:{y1}) out of range [0..{Values.Length}]");
            }

            (Values[index0], Values[index1]) = (Values[index1], Values[index0]);
        }

        public void SwapTiles(int tileValue0, int tileValue1)
        {
            var index0 = Array.IndexOf(Values, tileValue0);
            var index1 = Array.IndexOf(Values, tileValue1);

            if (index0 == -1)
            {
                throw new Exception($"tile value not found {index0}");
            }

            if (index1 == -1)
            {
                throw new Exception($"tile value not found {index1}");
            }
            
            (Values[index0], Values[index1]) = (Values[index1], Values[index0]);
        }

        public TilesGridModel Copy()
        {
            var valuesLength = Values.Length;
            
            if (valuesLength == 0)
            {
                return new TilesGridModel(Array.Empty<int>(), 0);
            }
            
            var newValues = new int[valuesLength];
            
            Array.Copy(Values, newValues, valuesLength);
            
            return new TilesGridModel(newValues, GridWidth);
        }

        public override string ToString()
        {
            return $"TilesGridModel[{nameof(Values)}: {string.Join(",", Values)}, {nameof(GridWidth)}: {GridWidth}]";
        }
    }
}