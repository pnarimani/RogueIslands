using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class PinToWorldObject : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private Camera _camera;

        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Target != null)
                transform.position = _camera.WorldToScreenPoint(Target.position);
        }
    }
}