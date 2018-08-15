/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System.IO;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public class TPProjectManager
    {
        /// <summary> Generates generic folders in RootDirectory to easier manage project </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateProjectStructure(string projectName, string rootDirectory)
        {
            string projectPath = Path.Combine(rootDirectory, projectName);
            string artPath = Path.Combine(projectPath, "Art");
            string audioPath = Path.Combine(projectPath, "Audio");
            string codePath = Path.Combine(projectPath, "GameCode");
            string scenePath = Path.Combine(projectPath, "Scene");
            string animPath = Path.Combine(projectPath, "Animation");

            CreateArtFolders(artPath);
            CreateAudioFolders(audioPath);
            CreateCodeFolders(codePath);

            Directory.CreateDirectory(scenePath);
            Directory.CreateDirectory(animPath);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CreateArtFolders(string root)
        {
            Directory.CreateDirectory(Path.Combine(root, "Character"));
            Directory.CreateDirectory(Path.Combine(root, "Environment"));
            Directory.CreateDirectory(Path.Combine(root, "Material"));
            Directory.CreateDirectory(Path.Combine(root, "UI"));
            Directory.CreateDirectory(Path.Combine(root, "VFX"));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CreateAudioFolders(string root)
        {
            Directory.CreateDirectory(Path.Combine(root, "SFX"));
            Directory.CreateDirectory(Path.Combine(root, "Theme"));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CreateCodeFolders(string root)
        {
            Directory.CreateDirectory(Path.Combine(root, "Plugins"));
            Directory.CreateDirectory(Path.Combine(root, "Editor"));
            Directory.CreateDirectory(Path.Combine(root, "Common"));
            Directory.CreateDirectory(Path.Combine(root, "Utilities"));
        }
    }
}
