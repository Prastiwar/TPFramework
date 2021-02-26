/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TP.Framework
{
    [Serializable]
    public class Achievement<TData> : IAchievement<TData>
        where TData : struct, IAchievementData
    {
        public Action OnCompleted { get; set; }
        public TData Data { get; private set; }

        public Achievement(TData data)
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
