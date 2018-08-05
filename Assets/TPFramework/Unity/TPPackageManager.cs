#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TPFramework.Core;
using UnityEditor;
using UnityEngine;

namespace TPFramework.Internal
{
    internal interface ITPPackage
    {
        string Name { get; }
        bool IsLoaded { get; }

        bool Reload();
    }

    internal struct TPDefines
    {
        public const string TPEditorMessages = "TPFrameworkLogs";
        public const string TPObjectPoolSafeChecks = "TPObjectPoolSafeChecks";
        public const string TPTooltipSafeChecks = "TPTooltipSafeChecks";
        public const string TPUISafeChecks = "TPUISafeChecks";
    }

    [InitializeOnLoad]
    internal class TPPackageManager
    {
        public const string MENU = "TPFramework";

        private static BuildTargetGroup _targetGroup { get { return EditorUserBuildSettings.selectedBuildTargetGroup; } }
        private static readonly string _TPNamespace = "TPFramework";
        private const int packagesLength = 15;
        private static readonly ITPPackage[] _TPPackages = new ITPPackage[packagesLength] {
            new TPAchievementPackage(), // 0
            new TPPersistencePackage(), // 1
            new TPCollectionsPackage(), // 2
            new TPExtensionsPackage(),  // 3
            new TPObjectPoolPackage(),  // 4
            new TPInventoryPackage(),   // 5
            new TPAttributePackage(),   // 6
            new TPSettingsPackage(),    // 7
            new TPTooltipPackage(),     // 8
            new TPRandomPackage(),      // 9
            new TPEditorPackage(),      // 10
            new TPAudioPackage(),       // 11
            new TPAnimPackage(),        // 12
            new TPFadePackage(),        // 13
            new TPUIPackage(),          // 14
        };

        private static List<Type> GetExistingPackageTypes {
            get {
                return Assembly.GetExecutingAssembly().GetTypes().Where(typ => typ.HasNamespace(_TPNamespace)).ToList();
            }
        }

        private static bool HasTMPro {
            get {
                return AppDomain.CurrentDomain.GetAssemblies().Any(assembly => assembly.GetTypes().Any(typ => typ.HasNamespace("TMPro")));
            }
        }

        static TPPackageManager()
        {
            ReloadPackages();
            CheckFirstRun();
        }

#if TPFrameworkLogs

        [MenuItem(MENU + "/Disable Package Logs", priority = 1)]
#else
        [MenuItem(Menu + "/Enable Package Logs", priority = 1)]
#endif
        private static void ToggleMessages() { ToggleDefine(TPDefines.TPEditorMessages); }

        [MenuItem(MENU + "/Reload Packages", priority = 0)]
        private static void ReloadPackages()
        {
            if (!HasTMPro)
            {
                Debug.LogError("You don't have TextMeshPro installed. You can download it from <color=cyan> https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126 </color>");
            }

            ReloadPackages(GetExistingPackageTypes);
#if TPFrameworkLogs
            CheckLoadedPackages();
#endif
        }

        private static void ReloadPackages(List<Type> tpPackages)
        {
            for (int i = 0; i < packagesLength; i++)
            {
                if (tpPackages.Any(x => x.Name == _TPPackages[i].Name))
                {
                    _TPPackages[i].Reload();
                }
            }
        }

        private static void CheckLoadedPackages()
        {
            for (int i = 0; i < packagesLength; i++)
            {
                if (!_TPPackages[i].IsLoaded)
                    Debug.Log(_TPPackages[i].Name + "<color=red> was not found </color>");
            }
            Debug.Log("You can disable Package Logs in " + MENU + "/Disable Package Logs");
        }

        internal static void CheckFirstRun()
        {
            if (EditorPrefs.GetBool("TP_IsFirstRun", true))
            {
                SetDefine(TPDefines.TPEditorMessages, true);
                SetDefine(TPDefines.TPObjectPoolSafeChecks, true);
                SetDefine(TPDefines.TPTooltipSafeChecks, true);
                SetDefine(TPDefines.TPUISafeChecks, true);
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
        public string Name { get { return "TPAchievementPackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPPersistencePackage : ITPPackage
    {
        public string Name { get { return "TPPersistencePackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPExtensionsPackage : ITPPackage
    {
        public string Name { get { return "TPExtensionsPackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPObjectPoolPackage : ITPPackage
    {
        public string Name { get { return "TPObjectPoolPackage"; } }
        public bool IsLoaded { get; private set; }

#if TPObjectPoolSafeChecks

        [MenuItem(TPPackageManager.MENU + "/Disable TPObjectPool SafeChecks", priority = 60)]
#else
        [MenuItem(TPPackageManager.Menu + "/Enable TPObjectPool SafeChecks", priority = 60)]
#endif
        private static void ToggleSafeChecks() { TPPackageManager.ToggleDefine(TPDefines.TPObjectPoolSafeChecks); }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPInventoryPackage : ITPPackage
    {
        public string Name { get { return "TPInventoryPackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPAttributePackage : ITPPackage
    {
        public string Name { get { return "TPAttributePackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPSettingsPackage : ITPPackage
    {
        public string Name { get { return "TPSettingsPackage"; } }
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


    internal struct TPCollectionsPackage : ITPPackage
    {
        public string Name { get { return "TPCollectionsPackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPTooltipPackage : ITPPackage
    {
        public string Name { get { return "TPTooltipPackage"; } }
        public bool IsLoaded { get; private set; }

#if TPTooltipSafeChecks
        [MenuItem(TPPackageManager.MENU + "/Disable TPTooltip SafeChecks", priority = 120)]
#else
        [MenuItem(TPPackageManager.Menu + "/Enable TPTooltip SafeChecks", priority = 120)]
#endif
        private static void ToggleSafeChecks() { TPPackageManager.ToggleDefine(TPDefines.TPTooltipSafeChecks); }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPRandomPackage : ITPPackage
    {
        public string Name { get { return "TPRandomPackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPEditorPackage : ITPPackage
    {
        public string Name { get { return "TPEditorPackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPAudioPackage : ITPPackage
    {
        public string Name { get { return "TPAudioPackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPAnimPackage : ITPPackage
    {
        public string Name { get { return "TPAnimPackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPFadePackage : ITPPackage
    {
        public string Name { get { return "TPFadePackage"; } }
        public bool IsLoaded { get; private set; }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal class TPUIPackage : ITPPackage
    {
        public string Name { get { return "TPUIPackage"; } }
        public bool IsLoaded { get; private set; }

#if TPUISafeChecks
        [MenuItem(TPPackageManager.MENU + "/Disable TPUI SafeChecks", priority = 160)]
#else
        [MenuItem(TPPackageManager.Menu + "/Enable TPUI SafeChecks", priority = 160)]
#endif
        private static void ToggleSafeChecks() { TPPackageManager.ToggleDefine(TPDefines.TPUISafeChecks); }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }
}
#endif
