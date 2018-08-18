/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    public class TPItem : ITPItem
    {
        public int ID { get; protected set; }
        public int Type { get; protected set; }

        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public double Worth { get; protected set; }

        public int AmountStack { get; protected set; }
        public int MaxStack { get; protected set; }

        public ITPModifier[] Modifiers { get; protected set; }
        public ITPItemSlot OnSlot { get; protected set; }

        public Action OnUsed { get; protected set; }
        public Action OnMoved { get; protected set; }
        public Action OnFailMoved { get; protected set; }
        public Action OnEquipped { get; protected set; }
        public Action OnUnEquipped { get; protected set; }

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
    }
}
