using System;
using System.Collections.Generic;
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
        private void OnPostprocessPrefab(GameObject g)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            if (g.GetComponent<UIRoot>() is { } root)
                RegisterUIRoot(root);
            if (g.GetComponent<IWindow>() != null)
                RegisterWindow(g);

            stopwatch.Stop();
            Debug.Log($"WindowRegistryBuilder.OnPostprocessPrefab took {stopwatch.ElapsedMilliseconds}ms");
        }

        private void RegisterWindow(GameObject gameObject)
        {
            var saver = EditorStaticResolver.Resolve<IAssetSaver>();

            if (!saver.CanAssetBeLoadedAtRuntime(gameObject))
            {
                Debug.LogError($"Window will not be able to be loaded at runtime. Window: {gameObject.name}");
                return;
            }

            var path = saver.GetRuntimeLoadKey(gameObject);
            var serializer = EditorStaticResolver.Resolve<ISerializer>();

            var currentDictionary = LoadWindowRegistry();

            var allTypes = new List<Type>();
            var allComponents = gameObject.GetComponents<Component>();
            foreach (var component in allComponents)
            {
                allTypes.Add(component.GetType());
                allTypes.AddRange(component.GetType().GetInterfaces());
            }

            foreach (var type in allTypes)
            {
                if (type.FullName == null)
                    continue;

                if (currentDictionary.TryGetValue(type.FullName, out var occupiedPath))
                {
                    var loader = EditorStaticResolver.Resolve<IAssetLoader>();
                    if (loader.Load<Object>(occupiedPath))
                        return;
                }
                
                currentDictionary[type.FullName] = path;
            }
            
            var serialized = serializer.Serialize(currentDictionary);
            var textAsset = new TextAsset(serialized);
            saver.Save(WindowRegistry.RegistryPath, textAsset);
        }

        private void RegisterUIRoot(UIRoot root)
        {
        }

        private Dictionary<string, string> LoadWindowRegistry()
        {
            var loader = EditorStaticResolver.Resolve<IAssetLoader>();
            var deserializer = EditorStaticResolver.Resolve<IDeserializer>();
            var textAsset = loader.Load<TextAsset>(WindowRegistry.RegistryPath);
            return deserializer.Deserialize<Dictionary<string, string>>(textAsset.text);
        }
    }
}