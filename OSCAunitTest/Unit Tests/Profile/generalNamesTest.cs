using OSCA.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1;
using OSCA;

namespace OSCAunitTest
{
    
    /// <summary>
    ///This is a test class for generalNamesTest and is intended
    ///to contain all generalNamesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class generalNamesTest
    {

        private static GeneralName gn1 = new GeneralName(1, "eric@foo.com");
        private static GeneralNames gn2 = new GeneralNames(gn1);
        private static GeneralName gn3 = new GeneralName(new X509Name("cn=foo, ou=bar"));
        private static GeneralName[] gn4 = { gn1, gn3 };
        private static GeneralNames gn5 = new GeneralNames(new DerSequence(gn4));

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
        ///A test for getGenName
        ///</summary>
        [TestMethod()]
        public void getGenNameTest()
        {
            string type = "dNSName";
            GenName expected = GenName.dNSName;
            GenName actual;
            actual = generalNames.getGenName(type);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for createGeneralNames
        ///</summary>
        [TestMethod()]
        public void createGeneralNamesTest()
        {
            GeneralName[] genNames = gn4;
            GeneralNames expected = gn5;
            GeneralNames actual;
            actual = generalNames.createGeneralNames(genNames);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for createGeneralNames
        ///</summary>
        [TestMethod()]
        public void createGeneralNamesTest1()
        {
            GeneralName genName = gn1;
            GeneralNames expected = gn2;
            GeneralNames actual;
            actual = generalNames.createGeneralNames(genName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for createGeneralNames
        ///</summary>
        [TestMethod()]
        public void createGeneralNamesTest2()
        {
            string type = "rfc822Name";
            string name = "eric@foo.com";
            GeneralNames expected = gn2;
            GeneralNames actual;
            actual = generalNames.createGeneralNames(type, name);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for createGeneralName
        ///</summary>
        [TestMethod()]
        public void createGeneralNameTest()
        {
            string type = "rfc822Name";
            string name = "eric@foo.com";
            GeneralName expected = gn1;
            GeneralName actual;
            actual = generalNames.createGeneralName(type, name);
            Assert.AreEqual(expected, actual);
        }
    }
}
