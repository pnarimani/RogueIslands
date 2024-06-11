using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AutofacUnity
{
    public class AutofacSettings : ScriptableObject
    {
        public AutofacScope RootScope;

        public static AutofacSettings Instance { get; private set; }

        private void OnEnable()
        {
            Instance = this;

            if (RootScope != null)
            {
                RootScope.IsRoot = true;
                RootScope.Build();
            }
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            var assets = PlayerSettings.GetPreloadedAssets();
            if (Array.Exists(assets, o => o is AutofacSettings))
                return;

            Array.Resize(ref assets, assets.Length + 1);
            var autofacSettings = CreateInstance<AutofacSettings>();
            assets[^1] = Instance = autofacSettings;

            if (!AssetDatabase.IsValidFolder("Assets/Autofac"))
                AssetDatabase.CreateFolder("Assets", "Autofac");
            AssetDatabase.CreateAsset(autofacSettings, "Assets/Autofac/Settings.asset");
            AssetDatabase.SaveAssets();

            PlayerSettings.SetPreloadedAssets(assets);
        }
#endif
    }
}