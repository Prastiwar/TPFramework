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
    public class ItemModel : IItem<AttributeModifier>
    {
        private int id;
        private int type;
        private string name;
        private string description;
        private double worth;
        private int amountStack;
        private int maxStack;
        private float weight;
        private AttributeModifier[] modifiers;

        public Action OnUsed { get; set; }
        public Action OnFailUsed { get; set; }

        public Action OnMoved { get; set; }
        public Action OnFailMoved { get; set; }

        public Action OnEquipped { get; set; }
        public Action OnUnEquipped { get; set; }

        public int ID { get { return id; } protected set { id = value; } }
        public int Type { get { return type; } protected set { type = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public double Worth { get { return worth; } set { worth = value; } }
        public int AmountStack { get { return amountStack; } set { amountStack = value; } }
        public int MaxStack { get { return maxStack; } set { maxStack = value; } }
        public float Weight { get { return weight; } set { weight = value; } }
        public AttributeModifier[] Modifiers { get { return modifiers; } set { modifiers = value; } }

        public ItemModel(int id, int type)
        {
            ID = id;
            Type = type;
            AmountStack = 0;
            MaxStack = 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool Use()
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
