/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Runtime.CompilerServices;

namespace TP.Framework
{
    /// <summary> Wrapper for TPAttribute<TPModifierList<TPModifier>, TPModifier> </summary>
    [Serializable]
    public class ModifiableAttribute : ModifiableAttribute<AttributeModifierList<AttributeModifier>, AttributeModifier>
    {
        public ModifiableAttribute()
        {
            if (Modifiers == null)
            {
                Modifiers = new AttributeModifierList<AttributeModifier>(Recalculate);
            }
        }

        public ModifiableAttribute(int baseValue) : this()
        {
            BaseValue = baseValue;
            Value = BaseValue;
        }
    }

    [Serializable]
    public class ModifiableAttribute<TModList, TModfifier> : IModifiableAttribute<TModfifier>
        where TModList : IAttributeModifierList<TModfifier>
        where TModfifier : IAttributeModifier
    {
        private float baseValue;

        /// <summary> Calculated value with all modifiers </summary>
        public float Value { get; protected set; }

        /// <summary> List collection of modifiers </summary>
        public TModList Modifiers { get; protected set; }

        /// <summary> Called after Recalculate </summary>
        public Action<float> OnChanged { get; protected set; }

        /// <summary> Base value without any modifier </summary>
        public float BaseValue {
            get { return baseValue; }
            set {
                baseValue = value;
                Recalculate();
            }
        }

        IAttributeModifierList<TModfifier> IModifiableAttribute<TModfifier>.Modifiers { get { return Modifiers; } }

        /// <summary> Request recalculating Value with modifiers </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Recalculate()
        {
            Value = BaseValue;
            Modifiers.Sort();

            int length = Modifiers.Count;
            for (int i = 0; i < length; i++)
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
                        Value *= 1 + Modifiers[i].Value;
                        break;
                }
            }
            OnChanged?.Invoke(Value);
        }
    }
}
