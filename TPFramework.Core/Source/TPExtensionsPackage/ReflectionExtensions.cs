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
        private static BindingFlags findBinding = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Object GetRelativePropertyValue(this Object obj, string propName)
        {
            string[] nameParts = propName.Split('.');
            if (nameParts.Length == 1)
            {
                return obj.GetType().GetProperty(propName, findBinding).GetValue(obj, null);
            }

            foreach (string part in nameParts)
            {
                if (obj == null)
                {
                    return null;
                }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null)
                {
                    return null;
                }
                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetRelativeFieldValue(this object obj, string fieldName)
        {
            string[] nameParts = fieldName.Split('.');
            if (nameParts.Length == 1)
            {
                return obj.GetType().GetField(fieldName, findBinding).GetValue(obj);
            }

            foreach (string part in nameParts)
            {
                if (obj == null)
                {
                    return null;
                }

                Type type = obj.GetType();
                FieldInfo info = type.GetField(part);
                if (info == null)
                {
                    return null;
                }
                obj = info.GetValue(obj);
            }
            return obj;
        }
    }
}
