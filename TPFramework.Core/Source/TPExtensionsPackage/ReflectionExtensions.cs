/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TP.Framework
{
    public static partial class TPExtensions
    {
        public static readonly BindingFlags PublicOrNotInstance = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

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
        public static object GetRelativePropertyValue(this object obj, string propName)
        {
            string[] nameParts = propName.Split('.');
            if (nameParts.Length == 1)
            {
                return obj.GetType().GetProperty(propName, PublicOrNotInstance).GetValue(obj, null);
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
                return obj.GetType().GetField(fieldName, PublicOrNotInstance).GetValue(obj);
            }

            int length = nameParts.Length;
            for (int i = 0; i < length; i++)
            {
                if (obj == null)
                {
                    return null;
                }

                Type type = obj.GetType();
                FieldInfo info = type.GetField(nameParts[i]);
                if (info == null)
                {
                    return null;
                }
                obj = info.GetValue(obj);
            }
            return obj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue(object source, string name)
        {
            if (source == null)
            {
                return null;
            }

            Type type = source.GetType();
            while (type != null)
            {
                FieldInfo f = type.GetField(name, PublicOrNotInstance);
                if (f != null)
                {
                    return f.GetValue(source);
                }

                PropertyInfo p = type.GetProperty(name, PublicOrNotInstance | BindingFlags.IgnoreCase);
                if (p != null)
                {
                    return p.GetValue(source, null);
                }
                type = type.BaseType;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetValue(object source, string name, int index)
        {
            if (!(GetValue(source, name) is IEnumerable enumerable))
            {
                return null;
            }

            IEnumerator enm = enumerable.GetEnumerator();
            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext())
                {
                    return null;
                }
            }
            return enm.Current;
        }
    }
}
