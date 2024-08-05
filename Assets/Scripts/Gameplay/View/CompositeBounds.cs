using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class CompositeBounds
    {
        public Bounds[] Bounds;
        public Vector3 Center;

        public CompositeBounds(Vector3 center, Bounds[] bounds)
        {
            Bounds = bounds;
            Center = center;
        }

        public void MoveCenter(Vector3 targetPosition)
        {
            var offset = targetPosition - Center;
            Center = targetPosition;
            for (var i = 0; i < Bounds.Length; i++)
            {
                Bounds[i].center += offset;
            }
        }
    }
}