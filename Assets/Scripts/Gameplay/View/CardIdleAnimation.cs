using System.Collections.Generic;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class CardIdleAnimation : MonoBehaviour
    {
        private static readonly List<CardIdleAnimation> _instances = new();
        public static IReadOnlyList<CardIdleAnimation> Instances => _instances;

        private void OnEnable()
        {
            _instances.Add(this);
        }
        
        private void OnDisable()
        {
            _instances.Remove(this);
        }
    }
}