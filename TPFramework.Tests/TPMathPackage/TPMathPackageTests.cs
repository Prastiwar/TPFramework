using Microsoft.VisualStudio.TestTools.UnitTesting;
using TPFramework.Core;

namespace TPFramework.Tests
{
    [TestClass]
    public class TPMathPackageTests
    {
        [TestMethod]
        public void Clamp()
        {
            int val = 10;
            val = TPMath.Clamp(val, 12, 20);
            Assert.AreEqual(12, val);
        }

        [TestMethod]
        public void StepTowards()
        {
            float val = 0;
            float target = 1;
            val = TPMath.StepTowards(val, target, 1);
            Assert.AreEqual(1, val);
        }

        [TestMethod]
        public void Normalize()
        {
            Assert.AreEqual(0, TPMath.Normalize(0, 100));
            Assert.AreEqual(0.5f, TPMath.Normalize(50, 100));
            Assert.AreEqual(1, TPMath.Normalize(100, 100));

            Assert.AreEqual(0, TPMath.Normalize(100, 100, 200));
            Assert.AreEqual(0.5f, TPMath.Normalize(150, 100, 200));
            Assert.AreEqual(1, TPMath.Normalize(200, 100, 200));

            Assert.AreEqual(0.5f, TPMath.Normalize(100, 100, 200, 0.5f, 1));
            Assert.AreEqual(0.75f, TPMath.Normalize(150, 100, 200, 0.5f, 1));
            Assert.AreEqual(1, TPMath.Normalize(200, 100, 200, 0.5f, 1));

            Assert.AreEqual(0.75f, TPMath.Normalize(100, 200, 0.5f, 1));
            Assert.AreEqual(0.875f, TPMath.Normalize(150, 200, 0.5f, 1));
            Assert.AreEqual(1, TPMath.Normalize(200, 200, 0.5f, 1));
        }
    }
}
