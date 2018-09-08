/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TP.Framework
{
    [Serializable]
    public class TPAchievement<TData> : ITPAchievement<TData>
        where TData : struct, ITPAchievementData
    {
        public Action OnCompleted { get; set; }
        public TData Data { get; private set; }

        public TPAchievement(TData data)
        {
            Data = data;
        }

        public void Complete()
        {
            TData data = Data;
            data.Points = Data.ReachPoints;
            data.IsCompleted = true;
            Data = data;
            OnCompleted?.Invoke();
        }

        public void AddPoints(float points = 1)
        {
            TData data = Data;
            data.Points += points;
            Data = data;
            if (Data.Points >= Data.ReachPoints)
            {
                Complete();
            }
        }
    }
}
