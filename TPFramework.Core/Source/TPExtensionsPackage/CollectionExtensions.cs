/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TP.Framework
{
    public enum EncodingType
    {
        ASCII,
        UTF7,
        UTF8,
        UTF32,
        Unicode,
        BigEndianUnicode,
        Default,
    }

    public static partial class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToText(this char[] chars)
        {
            return new string(chars);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToText(this byte[] bytes, EncodingType type)
        {
            switch (type)
            {
                case EncodingType.UTF8:
                    return Encoding.UTF8.GetString(bytes);
                case EncodingType.ASCII:
                    return Encoding.ASCII.GetString(bytes);
                case EncodingType.Unicode:
                    return Encoding.Unicode.GetString(bytes);
                case EncodingType.UTF7:
                    return Encoding.UTF7.GetString(bytes);
                case EncodingType.UTF32:
                    return Encoding.UTF32.GetString(bytes);
                case EncodingType.BigEndianUnicode:
                    return Encoding.BigEndianUnicode.GetString(bytes);
                case EncodingType.Default:
                    return Encoding.Default.GetString(bytes);
                default:
                    return string.Empty;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] SortReverse(this int[] integers)
        {
            Array.Sort(integers);
            Array.Reverse(integers);
            return integers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] SortReverse(this float[] floats)
        {
            Array.Sort(floats);
            Array.Reverse(floats);
            return floats;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this T[] array)
        {
            return array[array.Length - 1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Find<T>(this T[] array, Predicate<T> match)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                if (match(array[i]))
                {
                    return array[i];
                }
            }
            return default(T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FindIndex<T>(this T[] array, int startIndex, int count, Predicate<T> match)
        {
            int endIndex = startIndex + count;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(array[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FindIndex<T>(this T[] array, int startIndex, Predicate<T> match)
        {
            return FindIndex(array, startIndex, array.Length - startIndex, match);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FindIndex<T>(this T[] array, Predicate<T> match)
        {
            return FindIndex(array, 0, array.Length, match);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this T[] array, Predicate<T> match)
        {
            int count = 0;
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                if (match(array[i]))
                {
                    count++;
                }
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this List<T> list, Predicate<T> match)
        {
            int count = 0;
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                if (match(list[i]))
                {
                    count++;
                }
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum(this int[] integers)
        {
            int sum = 0;
            int length = integers.Length;
            for (int i = 0; i < length; i++)
            {
                sum += integers[i];
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sum(this float[] floatings)
        {
            float sum = 0;
            int length = floatings.Length;
            for (int i = 0; i < length; i++)
            {
                sum += floatings[i];
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum(this int[] integers, int length)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
            {
                sum += integers[i];
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sum(this float[] floatings, int length)
        {
            float sum = 0;
            for (int i = 0; i < length; i++)
            {
                sum += floatings[i];
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum(this List<int> integers)
        {
            int sum = 0;
            int length = integers.Count;
            for (int i = 0; i < length; i++)
            {
                sum += integers[i];
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sum(this List<float> floatings)
        {
            float sum = 0;
            int length = floatings.Count;
            for (int i = 0; i < length; i++)
            {
                sum += floatings[i];
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum(this List<int> integers, int length)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
            {
                sum += integers[i];
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sum(this List<float> floatings, int length)
        {
            float sum = 0;
            for (int i = 0; i < length; i++)
            {
                sum += floatings[i];
            }
            return sum;
        }
    }
}
