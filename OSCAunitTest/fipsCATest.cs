using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCA.Offline;
using System.IO;

namespace OSCAunitTest
{
    /// <summary>
    /// Test fipsCA
    /// </summary>
    [TestClass]
    public class fipsCATest
    {
        public fipsCATest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            CaTestHarness.InitialiseCA(true);

        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            CaTestHarness.Cleanup();

        }

        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// Test the constructor
        /// </summary>
        [TestMethod]
        public void fipsCAConstructorTest()
        {
            fipsCA ca = new fipsCA(CaTestHarness.testCAConfigFile);

            Assert.IsTrue(ca.FIPS140Mode);
            //Assert.AreEqual("RootCA", ca.CAType);
            Assert.AreEqual("SHA1withRSA", ca.SignatureAlgorithm);
        }

        /// <summary>
        /// Test the constructor for invalid args
        /// </summary>
        [TestMethod]
        public void fipsCAConstructorTest1()
        {
            try
            {
                fipsCA ca = new fipsCA(@"c:\foo.xml");
            }
            catch (IOException ex)
            {
                Assert.AreEqual(@"Could not find file 'c:\foo.xml'.", ex.Message);
            }
        }




    }
}
