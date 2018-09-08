/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Text;

namespace TP.Framework
{
    [Serializable]
    public struct TPModifier : ITPModifier
    {
        public object Source { get; set; }
        public float Value { get; private set; }
        public ModifierType Type { get; private set; }
        public int Priority { get; private set; }

        public TPModifier(ModifierType type, float value, object source) : this(type, value, (int)type, source) { }
        public TPModifier(ModifierType type, float value, int priority) : this(type, value, priority, null) { }
        public TPModifier(ModifierType type, float value) : this(type, value, (int)type, null) { }
        public TPModifier(ModifierType type, float value, int priority, object source)
        {
            Priority = priority;
            Value = value;
            Type = type;
            Source = source;
        }

        public static bool operator ==(TPModifier c1, TPModifier c2)
        {
            return c1.Value == c2.Value && c1.Type == c2.Type && c1.Priority == c2.Priority && c1.Source == c2.Source;
        }

        public static bool operator !=(TPModifier c1, TPModifier c2)
        {
            return !(c1 == c2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
