/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    /* ---------------------------------------------------------------- Core ---------------------------------------------------------------- */

    public enum ModifierType
    {
        FlatIncrease,
        FlatMultiply,
        Percentage
    }

    public interface ITPModifier
    {
        float Value { get; }
        object Source { get; }
        ModifierType Type { get; }
        int Priority { get; }
    }

    public interface ITPAttribute<T> where T : ITPModifier
    {
        ITPModifierList<T> Modifiers { get; }
        float BaseValue { get; }
        float Value { get; }
        void Recalculate();
        Action<float> OnChanged { get; }
    }

    public interface ITPModifierList<T> where T : ITPModifier
    {
        ITPAttribute<T> Attribute { get; }
        T this[int index] { get; }
        float Count { get; }
        void Add(T modifier);
        bool Remove(T modifier);
        bool HasModifier(T modifier);
        int Compare(T mod1, T mod2);
        void Sort();
    }

    /* ---------------------------------------------------------------- Modifier ---------------------------------------------------------------- */

    [Serializable]
    public struct TPModifier : ITPModifier
    {
        public object Source { get; set; }
        public float Value { get; private set; }
        public ModifierType Type { get; private set; }
        public int Priority { get; private set; }

        public TPModifier(ModifierType type, float value, object source) : this(type, value, (int)type, source) { }
        public TPModifier(ModifierType tye, float value, int priority) : this(tye, value, priority, null) { }
        public TPModifier(ModifierType type, float value) : this(type, value, (int)type, null) { }
        public TPModifier(ModifierType type, float value, int priority, object source)
        {
            Priority = priority;
            Value = value;
            Type = type;
            Source = source;
        }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder(32);
            builder.Append("TPModifier { ");
            builder.Append("Type: ");
            builder.Append(Type);
            builder.Append("; Priority: ");
            builder.Append(Priority);
            builder.Append("; Value: ");
            builder.Append(Value);
            builder.Append("; Source: ");
            builder.Append(Source);
            builder.Append("; }");
            return builder.ToString();
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
    }

    /* ---------------------------------------------------------------- Modifier Container ---------------------------------------------------------------- */

    [Serializable]
    public class TPModifierList<T> : ITPModifierList<T> where T : ITPModifier
    {
        private readonly ReusableList<T> reusableModifiers;
        protected virtual List<T> Modifiers { get; set; }

        public float Count { get { return Modifiers.Count; } }
        public ITPAttribute<T> Attribute { get; protected set; }

        public T this[int index] { get { return Modifiers[index]; } }

        public TPModifierList(ITPAttribute<T> attribute, int capacity = 10)
        {
            Modifiers = new List<T>(capacity);
            reusableModifiers = new ReusableList<T>(4);
            Attribute = attribute;
        }

        /// <summary> Adds modifier and recalculates Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void Add(T modifier)
        {
            Modifiers.Add(modifier);
            Attribute.Recalculate();
        }

        /// <summary> If modifier exist - remove it and recalculate Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool Remove(T modifier)
        {
            if (Modifiers.Remove(modifier))
            {
                Attribute.Recalculate();
                return true;
            }
            return false;
        }

        /// <summary> Removes all modifiers, recalculates Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void RemoveModifiers()
        {
            Modifiers.Clear();
            Attribute.Recalculate();
        }

        /// <summary> Removes all modifiers from source, recalculates Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void RemoveModifiers(object source)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Modifiers[i].Source == source)
                {
                    Remove(Modifiers[i]);
                    i--;
                }
            }
            Attribute.Recalculate();
        }

        /// <summary> Get first modifier from source </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public T GetModifier(object source)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Modifiers[i].Source == source)
                {
                    return Modifiers[i];
                }
            }
            return default(T);
        }

        /// <summary> Get all modifiers from source </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public T[] GetModifiers(object source)
        {
            List<T> sourceModifiers = reusableModifiers.CleanList;

            for (int i = 0; i < Count; i++)
            {
                if (Modifiers[i].Source == source)
                    sourceModifiers.Add(Modifiers[i]);
            }
            return sourceModifiers.ToArray();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool HasModifier(T modifier)
        {
            return Modifiers.Contains(modifier);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool HasModifier(object source)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Modifiers[i].Source == source)
                    return true;
            }
            return false;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool ChangeModifier(T modifier, T newModifier)
        {
            int index = Modifiers.IndexOf(modifier);
            if (index >= 0)
            {
                Modifiers[index] = newModifier;
                return true;
            }
            return false;
        }

        /// <summary> Compare modifiers by their priority </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public int Compare(T modifier1, T modifier2)
        {
            if (modifier1.Priority > modifier2.Priority)
                return 1;
            else if (modifier1.Priority < modifier2.Priority)
                return -1;
            return 0;
        }

        /// <summary> Sort modifiers by their priority </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void Sort()
        {
            Modifiers.Sort(Compare);
        }
    }

    /* ---------------------------------------------------------------- Attribute ---------------------------------------------------------------- */

    /// <summary> Wrapper for TPAttribute<TPModifierList<TPModifier>, TPModifier> </summary>
    [Serializable]
    public class TPAttribute : TPAttribute<TPModifierList<TPModifier>, TPModifier> { }

    [Serializable]
    public class TPAttribute<TModList, TModfifier> : ITPAttribute<TModfifier>
        where TModList : ITPModifierList<TModfifier>
        where TModfifier : ITPModifier
    {
        /// <summary> List collection of modifiers </summary>
        //public virtual ITPModifierList<TPModifier> Modifiers { get; protected set; }
        public virtual ITPModifierList<TModfifier> Modifiers { get; protected set; }

        /// <summary> Base value without any modifier </summary>
        public virtual float BaseValue { get; set; }

        /// <summary> Calculated value with all modifiers </summary>
        public virtual float Value { get; protected set; }

        /// <summary> Called after Recalculate </summary>
        public virtual Action<float> OnChanged { get; protected set; }

        public TPAttribute()
        {
            //Modifiers = new TPModifierList<TPModifier>(this);
            OnChanged = delegate { };
        }

        /// <summary> Request recalculating Value with modifiers </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void Recalculate()
        {
            Value = BaseValue;
            Modifiers.Sort();

            for (int i = 0; i < Modifiers.Count; i++)
            {
                switch (Modifiers[i].Type)
                {
                    case ModifierType.FlatIncrease:
                        Value += Modifiers[i].Value;
                        break;

                    case ModifierType.FlatMultiply:
                        Value *= Modifiers[i].Value;
                        break;

                    case ModifierType.Percentage:
                        Value *= (1 + (Modifiers[i].Value > 1.0f ? (Modifiers[i].Value / 100) : Modifiers[i].Value));
                        break;
                }
            }
            OnChanged(Value);
        }
    }
}
