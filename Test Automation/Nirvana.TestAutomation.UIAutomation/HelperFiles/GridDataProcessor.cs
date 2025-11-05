using System;
using System.Collections.Generic;
using System.Data;

namespace Nirvana.TestAutomation.UIAutomation
{
    public class GridDataProcessor
    {
        public Dictionary<string, List<FocusGridData>> ProcessFocusedGridData(DataTable gridDataTable)
        {
            var dataDictionary = new Dictionary<string, List<FocusGridData>>();

            try
            {
                int triggerButtonIndex = gridDataTable.Columns["CustomGridAutomationID"].Ordinal;
                int windowsAutomationID = gridDataTable.Columns["WindowAutomationId"].Ordinal;
                int extractGridDataIndex = gridDataTable.Columns["ExtractGridData"].Ordinal;
                int dataExtractionScriptNameIndex = gridDataTable.Columns["DataExtractionScriptName"].Ordinal;
                int customWaitIdIndex = gridDataTable.Columns["CustomWait"].Ordinal;
                int logAutomationIDIndex = gridDataTable.Columns["LogAutomationID"].Ordinal;
                int gridExtractionTypeIndex = gridDataTable.Columns["GridExtractionType"].Ordinal;
                int isGridExtractionTypeAvailableIndex = gridDataTable.Columns["DataGridAutomationID"].Ordinal;

                foreach (DataRow row in gridDataTable.Rows)
                {
                    string key = string.Empty;
                    string isAvailable = "false";

                    if (!string.IsNullOrEmpty(row[triggerButtonIndex].ToString()))
                    {
                        key = row[triggerButtonIndex].ToString();
                    }
                    else if (!string.IsNullOrEmpty(row[isGridExtractionTypeAvailableIndex].ToString()))
                    {
                        key = row[isGridExtractionTypeAvailableIndex].ToString();
                        isAvailable = "true";
                    }

                    if (!string.IsNullOrEmpty(key))
                    {
                        FocusGridData dataGridData = new FocusGridData();
                        dataGridData.WindowAutomationId = row[windowsAutomationID].ToString();
                        dataGridData.ExtractGridData = row[extractGridDataIndex].ToString();
                        dataGridData.DataExtractionScriptName = row[dataExtractionScriptNameIndex].ToString();
                        dataGridData.CustomWait = row[customWaitIdIndex].ToString();
                        dataGridData.LogAutomationID = row[logAutomationIDIndex].ToString();
                        dataGridData.GridExtractionType = row[gridExtractionTypeIndex].ToString();
                        dataGridData.isGridExtractionTypeAvailable = isAvailable;

                        if (!dataDictionary.ContainsKey(key))
                        {
                            dataDictionary[key] = new List<FocusGridData>();
                        }

                        dataDictionary[key].Add(dataGridData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing grid data: " + ex.Message);
            }

            return dataDictionary;
        }

       
    }
}
