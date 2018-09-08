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
        public static float Range(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min)) + min;
        }

        /// <summary> Returns selected element of probability </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PickObjectWithProbability<T>(params ProbabilityElement<T>[] elements)
        {
            return PickWithProbability(elements).Element;
        }

        /// <summary> Returns selected element of probability </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ProbabilityElement<T> PickWithProbability<T>(params ProbabilityElement<T>[] elements)
        {
            int length = elements.Length;
            float dice = Range(0, Sum(elements, length));

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
        public static float[] RandomTotalProbabilitiesDescent(int length, float total, float minValue = 1, float maxValue = 100)
        {
            float[] chances = RandomTotalProbabilitiesAscent(length, total, minValue, maxValue);
            Array.Reverse(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from high to low </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] RandomProbabilitiesDescent(int length, float minValue = 1, float maxValue = 100)
        {
            float[] chances = RandomProbabilitiesAscent(length, minValue, maxValue);
            Array.Reverse(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from low to high </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] RandomTotalProbabilitiesAscent(int length, float total, float minValue = 1, float maxValue = 100)
        {
            float[] chances = RandomTotalProbabilities(length, total, minValue, maxValue);
            Array.Sort(chances);
            return chances;
        }

        /// <summary> Returns int array of random probabilities sorted from low to high </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] RandomProbabilitiesAscent(int length, float minValue = 1, float maxValue = 100)
        {
            float[] chances = RandomProbabilities(length, minValue, maxValue);
            Array.Sort(chances);
            return chances;
        }

        /// <summary> Returns int array of random total probabilities from min to max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] RandomTotalProbabilities(int length, float total, float minValue = 1, float maxValue = 100)
        {
            if ((maxValue * length) < total)
                throw new Exception($"Max ({maxValue}) value can't be smaller than ({(total / length) + 1})");
            else if ((minValue * length) > total)
                throw new Exception($"Min ({minValue}) value can't be larger than {total / length}");
            else if (maxValue < minValue)
                throw new Exception("Max value can't be smaller than min!");

            float[] chances = new float[length];
            float sum = 0;
            maxValue = TPMath.Clamp(maxValue, ((total / length) + 1), total);
            minValue = TPMath.Clamp(minValue, 0, total / length);

            for (int index = 0; index < length; index++)
            {
                float lessTotal = total - sum;
                int counter = length - 1 - index;
                float low = TPMath.Clamp(lessTotal - (maxValue * counter), minValue, maxValue);
                float high = TPMath.Clamp(lessTotal - (minValue * counter), minValue, maxValue);

                chances[index] = Range(low, high + 1);
                sum += chances[index];
            }
            return chances;
        }

        /// <summary> Returns int array of random probabilities from min to max </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] RandomProbabilities(int length, float minValue = 1, float maxValue = 100)
        {
            float[] chances = new float[length];
            for (int i = 0; i < length; i++)
            {
                chances[i] = Range(minValue, maxValue);
            }
            return chances;
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
