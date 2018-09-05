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
        private readonly Predicate<TItemSlot> isEmptyMatch = new Predicate<TItemSlot>(x => !x.HasItem());
        private readonly Predicate<TEquipSlot> isEquipEmptyMatch = new Predicate<TEquipSlot>(x => !x.HasItem());

        private TItemSlot[] itemSlots;
        private TEquipSlot[] equipSlots;

        protected Dictionary<int, TItem> Items { get; set; }
        protected TItemSlot[] ItemSlots { get { return itemSlots; } set { itemSlots = value; } }
        protected TEquipSlot[] EquipSlots { get { return equipSlots; } set { equipSlots = value; } }

        public int ItemCount { get { return Items.Count; } }

        public virtual bool IsFull { get { return !HasEmptySlot(); } }
        public virtual int SlotCount { get { return ItemSlots.Length + EquipSlots.Length; } }
        public virtual int EmptySlotsCount { get { return ItemSlots.Count(isEmptyMatch) + EquipSlots.Count(isEquipEmptyMatch); } }

        TItemSlot[] ITPInventory<TItemSlot, TEquipSlot, TItem>.ItemSlots { get { return ItemSlots; } }
        TEquipSlot[] ITPInventory<TItemSlot, TEquipSlot, TItem>.EquipSlots { get { return EquipSlots; } }

        public TPInventory()
        {
            Items = new Dictionary<int, TItem>();
        }

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

        public ITPItemSlot<TItem> FindAnySlot(Predicate<ITPItemSlot<TItem>> match)
        {
            ITPItemSlot<TItem> slot = FindItemSlot(match);
            return slot ?? FindEquipSlot(match);
        }

        /// <summary> Finds first matches slot from ItemSlots (can return null) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TItemSlot FindItemSlot(Predicate<ITPItemSlot<TItem>> match)
        {
            int index = (ItemSlots as ITPItemSlot<TItem>[]).FindIndex(match);
            return index > -1 ? ItemSlots[index] : default(TItemSlot);
        }

        /// <summary> Finds first matches slot from EquipSlots (can return null) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TEquipSlot FindEquipSlot(Predicate<ITPItemSlot<TItem>> match)
        {
            int index = (EquipSlots as ITPItemSlot<TItem>[]).FindIndex(match);
            return index > -1 ? EquipSlots[index] : default(TEquipSlot);
        }

        /// <summary> Has inventory a slot with type which is empty? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasEmptySlot(int type = 0)
        {
            return FindItemSlot(x => x.Type == type && !x.HasItem()) != null
                || FindEquipSlot(x => x.Type == type && !x.HasItem()) != null;
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
                ITPItemSlot<TItem> slot = FindAnySlot(x => x.CanHoldItem(item));
                return slot != null ? AddItem(item, slot) : false;
            }
            return Items[item.ID].Stack();
        }

        /// <summary> Returns false if slot doesn't match item or item exist and can't be more stacked </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool AddItem(TItem item, ITPItemSlot<TItem> slot)
        {
            if (!HasItem(item.ID))
            {
                if (slot.CanHoldItem(item))
                {
                    if(item.AmountStack == 0)
                    {
                        item.Stack();
                    }
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
