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
        public void ComputerName_WhenNativeFileDoesNotDeclareMachineName_EmptyValueIsReturned()
        {
            var parser = new JUnitResultParser(_stream);
            Assert.AreEqual(null, parser.ComputerName);
        }

        [TestMethod]
        public void TestName_WhenNativeFileDeclaresTestPath_FilenameWithoutPathAndSuffixIsReturned()
        {
            var parser = new JUnitResultParser(_stream);
            Assert.AreEqual("qunitTests", parser.TestName);
        }

        [TestMethod]
        public void UserName_CurrentUserIsReturned()
        {
            var parser = new JUnitResultParser(_stream);
            Assert.AreEqual(Environment.UserName, parser.UserName);
        }

        [TestMethod]
        public void TotalTests_WhenNativeFileDeclares6_6IsReturned()
        {
            var parser = new JUnitResultParser(_stream);
            Assert.AreEqual(6, parser.TotalTests);
        }

        [TestMethod]
        public void ExecutedTests_WhenTestsIs6_6IsReturned()
        {
            var parser = new JUnitResultParser(_stream);
            Assert.AreEqual(6, parser.ExecutedTests);
        }

        [TestMethod]
        public void PassedTests_WhenNativeFileDeclares6totalAnd2failed_4IsReturned()
        {
            var parser = new JUnitResultParser(_stream);
            Assert.AreEqual(4, parser.PassedTests);
        }

        [TestMethod]
        public void ExecutionTime_WhenDateAndTimeIsNotSet_CurrentDateTimeIsReturned()
        {
            var parser = new JUnitResultParser(_stream);
            
            // Assert DayOfYear because DateTime.Now in itself is difficult.
            Assert.AreEqual(DateTime.Now.DayOfYear, parser.ExecutionTime.DayOfYear);
        }

        [TestMethod]
        public void FailedTests_WhenFailuresIs2_2IsReturned()
        {
            var parser = new JUnitResultParser(_stream);
            
            Assert.AreEqual(2, parser.FailedTests);
        }

        [TestMethod]
        public void Duration_WhenTimeIs76_CorrectTimeSpanIsReturned()
        {
            var parser = new JUnitResultParser(_stream);
            var expectedTime = new TimeSpan(0, 0, 0, 0, 76);
            Assert.AreEqual(expectedTime, parser.Duration);
        }

        [TestMethod]
        public void Result_WhenErrorsExist_FailedIsReturned()
        {
            var parser = new JUnitResultParser(_stream);
            Assert.AreEqual(TestResultStatus.Failed, parser.Result);
        }

        [TestMethod]
        public void TestCases_WhenNativeFileContains5tests_5testcasesAreReturned()
        {
            var parser = new JUnitResultParser(_stream);
            var result = parser.TestCases;
            Assert.AreEqual(6, result.Count);
        }
    }
}
