﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ALMTools.Test.Import
{
    public class NUnitResultParser : IResultParser
    {
        private resultType _nativeResult;

        public NUnitResultParser(XmlDocument document)
        {
            DeserializeDocument(document);
        }

        public NUnitResultParser(Stream stream)
        {
            DeserializeStream(stream);
        }

        private void DeserializeStream(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(resultType));
            _nativeResult = (resultType)serializer.Deserialize(stream);
        }

        private void DeserializeDocument(XmlDocument document)
        {
            var reader = new XmlNodeReader(document.DocumentElement);
            var serializer = new XmlSerializer(typeof(resultType));
            _nativeResult = (resultType)serializer.Deserialize(reader);
        }

        public string TestName 
        {
            get { return Path.GetFileNameWithoutExtension(_nativeResult.name); }
        }

        public string ComputerName 
        {
            get { return _nativeResult.environment.machinename; }
        }

        public string UserName 
        {
            get { return _nativeResult.environment.user; }
        }

        public int TotalTests
        {
            get { return Convert.ToInt32(_nativeResult.total + _nativeResult.notrun); }
        }

        public int ExecutedTests
        {
            get 
            {
                decimal executedTests = _nativeResult.total;
                return Convert.ToInt32(executedTests); 
            }
        }

        public int FailedTests
        {
            get 
            {
                decimal failedTests = _nativeResult.failures + _nativeResult.errors;
                return Convert.ToInt32(failedTests);
            }
        }

        public int PassedTests
        {
            get
            {
                decimal notPassedTests = (_nativeResult.inconclusive + _nativeResult.failures);
                decimal passedTests = _nativeResult.total - notPassedTests;

                return Convert.ToInt32(passedTests);
            }
        }

        public int InconclusiveTests
        {
            get { return Convert.ToInt32(_nativeResult.inconclusive); }
        }

        public DateTime ExecutionTime
        {
            get 
            {  
                string datetimestring = string.Format("{0} {1}", _nativeResult.date, _nativeResult.time);
                var date = DateTime.Parse(datetimestring);
                return date;
            }
        }


        public TimeSpan Duration
        {
            get 
            {
                string timeString = _nativeResult.testsuite.time;
                string[] timeParts = timeString.Split('.');
                int seconds = Convert.ToInt32(timeParts[0]);
                int milliseconds = 0;
                if (timeParts.Length > 1)
                {
                    milliseconds = Convert.ToInt32(timeParts[1]);
                }

                var timeSpan = new TimeSpan(0, 0, 0, seconds, milliseconds);
                return timeSpan;
            }
        }
    }
}
