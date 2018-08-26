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
    public class TPItemSlot : ITPItemSlot
    {
        private ITPItem storedItem;

        protected ITPItem StoredItem {
            get { return storedItem; }
            set {
                storedItem?.OnUsed.Remove(ShouldRemove);
                storedItem = value;
                storedItem?.OnUsed.Add(ShouldRemove);
            }
        }

        public int Type { get; protected set; }

        ITPItem ITPItemSlot.StoredItem {
            get { return StoredItem; }
            set { StoredItem = value; }
        }

        private void ShouldRemove()
        {
            if (StoredItem.AmountStack <= 0)
            {
                StoredItem = null;
            }
        }

        /// <summary> Holds given item and returns the old one </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITPItem SwitchItem(ITPItem item)
        {
            ITPItem returnItem = storedItem ?? null;
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
            return StoredItem != null && StoredItem.AmountStack >= StoredItem.MaxStack;
        }

        /// <summary> Is type of item same as slot type? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TypeMatch(ITPItem item)
        {
            return item.Type == Type || Type == 0;
        }

        /// <summary> Checks if given slot is opposite of this slot </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool IsSlotOpposite(ITPItemSlot slot)
        {
            return slot is ITPEquipSlot;
        }

        /// <summary> Checks for place in stack and type match </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CanHoldItem(ITPItem item)
        {
            return !IsFull() && TypeMatch(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool IsEmpty()
        {
            return StoredItem == null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool MoveItem(ITPItemSlot targetSlot)
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
