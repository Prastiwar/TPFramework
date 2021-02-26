/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TP.Framework
{
    [Serializable]
    public abstract class Inventory<TItemSlot, TEquipSlot, TItem> : IInventory<TItemSlot, TEquipSlot, TItem>
        where TItemSlot : IItemSlot<TItem>
        where TEquipSlot : IEquipSlot<TItem>
        where TItem : IItem
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

        TItemSlot[] IInventory<TItemSlot, TEquipSlot, TItem>.ItemSlots { get { return ItemSlots; } }
        TEquipSlot[] IInventory<TItemSlot, TEquipSlot, TItem>.EquipSlots { get { return EquipSlots; } }

        public Inventory()
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

        public IItemSlot<TItem> FindAnySlot(Predicate<IItemSlot<TItem>> match)
        {
            IItemSlot<TItem> slot = FindItemSlot(match);
            return slot ?? FindEquipSlot(match);
        }

        /// <summary> Finds first matches slot from ItemSlots (can return null) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TItemSlot FindItemSlot(Predicate<IItemSlot<TItem>> match)
        {
            int index = (ItemSlots as IItemSlot<TItem>[]).FindIndex(match);
            return index > -1 ? ItemSlots[index] : default(TItemSlot);
        }

        /// <summary> Finds first matches slot from EquipSlots (can return null) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TEquipSlot FindEquipSlot(Predicate<IItemSlot<TItem>> match)
        {
            int index = (EquipSlots as IItemSlot<TItem>[]).FindIndex(match);
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
                IItemSlot<TItem> slot = FindAnySlot(x => x.CanHoldItem(item));
                return slot != null ? AddItem(item, slot) : false;
            }
            return Items[item.ID].Stack();
        }

        /// <summary> Returns false if slot doesn't match item or item exist and can't be more stacked </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool AddItem(TItem item, IItemSlot<TItem> slot)
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
