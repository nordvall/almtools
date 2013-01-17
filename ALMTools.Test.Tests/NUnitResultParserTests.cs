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
        public void ComputerName_WhenNativeFileDeclaresMachineName_SameValueIsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            Assert.AreEqual("my-machine", parser.ComputerName);
        }

        [TestMethod]
        public void TestName_WhenNativeFileDeclaresTestAssembly_ValueWithoutPathAndSuffixIsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            Assert.AreEqual("NunitDemo", parser.TestName);
        }

        [TestMethod]
        public void UserName_WhenNativeFileDeclaresUserName_SameValueIsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            Assert.AreEqual("matnor", parser.UserName);
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

        [TestMethod]
        public void FailedTests_WhenFailuresIs2AndErrorsIs0_2IsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            Assert.AreEqual(2, parser.FailedTests);
        }

        [TestMethod]
        public void ExecutedTests_WhenTotalIs9_9IsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            Assert.AreEqual(9, parser.ExecutedTests);
        }

        [TestMethod]
        public void InconclusiveTests_WhenInconclusiveIs5_5TimeIsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            Assert.AreEqual(5, parser.InconclusiveTests);
        }

        [TestMethod]
        public void Duration_WhenTimeIs0403_CorrectTimeSpanIsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            var expectedTime = new TimeSpan(0, 0, 0, 0, 403);
            Assert.AreEqual(expectedTime, parser.Duration);
        }

        [TestMethod]
        public void Result_WhenStatusIsFailed_FailureIsReturned()
        {
            var parser = new NUnitResultParser(_stream);
            Assert.AreEqual(ResultStatus.Failed, parser.Result);
        }
    }
}
