/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    /// <summary> Wrapper for TPAttribute<TPModifierList<TPModifier>, TPModifier> </summary>
    [Serializable]
    public class TPAttribute : TPAttribute<TPModifierList<TPModifier>, TPModifier>
    {
        public TPAttribute()
        {
            if (Modifiers == null)
            {
                Modifiers = new TPModifierList<TPModifier>(Recalculate);
            }
        }
    }

    [Serializable]
    public class TPAttribute<TModList, TModfifier> : ITPAttribute<TModfifier>
        where TModList : ITPModifierList<TModfifier>
        where TModfifier : ITPModifier
    {
        /// <summary> Base value without any modifier </summary>
        public float BaseValue { get; set; }

        /// <summary> Calculated value with all modifiers </summary>
        public float Value { get; protected set; }

        /// <summary> List collection of modifiers </summary>
        public ITPModifierList<TModfifier> Modifiers { get; protected set; }

        /// <summary> Called after Recalculate </summary>
        public Action<float> OnChanged { get; protected set; }

        /// <summary> Request recalculating Value with modifiers </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Recalculate()
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
                        Value *= 1 + Modifiers[i].Value;
                        break;
                }
            }
            OnChanged?.Invoke(Value);
        }
    }
}
