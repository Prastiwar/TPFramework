/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public abstract class TPInventory : ITPInventory
    {
        public virtual Dictionary<int, ITPItem> Items { get; protected set; }
        public virtual ITPItemSlot[] ItemSlots { get; protected set; }
        public virtual ITPEquipSlot[] EquipSlots { get; protected set; }
        public virtual bool IsFull { get { return HasEmptySlot(); } }

        /// <summary> Has inventory a slot with type which is empty? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool HasEmptySlot(int type = 0)
        {
            int length = ItemSlots.Length;
            for (int i = 0; i < length; i++)
            {
                if (ItemSlots[i].Type == type && ItemSlots[i].IsEmpty())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary> Does item exist in any of slot? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool HasItem(ITPItem item)
        {
            return HasItem(item.ID);
            //int index = ItemSlots.FindIndex(x => x == item);
            //if (index == -1)
            //{
            //    return EquipSlots.FindIndex(x => x == item) > -1;
            //}
            //return true;
        }

        /// <summary> Does item exist in any of slot? </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool HasItem(int itemID)
        {
            return Items.ContainsKey(itemID);
            //int index = ItemSlots.FindIndex(x => x == item);
            //if (index == -1)
            //{
            //    return EquipSlots.FindIndex(x => x == item) > -1;
            //}
            //return true;
        }

        ///// <summary> Does item exist in any of slot? </summary>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public virtual ITPItemSlot GetItemSlot(ITPItem item)
        //{
        //    int index = ItemSlots.FindIndex(x => x == item);
        //    if (index == -1)
        //    {
        //        index = EquipSlots.FindIndex(x => x == item);
        //        return index > -1 ? ItemSlots[index] : null;
        //    }
        //    return ItemSlots[index];
        //}

    }

}
