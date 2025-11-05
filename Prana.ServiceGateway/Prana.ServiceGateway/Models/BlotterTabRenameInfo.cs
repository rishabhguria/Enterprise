namespace Prana.ServiceGateway.Models
{
    public class BlotterTabRenameInfo
    {
        public string viewId { get; set; }

        public CustomTabsDetails customTabsDetails { get; set; }
    }

    public class CustomTabsDetails
    {
        public Dictionary<string, string> tabtoRename { get; set; }
    }

    public class BlotterTabRemoveInfo
    {
        public string viewId { get; set; }

        public string tabName { get; set; }
    }
}
