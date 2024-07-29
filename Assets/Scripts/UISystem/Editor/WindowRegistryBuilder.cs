﻿using System;
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
    public class WindowRegistryBuilder
    {
        [MenuItem("Rogue Islands/Scan Windows")]
        public static void Scan()
        {
            var currentDictionary = new Dictionary<string, string>();

            foreach (var path in AssetDatabase.GetAllAssetPaths())
            {
                if (path.EndsWith(".prefab"))
                {
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab == null)
                        continue;

                    if (prefab.GetComponent<IWindow>() != null)
                        RegisterWindow(prefab, currentDictionary);
                }
            }

            var saver = EditorStaticResolver.Resolve<IAssetSaver>();
            var serializer = EditorStaticResolver.Resolve<ISerializer>();
            var serialized = serializer.Serialize(currentDictionary);
            var textAsset = new TextAsset(serialized);
            saver.Save(WindowRegistry.RegistryPath, textAsset);
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            Dictionary<string, string> currentDictionary = new();

            foreach (var path in importedAssets.Concat(movedAssets))
            {
                if (path.EndsWith(".prefab"))
                {
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab == null)
                        continue;
                    
                    if (prefab.GetComponent<IWindow>() != null)
                        RegisterWindow(prefab, currentDictionary);
                }
            }

            var saver = EditorStaticResolver.Resolve<IAssetSaver>();
            var serializer = EditorStaticResolver.Resolve<ISerializer>();
            var serialized = serializer.Serialize(currentDictionary);
            var textAsset = new TextAsset(serialized);
            saver.Save(WindowRegistry.RegistryPath, textAsset);
        }

        private static void RegisterWindow(GameObject gameObject, Dictionary<string, string> currentDictionary)
        {
            var saver = EditorStaticResolver.Resolve<IAssetSaver>();
            
            var path = saver.GetRuntimeLoadKey(gameObject);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"Window will not be able to be loaded at runtime. Window: {gameObject.name}");
                return;
            }

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
    }
}