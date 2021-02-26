/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

namespace TP.Framework
{
    public interface IAttributeModifierList<T> where T : IAttributeModifier
    {
        T this[int index] { get; }
        int Count { get; }
        void Add(T modifier);

        bool Remove(T modifier);
        void RemoveModifiers();
        void RemoveModifiers(object source);

        bool ChangeModifier(T modifier, T newModifier);
        bool HasModifier(T modifier);
        int Compare(T mod1, T mod2);
        void Sort();
    }
}
