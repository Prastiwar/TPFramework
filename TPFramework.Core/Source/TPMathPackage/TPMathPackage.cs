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

        /// <summary> Clamps value between min and max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        /// <summary> Clamps value between min and max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
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
