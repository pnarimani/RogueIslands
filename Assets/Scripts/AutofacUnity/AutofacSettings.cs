using System;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace AutofacUnity
{
    public class AutofacSettings : ScriptableObject
    {
        public const string FileResourcesPath = "Autofac/Settings";
        private const string FilePath = "Assets/Autofac/Resources/" + FileResourcesPath + ".asset";

        [FormerlySerializedAs("RootScope")] [SerializeField]
        private AutofacScope _rootScope;

        public static AutofacSettings Instance { get; internal set; }

        public AutofacScope RootScope { get; private set; }

        public static void LoadInstanceFromResources()
            => Instance = Resources.Load<AutofacSettings>(FileResourcesPath);

        public bool HasRootScope() => _rootScope != null;

        public void InitializeRootScope()
        {
            if (RootScope != null)
                return;
            _rootScope.AutoRun = false;
            RootScope = Instantiate(_rootScope);
            RootScope.IsRoot = true;
            RootScope.Build();
            DontDestroyOnLoad(RootScope.gameObject);
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            if (File.Exists(Path.GetFullPath(FilePath)))
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