using ALMTools.Documentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ALMTools.Documentation.Import
{
    /// <summary>
    /// Wraps XML document from Visual Studio/MSBuild and returns documentation element
    /// for types, methods and properties.
    /// </summary>
    public class MSBuildXmlParser
    {
        private XDocument _document;

        public MSBuildXmlParser(XDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            _document = document;
        }

        public string GetTypeSummary(Type type)
        {
            string xmlName = GetXmlTypeName(type);
            var element = GetMemberDescriptionElement(xmlName);
            return GetSummaryElementValue(element);
        }

        private static string GetXmlTypeName(Type type)
        {
            string typeName = string.Format("T:{0}", type.FullName);
            return typeName;
        }

        public string GetPropertySummary(PropertyInfo property)
        {
            string xmlPropertyName = GetXmlPropertyName(property);
            var element = GetMemberDescriptionElement(xmlPropertyName);
            string summary = GetSummaryElementValue(element);
            return summary;
        }


        private static string GetXmlPropertyName(PropertyInfo property)
        {
            string typeName = string.Format("P:{0}.{1}", property.DeclaringType.FullName, property.Name);
            return typeName;
        }

        public string GetMethodSummary(MethodInfo method)
        {
            string xmlName = GetXmlMethodName(method);
            var element = GetMemberDescriptionElement(xmlName);
            return GetSummaryElementValue(element);
        }

        private string GetSummaryElementValue(XElement element)
        {
            if (element != null)
            {
                XElement summaryElement = element.Element("summary");

                if (summaryElement != null)
                {
                    return summaryElement.Value.Trim();
                }
            }
            return null;
        }

        public string GetParamaterDescription(MethodInfo method, string parameterName)
        {
            string xmlMethodName = GetXmlMethodName(method);
            var descriptionElement = GetMemberDescriptionElement(xmlMethodName);

            XElement paramElement = descriptionElement.Elements("param").FirstOrDefault(p => p.Attribute("name").Value == parameterName);
            if (paramElement != null)
            {
                var description = paramElement.Value.Trim();
                return description;
            }
            else
            {
                return null;
            }
        }

        public string GetReturnValueDescription(MethodInfo method)
        {
            string xmlMethodName = GetXmlMethodName(method);
            var descriptionElement = GetMemberDescriptionElement(xmlMethodName);
            if (descriptionElement.Element("returns") != null)
            {
                return descriptionElement.Element("returns").Value.Trim();
            }
            else
            {
                return null;
            }
        }

        private static string GetXmlMethodName(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("M:{0}.{1}", method.DeclaringType.FullName, method.Name);

            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length > 0)
            {
                var parameterNames = parameters.Select(p => p.ParameterType.FullName);
                builder.AppendFormat("({0})", string.Join(",", parameterNames.ToArray()));
            }

            return builder.ToString();
        }

        

        private XElement GetMemberDescriptionElement(string xmlName)
        {
            XElement typeElement = _document.Descendants("member").FirstOrDefault(m => m.Attribute("name").Value == xmlName);
            return typeElement;
        }

        
    }
}
