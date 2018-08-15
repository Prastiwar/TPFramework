/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;

namespace TPFramework.Core
{
    public interface ITPItem
    {
        int ID { get; }
        int Type { get; }
        string Name { get; }
        string Description { get; }
        double Worth { get; }
        int AmountStack { get; }
        int MaxStack { get; }
        ITPModifier[] Modifiers { get; }
        ITPItemSlot OnSlot { get; }
        Action OnUsed { get; }
        Action OnFailMoved { get; }
        Action OnEquipped { get; }
        Action OnMoved { get; }
        Action<ITPItem> OnSwitched { get; }
    }

    public interface ITPItemSlot
    {
        int Type { get; }
        ITPItem HoldItem { get; }
        bool IsEquipSlot { get; }
        bool TryMoveItem(ITPItemSlot targetSlot);
    }
}