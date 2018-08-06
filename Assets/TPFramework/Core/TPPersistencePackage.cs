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
    public interface ITPPersistant
    {
        Type PersistantType { get; }
        HashSet<Type> SupportedTypes { get; }
        void SaveObject<T>(T source);
        void SaveObjects(params object[] objects);
        T LoadObject<T>(T source);
        void LoadObjects(params object[] objects);
        bool IsTypeSupported(Type type);
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class PersistantAttribute : Attribute
    {
        public readonly string Key;
        public readonly object DefaultValue;

        private PersistantAttribute(string persistantKey, object defaultValue)
        {
            Key = persistantKey;
            DefaultValue = defaultValue;
        }

        public PersistantAttribute(string persistantKey, int defaultValue) : this(persistantKey, defaultValue as object) { }
        public PersistantAttribute(string persistantKey, string defaultValue) : this(persistantKey, defaultValue as object) { }
        public PersistantAttribute(string persistantKey, float defaultValue) : this(persistantKey, defaultValue as object) { }
        public PersistantAttribute(string persistantKey, bool defaultValue) : this(persistantKey, defaultValue as object) { }
        public PersistantAttribute(string persistantKey) : this(persistantKey, null) { }
    }

    public abstract class TPPersistant : ITPPersistant
    {
        public Type PersistantType { get { return typeof(PersistantAttribute); } }
        public HashSet<Type> SupportedTypes { get { return GetSupportedTypes(); } }
     
        protected static TPPersistant Instance { get;  set; }
        
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Save(params object[] sources) { Instance.SaveObjects(sources); }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static void Save<T>(T source) { Instance.SaveObject(source); }
        
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public static T Load<T>(T source) { return Instance.LoadObject(source); }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        protected abstract void SaveValue(PersistantAttribute attribute, object saveValue);

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        protected abstract object LoadValue(PersistantAttribute attribute, object objectValue, bool loadDefault);

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        protected abstract HashSet<Type> GetSupportedTypes();

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SaveObjects(params object[] sources)
        {
            int length = sources.Length;
            for (int i = 0; i < length; i++)
            {
                SaveObject(sources[i]);
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void LoadObjects(params object[] sources)
        {
            int length = sources.Length;
            for (int i = 0; i < length; i++)
            {
                LoadObject(sources[i]);
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void SaveObject<T>(T source)
        {
            FieldInfo[] fields = source.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            int length = fields.Length;
            for (int i = 0; i < length; i++)
            {
                if (!IsFieldValid(fields[i]))
                    continue;

                PersistantAttribute att = (PersistantAttribute)fields[i].GetCustomAttributes(PersistantType, false)[0];                
                object fieldValue = fields[i].GetValue(source);

                if (!IsValid(att.DefaultValue, fieldValue))
                    continue;

                SaveValue(att, fieldValue);
            }
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public T LoadObject<T>(T source)
        {
            FieldInfo[] fields = source.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            int length = fields.Length;
            for (int i = 0; i < length; i++)
            {
                if (!IsFieldValid(fields[i]))
                    continue;

                PersistantAttribute att = (PersistantAttribute)fields[i].GetCustomAttributes(PersistantType, false)[0];
                object fieldValue = fields[i].GetValue(source);

                if (!IsValid(att.DefaultValue, fieldValue))
                    continue;

                bool hasDefault = att.DefaultValue != null;

#if NET_2_0 || NET_2_0_SUBSET
                fields[i].SetValue(source, LoadValue(att, fieldValue, hasDefault));
#else
                fields[i].SetValueDirect(__makeref(source), LoadValue(att, fieldValue, hasDefault));
#endif
            }
            return source;
        }
        
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        protected bool IsFieldValid(FieldInfo field)
        {
            if (!IsTypeSupported(field.FieldType) || !field.IsDefined(PersistantType, false))
                return false;
            return true;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool IsTypeSupported(Type objType)
        {
            return SupportedTypes.Contains(objType);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        protected bool IsDefaultValueValid(object defaultValue, Type shouldType)
        {
            return defaultValue.GetType() == shouldType;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
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
