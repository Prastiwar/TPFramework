/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

namespace TP.Framework
{
    public interface ITPInventory<TItemSlot, TEquipSlot, TItem>
        where TItemSlot : ITPItemSlot<TItem>
        where TEquipSlot : ITPEquipSlot<TItem>
        where TItem : ITPItem
    {
        TItemSlot[] ItemSlots { get; }
        TEquipSlot[] EquipSlots { get; }
        bool IsFull { get; }

        bool AddItem(TItem item);
        bool HasEmptySlot(int type = 0);
        bool HasItem(int itemID);
    }
}