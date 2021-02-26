/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TP.Framework
{
    public interface IModifiableAttribute<T> where T : IAttributeModifier
    {
        IAttributeModifierList<T> Modifiers { get; }
        float BaseValue { get; }
        float Value { get; }
        void Recalculate();
        Action<float> OnChanged { get; }
    }
}
