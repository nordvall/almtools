using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ALMTools.Documentation.Model
{
    public class ReturnValueDescription
    {
        public ReturnValueDescription()
        {

        }

        [XmlAttribute]
        public string TypeName { get; set; }

        [XmlText]
        public string Description { get; set; }
    }
}
