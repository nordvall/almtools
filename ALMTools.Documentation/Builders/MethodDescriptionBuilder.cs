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
    public class MethodDescriptionBuilder
    {
        private MSBuildXmlParser _xmlParser;

        public MethodDescriptionBuilder(MSBuildXmlParser xmlSource)
        {
            if (xmlSource == null)
            {
                throw new ArgumentNullException("xmlSource");
            }

            _xmlParser = xmlSource;
        }

        
        public MethodDescription BuildMethodDescription(MethodInfo method)
        {
            var methodDesc = new MethodDescription()
            {
                Name = method.Name
            };

            methodDesc.Summary = _xmlParser.GetMethodSummary(method);

            methodDesc.ReturnValue = BuildReturnValueDescription(method);
            methodDesc.Arguments = BuildArgumentDescriptions(method);

            return methodDesc;
        }


        private ReturnValueDescription BuildReturnValueDescription(MethodInfo method)
        {
            if (method.ReturnType == typeof(void))
            {
                return null;
            }

            var returnValue = new ReturnValueDescription();
            returnValue.TypeName = method.ReturnType.GetFriendlyTypeName();
            returnValue.Description = _xmlParser.GetReturnValueDescription(method);

            return returnValue;
        }

        private List<ArgumentDescription> BuildArgumentDescriptions(MethodInfo method)
        {
            var arguments = new List<ArgumentDescription>();
            var parameters = method.GetParameters();

            foreach (ParameterInfo parameter in parameters)
            {
                var argument = BuildArgumentDescription(method, parameter);
                arguments.Add(argument);
            }

            return arguments;
        }

        private ArgumentDescription BuildArgumentDescription(MethodInfo method, ParameterInfo parameter)
        {
            var description = new ArgumentDescription()
            {
                ArgumentName = parameter.Name,
                ArgumentType = parameter.ParameterType.GetFriendlyTypeName()
            };

            description.Description = _xmlParser.GetParamaterDescription(method, parameter.Name);

            return description;
        }

    }
}
