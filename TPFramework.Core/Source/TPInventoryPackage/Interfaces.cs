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
        Action OnUsed { get; }
        Action OnMoved { get; }
        Action OnFailMoved { get; }
        Action OnEquipped { get; }
        Action OnUnEquipped { get; }
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
        ITPItemSlot OnSlot { get; }
    }

    public interface ITPItem : ITPItemCallbacks, ITPItemData
    {
        void Use();
        bool Stack(int count = 1);
    }

    public interface ITPItemSlot
    {
        int Type { get; }
        ITPItem StoredItem { get; }

        ITPItem SwitchItem(ITPItem item);
        bool MoveItem(ITPItemSlot targetSlot);
        bool CanHoldItem(ITPItem item);
        bool TypeMatch(ITPItem item);
        bool IsFull();
        bool IsEmpty();
    }

    public interface ITPEquipSlot : ITPItemSlot { }

    public interface ITPInventory
    {
        ITPItemSlot[] ItemSlots { get; }
        bool IsFull { get; }

        bool AddItem(ITPItem item);
        bool HasEmptySlot(int type = 0);
        bool HasItem(int itemID);
    }
}