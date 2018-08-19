/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public class TPItemEquipSlot : TPItemSlot, ITPEquipSlot
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool TryMoveItem(ITPItemSlot targetSlot)
        {
            if (targetSlot.CanHoldItem(HoldItem))
            {
                HoldItem.OnUnEquipped?.Invoke();
                HoldItem = targetSlot.SwitchItem(HoldItem);
                HoldItem?.OnEquipped?.Invoke();
                return true;
            }
            HoldItem?.OnFailMoved?.Invoke();
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
