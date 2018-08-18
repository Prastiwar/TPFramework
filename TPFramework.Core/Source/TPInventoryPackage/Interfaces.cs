/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;

namespace TPFramework.Core
{
    public interface IItemCallbacks
    {
        Action OnUsed { get; }
        Action OnMoved { get; }
        Action OnFailMoved { get; }
        Action OnEquipped { get; }
        Action OnUnEquipped { get; }
    }

    public interface ITPItem : IItemCallbacks
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
    }

    public interface ITPItemSlot
    {
        int Type { get; }
        ITPItem HoldItem { get; }

        bool TryMoveItem(ITPItemSlot targetSlot);
        bool CanHoldItem(ITPItem item);
        ITPItem SwitchItem(ITPItem item);
    }

    public interface ITPEquipSlot : ITPItemSlot
    {
    }
}