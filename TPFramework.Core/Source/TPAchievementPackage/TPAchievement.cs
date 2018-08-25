/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TPFramework.Core
{
    [Serializable]
    public class TPAchievement<TData> : ITPAchievement<TData>
        where TData : class, ITPAchievementData
    {
        public Action OnCompleted { get; set; }
        public TData Data { get; private set; }

        public TPAchievement(TData data)
        {
            Data = data;
        }

        public void Complete()
        {
            Data.Points = Data.ReachPoints;
            Data.IsCompleted = true;
            OnCompleted?.Invoke();
        }

        public void AddPoints(float points = 1)
        {
            Data.Points += points;
            if (Data.Points >= Data.ReachPoints)
            {
                Complete();
            }
        }
    }
}
