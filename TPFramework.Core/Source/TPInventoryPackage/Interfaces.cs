/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;

namespace TPFramework.Core
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
        ITPModifier[] Modifiers { get; }
    }

    public interface ITPItem : ITPItemCallbacks, ITPItemData
    {
        bool Use();
        bool Stack(int count = 1);
    }

    public interface ITPItemSlot<TItem>
        where TItem : ITPItem
    {
        int Type { get; }
        TItem StoredItem { get; set; }

        TItem SwitchItem(TItem item);
        bool MoveItem(ITPItemSlot<TItem> targetSlot);
        bool CanHoldItem(TItem item);
        bool TypeMatch(TItem item);
        bool IsFull();
        bool IsEmpty();
    }

    public interface ITPEquipSlot<TItem> : ITPItemSlot<TItem> where TItem : ITPItem { }

    public interface ITPInventory<TItemSlot, TEquipSlot, TItem>
        where TItemSlot : ITPItemSlot<TItem>
        where TEquipSlot : ITPEquipSlot<TItem>
        where TItem : ITPItem
    {
        TItemSlot[] ItemSlots { get; }
        bool IsFull { get; }

        bool AddItem(TItem item);
        bool HasEmptySlot(int type = 0);
        bool HasItem(int itemID);
    }
}