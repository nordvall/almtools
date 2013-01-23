using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ALMTools.Documentation.Model
{
    public class BaseDescription
    {
        
        [XmlAttribute]
        public string Name { get; set; }

        public string Summary { get; set; }


    }
}
