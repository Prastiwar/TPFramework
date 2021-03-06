﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TP.Framework.Tests
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
        public void PingPong()
        {
            CompareFloats(0, TPMath.PingPong(0, 1));
            CompareFloats(0.1f, TPMath.PingPong(0.1f, 1));
            CompareFloats(0.5f, TPMath.PingPong(0.5f, 1));
            CompareFloats(0.9f, TPMath.PingPong(0.9f, 1));
            CompareFloats(1, TPMath.PingPong(1, 1));
            CompareFloats(0.9f, TPMath.PingPong(1.1f, 1));
            CompareFloats(0.8f, TPMath.PingPong(1.2f, 1));
            CompareFloats(0.7f, TPMath.PingPong(1.3f, 1));
            CompareFloats(0.6f, TPMath.PingPong(1.4f, 1));
            CompareFloats(0.2f, TPMath.PingPong(1.8f, 1));
        }

        [TestMethod]
        public void Repeat()
        {
            CompareFloats(0, TPMath.Repeat(0, 0.6f));
            CompareFloats(0.5f, TPMath.Repeat(0.5f, 0.6f));
            CompareFloats(0, TPMath.Repeat(0.6f, 0.6f));
            CompareFloats(0.1f, TPMath.Repeat(0.7f, 0.6f));
            CompareFloats(0.2f, TPMath.Repeat(0.8f, 0.6f));
            CompareFloats(0.3f, TPMath.Repeat(0.9f, 0.6f));
            CompareFloats(0.4f, TPMath.Repeat(1, 0.6f));
            CompareFloats(0.5f, TPMath.Repeat(1.1f, 0.6f));
            CompareFloats(0, TPMath.Repeat(1.2f, 0.6f));
            CompareFloats(0.1f, TPMath.Repeat(1.3f, 0.6f));
            CompareFloats(0.2f, TPMath.Repeat(1.4f, 0.6f));
            CompareFloats(0.3f, TPMath.Repeat(1.5f, 0.6f));
            CompareFloats(0.4f, TPMath.Repeat(1.6f, 0.6f));
        }

        [TestMethod]
        public void GetSequence()
        {
            int length = 10;
            float[] floats = TPMath.GetSequence(length);
            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(i, floats[i]);
            }

        }

        [TestMethod]
        public void GetReversedSequence()
        {
            int length = 10;
            float[] floats = TPMath.GetReversedSequence(length);
            float value = 0;
            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(value - i, floats[i]);
            }
        }

        [TestMethod]
        public void ReverseValue()
        {
            Assert.AreEqual(1, TPMath.Flip(0));
            Assert.AreEqual(10, TPMath.Flip(0, 10));

            Assert.AreEqual(0.5f, TPMath.Flip(0.5f));
            Assert.AreEqual(5, TPMath.Flip(5, 10));

            Assert.AreEqual(0.8f, TPMath.Flip(0.2f));
            Assert.AreEqual(8, TPMath.Flip(2, 10));

            Assert.AreEqual(4.8f, TPMath.Flip(0.2f, 5));
            Assert.AreEqual(3, TPMath.Flip(2, 5));
        }

        [TestMethod]
        public void DigitCount()
        {
            Assert.AreEqual(1, TPMath.DigitCount(1));
            Assert.AreEqual(2, TPMath.DigitCount(12));
            Assert.AreEqual(10, TPMath.DigitCount(1234567890));

            Assert.AreEqual(2, TPMath.DigitCount(12.125f));
            Assert.AreEqual(1, TPMath.DigitCount(5.9999f));
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

        private void CompareFloats(float a, float b)
        {
            try
            {
                Assert.AreEqual(a, b);
            }
            catch (Exception)
            {
                try
                {
                    Assert.IsTrue(TPMath.Approximately(a, b));
                }
                catch (Exception)
                {
                    throw new AssertFailedException($"Expected: {a} Actual: {b} are not equal/similiar");
                }
            }
        }
    }
}
