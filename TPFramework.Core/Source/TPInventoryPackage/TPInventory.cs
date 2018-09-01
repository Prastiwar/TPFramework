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
    [Serializable]
    public abstract class TPInventory<TItemSlot, TEquipSlot, TItem> : ITPInventory<TItemSlot, TEquipSlot, TItem>
        where TItemSlot : ITPItemSlot<TItem>
        where TEquipSlot : ITPEquipSlot<TItem>
        where TItem : ITPItem
    {
        private readonly Predicate<TItemSlot> isEmptyMatch = new Predicate<TItemSlot>(x => x.IsEmpty());
        private readonly Predicate<TEquipSlot> isEquipEmptyMatch = new Predicate<TEquipSlot>(x => x.IsEmpty());

        protected Dictionary<int, ITPItem> Items { get; set; }
        protected TItemSlot[] ItemSlots { get; set; }
        protected TEquipSlot[] EquipSlots { get; set; }

        public int ItemCount { get { return Items.Count; } }

        public virtual bool IsFull { get { return !HasEmptySlot(); } }
        public virtual int SlotCount { get { return ItemSlots.Length + EquipSlots.Length; } }
        public virtual int EmptySlotsCount { get { return ItemSlots.Count(isEmptyMatch) + EquipSlots.Count(isEquipEmptyMatch); } }

        TItemSlot[] ITPInventory<TItemSlot, TEquipSlot, TItem>.ItemSlots { get { return ItemSlots; } }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetItemSlots(TItemSlot[] slots)
        {
            ItemSlots = slots;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetEquipSlots(TEquipSlot[] slots)
        {
            EquipSlots = slots;
        }

        /// <summary> Finds first matches slot from ItemSlots (can return null) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TItemSlot FindItemSlot(Predicate<TItemSlot> match)
        {
            int index = ItemSlots.FindIndex(match);
            return index > -1 ? ItemSlots[index] : default(TItemSlot);
        }

        /// <summary> Finds first matches slot from EquipSlots (can return null) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TEquipSlot FindEquipSlot(Predicate<TEquipSlot> match)
        {
            int index = EquipSlots.FindIndex(match);
            return index > -1 ? EquipSlots[index] : default(TEquipSlot);
        }

        /// <summary> Has inventory a slot with type which is empty? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasEmptySlot(int type = 0)
        {
            return FindItemSlot(x => x.Type == type && x.IsEmpty()) != null
                || FindEquipSlot(x => x.Type == type && x.IsEmpty()) != null;
        }

        /// <summary> Does item exist in any of slot? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasItem(int itemID)
        {
            return Items.ContainsKey(itemID);
        }

        /// <summary> Returns false if inventory is full or item exist and can't be more stacked </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool AddItem(TItem item)
        {
            if (!HasItem(item.ID))
            {
                TItemSlot slot = FindItemSlot(x => x.CanHoldItem(item));
                if (slot == null)
                {
                    TEquipSlot equipSlot = FindEquipSlot(x => x.CanHoldItem(item));
                    return equipSlot != null ? AddItem(item, slot) : false;
                }
                return slot != null ? AddItem(item, slot) : false;
            }
            return Items[item.ID].Stack();
        }

        /// <summary> Returns false if slot doesn't match item or item exist and can't be more stacked </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool AddItem(TItem item, TItemSlot slot)
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
            return Items[item.ID].Stack();
        }
    }
}
