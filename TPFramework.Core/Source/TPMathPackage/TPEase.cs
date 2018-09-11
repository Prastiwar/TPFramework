/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System.Runtime.CompilerServices;

namespace TP.Framework
{
    public static class TPEase
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseIn(float from, float to, float time, float duration)
        {
            time /= duration;
            return to * time * time + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOut(float from, float to, float time, float duration)
        {
            time /= duration;
            return -to * time * (time - 2) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInCubic(float from, float to, float time, float duration)
        {
            time /= duration;
            return to * time * time * time + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutCubic(float time, float from, float to, float duration)
        {
            time /= duration;
            return to * (TPMath.Pow(time / duration - 1, 3) + 1) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutCubic(float time, float from, float to, float duration)
        {
            time /= duration;
            return (time / 2) < 1
                ? to / 2 * time * time * time + from
                : to / 2 * (TPMath.Pow(time - 2, 3) + 2) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInQuart(float from, float to, float time, float duration)
        {
            time /= duration;
            return to * time * time * time * time + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutQuart(float from, float to, float time, float duration)
        {
            return -to * (TPMath.Pow(time / duration - 1, 4) - 1) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutQuart(float from, float to, float time, float duration)
        {
            time /= duration;
            return (time / 2) < 1
                ? to / 2 * time * time * time * time + from
                : to / 2 * (TPMath.Pow(time - 2, 4) + 2) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInQuint(float from, float to, float time, float duration)
        {
            time /= duration;
            return to * time * time * time * time * time + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutQuint(float from, float to, float time, float duration)
        {
            return to * (TPMath.Pow(time / duration - 1, 5) + 1) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutQuint(float from, float to, float time, float duration)
        {
            time /= duration;
            return (time / 2) < 1
                ? to / 2 * time * time * time * time * time + from
                : to / 2 * (TPMath.Pow(time - 2, 5) + 2) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInSine(float from, float to, float time, float duration)
        {
            return to * (1 - TPMath.Cos(time / duration * (TPMath.PI / 2))) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutSine(float from, float to, float time, float duration)
        {
            return to * TPMath.Sin(time / duration * (TPMath.PI / 2)) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutSine(float from, float to, float time, float duration)
        {
            return to / 2 * (1 - TPMath.Cos(TPMath.PI * time / duration)) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInExpo(float from, float to, float time, float duration)
        {
            return to * TPMath.Pow(2, 10 * (time / duration - 1)) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutExpo(float from, float to, float time, float duration)
        {
            return to * (-TPMath.Pow(2, -10 * time / duration) + 1) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutExpo(float from, float to, float time, float duration)
        {
            return (time /= duration / 2) < 1
                ? to / 2 * TPMath.Pow(2, 10 * (time - 1)) + from
                : to / 2 * (-TPMath.Pow(2, -10 * --time) + 2) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInCirc(float from, float to, float time, float duration)
        {
            time /= duration;
            return to * (1 - TPMath.Sqrt(1 - time * time)) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutCirc(float from, float to, float time, float duration)
        {
            time /= duration;
            return to * TPMath.Sqrt(1 - (time - 1) * time) + from;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutCirc(float from, float to, float time, float duration)
        {
            time /= duration;
            return (time / 2) < 1
                ? to / 2 * (1 - TPMath.Sqrt(1 - time * time)) + from
                : to / 2 * (TPMath.Sqrt(1 - (time -= 2) * time) + 1) + from;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static float Spike(float t)
        //{
        //    return t <= 0.5f ? EaseIn(t / .5f) : EaseIn(TPTPMath.Flip(t) / .5f);
        //}
    }
}
