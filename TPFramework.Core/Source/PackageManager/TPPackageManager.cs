/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TPFramework.Internal
{
    internal class TPPackageManager
    {
        public ITPDefineManager DefineManager { get; protected set; }
        public TPPackage[] Packages { get; protected set; }

        protected int packagesLength;

        public TPPackageManager()
        {
            InitializePackages(TPFrameworkInfo.GetExistingPackagePaths);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReloadPackages()
        {
            for (int i = 0; i < packagesLength; i++)
            {
                Packages[i].Reload();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TPPackage[] GetFailedLoadPackages()
        {
            List<TPPackage> unloadedPackages = new List<TPPackage>();
            for (int i = 0; i < packagesLength; i++)
            {
                if (!Packages[i].IsLoaded)
                {
                    unloadedPackages.Add(Packages[i]);
                }
            }
            return unloadedPackages.ToArray();
        }

        protected virtual void InitializePackages(string[] packagesPaths, bool reload = true)
        {
            List<TPPackage> packages = new List<TPPackage>(TPFrameworkInfo.PackagesLength);

            int length = packagesPaths.Length;
            for (int i = 0; i < length; i++)
            {
                TPPackage package = new TPPackage(packagesPaths[i], () => {
                    Console.WriteLine(packagesPaths[i]);
                    return true;
                });
                packages.Add(package);
            }

            Packages = packages.ToArray();
            packagesLength = Packages.Length;

            if (reload)
                ReloadPackages();
        }
    }
}
