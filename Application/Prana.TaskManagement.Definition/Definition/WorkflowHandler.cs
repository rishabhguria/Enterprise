using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Dashboard;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Prana.TaskManagement.Definition.Definition
{
    public class WorkflowHandler : IDisposable
    {
        private static object lockObject = new Object();
        ConcurrentDictionary<String, List<int>> _accountBatchMapping = new ConcurrentDictionary<string, List<int>>();
        //ConcurrentDictionary<String, List<int>> _accountReconTemplatesMapping = new ConcurrentDictionary<string, List<int>>();
        List<WorkflowItem> _workflowSatsData = new List<WorkflowItem>();
        IMasterDashboard _DashboardManager = null;
        #region singleton
        private static volatile WorkflowHandler instance;

        public WorkflowHandler()
        {
            try
            {
                InitializeData();

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
        }

        private void InitializeData()
        {
            try
            {
                _DashboardManager = new DashboardManager();
                _accountBatchMapping = _DashboardManager.GetAccountBatchMapping();
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
        }


        public static WorkflowHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new WorkflowHandler();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        //List<WorkflowItem> workflowSatsData = new List<WorkflowItem>();

        /// <summary>
        /// Publish Workflow Event
        /// </summary>
        /// <param name="data"></param>
        internal void PublishWorkflowEvent(DataTable data)
        {
            try
            {
                List<WorkflowItem> workflowItems = ProcessDataToGetWorkFlowEvents(data);
                PublishAndSaveWorkflow(workflowItems);

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
        }

        /// <summary>
        /// To publish and save workflow event
        /// </summary>
        /// <param name="workflowItems"></param>
        public void PublishAndSaveWorkflow(List<WorkflowItem> workflowItems)
        {
            try
            {
                if (workflowItems.Count > 0)
                {
                    _DashboardManager.PublishWorkFlowItems(workflowItems);
                    UpdateCache(workflowItems);

                    ////TODO, Saving in chuncks 
                    //if (_workflowSatsData.Count > 0)
                    //  {

                    //    SaveWorkflowData();

                    //    //DataSet ds = GeneralUtilities.CreateDataSetFromCollection(workflowItems, null);
                    //    //_DashboardManager.SaveWorkflowData(ds.GetXml());

                    //}
                }
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
        }

        private void UpdateCache(List<WorkflowItem> workflowItems)
        {
            try
            {
                lock (lockObject)
                {
                    _workflowSatsData.AddRange(workflowItems);
                }
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
        }

        /// <summary>
        /// To publish and save workflow event
        /// </summary>
        /// <param name="workflow"></param>
        public void PublishWorkflow(WorkflowItem workflow)
        {
            try
            {
                List<WorkflowItem> workflowItems = new List<WorkflowItem>();
                workflowItems.Add(workflow);
                PublishAndSaveWorkflow(workflowItems);
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowData"></param>
        /// <returns></returns>
        private List<WorkflowItem> ProcessDataToGetWorkFlowEvents(DataTable workflowData)
        {
            List<WorkflowItem> workflowItems = new List<WorkflowItem>();
            try
            {
                //TDOD - Need to refactor the code - omshiv
                foreach (DataRow row in workflowData.Rows)
                {
                    #region Handle workflow in case of ReRun upload
                    if (workflowData.Columns.Contains("IsReRun") && row["IsReRun"].ToString().Equals("True"))
                    {
                        String type = row["Type"].ToString();
                        switch (type)
                        {
                            case "Import":
                                String taskId = row["Task"].ToString();
                                String[] taskArray = taskId.Split('~');
                                String batchId = taskArray[0];
                                List<int> accounts = GetAccountIdsFromBatchId(batchId);
                                foreach (int accountId in accounts)
                                {
                                    WorkflowItem importWorkFlowItem = new WorkflowItem();
                                    importWorkFlowItem.EventID = (int)NirvanaWorkFlows.Import;// row["Type"].ToString();
                                    importWorkFlowItem.AccountID = accountId;
                                    importWorkFlowItem.StatusID = (int)Enum.Parse(typeof(NirvanaTaskStatus), row["Status"].ToString());
                                    importWorkFlowItem.Comments = row["Error"] == System.DBNull.Value ? String.Empty : row["Error"].ToString();
                                    importWorkFlowItem.ContextID = 1;
                                    importWorkFlowItem.ContextValue = batchId;
                                    importWorkFlowItem.EventRunTime = DateTime.Parse(row["StartTime"].ToString());
                                    if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                                    {
                                        // Added By : Manvendra P.
                                        // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3347
                                        // Date : 17-Apr-2015
                                        DateTime executionDate = DateTimeConstants.MinValue;
                                        DateTime.TryParse(row["ExecutionDate"].ToString(), out executionDate);
                                        importWorkFlowItem.FileExecutionDate = executionDate;
                                    }
                                    else
                                    {
                                        importWorkFlowItem.FileExecutionDate = DateTimeConstants.MinValue;
                                    }
                                    workflowItems.Add(importWorkFlowItem);

                                    WorkflowItem fileUploadItem = new WorkflowItem();
                                    fileUploadItem.EventID = (int)NirvanaWorkFlows.FileUpload;
                                    fileUploadItem.AccountID = accountId;
                                    fileUploadItem.StatusID = (int)NirvanaTaskStatus.Pending;
                                    fileUploadItem.Comments = string.Empty;
                                    fileUploadItem.ContextID = 1;
                                    fileUploadItem.ContextValue = batchId;
                                    fileUploadItem.EventRunTime = DateTime.Parse(row["StartTime"].ToString());
                                    if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                                    {
                                        DateTime executionDate = DateTimeConstants.MinValue;
                                        DateTime.TryParse(row["ExecutionDate"].ToString(), out executionDate);
                                        fileUploadItem.FileExecutionDate = executionDate;

                                    }
                                    workflowItems.Add(fileUploadItem);

                                    WorkflowItem symbolValidationItem = new WorkflowItem();
                                    symbolValidationItem.EventID = (int)NirvanaWorkFlows.SMValidation;
                                    symbolValidationItem.AccountID = accountId;
                                    symbolValidationItem.StatusID = (int)NirvanaTaskStatus.Pending;
                                    symbolValidationItem.Comments = string.Empty;
                                    symbolValidationItem.ContextID = 1;
                                    symbolValidationItem.ContextValue = batchId;
                                    symbolValidationItem.EventRunTime = DateTime.Parse(row["StartTime"].ToString());
                                    if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                                    {
                                        DateTime executionDate = DateTimeConstants.MinValue;
                                        DateTime.TryParse(row["ExecutionDate"].ToString(), out executionDate);
                                        symbolValidationItem.FileExecutionDate = executionDate;
                                        //symbolValidationItem.FileExecutionDate = DateTime.Parse(row["ExecutionDate"].ToString());
                                    }
                                    workflowItems.Add(symbolValidationItem);

                                }
                                break;
                        }
                    }
                    #endregion

                    #region Handle by Account
                    //else if (workflowData.Columns.Contains("Accounts") && !string.IsNullOrWhiteSpace(row["Accounts"].ToString()))
                    //{
                    //    String accounts = row["Accounts"].ToString();
                    //    String taskId = row["Task"].ToString();
                    //    String[] taskArray = taskId.Split('~');
                    //    String batchId = taskArray[0];
                    //    String[] accountsArray = accounts.Split(',');

                    //    foreach (string account in accountsArray)
                    //    {
                    //        int accountId = _DashboardManager.GetAccountIDByAccountName(account);
                    //        if (accountId > 0)
                    //        {
                    //            //if (row["Type"].ToString().Equals("Import"))
                    //            //{

                    //            //    WorkflowItem importWorkFlowItem = new WorkflowItem();
                    //            //    importWorkFlowItem.EventID = (int)NirvanaWorkFlows.Import;
                    //            //    importWorkFlowItem.AccountID = accountId;
                    //            //    importWorkFlowItem.StatusID = (int)Enum.Parse(typeof(NirvanaTaskStatus), row["Status"].ToString());
                    //            //    importWorkFlowItem.Comments = row["Error"] == System.DBNull.Value ? String.Empty : row["Error"].ToString();
                    //            //    importWorkFlowItem.ContextID = 1;
                    //            //    importWorkFlowItem.ContextValue = batchId;
                    //            //    importWorkFlowItem.EventRunTime = DateTime.Parse(row["StartTime"].ToString());
                    //            //    if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                    //            //    {
                    //            //        importWorkFlowItem.FileExecutionDate = DateTime.Parse(row["ExecutionDate"].ToString());
                    //            //    }
                    //            //    workflowItems.Add(importWorkFlowItem);

                    //            //}
                    //            //if (workflowData.Columns.Contains("SymbolValidation"))
                    //            //{
                    //            //    WorkflowItem symbolValidationItem = new WorkflowItem();
                    //            //    symbolValidationItem.EventID = (int)NirvanaWorkFlows.SMValidation;
                    //            //    symbolValidationItem.AccountID = accountId;
                    //            //    if (!String.IsNullOrEmpty(row["SymbolValidation"].ToString()))
                    //            //    {
                    //            //        String validationStatus = row["SymbolValidation"].ToString().Replace(" ", "");
                    //            //        symbolValidationItem.StatusID = (int)Enum.Parse(typeof(NirvanaTaskStatus), validationStatus);
                    //            //    }
                    //            //    if (workflowData.Columns.Contains("Comments"))
                    //            //    {
                    //            //        symbolValidationItem.Comments = row["Comments"] == System.DBNull.Value ? String.Empty : row["Comments"].ToString();
                    //            //    }
                    //            //    symbolValidationItem.ContextID = 1;
                    //            //    symbolValidationItem.ContextValue = batchId;
                    //            //    symbolValidationItem.EventRunTime = DateTime.Parse(row["StartTime"].ToString());
                    //            //    if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                    //            //    {
                    //            //        symbolValidationItem.FileExecutionDate = DateTime.Parse(row["ExecutionDate"].ToString());
                    //            //    }
                    //            //    workflowItems.Add(symbolValidationItem);
                    //            //}

                    //            //if (workflowData.Columns.Contains("RetrievalStatus") && !String.IsNullOrWhiteSpace(row["RetrievalStatus"].ToString()))
                    //            //{
                    //            //    WorkflowItem fileUploadItem = new WorkflowItem();
                    //            //    fileUploadItem.EventID = (int)NirvanaWorkFlows.FileUpload;
                    //            //    fileUploadItem.AccountID = accountId;
                    //            //    String retrievalStatus = row["RetrievalStatus"].ToString().Replace(" ", "");
                    //            //    fileUploadItem.StatusID = (int)Enum.Parse(typeof(NirvanaTaskStatus), retrievalStatus);
                    //            //    if (workflowData.Columns.Contains("Comments") && !String.IsNullOrWhiteSpace(row["Comments"].ToString()))
                    //            //    {
                    //            //        fileUploadItem.Comments = row["Comments"] == System.DBNull.Value ? String.Empty : row["Comments"].ToString();

                    //            //    }
                    //            //    fileUploadItem.ContextID = 1;
                    //            //    fileUploadItem.ContextValue = batchId;
                    //            //    fileUploadItem.EventRunTime = DateTime.Parse(row["StartTime"].ToString());
                    //            //    if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                    //            //    {
                    //            //        fileUploadItem.FileExecutionDate = DateTime.Parse(row["ExecutionDate"].ToString());
                    //            //    }
                    //            //    workflowItems.Add(fileUploadItem);
                    //            //}

                    //            //if (workflowData.Columns.Contains("ImportIntoApp"))
                    //            //{
                    //            //    WorkflowItem fileUploadItem = new WorkflowItem();
                    //            //    fileUploadItem.EventID = (int)NirvanaWorkFlows.ImportIntoAPP;
                    //            //    fileUploadItem.AccountID = accountId;
                    //            //    String importStatus = row["ImportStatus"].ToString().Replace(" ", "");
                    //            //    fileUploadItem.StatusID = (int)Enum.Parse(typeof(NirvanaTaskStatus), importStatus);
                    //            //    if (workflowData.Columns.Contains("Comments") && !String.IsNullOrWhiteSpace(row["Comments"].ToString()))
                    //            //        fileUploadItem.Comments = row["Comments"] == System.DBNull.Value ? String.Empty : row["Comments"].ToString();
                    //            //    fileUploadItem.ContextID = 1;
                    //            //    fileUploadItem.ContextValue = batchId;
                    //            //    fileUploadItem.EventRunTime = DateTime.Parse(row["StartTime"].ToString());
                    //            //    if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                    //            //    {
                    //            //        fileUploadItem.FileExecutionDate = DateTime.Parse(row["ExecutionDate"].ToString());
                    //            //    }
                    //            //    workflowItems.Add(fileUploadItem);
                    //            //}
                    //            //if (workflowData.Columns.Contains("ImportStatus") && !String.IsNullOrWhiteSpace(row["ImportStatus"].ToString()))
                    //            //{
                    //            //    WorkflowItem fileUploadItem = new WorkflowItem();
                    //            //    fileUploadItem.EventID = (int)NirvanaWorkFlows.ImportIntoAPP;
                    //            //    fileUploadItem.AccountID = accountId;
                    //            //    String importStatus = row["ImportStatus"].ToString().Replace(" ", "");
                    //            //    fileUploadItem.StatusID = (int)Enum.Parse(typeof(NirvanaTaskStatus), importStatus);
                    //            //    if (fileUploadItem.StatusID == 3 || fileUploadItem.StatusID == 4)
                    //            //        fileUploadItem.Comments = "Account Lock required / SM Validation Failed";
                    //            //    else
                    //            //    {
                    //            //        if (workflowData.Columns.Contains("Comments") && !String.IsNullOrWhiteSpace(row["Comments"].ToString()))
                    //            //            fileUploadItem.Comments = row["Comments"] == System.DBNull.Value ? String.Empty : row["Comments"].ToString();
                    //            //    }
                    //            //    fileUploadItem.ContextID = 1;
                    //            //    fileUploadItem.ContextValue = batchId;
                    //            //    fileUploadItem.EventRunTime = DateTime.Parse(row["StartTime"].ToString());
                    //            //    if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                    //            //    {
                    //            //        fileUploadItem.FileExecutionDate = DateTime.Parse(row["ExecutionDate"].ToString());
                    //            //    }
                    //            //    workflowItems.Add(fileUploadItem);
                    //            //}

                    //            //if (row["Type"].ToString().Equals("Recon"))
                    //            //{
                    //            //    WorkflowItem reconWorkFlowItem = new WorkflowItem();
                    //            //    reconWorkFlowItem.EventID = (int)NirvanaWorkFlows.Recon;
                    //            //    reconWorkFlowItem.AccountID = accountId;
                    //            //    reconWorkFlowItem.StatusID = (int)Enum.Parse(typeof(NirvanaTaskStatus), row["Status"].ToString());
                    //            //    reconWorkFlowItem.Comments = "";
                    //            //    reconWorkFlowItem.EventRunTime = DateTime.Parse(row["StartTime"].ToString());
                    //            //    if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                    //            //    {
                    //            //        reconWorkFlowItem.FileExecutionDate = DateTime.Parse(row["ExecutionDate"].ToString());
                    //            //    }
                    //            //    else
                    //            //    {
                    //            //        reconWorkFlowItem.FileExecutionDate = DateTimeConstants.MinValue;
                    //            //    }

                    //            //    if (workflowData.Columns.Contains("Comments") && !String.IsNullOrWhiteSpace(row["Comments"].ToString()))
                    //            //        reconWorkFlowItem.Comments = row["Comments"] == System.DBNull.Value ? String.Empty : row["Error"].ToString();
                    //            //    reconWorkFlowItem.ContextID = 2;
                    //            //    reconWorkFlowItem.ContextValue = batchId;
                    //            //    workflowItems.Add(reconWorkFlowItem);
                    //            //}
                    //        }

                    //    }



                    //}
                    #endregion

                    #region if accounts are not available in Row
                    else if (workflowData.Columns.Contains("Task") && workflowData.Columns.Contains("Type") && !string.IsNullOrWhiteSpace(row["Type"].ToString()))
                    {
                        String type = row["Type"].ToString();
                        switch (type)
                        {


                            case "Import":

                                String taskId = row["Task"].ToString();
                                String[] taskArray = taskId.Split('~');
                                String batchId = taskArray[0];
                                List<int> accounts = GetAccountIdsFromBatchId(batchId);

                                List<int> accountListInResult = new List<int>();
                                if (workflowData.Columns.Contains("Accounts") && !string.IsNullOrWhiteSpace(row["Accounts"].ToString()))
                                {
                                    String accountsList = row["Accounts"].ToString();
                                    string[] accountsArray = accountsList.Split(',');
                                    foreach (string account in accountsArray)
                                    {
                                        int accountId = _DashboardManager.GetAccountIDByAccountName(account);
                                        if (accountId > 0)
                                            accountListInResult.Add(accountId);
                                    }
                                }

                                foreach (int accountId in accounts)
                                {
                                    bool isDataAvailable = false;
                                    if (accountListInResult.Contains(accountId) || accountListInResult.Count == 0)
                                    {
                                        isDataAvailable = true;
                                    }

                                    GetWorkFlowStates(workflowData, workflowItems, row, batchId, accountId, isDataAvailable);
                                }
                                break;
                            case "Recon":

                                if (workflowData.Columns.Contains("FormatName"))
                                {

                                    String FormatName = row["FormatName"] == System.DBNull.Value ? String.Empty : row["FormatName"].ToString();
                                    List<int> accountIDs = GetAccountIdsFromBatchId(FormatName);
                                    foreach (int accountId in accountIDs)
                                    {
                                        WorkflowItem reconWorkFlowItem = new WorkflowItem();
                                        reconWorkFlowItem.EventID = (int)NirvanaWorkFlows.Recon;
                                        reconWorkFlowItem.AccountID = accountId;
                                        reconWorkFlowItem.StatusID = (int)Enum.Parse(typeof(NirvanaTaskStatus), row["Status"].ToString());
                                        reconWorkFlowItem.Comments = row["Error"] == System.DBNull.Value ? String.Empty : row["Error"].ToString();
                                        reconWorkFlowItem.ContextID = 2;
                                        reconWorkFlowItem.ContextValue = FormatName.Trim();
                                        reconWorkFlowItem.EventRunTime = DateTime.Parse(row["StartTime"].ToString());
                                        if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                                        {
                                            DateTime executionDate = DateTimeConstants.MinValue;
                                            DateTime.TryParse(row["ExecutionDate"].ToString(), out executionDate);
                                            reconWorkFlowItem.FileExecutionDate = executionDate;
                                            //reconWorkFlowItem.FileExecutionDate = DateTime.Parse(row["ExecutionDate"].ToString());
                                        }
                                        else
                                        {
                                            DateTime executionDate = DateTimeConstants.MinValue;
                                            DateTime.TryParse(row["FromDate"].ToString(), out executionDate);
                                            reconWorkFlowItem.FileExecutionDate = executionDate;
                                            //reconWorkFlowItem.FileExecutionDate = DateTime.Parse(row["FromDate"].ToString()); ;
                                        }
                                        workflowItems.Add(reconWorkFlowItem);
                                    }
                                }
                                break;

                        }


                    }
                    #endregion
                }
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
            return workflowItems;
        }

        private static void GetWorkFlowStates(DataTable workflowData, List<WorkflowItem> workflowItems, DataRow row, String batchId, int accountId, bool isDataAvailable)
        {
            try
            {
                DateTime filExecutionDate = DateTimeConstants.MinValue, startTime = DateTimeConstants.MinValue;


                if (workflowData.Columns.Contains("ExecutionDate") && !string.IsNullOrWhiteSpace(row["ExecutionDate"].ToString()))
                {
                    DateTime.TryParse(row["ExecutionDate"].ToString(), out filExecutionDate);
                }
                if (workflowData.Columns.Contains("StartTime") && !string.IsNullOrWhiteSpace(row["StartTime"].ToString()))
                {
                    DateTime.TryParse(row["StartTime"].ToString(), out startTime);
                }

                if (filExecutionDate > DateTimeConstants.MinValue)
                {
                    string comments = string.Empty;
                    if (workflowData.Columns.Contains("Comments") && !String.IsNullOrWhiteSpace(row["Comments"].ToString()))
                        comments = row["Comments"] == System.DBNull.Value ? String.Empty : row["Comments"].ToString();
                    if (!isDataAvailable)
                        comments = "Data is not available for this account in file.";

                    WorkflowItem importWorkFlowItem = new WorkflowItem();
                    importWorkFlowItem.EventID = (int)NirvanaWorkFlows.Import;// row["Type"].ToString();
                    importWorkFlowItem.AccountID = accountId;
                    importWorkFlowItem.StatusID = GetStatusID(row, "Status");
                    if (isDataAvailable)
                        importWorkFlowItem.Comments = row["Error"] == System.DBNull.Value ? String.Empty : row["Error"].ToString();
                    else
                        importWorkFlowItem.Comments = "Data is not available for this account in file.";
                    importWorkFlowItem.FileExecutionDate = filExecutionDate;
                    importWorkFlowItem.ContextID = 1;
                    importWorkFlowItem.ContextValue = batchId;
                    importWorkFlowItem.EventRunTime = startTime;
                    workflowItems.Add(importWorkFlowItem);

                    if (workflowData.Columns.Contains("RetrievalStatus") && !string.IsNullOrWhiteSpace(row["RetrievalStatus"].ToString()))
                    {
                        WorkflowItem fileUploadItem = new WorkflowItem();
                        fileUploadItem.EventID = (int)NirvanaWorkFlows.FileUpload;
                        fileUploadItem.AccountID = accountId;
                        fileUploadItem.StatusID = GetStatusID(row, "RetrievalStatus");
                        fileUploadItem.Comments = comments;
                        fileUploadItem.ContextID = 1;
                        fileUploadItem.ContextValue = batchId;
                        fileUploadItem.EventRunTime = startTime;
                        fileUploadItem.FileExecutionDate = filExecutionDate;
                        workflowItems.Add(fileUploadItem);
                    }

                    if (workflowData.Columns.Contains("SymbolValidation") && !string.IsNullOrWhiteSpace(row["SymbolValidation"].ToString()))
                    {
                        WorkflowItem symbolValidationItem = new WorkflowItem();
                        symbolValidationItem.EventID = (int)NirvanaWorkFlows.SMValidation;
                        symbolValidationItem.AccountID = accountId;
                        symbolValidationItem.StatusID = GetStatusID(row, "SymbolValidation");
                        symbolValidationItem.Comments = comments;
                        symbolValidationItem.ContextID = 1;
                        symbolValidationItem.ContextValue = batchId;
                        symbolValidationItem.EventRunTime = startTime;
                        symbolValidationItem.FileExecutionDate = filExecutionDate;
                        workflowItems.Add(symbolValidationItem);
                    }

                    if ((workflowData.Columns.Contains("ImportStatus") && !string.IsNullOrWhiteSpace(row["ImportStatus"].ToString())))
                    {
                        WorkflowItem fileUploadItem = new WorkflowItem();
                        fileUploadItem.EventID = (int)NirvanaWorkFlows.ImportIntoAPP;
                        fileUploadItem.AccountID = accountId;
                        fileUploadItem.StatusID = GetStatusID(row, "ImportStatus");
                        fileUploadItem.Comments = comments;
                        fileUploadItem.ContextID = 1;
                        fileUploadItem.ContextValue = batchId;
                        fileUploadItem.EventRunTime = startTime;
                        fileUploadItem.FileExecutionDate = filExecutionDate;
                        workflowItems.Add(fileUploadItem);
                    }
                }
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
        }

        private static int GetStatusID(DataRow row, string statusColumn)
        {
            int statusID = (int)NirvanaTaskStatus.Pending;
            try
            {
                //if (isDataAvailable)
                //{
                String status = row[statusColumn].ToString().Replace(" ", "");
                statusID = (int)Enum.Parse(typeof(NirvanaTaskStatus), status);
                //}
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
            return statusID;
        }


        /// <summary>
        /// Get AccountIds From BatchId
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        private List<int> GetAccountIdsFromBatchId(string batchId)
        {
            List<int> accountList = new List<int>();
            try
            {
                if (_accountBatchMapping != null && _accountBatchMapping.ContainsKey(batchId))
                {
                    accountList = _accountBatchMapping[batchId];
                }
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
            return accountList;
        }

        /// <summary>
        /// Save Workflow Datas
        /// </summary>
        internal void SaveWorkflowData()
        {
            try
            {
                if (_workflowSatsData.Count > 0)
                {
                    List<WorkflowItem> tempWorkflowData = null;
                    lock (lockObject)
                    {
                        tempWorkflowData = Prana.Global.Utilities.DeepCopyHelper.Clone(_workflowSatsData);
                        _workflowSatsData.Clear();
                    }
                    if (tempWorkflowData != null && tempWorkflowData.Count > 0)
                    {
                        DataSet ds = GeneralUtilities.CreateDataSetFromCollection(tempWorkflowData, null);
                        _DashboardManager.SaveWorkflowData(ds.GetXml());
                    }
                }
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
        }



        #region IDisposable Members

        public void Dispose()
        {

            Dispose(true);

            // Now since we have done the cleanup already there is nothing left
            // for the Finalizer to do. So lets tell the GC not to call it later.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                SaveWorkflowData();
                if (instance != null)
                    instance = null;
            }


        }

        #endregion


    }
}
