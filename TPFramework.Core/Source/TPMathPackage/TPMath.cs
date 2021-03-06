﻿/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Runtime.CompilerServices;

namespace TP.Framework
{
    public static class TPMath
    {
        public const float Epsilon = 0.000001f;
        public const float PI = (float)Math.PI;

        /// <summary> Interpolates between from and to by percentage - Clamp it between 0 and 1 </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float from, float to, float percentage)
        {
            return from + (to - from) * Clamp(percentage);
        }

        /// <summary> Interpolates between from and to by percentage as angle - Clamp it between 0 and 1 </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LerpAngle(float from, float to, float percentage)
        {
            return from + (DeltaAngle(from, to) - from) * Clamp(percentage);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float StepTowards(float value, float targetValue, float maxStep)
        {
            float direction = targetValue - value;
            if (Math.Abs(direction) <= maxStep)
                return targetValue;
            return value + Math.Sign(direction) * maxStep;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float StepTowardsAngle(float value, float targetValue, float maxDelta)
        {
            float deltaAngle = DeltaAngle(value, targetValue);
            if (-maxDelta < deltaAngle && deltaAngle < maxDelta)
                return targetValue;
            targetValue = value + deltaAngle;
            return StepTowards(value, targetValue, maxDelta);
        }

        /// <summary> Returns the shortest difference between two angles </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DeltaAngle(float from, float to)
        {
            float delta = Repeat((to - from), 360.0f);
            return delta <= 180.0f ? delta : delta - 360.0f;
        }

        /// <summary> Loops value between 0 and maxValue(exclusive) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Repeat(float value, float maxValue)
        {
            float repeatValue = value - Floor(value / maxValue) * maxValue;
            return Clamp(repeatValue, 0.0f, maxValue);
        }

        /// <summary> Checks if two floating numbers are similiar </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(float a, float b)
        {
            return Math.Abs(b - a) < Math.Max(Epsilon * Math.Max(Math.Abs(a), Math.Abs(b)), Epsilon * 8);
        }

        /// <summary> PingPongs value between 0 and maxValue </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PingPong(float value, float maxValue)
        {
            value = Repeat(value, maxValue * 2);
            return maxValue - Math.Abs(value - maxValue);
        }

        /// <summary> Returns clamped value between min and max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float minValue = 0.0f, float maxValue = 1.0f)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            return value;
        }

        /// <summary> Returns clamped value between min and max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int minValue, int maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            return value;
        }

        /// <summary> Normalizes value between 0 and maxValue to 0f-1f range </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Normalize(float value, float maxValue)
        {
            return value / maxValue;
        }

        /// <summary> Normalizes value between minValue to maxValue to 0f - 1f range </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Normalize(float value, float minValue, float maxValue)
        {
            return (value - minValue) / (maxValue - minValue);
        }

        /// <summary> Normalizes value between 0 and maxValue to minNormalize - maxNormalize range </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Normalize(float value, float maxValue, float minNormalize, float maxNormalize)
        {
            float nomalizedNormalize = maxNormalize - minNormalize;
            return ((nomalizedNormalize * value) / maxValue) + minNormalize;
        }

        /// <summary> Normalizes value between minValue to maxValue to minNormalize - maxNormalize range </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Normalize(float value, float minValue, float maxValue, float minNormalize, float maxNormalize)
        {
            float normalizedValue = value - minValue;
            float normalizedMax = maxValue - minValue;
            float nomalizedNormalize = maxNormalize - minNormalize;
            return ((nomalizedNormalize * normalizedValue) / normalizedMax) + minNormalize;
        }

        /// <summary> Returns normalized percentage </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetPercentage(float current, float start, float end)
        {
            return (current - start) / (end - start);
        }

        /// <summary> Returns flipped value - in: 1, out: 0 - in: 0.8f, out 0.2f </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Flip(float value, float maxValue = 1)
        {
            return maxValue - value;
        }

        /// <summary> Returns count of digits in value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DigitCount(float value)
        {
            return value != 0.0f
                ? Math.Abs(FloorToInt(Log10(value) + 1))
                : 1;
        }

        /// <summary> Returns the base 10 logarithm of value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log10(float value)
        {
            return (float)Math.Log10(value);
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

        /// <summary> Returns the square root of value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(float value)
        {
            return (float)Math.Sin(value);
        }

        /// <summary> Returns the square root of value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(float value)
        {
            return (float)Math.Cos(value);
        }

        /// <summary> Returns value raised to power of exp </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Pow(float value, int exp)
        {
            float result = 1.0f;
            while (exp > 0)
            {
                if (exp % 2 == 1)
                    result *= value;
                exp >>= 1;
                value *= value;
            }
            return result;
        }

        /// <summary> Returns value raised to power of exp </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Pow(double value, int exp)
        {
            double result = 1.0;
            while (exp > 0)
            {
                if (exp % 2 == 1)
                    result *= value;
                exp >>= 1;
                value *= value;
            }
            return result;
        }

        /// <summary> Returns value raised to power of exp </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Pow(float value, float exp)
        {
            return (float)Math.Pow(value, exp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Factorial(float n)
        {
            float val = n;
            while (n > 2)
            {
                val *= n - 1;
                n -= 1;
            }
            return val;
        }

        /// <summary> Converts degrees to radians </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DegreesToRadians(double degrees)
        {
            return degrees * PI / 180.0;
        }

        /// <summary> Converts degrees to radians </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DegreesToRadians(float degrees)
        {
            return degrees * PI / 180.0f;
        }

        /// <summary> Returns array of values in sequence with gap between them (starts from 0) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] GetSequence(int length, float gap = 1)
        {
            return GetSequence(length, 0, gap);
        }

        /// <summary> Returns array of values in sequence with gap between them (starts from 0) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] GetReversedSequence(int length, float gap = 1)
        {
            return GetReversedSequence(length, 0, gap);
        }

        /// <summary> Returns array of values in sequence with gap between them (starts from startValue) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] GetSequence(int length, float startValue, float gap)
        {
            float[] sequence = new float[length];
            sequence[0] = startValue;
            for (int i = 1; i < length; i++)
            {
                sequence[i] = sequence[i - 1] + gap;
            }
            return sequence;
        }

        /// <summary> Returns array of values in reversed sequence with gap between them (starts from startValue) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] GetReversedSequence(int length, float startValue, float gap)
        {
            float[] sequence = new float[length];
            sequence[0] = startValue;
            for (int i = 1; i < length; i++)
            {
                sequence[i] = sequence[i - 1] - gap;
            }
            return sequence;
        }
    }
}
