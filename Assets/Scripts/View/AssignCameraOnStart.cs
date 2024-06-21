using System;
using UnityEngine;

namespace RogueIslands.View
{
    public class AssignCameraOnStart : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}