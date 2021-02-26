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
    public class ItemSlot : IItemSlot<ItemModel>
    {
        private Action onRemove;
        private ItemModel storedItem;
        private int type;

        public ItemModel StoredItem {
            get { return storedItem; }
            protected set {
                storedItem?.OnUsed.Remove(onRemove);
                storedItem = value;
                storedItem?.OnUsed.Add(onRemove);
                OnItemChanged?.Invoke();
            }
        }

        public int Type { get { return type; } protected set { type = value; } }

        public Action OnItemChanged { get; set; }

        ItemModel IItemSlot<ItemModel>.StoredItem { get { return StoredItem; } set { StoredItem = value; } }

        private void ShouldRemove()
        {
            if (StoredItem.AmountStack <= 0)
            {
                StoredItem = null;
            }
        }

        public ItemSlot(int type, ItemModel storeItem = null)
        {
            Type = type;
            StoredItem = storeItem;
            onRemove = ShouldRemove;
        }

        /// <summary> Holds given item and returns the old one </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ItemModel SwitchItem(ItemModel item)
        {
            ItemModel returnItem = storedItem ?? null;
            StoredItem = item;
            return returnItem;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool UseItem()
        {
            return StoredItem != null ? StoredItem.Use() : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool StackItem(int count = 1)
        {
            return StoredItem != null ? StoredItem.Stack(count) : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull()
        {
            return HasItem() && StoredItem.AmountStack >= StoredItem.MaxStack;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasItem()
        {
            return !(StoredItem is null);
        }

        /// <summary> Is type of item same as slot type? (or slot can hold anything (Type of 0)) </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TypeMatch(ItemModel item)
        {
            return item.Type == Type || Type == 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool IsSlotOpposite(IItemSlot<ItemModel> slot)
        {
            return slot is IEquipSlot<ItemModel>;
        }

        /// <summary> Checks if types match and there is a place in stack </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CanHoldItem(ItemModel item)
        {
            return TypeMatch(item) && !IsFull();
        }

        /// <summary> If targetSlot has item will check types match else check if CanHoldItem </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CanMoveItem(IItemSlot<ItemModel> targetSlot)
        {
            return targetSlot.StoredItem != null
                   ? TypeMatch(targetSlot.StoredItem) && targetSlot.TypeMatch(StoredItem)
                   : targetSlot.CanHoldItem(StoredItem);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool MoveItem(IItemSlot<ItemModel> targetSlot)
        {
            if (CanMoveItem(targetSlot))
            {
                StoredItem.OnMoved?.Invoke();
                StoredItem = targetSlot.SwitchItem(StoredItem);
                StoredItem?.OnMoved?.Invoke();
                return true;
            }
            StoredItem?.OnFailMoved?.Invoke();
            return false;
        }
    }
}
