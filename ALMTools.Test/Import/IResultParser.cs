using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ALMTools.Test.Import
{
    public interface IResultParser
    {
        DateTime ExecutionTime { get; }
        TimeSpan Duration { get; }
        string TestName { get; }
        string ComputerName { get; }
        string UserName { get; }
        int TotalTests { get; }
        int ExecutedTests { get; }
        int FailedTests { get; }
        int PassedTests { get; }
        int InconclusiveTests { get; }
        TestResultStatus Result { get; }
        ReadOnlyCollection<TestCaseResult> TestCases { get; }
    }
}
