/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;

namespace TP.Framework
{
    public interface IItemSlot { }

    public interface IItemSlot<TItem> : IItemSlot
        where TItem : IItem
    {
        int Type { get; }
        TItem StoredItem { get; set; }
        Action OnItemChanged { get; set; }

        TItem SwitchItem(TItem item);
        bool MoveItem(IItemSlot<TItem> targetSlot);
        bool CanHoldItem(TItem item);
        bool TypeMatch(TItem item);
        bool HasItem();
        bool IsFull();
    }

    public interface IEquipSlot : IItemSlot { }

    public interface IEquipSlot<TItem> : IItemSlot<TItem>, IEquipSlot
        where TItem : IItem
    { }
}
