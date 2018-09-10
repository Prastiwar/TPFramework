/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;

namespace TP.Framework
{
    public interface ITPItemSlot { }

    public interface ITPItemSlot<TItem> : ITPItemSlot
        where TItem : ITPItem
    {
        int Type { get; }
        TItem StoredItem { get; set; }
        Action OnItemChanged { get; set; }

        TItem SwitchItem(TItem item);
        bool MoveItem(ITPItemSlot<TItem> targetSlot);
        bool CanHoldItem(TItem item);
        bool TypeMatch(TItem item);
        bool HasItem();
        bool IsFull();
    }

    public interface ITPEquipSlot : ITPItemSlot { }

    public interface ITPEquipSlot<TItem> : ITPItemSlot<TItem>, ITPEquipSlot
        where TItem : ITPItem
    { }
}
