/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TPFramework
{
    public enum ModifierType
    {
        FlatIncrease = 3,
        FlatMultiply = 2,
        Percentage = 1
    }

    /* ---------------------------------------------------------------- Modifier ---------------------------------------------------------------- */

    [Serializable]
    public struct TPModifier
    {
        public float Value;
        public object Source;

        [SerializeField] private ModifierType type;
        [SerializeField] private int priority;

        public ModifierType Type { get { return type; } }
        public int Priority { get { return priority; } }

        public TPModifier(ModifierType type, float value, object source) : this(type, value, (int)type, source) { }

        public TPModifier(ModifierType tye, float value, int priority) : this(tye, value, priority, null) { }

        public TPModifier(ModifierType type, float value) : this(type, value, (int)type, null) { }

        public TPModifier(ModifierType type, float value, int priority, object source)
        {
            this.type = type;
            this.priority = priority;
            Source = source;
            Value = value;
        }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("TPModifier { ");
            builder.Append("Type: ");
            builder.Append(type);
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
            return c1.Value != c2.Value || c1.Type != c2.Type || c1.Priority != c2.Priority || c1.Source != c2.Source;
        }

        public override bool Equals(object obj)
        {
            return (TPModifier)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /* ---------------------------------------------------------------- Attribute ---------------------------------------------------------------- */

    [Serializable]
    public class TPAttribute
    {
        [SerializeField] private float baseValue;
        [SerializeField] private List<TPModifier> modifiers = new List<TPModifier>(4);

        private ReadOnlyCollection<TPModifier> readModifiers;
        private Action<float> onChanged = delegate { };
        private readonly ReusableList<TPModifier> reusableModifiers = new ReusableList<TPModifier>(4);

        /// <summary> Delegate called after value has changed (on Recalculate()) </summary>
        public Action<float> OnChanged {
            get { return onChanged; }
            set { OnChanged = value; }
        }

        /// <summary> Base value without any modifier </summary>
        public float BaseValue {
            get { return baseValue; }
            set { baseValue = value; }
        }

        public ReadOnlyCollection<TPModifier> Modifiers {
            get {
                if (readModifiers == null)
                    readModifiers = modifiers.AsReadOnly();
                return readModifiers;
            }
        }

        /// <summary> Calculated value with all modifiers </summary>
        public float Value { get; private set; }

        public float ModifiersCount { get; private set; }

        /// <summary> Adds TPModifier to attribute, recalculates Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void AddModifier(TPModifier modifier)
        {
            modifiers.Add(modifier);
            Recalculate();
        }

        /// <summary> Removes TPModifier from attribute, recalculates Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool RemoveModifier(TPModifier modifier)
        {
            if (modifiers.Remove(modifier))
            {
                Recalculate();
                return true;
            }
            return false;
        }

        /// <summary> Removes all modifiers in attribute, recalculates Value </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void RemoveModifiers()
        {
            modifiers.Clear();
            Recalculate();
        }

        /// <summary> Removes all modifiers in attribute from source, recalculates Value </summary>
        /// <param name="source"> object declared in constructor of TPModifier </param>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void RemoveModifiers(object source)
        {
            for (int i = 0; i < ModifiersCount; i++)
            {
                if (modifiers[i].Source == source)
                {
                    modifiers.Remove(modifiers[i]);
                    i--;
                    ModifiersCount--;
                }
            }
            Recalculate();
        }

        /// <summary> Get first modifier in attribute from source </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public TPModifier GetModifier(object source)
        {
            for (int i = 0; i < ModifiersCount; i++)
            {
                if (modifiers[i].Source == source)
                {
                    return modifiers[i];
                }
            }
            Debug.LogError("Modifier was not found. source: " + source);
            return default(TPModifier);
        }

        /// <summary> Get all modifiers in attribute from source </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public TPModifier[] GetModifiers(object source)
        {
            List<TPModifier> sourceModifiers = reusableModifiers.CleanList;

            for (int i = 0; i < ModifiersCount; i++)
            {
                if (modifiers[i].Source == source)
                    sourceModifiers.Add(modifiers[i]);
            }
            return sourceModifiers.ToArray();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool HasModifier(TPModifier modifier)
        {
            return modifiers.Contains(modifier);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool HasModifier(object source)
        {
            for (int i = 0; i < ModifiersCount; i++)
            {
                if (modifiers[i].Source == source)
                    return true;
            }
            return false;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool ChangeModifier(TPModifier modifier, TPModifier newModifier)
        {
            for (int i = 0; i < ModifiersCount; i++)
            {
                if (modifiers[i] == modifier)
                {
                    modifiers[i] = newModifier;
                    return true;
                }
            }
            return false;
        }

        /// <summary> Request recalculating Value with modifiers </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void Recalculate()
        {
            Value = BaseValue;
            modifiers.Sort(CompareModifiers);
            ModifiersCount = modifiers.Count;

            for (int i = 0; i < ModifiersCount; i++)
            {
                switch (modifiers[i].Type)
                {
                    case ModifierType.FlatIncrease:
                        Value += modifiers[i].Value;
                        break;

                    case ModifierType.FlatMultiply:
                        Value *= modifiers[i].Value;
                        break;

                    case ModifierType.Percentage:
                        Value *= (1 + (modifiers[i].Value > 1.0f ? (modifiers[i].Value / 100) : modifiers[i].Value));
                        break;
                }
            }
            OnChanged(Value);
        }

        /// <summary> Compare modifiers by their priority </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        private int CompareModifiers(TPModifier modifier1, TPModifier modifier2)
        {
            if (modifier1.Priority > modifier2.Priority)
                return 1;
            else if (modifier1.Priority < modifier2.Priority)
                return -1;
            return 0;
        }
    }
}
