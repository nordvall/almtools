using ALMTools.Documentation.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using ALMTools.Documentation.Import;
using System.Reflection;

namespace ALMTools.Documentation
{
    public class DocGenerator
    {
        private ClassDescriptionBuilder _classDescriptionBuilder;
        private MethodDescriptionBuilder _methodDescriptionBuilder;
        private PropertyDescriptionBuilder _propertyDescriptionBuilder;
        private BindingFlags? _methodsToInclude = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        private BindingFlags? _propertiesToInclude = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        public DocGenerator(XDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            var xmlSource = new MSBuildXmlParser(document);
            _classDescriptionBuilder = new ClassDescriptionBuilder(xmlSource);
            _methodDescriptionBuilder = new MethodDescriptionBuilder(xmlSource);
            _propertyDescriptionBuilder = new PropertyDescriptionBuilder(xmlSource);
        }

        public ClassDescription Generate(Type type)
        {
            ClassDescription desc = _classDescriptionBuilder.BuildClassDescription(type);

            if (_methodsToInclude != null)
            {
                desc.PublicMethods = CreateMethodDescriptions(type);
            }

            if (_propertiesToInclude != null)
            {
                desc.Properties = CreatePropertyDescriptions(type);
            }

            return desc;

        }
        public XDocument GenerateXml(Type type)
        {
            ClassDescription desc = Generate(type);

            XmlSerializer serializer = new XmlSerializer(typeof(ClassDescription));

            XDocument result = null;

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, desc);
                stream.Flush();
                stream.Position = 0;

                XmlReader reader = XmlReader.Create(stream);
                result = XDocument.Load(reader);
            }

            return result;
        }

        public BindingFlags? MethodsToInclude
        {
            set { _methodsToInclude = value; }
        }

        public BindingFlags? PropertiesToInclude
        {
            set { _propertiesToInclude = value; }
        }

        private List<PropertyDescription> CreatePropertyDescriptions(Type type)
        {
            var properties = new List<PropertyDescription>();
            var allProperties = type.GetProperties((BindingFlags)_propertiesToInclude);

            foreach (var property in allProperties)
            {
                var propertyDesc = _propertyDescriptionBuilder.BuildPropertyDescription(property);
                properties.Add(propertyDesc);
            }

            return properties;
        }

        private List<MethodDescription> CreateMethodDescriptions(Type type)
        {
            var methods = new List<MethodDescription>();

            var allMethods = type.GetMethods((BindingFlags)_methodsToInclude);
            var methodsExceptPropertyGetAndSet = allMethods.Where(m => m.IsSpecialName == false);

            foreach (var method in methodsExceptPropertyGetAndSet)
            {
                var methodDesc = _methodDescriptionBuilder.BuildMethodDescription(method);
                methods.Add(methodDesc);
            }
             
            return methods;
        }
    }
}
