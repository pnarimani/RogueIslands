using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Assets;
using RogueIslands.Assets.Editor;
using RogueIslands.Serialization;
using RogueIslands.Tools;
using RogueIslands.UISystem;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UISystem.Editor
{
    public class WindowRegistryBuilder : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            Dictionary<string, string> currentDictionary = null;

            foreach (var path in importedAssets.Concat(movedAssets))
            {
                if (path.EndsWith(".prefab"))
                {
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab == null)
                        continue;

                    currentDictionary ??= LoadWindowRegistry();

                    if (prefab.GetComponent<UIRoot>() is { } root)
                        RegisterUIRoot(root);
                    if (prefab.GetComponent<IWindow>() != null)
                        RegisterWindow(prefab, currentDictionary);
                }
            }

            if (currentDictionary != null)
            {
                var saver = EditorStaticResolver.Resolve<IAssetSaver>();
                var serializer = EditorStaticResolver.Resolve<ISerializer>();
                var serialized = serializer.Serialize(currentDictionary);
                var textAsset = new TextAsset(serialized);
                saver.Save(WindowRegistry.RegistryPath, textAsset);
            }
        }

        private static void RegisterWindow(GameObject gameObject, Dictionary<string, string> currentDictionary)
        {
            var saver = EditorStaticResolver.Resolve<IAssetSaver>();

            if (!saver.CanAssetBeLoadedAtRuntime(gameObject))
            {
                Debug.LogError($"Window will not be able to be loaded at runtime. Window: {gameObject.name}");
                return;
            }

            var path = saver.GetRuntimeLoadKey(gameObject);

            var allTypes = new List<Type>();
            var allComponents = gameObject.GetComponents<Component>();
            foreach (var component in allComponents)
            {
                allTypes.Add(component.GetType());
                allTypes.AddRange(component.GetType().GetInterfaces());
            }

            var loader = EditorStaticResolver.Resolve<IAssetLoader>();
            foreach (var type in allTypes)
            {
                if (type.FullName == null)
                    continue;
                if (type.Namespace == null || !type.Namespace.Contains("RogueIslands"))
                    continue;
                if (type == typeof(IWindow))
                    continue;

                if (!currentDictionary.TryGetValue(type.FullName, out var occupiedPath) ||
                    loader.Load<Object>(occupiedPath) == null)
                {
                    currentDictionary[type.FullName] = path;
                }
                
                if (!currentDictionary.TryGetValue(type.Name, out var occupiedShortPath) ||
                    loader.Load<Object>(occupiedShortPath) == null)
                {
                    currentDictionary[type.Name] = path;
                }
            }
        }

        private static void RegisterUIRoot(UIRoot root)
        {
        }

        private static Dictionary<string, string> LoadWindowRegistry()
        {
            var loader = EditorStaticResolver.Resolve<IAssetLoader>();
            var deserializer = EditorStaticResolver.Resolve<IDeserializer>();
            var textAsset = loader.Load<TextAsset>(WindowRegistry.RegistryPath);
            if (textAsset == null)
                return new Dictionary<string, string>();
            return deserializer.Deserialize<Dictionary<string, string>>(textAsset.text);
        }
    }
}