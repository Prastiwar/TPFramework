using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TP.Framework.Tests
{
    [TestClass]
    public class TPPersistencePackageTests
    {
        private class TPPersist : TPPersistSystem<TPPersist>
        {
            private readonly Dictionary<string, string> savedValues = new Dictionary<string, string>();
            private readonly HashSet<Type> supportTypes = new HashSet<Type>() { typeof(string) };

            protected override HashSet<Type> GetSupportedTypes() { return supportTypes; }

            protected override object LoadValue(PersistantAttribute attribute, Type objectType)
            {
                return savedValues.ContainsKey(attribute.Key)
                    ? savedValues[attribute.Key]
                    : attribute.DefaultValue;
            }

            protected override void SaveValue(PersistantAttribute attribute, object saveValue)
            {
                string stringValue = saveValue as string;
                Assert.IsNotNull(stringValue);
                savedValues.Add(attribute.Key, stringValue);
            }
        }

        private class DummyClass
        {
            [Persistant("MyKey", "Default")]
            public string Text = "NotDefault";

            [Persistant("MyOtherKey", "Default")]
            public string DefaultText;

            [Persistant("MyEvenOtherKey")]
            public string NotDefaultText;
        }

        private class DummyErrorClass
        {
            [Persistant("MyKey", true)]
            public string Text = "error";

            [Persistant("MyKey2", 10)]
            public string Text2 = "error";

            [Persistant("MyKey3", 10)]
            public int Integer = 50;
        }

        [TestMethod]
        public void SaveLoad()
        {
            DummyClass dummy = new DummyClass();
            TPPersist.Save(dummy);

            dummy.DefaultText = "WontSeeThis";
            dummy.Text = "WontSeeThis";
            dummy.NotDefaultText = "WontSeeThis";

            TPPersist.Load(dummy); // or dummy = TPPersistSystem.Load(dummy);
            Assert.AreEqual("NotDefault", dummy.Text);
            Assert.AreEqual("Default", dummy.DefaultText);
            Assert.IsNull(dummy.NotDefaultText);
        }

        [TestMethod]
        public void SaveLoadError()
        {
            DummyErrorClass dummy = new DummyErrorClass();
            TPPersist.Save(dummy);

            dummy.Text = "CantLoad";
            dummy.Text2 = "CantLoad";
            dummy.Integer = 111;

            dummy = TPPersist.Load(dummy);
            Assert.AreEqual("CantLoad", dummy.Text);
            Assert.AreEqual("CantLoad", dummy.Text2);
            Assert.AreEqual(111, dummy.Integer);
        }
    }
}
