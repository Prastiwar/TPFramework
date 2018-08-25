/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TPFramework.Core
{
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
    public struct ProbabilityElement<T>
    {
        public T Element;
        public float Probability;

        public ProbabilityElement(T element, float probability)
        {
            Element = element;
            Probability = probability;
        }
    }
}
