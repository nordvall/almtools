﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALMTools.Test.Import
{
    public interface IResultParser
    {
        DateTime ExecutionTime { get; }
        int TotalTests {get;}
        int ExecutedTests {get;}
        int FailedTests {get;}
        int PassedTests {get;}
        int InconclusiveTests {get;}
    }
}
