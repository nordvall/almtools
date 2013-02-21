using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace ALMTools.Test.Export
{
    public static class TrxHelper
    {
        public static XNamespace XmlNamespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
        
        public static Guid GuidFromString(string data)
        {
            var provider = new SHA1CryptoServiceProvider();
            byte[] hash = provider.ComputeHash(System.Text.Encoding.Unicode.GetBytes(data));

            byte[] toGuid = new byte[16];
            Array.Copy(hash, toGuid, 16);

            return new Guid(toGuid);
        }
    }
}
