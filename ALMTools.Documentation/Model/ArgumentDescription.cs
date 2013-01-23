using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace ALMTools.Documentation.Model
{
    public class ArgumentDescription
    {
        public ArgumentDescription()
        {

        }

        [XmlAttribute]
        public string ArgumentName { get; set; }

        [XmlAttribute]
        public string ArgumentType { get; set; }

        [XmlText]
        public string Description { get; set; }
    }
}
