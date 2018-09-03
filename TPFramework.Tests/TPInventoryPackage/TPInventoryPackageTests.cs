using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TPFramework.Core;

namespace TPFramework.Tests
{
    [TestClass]
    public class TPInventoryPackageTests
    {
        private class Inventory : TPInventory<TPItemSlot, TPEquipSlot, TPItem>
        {
            public Inventory()
            {
                Items = new Dictionary<int, ITPItem>();
            }

            public Inventory(params TPItemSlot[] slots) : this()
            {
                ItemSlots = slots;
                EquipSlots = new TPEquipSlot[0];
            }

            public Inventory(TPEquipSlot[] eqSlots, TPItemSlot[] slots) : this()
            {
                EquipSlots = eqSlots;
                ItemSlots = slots;
            }
        }

        private Inventory inv;
        private TPItem item1;
        private TPItem item2;
        private TPItemSlot slot1;
        private TPItemSlot slot2;

        private void Reset()
        {
            item1 = new TPItem(0, 0);
            item2 = new TPItem(1, 0) {
                MaxStack = 2
            };
            slot1 = new TPItemSlot(0);
            slot2 = new TPItemSlot(0);
            inv = new Inventory(slot1, slot2);
        }

        [TestMethod]
        public void AddItem()
        {
            Reset();

            bool added = inv.AddItem(item1, slot1);
            Assert.IsTrue(added);
            Assert.IsTrue(inv.HasItem(0));

            Reset();

            added = inv.AddItem(item1);
            Assert.IsTrue(added);
            Assert.IsTrue(inv.HasItem(0));
        }

        [TestMethod]
        public void MoveItem()
        {
            Reset();

            inv.AddItem(item1, slot1);
            bool moved = slot1.MoveItem(slot2);
            Assert.IsTrue(moved);
            Assert.IsTrue(inv.HasItem(0));
        }

        [TestMethod]
        public void SlotCount()
        {
            Reset();

            Assert.AreEqual(2, inv.SlotCount);
            Assert.AreEqual(2, inv.EmptySlotsCount);
            inv.AddItem(item1, slot1);
            Assert.AreEqual(2, inv.SlotCount);
            Assert.AreEqual(1, inv.EmptySlotsCount);
        }

        [TestMethod]
        public void ItemCount()
        {
            Reset();

            Assert.AreEqual(0, inv.ItemCount);
            inv.AddItem(item1);
            inv.AddItem(item2);
            Assert.AreEqual(2, inv.ItemCount);
        }

        [TestMethod]
        public void Full()
        {
            Reset();

            Assert.IsFalse(inv.IsFull);
            inv.AddItem(item1);
            inv.AddItem(item2);
            Assert.IsTrue(inv.IsFull);
        }

        [TestMethod]
        public void FindSlot()
        {
            Reset();

            ITPItemSlot slot = inv.FindAnySlot(x => x.CanHoldItem(item1));
            Assert.IsNotNull(slot);
        }

        [TestMethod]
        public void StackItem()
        {
            Reset();

            Assert.AreEqual(1, item1.AmountStack);
            inv.AddItem(item1, slot1);
            Assert.AreEqual(1, item1.AmountStack);
            bool stack = inv.AddItem(item1);
            Assert.IsFalse(stack);
            Assert.AreEqual(1, item1.AmountStack);
        }

        [TestMethod]
        public void StackItem2()
        {
            Reset();

            Assert.AreEqual(1, item2.AmountStack);
            inv.AddItem(item2, slot2);

            Assert.AreEqual(1, item2.AmountStack);
            Assert.IsTrue(inv.AddItem(item2));
            Assert.AreEqual(2, item2.AmountStack);

            Assert.IsFalse(inv.AddItem(item2));
            Assert.AreEqual(2, item2.AmountStack);
        }

        [TestMethod]
        public void UseItem()
        {
            Reset();

            inv.AddItem(item1, slot1);
            inv.AddItem(item1);
            bool used = slot1.UseItem();
            Assert.IsTrue(used);
            used = slot1.UseItem();
            Assert.IsFalse(used);
        }

        [TestMethod]
        public void CanHoldOther()
        {
            Reset();

            bool can = slot1.CanHoldItem(item1);
            Assert.IsTrue(can);
            inv.AddItem(item1, slot1);
            can = slot1.CanHoldItem(item2);
            Assert.IsFalse(can);
        }
    }
}
