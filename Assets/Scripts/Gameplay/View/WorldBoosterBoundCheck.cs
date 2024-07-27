using System.Collections.Generic;
using RogueIslands.Gameplay.View.Boosters;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class WorldBoosterBoundCheck
    {
        private static readonly Collider[] _colliderBuffer = new Collider[1000];
        private static readonly List<WorldBoosterView> _overlappingBoosters = new();
        
        public static void HighlightOverlappingWorldBoosters(Transform building)
        {
        }
        
        public static void HideAllDeletionWarnings()
        {
         
        }
    }
}