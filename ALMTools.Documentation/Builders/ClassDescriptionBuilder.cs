using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class ClassDescriptionBuilder
    {
        private MSBuildXmlParser _xmlParser;

        public ClassDescriptionBuilder(MSBuildXmlParser xmlSource)
        {
            if (xmlSource == null)
            {
                throw new ArgumentNullException("xmlSource");
            }

            _xmlParser = xmlSource;
        }

        public ClassDescription BuildClassDescription(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var description = new ClassDescription()
            {
                Name = type.Name,
                Namespace = type.Namespace
            };

            SetAssemblyProperties(description, type.Assembly);

            description.Summary = _xmlParser.GetTypeSummary(type);

            return description;
        }

        private void SetAssemblyProperties(ClassDescription description, Assembly assembly)
        {
            AssemblyName assemblyName = assembly.GetName();
            description.AssemblyName = assemblyName.Name;
            description.AssemblyVersion = assemblyName.Version.ToString();
            
            FileVersionInfo version = FileVersionInfo.GetVersionInfo(assembly.Location);
            description.AssemblyFileVersion = version.FileVersion;
        }

        
    }
}
