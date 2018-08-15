/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public static class TPExtensions
    {
        /* --------------------------------------------------------------- Utility --------------------------------------------------------------- */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke(this Action action)
        {
            if (action != null)            
                action();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<T>(this Action<T> action, T obj)
        {
            if (action != null)
                action(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 obj, T2 obj2)
        {
            if (action != null)
                action(obj, obj2);
        }

#if NET_2_0 || NET_2_0_SUBSET

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Collections.IEnumerator DelayAction(float delay, Action action)
        {
            while (delay-- >= 0)
                yield return null;
            if (action != null)
                action();
        }

#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async void DelayAction(float delay, Action action)
        {
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(delay));
            if (action != null)
                action();
        }
#endif

        /* --------------------------------------------------------------- Primitives --------------------------------------------------------------- */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOutOfBounds<T>(this int integer, IEnumerable<T> collection)
        {
            if (integer < 0 || integer >= collection.Count())
                return true;
            return false;
        }

        /// <summary> Returns if integer is out of min(exclusive) and max(inclusive) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOutOfBounds<T>(this int integer, int min, int max)
        {
            if (integer < min || integer >= max)
                return true;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this bool boolean)
        {
            return boolean ? 1 : 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToBool(this int integer)
        {
            return integer > 0;
        }

        /* --------------------------------------------------------------- Collection --------------------------------------------------------------- */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortReverse(this List<int> integers)
        {
            int count = integers.Count;
            int shouldIndex = count - 1;
            int halfLength = count / 2;
            for (int i = 0; i < halfLength; i++)
            {
                int tempShuffle = integers[i];
                integers[i] = integers[shouldIndex];
                integers[shouldIndex] = tempShuffle;
                shouldIndex--;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] SortReverse(this int[] integers)
        {
            int count = integers.Length;
            int shouldIndex = count - 1;
            int halfLength = count / 2;
            for (int i = 0; i < halfLength; i++)
            {
                int tempShuffle = integers[i];
                integers[i] = integers[shouldIndex];
                integers[shouldIndex] = tempShuffle;
                shouldIndex--;
            }
            return integers;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<float> SortReverse(this List<float> floats)
        {
            int count = floats.Count;
            int shouldIndex = count - 1;
            int halfLength = count / 2;
            for (int i = 0; i < halfLength; i++)
            {
                float tempShuffle = floats[i];
                floats[i] = floats[shouldIndex];
                floats[shouldIndex] = tempShuffle;
                shouldIndex--;
            }
            return floats;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] SortReverse(this float[] floats)
        {
            int count = floats.Length;
            int shouldIndex = count - 1;
            int halfLength = count / 2;
            for (int i = 0; i < halfLength; i++)
            {
                float tempShuffle = floats[i];
                floats[i] = floats[shouldIndex];
                floats[shouldIndex] = tempShuffle;
                shouldIndex--;
            }
            return floats;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this T[] list)
        {
            return list[list.Length - 1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum(this int[] integers)
        {
            int sum = 0;
            int length = integers.Length;
            for (int i = 0; i < length; i++)
                sum += integers[i];
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sum(this float[] floatings)
        {
            float sum = 0;
            int length = floatings.Length;
            for (int i = 0; i < length; i++)
                sum += floatings[i];
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum(this int[] integers, int length)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
                sum += integers[i];
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sum(this float[] floatings, int length)
        {
            float sum = 0;
            for (int i = 0; i < length; i++)
                sum += floatings[i];
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum(this List<int> integers)
        {
            int sum = 0;
            int length = integers.Count;
            for (int i = 0; i < length; i++)
                sum += integers[i];
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sum(this List<float> floatings)
        {
            float sum = 0;
            int length = floatings.Count;
            for (int i = 0; i < length; i++)
                sum += floatings[i];
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum(this List<int> integers, int length)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
                sum += integers[i];
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sum(this List<float> floatings, int length)
        {
            float sum = 0;
            for (int i = 0; i < length; i++)
                sum += floatings[i];
            return sum;
        }

        /* --------------------------------------------------------------- Reflection --------------------------------------------------------------- */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasNamespace(this Type type, string nameSpace)
        {
            return type.IsClass && type.Namespace != null && type.Namespace.Contains(nameSpace);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetSingleCustomAttribute<T>(this FieldInfo fieldInfo, bool inherited = false) where T : Attribute
        {
            Type type = typeof(T);
            if (fieldInfo.IsDefined(type, inherited))
            {
                return (T)fieldInfo.GetCustomAttributes(type, inherited)[0];
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetCustomAttribute<T>(this FieldInfo fieldInfo, out T attribute, bool inherited = false) where T : Attribute
        {
            Type type = typeof(T);
            if (fieldInfo.IsDefined(type, inherited))
            {
                attribute = (T)fieldInfo.GetCustomAttributes(type, inherited)[0];
                return true;
            }
            attribute = null;
            return false;
        }
    }
}
