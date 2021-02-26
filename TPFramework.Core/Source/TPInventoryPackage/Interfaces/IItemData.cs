/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

namespace TP.Framework
{
    public interface IItemData
    {
        int ID { get; }
        int Type { get; }
        string Name { get; }
        string Description { get; }
        double Worth { get; }
        int AmountStack { get; }
        int MaxStack { get; }
        float Weight { get; }
    }

    public interface IItemData<TModifiers> : IItemData
        where TModifiers : IAttributeModifier
    {
        TModifiers[] Modifiers { get; }
    }
}
