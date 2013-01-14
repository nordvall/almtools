using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ALMTools.Test.Import
{
    public class JUnitResultParser : IResultParser
    {
        private testsuite _nativeResult;

        public JUnitResultParser(XmlDocument document)
        {
            DeserializeDocument(document);
        }

        public JUnitResultParser(Stream stream)
        {
            DeserializeStream(stream);
        }

        private void DeserializeStream(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(testsuites));
            testsuites suites = (testsuites)serializer.Deserialize(stream);
            _nativeResult = suites.testsuite[0];
        }

        private void DeserializeDocument(XmlDocument document)
        {
            var reader = new XmlNodeReader(document.DocumentElement);
            var serializer = new XmlSerializer(typeof(resultType));
            testsuites suites = (testsuites)serializer.Deserialize(reader);
            _nativeResult = suites.testsuite[0];
        }

        public int TotalTests
        {
            get { throw new NotImplementedException(); }
        }

        public int ExecutedTests
        {
            get { throw new NotImplementedException(); }
        }

        public int FailedTests
        {
            get { throw new NotImplementedException(); }
        }

        public int PassedTests
        {
            get { throw new NotImplementedException(); }
        }

        public int InconclusiveTests
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime ExecutionTime
        {
            get { throw new NotImplementedException(); }
        }
    }
}
