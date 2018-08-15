/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public static partial class TPExtensions
    {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async void DelayAction(float delay, Action action)
        {
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(delay));
            SafeInvoke(action);
        }
    }
}
