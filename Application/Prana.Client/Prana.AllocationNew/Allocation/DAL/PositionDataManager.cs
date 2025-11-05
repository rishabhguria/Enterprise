using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Utilities.XMLUtilities;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;

namespace Prana.AllocationNew
{
    class PositionDataManager
    {

        public static List<CurrentPosition> GetCurrentPositions()
        {
            Database db = DatabaseFactory.CreateDatabase();
            object[] parematers = new object[1];
            List<CurrentPosition> list = new List<CurrentPosition>();

            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_GetOpenPositionFundWise"))
                {

                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        CurrentPosition position = new CurrentPosition();
                        position.Symbol = row[0].ToString();
                        position.Qty =double.Parse(row[1].ToString());
                        position.AccountID =int.Parse(row[2].ToString());
                        list.Add(position);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }


            return list;
        }
    }
}
