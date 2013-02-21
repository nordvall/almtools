using ALMTools.Test.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace ALMTools.Test.Export
{
    public class TrxGenerator
    {
        private static XNamespace ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
        private static string vsTestAdapterType = "Microsoft.VisualStudio.TestTools.TestTypes.Unit.UnitTestAdapter, Microsoft.VisualStudio.QualityTools.Tips.UnitTest.Adapter, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
        private static Guid defaultTestListId = new Guid("8c84fa94-04c1-424b-9868-57a2d4851a1d");
        private static Guid unitTestType = new Guid("13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b");
        
        private IResultParser _inputFile;

        public TrxGenerator()
        {
        }

        public void Clear()
        {
            _inputFile = null;
        }

        public void AddInputFile(IResultParser result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _inputFile = result;
        }

        public XDocument Generate()
        {
            XDocument document = GetBaseTrxDocument();
            InsertEnvironmentInfo(document);
            InsertRunTimes(document);
            InsertResultSummary(document);
            InsertTestCases(document);
            return document;
        }

        private void InsertEnvironmentInfo(XDocument document)
        {
            throw new NotImplementedException();
        }

        private static XDocument GetBaseTrxDocument()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream trxStream = assembly.GetManifestResourceStream("ALMTools.Test.Export.BaseTrx.xml");
            XDocument document = XDocument.Load(trxStream);
            return document;
        }

        public static Guid GuidFromString(string data)
        {
            var provider = new SHA1CryptoServiceProvider();
            byte[] hash = provider.ComputeHash(System.Text.Encoding.Unicode.GetBytes(data));

            byte[] toGuid = new byte[16];
            Array.Copy(hash, toGuid, 16);

            return new Guid(toGuid);
        }

        private void InsertResultSummary(XDocument document)
        {
            var resultSummaryElement = document.Root.Element(ns + "ResultSummary");
            resultSummaryElement.SetAttributeValue("outcome", _inputFile.Result.ToString());

            var notExecutedTests = _inputFile.TotalTests - _inputFile.ExecutedTests;

            var countersElement = resultSummaryElement.Element(ns + "Counters");
            countersElement.SetAttributeValue("total", _inputFile.TotalTests);
            countersElement.SetAttributeValue("notExecuted", notExecutedTests);
            countersElement.SetAttributeValue("failed", _inputFile.FailedTests);
            countersElement.SetAttributeValue("inconclusive", _inputFile.InconclusiveTests);
            countersElement.SetAttributeValue("executed", _inputFile.ExecutedTests);
            countersElement.SetAttributeValue("passed", _inputFile.PassedTests);
            countersElement.SetAttributeValue("completed", _inputFile.ExecutedTests);
        }

        private void InsertRunTimes(XDocument document)
        {
            var timesElement = document.Root.Element(ns + "Times");
            timesElement.SetAttributeValue("creation", _inputFile.ExecutionTime);
            timesElement.SetAttributeValue("queuing", _inputFile.ExecutionTime);
            timesElement.SetAttributeValue("start", _inputFile.ExecutionTime);
            timesElement.SetAttributeValue("finish", _inputFile.ExecutionTime + _inputFile.Duration);
        }

        private void InsertTestCases(XDocument document)
        {
            DateTime runTime = _inputFile.ExecutionTime;

            foreach (var testCase in _inputFile.TestCases)
            {
                string classNameWithAssembly = string.Format("{0}, {1}", testCase.ModuleName, testCase.TestSuiteName);

                Guid testguid = GuidFromString(string.Format("{0}.{1}", testCase.ModuleName, testCase.TestCaseName));
                Guid executionId = Guid.NewGuid();

                var unitTestElement = new XElement(ns + "UnitTest",
                    new XAttribute("id", testguid),
                    new XAttribute("name", string.Format("{0}: {1}", testCase.ModuleName, testCase.TestCaseName)),
                    new XElement(ns + "Execution",
                        new XAttribute("id", executionId)),
                    new XElement(ns + "TestMethod",
                        new XAttribute("adapterTypeName", vsTestAdapterType),
                        new XAttribute("className", classNameWithAssembly),
                        new XAttribute("codeBase", testCase.TestSuiteName),
                        new XAttribute("name", testCase.TestCaseName)));

                var testDefinitionsNode = document.Root.Element(ns + "TestDefinitions");
                testDefinitionsNode.Add(unitTestElement);

                var testEntryElement = new XElement(ns + "TestEntry",
                    new XAttribute("testId", testguid),
                    new XAttribute("executionId", executionId),
                    new XAttribute("testListId", defaultTestListId));

                var testEntriesNode = document.Root.Element(ns + "TestEntries");
                testEntriesNode.Add(testEntryElement);

                var unitTestResultElement = new XElement(ns + "UnitTestResult",
                    new XAttribute("executionId", executionId),
                    new XAttribute("testId", testguid),
                    new XAttribute("testName", testCase.TestCaseName),
                    new XAttribute("computerName", ""),
                    new XAttribute("testListId", defaultTestListId),
                    new XAttribute("testType", unitTestType));

                unitTestResultElement.Add(new XAttribute("duration", testCase.Duration));
                unitTestResultElement.Add(new XAttribute("startTime", runTime));
                runTime = runTime + testCase.Duration;
                unitTestResultElement.Add(new XAttribute("endTime", runTime));

                unitTestResultElement.Add(new XAttribute("outcome", testCase.Result));

                var outputElement = new XElement(ns + "Output");
                unitTestResultElement.Add(outputElement);

                if (string.IsNullOrEmpty(testCase.Message) == false)
                {
                    var messageElement = new XElement(ns + "TextMessages",
                            new XElement(ns + "Message", testCase.Message));

                    outputElement.Add(messageElement);
                }

                if (string.IsNullOrEmpty(testCase.StackTrace) == false)
                {
                    var messageElement = new XElement(ns + "ErrorInfo",
                            new XElement(ns + "StackTrace", testCase.StackTrace));

                    outputElement.Add(messageElement);
                }

                var resultsNode = document.Root.Element(ns + "Results");
                resultsNode.Add(unitTestResultElement);
            }
        }
    }
}
