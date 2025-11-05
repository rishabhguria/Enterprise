using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.CalculationService.Models
{
    internal class WidgetChannelLayoutDto
    {
        public class WidgetLayout
        {
            public object CurrentLayout { get; set; }
            public List<UpdatedWidgetsDetail> updatedWidgetsDetails { get; set; }
        }

        public class UpdatedWidgetsDetail
        {
            public string widgetName { get; set; }
            public string widgetType { get; set; }
            public string widgetId { get; set; }
            public string headerId { get; set; }
            public string channelDetails { get; set; }
            public string headerOperation { get; set; }
            public string gridLayout { get; set; }
        }


    }
}
