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
                return true;
            }
            HoldItem?.OnFailMoved?.Invoke();
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override ITPItem SwitchItem(ITPItem item)
        {
            ITPItem returnItem = HoldItem ?? null;
            HoldItem = item;
            HoldItem?.OnEquipped?.Invoke();
            return returnItem;
        }
    }
}
