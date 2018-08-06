#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using TPFramework.Core;

namespace TPFramework.Internal
{
    internal struct TPDefineInfo
    {
        public const string TPEditorMessages = "TPFrameworkLogs";
        public const string TPObjectPoolSafety = "TPObjectPoolSafeChecks";
        public const string TPTooltipSafety = "TPTooltipSafeChecks";
        public const string TPUISafety = "TPUISafeChecks";

        internal struct MenuMessage
        {
#if TPFrameworkLogs
            public const string TPEditorMessages = "Disable Package Logs";
#else
            public const string TPEditorMessages = "Enable Package Logs";
#endif
#if TPObjectPoolSafeChecks
            public const string TPObjectPoolSafeChecks = "Disable TPObjectPool Safe Checks";
#else
            public const string TPObjectPoolSafeChecks = "Enable TPObjectPool Safe Checks";
#endif
#if TPTooltipSafeChecks
            public const string TPTooltipSafeChecks = "Disable TPTooltip Safe Checks";
#else
            public const string TPTooltipSafeChecks = "Enable Package Logs";
#endif
#if TPUISafeChecks
            public const string TPUISafeChecks = "Disable TPUI Safe Checks";
#else
            public const string TPUISafeChecks = "Enable TPUI Safe Checks";
#endif
        }
    }

    internal struct TPDefineManager : ITPDefineManager
    {
        private static BuildTargetGroup _TargetGroup { get { return EditorUserBuildSettings.selectedBuildTargetGroup; } }

        public void CheckFirstRun()
        {
            if (EditorPrefs.GetBool("TP_IsFirstRun", true))
            {
                SetDefine(TPDefineInfo.TPEditorMessages, true);
                SetDefine(TPDefineInfo.TPObjectPoolSafety, true);
                SetDefine(TPDefineInfo.TPTooltipSafety, true);
                SetDefine(TPDefineInfo.TPUISafety, true);
                EditorPrefs.SetBool("TP_IsFirstRun", false);
            }
        }

        public void ToggleDefine(string define)
        {
            bool enabled = !EditorPrefs.GetBool(define, false);
            EditorPrefs.SetBool(define, enabled);
            SetDefine(define, enabled);
        }

        public void SetDefine(string define, bool enabled)
        {
            if (enabled)
                TryAddDefine(define);
            else
                TryRemoveDefine(define);
        }

        public bool IsDefined(string define)
        {
            return GetDefines().Contains(define);
        }

        private bool TryAddDefine(string define)
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

        private bool TryRemoveDefine(string define)
        {
            List<string> allDefines = GetDefines();
            if (allDefines.Contains(define))
            {
                allDefines.Remove(define);
                SetDefines(allDefines);
            }
            return false;
        }

        private void SetDefines(List<string> allDefines)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(_TargetGroup, string.Join(";", allDefines.ToArray()));
        }

        private List<string> GetDefines()
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(_TargetGroup);
            return defines.Split(';').ToList();
        }
    }

    [InitializeOnLoad]
    internal class TPPackageManager
    {
        internal const string MENU = "TPFramework/";
        internal static Core.TPPackageManager Manager { get; private set; }

        private static readonly ITPPackage[] _TPPackages = new ITPPackage[TPFrameworkInfo.PackagesLength] {
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

        private static bool HasTMPro {
            get {
                return AppDomain.CurrentDomain.GetAssemblies().Any(assembly => assembly.GetTypes().Any(typ => typ.HasNamespace("TMPro")));
            }
        }

        static TPPackageManager()
        {
            Manager = new Core.TPPackageManager(_TPPackages, new TPDefineManager());
            ReloadPackages();
            ((TPDefineManager)Manager.DefineManager).CheckFirstRun();
        }

        [MenuItem(MENU + TPDefineInfo.MenuMessage.TPEditorMessages, priority = 1)]
        private static void ToggleMessages() { Manager.DefineManager.ToggleDefine(TPDefineInfo.TPEditorMessages); }

        [MenuItem(MENU + "Reload Packages", priority = 0)]
        private static void ReloadPackages()
        {
            if (!HasTMPro)
            {
                Debug.LogError("You don't have TextMeshPro installed. You can download it from <color=cyan> https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126 </color>");
            }

            Manager.ReloadPackages(TPFrameworkInfo.GetExistingPackagePaths);

#if TPFrameworkLogs
            ITPPackage[] unloadedPackages = Manager.GetUnloadedPackages();
            if (unloadedPackages.Length > 0)
            {
                for (int i = 0; i < unloadedPackages.Length; i++)
                {
                    Debug.Log(unloadedPackages[i].Name + "<color=red> was not found </color>");
                }
                Debug.Log("You can disable Package Logs in " + MENU + "/Disable Package Logs");
            }
#endif
        }
    }


    internal struct TPObjectPoolPackage : ITPPackage
    {
        public string Name { get { return "TPObjectPoolPackage"; } }
        public bool IsLoaded { get; private set; }
        public int Index { get { return 4; } }

        [MenuItem(TPPackageManager.MENU + TPDefineInfo.MenuMessage.TPObjectPoolSafeChecks, priority = 60)]
        private static void ToggleSafeChecks() { TPPackageManager.Manager.DefineManager.ToggleDefine(TPDefineInfo.TPObjectPoolSafety); }

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
        public int Index { get { return 7; } }

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


    internal struct TPTooltipPackage : ITPPackage
    {
        public string Name { get { return "TPTooltipPackage"; } }
        public bool IsLoaded { get; private set; }
        public int Index { get { return 8; } }

        [MenuItem(TPPackageManager.MENU + TPDefineInfo.MenuMessage.TPTooltipSafeChecks, priority = 120)]
        private static void ToggleSafeChecks() { TPPackageManager.Manager.DefineManager.ToggleDefine(TPDefineInfo.TPTooltipSafety); }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }


    internal struct TPUIPackage : ITPPackage
    {
        public string Name { get { return "TPUIPackage"; } }
        public bool IsLoaded { get; private set; }
        public int Index { get { return 14; } }

        [MenuItem(TPPackageManager.MENU + TPDefineInfo.MenuMessage.TPUISafeChecks, priority = 160)]
        private static void ToggleSafeChecks() { TPPackageManager.Manager.DefineManager.ToggleDefine(TPDefineInfo.TPUISafety); }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }
}
#endif
