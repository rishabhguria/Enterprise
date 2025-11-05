using Prana.BusinessObjects;
using Prana.ClientCommon;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Analytics
{
    [XmlRoot("RiskGridPrefs")]
    [Serializable]
    public class RiskLayout
    {
        [XmlElement("StepAnalysisViewLayoutList")]
        public SerializableDictionary<string, StepAnalLayout> StepAnalysisColumnsList = new SerializableDictionary<string, StepAnalLayout>();

        #region Risk Report
        [XmlArray("RiskReportColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> RiskReportColumns = new List<ColumnData>();

        [XmlArray("RiskReportGroupByColumnsCollection"), XmlArrayItem("GroupByColumn", typeof(SortedColumnData))]
        public List<SortedColumnData> RiskReportGroupByColumnsCollection = new List<SortedColumnData>();
        #endregion

        #region Risk Simulation
        [XmlArray("RiskSimulationColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> RiskSimulationColumns = new List<ColumnData>();

        [XmlArray("RiskSimulationGroupByColumnsCollection"), XmlArrayItem("GroupByColumn", typeof(SortedColumnData))]
        public List<SortedColumnData> RiskSimulationGroupByColumnsCollection = new List<SortedColumnData>();
        #endregion

        #region Stress Test
        [XmlArray("StressTestColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> StressTestColumns = new List<ColumnData>();

        [XmlArray("StressTestGroupByColumnsCollection"), XmlArrayItem("GroupByColumn", typeof(SortedColumnData))]
        public List<SortedColumnData> StressTestGroupByColumnsCollection = new List<SortedColumnData>();
        #endregion

        public StepAnalLayout GetStepAnalLayout(string key)
        {
            if (StepAnalysisColumnsList.ContainsKey(key))
            {
                return StepAnalysisColumnsList[key];
            }

            // returning the default layout if not saved
            StepAnalLayout DefaultLayout = GetDefaultStepAnalysisLayout(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
            StepAnalysisColumnsList.Add(key, DefaultLayout);
            return DefaultLayout;
        }

        public StepAnalLayout GetDefaultStepAnalysisLayout(int userID)
        {
            string riskLayoutDirectoryPath = System.Windows.Forms.Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID.ToString();
            string riskDefaultLayoutFilePath = riskLayoutDirectoryPath + @"\RiskDefaultLayout.xml";

            StepAnalLayout riskDefaultLayout = new StepAnalLayout();
            try
            {
                if (!Directory.Exists(riskLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(riskLayoutDirectoryPath);
                }
                if (File.Exists(riskDefaultLayoutFilePath))
                {
                    using (FileStream fs = File.OpenRead(riskDefaultLayoutFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(StepAnalLayout));
                        riskDefaultLayout = (StepAnalLayout)serializer.Deserialize(fs);
                    }
                }
                else
                {
                    riskDefaultLayout = new StepAnalLayout();

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return riskDefaultLayout;
        }

        public void SaveDefaultStepAnalysisLayout(StepAnalLayout stepAnalLayout, int userID)
        {
            try
            {
                string riskLayoutDirectoryPath = System.Windows.Forms.Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + userID.ToString();
                string riskDefaultLayoutFilePath = riskLayoutDirectoryPath + @"\RiskDefaultLayout.xml";
                using (XmlTextWriter writer = new XmlTextWriter(riskDefaultLayoutFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(StepAnalLayout));
                    serializer.Serialize(writer, stepAnalLayout);

                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}