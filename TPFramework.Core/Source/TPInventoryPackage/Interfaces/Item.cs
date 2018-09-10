/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;

namespace TP.Framework
{
    public interface ITPItemCallbacks
    {
        Action OnUsed { get; set; }
        Action OnFailUsed { get; set; }

        Action OnMoved { get; set; }
        Action OnFailMoved { get; set; }

        Action OnEquipped { get; set; }
        Action OnUnEquipped { get; set; }
    }

    public interface ITPItemData
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

    public interface ITPItemData<TModifiers> : ITPItemData
        where TModifiers : ITPModifier
    {
        TModifiers[] Modifiers { get; }
    }

    public interface ITPItem : ITPItemCallbacks, ITPItemData
    {
        bool Use();
        bool Stack(int count = 1);
    }

    public interface ITPItem<TModifiers> : ITPItem, ITPItemData<TModifiers>
        where TModifiers : ITPModifier
    { }
}
