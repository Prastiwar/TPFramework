/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.IO;

namespace TP.Framework.Internal
{
    public struct TPFrameworkInfo
    {
        public const int PackagesLength = 9;
        public const string TPCoreNamespace = "TPFramework.Core";

        internal static string[] GetExistingPackagePaths {
            get { return Directory.GetDirectories(Environment.CurrentDirectory, "TP*Package", SearchOption.AllDirectories); }
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
                    "TPRandomPackage",
                    "TPMathPackage",
                };
            }
        }

        internal struct DefineNames
        {
            public const string TP = "TP";
        }
    }
}
