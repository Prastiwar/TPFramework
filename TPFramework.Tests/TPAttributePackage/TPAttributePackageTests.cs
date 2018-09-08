using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TP.Framework.Tests
{
    [TestClass]
    public class TPAttributePackageTests
    {
        private TPAttribute health;
        private TPModifier healthIncreaser;

        private void Reset()
        {
            health = new TPAttribute(100);
            healthIncreaser = new TPModifier(ModifierType.FlatIncrease, 10);
        }

        [TestMethod]
        public void AddModifier()
        {
            Reset();

            Assert.AreEqual(100, health.Value);
            health.Modifiers.Add(healthIncreaser);
            Assert.AreEqual(110, health.Value);
        }

        [TestMethod]
        public void RemoveModifier()
        {
            Reset();

            health.Modifiers.Add(healthIncreaser);
            bool removed = health.Modifiers.Remove(healthIncreaser);
            Assert.IsTrue(removed);
            Assert.AreEqual(100, health.Value);
        }

        [TestMethod]
        public void HasModifier()
        {
            Reset();

            bool has = false;
            has = health.Modifiers.HasModifier(healthIncreaser);
            Assert.IsFalse(has);

            health.Modifiers.Add(healthIncreaser);
            has = health.Modifiers.HasModifier(healthIncreaser);
            Assert.IsTrue(has);

            health.Modifiers.Remove(healthIncreaser);
            has = health.Modifiers.HasModifier(healthIncreaser);
            Assert.IsFalse(has);
        }

        [TestMethod]
        public void Compare()
        {
            Reset();

            int compare = health.Modifiers.Compare(healthIncreaser, healthIncreaser);
            Assert.AreEqual(0, compare);

            compare = health.Modifiers.Compare(healthIncreaser, new TPModifier(ModifierType.FlatIncrease, 10, 1));
            Assert.AreEqual(-1, compare);

            compare = health.Modifiers.Compare(healthIncreaser, new TPModifier(ModifierType.FlatIncrease, 10, -1));
            Assert.AreEqual(1, compare);
        }

        [TestMethod]
        public void PrioritySorting()
        {
            Reset();

            TPModifier healthIncreaserII = new TPModifier(ModifierType.Percentage, 0.5f, 1);

            health.Modifiers.Add(healthIncreaser);
            health.Modifiers.Add(healthIncreaserII);

            Assert.AreEqual(165, health.Value);
            health.Modifiers.ChangeModifier(healthIncreaserII, new TPModifier(ModifierType.Percentage, 0.5f, -1));
            Assert.AreEqual(160, health.Value);
        }
    }
}
