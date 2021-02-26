/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

namespace TP.Framework
{
    public interface IInventory<TItemSlot, TEquipSlot, TItem>
        where TItemSlot : IItemSlot<TItem>
        where TEquipSlot : IEquipSlot<TItem>
        where TItem : IItem
    {
        TItemSlot[] ItemSlots { get; }
        TEquipSlot[] EquipSlots { get; }
        bool IsFull { get; }

        bool AddItem(TItem item);
        bool HasEmptySlot(int type = 0);
        bool HasItem(int itemID);
    }
}