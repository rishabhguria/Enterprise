using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.UIAutomation
{
    class AsyncGridExtractor
    {
        public DataTable ExtractGridDataAsDataTable(string parentAutomationID, string gridAutomationID)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string gridName = gridAutomationID;
                string windowName = parentAutomationID;
                string tabName = "DefaultTab"; // Optional
                string filePath = Path.Combine(Path.GetTempPath(), gridName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");

                SendExportRequestToClient(gridName, windowName, tabName, filePath);

                if (File.Exists(filePath))
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        bool isHeader = true;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            if (isHeader)
                            {
                                foreach (var header in values)
                                    dataTable.Columns.Add(header.Trim());
                                isHeader = false;
                            }
                            else
                            {
                                dataTable.Rows.Add(values);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error extracting DataTable: " + ex.Message);
            }
            return dataTable;
        }
        public DataSet ExtractGridDataAsDataSet(string parentAutomationID, string gridAutomationID, string commaJoinedGridLog = "")
        {
            DataSet dataSet = new DataSet();
            try
            {
                string gridName = gridAutomationID;
                string windowName = parentAutomationID;
                string tabName = "DefaultTab"; // Optional
                string filePath = Path.Combine(ApplicationArgumentsAndConstants.AyncFolderPath, gridName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                Directory.CreateDirectory(ApplicationArgumentsAndConstants.AyncFolderPath);

                SendExportRequestToClient(gridName, windowName, tabName, filePath);

                if (File.Exists(filePath))
                {
                    try
                    {
                        dataSet = WinDataUtilities.ConvertExcelToDataSet(filePath, "Sheet1,Sheet2,Sheet3,Sheet4");
                        WinDataUtilities.RemoveEmptyRowsFromAllTables(dataSet);
                        WinDataUtilities.ManageExtraColumn(dataSet);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error while processing Excel file: " + ex.Message);
                        Console.WriteLine("Stack Trace: " + ex.StackTrace);
                        return null;
                    }

                }
                else
                    return null;

            }
            catch (Exception ex)
            {

                Console.WriteLine("Error extracting DataSet: " + ex.Message);
                return null;
            }
            return dataSet;
        }
        public void ExtractGridData(string parentAutomationID, string gridAutomationID)
        {

            try
            {
                string gridName = gridAutomationID;
                string windowName = parentAutomationID;
                string tabName = "DefaultTab"; // You can pass actual tabName if needed
                string filePath = Path.Combine(Path.GetTempPath(), gridName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");

                SendExportRequestToClient(gridName, windowName, tabName, filePath);


            }
            catch (Exception ex)
            {
                Console.WriteLine("Async extraction failed: " + ex.Message);
            }
        }

        public DataSet ExtractSelectedRowGridDataAsDataSet(string parentAutomationID, string gridAutomationID, string commaJoinedGridLog = "")
        {
            DataSet dataSet = new DataSet();
            try
            {
                string gridName = gridAutomationID;
                string windowName = parentAutomationID;
                string tabName = "DefaultTab"; // Optional
                string filePath = Path.Combine(ApplicationArgumentsAndConstants.AyncFolderPath, gridName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");

                SendExportRequestToClient(gridName, windowName, tabName, filePath, "GetSelectedRowData");

                if (File.Exists(filePath))
                {
                    // Convert it into dataset

                }
                else
                    return null;

            }
            catch (Exception ex)
            {

                Console.WriteLine("Error extracting DataSet: " + ex.Message);
                return null;
            }
            return dataSet;
        }


        private void SendExportRequestToClient(string gridName, string windowName, string tabName, string filePath, string exportMode = "ExportData")
        {
            var factory = new ChannelFactory<IExportGridData>(
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/ExportGridData"));

            var proxy = factory.CreateChannel();

            if (exportMode.Equals("GetSelectedRowData", StringComparison.OrdinalIgnoreCase))
            {
                //proxy.GetSelectedRowData(gridName, windowName, tabName, filePath);
            }
            else
            {
                proxy.ExportData(gridName, windowName, tabName, filePath);
            }
        }
    }

    
}
