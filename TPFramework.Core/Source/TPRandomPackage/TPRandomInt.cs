/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Runtime.CompilerServices;

namespace TP.Framework
{
    public static partial class TPRandom
    {        
        /// <summary> Get randomly value between min [inclusive] and max [exclusive] </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary> Returns selected object from elements </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PickObjectWithProbability<T>(params ProbabilityElementInt<T>[] elements)
        {
            return PickWithProbability(elements).Element;
        }

        /// <summary> Returns selected element from elements </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ProbabilityElementInt<T> PickWithProbability<T>(params ProbabilityElementInt<T>[] elements)
        {
            int length = elements.Length;
            int dice = Range(0, Sum(elements, length));

            for (int i = 0; i < length; i++)
            {
                if (dice < elements[i].Probability)
                {
                    return elements[i];
                }
                dice -= elements[i].Probability;
            }
            return elements[length - 1];
        }

        /// <summary> Returns selected number from probabilities </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PickObjectWithProbability(params int[] probabilities)
        {
            return probabilities[PickWithProbability(probabilities)];
        }

        /// <summary> Returns selected index from probabilities </summary>
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
        
        /// <summary> Returns int array of random probabilities sorted from high to low </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomTotalProbabilitiesDescent(int length, int total, int minValue = 1, int maxValue = 100)
        {
            int[] chances = RandomTotalProbabilitiesAscent(length, total, minValue, maxValue);
            Array.Reverse(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from high to low </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomProbabilitiesDescent(int length, int minValue = 1, int maxValue = 100)
        {
            int[] chances = RandomProbabilitiesAscent(length, minValue, maxValue);
            Array.Reverse(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from low to high </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomTotalProbabilitiesAscent(int length, int total, int minValue = 1, int maxValue = 100)
        {
            int[] chances = RandomTotalProbabilities(length, total, minValue, maxValue);
            Array.Sort(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from low to high </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomProbabilitiesAscent(int length, int minValue = 1, int maxValue = 100)
        {
            int[] chances = RandomProbabilities(length, minValue, maxValue);
            Array.Sort(chances);
            return chances;
        }

        /// <summary> Returns int array of random total probabilities from min to max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomTotalProbabilities(int length, int total, int minValue = 1, int maxValue = 100)
        {
            if ((maxValue * length) < total)
                throw new Exception($"Max ({maxValue}) value can't be smaller than ({(total / length) + 1})");
            else if ((minValue * length) > total)
                throw new Exception($"Min ({minValue}) value can't be larger than {total / length}");
            else if (maxValue < minValue)
                throw new Exception("Max value can't be smaller than min!");

            int[] chances = new int[length];
            int sum = 0;
            maxValue = TPMath.Clamp(maxValue, ((total / length) + 1), total);
            minValue = TPMath.Clamp(minValue, 0, total / length);

            for (int index = 0; index < length; index++)
            {
                int lessTotal = total - sum;
                int counter = length - 1 - index;
                int low = TPMath.Clamp(lessTotal - (maxValue * counter), minValue, maxValue);
                int high = TPMath.Clamp(lessTotal - (minValue * counter), minValue, maxValue);

                chances[index] = Range(low, high + 1);
                sum += chances[index];
            }
            return chances;
        }

        /// <summary> Returns int array of random probabilities from min to max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] RandomProbabilities(int length, int minValue = 1, int maxValue = 100)
        {
            int[] chances = new int[length];
            for (int i = 0; i < length; i++)
            {
                chances[i] = Range(minValue, maxValue);
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
    }
}
