/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;

namespace TPFramework.Internal
{
    internal sealed class TPPackage
    {
        private readonly Func<bool> func;

        public string FileName { get; }
        public bool IsLoaded { get; set; }

        public TPPackage(string fileName, Func<bool> onReload)
        {
            FileName = fileName;
            func = onReload;
        }

        public bool Reload()
        {
            IsLoaded = func();
            return IsLoaded;
        }
    }
}
