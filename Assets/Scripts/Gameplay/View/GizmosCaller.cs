using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class GizmosCaller : MonoBehaviour, IDisposable
    {
        private IReadOnlyList<IGizmosDrawer> _drawers;

        public void Initialize(IReadOnlyList<IGizmosDrawer> drawers)
        {
            _drawers = drawers;
        }

        private void OnDrawGizmos()
        {
            if (_drawers == null)
                return;

            foreach (var drawer in _drawers)
            {
                drawer.OnDrawGizmos();
            }
        }

        public void Dispose()
        {
            if (this != null)
                Destroy(gameObject);
        }
    }
}