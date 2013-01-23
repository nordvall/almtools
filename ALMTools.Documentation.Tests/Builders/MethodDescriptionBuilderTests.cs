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
    public class MethodDescriptionBuilderTests
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
        public void BuildMethodDescription_WhenMethodHasNoReturnValue_ReturnValueIsNull()
        {
            MethodInfo method = _testClass.GetMethod("PublicVoidMethodWithoutArguments");

            var builder = new MethodDescriptionBuilder(_xmlSource);
            MethodDescription description = builder.BuildMethodDescription(method);

            Assert.IsNull(description.ReturnValue);
        }

        [TestMethod]
        public void BuildMethodDescription_WhenMethodHasReturnValue_ReturnValueIsCorrect()
        {
            MethodInfo method = _testClass.GetMethod("PublicStringMethodWithNoArguments");

            var builder = new MethodDescriptionBuilder(_xmlSource);
            MethodDescription description = builder.BuildMethodDescription(method);
            ReturnValueDescription returnValue = description.ReturnValue;

            Assert.AreEqual("String", returnValue.TypeName);
        }

        [TestMethod]
        public void BuildMethodDescription_WhenReturnValueIsDescribed_DescriptionIsIncluded()
        {
            MethodInfo method = _testClass.GetMethod("PublicStringMethodWithTwoArguments");

            var builder = new MethodDescriptionBuilder(_xmlSource);
            MethodDescription description = builder.BuildMethodDescription(method);
            ReturnValueDescription returnValue = description.ReturnValue;

            Assert.IsTrue(returnValue.Description.Length > 0);
        }

        [TestMethod]
        public void BuildMethodDescription_WhenMethodHasTwoArguments_TwoArgumentsAreReturned()
        {
            MethodInfo method = _testClass.GetMethod("PublicStringMethodWithTwoArguments");

            var builder = new MethodDescriptionBuilder(_xmlSource);
            MethodDescription description = builder.BuildMethodDescription(method);

            Assert.AreEqual(2, description.Arguments.Count);
        }

        [TestMethod]
        public void BuildMethodDescription_WhenMethodHasArguments_ArgumentsAreCorrectlyDescribed()
        {
            MethodInfo method = _testClass.GetMethod("PublicStringMethodWithTwoArguments");

            var builder = new MethodDescriptionBuilder(_xmlSource);
            MethodDescription description = builder.BuildMethodDescription(method);

            ArgumentDescription argument = description.Arguments[0];

            Assert.AreEqual("DateTime", argument.ArgumentType);
            Assert.AreEqual("time", argument.ArgumentName);
        }

        [TestMethod]
        public void BuildMethodDescription_WhenXmlHasArgumentDescription_DescriptionIsIncluded()
        {
            MethodInfo method = _testClass.GetMethod("PublicStringMethodWithTwoArguments");

            var builder = new MethodDescriptionBuilder(_xmlSource);
            MethodDescription description = builder.BuildMethodDescription(method);

            ArgumentDescription argument = description.Arguments[0];

            Assert.IsTrue(argument.Description.Length > 0);
        }

        [TestMethod]
        public void BuildMethodDescription_WhenInvoked_NameIsCorrectlySet()
        {
            MethodInfo method = _testClass.GetMethod("PublicVoidMethodWithoutArguments");

            var builder = new MethodDescriptionBuilder(_xmlSource);
            MethodDescription description = builder.BuildMethodDescription(method);

            Assert.AreEqual(method.Name, description.Name);
        }

        [TestMethod]
        public void BuildMethodDescription_WhenXmlSourceContainsSummary_SummaryIsPopulated()
        {
            MethodInfo method = _testClass.GetMethod("PublicVoidMethodWithoutArguments");

            var builder = new MethodDescriptionBuilder(_xmlSource);
            MethodDescription description = builder.BuildMethodDescription(method);
            
            Assert.IsNotNull(description.Summary);
        }

        
    }
}
