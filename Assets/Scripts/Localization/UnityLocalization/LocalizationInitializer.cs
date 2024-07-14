using Cysharp.Threading.Tasks;
using RogueIslands.Initialization;
using UnityEngine.Localization.Settings;

namespace RogueIslands.Localization.UnityLocalization
{
    public class LocalizationInitializer : IInitializationStep
    {
        public UniTask Initialize()
        {
            return LocalizationSettings.InitializationOperation.ToUniTask();
        }
    }
}