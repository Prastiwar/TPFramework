/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.IO;

namespace TPFramework.Internal
{
    internal struct TPFrameworkInfo
    {
        public const int PackagesLength = 14;
        public const string TPCoreNamespace = "TPFramework.Core";

        public static string[] GetExistingPackagePaths {
            get { return Directory.GetDirectories(Environment.CurrentDirectory, "TP*Package.cs", SearchOption.AllDirectories); }
        }

        public static string[] GetTPPackageNames {
            get {
                return new string[PackagesLength]{
                    "TPAchievementPackage",
                    "TPPersistencePackage",
                    "TPCollectionsPackage",
                    "TPExtensionsPackage",
                    "TPObjectPoolPackage",
                    "TPInventoryPackage",
                    "TPAttributePackage",
                    "TPTooltipPackage",
                    "TPRandomPackage",
                    "TPAudioPackage",
                    "TPAnimPackage",
                    "TPFadePackage",
                    "TPMathPackage",
                    "TPUIPackage"
                };
            }
        }

        internal struct DefineNames
        {
            public const string TP = "TP";
        }
    }
}
