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
        public Action OnMoved { get; set; }
        public Action OnFailMoved { get; set; }
        public Action OnEquipped { get; set; }
        public Action OnUnEquipped { get; set; }

        public int ID { get; protected set; }
        public int Type { get; protected set; }

        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public double Worth { get; protected set; }

        public int AmountStack { get; protected set; }
        public int MaxStack { get; protected set; }
        public float Weight { get; protected set; }

        public ITPModifier[] Modifiers { get; protected set; }
        public ITPItemSlot OnSlot { get; protected set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Use()
        {
            AmountStack--;
            if (AmountStack <= 0)
            {
                OnSlot.SwitchItem(null);
            }
            OnUsed();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool Stack(int count = 1)
        {
            if (CanStack(count))
            {
                AmountStack += count;
                return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool CanStack(int count = 1)
        {
            return AmountStack + count <= MaxStack;
        }
    }
}
