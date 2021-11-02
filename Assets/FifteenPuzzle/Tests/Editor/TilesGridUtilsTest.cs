using System.Linq;
using FifteenPuzzle.model;
using FifteenPuzzle.utils;
using NUnit.Framework;

namespace FifteenPuzzle.Tests.Editor
{
    
    public class TilesGridUtilsTest
    {

        [Test]
        public void TilesGridCreateTest()
        {
            const int gridWidth = 5;
            
            var tilesGridModel = TilesGridModelFactory.Create(5);
            
            Assert.AreEqual(gridWidth, tilesGridModel.GridWidth);
            Assert.AreEqual(gridWidth*gridWidth, tilesGridModel.Values.Length);

            for (var i = 0; i < tilesGridModel.Values.Length - 1; i++)
            {
                Assert.AreEqual(i + 1, tilesGridModel.Values[i]);
            }
            
            Assert.AreEqual(0, tilesGridModel.Values[tilesGridModel.Values.Length - 1]);
        }

        [Test][Repeat(100)]
        public void ShuffleGridTest()
        {
            const int gridWidth = 5;
            const int shuffleSteps = gridWidth * gridWidth * gridWidth;
            
            var tilesGridModel = TilesGridModelFactory.Create(gridWidth);

            var shuffleResult = TilesGridUtils.ShuffleTiles(
                tilesGridModel, shuffleSteps);
            
            Assert.False(
                TilesGridUtils.AreEqual(tilesGridModel, shuffleResult.TilesGridModel));
            
            Assert.AreEqual(shuffleSteps, shuffleResult.ShuffleOrder.Count);
            
            // try to get original tiles grid model from shuffled one using ShuffleOrder

            var shuffleOrder = shuffleResult.ShuffleOrder.ToList();
            shuffleOrder.Reverse();

            var testGridModel = shuffleResult.TilesGridModel.Copy();
            
            foreach (var i in shuffleOrder)
            {
                testGridModel.SwapTiles(0,i);
            }
            
            Assert.True(
                TilesGridUtils.AreEqual(tilesGridModel, testGridModel));

            // try to get shuffled tiles grid model from original one using ShuffleOrder
            
            testGridModel = tilesGridModel.Copy();
            shuffleOrder = shuffleResult.ShuffleOrder.ToList();
            
            foreach (var i in shuffleOrder)
            {
                testGridModel.SwapTiles(0,i);
            }
            
            
            Assert.True(
                TilesGridUtils.AreEqual(shuffleResult.TilesGridModel, testGridModel));
        }
        
    }
}