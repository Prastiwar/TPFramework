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
        float ReachPoints { get; }
        float Points { get; set; }
        bool IsCompleted { get; set; }
    }

    public interface ITPAchievement<TData>
        where TData : struct, ITPAchievementData
    {
        TData Data { get; }

        Action OnCompleted { get; }

        void AddPoints(float points = 1);
        void Complete();
    }
}
