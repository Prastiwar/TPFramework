/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TP.Framework
{
    public static partial class RandomSystem
    {
        private static Random random = new Random();
        private static readonly int[] signs = new int[3] { 0, 1, -1 };
        private static readonly float[] signsProbability = new float[3] { 0.33f, 0.33f, 0.33f };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RandomSign(float probability = 0.5f)
        {
            return signs[PickWithProbability(signsProbability)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RandomBool(float probability = 0.5f)
        {
            return Range(0f, 1f) < probability;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T>(List<T> collection)
        {
            return collection[Range(0, collection.Count)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T>(T[] collection)
        {
            return collection[Range(0, collection.Length)];
        }
    }
}
