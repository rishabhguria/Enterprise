using Prana.BusinessObjects.Authorization;
using Prana.BusinessObjects.Enums;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Auth.Authorization.DAL
{
    /// <summary>
    /// 
    /// </summary>
    internal static class NirvanaAuthorizationDataManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<NirvanaPrincipalType, Dictionary<int, List<ResourceAction>>> GetDataFromDatabase()
        {

            //   String connectionString = "Database=MySampleDatabase;Server=(local);Integrated Security=false;User ID=sa;Password=NIRvana2@@6;";
            //   String sqlCommand = "Select * from T_AuthPermissions";
            DataSet ds = new DataSet();
            var returnDict = new Dictionary<NirvanaPrincipalType, Dictionary<int, List<ResourceAction>>>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAuthPermissions";

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    NirvanaPrincipalType principalType = (NirvanaPrincipalType)Enum.Parse(typeof(NirvanaPrincipalType), dr["PrincipalType"].ToString());
                    int pricipalValue = int.Parse(dr["PricipalValue"].ToString());
                    int resourceId = int.Parse(dr["ResourceDataValue"].ToString());
                    int resourceType = int.Parse(dr["ResourceDataType"].ToString());
                    int action = int.Parse(dr["AuthActionValue"].ToString());

                    ResourceAction rActionTemp = new ResourceAction(resourceId, resourceType, action);

                    if (!returnDict.ContainsKey(principalType))
                    {
                        Dictionary<int, List<ResourceAction>> principalTypeDataDict = new Dictionary<int, List<ResourceAction>>();

                        returnDict.Add(principalType, principalTypeDataDict);
                    }
                    if (returnDict.ContainsKey(principalType))
                    {
                        List<ResourceAction> rActionList = new List<ResourceAction>();
                        Dictionary<int, List<ResourceAction>> principalTypeDataDict = returnDict[principalType];
                        if (!principalTypeDataDict.ContainsKey(pricipalValue))
                        {
                            rActionList.Add(rActionTemp);
                            principalTypeDataDict.Add(pricipalValue, rActionList);
                        }
                        else
                        {
                            rActionList = principalTypeDataDict[pricipalValue];
                            rActionList.Add(rActionTemp);
                        }
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


            return returnDict;
        }

    }
}
