using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RogueIslands.Localization;
using RogueIslands.Tools;
using TMPro;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;

namespace Localization.UnityLocalization.Editor
{
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : UnityEditor.Editor
    {
        private SerializedProperty _keyProperty;
        private List<string> _keys;
        private ReadOnlyCollection<StringTableCollection> _tables;

        private void OnEnable()
        {
            _keyProperty = serializedObject.FindProperty("_key");
            _tables = LocalizationEditorSettings.GetStringTableCollections();
            _keys = _tables.SelectMany(t => t.StringTables)
                .SelectMany(t => t.Values)
                .Select(x => x.Key).ToList();
            UpdateText();
        }

        public override void OnInspectorGUI()
        {
            var color = GUI.color;
            if (IsInvalidKey())
            {
                GUI.color = Color.red;
            }

            EditorGUILayout.PropertyField(_keyProperty);

            GUI.color = color;

            if (IsInvalidKey() && !string.IsNullOrEmpty(_keyProperty.stringValue))
            {
                foreach (var autoComplete in _keys.Where(k =>
                                 k.Contains(_keyProperty.stringValue, StringComparison.InvariantCultureIgnoreCase))
                             .Take(10))
                {
                    if (GUILayout.Button(autoComplete))
                    {
                        EditorGUIUtility.editingTextField = false;
                        _keyProperty.stringValue = autoComplete;
                        break;
                    }
                }
            }

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
                UpdateText();
            }
        }

        private void UpdateText()
        {
            var text = ((LocalizedText)serializedObject.targetObject).GetComponent<TextMeshProUGUI>();
            if (text != null) 
                text.text = GetLocalizedString();
        }

        private string GetLocalizedString()
        {
            return _tables.SelectMany(t => t.StringTables)
                .SelectMany(t => t.Values)
                .FirstOrDefault(t => t.Key == _keyProperty.stringValue)?
                .GetLocalizedString() ?? _keyProperty.stringValue;
        }

        private bool IsInvalidKey() => !_keys.Contains(_keyProperty.stringValue);
    }
}