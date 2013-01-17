﻿using System;
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

        public string TestName
        {
            get { return Path.GetFileNameWithoutExtension(_nativeResult.name); }
        }

        public string ComputerName
        {
            get { return _nativeResult.hostname; }
        }

        public string UserName
        {
            get { return Environment.UserName; }
        }

        public int TotalTests
        {
            get { return _nativeResult.tests; }
        }

        public int ExecutedTests
        {
            get { return _nativeResult.tests; }
        }

        public int FailedTests
        {
            get { return _nativeResult.failures; }
        }

        public int PassedTests
        {
            get { return (_nativeResult.tests - _nativeResult.failures); }
        }

        public int InconclusiveTests
        {
            get { return 0; }
        }

        public DateTime ExecutionTime
        {
            get 
            {
                if (_nativeResult.timestamp == DateTime.MinValue)
                {
                    return DateTime.Now;
                }
                else
                {
                    return _nativeResult.timestamp;
                }
            }
        }

        public TimeSpan Duration
        {
            get 
            { 
                int milliseconds = Convert.ToInt32(_nativeResult.time);
                var timeSpan = new TimeSpan(0, 0, 0, 0, milliseconds);
                return timeSpan;
            }
        }


        
    }
}
