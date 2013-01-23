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
    public class PropertyDescriptionBuilderTests
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
        public void BuildPropertyDescription_WhenMethodHasNoReturnValue_ReturnValueIsNull()
        {
            PropertyInfo property = _testClass.GetProperty("PublicStringProperty");

            var builder = new PropertyDescriptionBuilder(_xmlSource);
            PropertyDescription description = builder.BuildPropertyDescription(property);

            Assert.AreEqual("String", description.TypeName);
            Assert.AreEqual("PublicStringProperty", description.Name);
        }

        [TestMethod]
        public void BuildPropertyDescription_WhenReturnValueIsDescribed_DescriptionIsIncluded()
        {
            PropertyInfo property = _testClass.GetProperty("PublicStringProperty");

            var builder = new PropertyDescriptionBuilder(_xmlSource);
            PropertyDescription description = builder.BuildPropertyDescription(property);

            Assert.IsTrue(description.Summary.Length > 0);
        }

    }
}
