/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TPFramework.Core
{
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
}
