using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.BusinessObjects.AppConstants;
using Infragistics.Win.UltraWinGrid;
using System.Xml.Serialization;
using Prana.Utilities.UI.UIUtilities;
using System.IO;
using Prana.Utilities.MiscUtilities;
using Prana.BusinessObjects;
using Prana.LogManager;

namespace Prana.MonitoringServices
{
    public partial class ManageConnectionsCtrl : UserControl
    {

        delegate void SetTextCallback(string ipAddress, int port);
        List<MonitoringConnection> _listMachines = new List<MonitoringConnection>();

        XmlSerializer SerializerObj = new XmlSerializer(typeof(List<MonitoringConnection>));
        CheckBoxOnHeader_CreationFilter headerChkbx = new CheckBoxOnHeader_CreationFilter();

        public ManageConnectionsCtrl()
        {
            InitializeComponent();
        }

        public void Initilise()
        {
            try
            {
                MonitoringCache.GetInstance.Initlise();

                DataBind();

                AddCheckBoxinGrid(grdData, headerChkbx);
                grdData.DisplayLayout.Bands[0].Columns["CheckBox"].CellClickAction = CellClickAction.Edit;
                SetUpAfterMakingConnection();
                foreach (UltraGridRow row in grdData.Rows)
                {
                    row.Cells["CheckBox"].Value = true;
                }
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
        }

        public void SetUpAfterMakingConnection()
        {
            try
            {
                grdData.DisplayLayout.Bands[0].Columns["IpAddress"].Hidden = true;
                grdData.DisplayLayout.Bands[0].Columns["Ports"].Hidden = true;
                MonitoringCache.GetInstance.Refresh += new MonitoringCache.RefreshDataHandler(MonitoringCache_Refresh);
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
        }

        private string GetPath()
        {
            return @"D:\NirvanaSourceCode\Dev\Prana\Application\Prana.MonitoringWebApplication\XmlConnection.xml";
        }

        private void DataBind()
        {
            grdData.DataSource = MonitoringCache.GetInstance.getAllConnections();
            grdData.DataBind();
            grdData.DisplayLayout.Bands[0].Columns["Name"].Header.Caption = "CustomerName";

            grdData.DisplayLayout.Bands[0].Columns[Servers.TradeServer.ToString()].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            grdData.DisplayLayout.Bands[0].Columns[Servers.TradeServer.ToString()].ButtonDisplayStyle = ButtonDisplayStyle.Always;
            grdData.DisplayLayout.Bands[0].Columns["Expnlserver"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            grdData.DisplayLayout.Bands[0].Columns["Expnlserver"].ButtonDisplayStyle = ButtonDisplayStyle.Always;
            grdData.DisplayLayout.Bands[0].Columns["PricingServer"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            grdData.DisplayLayout.Bands[0].Columns["PricingServer"].ButtonDisplayStyle = ButtonDisplayStyle.Always;
            grdData.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;
        }

        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            grid.CreationFilter = headerCheckBox;
            grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "");
            grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
            SetCheckBoxAtFirstPosition(grid);
        }

        private void SetCheckBoxAtFirstPosition(UltraGrid grid)
        {
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Hidden = false;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Header.VisiblePosition = 0;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].Width = 10;
            grid.DisplayLayout.Bands[0].Columns["checkBox"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

        }

        void MonitoringCache_Refresh(string ipAddress, int port)
        {
            try
            {
                if (grdData.InvokeRequired)
                {
                    SetTextCallback mi = new SetTextCallback(MonitoringCache_Refresh);
                    this.Invoke(mi, new object[] { ipAddress, port });
                }
                else
                {
                    MonitoringConnection connDetails = MonitoringCache.GetInstance.GetMachineDetails(ipAddress, port.ToString());

                    foreach (UltraGridRow row in grdData.Rows)
                    {
                        if (row.ListObject == connDetails)
                        {
                            string cellheader = MonitoringCache.GetInstance.GetServiceName(ipAddress, port);
                            if (cellheader != string.Empty)
                            {
                                row.Cells[cellheader].ButtonAppearance.BackColor = Color.Orange;
                            }
                            UpdateRow(connDetails, row, port);
                            break;
                        }
                    }
                }
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
        }

        private string GetIPPortKey(UltraGridCell cell)
        {
            string serviceName = cell.Column.Key.ToString();
            String machineName = cell.Row.Cells["Name"].Value.ToString();
            return MonitoringCache.GetInstance.GetIPPortKeyForMachineAndService(machineName, serviceName);
        }

        private void grdData_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                string ipaddress = e.Cell.Row.Cells["IpAddress"].Value.ToString();
                e.Cell.ButtonAppearance.BackColor = Color.LightGray;
                string ipportKey = GetIPPortKey(e.Cell);
                MonitoringConnection conn = (MonitoringConnection)e.Cell.Row.ListObject;

                SetDetailsGrids(conn, ipportKey);
                if (e.Cell.Value.ToString() == "Connect")
                {
                    SetCellValue(e.Cell, "Connecting...");
                    e.Cell.ButtonAppearance.BackColor = Color.LightGray;
                    MonitoringCache.GetInstance.ConnectToServer(ipportKey);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_EVENTLOGONLYPOLICY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetCellValue(UltraGridCell cell, string cellvalue)
        {
            cell.Value = cellvalue;
        }

        private void SetDetailsGrids(MonitoringConnection conn, string ipportKey)
        {
            try
            {
                ServiceClient client = MonitoringCache.GetInstance.GetServiceClient(ipportKey);

                if (client != null)
                {
                    lblDetailsStatus.Text = MonitoringCache.GetInstance.GetMachineName(ipportKey);
                    grdUsers.DataSource = client.GetChilds(ConnectedEntityTypes.Users);
                    grdUsers.DataBind();
                    List<string> columns = new List<string>();
                    columns.Add("IdentifierName");
                    columns.Add("Status");
                    Prana.Utilities.UI.UIUtilities.UltraWinGridUtils.SetColumns(columns, grdUsers);
                    grdUsers.DisplayLayout.Bands[0].Columns["IdentifierName"].Header.Caption = "Name";
                    SetCellColor(grdUsers);
                    List<ServiceClient> missConn = new List<ServiceClient>();
                    switch (MonitoringCache.GetInstance.GetServiceName(ipportKey))
                    {
                        case "TradeServer":
                            missConn = client.GetChilds(ConnectedEntityTypes.FixSessions);
                            break;
                        default:
                            missConn = client.GetChilds(ConnectedEntityTypes.MiscConnection);
                            break;
                    }

                    grdSessionstatus.DataSource = missConn;
                    grdSessionstatus.DataBind();
                    columns = new List<string>();
                    columns.Add("IdentifierName");
                    columns.Add("Status");
                    Prana.Utilities.UI.UIUtilities.UltraWinGridUtils.SetColumns(columns, grdSessionstatus);
                    grdSessionstatus.DisplayLayout.Bands[0].Columns["IdentifierName"].Header.Caption = "Name";
                    SetCellColor(grdSessionstatus);

                    grdErrors.DataSource = MonitoringCache.GetInstance.GetExceptions(ipportKey);
                    grdErrors.DataBind();
                    grdErrors.DisplayLayout.Bands[0].Columns[0].Header.Caption = "Error";
                }
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
        }

        private void SetCellColor(UltraGrid grid)
        {
            foreach (UltraGridRow row in grid.Rows)
            {
                if (row.Cells["Status"].Value.ToString() == "CONNECTED")
                {
                    row.Cells["Status"].Appearance.BackColor = Color.Green;
                }
                else
                {
                    row.Cells["Status"].Appearance.BackColor = Color.Red;
                }
            }
        }

        public void UpdateRow(MonitoringConnection connDetails, UltraGridRow row, int port)
        {
            try
            {
                string columnKey = MonitoringCache.GetInstance.GetServiceName(connDetails.IpAddress, port);
                ServiceClient mainNode = MonitoringCache.GetInstance.GetServiceClient(connDetails.IpAddress, port);
                string status = string.Empty;
                if (mainNode != null && mainNode.Status == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    string users = mainNode.GetConnectedUsers(ConnectedEntityTypes.Users) + " Users";

                    string sessions = string.Empty;
                    switch (columnKey)
                    {
                        case "TradeServer":
                            sessions = mainNode.GetConnectedUsers(ConnectedEntityTypes.FixSessions) + " FixSessions";
                            break;
                        default:
                            sessions = mainNode.GetConnectedUsers(ConnectedEntityTypes.MiscConnection) + " MiscConnection";
                            break;
                    }

                    string errors = MonitoringCache.GetInstance.GetExceptions(connDetails.IpAddress, port).Count + " Errors";
                    status = users + " " + sessions + " " + errors;
                }
                else
                {
                    status = "Connect";
                    row.Cells[columnKey].ButtonAppearance.BackColor = Color.Red;
                }
                SetCellValue(row.Cells[columnKey], status);
                SetDetailsGrids(connDetails, port.ToString());
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
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                btnConnect.Enabled = false;
                btnSave_Click(null, null);
                foreach (UltraGridRow row in grdData.Rows)
                {
                    if (row.Cells["CheckBox"].Value.ToString() == "True")
                    {
                        MonitoringConnection connection = (MonitoringConnection)row.ListObject;

                        string[] services = connection.ServiceNames.Split(',');
                        foreach (String service in services)
                        {
                            string ipportKey = GetIPPortKey(row.Cells[service]);
                            MonitoringCache.GetInstance.ConnectToServer(ipportKey);
                        }
                    }
                }
                btnConnect.Enabled = true;
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
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult userChoice = MessageBox.Show("Do you want to delete selected Rows", "Prana:Warning", MessageBoxButtons.YesNo);
                if (userChoice == DialogResult.Yes)
                {
                    foreach (UltraGridRow row in grdData.Rows)
                    {
                        if (row.Cells["checkBox"].Value.ToString().Equals("TRUE", StringComparison.CurrentCultureIgnoreCase))
                        {
                            MonitoringConnection conn = (MonitoringConnection)row.ListObject;
                        }
                    }
                    DataBind();
                    btnSave_Click(null, null);
                }
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                MonitoringCache.GetInstance.SaveSettings();
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
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewServerCtrl addNewCtrl = new AddNewServerCtrl();
                addNewCtrl.ServerAdded += new MonitoringCache.ServerAdditionHandler(addNewCtrl_ServerAdded);
                addNewCtrl.ShowDialog(this);
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
        }

        void addNewCtrl_ServerAdded(MonitoringConnection conn)
        {
            try
            {
                MonitoringCache.GetInstance.AddIpNode(conn);
                DataBind();
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
        }
    }
}
