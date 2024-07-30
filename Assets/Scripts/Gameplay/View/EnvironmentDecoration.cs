using System.Collections.Generic;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class EnvironmentDecoration : MonoBehaviour
    {
        public static readonly List<EnvironmentDecoration> All = new List<EnvironmentDecoration>();

        private void Awake()
        {
            All.Add(this);
        }

        private void OnDestroy()
        {
            All.Remove(this);
        }
    }
}