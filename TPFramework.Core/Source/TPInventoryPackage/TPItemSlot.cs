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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual ITPItem SwitchItem(ITPItem item)
        {
            ITPItem returnItem = HoldItem ?? null;
            HoldItem = item;
            HoldItem?.OnMoved?.Invoke();
            return returnItem;
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
