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
        Dictionary<Type, ITPPackage> Packages { get; }
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
        string FileName { get; }
        bool IsLoaded { get; }
        bool Reload();
    }

    /* ---------------------------------------------------------------- Framework Info ---------------------------------------------------------------- */

    internal struct TPFrameworkInfo
    {
        public const int PackagesLength = 16;
        public const string TPCoreNamespace = "TPFramework.Core";

        public static string[] GetExistingPackagePaths {
            get { return Directory.GetFiles(Environment.CurrentDirectory, "TP*Package.cs", SearchOption.AllDirectories); }
        }
    }

    /* ---------------------------------------------------------------- Package Manager ---------------------------------------------------------------- */

    internal class TPPackageManager : ITPPackageManager
    {
        public ITPDefineManager DefineManager { get; private set; }
        public Dictionary<Type, ITPPackage> Packages { get; private set; }

        public TPPackageManager(ITPDefineManager defineManager, ITPPackage[] overridePackages = null)
        {
            DefineManager = defineManager;
            Packages = new Dictionary<Type, ITPPackage>(TPFrameworkInfo.PackagesLength) {
                { typeof(TPAchievementPackage), new TPAchievementPackage() }, // 0
                { typeof(TPPersistencePackage), new TPPersistencePackage() }, // 1
                { typeof(TPCollectionsPackage), new TPCollectionsPackage() }, // 2
                { typeof(TPExtensionsPackage),  new TPExtensionsPackage()  }, // 3
                { typeof(TPObjectPoolPackage),  new TPObjectPoolPackage()  }, // 4
                { typeof(TPInventoryPackage),   new TPInventoryPackage()   }, // 5
                { typeof(TPAttributePackage),   new TPAttributePackage()   }, // 6
                { typeof(TPSettingsPackage),    new TPSettingsPackage()    }, // 7
                { typeof(TPTooltipPackage),     new TPTooltipPackage()     }, // 8
                { typeof(TPRandomPackage),      new TPRandomPackage()      }, // 9
                { typeof(TPEditorPackage),      new TPEditorPackage()      }, // 10
                { typeof(TPAudioPackage),       new TPAudioPackage()       }, // 11
                { typeof(TPAnimPackage),        new TPAnimPackage()        }, // 12
                { typeof(TPFadePackage),        new TPFadePackage()        }, // 13
                { typeof(TPMathPackage),        new TPMathPackage()        }, // 14
                { typeof(TPUIPackage),          new TPUIPackage()          }  // 15
            };

            if (overridePackages != null)
            {
                int length = overridePackages.Length;
                for (int i = 0; i < length; i++)
                {
                    Packages[overridePackages[i].GetType()] = overridePackages[i];
                }
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void ReloadPackages(string[] tpPackagesPaths)
        {
            int pathsLength = tpPackagesPaths.Length;
            foreach (var package in Packages)
            {
                Type typ = package.Key;
                for (int p = 0; p < pathsLength; p++)
                {
                    if (tpPackagesPaths[p].Contains(Packages[typ].FileName))
                    {
                        Packages[typ].Reload();
                        break;
                    }
                }
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public ITPPackage[] GetUnloadedPackages()
        {
            List<ITPPackage> unloadedPackages = new List<ITPPackage>();
            foreach (var package in Packages)
            {
                Type i = package.Key;
                if (!Packages[i].IsLoaded)
                {
                    unloadedPackages.Add(Packages[i]);
                }
            }
            return unloadedPackages.ToArray();
        }
    }

    /* ---------------------------------------------------------------- Packages ---------------------------------------------------------------- */

    internal class TPAchievementPackage : ITPPackage
    {
        public string FileName { get { return "TPAchievementPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPPersistencePackage : ITPPackage
    {
        public string FileName { get { return "TPPersistencePackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPCollectionsPackage : ITPPackage
    {
        public string FileName { get { return "TPCollectionsPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPExtensionsPackage : ITPPackage
    {
        public string FileName { get { return "TPExtensionsPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPObjectPoolPackage : ITPPackage
    {
        public string FileName { get { return "TPObjectPoolPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPInventoryPackage : ITPPackage
    {
        public string FileName { get { return "TPInventoryPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPAttributePackage : ITPPackage
    {
        public string FileName { get { return "TPAttributePackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPSettingsPackage : ITPPackage
    {
        public string FileName { get { return "TPSettingsPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPTooltipPackage : ITPPackage
    {
        public string FileName { get { return "TPTooltipPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPRandomPackage : ITPPackage
    {
        public string FileName { get { return "TPRandomPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPEditorPackage : ITPPackage
    {
        public string FileName { get { return "TPEditorPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPAudioPackage : ITPPackage
    {
        public string FileName { get { return "TPAudioPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPAnimPackage : ITPPackage
    {
        public string FileName { get { return "TPAnimPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPFadePackage : ITPPackage
    {
        public string FileName { get { return "TPFadePackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPMathPackage : ITPPackage
    {
        public string FileName { get { return "TPMathPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }

    internal class TPUIPackage : ITPPackage
    {
        public string FileName { get { return "TPUIPackage"; } }
        public bool IsLoaded { get; protected set; }

        public virtual bool Reload()
        {
            IsLoaded = true;
            return IsLoaded;
        }
    }
}
