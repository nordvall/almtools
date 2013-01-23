using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace ALMTools.Documentation.Model
{
    public class PropertyDescription : BaseDescription
    {
        public PropertyDescription()
        {

        }

        [XmlAttribute]
        public string TypeName { get; set; }
    }
}
