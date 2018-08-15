using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TPFramework.Core;

namespace TPFramework.Tests
{
    [TestClass]
    public class TPExtensionsPackageTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Action action = null;
            action.SafeInvoke();
            TPExtensions.DelayAction(1, action);

            List<int> list = new List<int> { 5, 5, 5 };
            float[] floatings = new float[] { 1, 2, 3 };
            int[] integers = new int[] { 1, 2, 3 };
            int[] integers2 = new int[] { 1, 7, 10, 2, 5 };

            floatings.SortReverse();
            integers.SortReverse();
            integers2.SortReverse();

            Assert.AreEqual(15, list.Sum());
            Assert.AreEqual(10, list.Sum(2));

            Assert.AreEqual(6, floatings.Sum());
            Assert.AreEqual(5, floatings.Sum(2));

            Assert.AreEqual(6, integers.Sum());
            Assert.AreEqual(5, integers.Sum(2));

            Assert.AreEqual(3, floatings[0]);
            Assert.AreEqual(3, integers[0]);

            Assert.AreEqual(10, integers2[0]);
        }
    }
}
