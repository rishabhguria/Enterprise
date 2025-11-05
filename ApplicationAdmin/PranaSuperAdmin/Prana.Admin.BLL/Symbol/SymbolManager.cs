using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// SymbolManager would be a static class to handel <see cref="Symbol"/> related details.
    /// </summary>
    /// <remarks>Its a Static class like other managers class so that we don't have to instatiate it again and again while working with them.</remarks>
    public class SymbolManager
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private SymbolManager()
        {
        }

        #endregion

        /// <summary>
        /// Adds/Updates <see cref="Symbol"/> into database.
        /// </summary>
        /// <param name="venue"><see cref="Symbol"/> to be added.</param>
        /// <returns>Result. True if succesfull else False.</returns>
        #region Basic methods like Add/Update/Delete/Get for Symbol Conversion

        public static SymbolConversion FillSymbolConversions(object[] row, int offSet)
        {
            int symbolConversionID = 0 + offSet;
            int symbolName = 1 + offSet;

            SymbolConversion symbolConversion = new SymbolConversion();
            try
            {
                if (row[symbolConversionID] != null)
                {
                    symbolConversion.SymbolID = int.Parse(row[symbolConversionID].ToString());
                }
                if (row[symbolName] != null)
                {
                    symbolConversion.SymbolName = row[symbolName].ToString();
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
            return symbolConversion;
        }

        public static SymbolConversions GetSymbolConversions()
        {
            SymbolConversions symbolConversions = new SymbolConversions();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllSymbolConversions";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        symbolConversions.Add(FillSymbolConversions(row, 0));
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
            return symbolConversions;
        }

        public static SymbolConversion GetSymbolConversion(int symbolConversionID)
        {
            SymbolConversion symbolConversion = new SymbolConversion();

            object[] parameter = new object[1];
            parameter[0] = symbolConversionID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSymbolConversion", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        symbolConversion = FillSymbolConversions(row, 0);
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
            return symbolConversion;
        }

        public static bool DeleteSymbolConversion(int symbolConversionID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = symbolConversionID;

            try
            {
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSymbolConversion", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
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
            return result;
        }

        #endregion

        #region Basic methods like Add/Update/Delete/Get for Symbol

        public static Symbol FillSymbols(object[] row, int offSet)
        {
            int symbolID = 0 + offSet;
            int companySymbol = 1 + offSet;

            Symbol symbol = new Symbol();
            try
            {
                if (row[symbolID] != null)
                {
                    symbol.SymbolID = int.Parse(row[symbolID].ToString());
                }
                if (row[companySymbol] != null)
                {
                    symbol.CompanySymbol = row[companySymbol].ToString();
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
            return symbol;
        }

        public static Symbols GetSymbols()
        {
            Symbols symbols = new Symbols();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllSymbols";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        symbols.Add(FillSymbols(row, 0));
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
            return symbols;
        }

        public static Symbol GetSymbol(int symbolID)
        {
            Symbol symbol = new Symbol();

            object[] parameter = new object[1];
            parameter[0] = symbolID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSymbol", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        symbol = FillSymbols(row, 0);
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
            return symbol;
        }
        #endregion

        #region Basic methods like Add/Update/Delete/Get for SymbolIdentifiers

        public static SymbolIdentifier FillSymbolIdentifiers(object[] row, int offSet)
        {
            int symbolIdentifierID = 0 + offSet;
            int symbolIdentifierName = 1 + offSet;
            int symbolIdentifierShortName = 2 + offSet;

            SymbolIdentifier symbolIdentifier = new SymbolIdentifier();
            try
            {
                if (row[symbolIdentifierID] != null)
                {
                    symbolIdentifier.SymbolIdentifierID = int.Parse(row[symbolIdentifierID].ToString());
                }
                if (row[symbolIdentifierName] != null)
                {
                    symbolIdentifier.SymbolIdentifierName = row[symbolIdentifierName].ToString();
                }
                if (row[symbolIdentifierShortName] != null)
                {
                    symbolIdentifier.ShortName = row[symbolIdentifierShortName].ToString();
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
            return symbolIdentifier;
        }

        public static SymbolIdentifiers GetSymbolIdentifiers()
        {
            SymbolIdentifiers symbolIdentifiers = new SymbolIdentifiers();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllSymbols";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        symbolIdentifiers.Add(FillSymbolIdentifiers(row, 0));
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
            return symbolIdentifiers;
        }

        #region SymbolConventions

        public static SymbolConvention FillSymbolConventions(object[] row, int offSet)
        {
            int symbolConventionID = 0 + offSet;
            int symboConventionName = 1 + offSet;
            int symbolConventionShortName = 2 + offSet;

            SymbolConvention symbolConvention = new SymbolConvention();
            try
            {
                if (row[symbolConventionID] != null)
                {
                    symbolConvention.SymbolConventionID = int.Parse(row[symbolConventionID].ToString());
                }
                if (row[symboConventionName] != null)
                {
                    symbolConvention.SymbolConventionName = row[symboConventionName].ToString();
                }
                if (row[symbolConventionShortName] != null)
                {
                    symbolConvention.ShortName = row[symbolConventionShortName].ToString();
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
            return symbolConvention;
        }

        public static SymbolConventions GetSymbolConventions()
        {
            SymbolConventions symbolConventions = new SymbolConventions();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllSymbolConventions";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        symbolConventions.Add(FillSymbolConventions(row, 0));
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
            return symbolConventions;
        }

        public static SymbolConvention GetSymbolConvention(int symbolConventionID)
        {
            SymbolConvention symbolConvention = new SymbolConvention();

            object[] parameter = new object[1];
            parameter[0] = symbolConventionID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSymbol", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        symbolConvention = FillSymbolConventions(row, 0);
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
            return symbolConvention;
        }

        public static int SaveSymbolConventions(SymbolConvention symbolConvention)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];

            parameter[0] = symbolConvention.SymbolConventionID;
            parameter[1] = symbolConvention.SymbolConventionName;
            parameter[2] = symbolConvention.ShortName;
            parameter[3] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveSymbolConvention", parameter).ToString());
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
            return result;
        }

        public static bool DeleteSymbolConvention(int symbolConventionID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = symbolConventionID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSymbolConvention", parameter) > 0)
                {
                    result = true;
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
            return result;
        }

        #endregion

        public static SymbolIdentifier GetSymbolIdentifier(int symbolIdentifierID)
        {
            SymbolIdentifier symbolIdentifier = new SymbolIdentifier();

            object[] parameter = new object[1];
            parameter[0] = symbolIdentifierID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSymbol", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        symbolIdentifier = FillSymbolIdentifiers(row, 0);
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
            return symbolIdentifier;
        }

        public static int SaveSymbolIdentifiers(SymbolIdentifier symbolIdentifier)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];

            parameter[0] = symbolIdentifier.SymbolIdentifierID;
            parameter[1] = symbolIdentifier.SymbolIdentifierName;
            parameter[2] = symbolIdentifier.ShortName;
            parameter[3] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveSymbolIdentifier", parameter).ToString());
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
            return result;
        }

        public static bool DeleteSymbolIdentifier(int symbolIdentifierID, bool deleteForceFully)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[2];
                parameter[0] = symbolIdentifierID;
                parameter[1] = (deleteForceFully == true ? 1 : 0);
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSymbolIdentifier", parameter) > 0)
                {
                    result = true;
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
            return result;
        }

        #endregion

    }
}
