/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TPFramework.Internal
{
    public class TPPackageManager
    {
        protected int packagesLength;

        public static TPPackageManager Manager { get; protected set; }
        public ITPDefineManager DefineManager { get; protected set; }
        public TPPackage[] Packages { get; protected set; }

        public TPPackageManager(ITPDefineManager defineManager = null, TPPackage[] packages = null)
        {
            Manager = this;
            DefineManager = defineManager;
            Packages = packages;
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

        public void InitializePackages(string[] packageNames, bool reload = true)
        {
            int length = packageNames.Length;
            List<TPPackage> packages = new List<TPPackage>(length);

            for (int i = 0; i < length; i++)
            {
                TPPackage package = new TPPackage(packageNames[i], () => { return true; });
                packages.Add(package);
            }

            Packages = packages.ToArray();
            packagesLength = length;

            if (reload)
            {
                ReloadPackages();
            }
        }

        public void SetPackages(TPPackage[] packages)
        {
            Packages = packages;
        }
    }
}
