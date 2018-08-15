/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public static class TPMath
    {
        public static readonly float Epsilon = 0.000001f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sign(float floatNum)
        {
            return floatNum >= 0f ? 1f : -1f;
        }

        /// <summary> Interpolates between from and to by percentage - Clamp it between 0 and 1 </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float from, float to, float percentage)
        {
            return from + (to - from) * Clamp(percentage, 0f, 1f);
        }

        /// <summary> Interpolates between from and to by percentage as angle - Clamp it between 0 and 1 </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LerpAngle(float from, float to, float percentage)
        {
            float deltaAngle = DeltaAngle(from, to);
            return from + deltaAngle * Clamp01(percentage);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public float MoveTowards(float value, float targetValue, float maxDelta)
        {
            if (Math.Abs(targetValue - value) <= maxDelta)
                return targetValue;
            return value + Sign(targetValue - value) * maxDelta;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public float MoveTowardsAngle(float value, float targetValue, float maxDelta)
        {
            float deltaAngle = DeltaAngle(value, targetValue);
            if (-maxDelta < deltaAngle && deltaAngle < maxDelta)
                return targetValue;
            targetValue = value + deltaAngle;
            return MoveTowards(value, targetValue, maxDelta);
        }
        
        /// <summary> Loops value between 0 and length(exclusive) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Repeat(float value, float length)
        {
            float repeatValue = value - Floor(value / length) * length;
            return Clamp(repeatValue, 0.0f, length);
        }
        
        /// <summary> PingPongs value between 0 and length </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PingPong(float value, float length)
        {
            value = Repeat(value, length * 2f);
            return length - Math.Abs(value - length);
        }
        
        /// <summary> Returns the shortest difference between two angles </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DeltaAngle(float from, float to)
        {
            float delta = Repeat((to - from), 360.0f);
            if (delta > 180.0f)
                delta -= 360.0f;
            return delta;
        }

        /// <summary> Returns clamped value between min and max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            return value;
        }

        /// <summary> Returns clamped value between min and max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            else if (value > max)
                return max;
            return value;
        }

        /// <summary> Shorthand to clamp normalized float value between 0 to 1 </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp01(float value)
        {
            return Clamp(value, 0f, 1f);
        }

        /// <summary> Returns smallest integral value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Ceil(float value)
        {
            return (float)Math.Ceiling(value);
        }

        /// <summary> Returns the smallest integral value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilToInt(float value)
        {
            return (int)Math.Ceiling(value);
        }

        /// <summary> Returns the largest integral value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Floor(float value)
        {
            return (float)Math.Floor(value);
        }

        /// <summary> Returns the largest integral value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(float value)
        {
            return (int)Math.Floor(value);
        }

        /// <summary> Returns value raised to power of powValue </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Pow(float value, float powValue)
        {
            return (float)Math.Pow(value, powValue);
        }

        /// <summary> Returns value to nearest integral value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float value)
        {
            return (float)Math.Round(value);
        }

        /// <summary> Returns value to nearest integral value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(float value)
        {
            return (int)Math.Round(value);
        }

        /// <summary> Returns the square root of value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqrt(float value)
        {
            return (float)Math.Sqrt(value);
        }
    }
}
