using System;
using UnityEngine;

namespace TPFramework
{
    [Serializable]
    public struct TPAnimation
    {
        public AnimationCurve Curve;
        public float Speed;
    }

    public static class TPAnim
    {
        /// <summary> returns normalized value (0 to 1) when evaluatedTime is 0 - 0,5 and normalized value (1 to 0) when 0,5 - 1 </summary>
        public static float NormalizedCurveTime(float evaluatedTime)
        {
            return evaluatedTime <= 0.5f
                    ? (2 * evaluatedTime)         // grow from 0 to 1 when evaluate is from 0 to 0.5f 
                    : (2 - (2 * evaluatedTime));  // decrease from 1 to 0 when evaluate is from 0.5f to 1f
        }
    }
}
