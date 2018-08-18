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

        /// <summary> Holds given item and returns the old one </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override ITPItem SwitchItem(ITPItem item)
        {
            ITPItem returnItem = HoldItem ?? null;
            HoldItem = item;
            HoldItem?.OnEquipped?.Invoke();
            return returnItem;
        }

        /// <summary> Returns opposite Slot (ItemSlot-EquipSlot) that can hold HoldItem </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override ITPItemSlot FindFreeOppositeSlot(ITPItemSlot[] slots)
        {
            int length = slots.Length;
            for (int i = 0; i < length; i++)
            {
                if (slots[i].CanHoldItem(HoldItem) && !(slots[i] is ITPEquipSlot))
                {
                    return slots[i];
                }
            }
            return null;
        }
    }
}
