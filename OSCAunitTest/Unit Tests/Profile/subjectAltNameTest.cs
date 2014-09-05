using OSCA.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Org.BouncyCastle.Asn1.X509;
using OSCA.Crypto;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1;
using OSCA;

namespace OSCAunitTest
{
    
    
    /// <summary>
    ///This is a test class for subjectAltNameTest and is intended
    ///to contain all subjectAltNameTest Unit Tests
    ///It also tests the underlying alternativeName class
    ///</summary>
    [TestClass()]
    public class subjectAltNameTest
    {
        
        /*  OSCA Profile XML
        ================
        <Extension>
            <name displayName="Subject Alternative Name">SubjectAlternativeName</name>
            <critical>false</critical>
            <value>
                <altName type="rc822Name">peter@foo.com</altName>
                <altName type="dNSName">peter.foo.com</altName>
            </value>
        </Extension>
    */
        private XElement testData1 = new XElement("Extension",
            new XElement("name", "SubjectAlternativeName",
                new XAttribute("displayName", "Subject Alternative Name")),
            new XElement("critical", "False"),
            new XElement("value",
                new XElement("altName", "peter@foo.com",
                    new XAttribute("type", "rfc822Name")),
                new XElement("altName", "peter.foo.com",
                    new XAttribute("type", "dNSName"))
            )
        );

        private OSCAGeneralName testData2 = new OSCAGeneralName()
        {
            Name = "peter@foo.com",
            Type = GenName.rfc822Name
        };

        private OSCAGeneralName testData3 = new OSCAGeneralName()
        {
            Name = "eric@bar.org",
            Type = GenName.rfc822Name
        };

        private string extValue = "#301e810d706574657240666f6f2e636f6d820d70657465722e666f6f2e636f6d";

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
        ///A test for subjectAltName Constructor
        ///</summary>
        [TestMethod()]
        public void subjectAltNameConstructorTest()
        {
            subjectAltName target = new subjectAltName();
            Assert.AreEqual("SubjectAlternativeName", target.Name);
        }

        /// <summary>
        ///A test for subjectAltName Constructor
        ///</summary>
        [TestMethod()]
        public void subjectAltNameConstructorTest1()
        {
            bool critical = true;
            subjectAltName target = new subjectAltName(critical);
            Assert.IsTrue(target.Critical);
        }

        /// <summary>
        ///A test for subjectAltName Constructor
        ///</summary>
        [TestMethod()]
        public void subjectAltNameConstructorTest2()
        {
            XElement Xml = testData1;
            subjectAltName target = new subjectAltName(Xml);
            Assert.AreEqual("SubjectAlternativeName", target.Name);
            Assert.IsFalse(target.Critical);
            Assert.IsTrue(target.SubjAltNames.Count == 2);
        }

        /// <summary>
        ///A test for SubjAltNames
        ///</summary>
        [TestMethod()]
        public void SubjAltNamesTest()
        {
            subjectAltName target = new subjectAltName(testData1);
            List<OSCAGeneralName> actual;
            actual = target.SubjAltNames;

            Assert.IsTrue(actual[0].Name == "peter@foo.com");
            Assert.IsTrue(actual[0].Type == GenName.rfc822Name);
            Assert.IsTrue(actual[1].Name == "peter.foo.com");
            Assert.IsTrue(actual[1].Type == GenName.dNSName);
        }

        /// <summary>
        ///A test for GeneralNames
        ///</summary>
        [TestMethod()]
        public void GeneralNamesTest()
        {
            subjectAltName target = new subjectAltName(testData1);
            GeneralNames actual;
            actual = target.GeneralNames;
            GeneralName[] gn = actual.GetNames();
            Assert.AreEqual("peter@foo.com", gn[0].Name.ToString());
            Assert.IsTrue(gn[1].TagNo == 2);
        }

        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest()
        {
            subjectAltName target = new subjectAltName(testData1);
            XNode expected = testData1;
            XNode actual;
            actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            OSCAGeneralName Name = testData2;
            subjectAltName target = new subjectAltName();
            target.Add(Name);
            Assert.AreEqual(testData2, target.SubjAltNames[0]);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            subjectAltName target = new subjectAltName(testData1);
            OSCAGeneralName Name = testData2;
            target.Remove(Name);
            Assert.AreEqual(1, target.SubjAltNames.Count);
        }

    }
}
