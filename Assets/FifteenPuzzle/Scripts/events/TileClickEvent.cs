using System;
using UnityEngine.Events;

namespace FifteenPuzzle.events
{
    [Serializable]
    public class TileClickEvent : UnityEvent<int> { }
}