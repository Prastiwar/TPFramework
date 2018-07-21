/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/
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

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static Vector3[] GetChildrenPositions(this Transform parent)
        {
            int length = parent.childCount;
            List<Vector3> positions = reusableVector3.CleanList;
            for (int i = 0; i < length; i++)
                positions.Add(parent.GetChild(i).position);
            return positions.ToArray();
        }

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

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SetAlpha(this Image image, float alpha)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void SortHighToLow(this List<int> list)
        {
            int count = list.Count;
            int shouldIndex = count - 1;
            int halfLength = count / 2;
            for (int i = 0; i < halfLength; i++)
            {
                int tempShuffle = list[i];
                list[i] = list[shouldIndex];
                list[shouldIndex] = tempShuffle;
                shouldIndex--;
            }
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
            string[] strings = resolutionText.Split(resolutionSeparators, System.StringSplitOptions.RemoveEmptyEntries);
            return new Resolution() {
                width = int.Parse(strings[0]),
                height = int.Parse(strings[1]),
                refreshRate = strings.Length >= 3 ? int.Parse(strings[2]) : 0
            };
        }
    }
}
