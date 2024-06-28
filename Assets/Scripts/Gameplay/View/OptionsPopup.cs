﻿using RogueIslands.Gameplay.View.DeckBuilding;
using RogueIslands.UISystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RogueIslands.Gameplay.View
{
    public class OptionsPopup : MonoBehaviour, IWindow
    {
        [SerializeField] private Button _resume, _leave, _settings;
        
        private void Start()
        {
            _settings.onClick.AddListener(() => GameManager.Instance.ShowSettingsPopup());
            _resume.onClick.AddListener(() => Destroy(gameObject));
            _leave.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        }
    }
}