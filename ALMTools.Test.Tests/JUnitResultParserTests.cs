using System;
using System.IO;
using System.Reflection;
using ALMTools.Test.Import;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ALMTools.Test.Tests
{
    [TestClass]
    public class JUnitResultParserTests
    {
        private Stream _stream;

        [TestInitialize]
        public void InitializeStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            _stream = assembly.GetManifestResourceStream("ALMTools.Test.Tests.Import.Xml.JUnit.xml");
        }

        [TestMethod]
        public void TestMethod1()
        {
            var parser = new JUnitResultParser(_stream);
        }
    }
}
