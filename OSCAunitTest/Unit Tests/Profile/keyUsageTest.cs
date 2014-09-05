using OSCA.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using System.Xml.Linq;
using OSCA;
using OSCA.Crypto;

namespace OSCAunitTest
{
    
    
    /// <summary>
    ///This is a test class for keyUsageTest and is intended
    ///to contain all keyUsageTest Unit Tests
    ///</summary>
    [TestClass()]
    public class keyUsageTest
    {
        /*
            OSCA Profile XML
            ================
            <Extension>
                <name displayName="Key Usage">KeyUsage</name>
                <critical>true</critical>
                <value>
                    <usage>KeyCertSign</usage>
                    <usage>CrlSign</usage>
                </value>
            </Extension>
     */

        private XElement testData1 = new XElement("Extension",
            new XElement("name", "KeyUsage",
                new XAttribute("displayName", "Key Usage")),
            new XElement("critical", "True"),
            new XElement("value", 
                new XElement("usage", "KeyCertSign"),
                new XElement("usage", "CRLSign"))
        );

        private string extValue = "#03020106";
        private string extValue1 = "KeyUsage: 0x6";

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Kusage
        ///</summary>
        [TestMethod()]
        public void KusageTest()
        {
            keyUsage target = new keyUsage(testData1);
            List<string> actual;
            actual = target.Kusage;
            Assert.AreEqual("CRLSign", actual[1]);
        }

        /// <summary>
        ///A test for KeyUsages
        ///</summary>
        [TestMethod()]
        public void KeyUsagesTest()
        {
            string[] actual;
            actual = keyUsage.KeyUsages;
            Assert.AreEqual("KeyCertSign", actual[5]);
        }

        /// <summary>
        ///A test for KeyUsage
        ///</summary>
        [TestMethod()]
        public void KeyUsageTest()
        {
            keyUsage target = new keyUsage(testData1);
            KeyUsage actual;
            actual = target.KeyUsage;
            Assert.AreEqual(extValue1, actual.ToString());
        }

        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest()
        {
            keyUsage target = new keyUsage(testData1);
            XNode expected = testData1;
            XNode actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            keyUsage target = new keyUsage(testData1);
            string usage = "KeyCertSign";
            target.Remove(usage);
            Assert.AreEqual(1, target.Kusage.Count);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            keyUsage target = new keyUsage();
            string usage = "KeyAgreement";
            target.Add(usage);
            Assert.IsTrue(target.Kusage.Count == 1);
            Assert.AreEqual("KeyAgreement", target.Kusage[0]);
        }

        /// <summary>
        ///A test for keyUsage Constructor
        ///</summary>
        [TestMethod()]
        public void keyUsageConstructorTest()
        {
            keyUsage target = new keyUsage();
            Assert.AreEqual("KeyUsage", target.Name);
        }

        /// <summary>
        ///A test for keyUsage Constructor
        ///</summary>
        [TestMethod()]
        public void keyUsageConstructorTest1()
        {
            bool Critical = true;
            keyUsage target = new keyUsage(Critical);
            Assert.IsTrue(target.Critical);
        }

        /// <summary>
        ///A test for keyUsage Constructor
        ///</summary>
        [TestMethod()]
        public void keyUsageConstructorTest2()
        {
            XElement xml = testData1;
            keyUsage target = new keyUsage(xml);
            Assert.AreEqual("KeyUsage", target.Name);
            Assert.AreEqual("Key Usage", target.DisplayName);
            Assert.IsTrue(target.Critical);
            Assert.IsTrue(target.Kusage.Count == 2);
           
        }
    }
}
