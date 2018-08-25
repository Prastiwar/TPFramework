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
    public class TPItemEquipSlot : TPItemSlot, ITPEquipSlot
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool MoveItem(ITPItemSlot targetSlot)
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
        public override bool IsSlotOpposite(ITPItemSlot slot)
        {
            return !(slot is ITPEquipSlot);
        }
    }
}
