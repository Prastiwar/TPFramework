using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public static class TPMath
    {
        public static readonly float Epsilon = 0.000001f;

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float Sign(float floating)
        {
            return floating >= 0F ? 1F : -1F;
        }

        /// <summary> Interpolates between from and to by percentage - Clamp it between 0 and 1 </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float Lerp(float from, float to, float percentage)
        {
            return from + (to - from) * Clamp(percentage, 0f, 1f);
        }

        /// <summary> Clamps value between min and max</summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        /// <summary> Clamps value between min and max</summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }
    }
}
