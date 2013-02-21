using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ALMTools.Test.Import;

namespace ALMTools.Test.Export
{
    public class BasicDataTransformer
    {
        public void InsertEnvironmentInfo(IResultParser inputFile, XDocument document)
        {
            string fullTestName = string.Format("{0}@{1} {2}", inputFile.TestName, inputFile.ComputerName, inputFile.ExecutionTime);
            var testRunId = TrxHelper.GuidFromString(fullTestName);

            var rootElement = document.Root;
            rootElement.SetAttributeValue("id", testRunId);
            rootElement.SetAttributeValue("runUser", inputFile.UserName);
            rootElement.SetAttributeValue("name", fullTestName);

            var deploymentElement = document.Root
                .Element(TrxHelper.XmlNamespace + "TestSettings")
                .Element(TrxHelper.XmlNamespace + "Deployment");
            deploymentElement.SetAttributeValue("runDeploymentRoot", testRunId);
        }

        public void InsertResultSummary(IResultParser inputFile, XDocument document)
        {
            var resultSummaryElement = document.Root.Element(TrxHelper.XmlNamespace + "ResultSummary");
            resultSummaryElement.SetAttributeValue("outcome", inputFile.Result.ToString());

            var notExecutedTests = inputFile.TotalTests - inputFile.ExecutedTests;

            var countersElement = resultSummaryElement.Element(TrxHelper.XmlNamespace + "Counters");
            countersElement.SetAttributeValue("total", inputFile.TotalTests);
            countersElement.SetAttributeValue("notExecuted", notExecutedTests);
            countersElement.SetAttributeValue("failed", inputFile.FailedTests);
            countersElement.SetAttributeValue("inconclusive", inputFile.InconclusiveTests);
            countersElement.SetAttributeValue("executed", inputFile.ExecutedTests);
            countersElement.SetAttributeValue("passed", inputFile.PassedTests);
            countersElement.SetAttributeValue("completed", inputFile.ExecutedTests);
        }

        public void InsertRunTimes(IResultParser inputFile, XDocument document)
        {
            var timesElement = document.Root.Element(TrxHelper.XmlNamespace + "Times");
            timesElement.SetAttributeValue("creation", inputFile.ExecutionTime);
            timesElement.SetAttributeValue("queuing", inputFile.ExecutionTime);
            timesElement.SetAttributeValue("start", inputFile.ExecutionTime);
            timesElement.SetAttributeValue("finish", inputFile.ExecutionTime + inputFile.Duration);
        }
    }
}
