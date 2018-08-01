﻿/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TPFramework
{
    public static class TPExtensions
    {
        private static readonly ReusableList<Vector3> reusableVector3 = new ReusableList<Vector3>();
        private static readonly char[] resolutionSeparators = new char[] { ' ', 'x', '@', 'H', 'z' };

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool IsFrame(int frameModulo)
        {
            return Time.frameCount % frameModulo == 0;
        }

        /* --------------------------------------------------------------- Primitives --------------------------------------------------------------- */

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool IsOutOfBounds<T>(this int integer, IEnumerable<T> collection)
        {
            if (integer < 0 || integer >= collection.Count())
                return true;
            return false;
        }

        /// <summary> Returns if integer is out of min(exclusive) and max(inclusive) </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool IsOutOfBounds<T>(this int integer, int min, int max)
        {
            if (integer < min || integer >= max)
                return true;
            return false;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int ToInt(this bool boolean)
        {
            return boolean ? 1 : 0;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool ToBool(this int integer)
        {
            return integer > 0;
        }

        /* --------------------------------------------------------------- Image --------------------------------------------------------------- */

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetAlpha(this Image image, float alpha)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        /* --------------------------------------------------------------- Rect --------------------------------------------------------------- */

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool IsInside(this Rect thisRect, Rect rect)
        {
            return thisRect.xMin <= rect.xMin
                && thisRect.xMax >= rect.xMax
                && thisRect.yMin <= rect.yMin
                && thisRect.yMax >= rect.yMax;
        }

        /* --------------------------------------------------------------- Transform --------------------------------------------------------------- */

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetPosX(this Transform transform, float x)
        {
            transform.position.Set(x, transform.position.y, transform.position.z);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetPosY(this Transform transform, float y)
        {
            transform.position.Set(transform.position.x, y, transform.position.z);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetPosZ(this Transform transform, float z)
        {
            transform.position.Set(transform.position.x, transform.position.y, z);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static Vector3[] GetChildrenPositions(this Transform parent)
        {
            int length = parent.childCount;
            List<Vector3> positions = reusableVector3.CleanList;
            for (int i = 0; i < length; i++)
                positions.Add(parent.GetChild(i).position);
            return positions.ToArray();
        }

        /* --------------------------------------------------------------- Vectors --------------------------------------------------------------- */

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool IsEqualTo(this Vector3 vector, Vector3 equalVector)
        {
            return vector.x == equalVector.x
                && vector.y == equalVector.y;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool IsEqualTo(this Vector3 vector, Vector2 equalVector)
        {
            return vector.x == equalVector.x
                && vector.y == equalVector.y;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static bool IsEqualTo(this Vector2 vector, Vector3 equalVector)
        {
            return vector.x == equalVector.x
                && vector.y == equalVector.y;
        }

        /* --------------------------------------------------------------- Collection --------------------------------------------------------------- */

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
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

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SortReverse(this int[] integers)
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
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SortReverse(this List<float> floats)
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
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SortReverse(this float[] floats)
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
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static T Last<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static T Last<T>(this T[] list)
        {
            return list[list.Length - 1];
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int Sum(this int[] integers)
        {
            int sum = 0;
            int length = integers.Length;
            for (int i = 0; i < length; i++)
                sum += integers[i];
            return sum;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float Sum(this float[] floatings)
        {
            float sum = 0;
            int length = floatings.Length;
            for (int i = 0; i < length; i++)
                sum += floatings[i];
            return sum;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int Sum(this int[] integers, int length)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
                sum += integers[i];
            return sum;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float Sum(this float[] floatings, int length)
        {
            float sum = 0;
            for (int i = 0; i < length; i++)
                sum += floatings[i];
            return sum;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int Sum(this List<int> integers)
        {
            int sum = 0;
            int length = integers.Count;
            for (int i = 0; i < length; i++)
                sum += integers[i];
            return sum;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float Sum(this List<float> floatings)
        {
            float sum = 0;
            int length = floatings.Count;
            for (int i = 0; i < length; i++)
                sum += floatings[i];
            return sum;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static int Sum(this List<int> integers, int length)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
                sum += integers[i];
            return sum;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float Sum(this List<float> floatings, int length)
        {
            float sum = 0;
            for (int i = 0; i < length; i++)
                sum += floatings[i];
            return sum;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static float GetFloat(this AudioMixer audioMixer, string paramName)
        {
            float value;
            bool result = audioMixer.GetFloat(paramName, out value);
            if (result)
                return value;
            else
                return 0f;
        }

        /* --------------------------------------------------------------- Strings --------------------------------------------------------------- */

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static string ToStringWithoutHZ(this Resolution resolution)
        {
            return resolution.width + " x " + resolution.height;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static string[] ToStringWithoutHZ(this Resolution[] resolutions)
        {
            int length = resolutions.Length;
            string[] resolutionsString = new string[length];
            for (int i = 0; i < length; i++)
                resolutionsString[i] = resolutions[i].ToStringWithoutHZ();
            return resolutionsString;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static string[] ToStringWithtHZ(this Resolution[] resolutions)
        {
            int length = resolutions.Length;
            string[] resolutionsString = new string[length];
            for (int i = 0; i < length; i++)
                resolutionsString[i] = resolutions[i].ToString();
            return resolutionsString;
        }

        /// <summary> resolutionText should be formatted as: "320 x 200 @ 60Hz" or "320 x 200" </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline        
        public static Resolution ToResolution(this string resolutionText)
        {
            string[] strings = resolutionText.Split(resolutionSeparators, StringSplitOptions.RemoveEmptyEntries);
            return new Resolution() {
                width = int.Parse(strings[0]),
                height = int.Parse(strings[1]),
                refreshRate = strings.Length >= 3 ? int.Parse(strings[2]) : 0
            };
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void ToLog(this object obj, string label = null)
        {
            Debug.Log(label != null ? label + ": " + obj : obj);
        }

#if NET_2_0 || NET_2_0_SUBSET

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static System.Collections.IEnumerator DelayAction(float delay, Action action)
        {
            while (delay-- >= 0)
                yield return null;
            if (action != null)
                action();
        }

#else
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static async void DelayAction(float delay, Action action)
        {
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(delay));
            if (action != null)
                action();
        }
#endif
    }
}
