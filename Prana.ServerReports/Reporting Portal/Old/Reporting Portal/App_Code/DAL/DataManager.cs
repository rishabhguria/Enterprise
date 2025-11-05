using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


/// <summary>
/// Summary description for DataManager
/// </summary>
public class DataManager
{
    static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

    public DataManager()
    {

    }

    public DataTable getUsers(string userName, string password)
    {
        SqlCommand command = new SqlCommand("P_GetUser", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@userName", userName));
        command.Parameters.Add(new SqlParameter("@password", password));
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "User");
        return ds.Tables[0];
    }

    public bool TermAccepted(string userID)
    {
        bool rowsEffected = false;
        SqlCommand cmd = new SqlCommand("update T_User set TermAccepted=1 where UserID=" + userID, conn);
        if (cmd.ExecuteNonQuery() > 0)
            rowsEffected = true;
        return rowsEffected;
    }

    public DataTable getReports(string clientID)
    {
        SqlCommand command = new SqlCommand("P_getReports", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@clientID", clientID));
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "Reports");
        return ds.Tables[0];
    }

    public DataTable getClientDetailsByID(string sClientID)
    {
        SqlCommand command = new SqlCommand("P_GetSubClientDetails", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@clientID", sClientID));
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "ClientDetails");
        return ds.Tables[0];
    }

    public DataTable getSubClients(string userID)
    {
        SqlCommand command = new SqlCommand("P_GetSubClients", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@UserID", userID));
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "User");
        return ds.Tables[0];
    }

    public static DataTable getBaseSettings()
    {
        SqlCommand command = new SqlCommand("P_GetBaseSettings", conn);
        command.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "BaseSettings");
        return ds.Tables[0];
    }

    public static DataSet getCompanyMasterFundsFromDb()
    {
        SqlConnection cnMasterFund = new SqlConnection(ConfigurationManager.ConnectionStrings["CnMasterFund"].ConnectionString);
        SqlCommand command = new SqlCommand("SELECT * FROM T_CompanyMasterFunds", cnMasterFund);
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "User");
        return ds;
    }

    public static DateTime getLastBusinessDay()
    {
        SqlConnection cnClientDB = new SqlConnection(ConfigurationManager.ConnectionStrings["CnMasterFund"].ConnectionString);

        SqlCommand command = new SqlCommand("P_GetLastBusinessDay", cnClientDB);
        command.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "User");
        return DateTime.Parse(ds.Tables[0].Rows[0]["LastBusinessDay"].ToString());
    }
    public static DateTime getMonthStartForDate(DateTime dtDate)
    {
        SqlConnection cnClientDB = new SqlConnection(ConfigurationManager.ConnectionStrings["CnMasterFund"].ConnectionString);

        SqlCommand command = new SqlCommand("P_GetMonthStartForDate", cnClientDB);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add(new SqlParameter("@date", dtDate));
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "User");
        return DateTime.Parse(ds.Tables[0].Rows[0]["MonthStartDate"].ToString());
    }
    public static DateTime AdjustBusinessDay(DateTime dtDate,int nDays)
    {
        SqlConnection cnClientDB = new SqlConnection(ConfigurationManager.ConnectionStrings["CnMasterFund"].ConnectionString);
        int iDefaultAUECID = 1; //In ideal world is should be picked up from T_AUEC

        SqlCommand command = new SqlCommand("Select dbo.AdjustBusinessDays('" + dtDate.ToString() + "'," + nDays.ToString() + "," + iDefaultAUECID.ToString() + ")", cnClientDB);
        command.CommandType = CommandType.Text;
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "User");
        return DateTime.Parse(ds.Tables[0].Rows[0][0].ToString());
    }

    public static DateTime getStartDate(DateTime dtDate)
    {
        SqlConnection cnClientDB = new SqlConnection(ConfigurationManager.ConnectionStrings["CnMasterFund"].ConnectionString);
        int iDefaultAUECID = 1; //In ideal world is should be picked up from T_AUEC

        SqlCommand command = new SqlCommand("Select dbo.AdjustBusinessDays(dbo.AdjustBusinessDays('" + dtDate.ToString() + "',-1," + iDefaultAUECID.ToString() + "),+1," + iDefaultAUECID.ToString() + ")", cnClientDB);
        command.CommandType = CommandType.Text;
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "User");
        return DateTime.Parse(ds.Tables[0].Rows[0][0].ToString());
    }
}
