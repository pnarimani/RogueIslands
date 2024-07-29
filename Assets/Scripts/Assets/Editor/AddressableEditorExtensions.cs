using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using static UnityEditor.AddressableAssets.Settings.AddressableAssetSettings;
using Object = UnityEngine.Object;

namespace RogueIslands.Assets.Editor
{
    internal static class AddressableEditorExtension
    {
        public static void SetAddressableID(this Object o, string id)
        {
            if (id.Length == 0) throw new ArgumentException(nameof(id));

            var settings = AddressableAssetSettingsDefaultObject.Settings;

            var entry = GetAddressableAssetEntry(o);
            if (entry != null)
            {
                entry.address = id;
            }
            else
            {
                var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(o));
                entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
                entry.address = id;
            }

            settings.SetDirty(ModificationEvent.EntryModified, entry, true, true);
        }

        public static AddressableAssetEntry GetAddressableAssetEntry(this Object o)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
                return default;

            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out var guid, out long _))
                return settings.FindAssetEntry(guid);
            return null;
        }
    }
}