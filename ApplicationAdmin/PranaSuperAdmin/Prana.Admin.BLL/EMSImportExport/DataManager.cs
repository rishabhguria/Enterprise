using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;
using System.IO;

namespace Prana.Admin.BLL
{
    public class DataManager
    {
        public static ImportTradeXSLTFileCollection GetImportTradeDetails()
        {
            ImportTradeXSLTFileCollection importTrades = new ImportTradeXSLTFileCollection();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetImportTradeDetails";

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    importTrades.Add(FillImportTradeDetails(row));

                }
            }
            return importTrades;
        }

        private static ImportTradeXSLTFile FillImportTradeDetails(object[] row)
        {
            ImportTradeXSLTFile importTrade = new ImportTradeXSLTFile();

            if (row != null)
            {

                int IMPORTSOURCEID = 0;
                int IMPORTSOURCENAME = 1;
                int XSLTFILEID = 2;
                int XSLTFILENAME = 3;

                importTrade.ImportSourceID = Convert.ToInt32(row[IMPORTSOURCEID]);
                importTrade.EMSSource = Convert.ToString(row[IMPORTSOURCENAME]);
                importTrade.FileID = Convert.ToInt32(row[XSLTFILEID]);
                importTrade.FileName = Convert.ToString(row[XSLTFILENAME]);
            }
            return importTrade;
        }

        public static void SaveImportTradeDetails(ImportTradeXSLTFileCollection importTrades)
        {
            object[] parameter = new object[7];

            foreach (ImportTradeXSLTFile importFile in importTrades)
            {
                if (importFile.EMSSource != null || importFile.FileName != null)
                {
                    byte[] binaryData = TransformToBinary(importFile.FileName);
                    importFile.FileName = GetFileNameFromPath(importFile.FileName);

                    parameter[0] = importFile.ImportSourceID;
                    parameter[1] = importFile.EMSSource;
                    parameter[2] = importFile.FileID;
                    parameter[3] = importFile.FileName;
                    parameter[4] = binaryData;
                    parameter[5] = DateTime.UtcNow;
                    parameter[6] = Convert.ToInt32(Prana.Global.ApplicationConstants.MappingFileType.EMSImportXSLT);

                    DatabaseManager.DatabaseManager.ExecuteScalar("[P_SaveImportTradeDetails]", parameter);
                }
            }
        }

        private static string GetFileNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1);
        }

        private static byte[] TransformToBinary(string path)
        {
            if (path != "" && path.Contains("\\"))
            {
                FileStream fs = null;
                BinaryReader br = null;
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                br = new BinaryReader(fs);
                byte[] data = new byte[fs.Length];
                int length = (int)fs.Length;
                br.Read(data, 0, length);

                if (fs != null)
                {
                    fs.Close();
                    // br.Close();
                }

                return data;
            }
            else
            {
                return null;
            }
        }

        public static bool RemoveEntry(int importrSourceID, int XSLTFileID)
        {
            bool IsRemoved = new bool();
            int result = 0;

            IsRemoved = false;

            object[] parameter = new object[2];

            parameter[0] = importrSourceID;
            parameter[1] = XSLTFileID;

            try
            {
                result = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteEMSImportDetails", parameter));
                if (result > 0)
                {
                    IsRemoved = true;
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
            return IsRemoved;
        }

        public static ImportTradeXSLTFileCollection GetAllEMSSources()
        {
            return GetImportTradeDetails();
        }
    }
}
