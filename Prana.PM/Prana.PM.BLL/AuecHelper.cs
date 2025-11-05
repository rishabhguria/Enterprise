using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PM.BLL
{
    public static class AuecHelper
    {
        static Dictionary<int, AuecDetail> _auecLookup = new Dictionary<int, AuecDetail>();

        public static AuecDetail GetAUECDetail(int auecID)
        {
            AuecDetail auecDetail = null;

            try
            {
                if (auecID != -1 && auecID != int.MinValue)
                {
                    if (!_auecLookup.ContainsKey(auecID))
                    {
                        QueryData queryData = new QueryData();
                        queryData.StoredProcedureName = "P_GetAUECDetailsForAUECID";
                        queryData.DictionaryDatabaseParameter.Add("@AUECID", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@AUECID",
                            ParameterType = DbType.Int32,
                            ParameterValue = auecID
                        });

                        using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                        {
                            while (reader.Read())
                            {
                                object[] row = new object[reader.FieldCount];
                                reader.GetValues(row);

                                auecDetail = new AuecDetail();
                                auecDetail.AUEC = new AUEC(int.Parse(row[0].ToString()), row[1].ToString());

                                auecDetail.Asset = new Asset(int.Parse(row[2].ToString()), row[3].ToString());

                                auecDetail.Underlying = new Underlying(int.Parse(row[4].ToString()), row[5].ToString());

                                auecDetail.Exchange = new Exchange(int.Parse(row[6].ToString()), row[7].ToString());

                                auecDetail.Currency = new Currency(int.Parse(row[8].ToString()), row[9].ToString(), row[10].ToString());

                                _auecLookup.Add(auecDetail.AUEC.AUECID, auecDetail);
                            }
                        }
                    }
                    else
                    {
                        auecDetail = _auecLookup[auecID];
                    }
                }

            }
            #region catch

            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

                #endregion
            }

            return auecDetail;
        }

        public static Asset GetAsset(int auecID)
        {
            if (auecID != -1 && auecID != int.MinValue)
            {
                return GetAUECDetail(auecID).Asset;
            }
            else
            {
                return null;
            }

        }

        public static Underlying GetUnderlying(int auecID)
        {
            if (auecID != -1 && auecID != int.MinValue)
            {
                return GetAUECDetail(auecID).Underlying;
            }
            else
            {
                return null;
            }


        }

        public static Exchange GetExchange(int auecID)
        {
            if (auecID != -1 && auecID != int.MinValue)
            {

                return GetAUECDetail(auecID).Exchange;
            }
            else
            {
                return null;
            }

        }

        public static Currency GetCurrency(int auecID)
        {
            if (auecID != -1 && auecID != int.MinValue)
            {
                return GetAUECDetail(auecID).Currency;
            }
            else
            {
                return null;
            }

        }

        public static AUEC GetAUEC(int auecID)
        {
            if (auecID != -1 && auecID != int.MinValue)
            {
                return GetAUECDetail(auecID).AUEC;
            }
            else
            {
                return null;
            }

        }
    }
}
