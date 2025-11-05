using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Prana.LayoutService.Models
{
    public class RtpnlLayoutInfo
    {
        public PageInfo pageInfo { get; set; }
        public List<ViewInfo> internalPageInfo { get; set; }
    }

    public class RtpnlLayoutInfoResponse
    {
        public List<PageInfo> Pages { get; set; }
        public List<ViewInfo> Views { get; set; }
    }

    public class PageInfo
    {
        [Required]
        public string pageId { get; set; }
        [Required]
        public string pageLayout { get; set; }
        public string pageName { get; set; }
        public string pageTag { get; set; }
        public string oldPageName { get; set; }
    }

    public class ViewInfo
    {        
        public string description { get; set; }
        public string viewLayout { get; set; }
        public string viewName { get; set; }        
        public string viewId { get; set; }
        public string moduleName {  get; set; }
        public string menuItem { get; set; }
    }

    public class ModuleNameDto
    {
        public string Data { get; set; }
    }

    public class ConfigDetailsInfo
    {
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
        public string pageId { get; set; }
    }

    public class WidgetConfigAndOldWidgetIds
    {
        public List<string> oldWidgetId { get; set; }
        public List<ConfigDetailsInfo> widgetConfigDetails { get; set; }
    }
}
