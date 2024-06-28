using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class AssignCameraOnStart : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}