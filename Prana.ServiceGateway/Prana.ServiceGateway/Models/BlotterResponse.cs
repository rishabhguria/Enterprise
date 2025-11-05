using System.ComponentModel;

namespace Prana.ServiceGateway.Models
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
        [Description("GetData")]
        GetData = 1,

        [Description("PublishOrder")]
        PublishOrder = 2,

        [Description("RemoveStageOrder")]
        RemoveStageOrder = 3,

        [Description("RemoveExecution")]
        RemoveExecution = 4,

        [Description("OrderUpdates")]
        OrderUpdates = 5,

        [Description("PendingNewAlert")]
        PendingNewAlert = 6,
    }
}
