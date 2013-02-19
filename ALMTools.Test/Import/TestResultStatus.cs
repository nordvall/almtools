using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALMTools.Test.Import
{
    public enum TestResultStatus
    {
        Unspecified = 0,
        None = 1,
        Passed = 2,
        Failed = 3,
        Inconclusive = 4,
        Timeout = 5,
        Aborted = 6,
        Blocked = 7,
        NotExecuted = 8,
        Warning = 9,
        Error = 10
    }
}
