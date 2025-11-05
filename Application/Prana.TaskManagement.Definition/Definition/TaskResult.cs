using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.TaskManagement.Definition.Definition
{
    public class TaskResult : EventArgs
    {
        //public TaskInfo TaskInfo { get; set; }

        private ExecutionInfo _executionInfo = new ExecutionInfo();

        public ExecutionInfo ExecutionInfo { get { return _executionInfo; } set { _executionInfo = value; } }

        public Exception Error { get; set; }

        private TaskStatistics _taskStatistics = new TaskStatistics();
        public TaskStatistics TaskStatistics { get { return _taskStatistics; } set { _taskStatistics = value; } }


        public String AsXML
        {
            get { return LoadResultAsXml(); }
        }

        public DataTable AsTable
        {
            get { return LoadResultAsTable(); }
        }

        private DataTable LoadResultAsTable()
        {
            DataTable dtReturn = new DataTable("Statistics");
            try
            {
                List<object> dataRowObjects = new List<object>();

                dtReturn.Columns.Add("Task");
                dataRowObjects.Add(ExecutionInfo.ExecutionName);

                dtReturn.Columns.Add("Type");
                dataRowObjects.Add(ExecutionInfo.TaskInfo.TaskName);

                dtReturn.Columns.Add("StartTime");
                dataRowObjects.Add(TaskStatistics.StartTime);

                dtReturn.Columns.Add("EndTime");
                dataRowObjects.Add(TaskStatistics.EndTime);

                dtReturn.Columns.Add("Status");
                dataRowObjects.Add(TaskStatistics.Status);

                dtReturn.Columns.Add("Error");
                dataRowObjects.Add(this.Error == null ? string.Empty : this.Error.Message);

                Dictionary<String, Object> taskSpecificData = TaskStatistics.TaskSpecificData.AsDictionary;
                foreach (String key in taskSpecificData.Keys)
                {
                    if (!dtReturn.Columns.Contains(key))
                    {
                        dtReturn.Columns.Add(key);
                        dataRowObjects.Add(taskSpecificData[key]);
                    }
                }

                Dictionary<String, Object> taskSpecificRefData = this.TaskStatistics.TaskSpecificData.GetRefStatisticsData();
                foreach (String key in taskSpecificRefData.Keys)
                {
                    if (!dtReturn.Columns.Contains(key + "Ref"))
                    {
                        dtReturn.Columns.Add(key + "Ref");
                        dataRowObjects.Add(taskSpecificRefData[key]);
                    }
                }

                dtReturn.Rows.Add(dataRowObjects.ToArray());

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
            return dtReturn;
        }

        private String LoadResultAsXml()
        {
            string xml = string.Empty;
            try
            {
                System.IO.StringWriter writer = new System.IO.StringWriter();
                this.AsTable.WriteXml(writer, XmlWriteMode.WriteSchema, false);
                xml = writer.ToString();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return xml;
        }

        //TODO: this is task specific BL
        //Need to be handled in specific caller class
        public string GetDashBoardXmlPath()
        {
            //added by: Bharat Raturi, 25 jun 2014
            //purpose: Get the existing path in case the batch was executed earlier with the same file
            if (this._taskStatistics.TaskSpecificData.GetKeySet().Contains("DashboardFile") && !string.IsNullOrWhiteSpace(this._taskStatistics.TaskSpecificData.GetValueForKey("DashboardFile").ToString()))
            {
                int pathLength = this._taskStatistics.TaskSpecificData.GetValueForKey("DashboardFile").ToString().Length;
                string dashboardpath = this._taskStatistics.TaskSpecificData.GetValueForKey("DashboardFile").ToString().Substring(0, pathLength - 4);
                return dashboardpath;
            }
            return @"\DashBoardData\" + this.ExecutionInfo.TaskInfo.TaskName + @"\" + DateTime.UtcNow.ToString("yyyyMMdd") + @"\" + this.ExecutionInfo.ExecutionId + "_" + this.ExecutionInfo.ExecutionName;
        }


        public void LogResult()
        {
            string xmlPath = GetDashBoardXmlPath() + ".xml";
            XMLUtilities.WriteXml(this.AsXML, xmlPath);
        }
    }
}
