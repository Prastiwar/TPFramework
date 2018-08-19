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
        public int Type { get; protected set; }
        public ITPItem HoldItem { get; protected set; }

        /// <summary> Holds given item and returns the old one </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ITPItem SwitchItem(ITPItem item)
        {
            ITPItem returnItem = HoldItem ?? null;
            HoldItem = item;
            return returnItem;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool MoveItem(ITPItemSlot targetSlot)
        {
            if (targetSlot.CanHoldItem(HoldItem))
            {
                HoldItem.OnMoved?.Invoke();
                HoldItem = targetSlot.SwitchItem(HoldItem);
                HoldItem?.OnMoved?.Invoke();
                return true;
            }
            HoldItem?.OnFailMoved?.Invoke();
            return false;
        }

        ///// <summary> Returns opposite Slot (ItemSlot-EquipSlot) that can hold HoldItem </summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public ITPItemSlot FindEmptyOppositeSlot(ITPItemSlot[] slots)
        //{
        //    int length = slots.Length;
        //    for (int i = 0; i < length; i++)
        //    {
        //        if (slots[i].CanHoldItem(HoldItem) && IsSlotOpposite(slots[i]))
        //        {
        //            return slots[i];
        //        }
        //    }
        //    return null;
        //}

        ///// <summary> If there is slot matching to HoldItem with opposite Slot (ItemSlot-EquipSlot), call TryMoveItem(..) </summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public virtual bool TryMoveItemToEmptyOppositeSlot(ITPItemSlot[] slots)
        //{
        //    if (HoldItem == null)
        //        return false;

        //    ITPItemSlot slot = FindEmptyOppositeSlot(slots);
        //    if (slot != null)
        //    {
        //        return TryMoveItem(slot);
        //    }
        //    HoldItem.OnFailMoved?.Invoke();
        //    return false;
        //}

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

        /// <summary> Is type of item same as slot type? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool TypeMatch(ITPItem item)
        {
            return item.Type == Type || Type == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool IsFull()
        {
            return HoldItem != null && HoldItem.AmountStack >= HoldItem.MaxStack;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool IsEmpty()
        { 
            return HoldItem == null;
        }
    }
}
