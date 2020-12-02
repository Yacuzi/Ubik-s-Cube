using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;

namespace Sekamelicraft
{
    [InitializeOnLoad]
    public static class NoEditorShortcutsInPlaymode
    {
        private const string PLAYMODE_SHORTCUT_PROFILE = "Playmode";

        static NoEditorShortcutsInPlaymode()
        {
            SetDefaultShortcuts();
            EditorApplication.playModeStateChanged += PlayModeStateChanged;
        }

        static void PlayModeStateChanged(PlayModeStateChange stateChange)
        {
            switch (stateChange)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    SetPlaymodeShortcuts();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    SetDefaultShortcuts();
                    break;
            }
            if (!EditorApplication.isPlayingOrWillChangePlaymode &&
                 EditorApplication.isPlaying)
            {
                Debug.Log("Exiting playmode.");
            }
        }

        private static void SetDefaultShortcuts()
        {
            ShortcutManager.instance.activeProfileId = ShortcutManager.defaultProfileId;
        }

        private static void SetPlaymodeShortcuts()
        {
            IEnumerable<string> profileIDs = ShortcutManager.instance.GetAvailableProfileIds();
            bool exists = false;
            foreach (string profileID in profileIDs)
            {
                if (profileID == PLAYMODE_SHORTCUT_PROFILE)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                ShortcutManager.instance.CreateProfile(PLAYMODE_SHORTCUT_PROFILE);
                ShortcutManager.instance.activeProfileId = PLAYMODE_SHORTCUT_PROFILE;
                IEnumerable<string> shortcutIDs = ShortcutManager.instance.GetAvailableShortcutIds();
                foreach (string shortcutID in shortcutIDs)
                    ShortcutManager.instance.RebindShortcut(shortcutID, ShortcutBinding.empty);
            }
            ShortcutManager.instance.activeProfileId = PLAYMODE_SHORTCUT_PROFILE;
        }
    }
}