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

namespace OSCAunitTest
{
    /*
      id-ce-nameConstraints OBJECT IDENTIFIER ::=  { id-ce 30 }

      NameConstraints ::= SEQUENCE {
           permittedSubtrees       [0]     GeneralSubtrees OPTIONAL,
           excludedSubtrees        [1]     GeneralSubtrees OPTIONAL }

      GeneralSubtrees ::= SEQUENCE SIZE (1..MAX) OF GeneralSubtree

      GeneralSubtree ::= SEQUENCE {
           base                    GeneralName,
           minimum         [0]     BaseDistance DEFAULT 0,
           maximum         [1]     BaseDistance OPTIONAL }

      BaseDistance ::= INTEGER (0..MAX)
 * 
      OSCA xml
 *    ========
        <Extension>
            <name description="Name Constraints">NameConstraints</name>
            <critical>true</critical>
            <value>
                <permitted>
                    <name type="rfc822Name">*.foo.org</name>
                </permitted>
                <excluded>
                    <name type="dNSName">*.bar.net</name>
                </excluded>
            </value>
        </Extension>
 */
    
    /// <summary>
    ///This is a test class for nameConstraintsTest and is intended
    ///to contain all nameConstraintsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class nameConstraintsTest
    {
        // TEST DATA
        private XElement testData1 = new XElement("Extension",
            new XElement("name", "NameConstraints",
                new XAttribute("displayName", "Name Constraints")),
            new XElement("critical", "True"),
            new XElement("value",
                new XElement("permitted",
                    new XElement("name", "*.foo.org",
                        new XAttribute("type", "rfc822Name"))
                ),
            new XElement("excluded",
                new XElement("name", "*.bar.net",
                    new XAttribute("type", "dNSName"))
                )
            )
        );

        private OSCAGeneralName testData2 = new OSCAGeneralName()
        {
            Name = "*.foo.org",
            Type = GenName.rfc822Name
        };

        private OSCAGeneralName testData3 = new OSCAGeneralName()
        {
            Name = "*.bar.net",
            Type = GenName.dNSName
        };

        private string testData4 = "NameConstraints (2.5.29.30)\n\tPermitted Names:\n\t\trfc822Name: *.foo.org\n\tExcluded Names:\n\n";

        private string extValue = "#301ea00d300b81092a2e666f6f2e6f7267a10d300b82092a2e6261722e6e6574";

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
        //   th.CreateCA();
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
        ///A test for Permitted
        ///</summary>
        [TestMethod()]
        public void PermittedTest()
        {
            nameConstraints target = new nameConstraints(testData1);
            List<OSCAGeneralName> actual;
            actual = target.Permitted;
            Assert.AreEqual(testData2, actual[0]);
        }

        /// <summary>
        ///A test for NameConstraint
        ///</summary>
        [TestMethod()]
        public void NameConstraintTest()
        {
            nameConstraints target = new nameConstraints(testData1);
            Assert.IsInstanceOfType(target.NameConstraint, typeof(NameConstraints));
        }

        /// <summary>
        ///A test for Excluded
        ///</summary>
        [TestMethod()]
        public void ExcludedTest()
        {
            nameConstraints target = new nameConstraints(testData1);
            List<OSCAGeneralName> actual;
            actual = target.Excluded;
            Assert.AreEqual(testData3, actual[0]);
        }

        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest()
        {
            nameConstraints target = new nameConstraints(testData1);
            XNode expected = testData1;
            XNode actual;
            actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            nameConstraints target = new nameConstraints(testData1);
            NameConstraintTree Tree = NameConstraintTree.Permitted;
            target.Remove(Tree, testData2);
            Assert.AreEqual(0, target.Permitted.Count);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            nameConstraints target = new nameConstraints(true);
            NameConstraintTree Tree = NameConstraintTree.Permitted;
            target.Add(Tree, testData2);
            Assert.AreEqual(testData2, target.Permitted[0]);
        }

        /// <summary>
        /// To the string test.
        /// </summary>
        [TestMethod()]
        public void ToStringTest()
        {
            nameConstraints target = new nameConstraints(true);
            NameConstraintTree Tree = NameConstraintTree.Permitted;
            target.Add(Tree, testData2);
            Assert.AreEqual(testData4, target.ToString());
        }

        /// <summary>
        ///A test for nameConstraints Constructor
        /// - Default critical = true
        ///</summary>
        [TestMethod()]
        public void nameConstraintsConstructorTest()
        {
            nameConstraints target = new nameConstraints();
            Assert.IsTrue(target.Critical);
        }

        /// <summary>
        ///A test for nameConstraints Constructor
        ///</summary>
        [TestMethod()]
        public void nameConstraintsConstructorTest1()
        {
            bool Critical = false;
            nameConstraints target = new nameConstraints(Critical);
            Assert.IsFalse(target.Critical);
        }

        /// <summary>
        ///A test for nameConstraints Constructor
        ///</summary>
        [TestMethod()]
        public void nameConstraintsConstructorTest2()
        {
            XElement xml = testData1;
            nameConstraints target = new nameConstraints(xml);
            Assert.IsTrue(target.Critical);
            Assert.AreEqual(target.Name, "NameConstraints");
            Assert.AreEqual(target.DisplayName, "Name Constraints");
        }

        /// <summary>
        ///A test for nameConstraints Constructor
        ///</summary>
        [TestMethod()]
        public void nameConstraintsConstructorTest3()
        {
            XElement xml = testData1;
            nameConstraints target = new nameConstraints(xml);
            Assert.IsTrue(target.Critical);
            Assert.AreEqual(target.Name, "NameConstraints");
            Assert.AreEqual(target.DisplayName, "Name Constraints");
        }
    }
}
