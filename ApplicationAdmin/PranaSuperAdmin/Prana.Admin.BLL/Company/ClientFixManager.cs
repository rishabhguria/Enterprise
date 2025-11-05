#region Using

using Prana.LogManager;
using System;
using System.Data;

#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ClientFixManager.
    /// </summary>
    public class ClientFixManager
    {
        public ClientFixManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static int SaveCompanyClientFix(ClientFix clientFix, int companyClientID)
        {

            int result = int.MinValue;
            object[] parameter = new object[6];
            try
            {
                parameter[0] = clientFix.SenderCompID;
                parameter[1] = clientFix.TargetCompID;
                parameter[2] = clientFix.OnBehalfOfCompID;
                parameter[3] = clientFix.IP;
                parameter[4] = clientFix.Port;
                parameter[5] = companyClientID;

                DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveClientFix", parameter);
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

        public static int SaveCompanyIdentifiers(Identifiers identifiers, int companyClientID)
        {


            //TODO: Add transaction.
            int result = int.MinValue;

            object[] parameter = new object[4];
            object[] parameterDelete = new object[1];
            parameterDelete[0] = companyClientID;
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyClientIdentifiers", parameterDelete).ToString();

                foreach (Identifier identifier in identifiers)
                {
                    parameter[0] = identifier.PrimaryKey;
                    parameter[1] = companyClientID;
                    parameter[2] = identifier.IdentifierID;
                    parameter[3] = identifier.ClientIdentifierName;

                    DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveClientIdentifiers", parameter);
                    result = 1;
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

        public static ClientFix GetClientFix(int intCompanyClientID)
        {
            ClientFix clientFix = new ClientFix();
            Object[] parameter = new object[1];
            parameter[0] = intCompanyClientID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetClientFix", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        clientFix = FillClientFix(row, 0);
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
            return clientFix;

        }

        public static ClientFix FillClientFix(object[] row, int offSet)
        {
            int SenderCompID = 0 + offSet;
            int TargetCompID = 1 + offSet;
            int OnBehalfOfCompID = 2 + offSet;
            int IP = 3 + offSet;
            int Port = 4 + offSet;



            ClientFix clientFix = new ClientFix();
            if (row[SenderCompID] != null)
            {
                clientFix.SenderCompID = row[SenderCompID].ToString();
            }
            if (row[TargetCompID] != null)
            {
                clientFix.TargetCompID = row[TargetCompID].ToString();
            }
            if (row[OnBehalfOfCompID] != null)
            {
                clientFix.OnBehalfOfCompID = row[OnBehalfOfCompID].ToString();
            }

            if (row[IP] != null)
            {
                clientFix.IP = row[IP].ToString();
            }
            if (row[Port] != null)
            {
                clientFix.Port = int.Parse(row[Port].ToString());
            }



            return clientFix;
        }

        public static Identifiers GetClientIdentifiers(int companyClientID)
        {
            Identifiers identifiers = new Identifiers();

            Object[] parameter = new object[1];
            parameter[0] = companyClientID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClientIdentifiers", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        identifiers.Add(FillClientIdentifiers(row, 0));
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
            return identifiers;
        }
        public static Identifier FillClientIdentifiers(object[] row, int offSet)
        {
            int PrimaryKey = 0 + offSet;
            int IdentifierID = 1 + offSet;
            int Identifier = 2 + offSet;
            int ClientIdentifierName = 3 + offSet;


            Identifier identifier = new Identifier();

            if (row[Identifier] != null)
            {
                identifier.IdentifierName = row[Identifier].ToString();
            }
            if (row[IdentifierID] != null)
            {
                identifier.IdentifierID = int.Parse(row[IdentifierID].ToString());
            }
            if (row[ClientIdentifierName] != null)
            {
                identifier.ClientIdentifierName = row[ClientIdentifierName].ToString();
            }
            if (row[PrimaryKey] != null)
            {
                identifier.PrimaryKey = row[PrimaryKey].ToString();
            }

            return identifier;
        }




    }
}
