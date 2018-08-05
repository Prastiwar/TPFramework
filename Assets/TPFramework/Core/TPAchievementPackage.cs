using System;
using System.Collections;
using System.Collections.Generic;

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

    public interface ITPAchievement
    {
        TPAchievementData Data { get; }
        Action OnComplete { get; }
        void AddPoints(float points);
        void Complete();
    }
}
