﻿/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace TP.Framework
{
    [Serializable]
    public struct AttributeModifier : IAttributeModifier
    {
        public object Source { get; set; }
        public float Value { get; private set; }
        public ModifierType Type { get; private set; }
        public int Priority { get; private set; }

        public AttributeModifier(ModifierType type, float value, object source) : this(type, value, (int)type, source) { }
        public AttributeModifier(ModifierType type, float value, int priority) : this(type, value, priority, null) { }
        public AttributeModifier(ModifierType type, float value) : this(type, value, (int)type, null) { }
        public AttributeModifier(ModifierType type, float value, int priority, object source)
        {
            Priority = priority;
            Value = value;
            Type = type;
            Source = source;
        }

        public static bool operator ==(AttributeModifier c1, AttributeModifier c2)
        {
            return c1.Value == c2.Value && c1.Type == c2.Type && c1.Priority == c2.Priority && c1.Source == c2.Source;
        }

        public static bool operator !=(AttributeModifier c1, AttributeModifier c2)
        {
            return !(c1 == c2);
        }

        public override bool Equals(object obj)
        {
            return obj is AttributeModifier mod ? mod == this : false;
        }

        public override int GetHashCode()
        {
            var hashCode = 328636640;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Source);
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + Priority.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(32);
            builder.Append("{ ");
            builder.Append($"\"Source\": { Source },");
            builder.Append($"\"Value\": { Value },");
            builder.Append($"\"Type\": { Type },");
            builder.Append($"\"Priority\": { Priority },");
            builder.Append(" }");
            return builder.ToString();
        }
    }
}
