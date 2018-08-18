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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool TryMoveItem(ITPItemSlot targetSlot)
        {
            if (targetSlot.CanHoldItem(HoldItem))
            {
                HoldItem = targetSlot.SwitchItem(HoldItem);
                HoldItem?.OnMoved?.Invoke();
                return true;
            }
            HoldItem?.OnFailMoved?.Invoke();
            return false;
        }

        /// <summary> Holds given item and returns the old one </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual ITPItem SwitchItem(ITPItem item)
        {
            ITPItem returnItem = HoldItem ?? null;
            HoldItem = item;
            HoldItem?.OnMoved?.Invoke();
            return returnItem;
        }

        /// <summary> If there is slot matching to HoldItem with opposite Slot (ItemSlot-EquipSlot), call TryMoveItem(..) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool TryMoveItemToFreeOpposite(ITPItemSlot[] slots)
        {
            if (HoldItem == null)
                return false;

            ITPItemSlot slot = FindFreeOppositeSlot(slots);
            if (slot != null)
            {
                return TryMoveItem(slot);
            }
            HoldItem?.OnFailMoved?.Invoke();
            return false;
        }

        /// <summary> Returns opposite Slot (ItemSlot-EquipSlot) that can hold HoldItem </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual ITPItemSlot FindFreeOppositeSlot(ITPItemSlot[] slots)
        {
            int length = slots.Length;
            for (int i = 0; i < length; i++)
            {
                if (slots[i].CanHoldItem(HoldItem) && slots[i] is ITPEquipSlot)
                {
                    return slots[i];
                }
            }
            return null;
        }

        /// <summary> Checks for place in stack and type match </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanHoldItem(ITPItem item)
        {
            return !IsFull() && TypeMatch(item);
        }

        /// <summary> Is type of item same as slot type? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TypeMatch(ITPItem item)
        {
            return item.Type == Type;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull()
        {
            return HoldItem != null && HoldItem.AmountStack >= HoldItem.MaxStack;
        }
    }
}
