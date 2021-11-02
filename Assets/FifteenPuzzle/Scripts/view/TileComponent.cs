using FifteenPuzzle.events;
using UnityEngine;

namespace FifteenPuzzle.view
{
    public class TileComponent : MonoBehaviour
    {
        [SerializeField] private TextMesh Text;

        private TileClickEvent _tileClickEvent;
        private int _tileValue;

        public void Setup(TileClickEvent tileClickEvent)
        {
            _tileClickEvent = tileClickEvent;
        }

        public int GetTileValue()
        {
            return _tileValue;
        }
        
        public void SetTileValue(int value)
        {
            _tileValue = value;
            Text.text = value.ToString();
        }

        private void OnMouseDown()
        {
            _tileClickEvent?.Invoke(_tileValue);
        }
    }
}