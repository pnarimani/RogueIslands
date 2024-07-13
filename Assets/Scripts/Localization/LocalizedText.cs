using System;
using RogueIslands.DependencyInjection;
using TMPro;
using UnityEngine;

namespace RogueIslands.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;
        
        private ILocalization _localization;
        private TextMeshProUGUI _textMeshProUGUI;

        private void Awake()
        {
            _localization = StaticResolver.Resolve<ILocalization>();
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            
            _localization.LocaleChanged += OnLocaleChanged;
            UpdateText();
        }

        private void OnDestroy()
        {
            if (_localization != null) 
                _localization.LocaleChanged -= OnLocaleChanged;
        }

        private void OnLocaleChanged(string obj) 
            => UpdateText();

        private void UpdateText() 
            => _textMeshProUGUI.text = _localization.Get(_key);
    }
}