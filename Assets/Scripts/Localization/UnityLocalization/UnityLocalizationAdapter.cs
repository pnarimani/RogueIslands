using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace RogueIslands.Localization.UnityLocalization
{
    public class UnityLocalizationAdapter : ILocalization, IDisposable
    {
        private IList<StringTable> _stringTables;
        
        public event Action<string> LocaleChanged;

        public UnityLocalizationAdapter()
        {
            LocalizationSettings.Instance.OnSelectedLocaleChanged += OnSelectedLocaleChanged;
        }

        public IReadOnlyList<string> GetAvailableLocales()
        {
            return LocalizationSettings.Instance.GetAvailableLocales()
                .Locales
                .Select(locale => locale.LocaleName)
                .ToArray();
        }

        public void SelectLocale(string locale)
        {
            var localeObject = LocalizationSettings.Instance.GetAvailableLocales().Locales
                .First(l => l.LocaleName == locale);
            LocalizationSettings.Instance.SetSelectedLocale(localeObject);
        }

        public string Get(string key)
        {
            if ( GetEntry(key) is { } entry)
                return entry.GetLocalizedString();

            return key;
        }

        public string Get(string key, params object[] args)
        {
            if ( GetEntry(key) is { } entry)
                return entry.GetLocalizedString(args);
            return key;
        }

        public void Dispose()
        {
            LocalizationSettings.Instance.OnSelectedLocaleChanged -= OnSelectedLocaleChanged;
        }

        private void OnSelectedLocaleChanged(Locale obj)
            => LocaleChanged?.Invoke(obj.LocaleName);

        private  StringTableEntry GetEntry(string key)
        {
            if (_stringTables == null)
            {
                 LocalizationSettings.InitializationOperation.WaitForCompletion();
                _stringTables = LocalizationSettings.Instance.GetStringDatabase().GetAllTables().WaitForCompletion();
            }

            foreach (var table in _stringTables)
            {
                if (table.GetEntry(key) is { } entry)
                {
                    return entry;
                }
            }

            return null;
        }
    }
}