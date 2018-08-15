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
    /* ---------------------------------------------------------------- Core ---------------------------------------------------------------- */

    /// <summary> Struct holds probability of int and element of T which can be returned in PickWithProbability </summary>
    /// <typeparam name="T"> Element can be returned if selected in Pick </typeparam>
    [Serializable]
    public struct ProbabilityElementInt<T>
    {
        public T Element;
        public int Probability;

        public ProbabilityElementInt(T element, int probability)
        {
            Element = element;
            Probability = probability;
        }
    }

    /// <summary> Struct holds probability of float and element of T which can be returned in PickWithProbability </summary>
    /// <typeparam name="T"> Element can be returned if selected in Pick </typeparam>
    [Serializable]
    public struct ProbabilityElementFloat<T>
    {
        public T Element;
        public float Probability;

        public ProbabilityElementFloat(T element, float probability)
        {
            Element = element;
            Probability = probability;
        }
    }


    public static class TPRandom
    {
        private static Random random = new Random();

        /// <summary> Select between min [inclusive] and max [exclusive] </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float Range(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min)) + min;
        }

        /// <summary> Select between min [inclusive] and max [exclusive] </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }

        // Example chances PickWithProbability(..)
        // [0] - 65
        // [1] - 15
        // [2] - 10
        // [3] - 10
        // x = dice from 0 to 100
        //              x < 65
        //           x-65 < 15
        //      (x-65)-10 < 10
        // ((x-65)-10)-10 < 10
        // dice e.g = 65 - select [1]
        // dice e.g = 43 - select [0]

        /// <summary> Returns selected element of probability </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
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
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static T PickWithProbability<T>(params ProbabilityElementFloat<T>[] elements)
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

        /// <summary> Returns selected index of probability </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
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

        /// <summary> Returns selected index of probability </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
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
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int[] RandomProbabilitiesDescent(int length, int total, int min, int max)
        {
            int[] chances = RandomProbabilitiesAscent(length, total, min, max);
            Array.Reverse(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from low to high </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int[] RandomProbabilitiesAscent(int length, int total, int min, int max)
        {
            int[] chances = RandomProbabilities(length, total, min, max);
            Array.Sort(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from low to high </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int[] RandomProbabilitiesAscent(int length, int min, int max)
        {
            int[] chances = new int[length];
            for (int i = 0; i < length; i++)
                chances[i] = Range(min, max);
            Array.Sort(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from high to low </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int[] RandomProbabilitiesDescent(int length, int min, int max)
        {
            int[] chances = RandomProbabilitiesAscent(length, min, max);
            Array.Reverse(chances);
            return chances;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
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

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool RandomBool(float probability = 0.5f)
        {
            return Range(0f, 1f) < probability;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static T RandomElement<T>(List<T> collection)
        {
            return collection[Range(0, collection.Count)];
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static T RandomElement<T>(T[] collection)
        {
            return collection[Range(0, collection.Length)];
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static int Sum<T>(ProbabilityElementInt<T>[] elements, int length)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
                sum += elements[i].Probability;
            return sum;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private static float Sum<T>(ProbabilityElementFloat<T>[] elements, int length)
        {
            float sum = 0;
            for (int i = 0; i < length; i++)
                sum += elements[i].Probability;
            return sum;
        }
    }
}
