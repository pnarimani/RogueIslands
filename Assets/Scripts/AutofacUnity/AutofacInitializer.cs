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
            if (AutofacSettings.Instance.RootScope != null) 
                AutofacSettings.Instance.RootScope.Build();
        }
    }
}