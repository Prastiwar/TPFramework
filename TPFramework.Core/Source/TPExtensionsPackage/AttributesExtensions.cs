using System;
using System.Reflection;

namespace TPFramework.Core
{
    public static partial class TPExtensions
    {
        public static string GetStringValue(Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            StringValueAttribute att = (StringValueAttribute)fieldInfo.GetCustomAttribute(typeof(StringValueAttribute), false);
            return att != null ? att.StringValue : value.ToString();
        }
    }
}
