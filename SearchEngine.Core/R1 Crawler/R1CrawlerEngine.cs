using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchEngine.Core.R1_Crawler.Model
{
    public class R1CrawlerEngine
    {
        public string WebUrl { get; set; }
        public R1CrawlerEngine(string url)
        {
            this.WebUrl = url;
        }
        public R1CrawlerResult DoWork()
        {
            string html = GetHtmlFromPage(WebUrl);
            if (html == "error")
            {
                return new R1CrawlerResult
                {
                    h1Text = "error"
                };
            }
            string h1Text = GetPageH1(html);
            List<string> getLinks = GetAllPageLink(html);
            string metaDesc = GetMetaDesc(html);
            return new R1CrawlerResult
            {
                h1Text = h1Text,
                Links = getLinks,
                MetaDesc = metaDesc
            };
        }
        private string GetHtmlFromPage(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.UserAgent = "R1 Web Crawler";

                WebResponse response = request.GetResponse();

                Stream stream = response.GetResponseStream();

                StreamReader reader = new StreamReader(stream);
                string htmlText = reader.ReadToEnd();
                return htmlText;

            }
            catch
            {
                return "error";
            }

        }
        private string GetPageH1(string html)
        {
            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(html);
            var x = document.GetElementsByTagName("<h1>");
            string h1Text = x[0].InnerHtml;
            return h1Text;
        }
        private List<string> GetAllPageLink(string html)
        {
            Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");
            ISet<string> newLinks = new HashSet<string>();
            List<string> result = new List<string>();
            foreach (var match in regexLink.Matches(html))
            {
                if (!newLinks.Contains(match.ToString()))
                {
                    newLinks.Add(match.ToString());
                    result.Add(match.ToString());
                }
            }
            return result;
        }
        private string GetMetaDesc(string html)
        {
            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(html);
            string x = document.QuerySelector("meta[name='description']").GetAttribute("content");
            return x;
        }
    }
}
