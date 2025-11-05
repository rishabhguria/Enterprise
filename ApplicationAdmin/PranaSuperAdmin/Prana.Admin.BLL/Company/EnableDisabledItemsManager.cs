using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class EnableDisabledItemsManager
    {

        /// <summary>
        /// Created By Faisal Shah 14/07/14
        /// Creating DataSet for the Items that we need to enable.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <param name="enableEntries"></param>
        /// <returns></returns>
        public static DataSet CreateEnableDisabledItemsDataSet(int selectedValue, List<int> enableEntries)
        {
            DataSet dsEnableDisabledItems = new DataSet("dsEnableDisabledItems");
            DataTable dtEnableDisabledItems = new DataTable("dtEnableDisabledItems");

            dtEnableDisabledItems.Columns.Add("ID", typeof(int));
            dtEnableDisabledItems.Columns.Add("IDToBeEnabled", typeof(int));
            foreach (int Item in enableEntries)
            {
                dtEnableDisabledItems.Rows.Add(selectedValue, Item);
            }
            try
            {

                dsEnableDisabledItems.Tables.Add(dtEnableDisabledItems);
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
            return dsEnableDisabledItems;
        }

        /// <summary>
        /// Convert data set to XML Document
        /// </summary>
        /// <param name="ds">Data set holding the table of IDs to be Enabled</param>
        /// <returns>XML documents</returns>
        private static string ConvertDataSetToXml(DataSet dsEnableDisabledItems)
        {
            string xmlEnableDisabledItems = null;
            try
            {
                xmlEnableDisabledItems = dsEnableDisabledItems.GetXml();
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
            return xmlEnableDisabledItems;
        }

        /// <summary>
        /// Created By Faisal Shah 15/07/14
        /// Update Database for the Selected Items
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <param name="ItemstoBeEnabled"></param>
        /// <returns></returns>
        public static int SaveItemsToBeEnabled(int selectedValue, List<int> ItemstoBeEnabled)
        {
            int count = -1;
            try
            {
                DataSet dsItemsToBeEnabled = CreateEnableDisabledItemsDataSet(selectedValue, ItemstoBeEnabled);
                String xmlDoc = ConvertDataSetToXml(dsItemsToBeEnabled);
                count = EnableDisabledItemsDAL.SaveItemsToBeEnabled(xmlDoc);
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
            return count;
        }

        /// <summary>
        /// Created By Faisal Shah 15/07/14
        /// Get all Disabled Items for Selected Itam in Combo Box
        /// </summary>
        /// <param name="ItemSelected"></param>
        /// <returns></returns>
        public static DataTable GetSelectedDisabledItems(int ItemSelected)
        {
            try
            {
                if (ItemSelected > 0)
                {
                    DataTable dtDisabledItems = EnableDisabledItemsDAL.GetAllDisabledItems(ItemSelected);
                    return dtDisabledItems;
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
            return null;
        }
    }
}
