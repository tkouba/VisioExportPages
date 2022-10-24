using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ExportPages
{
    internal class MdLinkLabel : LinkLabel
    {
        const string LF = "\n'";
        const string CRLF = "\r\n";
        const string CR = "\r";

        Regex regexLinks = new Regex(@"(?:(?:\[(?<text>.*)\])(?:\((?<url>https?\:\/\/.*?)(?:\s\""(?<alt>.*?)\""\)|\)))|(?<text>(?<url>https?\:\/\/\S*)))",
            RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private string mdText;
        public override string Text { get => mdText ?? base.Text; set => SetLinkLabelText(value); }

        public MdLinkLabel() : base()
        {
            LinkArea = new LinkArea();
        }

        private void SetLinkLabelText(string text)
        {
            mdText = text;
            List<LinkLabel.Link> links = new List<LinkLabel.Link>();
            StringBuilder sb = new StringBuilder();
            text = text.Replace(CRLF, LF).Replace(CR, LF);
            MatchCollection matches = regexLinks.Matches(text);
            int startPos = 0;
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    sb.Append(text.Substring(startPos, match.Index - startPos));
                    startPos = sb.Length;
                    sb.Append(match.Groups["text"].Value);
                    links.Add(new LinkLabel.Link()
                    {
                        Start = startPos,
                        Length = sb.Length - startPos,
                        LinkData = match.Groups["url"].Value,
                        Description = match.Groups["alt"].Value
                    });
                    startPos = match.Index + match.Length;
                }
                LinkArea = new LinkArea(0, 0);
                Links.Clear();
                base.Text = sb.ToString();
                foreach (LinkLabel.Link item in links)
                {
                    Links.Add(item);
                }
            }
            else
            {
                base.Text = text;
            }
        }

    }
}
