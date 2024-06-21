using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AutofacUnity
{
    public class AutofacSettings : ScriptableObject
    {
        public const string FileResourcesPath = "Autofac/Settings";
        private const string FilePath = "Assets/Autofac/Resources/" + FileResourcesPath + ".asset";

        public AutofacScope RootScope;
        
        public static AutofacSettings Instance { get; internal set; }

        public static void LoadInstanceFromResources() 
            => Instance = Resources.Load<AutofacSettings>(FileResourcesPath);

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            if (AssetDatabase.LoadAssetAtPath<AutofacSettings>(FilePath) != null)
                return;

            var autofacSettings = CreateInstance<AutofacSettings>();

            if (!AssetDatabase.IsValidFolder("Assets/Autofac/Resources/Autofac/"))
                AssetDatabase.CreateFolder("Assets", "Autofac/Resources/Autofac/");

            AssetDatabase.CreateAsset(autofacSettings, FilePath);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}