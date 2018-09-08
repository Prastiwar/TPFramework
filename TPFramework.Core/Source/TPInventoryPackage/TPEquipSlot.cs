/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Runtime.CompilerServices;

namespace TP.Framework
{
    [Serializable]
    public class TPEquipSlot : TPItemSlot, ITPEquipSlot<TPItem>
    {
        public TPEquipSlot(int type, TPItem storeItem = null) : base(type, storeItem) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool MoveItem(ITPItemSlot<TPItem> targetSlot)
        {
            if (targetSlot.CanHoldItem(StoredItem))
            {
                StoredItem.OnUnEquipped?.Invoke();
                StoredItem = targetSlot.SwitchItem(StoredItem);
                StoredItem?.OnEquipped?.Invoke();
                return true;
            }
            StoredItem?.OnFailMoved?.Invoke();
            return false;
        }

        /// <summary> Checks if given slot is opposite of this slot </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool IsSlotOpposite(ITPItemSlot<TPItem> slot)
        {
            return !(slot is ITPEquipSlot<TPItem>);
        }
    }
}
