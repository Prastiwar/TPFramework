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
        public static TPProjectFolder[] DefaultFolders {
            get {
                return new TPProjectFolder[] {
                    new TPProjectFolder("Art", "Icon", "Character", "Environment", "Material", "VFX", "UI"),
                    new TPProjectFolder("GameCode", "Editor", "Common", "Utilities"),
                    new TPProjectFolder("Audio", "SFX", "Theme"),
                    new TPProjectFolder("Animation", null),
                    new TPProjectFolder("Plugins", null),
                    new TPProjectFolder("Scene", null),
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
        public static void CreateProjectStructure<T>(T[] folders, string projectName, string rootDirectory) where T : TPProjectFolder
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
