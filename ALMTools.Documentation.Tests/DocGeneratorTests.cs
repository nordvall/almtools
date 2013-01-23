using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using ALMTools.Documentation.Import;
using ALMTools.Documentation.Model;
using ALMTools.Documentation.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ALMTools.Documentation.Tests
{

    [TestClass]
    public class DocGeneratorTests
    {
        XDocument _document;

        [TestInitialize]
        public void Init()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("ALMTools.Documentation.Tests.TestData.ALMTools.Documentation.Tests.xml");
            using (XmlReader reader = XmlReader.Create(stream))
            {
                _document = XDocument.Load(reader);
            }
        }

        [TestMethod]
        public void Generate_WhenInvokedWithoutSettingsAltered_PublicMethodsAreIncluded()
        {
            var generator = new DocGenerator(_document);
            Type type = typeof(TestClass);
            ClassDescription description = generator.Generate(type);

            Assert.AreEqual(4, description.PublicMethods.Count);
        }

        [TestMethod]
        public void Generate_WhenInvokedWithoutSettingsAltered_PropertiesAreIncluded()
        {
            var generator = new DocGenerator(_document);
            Type type = typeof(TestClass);
            ClassDescription description = generator.Generate(type);

            Assert.AreEqual(2, description.Properties.Count);
        }

        [TestMethod]
        public void Generate_WhenInvokedAndSettingsAreAltered_PublicMethodsAreNotIncluded()
        {
            var generator = new DocGenerator(_document);
            generator.MethodsToInclude = null;

            Type type = typeof(TestClass);
            ClassDescription description = generator.Generate(type);

            Assert.AreEqual(0, description.PublicMethods.Count);
        }

        [TestMethod]
        public void Generate_WhenInvokedAndSettingsAreAltered_PropertiesAreNotIncluded()
        {
            var generator = new DocGenerator(_document);
            generator.PropertiesToInclude = null;

            Type type = typeof(TestClass);
            ClassDescription description = generator.Generate(type);

            Assert.AreEqual(0, description.Properties.Count);
        }
    }
}
