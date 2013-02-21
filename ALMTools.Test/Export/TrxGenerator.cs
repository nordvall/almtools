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
        
        public XDocument Generate(IResultParser inputFile)
        {
            XDocument trxDocument = GetBaseTrxDocument();

            var basicTransformer = new BasicDataTransformer();
            basicTransformer.InsertEnvironmentInfo(inputFile, trxDocument);
            basicTransformer.InsertRunTimes(inputFile, trxDocument);
            basicTransformer.InsertResultSummary(inputFile, trxDocument);

            var testCaseTransformer = new TestCaseTransformer();
            foreach (var testCase in inputFile.TestCases)
            {
                testCaseTransformer.InsertTestCase(testCase, trxDocument);
            }

            return trxDocument;
        }

        private static XDocument GetBaseTrxDocument()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream trxStream = assembly.GetManifestResourceStream("ALMTools.Test.Export.BaseTrx.xml");
            XDocument document = XDocument.Load(trxStream);
            return document;
        }
    }
}
