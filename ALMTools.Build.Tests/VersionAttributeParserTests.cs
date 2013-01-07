using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ALMTools.Build.Tests
{
    [TestClass]
    public class VersionAttributeParserTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WhenInvokenWithNull_ArgumentNullExceptionIsThrown()
        {
            var parser = new VersionAttributeParser(null);
        }
        
        [TestMethod]
        public void GetVersion_WhenStreamContainsCorrectAttribute_VersionIsReturned()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.WriteLine("[assembly: AssemblyProduct(\"AnyProduct\")]");
            writer.WriteLine("[assembly: AssemblyVersion(\"1.0.0.0\")]");
            writer.WriteLine("[assembly: AssemblyFileVersion(\"2.0.0.0\")]");
            writer.Flush();

            VersionAttributeParser parser = new VersionAttributeParser(stream);

            Version version = parser.GetVersion("AssemblyVersion");
            Assert.AreEqual(new Version(1, 0, 0, 0), version);

            Version fileVersion = parser.GetVersion("AssemblyFileVersion");
            Assert.AreEqual(new Version(2, 0, 0, 0), fileVersion);

        }

        [TestMethod]
        public void GetVersion_WhenStreamDoesNotContainAttribute_NullIsReturned()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.WriteLine("[assembly: AssemblyProduct(\"AnyProduct\")]");
            writer.Flush();

            VersionAttributeParser parser = new VersionAttributeParser(stream);
            Version version = parser.GetVersion("AssemblyVersion");

            Assert.IsNull(version);

        }

        [TestMethod]
        public void SetVersion_WhenStreamContainsCorrectAttribute_VersionIsUpdated()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.WriteLine("[assembly: AssemblyProduct(\"AnyProduct\")]");
            writer.WriteLine("[assembly: AssemblyVersion(\"1.0.0.0\")]");
            writer.WriteLine("[assembly: AssemblyFileVersion(\"2.0.0.0\")]");
            writer.Flush();

            VersionAttributeParser parser = new VersionAttributeParser(stream);

            Version newVersion = new Version(1, 1, 0, 0);
            parser.SetVersion("AssemblyVersion", newVersion);

            Version updatedVersion = parser.GetVersion("AssemblyVersion");

            Assert.AreEqual(newVersion, updatedVersion);

        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void SetVersion_WhenStreamDoesNotContainAttribute_ArgumentExceptionIsThrown()
        {
            MemoryStream stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.WriteLine("[assembly: AssemblyProduct(\"AnyProduct\")]");
            writer.Flush();

            VersionAttributeParser parser = new VersionAttributeParser(stream);

            Version newVersion = new Version(1, 1, 0, 0);
            parser.SetVersion("AssemblyVersion", newVersion);
        }

        [TestMethod]
        public void SetVersion_WhenUpdatingToShorterVersion_StreamLengthIsReduced()
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.WriteLine("[assembly: AssemblyVersion(\"1.0.0.255\")]");
            writer.Flush();

            long oldLength = stream.Length;

            VersionAttributeParser parser = new VersionAttributeParser(stream);

            Version newVersion = new Version(1, 1, 0, 0);
            parser.SetVersion("AssemblyVersion", newVersion);

            long newLength = stream.Length;

            Assert.IsTrue(newLength < oldLength);
        }
    }
}
