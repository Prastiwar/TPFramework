/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TP.Framework
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class PersistantAttribute : Attribute
    {
        public string Key { get; }
        public object DefaultValue { get; }

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
}
