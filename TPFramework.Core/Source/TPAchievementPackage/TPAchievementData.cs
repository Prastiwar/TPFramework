/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TPFramework.Core
{
    [Serializable]
    public struct TPAchievementData
    {
        public string Title;
        public string Description;
        public float Points;
        public float ReachPoints;
        public bool IsCompleted;
    }
}
