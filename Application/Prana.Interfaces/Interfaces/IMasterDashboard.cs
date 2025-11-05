using Prana.BusinessObjects;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Prana.Interfaces
{
    public interface IMasterDashboard
    {

        ConcurrentDictionary<string, List<int>> GetAccountBatchMapping();

        void PublishWorkFlowItems(List<WorkflowItem> workflowItems);

        void SaveWorkflowData(string DataAsXml);

        int GetAccountIDByAccountName(string accountName);
    }
    public delegate void WorkflowListner(DataTable item);
}
