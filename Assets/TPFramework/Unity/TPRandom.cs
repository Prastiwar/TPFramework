/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TPFramework.Core;
using UnityEngine;

namespace TPFramework.Unity
{
    /// <summary> Struct holds probability of int and element of T which can be returned in PickWithProbability </summary>
    /// <typeparam name="T"> Element can be returned if selected in Pick </typeparam>
    [System.Serializable]
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
    [System.Serializable]
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
            int dice = Random.Range(0, Sum(elements, length));

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
            float dice = Random.Range(0, Sum(elements, length));

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
            int dice = Random.Range(0, probabilities.Sum(length));

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
            float dice = Random.Range(0, probabilities.Sum(length));

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
            System.Array.Reverse(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from low to high </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int[] RandomProbabilitiesAscent(int length, int total, int min, int max)
        {
            int[] chances = RandomProbabilities(length, total, min, max);
            System.Array.Sort(chances);
            return chances;
        }

        public static int[] RandomProbabilitiesAscent(int length, int min, int max)
        {
            int[] chances = new int[length];
            for (int i = 0; i < length; i++)
                chances[i] = Random.Range(min, max);
            System.Array.Sort(chances);
            return chances;
        }

        public static int[] RandomProbabilitiesDescent(int length, int min, int max)
        {
            int[] chances = RandomProbabilitiesAscent(length, min, max);
            System.Array.Reverse(chances);
            return chances;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int[] RandomProbabilities(int length, int total = 100, int min = 1, int max = 100)
        {
            if ((max * length) < total)
                Debug.LogWarning("Max (" + max + ") value can't be smaller than " + ((total / length) + 1));
            else if ((min * length) > total)
                Debug.LogWarning("Min (" + min + ") value can't be larger than " + total / length);
            else if (max < min)
                Debug.LogWarning("Max value can't be smaller than min!");

            int[] chances = new int[length];
            int sum = 0;
            max = Mathf.Clamp(max, ((total / length) + 1), total);
            min = Mathf.Clamp(min, 0, total / length);

            for (int index = 0; index < length; index++)
            {
                int lessTotal = total - sum;
                int counter = length - 1 - index;
                int low = Mathf.Clamp(lessTotal - (max * counter), min, max);
                int high = Mathf.Clamp(lessTotal - (min * counter), min, max);

                chances[index] = Random.Range(low, high + 1);
                sum += chances[index];
            }
            return chances;
        }

        /// <summary> Returns a random point inside a box with radius 1 </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static Vector3 InsideUnitBox()
        {
            float randX = Random.Range(-1f, 1f);
            float randY = Random.Range(-1f, 1f);
            float randZ = Random.Range(-1f, 1f);
            return new Vector3(randX, randY, randZ);
        }

        /// <summary> Returns a random point inside a square with radius 1 </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static Vector2 InsideUnitSquare()
        {
            float randX = Random.Range(-1f, 1f);
            float randY = Random.Range(-1f, 1f);
            return new Vector2(randX, randY);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool RandomBool(float probability = 0.5f)
        {
            return Random.Range(0f, 1f) < probability;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static T RandomElement<T>(List<T> collection)
        {
            return collection[Random.Range(0, collection.Count)];
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static T RandomElement<T>(T[] collection)
        {
            return collection[Random.Range(0, collection.Length)];
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
