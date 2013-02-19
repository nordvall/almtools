using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public TestResultStatus Result
        {
            get
            {
                return ConvertNUnitResultStatus(_nativeResult.testsuite.result);              
            }
        }

        private TestResultStatus ConvertNUnitResultStatus(string result)
        {
            switch (result)
            {
                case "Failure":
                    return TestResultStatus.Failed;
                case "Success":
                    return TestResultStatus.Passed;
                case "Inconclusive":
                    return TestResultStatus.Inconclusive;
                case "Error":
                    return TestResultStatus.Failed;
                case "Ignored":
                    return TestResultStatus.NotExecuted;
                default:
                    return TestResultStatus.None;
            }
        }


        public ReadOnlyCollection<TestCaseResult> TestCases
        {
            get 
            {
                var cases = ReadTestCases(_nativeResult.testsuite);
                return new ReadOnlyCollection<TestCaseResult>(cases);
            }
        }

        private List<TestCaseResult> ReadTestCases(testsuiteType suite)
        {
            if (suite.results == null)
                return null;

            var results = new List<TestCaseResult>();
            
            foreach (object obj in suite.results.Items)
            {
                if (obj is testsuiteType)
                {
                    var nextLevelResults = ReadTestCases(obj as testsuiteType);
                    results.AddRange(nextLevelResults);
                }
                else if (obj is testcaseType)
                {
                    TestCaseResult result = ParseTest(obj as testcaseType);
                    results.Add(result);
                }
            }

            return results;
        }

        private TestCaseResult ParseTest(testcaseType test)
        {
            var result = new TestCaseResult();

            string fullName = test.name;
            result.TestCaseName = test.name.Substring(fullName.LastIndexOf('.') + 1);
            result.ModuleName = test.name.Substring(0, fullName.LastIndexOf('.'));

            if (bool.Parse(test.executed) == true)
            {
                if (string.IsNullOrEmpty(test.time) == false)
                {
                    string[] time = test.time.Split('.');
                    result.Duration = new TimeSpan(0, 0, 0, int.Parse(time[0]), int.Parse(time[1]));
                }
            }

            result.Result = ConvertNUnitResultStatus(test.result);

            if (test.Item is reasonType)
            {
                var reason = test.Item as reasonType;
                result.Message = reason.message;
            }

            if (test.Item is failureType)
            {
                var failure = test.Item as failureType;
                result.Message = failure.message;
                result.StackTrace = failure.stacktrace;
            }

            return result;
        }
    }
}
