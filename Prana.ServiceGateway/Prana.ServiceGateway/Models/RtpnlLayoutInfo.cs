namespace Prana.ServiceGateway.Models
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
        public string pageId { get; set; }
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
        public string moduleName { get; set; }
        public string menuItem { get; set; }
    }
    public class WidgetConfigAndOldWidgetIds
    {
         public List<string> oldWidgetId { get; set; }
        public List<ConfigDetailsInfo> widgetConfigDetails { get; set; }
    }


}