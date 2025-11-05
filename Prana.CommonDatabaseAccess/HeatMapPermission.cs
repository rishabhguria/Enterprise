using Prana.LogManager;
using System;
using System.Data;

namespace Prana.CommonDataCache.DAL
{
    public class HeatMapPermission
    {
        static HeatMapPermission _heatMapPermission;
        public static HeatMapPermission GetInstance()
        {
            if (_heatMapPermission == null)
                _heatMapPermission = new HeatMapPermission();
            return _heatMapPermission;
        }

        /// <summary>
        /// Checking HeatMap Enabled Permission for company Id
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Boolean CheckHeatMapEnabled(int companyId)
        {
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyId;

                DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_CA_GetHeatMapModuleEnabled", parameter);
                if (ds.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
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
                return false;
            }
        }

        /// <summary>
        /// get permission of Read write Of HeatMap Compliance module from Data Base
        /// </summary>
        /// <returns>Module id and read Write Id for a user</returns>
        public DataSet GetHeatMapModulePermission(int companyId)
        {
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyId;

                return DatabaseManager.DatabaseManager.ExecuteDataSet("P_CA_GetHeatMapModulePermission", parameter);
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
                return null;
            }
        }

    }
}
