using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ALMTools.Documentation.Model
{
    public class MethodDescription : BaseDescription
    {
        public MethodDescription()
        {
            Arguments = new List<ArgumentDescription>();
        }
        
        public List<ArgumentDescription> Arguments { get; set; }

        public ReturnValueDescription ReturnValue { get; set; }
    }
}
