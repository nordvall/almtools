using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ALMTools.Documentation.Model
{
    public class ClassDescription : BaseDescription
    {
        public ClassDescription()
        {
            PublicMethods = new List<MethodDescription>();
            Properties = new List<PropertyDescription>();
        }

        
        [XmlAttribute]
        public string Namespace { get; set; }

        [XmlAttribute]
        public string AssemblyName { get; set; }

        [XmlAttribute]
        public string AssemblyVersion { get; set; }

        [XmlAttribute]
        public string AssemblyFileVersion { get; set; }

        public List<MethodDescription> PublicMethods { get; set; }

        public List<PropertyDescription> Properties { get; set; }

    }
}
