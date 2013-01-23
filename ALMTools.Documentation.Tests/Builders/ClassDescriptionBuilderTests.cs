using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using ALMTools.Documentation.Import;
using ALMTools.Documentation.Model;
using ALMTools.Documentation.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ALMTools.Documentation.Tests.Builders
{
    [TestClass]
    public class ClassDescriptionBuilderTests
    {
        MSBuildXmlParser _xmlSource;
        Type _testClass = typeof(TestClass);

        [TestInitialize]
        public void Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ALMTools.Documentation.Tests.TestData.ALMTools.Documentation.Tests.xml");
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument document = XDocument.Load(reader);
                _xmlSource = new MSBuildXmlParser(document);
            }
        }

        [TestMethod]
        public void BuildClassDescription_WhenInvoked_NameAndNamespaceAreCorrectlySet()
        {
            var builder = new ClassDescriptionBuilder(_xmlSource);
            ClassDescription description = builder.BuildClassDescription(_testClass);

            Assert.AreEqual("TestClass", description.Name);
            Assert.AreEqual("ALMTools.Documentation.Tests.TestData", description.Namespace);
        }

        [TestMethod]
        public void BuildClassDescription_WhenInvoked_AssemblyInformationIsCorrectlySet()
        {
            var builder = new ClassDescriptionBuilder(_xmlSource);
            ClassDescription description = builder.BuildClassDescription(_testClass);

            AssemblyName assemblyName = _testClass.Assembly.GetName();

            Assert.AreEqual(assemblyName.Name, description.AssemblyName);
        }

        [TestMethod]
        public void BuildClassDescription_WhenClassIsDescribedInXml_DescriptionIsIncluded()
        {
            var builder = new ClassDescriptionBuilder(_xmlSource);
            ClassDescription description = builder.BuildClassDescription(_testClass);

            Assert.IsTrue(description.Summary.Length > 0);
        }

    }
}
