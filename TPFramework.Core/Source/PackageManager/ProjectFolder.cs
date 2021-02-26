/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TP.Framework.Internal
{
    [Serializable]
    public class ProjectFolder
    {
        public string RootName;
        public List<string> ChildNames;

        public ProjectFolder(string rootName, params string[] childNames)
        {
            RootName = rootName;
            ChildNames = childNames != null ? childNames.ToList() : new List<string>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateFolders(string rootPath)
        {
            string childPath = Path.Combine(rootPath, RootName);
            Directory.CreateDirectory(childPath);
            Parallel.ForEach(ChildNames, childName => Directory.CreateDirectory(Path.Combine(childPath, childName)));
        }
    }
}
