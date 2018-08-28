using System;

namespace TPFramework.Core
{
    [Serializable]
    public class PersistDefaultValueTypeMismatch : Exception
    {
        public PersistDefaultValueTypeMismatch(Type type, Type shouldType)
            : base(" Default value doesnt match field type! " + type + " should be " + shouldType)
        { }
    }

    [Serializable]
    public class PersistNotSupportedType : Exception
    {
        public PersistNotSupportedType(Type type) : base("Type " + type + " is not supported to be persistant!") { }
    }
}
