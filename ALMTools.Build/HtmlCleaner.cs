using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ALMTools.Build
{
    public class HtmlCleaner
    {
        public HtmlCleaner()
        {
            HtmlComments = true;
            WhiteSpaceBetweenTags = true;
        }

        public string Clean(string inputSource)
        {
            string modifiedSource = inputSource;

            StripWhiteSpaceAtEndOfLine(ref modifiedSource);

            if (HtmlComments == true)
            {
                StripHtmlComments(ref modifiedSource);
            }

            if (WhiteSpaceBetweenTags == true)
            {
                StripWhiteSpaceBetweenTags(ref modifiedSource);
            }

            return modifiedSource;
        }

        private void StripWhiteSpaceAtEndOfLine(ref string modifiedSource)
        {
            var whitespaceFinder = new Regex(@" +$", RegexOptions.Multiline);
            modifiedSource = whitespaceFinder.Replace(modifiedSource, "");
        }

        private void StripWhiteSpaceBetweenTags(ref string modifiedSource)
        {
            var whitespaceFinder = new Regex(@"\>\s+\<", RegexOptions.Multiline);

            modifiedSource = whitespaceFinder.Replace(modifiedSource, @"><");
        }

        private void StripHtmlComments(ref string modifiedSource)
        {
            var htmlCommentFinder = new Regex(@"<!--[^\[].*?[^\]]-->", RegexOptions.Singleline);

            modifiedSource = htmlCommentFinder.Replace(modifiedSource, "");
        }

        public bool HtmlComments { get; set; }

        public bool WhiteSpaceBetweenTags { get; set; }
    }
}
