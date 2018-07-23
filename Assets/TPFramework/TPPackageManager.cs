#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace TPFramework.Editor
{
    interface ITPPackage
    {
        string Name { get; }
        bool IsLoaded { get; }
        bool Reload();
    }

    internal struct TPDefines
    {
        public const string HasTMPRO = "HAS_TMPRO";
        public const string TPEditorMessages = "TPFrameworkLogs";
        public const string TPObjectPoolSafeChecks = "TPObjectPoolSafeChecks";
        public const string TPTooltipSafeChecks = "TPTooltipSafeChecks";
    }

    [InitializeOnLoad]
    internal class TPPackageManager
    {
        internal const string Menu = "TPFramework";
        private static BuildTargetGroup _targetGroup { get { return EditorUserBuildSettings.selectedBuildTargetGroup; } }

        private static readonly string _TPNamespace = "TPFramework";
        private static readonly int packagesLength = 12;
        private static readonly ITPPackage[] _TPPackages = new ITPPackage[] {
            new TPAchievementPackage(), // 0
            new TPPersistencePackage(), // 1
            new TPExtensionsPackage(),  // 2
            new TPObjectPoolPackage(),  // 3
            new TPAudioPoolPackage(),   // 4
            new TPInventoryPackage(),   // 5
            new TPAttributePackage(),   // 6
            new TPSettingsPackage(),    // 7
            new TPReusablePackage(),    // 8
            new TPTooltipPackage(),     // 9
            new TPRandomPackage(),      // 10
            new TPFadePackage(),        // 11
        };

#if TPFrameworkLogs
        [MenuItem(Menu + "/Disable Package Logs", priority = 1)]
#else
        [MenuItem("TPFramework/Enable Package Logs", priority = 1)]
#endif
        private static void ToggleMessages() { ToggleDefine(TPDefines.TPEditorMessages); }

        [MenuItem(Menu + "/Reload Packages", priority = 0)]
        private static void ReloadPackages()
        {
            SetDefine(TPDefines.HasTMPRO, Assembly.GetExecutingAssembly().GetTypes().Any(a => a.IsClass && a.Namespace != null && a.Namespace.Contains("TMPro")));
            
            List<Type> tpPackages = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsClass && a.Namespace != null && a.Namespace.Contains(_TPNamespace)).ToList();
            for (int i = 0; i < packagesLength; i++)
            {
                if (tpPackages.Any(x => x.Name == _TPPackages[i].Name))
                {
                    _TPPackages[i].Reload();
                }
            }
#if TPFrameworkLogs
            for (int i = 0; i < packagesLength; i++)
            {
                if (!_TPPackages[i].IsLoaded)
                    Debug.Log(_TPPackages[i].Name + "<color=red> was not found</color>");
            }
            Debug.Log("You can disable Package Logs in " + Menu + "/Disable Package Logs");
#endif
        }

        static TPPackageManager()
        {
            ReloadPackages();
            CheckFirstRun();
        }

        internal static void CheckFirstRun()
        {
            if (EditorPrefs.GetBool("TP_IsFirstRun", true))
            {
                SetDefine(TPDefines.TPEditorMessages, true);
                SetDefine(TPDefines.TPObjectPoolSafeChecks, true);
                SetDefine(TPDefines.TPTooltipSafeChecks, true);
                EditorPrefs.SetBool("TP_IsFirstRun", false);
            }
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

        internal static void SetDefine(string define, bool enabled)
        {
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

        internal static bool IsDefined(string define)
        {
            return GetDefines().Contains(define);
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


    internal struct TPAchievementPackage : ITPPackage
    {
        public string Name { get { return "TPAchievement"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPPersistencePackage : ITPPackage
    {
        public string Name { get { return "TPPersistence"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPExtensionsPackage : ITPPackage
    {
        public string Name { get { return "TPExtensions"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPObjectPoolPackage : ITPPackage
    {
        public string Name { get { return "TPObjectPool"; } }
        public bool IsLoaded { get; private set; }

#if TPObjectPoolSafeChecks
        [MenuItem(TPPackageManager.Menu + "/Disable TPObjectPool SafeChecks", priority = 60)]
#else
        [MenuItem(TPPackageManager.Menu + "/Enable TPObjectPool SafeChecks", priority = 60)]
#endif
        private static void TPObjectPoolSafeCheckToggle() { TPPackageManager.ToggleDefine(TPDefines.TPObjectPoolSafeChecks); }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPAudioPoolPackage : ITPPackage
    {
        public string Name { get { return "TPAudioPool"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPInventoryPackage : ITPPackage
    {
        public string Name { get { return "TPInventory"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPAttributePackage : ITPPackage
    {
        public string Name { get { return "TPAttribute"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPSettingsPackage : ITPPackage
    {
        public string Name { get { return "TPSettings"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;

            if (!QualitySettings.names.Any(x => x == "Custom"))
            {
                Debug.LogError("No 'Custom' quality level found. Create one in Edit -> Project Settings -> Quality -> Add Quality Level");
            }

            return IsLoaded;
        }
    }


    internal struct TPReusablePackage : ITPPackage
    {
        public string Name { get { return "TPReusable"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPTooltipPackage : ITPPackage
    {
        public string Name { get { return "TPTooltip"; } }
        public bool IsLoaded { get; private set; }

#if TPTooltipSafeChecks
        [MenuItem(TPPackageManager.Menu + "/Disable TPTooltip SafeChecks", priority = 120)]
#else
        [MenuItem(TPPackageManager.Menu + "/Enable TPTooltip SafeChecks", priority = 120)]
#endif
        private static void TPObjectPoolSafeCheckToggle() { TPPackageManager.ToggleDefine(TPDefines.TPTooltipSafeChecks); }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPRandomPackage : ITPPackage
    {
        public string Name { get { return "TPRandom"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPFadePackage : ITPPackage
    {
        public string Name { get { return "TPFade"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }
}
#endif
