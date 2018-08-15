/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public static partial class TPExtensions
    {
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
    }
}
