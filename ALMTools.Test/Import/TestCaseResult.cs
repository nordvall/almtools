using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALMTools.Test.Import
{
    public class TestCaseResult
    {
        /// <summary>
        /// Method name
        /// </summary>
        public string TestCaseName { get; set; }

        /// <summary>
        /// Test class (.NET) or Module (QUnit)
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Assembly name (.NET) or test file (JS)
        /// </summary>
        public string TestSuiteName { get; set; }

        public TimeSpan Duration { get; set; }
        public TestResultStatus Result { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
