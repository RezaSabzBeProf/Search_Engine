using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Core.R1_Crawler.Model
{
    public class R1CrawlerResult
    {
        public string h1Text { get; set; }

        public string MetaDesc { get; set; }

        public List<string> Links { get; set; } = new List<string>();
    }
}
