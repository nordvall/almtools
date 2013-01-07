using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ALMTools.Build
{
    public class VersionAttributeParser
    {
        private Stream _fileStream;

        public VersionAttributeParser(Stream filestream)
        {
            if (filestream == null)
            {
                throw new ArgumentNullException("filestream");
            }

            _fileStream = filestream;
        }

        public Version GetVersion(string attributeName)
        {
            if (string.IsNullOrEmpty(attributeName))
            {
                throw new ArgumentNullException("attributeName");
            }

            string text = GetStreamContents();

            int attributePosition = text.IndexOf(attributeName);
            if (attributePosition >= 0)
            {
                Regex regex = new Regex(@"\(""\d+\.\d+\.\d+\.\d+""\)");
                Match match = regex.Match(text, attributePosition);

                if (match.Success)
                {
                    string versionString = match.Value.Trim('(', '"', ')');
                    Version version = new Version(versionString);
                    return version;
                }
            }

            return null;
        }

        public void SetVersion(string attributeName, Version version)
        {
            if (string.IsNullOrEmpty(attributeName)) 
            { 
                throw new ArgumentNullException("attributeName"); 
            }
            if (version == null) 
            { 
                throw new ArgumentNullException("version"); 
            }

            string text = GetStreamContents();

            Regex regex = new Regex(attributeName + @"\(""\d+\.\d+\.\d+\.\d+""\)");
            Match match = regex.Match(text);

            if (match.Success)
            {
                string newText = regex.Replace(text, attributeName + "(\"" + version + "\")");
                SetStreamContents(newText);
            }
            else
            {
                throw new ArgumentException("Attribute not found.");
            }
        }

        private void SetStreamContents(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            _fileStream.Position = 0;
            _fileStream.SetLength(bytes.Length);
            _fileStream.Write(bytes, 0, bytes.Length);
        }

        private string GetStreamContents()
        {
            _fileStream.Position = 0;

            var reader = new StreamReader(_fileStream);
            string text = reader.ReadToEnd();
            return text;
        }
    }
}
