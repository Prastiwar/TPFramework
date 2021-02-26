/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Runtime.CompilerServices;

namespace TP.Framework
{
    public static partial class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Remove(this Action action, Action action2)
        {
            if (action != null)
            {
                action -= action2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeRemove<T>(this Action<T> action, Action<T> action2)
        {
            if (action != null)
            {
                action -= action2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeRemove<T1, T2>(this Action<T1, T2> action, Action<T1, T2> action2)
        {
            if (action != null)
            {
                action -= action2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(this Action action, Action action2)
        {
            if (action != null)
            {
                action += action2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeAdd<T>(this Action<T> action, Action<T> action2)
        {
            if (action != null)
            {
                action += action2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeAdd<T1, T2>(this Action<T1, T2> action, Action<T1, T2> action2)
        {
            if (action != null)
            {
                action += action2;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke(this Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<T>(this Action<T> action, T obj)
        {
            if (action != null)
            {
                action(obj);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 obj, T2 obj2)
        {
            if (action != null)
            {
                action(obj, obj2);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async void DelayAction(float delay, Action action)
        {
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(delay));
            SafeInvoke(action);
        }
    }
}
