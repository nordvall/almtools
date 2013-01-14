using System;
using System.IO;
using System.Reflection;
using ALMTools.Test.Import;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ALMTools.Test.Tests
{
    [TestClass]
    public class NUnitResultParserTests
    {
        private Stream _stream;

        [TestInitialize]
        public void InitializeStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            _stream = assembly.GetManifestResourceStream("ALMTools.Test.Tests.Import.Xml.NUnit.xml");
        }

        [TestMethod]
        public void TotalTests_WhenNativeFileDeclares8_8IsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            Assert.AreEqual(10, parser.TotalTests);
        }

        [TestMethod]
        public void PassedTests_WhenNativeFileDeclares6totalAnd4failed_2IsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            Assert.AreEqual(2, parser.PassedTests);
        }

        [TestMethod]
        public void ExecutionTime_WhenDateAndTimeIsSet_CorrectDateTimeIsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            var expectedDate = new DateTime(2013, 1, 14, 15, 49, 34);
            Assert.AreEqual(expectedDate, parser.ExecutionTime);
        }
    }
}
