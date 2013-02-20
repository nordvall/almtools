using ALMTools.Test.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ALMTools.Test.Export
{
    public class TrxGenerator
    {
        private List<IResultParser> _resultFiles;

        public TrxGenerator()
        {
            Clear();
        }

        public void Clear()
        {
            _resultFiles = new List<IResultParser>();
        }

        public void AddInputFile(IResultParser result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _resultFiles.Add(result);
        }

        public XDocument Generate()
        {
            XDocument document = GetBaseTrxDocument();

            return document;
        }

        private static XDocument GetBaseTrxDocument()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream trxStream = assembly.GetManifestResourceStream("ALMTools.Test.Export.BaseTrx.xml");
            XDocument document = XDocument.Load(trxStream);
            return document;
        }
    }
}
