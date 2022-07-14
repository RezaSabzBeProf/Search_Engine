using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Datalayer.Entities
{
    public class Page
    {
        [Key]
        public int Id { get; set; }
        public string title { get; set; }
        public string area { get; set; }
        public string url { get; set; }
        public string ImagePageUrl { get; set; }
        public bool IsImage { get; set; }
        public bool IsDone { get; set; } = false;
    }
}
