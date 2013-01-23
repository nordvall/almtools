using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using ALMTools.Documentation.Import;
using ALMTools.Documentation.Model;

namespace ALMTools.Documentation
{
    /// <summary>
    /// Creates description objects and adds information from xml documentation file.
    /// </summary>
    public class PropertyDescriptionBuilder
    {
        private MSBuildXmlParser _xmlParser;

        public PropertyDescriptionBuilder(MSBuildXmlParser xmlSource)
        {
            if (xmlSource == null)
            {
                throw new ArgumentNullException("xmlSource");
            }

            _xmlParser = xmlSource;
        }

        
        public PropertyDescription BuildPropertyDescription(PropertyInfo property)
        {
            var propertyDesc = new PropertyDescription()
            {
                Name = property.Name,
                TypeName = Utilities.GetFriendlyTypeName(property.PropertyType)
            };

            propertyDesc.Summary = _xmlParser.GetPropertySummary(property);

            return propertyDesc;
        }
    }
}
