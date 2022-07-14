using SearchEngine.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Crawler.R1.Repository
{
    public interface ICrawlerService
    {
        List<Page> GetAllPageNotDone();

        void AddNewLinks(List<string> links);

        void UpdatePage(Page page);
        void AddNewImages(Tuple<List<string>, List<string>> links,string url);
    }
}
