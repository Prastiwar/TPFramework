/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public static class TPRandom
    {
        private static Random random = new Random();
        private static readonly int[] signs = new int[3] { 0, 1, -1 };
        private static readonly float[] signsProbability = new float[3] { 0.33f, 0.33f, 0.33f };

        /// <summary> Select between min [inclusive] and max [exclusive] </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Range(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min)) + min;
        }

        /// <summary> Select between min [inclusive] and max [exclusive] </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }

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

        /// <summary> Returns selected element of probability </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PickWithProbability<T>(params ProbabilityElementInt<T>[] elements)
        {
            int length = elements.Length;
            int dice = Range(0, Sum(elements, length));

            for (int i = 0; i < length; i++)
            {
                if (dice < elements[i].Probability)
                {
                    return elements[i].Element;
                }
                dice -= elements[i].Probability;
            }
            return elements[length - 1].Element;
        }

        /// <summary> Returns selected element of probability </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PickWithProbability<T>(params ProbabilityElement<T>[] elements)
        {
            int length = elements.Length;
            float dice = Range(0, Sum(elements, length));

            for (int i = 0; i < length; i++)
            {
                if (dice < elements[i].Probability)
                {
                    return elements[i].Element;
                }
                dice -= elements[i].Probability;
            }
            return elements[length - 1].Element;
        }

        /// <summary> Returns selected index of probabilities </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PickWithProbability(params int[] probabilities)
        {
            int length = probabilities.Length;
            int dice = Range(0, probabilities.Sum(length));

            for (int i = 0; i < length; i++)
            {
                if (dice < probabilities[i])
                {
                    return i;
                }
                dice -= probabilities[i];
            }
            return length - 1;
        }

        /// <summary> Returns selected index of probabilities </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PickWithProbability(params float[] probabilities)
        {
            int length = probabilities.Length;
            float dice = Range(0, probabilities.Sum(length));

            for (int i = 0; i < length; i++)
            {
                if (dice < probabilities[i])
                {
                    return i;
                }
                dice -= probabilities[i];
            }
            return length - 1;
        }

        /// <summary> Returns int array of random probabilities sorted from high to low </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomProbabilitiesDescent(int length, int total, int min, int max)
        {
            int[] chances = RandomProbabilitiesAscent(length, total, min, max);
            Array.Reverse(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from low to high </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomProbabilitiesAscent(int length, int total, int min, int max)
        {
            int[] chances = RandomProbabilities(length, total, min, max);
            Array.Sort(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from low to high </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomProbabilitiesAscent(int length, int min, int max)
        {
            int[] chances = new int[length];
            for (int i = 0; i < length; i++)
                chances[i] = Range(min, max);
            Array.Sort(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from high to low </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomProbabilitiesDescent(int length, int min, int max)
        {
            int[] chances = RandomProbabilitiesAscent(length, min, max);
            Array.Reverse(chances);
            return chances;
        }

        /// <summary> Returns int array of random total probabilities from min to max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomProbabilities(int length, int total = 100, int min = 1, int max = 100)
        {
            if ((max * length) < total)
                throw new Exception("Max (" + max + ") value can't be smaller than " + ((total / length) + 1));
            else if ((min * length) > total)
                throw new Exception("Min (" + min + ") value can't be larger than " + total / length);
            else if (max < min)
                throw new Exception("Max value can't be smaller than min!");

            int[] chances = new int[length];
            int sum = 0;
            max = TPMath.Clamp(max, ((total / length) + 1), total);
            min = TPMath.Clamp(min, 0, total / length);

            for (int index = 0; index < length; index++)
            {
                int lessTotal = total - sum;
                int counter = length - 1 - index;
                int low = TPMath.Clamp(lessTotal - (max * counter), min, max);
                int high = TPMath.Clamp(lessTotal - (min * counter), min, max);

                chances[index] = Range(low, high + 1);
                sum += chances[index];
            }
            return chances;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Sum<T>(ProbabilityElementInt<T>[] elements, int length)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
                sum += elements[i].Probability;
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Sum<T>(ProbabilityElement<T>[] elements, int length)
        {
            float sum = 0;
            for (int i = 0; i < length; i++)
                sum += elements[i].Probability;
            return sum;
        }
    }
}
