/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    [Serializable]
    public class TPModifierList<T> : ITPModifierList<T> where T : ITPModifier
    {
        private readonly ReusableList<T> reusableModifiers;
        protected virtual List<T> Modifiers { get; set; }

        public float Count { get { return Modifiers.Count; } }
        public ITPAttribute<T> Attribute { get; protected set; }

        public T this[int index] { get { return Modifiers[index]; } }

        public TPModifierList(ITPAttribute<T> attribute, int capacity = 10)
        {
            Modifiers = new List<T>(capacity);
            reusableModifiers = new ReusableList<T>(4);
            Attribute = attribute;
        }

        /// <summary> Adds modifier and recalculates Value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T modifier)
        {
            Modifiers.Add(modifier);
            Attribute.Recalculate();
        }

        /// <summary> If modifier exist - remove it and recalculate Value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(T modifier)
        {
            if (Modifiers.Remove(modifier))
            {
                Attribute.Recalculate();
                return true;
            }
            return false;
        }

        /// <summary> Removes all modifiers, recalculates Value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveModifiers()
        {
            Modifiers.Clear();
            Attribute.Recalculate();
        }

        /// <summary> Removes all modifiers from source, recalculates Value </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveModifiers(object source)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Modifiers[i].Source == source)
                {
                    Remove(Modifiers[i]);
                    i--;
                }
            }
            Attribute.Recalculate();
        }

        /// <summary> Get first modifier from source </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetModifier(object source)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Modifiers[i].Source == source)
                {
                    return Modifiers[i];
                }
            }
            return default(T);
        }

        /// <summary> Get all modifiers from source </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] GetModifiers(object source)
        {
            List<T> sourceModifiers = reusableModifiers.CleanList;

            for (int i = 0; i < Count; i++)
            {
                if (Modifiers[i].Source == source)
                    sourceModifiers.Add(Modifiers[i]);
            }
            return sourceModifiers.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasModifier(T modifier)
        {
            return Modifiers.Contains(modifier);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasModifier(object source)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Modifiers[i].Source == source)
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ChangeModifier(T modifier, T newModifier)
        {
            int index = Modifiers.IndexOf(modifier);
            if (index >= 0)
            {
                Modifiers[index] = newModifier;
                return true;
            }
            return false;
        }

        /// <summary> Compare modifiers by their priority </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare(T modifier1, T modifier2)
        {
            if (modifier1.Priority > modifier2.Priority)
                return 1;
            else if (modifier1.Priority < modifier2.Priority)
                return -1;
            return 0;
        }

        /// <summary> Sort modifiers by their priority </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Sort()
        {
            Modifiers.Sort(Compare);
        }
    }
}
