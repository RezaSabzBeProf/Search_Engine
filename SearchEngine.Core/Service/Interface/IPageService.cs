using SearchEngine.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Core.Service.Interface
{
    public interface IPageService
    {
        List<Page> Search(string q);

        List<Page> SearchImage(string q);
    }
}
