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
            Modifiers = Modifiers ?? new TPModifierList<TPModifier>(Recalculate);
            OnChanged = OnChanged ?? delegate { };
        }
    }

    [Serializable]
    public class TPAttribute<TModList, TModfifier> : ITPAttribute<TModfifier>
        where TModList : ITPModifierList<TModfifier>
        where TModfifier : ITPModifier
    {
        /// <summary> List collection of modifiers </summary>
        public virtual ITPModifierList<TModfifier> Modifiers { get; protected set; }

        /// <summary> Base value without any modifier </summary>
        public virtual float BaseValue { get; set; }

        /// <summary> Calculated value with all modifiers </summary>
        public virtual float Value { get; protected set; }

        /// <summary> Called after Recalculate </summary>
        public virtual Action<float> OnChanged { get; protected set; }

        public TPAttribute()
        {
            Modifiers = Modifiers ?? default(TModList);
            OnChanged = OnChanged ?? delegate { };
        }

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
            OnChanged(Value);
        }
    }
}
