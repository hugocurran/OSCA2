using OSCA.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using OSCA;
using OSCA.Crypto;

namespace OSCAunitTest
{
    
    
    /// <summary>
    ///This is a test class for ProfileTest and is intended
    ///to contain all ProfileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProfileTest
    {


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
        ///A test for Version
        ///</summary>
        [TestMethod()]
        public void VersionTest()
        {
            Profile target = new Profile(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Version = expected;
            actual = target.Version;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            Profile target = new Profile(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LifetimeUnits
        ///</summary>
        [TestMethod()]
        public void LifetimeUnitsTest()
        {
            Profile target = new Profile(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            //target.CertificateLifetime.Units = expected;
            //actual = target.LifetimeUnits;
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }



        /// <summary>
        ///A test for Extensions
        ///</summary>
        [TestMethod()]
        public void ExtensionsTest()
        {
            Profile target = new Profile(); // TODO: Initialize to an appropriate value
            IList<ProfileExtension> actual;
            actual = target.Extensions;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Description
        ///</summary>
        [TestMethod()]
        public void DescriptionTest()
        {
            Profile target = new Profile(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Description = expected;
            actual = target.Description;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for loadProfile
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OSCA.dll")]
        public void loadProfileTest()
        {
            Profile_Accessor target = new Profile_Accessor(); // TODO: Initialize to an appropriate value
            XDocument profile = null; // TODO: Initialize to an appropriate value
            target.loadProfile(profile);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest()
        {
            Profile target = new Profile(); // TODO: Initialize to an appropriate value
            XDocument expected = null; // TODO: Initialize to an appropriate value
            XDocument actual;
            actual = target.ToXml();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SaveXml
        ///</summary>
        [TestMethod()]
        public void SaveXmlTest()
        {
            Profile target = new Profile(); // TODO: Initialize to an appropriate value
            string folder = string.Empty; // TODO: Initialize to an appropriate value
            target.SaveXml(folder);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }


        /// <summary>
        ///A test for Profile Constructor
        ///</summary>
        [TestMethod()]
        public void ProfileConstructorTest()
        {
            Profile target = new Profile();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Profile Constructor
        ///</summary>
        [TestMethod()]
        public void ProfileConstructorTest1()
        {
            string profileFile = string.Empty; // TODO: Initialize to an appropriate value
            Profile target = new Profile(profileFile);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Profile Constructor
        ///</summary>
        [TestMethod()]
        public void ProfileConstructorTest2()
        {
            XDocument profile = null; // TODO: Initialize to an appropriate value
            Profile target = new Profile(profile);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
