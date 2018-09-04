/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    [Serializable]
    public class TPItemSlot : ITPItemSlot<TPItem>
    {
        private TPItem storedItem;
        private int type;

        public TPItem StoredItem {
            get { return storedItem; }
            protected set {
                storedItem?.OnUsed.Remove(ShouldRemove);
                storedItem = value;
                storedItem?.OnUsed.Add(ShouldRemove);
                OnItemChanged?.Invoke();
            }
        }

        public int Type { get { return type; } protected set { type = value; } }

        public Action OnItemChanged { get; set; }

        TPItem ITPItemSlot<TPItem>.StoredItem { get { return StoredItem; } set { StoredItem = value; } }

        private void ShouldRemove()
        {
            if (StoredItem.AmountStack <= 0)
            {
                StoredItem = null;
            }
        }

        public TPItemSlot(int type, TPItem storeItem = null)
        {
            Type = type;
            StoredItem = storeItem;
        }

        /// <summary> Holds given item and returns the old one </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TPItem SwitchItem(TPItem item)
        {
            TPItem returnItem = storedItem ?? null;
            StoredItem = item;
            return returnItem;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasItem()
        {
            return !(StoredItem is null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool UseItem()
        {
            return StoredItem != null ? StoredItem.Use() : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool StackItem(int count = 1)
        {
            return StoredItem != null ? StoredItem.Stack(count) : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull()
        {
            return StoredItem != null && StoredItem.AmountStack >= StoredItem.MaxStack;
        }

        /// <summary> Is type of item same as slot type? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TypeMatch(TPItem item)
        {
            return item.Type == Type || Type == 0;
        }

        /// <summary> Checks if given slot is opposite of this slot </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool IsSlotOpposite(ITPItemSlot<TPItem> slot)
        {
            return slot is ITPEquipSlot<TPItem>;
        }

        /// <summary> Checks for place in stack and type match </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CanHoldItem(TPItem item)
        {
            return !IsFull() && TypeMatch(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool IsEmpty()
        {
            return StoredItem == null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool MoveItem(ITPItemSlot<TPItem> targetSlot)
        {
            if (targetSlot.CanHoldItem(StoredItem))
            {
                StoredItem.OnMoved?.Invoke();
                StoredItem = targetSlot.SwitchItem(StoredItem);
                StoredItem?.OnMoved?.Invoke();
                return true;
            }
            StoredItem?.OnFailMoved?.Invoke();
            return false;
        }
    }
}
