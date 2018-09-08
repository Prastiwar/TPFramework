using System;
using System.Reflection;

namespace TP.Framework
{
    public static partial class TPExtensions
    {
        /// <summary> Gets string from enum value that has StringValueAttribute </summary>
        public static string GetStringValue(Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            StringValueAttribute att = (StringValueAttribute)fieldInfo.GetCustomAttribute(typeof(StringValueAttribute), false);
            return att != null ? att.StringValue : value.ToString();
        }
    }
}
