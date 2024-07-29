using System;
using RogueIslands.Autofac;
using TMPro;
using UnityEngine;

namespace RogueIslands.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;

        private ILocalization _localization;
        private TextMeshProUGUI _textMeshProUGUI;

        private void Start()
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

        public void UpdateText()
        {
            if (!string.IsNullOrEmpty(_key))
                _textMeshProUGUI.text = _localization.Get(_key);
        }
    }
}