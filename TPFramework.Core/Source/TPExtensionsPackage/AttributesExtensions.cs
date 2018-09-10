/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TP.Framework
{
    public static partial class TPExtensions
    {
        /// <summary> Gets string from enum value that has StringValueAttribute </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetStringValue(Enum value)
        {
            Type enumType = value.GetType();
            string valueString = value.ToString();
            FieldInfo fieldInfo = enumType.GetField(valueString);
            StringValueAttribute att = fieldInfo.GetSingleCustomAttribute<StringValueAttribute>();
            return att != null ? att.StringValue : valueString;
        }
    }
}
