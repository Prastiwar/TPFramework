﻿/**
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
        ITPModifierContainer<T> Modifiers { get; }
        float BaseValue { get; }
        float Value { get; }
        void Recalculate();
        Action<float> OnChanged { get; }
    }

    public interface ITPModifierContainer<T> where T : ITPModifier
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
    public struct TPModifierContainer<T> : ITPModifierContainer<T> where T : ITPModifier
    {
        private readonly List<T> modifiers;
        private readonly ReusableList<T> reusableModifiers;

        public float Count { get; private set; }
        public ITPAttribute<T> Attribute { get; private set; }
        public T this[int index] { get { return modifiers[index]; } }

        public TPModifierContainer(ITPAttribute<T> attribute, int capacity = 10)
        {
            modifiers = new List<T>(capacity);
            reusableModifiers = new ReusableList<T>(4);
            Attribute = attribute;
            Count = 0;
        }

        /// <summary> Adds modifier and recalculates Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void Add(T modifier)
        {
            modifiers.Add(modifier);
            Count++;
            Attribute.Recalculate();
        }

        /// <summary> If modifier exist - remove it and recalculate Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool Remove(T modifier)
        {
            if (modifiers.Remove(modifier))
            {
                Count--;
                Attribute.Recalculate();
                return true;
            }
            return false;
        }

        /// <summary> Removes all modifiers, recalculates Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void RemoveModifiers()
        {
            modifiers.Clear();
            Attribute.Recalculate();
        }

        /// <summary> Removes all modifiers from source, recalculates Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void RemoveModifiers(object source)
        {
            for (int i = 0; i < Count; i++)
            {
                if (modifiers[i].Source == source)
                {
                    Remove(modifiers[i]);
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
                if (modifiers[i].Source == source)
                {
                    return modifiers[i];
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
                if (modifiers[i].Source == source)
                    sourceModifiers.Add(modifiers[i]);
            }
            return sourceModifiers.ToArray();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool HasModifier(T modifier)
        {
            return modifiers.Contains(modifier);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool HasModifier(object source)
        {
            for (int i = 0; i < Count; i++)
            {
                if (modifiers[i].Source == source)
                    return true;
            }
            return false;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool ChangeModifier(T modifier, T newModifier)
        {
            int index = modifiers.IndexOf(modifier);
            if (index >= 0)
            {
                modifiers[index] = newModifier;
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
            modifiers.Sort(Compare);
        }
    }

    /* ---------------------------------------------------------------- Attribute ---------------------------------------------------------------- */

    [Serializable]
    public class TPAttribute : ITPAttribute<TPModifier>
    {
        /// <summary> List collection of modifiers </summary>
        public ITPModifierContainer<TPModifier> Modifiers { get; private set; }

        /// <summary> Base value without any modifier </summary>
        public float BaseValue { get; set; }

        /// <summary> Calculated value with all modifiers </summary>
        public float Value { get; private set; }

        /// <summary> Called after Recalculate </summary>
        public Action<float> OnChanged { get; private set; }

        public TPAttribute()
        {
            Modifiers = new TPModifierContainer<TPModifier>(this);
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
