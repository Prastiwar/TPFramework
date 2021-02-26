/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TP.Framework
{
    public interface IAchievement<TData>
        where TData : struct, IAchievementData
    {
        TData Data { get; }

        Action OnCompleted { get; }

        void AddPoints(float points = 1);
        void Complete();
    }
}
