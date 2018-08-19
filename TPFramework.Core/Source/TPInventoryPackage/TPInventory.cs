/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public abstract class TPInventory : ITPInventory
    {
        public virtual Dictionary<int, ITPItem> Items { get; protected set; }
        public virtual ITPItemSlot[] ItemSlots { get; protected set; }
        public virtual ITPEquipSlot[] EquipSlots { get; protected set; }
        public virtual bool IsFull { get { return HasEmptySlot(); } }

        /// <summary> Finds first matches slot from ItemSlots OR EquipSlots (can return null) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITPItemSlot FindAnySlot(Predicate<ITPItemSlot> match)
        {
            ITPItemSlot slot = FindItemSlot(match);
            return slot ?? FindEquipSlot(match);
        }

        /// <summary> Finds first matches slot from ItemSlots (can return null) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITPItemSlot FindItemSlot(Predicate<ITPItemSlot> match)
        {
            int index = ItemSlots.FindIndex(match);
            return index > -1 ? ItemSlots[index] : null;
        }

        /// <summary> Finds first matches slot from EquipSlots (can return null) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITPItemSlot FindEquipSlot(Predicate<ITPItemSlot> match)
        {
            int index = EquipSlots.FindIndex(match);
            return index > -1 ? EquipSlots[index] : null;
        }

        /// <summary> Returns false if inventory is full or item exist and can't be more stacked </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool AddItem(ITPItem item)
        {
            ITPItemSlot slot = FindAnySlot(x => x.CanHoldItem(item));
            return slot != null ? AddItem(item, slot) : false;
        }

        /// <summary> Returns false if slot doesn't match item or item exist and can't be more stacked </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool AddItem(ITPItem item, ITPItemSlot slot)
        {
            if (!HasItem(item.ID))
            {
                if (slot.CanHoldItem(item))
                {
                    Items[item.ID] = item;
                    slot.SwitchItem(item);
                    return true;
                }
                return false;
            }
            return StackItem(item.ID);
        }

        /// <summary> Has inventory a slot with type which is empty? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool HasEmptySlot(int type = 0)
        {
            int length = ItemSlots.Length;
            for (int i = 0; i < length; i++)
            {
                if (ItemSlots[i].Type == type && ItemSlots[i].IsEmpty())
                {
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool StackItem(int itemID)
        {
            return Items[itemID].Stack();
        }

        /// <summary> Does item exist in any of slot? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool HasItem(int itemID)
        {
            return Items.ContainsKey(itemID);
        }
    }
}
