using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace RogueIslands.Localization
{
    public interface ILocalization
    {
        event Action<string> LocaleChanged;
        
        IReadOnlyList<string> GetAvailableLocales();
        void SelectLocale(string locale);
        
        string Get(string key);
        string Get(string key, params object[] args);
    }
}