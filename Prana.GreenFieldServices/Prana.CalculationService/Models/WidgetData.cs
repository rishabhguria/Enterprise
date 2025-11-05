using System.Collections.Generic;

namespace Prana.CalculationService.Models
{
    internal class WidgetData
    {
        public string pageId { get; set; }
        public string viewName { get; set; }
        public string widgetName { get; set; }
        public string widgetType { get; set; }
        public string defaultColumns { get; set; }
        public string coloredColumns { get; set; }
        public string graphType { get; set; }
        public bool isFlashColorEnabled { get; set; }
        public string channelDetail { get; set; }
        public string linkedWidget { get; set; }
        public string widgetId { get; set; }
        public string primaryMetric { get; set; }
    }

    internal class WidgetConfigDataAndOldWidgetIds
    {
        public List<string> oldWidgetId { get; set; }
        public List<WidgetData> widgetConfigDetails { get; set; }
    }
}