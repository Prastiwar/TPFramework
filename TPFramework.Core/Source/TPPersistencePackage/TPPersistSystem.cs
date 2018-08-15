﻿/**
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
    public abstract class TPPersistSystem : ITPPersistSystem
    {
        public Type PersistantAttType { get { return typeof(PersistantAttribute); } }
        public HashSet<Type> SupportedTypes { get { return GetSupportedTypes(); } }

        /// <summary> Instance of script that static Save/Load methods should get Values from </summary>
        protected static TPPersistSystem Instance { get; set; }

        /// <summary> Called on Save() for field with PersistantAttribute </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract void SaveValue(PersistantAttribute attribute, object saveValue);

        /// <summary> Called on Load() for field with PersistantAttribute </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract object LoadValue(PersistantAttribute attribute, object objectValue);

        /// <summary> Returns Types that can be persistant </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract HashSet<Type> GetSupportedTypes();

        /// <summary> Looks for fields in sources with PersistantAttribute and saves them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SaveSources(params object[] sources)
        {
            Instance.SaveObjects(sources);
        }

        /// <summary> Looks for fields in source with PersistantAttribute and saves them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save<T>(T source)
        {
            Instance.SaveObject(source);
        }

        /// <summary> Looks for fields in source with PersistantAttribute and loads them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Load<T>(T source)
        {
            return Instance.LoadObject(source);
        }

        /// <summary> Looks for fields in sources with PersistantAttribute and loads them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoadSources(params object[] sources)
        {
            Instance.LoadObjects(sources);
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
            FieldInfo[] fields = source.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            int length = fields.Length;
            for (int i = 0; i < length; i++)
            {
                if (!IsFieldValid(fields[i]))
                    continue;

                PersistantAttribute att = (PersistantAttribute)fields[i].GetCustomAttributes(PersistantAttType, false)[0];
                object fieldValue = fields[i].GetValue(source);

                if (!IsValid(att.DefaultValue, fieldValue))
                    continue;

                SaveValue(att, fieldValue);
            }
        }

        /// <summary> Looks for fields in source with PersistantAttribute and loads them </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T LoadObject<T>(T source)
        {
            FieldInfo[] fields = source.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            int length = fields.Length;
            for (int i = 0; i < length; i++)
            {
                if (!IsFieldValid(fields[i]))
                    continue;

                PersistantAttribute att = (PersistantAttribute)fields[i].GetCustomAttributes(PersistantAttType, false)[0];
                object fieldValue = fields[i].GetValue(source);

                if (!IsValid(att.DefaultValue, fieldValue))
                    continue;

                fields[i].SetValueDirect(__makeref(source), LoadValue(att, fieldValue));
            }
            return source;
        }

        /// <summary> Is field supported and has PersistantAttribute? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool IsFieldValid(FieldInfo field)
        {
            if (!IsTypeSupported(field.FieldType) || !field.IsDefined(PersistantAttType, false))
                return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsTypeSupported(Type objType)
        {
            return SupportedTypes.Contains(objType);
        }

        /// <summary> Is default value type same as object type? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool IsDefaultValueValid(object defaultValue, Type shouldType)
        {
            return defaultValue.GetType() == shouldType;
        }

        /// <summary> Is field supported and has PersistantAttribute and default value is same type as object? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool IsValid(object defaultValue, object fieldValue)
        {
            Type fieldType = fieldValue.GetType();
            if (IsTypeSupported(fieldType))
            {
                if (defaultValue != null)
                {
                    return IsDefaultValueValid(defaultValue, fieldType);
                }
                return true;
            }
            return false;
        }
    }

}