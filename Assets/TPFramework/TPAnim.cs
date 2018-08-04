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
        /// <summary> returns normalized value (0 to 1) when evaluatedTime is 0 - 0,5 and normalized value (1 to 0) when 0,5 - 1 </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float NormalizedCurveTime(float evaluatedTime)
        {
            return evaluatedTime <= 0.5f
                    ? (2 * evaluatedTime)         // grow from 0 to 1 when evaluate is from 0 to 0.5f 
                    : (2 - (2 * evaluatedTime));  // decrease from 1 to 0 when evaluate is from 0.5f to 1f
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Animate(TPAnimation anim, Action<float> onAnimation, Action onStart = null, Action onEnd = null)
        {
            TPCoroutine.RunCoroutine(IEAnimate(anim, onAnimation));
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static IEnumerator IEAnimate(TPAnimation anim, Action<float> onAnimation, Action onStart = null, Action onEnd = null)
        {
            onStart.SafeInvoke();
            float percentage = 0.0f;
            float time = anim.Curve.Evaluate(percentage);
            while (time < 1.0f)
            {
                onAnimation(time);
                percentage += Time.deltaTime * anim.Speed;
                time = anim.Curve.Evaluate(percentage);
                yield return null;
            }
            onEnd.SafeInvoke();
        }
    }
}
