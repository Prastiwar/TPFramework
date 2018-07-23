#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System;
using TPFramework;
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace TPFramework.Editor
{
    interface IPackageEditor { void Initialize(); }

    [Serializable]
    internal struct TPPackage
    {
        public string Name;
        public bool IsLoaded;
        public IPackageEditor Editor;

        internal TPPackage(string name, IPackageEditor editor, bool isLoaded = false)
        {
            Name = name;
            IsLoaded = isLoaded;
            Editor = editor;
        }
    }

    internal struct TPDefines
    {
        public const string TPEditorMessages = "TPFrameworkEditorMessages";
        public const string TPObjectPoolSafeChecks = "TPObjectPoolSafeChecks";
    }

    [InitializeOnLoad]
    internal class TPPackageManager
    {
        private static BuildTargetGroup _targetGroup { get { return EditorUserBuildSettings.selectedBuildTargetGroup; } }

        private static readonly string _TPNamespace = "TPFramework";
        private static readonly TPPackage[] _TPPackages = new TPPackage[] {
            new TPPackage("TPAchievement", new TPAchievementEditor()),
            new TPPackage("TPPersistence", new TPPersistenceEditor()),
            new TPPackage("TPExtensions", new TPExtensionsEditor()),
            new TPPackage("TPObjectPool", new TPObjectPoolEditor()),
            new TPPackage("TPAudioPool", new TPAudioPoolEditor()),
            new TPPackage("TPInventory", new TPInventoryEditor()),
            new TPPackage("TPAttribute", new TPAttributeEditor()),
            new TPPackage("TPSettings", new TPSettingsEditor()),
            new TPPackage("TPReusable", new TPReusableEditor()),
            new TPPackage("TPTooltip", new TPTooltipEditor()),
            new TPPackage("TPRandom", new TPRandomEditor()),
            new TPPackage("TPFade", new TPFadeEditor())
        };

#if TPFrameworkEditorMessages
        [MenuItem("TPFramework/Disable Package Editor Messages", priority = 0)]
#else
        [MenuItem("TPFramework/Enable Package Editor Messages", priority = 0)]
#endif
        private static void ToggleMessages() { ToggleDefine(TPDefines.TPEditorMessages); }

        [MenuItem("TPFramework/Reload Packages", priority = 1)]
        private static void ReloadPackages()
        {
            var allClasses = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsClass && a.Namespace != null && a.Namespace.Contains(_TPNamespace)).ToList();
            int length = _TPPackages.Length;
            for (int i = 0; i < length; i++)
            {
                if (allClasses.Any(x => x.Name == _TPPackages[i].Name))
                {
                    _TPPackages[i].IsLoaded = true;
                    _TPPackages[i].Editor.Initialize();
                }
#if TPFrameworkEditorMessages
                else
                {
                    Debug.Log(_TPPackages[i].Name + "<color=red> not found</color>");
                }
#endif
            }
#if TPFrameworkEditorMessages
            Debug.Log("You can disable package loader messages in TP/Disable Package Editor Messages");
#endif
        }

        static TPPackageManager()
        {
            ReloadPackages();
        }

        internal static void ToggleDefine(string define)
        {
            bool enabled = !EditorPrefs.GetBool(define, false);
            EditorPrefs.SetBool(define, enabled);

            if (enabled)
                TryAddDefine(define);
            else
                TryRemoveDefine(define);
        }

        internal static bool TryAddDefine(string define)
        {
            List<string> allDefines = GetDefines();
            if (!allDefines.Contains(define))
            {
                allDefines.Add(define);
                SetDefines(allDefines);
                return true;
            }
            return false;
        }

        internal static bool TryRemoveDefine(string define)
        {
            List<string> allDefines = GetDefines();
            if (allDefines.Contains(define))
            {
                allDefines.Remove(define);
                SetDefines(allDefines);
            }
            return false;
        }

        internal static void SetDefines(List<string> allDefines)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(_targetGroup, string.Join(";", allDefines.ToArray()));
        }

        internal static List<string> GetDefines()
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(_targetGroup);
            return defines.Split(';').ToList();
        }
    }


    internal struct TPAchievementEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }


    internal struct TPPersistenceEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }


    internal struct TPExtensionsEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }


    internal struct TPObjectPoolEditor : IPackageEditor
    {
#if TPObjectPoolSafeChecks
        [MenuItem("TPFramework/Disable TPObjectPool SafeChecks", priority = 5)]
#else
        [MenuItem("TPFramework/Enable TPObjectPool SafeChecks", priority = 5)]
#endif
        private static void TPObjectPoolSafeCheckToggle() { TPPackageManager.ToggleDefine(TPDefines.TPObjectPoolSafeChecks); }

        public void Initialize() { }
    }


    internal struct TPAudioPoolEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }


    internal struct TPInventoryEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }


    internal struct TPAttributeEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }


    internal struct TPSettingsEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }


    internal struct TPReusableEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }


    internal struct TPTooltipEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }


    internal struct TPRandomEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }


    internal struct TPFadeEditor : IPackageEditor
    {
        public void Initialize()
        {
        }
    }
}
#endif
