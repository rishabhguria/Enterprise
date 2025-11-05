using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.LayoutService.Models
{
    public class DeletePageDTO
    {
        public string pageId { get; set; }
        public List<String> viewIds { get; set; }
        public string title { get; set; }
    }
}
