using System;
using System.Collections;
using System.Runtime.CompilerServices;
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
        public delegate void OnAnimActivationHandler(float time, Transform transform);
        
        /// <summary> Returns normalized value (0 to 1) when evaluatedTime is 0 - 0,5 and normalized value (1 to 0) when 0,5 - 1 </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float NormalizedCurveTime(float evaluatedTime)
        {
            return evaluatedTime <= 0.5f
                    ? (2 * evaluatedTime)         // grow from 0 to 1 when evaluate is from 0 to 0.5f 
                    : (2 - (2 * evaluatedTime));  // decrease from 1 to 0 when evaluate is from 0.5f to 1f
        }

        /// <summary> Runs coroutine that will call onAnimation every frame till evaluated time of anim.Curve will be 1.0f </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Animate(TPAnimation anim, Action<float> onAnimation, Action onStart = null, Action onEnd = null, float reachPoint = 1.0f, float startPoint = 0.0f)
        {
            TPCoroutine.RunCoroutine(IEAnimate(anim, onAnimation, onStart, onEnd, reachPoint, startPoint));
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static IEnumerator IEAnimate(TPAnimation anim, Action<float> onAnimation, Action onStart = null, Action onEnd = null, float reachPoint = 1.0f, float startPoint = 0.0f)
        {
            onStart.SafeInvoke();
            float percentage = startPoint;
            while (true)
            {
                float time = Mathf.Clamp01(anim.Curve.Evaluate(percentage));
                onAnimation(time);
                percentage += Time.deltaTime * anim.Speed;
                if (time >= reachPoint)
                    break;
                yield return null;
            }
            onEnd.SafeInvoke();
        }
    }
}
