/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

namespace TP.Framework
{
    public interface IAchievementData
    {
        string Title { get; }
        string Description { get; }
        float ReachPoints { get; }
        float Points { get; set; }
        bool IsCompleted { get; set; }
    }
}
