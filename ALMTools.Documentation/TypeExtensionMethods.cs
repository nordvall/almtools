using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ALMTools.Documentation
{
    public static class TypeExtensionMethods
    {
        /// <summary>
        /// Translates List`1 to more readable form
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFriendlyTypeName(this Type type)
        {
            if (type.IsGenericType == false)
            {
                return type.Name;
            }
            else
            {
                string outerPart = type.Name.Substring(0, type.Name.IndexOf('`'));
                Type[] genericTypeArguments = type.GetGenericArguments();
                string[] innerParts = new string[genericTypeArguments.Length];
                for (int i=0; i < genericTypeArguments.Length; i++)
                {
                    // Recursive. InnerPart could also be generic.
                    innerParts[i] = genericTypeArguments[i].GetFriendlyTypeName();
                }
                
                return string.Format("{0}<{1}>", outerPart, string.Join(",", innerParts));
            }

        }
    }
}
