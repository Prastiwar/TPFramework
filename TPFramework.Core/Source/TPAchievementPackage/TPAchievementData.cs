/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TP.Framework
{
    [Serializable]
    public struct TPAchievementData : ITPAchievementData
    {
        public string Title       { get; set; }
        public string Description { get; set; }
        public float Points       { get; set; }
        public float ReachPoints  { get; set; }
        public bool IsCompleted   { get; set; }
    }
}
