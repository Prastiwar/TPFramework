/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;

namespace TPFramework.Core
{
    public class TPItemSlot : ITPItemSlot
    {
        public int Type { get; protected set; }
        public ITPItem HoldItem { get; protected set; }
        public bool IsEquipSlot { get; protected set; }

        public bool TryMoveItem(ITPItemSlot targetSlot)
        {
            throw new NotImplementedException();
        }

        public bool CanHoldItem(ITPItem item)
        {
            if (HoldItem == null && TypeMatch(item))
                return true;
            else if (HoldItem == null && !TypeMatch(item))
                return false;
            //return HoldItem.MaxStack;
            return true;
        }

        public bool TypeMatch(ITPItem item)
        {
            return item.Type == Type;
        }

        public bool IsOverloaded()
        {
            return true;
        }

        public static bool operator ==(TPItemSlot v1, TPItemSlot v2)
        {
            return v1.Type == v2.Type && v1.IsEquipSlot && v2.IsEquipSlot;
        }

        public static bool operator !=(TPItemSlot v1, TPItemSlot v2)
        {
            return !(v1 == v2);
        }
    }
}
