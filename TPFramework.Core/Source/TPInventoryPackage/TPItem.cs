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
    public class TPItem : ITPItem
    {
        public Action OnUsed { get; set; }
        public Action OnFailUsed { get; set; }

        public Action OnMoved { get; set; }
        public Action OnFailMoved { get; set; }

        public Action OnEquipped { get; set; }
        public Action OnUnEquipped { get; set; }

        public int ID { get; protected set; }
        public int Type { get; protected set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Worth { get; set; }

        public int AmountStack { get; set; }
        public int MaxStack { get; set; }
        public float Weight { get; set; }

        public ITPModifier[] Modifiers { get; set; }

        public TPItem(int id, int type)
        {
            ID = id;
            Type = type;
            AmountStack = 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Use()
        {
            return UseItem();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Stack(int count)
        {
            return StackItem(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual bool UseItem()
        {
            if (CanUse())
            {
                AmountStack--;
                OnUsed?.Invoke();
                return true;
            }
            OnFailUsed?.Invoke();
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual bool StackItem(int count = 1)
        {
            if (CanStack(count))
            {
                AmountStack += count;
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual bool CanUse()
        {
            return AmountStack > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual bool CanStack(int count = 1)
        {
            return AmountStack + count <= MaxStack;
        }
    }
}
