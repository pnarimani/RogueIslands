using UnityEngine;

namespace AutofacUnity
{
    internal static class AutofacInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            AutofacSettings.LoadInstanceFromResources();
            if(AutofacSettings.Instance == null)
                return;
            AutofacSettings.Instance.InitializeRootScope();
        }
    }
}