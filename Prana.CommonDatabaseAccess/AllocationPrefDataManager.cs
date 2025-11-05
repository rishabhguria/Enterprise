using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Prana.CommonDatabaseAccess
{
    /// <summary>
    /// Summary description for AccountManager.
    /// </summary>
    public class AllocationPrefDataManager : IAllocationPrefDataManager
    {
        public List<AllocationDefault> GetAccountDefaults()
        {
            List<AllocationDefault> defaults = new List<AllocationDefault>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetUserFundDefaults";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        defaults.Add(FillUserAccountDefaults(row, 0));
                    }
                }
            }
            #region Catch
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
            #endregion
            return defaults;
        }
        public void SaveDefaults(List<AllocationDefault> defaults)
        {
            object[] parameter = new object[3];
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            BinaryFormatter bf = new BinaryFormatter();
            foreach (AllocationDefault default1 in defaults)
            {
                #region try
                try
                {
                    if (default1.DefaultAllocationLevelList != null)
                    {
                        parameter[0] = default1.DefaultID;
                        parameter[1] = default1.DefaultName;
                        bf.Serialize(stream, default1.DefaultAllocationLevelList);
                        byte[] data = new byte[stream.Length];
                        stream.Write(data, 0, data.Length);
                        stream.Seek(0, 0);
                        parameter[2] = stream.ToArray(); //Convert.ToBase64String(data);
                        DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveFundDefaults", parameter);
                    }
                }
                # endregion
                #region Catch
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
                #endregion
            }

        }
        public void DeleteDefaults(List<int> listDefaultID)
        {
            object[] parameter = new object[1];

            #region try
            try
            {
                foreach (int defalutID in listDefaultID)
                {
                    parameter[0] = defalutID;
                    DatabaseManager.DatabaseManager.ExecuteScalar("P_DeleteFundDefault", parameter);
                }
            }
            # endregion
            #region Catch
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
            #endregion

        }
        public AllocationDefault FillUserAccountDefaults(object[] row, int offSet)
        {
            int DefaultID = 0 + offSet;
            int DefaultName = 1 + offSet;
            int DefaultAllocation = 2 + offSet;
            AllocationDefault defaultaccount = new AllocationDefault();
            if (row[DefaultName] != null)
            {
                defaultaccount.DefaultName = row[DefaultName].ToString();
            }
            if (row[DefaultID] != null)
            {
                defaultaccount.DefaultID = int.Parse(row[DefaultID].ToString());
            }
            if (row[DefaultAllocation] != null)
            {
                byte[] data = (byte[])row[DefaultAllocation];
                MemoryStream stream = new MemoryStream(data);
                BinaryFormatter bf = new BinaryFormatter();
                defaultaccount.DefaultAllocationLevelList = (AllocationLevelList)bf.Deserialize(stream);
            }
            return defaultaccount;
        }
    }
}
