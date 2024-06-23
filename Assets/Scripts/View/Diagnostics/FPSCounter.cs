using System;
using TMPro;
using UnityEngine;

namespace RogueIslands.View.Diagnostics
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fpsText;

        private float _timer;

        private void Update()
        {
            _timer += Time.unscaledDeltaTime;
            if (_timer >= 1)
            {
                _fpsText.text = $"FPS: {1 / Time.unscaledDeltaTime:0}";
                _timer = 0;
            }
        }
    }
}