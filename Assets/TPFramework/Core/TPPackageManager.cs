/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    /* ---------------------------------------------------------------- Core ---------------------------------------------------------------- */

    internal interface ITPPackageManager
    {
        ITPPackage[] Packages { get; }
        ITPPackage[] GetUnloadedPackages();
        ITPDefineManager DefineManager { get; }
        void ReloadPackages(string[] packagePaths);
    }

    internal interface ITPDefineManager
    {
        void ToggleDefine(string define);
        void SetDefine(string define, bool enabled);
        bool IsDefined(string define);
    }

    internal interface ITPPackage
    {
        string Name { get; }
        int Index { get; }
        bool IsLoaded { get; }
        bool Reload();
    }

    /* ---------------------------------------------------------------- Framework Info ---------------------------------------------------------------- */

    internal struct TPFrameworkInfo
    {
        public const int PackagesLength = 15;
        public const string TPCoreNamespace = "TPFramework.Core";

        public static string[] GetExistingPackagePaths {
            get { return Directory.GetFiles(Environment.CurrentDirectory, "TP*Package.cs", SearchOption.AllDirectories); }
        }
    }

    /* ---------------------------------------------------------------- Package Manager ---------------------------------------------------------------- */

    internal class TPPackageManager : ITPPackageManager
    {
        public ITPPackage[] Packages { get; private set; }
        public ITPDefineManager DefineManager { get; private set; }

        public TPPackageManager(ITPPackage[] packages, ITPDefineManager defineManager)
        {
            Packages = packages;
            DefineManager = defineManager;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void ReloadPackages(string[] tpPackagesPaths)
        {
            int pathsLength = tpPackagesPaths.Length;
            for (int i = 0; i < TPFrameworkInfo.PackagesLength; i++)
            {
                for (int p = 0; p < pathsLength; p++)
                {
                    if (tpPackagesPaths[p].Contains(Packages[i].Name))
                    {
                        Packages[i].Reload();
                        break;
                    }
                }
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public ITPPackage[] GetUnloadedPackages()
        {
            List<ITPPackage> unloadedPackages = new List<ITPPackage>();
            for (int i = 0; i < TPFrameworkInfo.PackagesLength; i++)
            {
                if (!Packages[i].IsLoaded)
                {
                    unloadedPackages.Add(Packages[i]);
                }
            }
            return unloadedPackages.ToArray();
        }
    }

    /* ---------------------------------------------------------------- Packages ---------------------------------------------------------------- */

    internal struct TPAchievementPackage : ITPPackage
    {
        public string Name { get { return "TPAchievementPackage"; } }
        public bool IsLoaded { get; private set; }
        public int Index { get { return 0; } }

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
        public int Index { get { return 1; } }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal struct TPCollectionsPackage : ITPPackage
    {
        public string Name { get { return "TPCollectionsPackage"; } }
        public bool IsLoaded { get; private set; }
        public int Index { get { return 2; } }

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
        public int Index { get { return 3; } }

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
        public int Index { get { return 4; } }

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
        public int Index { get { return 5; } }

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
        public int Index { get { return 6; } }

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
            return IsLoaded;
        }
    }

    internal struct TPTooltipPackage : ITPPackage
    {
        public string Name { get { return "TPTooltipPackage"; } }
        public bool IsLoaded { get; private set; }
        public int Index { get { return 8; } }

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
        public int Index { get { return 9; } }

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
        public int Index { get { return 10; } }

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
        public int Index { get { return 11; } }

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
        public int Index { get { return 12; } }

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
        public int Index { get { return 13; } }

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
        public int Index { get { return 14; } }

        public bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }
}
