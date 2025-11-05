using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Data;
using System.IO;

namespace Prana.Import
{
    public class GenericImportHandler : IImportHandler
    {
        public GenericImportHandler() { }

        #region IImportHandler Members

        public void ProcessRequest(DataSet ds, RunUpload runUpload, object taskResult, bool isSaveDataInApplication)
        {
            try
            {
                DataTable dTable = ds.Tables[0];
                StringWriter writer = new System.IO.StringWriter();
                dTable.WriteXml(writer, XmlWriteMode.IgnoreSchema, false);
                string xml = writer.ToString();

                ImportDataManager.SaveDataForTheGivenSP(xml, runUpload.SPName);
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
        }

        public void UpdateCollection(SecMasterBaseObj secMasterObj, string collectionKey)
        {
            throw new NotImplementedException();
        }

        public string GetXSDName()
        {
            return "GenericImport.xsd";
        }

        public DataTable ValidatePriceTolerance(DataSet ds)
        {
            return new DataTable();
        }

        #endregion
    }

}