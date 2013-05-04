using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ALMTools.Build.Tests
{
    [TestClass]
    public class HtmlCleanerTests
    {
        [TestMethod]
        public void Clean_WhenCalledWithHtmlWithOneComment_CommentIsRemoved()
        {
            string inputString = "<html><head><!-- kommentar --></head></html>";
            var cleaner = new HtmlCleaner();
            string outputString = cleaner.Clean(inputString);

            Assert.AreEqual("<html><head></head></html>", outputString);
        }

        [TestMethod]
        public void Clean_WhenCalledWithHtmlIfBlocks_CommentIsNotRemoved()
        {
            string inputString = "<html><head><!--[if lt IE 9]><link rel=\"stylesheet\" href=\"explorer.css\" type=\"text/css\" /><![endif]--></head></html>";
            var cleaner = new HtmlCleaner();
            string outputString = cleaner.Clean(inputString);

            Assert.AreEqual(inputString, outputString);
        }

        [TestMethod]
        public void Clean_WhenCalledWithHtmlWithMultiLineBreaks_ReplacedBySingleLineBreaks()
        {
            string inputString = "<html>\r\n<head>\r\n\r\n</head>\r\n\r\n\r\n</html>";
            var cleaner = new HtmlCleaner();
            string outputString = cleaner.Clean(inputString);

            Assert.AreEqual("<html>\r\n<head>\r\n</head>\r\n</html>", outputString);
        }

        [TestMethod]
        public void Clean_WhenRowEndWithMultipleSpaces_SpacesAreRemoved()
        {
            string inputString = "<html> \r\n  \r\n<head>  \r\n</head>\r\n </html>";
            var cleaner = new HtmlCleaner();
            string outputString = cleaner.Clean(inputString);

            Assert.AreEqual("<html>\r\n<head>\r\n</head>\r\n </html>", outputString);
        }

        [TestMethod]
        public void Clean_WhenThereAreWhitespaceBetweenTags_SpacesAreRemoved()
        {
            string inputString = "<html>   <head>  </head> </html>";
            var cleaner = new HtmlCleaner();
            string outputString = cleaner.Clean(inputString);

            Assert.AreEqual("<html><head></head></html>", outputString);
        }
    }
}
