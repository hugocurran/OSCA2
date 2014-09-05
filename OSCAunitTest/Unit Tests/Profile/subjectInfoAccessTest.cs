using OSCA.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509.Extension;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System.Xml.Linq;
using OSCA;
using OSCA.Crypto;
using OSCA.Crypto.X509;

namespace OSCAunitTest
{
    
    
    /// <summary>
    ///This is a test class for subjectInfoAccessTest and is intended
    ///to contain all subjectInfoAccessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class subjectInfoAccessTest
    {
        /*      OSCA XML
                    ========
                    <Extension>
                    <name description="Subject Info Access">SubjectInfoAccess</name>
                    <critical>false</critical>
                    <value>
                        <accessDescription>
                            <method>CARepository</method>
                            <location type="uniformResourceIdentifier">http://foo.com/issuer</location>
                        </accessDescription>
                         <accessDescription>
                            <method>TimeStamping</method>
                            <location type="uniformResourceIdentifier">http://foo.com/time</location>
                        </accessDescription>    
                    </value>
                    </Extension>
               */

        private XElement testData1 = new XElement("Extension",
            new XElement("name", "SubjectInfoAccess",
                new XAttribute("displayName", "Subject Info Access")),
            new XElement("critical", "False"),
            new XElement("value",
                new XElement("accessDescription",
                    new XElement("method", "CARepository"),
                    new XElement("location", "http://foo.com/issuer",
                        new XAttribute("type", "uniformResourceIdentifier"))
                ),
                new XElement("accessDescription",
                    new XElement("method", "TimeStamping"),
                    new XElement("location", "http://foo.com/time",
                        new XAttribute("type", "uniformResourceIdentifier"))
                )
            )
        );

        private AccessDesc testData2 = new AccessDesc()
        {
            Method = AccessMethod.CARepository,
            Location = new OSCAGeneralName()
            {
                Name = "http://foo.com/issuer",
                Type = GenName.uniformResourceIdentifier
            }
        };

        private AccessDesc testData3 = new AccessDesc()
        {
            Method = AccessMethod.TimeStamping,
            Location = new OSCAGeneralName()
            {
                Name = "http://foo.com/time",
                Type = GenName.uniformResourceIdentifier
            }
        };

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
        ///A test for subjectInfoAccess empty Constructor
        ///</summary>
        [TestMethod()]
        public void subjectInfoAccessConstructorTest()
        {
            subjectInfoAccess target = new subjectInfoAccess();
            target.Add(testData2);
            target.Add(testData3);
            Assert.AreEqual(testData1.ToString(), target.ToXml().ToString());
        }

        /// <summary>
        ///A test for subjectInfoAccess Constructor
        ///</summary>
        [TestMethod()]
        public void subjectInfoAccessConstructorTest1()
        {
            bool critical = false;
            subjectInfoAccess target = new subjectInfoAccess(critical);
            Assert.IsFalse(target.Critical);
        }

        /// <summary>
        ///A test for subjectInfoAccess XML Constructor
        ///</summary>
        [TestMethod()]
        public void subjectInfoAccessConstructorTest2()
        {
            subjectInfoAccess target = new subjectInfoAccess(testData1);
            Assert.IsFalse(target.Critical);
            Assert.AreEqual(testData1.ToString(), target.ToXml().ToString());
        }

        /// <summary>
        ///A test for subjectInfoAccess Constructor (from X509Extension)
        ///</summary>
        [TestMethod()]
        public void subjectInfoAccessConstructorTest3()
        {
            // Build an extension
            BcV3CertGen gen = new BcV3CertGen();
            subjectInfoAccess testExt = new subjectInfoAccess(testData1);
            gen.AddExtension(testExt.OID, testExt.Critical, testExt.DerEncoding);
            X509Certificate cert = CertTestHarness.RunCertGenerator(gen);
            X509Extension ext = CertTestHarness.ExtractExtension(cert, X509Extensions.SubjectInfoAccess);

            // Test code
            subjectInfoAccess target = new subjectInfoAccess(ext);
            List<AccessDesc> list = target.SubjectInfoAccess;
            Assert.IsFalse(target.Critical);
            Assert.AreEqual(2, list.Count);
        }

        /// <summary>
        ///A test for encode
        ///</summary>
        [TestMethod()]
        public void encodeTest1()
        {
            subjectInfoAccess target = new subjectInfoAccess(testData1);
            SubjectInformationAccess actual = target.SubjectInformationAccess;
            Assert.IsInstanceOfType(actual, typeof(SubjectInformationAccess));
        }
    }
}
