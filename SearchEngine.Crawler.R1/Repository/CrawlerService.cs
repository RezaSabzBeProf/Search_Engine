using Microsoft.EntityFrameworkCore;
using SearchEngine.Datalayer.Context;
using SearchEngine.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Crawler.R1.Repository
{
    public class CrawlerService : ICrawlerService
    {
        EngineDbContext _context;
        public CrawlerService()
        {
            var option = new DbContextOptionsBuilder<EngineDbContext>();
            option.UseSqlServer(@"Server=.;Database=SearchEngine_Database;Trusted_Connection=True;");
            _context = new EngineDbContext(option.Options);
        }

        public void AddNewLinks(List<string> links)
        {
            foreach(var item in links)
            {
                if(_context.Pages.Any(p => p.url == item))
                {

                }
                else
                {
                    _context.Pages.Add(new Page
                    {
                        title = "none",
                        url = item,
                        area = "none",
                        IsDone = false,
                    });
                }
                
            }
            _context.SaveChanges();
        }

        public List<Page> GetAllPageNotDone()
        {
            return _context.Pages.Where(p => p.IsDone == false).ToList();
        }
        public void UpdatePage(Page page) 
        {
            var item = _context.Pages.SingleOrDefault(p => p.url == page.url);
            if(item != null)
            {
                item.IsDone = true;
                item.area = page.area;
                item.title = page.title;
            }
            _context.Update(item);
            _context.SaveChanges();
        }
    }
}
