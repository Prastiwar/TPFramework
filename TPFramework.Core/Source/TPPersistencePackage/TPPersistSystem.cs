/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public abstract class TPPersistSystem<TInstance> : ITPPersistSystem
        where TInstance : TPPersistSystem<TInstance>
    {
        private static TInstance instance = (TInstance)Activator.CreateInstance(typeof(TInstance));
        private readonly BindingFlags findFieldFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        protected Type PersistantAttType { get { return typeof(PersistantAttribute); } }

        public HashSet<Type> SupportedTypes { get { return GetSupportedTypes(); } }

        /// <summary> Looks for fields in sources with PersistantAttribute and saves them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SaveSources(params object[] sources)
        {
            instance.SaveObjects(sources);
        }

        /// <summary> Looks for fields in source with PersistantAttribute and saves them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save<T>(T source)
        {
            instance.SaveObject(source);
        }

        /// <summary> Looks for fields in source with PersistantAttribute and loads them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Load<T>(T source)
        {
            return instance.LoadObject(source);
        }

        /// <summary> Looks for fields in sources with PersistantAttribute and loads them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadSources(params object[] sources)
        {
            instance.LoadObjects(sources);
        }

        /// <summary> Looks for fields in sources with PersistantAttribute and saves them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SaveObjects(params object[] sources)
        {
            int length = sources.Length;
            for (int i = 0; i < length; i++)
            {
                SaveObject(sources[i]);
            }
        }

        /// <summary> Looks for fields in sources with PersistantAttribute and loads them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LoadObjects(params object[] sources)
        {
            int length = sources.Length;
            for (int i = 0; i < length; i++)
            {
                LoadObject(sources[i]);
            }
        }

        /// <summary> Looks for fields in source with PersistantAttribute and saves them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SaveObject<T>(T source)
        {
            FieldInfo[] fields = source.GetType().GetFields(findFieldFlags);
            int length = fields.Length;
            for (int i = 0; i < length; i++)
            {
                if (!IsFieldPersistant(fields[i]))
                {
                    continue;
                }

                PersistantAttribute att = (PersistantAttribute)fields[i].GetCustomAttribute(PersistantAttType, false);
                object fieldValue = fields[i].GetValue(source);

                if (!CanSetValue(att.DefaultValue, fieldValue))
                {
                    continue;
                }
                SaveValue(att, fieldValue);
            }
        }

        /// <summary> Looks for fields in source with PersistantAttribute and loads them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T LoadObject<T>(T source)
        {
            FieldInfo[] fields = source.GetType().GetFields(findFieldFlags);
            int length = fields.Length;
            for (int i = 0; i < length; i++)
            {
                if (!IsFieldPersistant(fields[i]))
                {
                    continue;
                }

                PersistantAttribute att = (PersistantAttribute)fields[i].GetCustomAttribute(PersistantAttType, false);
                object fieldValue = fields[i].GetValue(source);

                if (!CanSetValue(att.DefaultValue, fieldValue))
                {
                    continue;
                }
                fields[i].SetValue(source, LoadValue(att, fieldValue.GetType()));
                //fields[i].SetValueDirect(__makeref(source), LoadValue(att, fieldValue.GetType())); // doesn't work in unity 2017.3
            }
            return source;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsTypeSupported(Type objType)
        {
            return SupportedTypes.Contains(objType);
        }

        /// <summary> Is field supported and has PersistantAttribute? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool IsFieldPersistant(FieldInfo field)
        {
            return IsTypeSupported(field.FieldType) && field.IsDefined(PersistantAttType, false);
        }

        /// <summary> Is field supported and has PersistantAttribute and default value is same type as object? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool CanSetValue(object defaultValue, object fieldValue)
        {
            if (fieldValue == null)
            {
                return false;
            }

            Type fieldValueType = fieldValue.GetType();
            if (IsTypeSupported(fieldValueType))
            {
                return defaultValue != null
                    ? MatchDefaultValueType(defaultValue, fieldValueType)
                    : true;
            }
            return false;
        }

        /// <summary> Is default value type same as object type? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool MatchDefaultValueType(object defaultValue, Type shouldType)
        {
            return defaultValue.GetType() == shouldType;
        }

        /// <summary> Called on Save() for field with PersistantAttribute </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract void SaveValue(PersistantAttribute attribute, object saveValue);

        /// <summary> Called on Load() for field with PersistantAttribute </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract object LoadValue(PersistantAttribute attribute, Type objectType);

        /// <summary> Returns Types that can be persistant </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract HashSet<Type> GetSupportedTypes();
    }

}
