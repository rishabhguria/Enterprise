using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Prana.MonitoringServices;
using System.Collections.Generic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System.Drawing;

public partial class _Default : System.Web.UI.Page 
{
    int selectedRowIndex=-1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            selectedRowIndex = -1;
            MonitoringCache.GetInstance.setPath(Server.MapPath(@"XmlConnection.xml"));
            MonitoringCache.GetInstance.Initlise();
            BindDataInGrid();
            MonitoringCache.GetInstance.Refresh += new MonitoringCache.RefreshDataHandler(GetInstance_Refresh);
        }
    }

    void GetInstance_Refresh(string ipAddress, int port)
    {
        //MonitoringConnection connDetails = MonitoringCache.GetInstance.GetMachineDetails(ipAddress, port.ToString());


        //foreach (GridViewRow  row in grdAllConnections.Rows)
        //{

        //    if (getMonitoringConnectionFromGridRow(row).IpAddress == connDetails.IpAddress)
        //    {
        //        SetRowColor(row);
        //       // UpdateRow(connDetails, row, port);

        //        break;
        //    }
        //}
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (((LinkButton)grdAllConnections.Rows[0].Cells[0].Controls[0]).Text == "Insert")
        {
            MonitoringConnection con = getMonitoringConnectionFromEditableGridRow(grdAllConnections.Rows[e.RowIndex]);
           
            MonitoringCache.GetInstance.AddIpNode(con);
        }
        else
        {
            MonitoringConnection con = getMonitoringConnectionFromEditableGridRow(grdAllConnections.Rows[e.RowIndex]);
            MonitoringCache.GetInstance.RemoveConnection(con.Name);
            MonitoringCache.GetInstance.AddIpNode(con);
        }


        grdAllConnections.EditIndex = -1;
        BindDataInGrid();
        Timer1.Enabled = true ;
    }
    private MonitoringConnection getMonitoringConnectionFromEditableGridRow(GridViewRow row)
    {
        MonitoringConnection con = new MonitoringConnection();
        con.IpAddress =((TextBox)row.Cells[1].Controls[0]).Text;
        con.Name = ((TextBox)row.Cells[2].Controls[0]).Text;
        con.Ports = ((TextBox)row.Cells[3].Controls[0]).Text;
        con.ServiceNames  = ((TextBox)row.Cells[4].Controls[0]).Text;
        return con;
       
    }
    private MonitoringConnection getMonitoringConnectionFromGridRow(GridViewRow row)
    {
        MonitoringConnection con = new MonitoringConnection();
        con.IpAddress = row.Cells[1].Text;
        con.Name = row.Cells[2].Text;
        con.Ports = row.Cells[3].Text;
        con.ServiceNames =row.Cells[4].Text;
        return con;

    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdAllConnections.EditIndex = -1;
        BindDataInGrid();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
       
        grdAllConnections.EditIndex = e.NewEditIndex;
       
        BindDataInGrid();
        ((TextBox)grdAllConnections.Rows[e.NewEditIndex].Cells[2].Controls[0]).Enabled = false;
        Timer1.Enabled = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        MonitoringCache.GetInstance.SaveSettings();
        Timer1.Enabled = true;
    }
    private void BindDataInGrid()
    {
        grdAllConnections.DataSource = MonitoringCache.GetInstance.getAllConnections();
        grdAllConnections.DataBind();
        foreach (GridViewRow row in grdAllConnections.Rows)
        {
            SetRowColor(row);
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        
        grdAllConnections.EditIndex = 0;
        
        List<MonitoringConnection> list = MonitoringCache.GetInstance.getAllConnections();
        MonitoringConnection conn=new MonitoringConnection();
        conn.Ports = "5000,5001,5002";
        conn.ServiceNames = "TradeServer,ExpnlServer,PricingServer";
        list.Insert(0, conn);
        grdAllConnections.DataSource = list;
        grdAllConnections.DataBind();
        ((LinkButton)grdAllConnections.Rows[0].Cells[0].Controls[0]).Text = "Insert";

        Timer1.Enabled = false ;

    }
    
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "ConnectToTradeServer":
            case "ConnectToExpnlServer":
            case "ConnectToPricingServer":
                {
                    selectedRowIndex = Convert.ToInt32(e.CommandArgument);
                    string serverName = e.CommandName.Substring(9);
                    if (getServerStatus(grdAllConnections.Rows[selectedRowIndex], serverName) != PranaInternalConstants.ConnectionStatus.CONNECTED)
                    {
                        MonitoringConnection con = getMonitoringConnectionFromGridRow(grdAllConnections.Rows[selectedRowIndex]);
                        MonitoringCache.GetInstance.ConnectToServer(GetIPPortKey(con, serverName));
                    }
                    else
                    {
                        MonitoringConnection con = getMonitoringConnectionFromGridRow(grdAllConnections.Rows[selectedRowIndex]);
                        MonitoringCache.GetInstance.Disconnect(GetIPPortKey(con, serverName));
                    }

                }

                break;

            case "ShowDetails":
                {
                    selectedRowIndex = Convert.ToInt32(e.CommandArgument);
                    SetServerStatus(selectedRowIndex);
                    //lbltest.Text = "test" +DateTime.Now.ToString();
                }
                break;
            case "Delete":
                selectedRowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow deleteRow = grdAllConnections.Rows[selectedRowIndex];
               MonitoringConnection conn= getMonitoringConnectionFromGridRow(deleteRow);
               MonitoringCache.GetInstance.RemoveConnection(conn.Name);
               BindDataInGrid();
                break;
            
               
               
        }
        Session.Add("selectedRowIndex", selectedRowIndex);
    }
    private void SetServerStatus(int index)
    {
        if (grdAllConnections.Rows.Count  > index)
        {
            MonitoringConnection con = getMonitoringConnectionFromGridRow(grdAllConnections.Rows[index]);
            //MonitoringCache.GetInstance.ConnectToServer(GetIPPortKey(con, "TradeServer"));
            SetDetailsGrids(GridViewUsers, con, GetIPPortKey(con, Servers.TradeServer.ToString()));
            SetDetailsGrids(grdPricingServer, con, GetIPPortKey(con, Servers.PricingServer.ToString()));
            SetDetailsGrids(grdExpnlUsers, con, GetIPPortKey(con, Servers.ExpnlServer.ToString()));

            SetMiscConnGrids(grdPricingMiscConn, con, GetIPPortKey(con, Servers.PricingServer.ToString()));
            SetMiscConnGrids(grdExpnlMiscConn, con, GetIPPortKey(con, Servers.ExpnlServer.ToString()));

            SetExceptionDetails(grdTradeServerErrors, GetIPPortKey(con, Servers.TradeServer.ToString()));
            SetExceptionDetails(grdExpnlServerErrors, GetIPPortKey(con, Servers.ExpnlServer.ToString()));
            SetExceptionDetails(grdPricingServerErrors, GetIPPortKey(con, Servers.PricingServer.ToString()));


            SetSessionDetails(GetIPPortKey(con, Servers.TradeServer.ToString()));
            SetDetailsGridColors(GridViewUsers);
            SetDetailsGridColors(GridViewFixConn);

            SetDetailsGridColors(grdExpnlUsers);
            SetDetailsGridColors(grdExpnlMiscConn);

            SetDetailsGridColors(grdPricingServer);
            SetDetailsGridColors(grdPricingMiscConn);
        }
       
    }
    private void SetDetailsGridColors(GridView grd)
    {
        foreach (GridViewRow row in grd.Rows)
        {
            SetDetailsRowColor(row);
        }
    }
    private string GetIPPortKey(MonitoringConnection con,string serviceName)
    {

        //string serviceName = ""; //((ButtonField)cell.Controls[0]).HeaderText;
        String machineName = con.Name;// cell.RowSpan.Cells["Name"].Value.ToString();


        return MonitoringCache.GetInstance.GetIPPortKeyForMachineAndService(machineName, serviceName);


    }
    private void SetRowColor(GridViewRow row)
    {
        LinkButton tradedServerButton=((LinkButton)row.Cells[(int)Cellposition.TradeServer].Controls[0]);
        LinkButton expnlServerButton = ((LinkButton)row.Cells[(int)Cellposition.ExpnlServer].Controls[0]);
        LinkButton pricingServerButton = ((LinkButton)row.Cells[(int)Cellposition.PricingServer].Controls[0]);
        if (getServerStatus(row,Servers.TradeServer.ToString()) == PranaInternalConstants.ConnectionStatus.CONNECTED)
        {
            tradedServerButton.Text = "Disconnect";
            tradedServerButton.BackColor = Color.Green ;
        }
        else 
        {
            tradedServerButton.Text = "Connect";
            tradedServerButton.BackColor = Color.Red;
        }
        if (getServerStatus(row, Servers.ExpnlServer.ToString()) == PranaInternalConstants.ConnectionStatus.CONNECTED)
        {
            expnlServerButton.Text = "Disconnect";
            expnlServerButton.BackColor = Color.Green;
        }
        else
        {
            expnlServerButton.Text = "Connect";
            expnlServerButton.BackColor = Color.Red;
        }
        if (getServerStatus(row, Servers.PricingServer.ToString()) == PranaInternalConstants.ConnectionStatus.CONNECTED)
        {
            pricingServerButton.Text = "Disconnect";
            pricingServerButton.BackColor = Color.Green;
        }
        else
        {
            pricingServerButton.Text = "Connect";
            pricingServerButton.BackColor = Color.Red;
        }
    }
    private void SetDetailsRowColor(GridViewRow row)
    {
        
        if (row.Cells[1].Text  == PranaInternalConstants.ConnectionStatus.CONNECTED.ToString())
        {
            row.Cells[1].BackColor = Color.Green;
        }
        else
        {
            row.Cells[1].BackColor = Color.Red;
        }
      
    }
    private PranaInternalConstants.ConnectionStatus getServerStatus(GridViewRow row,String serviceName)
    {
          MonitoringConnection con = getMonitoringConnectionFromGridRow(row);
          string ipportkey = GetIPPortKey(con, serviceName);
         if (ipportkey != string.Empty)
         {
             return MonitoringCache.GetInstance.GetServiceClient(ipportkey).Status;
         }
         else
         {
             return PranaInternalConstants.ConnectionStatus.DISCONNECTED;
         }
    }
    private void SetDetailsGrids(GridView grd, MonitoringConnection conn, string ipportKey)
    {
        try
        {
            ServiceClient client = MonitoringCache.GetInstance.GetServiceClient(ipportKey);

            if (client != null)
            {

               // lblDetailsStatus.Text = MonitoringCache.GetInstance.GetMachineName(ipportKey);
                grd.DataSource = null;
                grd.DataSource = client.GetChilds(ConnectedEntityTypes.Users);
                grd.DataBind();
                
            }
        }
        catch (Exception ex)
        {

            
        }
    }
    private void SetMiscConnGrids(GridView grd, MonitoringConnection conn, string ipportKey)
    {
        try
        {
            ServiceClient client = MonitoringCache.GetInstance.GetServiceClient(ipportKey);

            if (client != null)
            {
                grd.DataSource = null;
                // lblDetailsStatus.Text = MonitoringCache.GetInstance.GetMachineName(ipportKey);
                grd.DataSource = client.GetChilds(ConnectedEntityTypes.MiscConnection);
                grd.DataBind();

            }
        }
        catch (Exception ex)
        {


        }
    }

    private void SetExceptionDetails(GridView grd, string ipportKey)
    {
        try
        {
            // ServiceClient client = MonitoringCache.GetInstance.GetServiceClient(ipportKey);
            // lblDetailsStatus.Text = MonitoringCache.GetInstance.GetMachineName(ipportKey);
            grd.DataSource = null;
            grd.DataSource = MonitoringCache.GetInstance.GetExceptions(ipportKey);
            grd.DataBind();


        }
        catch (Exception ex)
        {


        }
    }
    private void SetSessionDetails( string ipportKey)
    {
        try
        {
            ServiceClient client = MonitoringCache.GetInstance.GetServiceClient(ipportKey);
            List<ServiceClient> missConn = new List<ServiceClient>();
            if (client != null)
            {
                missConn = client.GetChilds(ConnectedEntityTypes.FixSessions);
                // lblDetailsStatus.Text = MonitoringCache.GetInstance.GetMachineName(ipportKey);
                GridViewFixConn.DataSource = null;
                GridViewFixConn.DataSource = missConn;
                GridViewFixConn.DataBind();

            }
        }
        catch (Exception ex)
        {


        }
    }
    enum Cellposition
    {
        TradeServer=5,
        ExpnlServer=6,
        PricingServer=7
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdAllConnections.Rows)
        {
            SetRowColor(row);
        }
        if (Session["selectedRowIndex"] != null)
        {
            int.TryParse(Session["selectedRowIndex"].ToString(), out selectedRowIndex);
            if (selectedRowIndex != -1)
            {
                SetServerStatus(selectedRowIndex);
            }
        }
    }
    protected void grdViewUsers_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
