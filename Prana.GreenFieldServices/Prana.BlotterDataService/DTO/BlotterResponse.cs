using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System.Collections.Generic;

namespace Prana.BlotterDataService.DTO
{
    public class BlotterResponse
    {
        public BlotterRequestType BlotterRequestType { get; set; }
        public List<BlotterOrder> OrderTabData { get; set; }
        public List<BlotterOrder> WorkingTabData { get; set; }
        public string RemovedParentClOrderIds;
        public string OrderUpdates;

        public BlotterResponse(BlotterRequestType blotterRequestType, List<BlotterOrder> orderTabData, List<BlotterOrder> workingTabData, string removedParentClOrderIds = "", string orderUpdatesData = "")
        {
            BlotterRequestType = blotterRequestType;
            OrderTabData = orderTabData;
            WorkingTabData = workingTabData;
            RemovedParentClOrderIds = removedParentClOrderIds;
            OrderUpdates = orderUpdatesData;
        }
    }

    public enum BlotterRequestType
    {
        [EnumDescriptionAttribute("GetData")]
        GetData = 1,

        [EnumDescriptionAttribute("PublishOrder")]
        PublishOrder = 2,

        [EnumDescriptionAttribute("RemoveStageOrder")]
        RemoveStageOrder = 3,

        [EnumDescriptionAttribute("RemoveExecution")]
        RemoveExecution = 4,

        [EnumDescriptionAttribute("OrderUpdates")]
        OrderUpdates = 5,

        [EnumDescriptionAttribute("PendingNewAlert")]
        PendingNewAlert = 6,
    }
}
