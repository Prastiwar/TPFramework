using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TP.Framework.Collections.Tests
{
    [TestClass]
    public class TPCollectionsPackageTests
    {
        [TestMethod]
        public void Queue()
        {
            Queue<int, float> queue = new Queue<int, float>();
            queue.Enqueue(10, 20.5f);
            Assert.IsTrue(queue.Contains(10, 20.5f));
            Assert.AreEqual(new KeyValuePair<int, float>(10, 20.5f), queue.Dequeue());
            Assert.IsFalse(queue.Contains(10, 20.5f));
        }

        [TestMethod]
        public void Stack()
        {
            Stack<int, float> stack = new Stack<int, float>();
            stack.Push(10, 20.5f);
            Assert.IsTrue(stack.Contains(10, 20.5f));
            Assert.AreEqual(new KeyValuePair<int, float>(10, 20.5f), stack.Pop());
            Assert.IsFalse(stack.Contains(10, 20.5f));
        }

        [TestMethod]
        public void List()
        {
            ReusableList<int> integers = new ReusableList<int>();
            List<int> list = integers.CleanList;
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Dictionary()
        {
            ReusableDictionary<int, float> intFloats = new ReusableDictionary<int, float>();
            Dictionary<int, float> dictionary = intFloats.CleanDictionary;
            Assert.AreEqual(0, dictionary.Count);
        }
    }
}
