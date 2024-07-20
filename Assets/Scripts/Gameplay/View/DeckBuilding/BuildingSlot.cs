using System;
using UnityEngine;

namespace RogueIslands.Gameplay.View.DeckBuilding
{
    [Serializable]
    public class BuildingSlot
    {
        public bool ShouldFlipOnSubmit;
        public bool DestroyOnSubmit;
        public RectTransform Transform;
    }
}