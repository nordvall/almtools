using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ALMTools.Test.Import;

namespace ALMTools.Test.Export
{
    public class TestCaseTransformer
    {
        private const string vsTestAdapterType = "Microsoft.VisualStudio.TestTools.TestTypes.Unit.UnitTestAdapter, Microsoft.VisualStudio.QualityTools.Tips.UnitTest.Adapter, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
        private readonly Guid defaultTestListId = new Guid("8c84fa94-04c1-424b-9868-57a2d4851a1d");
        private readonly Guid unitTestType = new Guid("13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b");
        
        public void InsertTestCase(TestCaseResult testCase, XDocument trxDocument)
        {
            Guid testguid = TrxHelper.GuidFromString(string.Format("{0}.{1}", testCase.ModuleName, testCase.TestCaseName));
            Guid executionId = Guid.NewGuid();

            InsertTestDefinition(testCase, trxDocument, testguid, executionId);
            InsertTestEntry(trxDocument, testguid, executionId);
            InsertTestResult(testCase, trxDocument, testguid, executionId);
        }

        private void InsertTestResult(TestCaseResult testCase, XDocument trxDocument, Guid testguid, Guid executionId)
        {
            var unitTestResultElement = new XElement(TrxHelper.XmlNamespace + "UnitTestResult",
                new XAttribute("executionId", executionId),
                new XAttribute("testId", testguid),
                new XAttribute("testName", testCase.TestCaseName),
                new XAttribute("computerName", ""),
                new XAttribute("testListId", defaultTestListId),
                new XAttribute("testType", unitTestType));

            unitTestResultElement.Add(new XAttribute("duration", testCase.Duration));
            //unitTestResultElement.Add(new XAttribute("startTime", runTime));
            //runTime = runTime + testCase.Duration;
            //unitTestResultElement.Add(new XAttribute("endTime", runTime));

            unitTestResultElement.Add(new XAttribute("outcome", testCase.Result));

            var outputElement = new XElement(TrxHelper.XmlNamespace + "Output");
            unitTestResultElement.Add(outputElement);

            if (string.IsNullOrEmpty(testCase.Message) == false)
            {
                var messageElement = new XElement(TrxHelper.XmlNamespace + "TextMessages",
                        new XElement(TrxHelper.XmlNamespace + "Message", testCase.Message));

                outputElement.Add(messageElement);
            }

            if (string.IsNullOrEmpty(testCase.StackTrace) == false)
            {
                var messageElement = new XElement(TrxHelper.XmlNamespace + "ErrorInfo",
                        new XElement(TrxHelper.XmlNamespace + "StackTrace", testCase.StackTrace));

                outputElement.Add(messageElement);
            }

            var resultsNode = trxDocument.Root.Element(TrxHelper.XmlNamespace + "Results");
            resultsNode.Add(unitTestResultElement);
        }

        private void InsertTestEntry(XDocument trxDocument, Guid testguid, Guid executionId)
        {
            var testEntryElement = new XElement(TrxHelper.XmlNamespace + "TestEntry",
                new XAttribute("testId", testguid),
                new XAttribute("executionId", executionId),
                new XAttribute("testListId", defaultTestListId));

            var testEntriesNode = trxDocument.Root.Element(TrxHelper.XmlNamespace + "TestEntries");
            testEntriesNode.Add(testEntryElement);
        }

        private static void InsertTestDefinition(TestCaseResult testCase, XDocument trxDocument, Guid testguid, Guid executionId)
        {
            string classNameWithAssembly = string.Format("{0}, {1}", testCase.ModuleName, testCase.TestSuiteName);

            var unitTestElement = new XElement(TrxHelper.XmlNamespace + "UnitTest",
                new XAttribute("id", testguid),
                new XAttribute("name", string.Format("{0}: {1}", testCase.ModuleName, testCase.TestCaseName)),
                new XElement(TrxHelper.XmlNamespace + "Execution",
                    new XAttribute("id", executionId)),
                new XElement(TrxHelper.XmlNamespace + "TestMethod",
                    new XAttribute("adapterTypeName", vsTestAdapterType),
                    new XAttribute("className", classNameWithAssembly),
                    new XAttribute("codeBase", testCase.TestSuiteName),
                    new XAttribute("name", testCase.TestCaseName)));

            var testDefinitionsNode = trxDocument.Root.Element(TrxHelper.XmlNamespace + "TestDefinitions");
            testDefinitionsNode.Add(unitTestElement);
        }
    }
}
