/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TPFramework.Core
{
    public interface ITPAchievementData
    {
        string Title { get; }
        string Description { get; }
        float Points { get; }
        float ReachPoints { get; }
        bool IsCompleted { get; }
    }

    public interface ITPAchievement
    {
        ITPAchievementData Data { get; }

        Action OnComplete { get; }

        void AddPoints(float points);
        void Complete();
    }
}
