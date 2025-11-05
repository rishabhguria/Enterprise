using Prana.LogManager;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class WorkflowItem
    {
        public DateTime EventRunTime { get; set; }
        private DateTime _fileExecutionDate = DateTimeConstants.MinValue;
        public DateTime FileExecutionDate { get { return _fileExecutionDate; } set { _fileExecutionDate = value; } }

        public int EventID { get; set; }

        public int AccountID { get; set; }

        public String Comments { get; set; }

        public int ContextID { get; set; }

        public int StatusID { get; set; }



        public static WorkflowItem GetWorkFlowItem(System.Data.DataRow dr)
        {
            WorkflowItem uiObject = new WorkflowItem();
            try
            {
                uiObject.AccountID = Convert.ToInt32(dr["CompanyFundID"]);
                uiObject.FileExecutionDate = DateTime.Parse(dr["Date"].ToString());
                // uiObject.AccountName = dr["AccountName"].ToString();
                //  uiObject.CompanyName = dr["CompanyName"].ToString();
                //   uiObject.ThirdPartyName = dr["ThirdPartyName"].ToString();
                uiObject.EventRunTime = DateTime.Parse(dr["TaskRunTime"].ToString());
                uiObject.ContextValue = dr["FormatName"].ToString();
                uiObject.Comments = dr["Comments"].ToString();

                uiObject.EventID = Convert.ToInt32(dr["WorkflowID"].ToString());
                uiObject.StatusID = Convert.ToInt32(dr["StatusID"].ToString());

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return uiObject;
        }



        public string ContextValue { get; set; }
    }
}
