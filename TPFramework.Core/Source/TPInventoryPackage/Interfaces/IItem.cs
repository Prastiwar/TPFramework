/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

namespace TP.Framework
{
    public interface IItem : IItemCallbacks, IItemData
    {
        bool Use();
        bool Stack(int count = 1);
    }

    public interface IItem<TModifiers> : IItem, IItemData<TModifiers>
        where TModifiers : IAttributeModifier
    { }
}
