/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public static partial class TPExtensions
    {
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
            attribute = GetSingleCustomAttribute<T>(fieldInfo, inherited);
            return attribute != null;
        }
    }
}
