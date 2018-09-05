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
            return HasItem() && StoredItem.AmountStack >= StoredItem.MaxStack;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasItem()
        {
            return !(StoredItem is null);
        }

        /// <summary> Is type of item same as slot type? (or slot can hold anything (Type of 0)) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TypeMatch(TPItem item)
        {
            return item.Type == Type || Type == 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool IsSlotOpposite(ITPItemSlot<TPItem> slot)
        {
            return slot is ITPEquipSlot<TPItem>;
        }

        /// <summary> Checks if types match and there is a place in stack </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CanHoldItem(TPItem item)
        {
            return TypeMatch(item) && !IsFull();
        }

        /// <summary> If targetSlot has item will check types match else check if CanHoldItem </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CanMoveItem(ITPItemSlot<TPItem> targetSlot)
        {
            return targetSlot.StoredItem != null
                   ? TypeMatch(targetSlot.StoredItem) && targetSlot.TypeMatch(StoredItem)
                   : targetSlot.CanHoldItem(StoredItem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool MoveItem(ITPItemSlot<TPItem> targetSlot)
        {
            if (CanMoveItem(targetSlot))
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
