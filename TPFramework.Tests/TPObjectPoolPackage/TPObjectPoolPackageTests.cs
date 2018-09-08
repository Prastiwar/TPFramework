using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TP.Framework.Tests
{
    [TestClass]
    public class TPObjectPoolPackageTests
    {
        private class IntPool : TPObjectPool<int>
        {
            public IntPool(int capacity = 4) : base(capacity) { }

            protected override int CreateNewObject()
            {
                return 0;
            }
        }

        [TestMethod]
        public void Get()
        {
            IntPool pool = new IntPool();
            pool.Push(10);
            Assert.AreEqual(10, pool.Get());
            Assert.AreEqual(0, pool.Get());
        }

        [TestMethod]
        public void Length()
        {
            IntPool pool = new IntPool();
            Assert.AreEqual(0, pool.Length);
            pool.Push(10);
            Assert.AreEqual(1, pool.Length);
            Assert.AreEqual(10, pool.Get());
            Assert.AreEqual(0, pool.Get());
            Assert.AreEqual(0, pool.Length);
        }
    }
}
