using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using System;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    public class GroupOperationsEventArgs : EventArgs
    {
        public String GroupId { get; set; }
        public NotificationSetting Notification { get; set; }
        public GroupOperations Operation { get; set; }
    }
}
