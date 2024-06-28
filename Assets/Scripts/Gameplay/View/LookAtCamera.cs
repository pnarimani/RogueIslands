﻿using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }
        
        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
        }
    }
}