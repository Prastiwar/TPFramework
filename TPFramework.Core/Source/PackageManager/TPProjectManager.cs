/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System.IO;
using System.Runtime.CompilerServices;

namespace TP.Framework.Internal
{
    public class TPProjectManager
    {
        public static ProjectFolder[] DefaultFolders {
            get {
                return new ProjectFolder[] {
                    new ProjectFolder("Art", "Icon", "Character", "Environment", "Material", "VFX", "UI"),
                    new ProjectFolder("GameCode", "Editor", "Common", "Utilities"),
                    new ProjectFolder("Audio", "SFX", "Theme"),
                    new ProjectFolder("Animation", null),
                    new ProjectFolder("Plugins", null),
                    new ProjectFolder("Scene", null),
                };
            }
        }

        /// <summary> Generates generic folders in RootDirectory to easier manage project </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateProjectStructure(string projectName, string rootDirectory)
        {
            CreateProjectStructure(DefaultFolders, projectName, rootDirectory);
        }

        /// <summary> Generates generic folders in RootDirectory to easier manage project </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateProjectStructure<T>(T[] folders, string projectName, string rootDirectory) where T : ProjectFolder
        {
            string projectPath = Path.Combine(rootDirectory, projectName);
            int length = folders.Length;
            for (int i = 0; i < length; i++)
            {
                folders[i].CreateFolders(projectPath);
            }
        }
    }
}
