using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTree;
using Prana.Admin.BLL;
using Prana.BusinessObjects.CommonObjects;
using Prana.LogManager;
using Prana.Utilities.ImportExportUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.AdminForms
{
    public partial class CalendarHolidays : Form
    {

        private static CalendarHolidays _cHolidays = null;
        private static object locker = new object();

        //Global Variables to check Unsaved Data
        bool _flag = false;
        bool _previous = false;
        bool _textchanged = false;

        private int _year;
        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public Calendars _calcollection = AUECManager.GetCalendar();

        public CalendarHolidays()
        {
            InitializeComponent();
        }

        public static CalendarHolidays GetInstance()
        {
            lock (locker)
            {
                if (_cHolidays == null)
                {
                    _cHolidays = new CalendarHolidays();
                }
            }
            return _cHolidays;
        }

        private void CalendarHolidays_Load(object sender, EventArgs e)
        {
            BindTree();
            BindCalendar();
        }

        private void BindTree()
        {
            try
            {
                //Bind all the years as nodes in which calendars exists
                trvCalendar.Nodes.Clear();
                List<int> year = new List<int>();
                foreach (Calendar cal in _calcollection)
                {
                    if (!year.Contains(cal.CalendarYear))
                    {
                        year.Add(cal.CalendarYear);
                    }
                }

                if (year.Count != 0)
                {
                    year.Sort();
                    foreach (int item in year)
                    {
                        UltraTreeNode nodeYear = new UltraTreeNode(item.ToString(), item.ToString());
                        trvCalendar.Nodes.Add(nodeYear);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindCalendar()
        {
            try
            {
                foreach (UltraTreeNode nodeYear in trvCalendar.Nodes)
                {
                    Calendars calendars = GetCalendars(int.Parse(nodeYear.Key.ToString()));
                    foreach (Calendar calendar in calendars)
                    {
                        string name = calendar.CalendarName;
                        //unique key for each calendar
                        string calendarKey = calendar.CalendarID.ToString();
                        UltraTreeNode nodeCalendar = new UltraTreeNode(calendarKey, name);
                        nodeYear.Nodes.Add(nodeCalendar);
                    }
                }
                trvCalendar.ExpandAll();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void BindDateRange(int year)
        {
            try
            {
                //restrict dates to the particular calendar year
                DateTime minDate = Convert.ToDateTime("01/01/" + year.ToString());
                DateTime maxDate = Convert.ToDateTime("12/31/" + year.ToString());
                if (minDate > dtpicker.MaxDate)
                {
                    dtpicker.MaxDate = maxDate;
                    dtpicker.MinDate = minDate;
                }
                else
                {
                    dtpicker.MinDate = minDate;
                    dtpicker.MaxDate = maxDate;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private string AuecAssociated(string name, int year)
        {
            //Return all the AUECs associated with each calendar as a string.
            string auecsAssociated = "AUEC ASSOCIATED: ";
            try
            {
                AUECs auecs = AUECManager.auecAssociated(name, year);

                foreach (AUEC auec in auecs)
                {
                    auecsAssociated += auec.DisplayName + ", ";
                }
                int count = auecsAssociated.LastIndexOf(',');
                if (count > 0)
                {
                    //Remove the last comma from the string.
                    auecsAssociated = auecsAssociated.Substring(0, count);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return auecsAssociated;
        }

        private void ControlsEnabled(bool value)
        {
            //Check whether the selected node is a calendar or a year and enable the controls accordingly..
            txtYearValue.ReadOnly = value;
            grpAdd.Enabled = value;
            grpCalendar.Enabled = value;
            grdHoliday.Enabled = value;
            grdHoliday.DataSource = null;
        }

        private void trvCalendar_AfterSelect(object sender, SelectEventArgs e)
        {
            try
            {
                //Variables checking unsaved data are set to false ....
                _flag = false;
                _textchanged = false;

                lblAuecAssociated.Text = string.Empty;
                int counter = int.MinValue;

                if (e.NewSelections.Count > 0)
                {
                    toolStripStatusLabel2.Text = string.Empty;
                    UltraTreeNode node = (UltraTreeNode)e.NewSelections.All[0];


                    if (node.Parent != null)
                    {
                        txtCalendar.Text = node.Text;
                        txtYearValue.Text = node.Parent.Key;
                        ResetTreeAfterSave(node.Text, node.Parent.Key);

                        ControlsEnabled(true);

                        int year = int.Parse(txtYearValue.Text);

                        BindDateRange(year);
                        dtpicker.Value = dtpicker.MinDate;

                        Holidays holidays = AUECManager.GetCalendarHolidays(int.Parse(node.Key));


                        if (holidays.Count != 0)
                        {
                            if (holidays.Count == 1)
                            {
                                foreach (Holiday holiday in holidays)
                                {
                                    if (holiday.Date == Prana.BusinessObjects.DateTimeConstants.MinValue)
                                    {
                                        BindColumns();
                                    }
                                    else
                                    {
                                        counter++;
                                    }
                                }
                            }
                            else
                            {
                                counter++;
                            }
                            if (counter != int.MinValue)
                            {
                                grdHoliday.DataSource = holidays;
                                showColumns(grdHoliday);
                            }

                            lblAuecAssociated.Text = AuecAssociated(node.Text, year);

                        }
                        else
                        {
                            lblAuecAssociated.Text = AuecAssociated(node.Text, year);
                            BindColumns();
                            showColumns(grdHoliday);
                        }

                    }

                    else
                    {
                        DefaultSetUp(node);
                    }


                    _flag = false;
                    _textchanged = false;
                }
            }



            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void DefaultSetUp(UltraTreeNode node)
        {
            grpCalendar.Enabled = false;
            txtYearValue.Text = "";
            txtCalendar.Text = "";
            grdHoliday.DataSource = null;
            grpAdd.Enabled = false;
            BindDateRange(int.Parse(node.Key));
            dtpicker.Value = dtpicker.MinDate;

        }

        private void showColumns(UltraGrid grid)
        {
            //Show only date and description columns to the grid
            ColumnsCollection columns = grdHoliday.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn col in columns)
            {
                if (col.Key != "Date")
                {
                    if (col.Key != "Description")
                    {
                        // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                        if (col.Key != "MarketOff")
                        {
                            if (col.Key != "SettlementOff")
                            {
                                col.Hidden = true;
                            }
                        }
                    }
                }
            }

            UltraGridColumn colDate = grdHoliday.DisplayLayout.Bands[0].Columns["Date"];
            colDate.Header.VisiblePosition = 1;
            colDate.Hidden = false;
            colDate.CellActivation = Activation.AllowEdit;

            UltraGridColumn colDescription = grdHoliday.DisplayLayout.Bands[0].Columns["Description"];
            colDescription.Header.VisiblePosition = 2;
            colDescription.Hidden = false;
            colDescription.CellActivation = Activation.AllowEdit;

            UltraGridColumn colMarketOff = grdHoliday.DisplayLayout.Bands[0].Columns["MarketOff"];
            colMarketOff.Header.VisiblePosition = 3;
            colMarketOff.Hidden = false;
            // colMarketOff.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            colMarketOff.CellActivation = Activation.AllowEdit;

            UltraGridColumn colSettlementOff = grdHoliday.DisplayLayout.Bands[0].Columns["SettlementOff"];
            colSettlementOff.Header.VisiblePosition = 4;
            // colSettlementOff.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            colSettlementOff.Hidden = false;
            colSettlementOff.CellActivation = Activation.AllowEdit;

        }

        private void BindColumns()
        {
            try
            {
                DataTable dt = new DataTable();
                DataColumn colDate = new DataColumn("Date");
                DataColumn colDescription = new DataColumn("Description");

                // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                DataColumn colMarketOff = new DataColumn("MarketOff");
                DataColumn colSettlementOff = new DataColumn("SettlementOff");
                dt.Columns.Add(colDate);
                dt.Columns.Add(colDescription);
                dt.Columns.Add(colMarketOff);
                dt.Columns.Add(colSettlementOff);
                grdHoliday.DataSource = dt;

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void btnAddHoliday_Click(object sender, EventArgs e)
        {
            //Add holiday to the grid
            try
            {
                if (txtDescription.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Description", "Prana Alert");
                }
                else
                {
                    bool flag = false;
                    bool count = false;

                    if (grdHoliday.Rows.Count != 0)
                    {
                        Holidays holidays = ((Holidays)grdHoliday.DataSource);
                        foreach (Holiday holiday in holidays)
                        {
                            if (holiday.Date.ToShortDateString() == dtpicker.Value.ToShortDateString())
                            {
                                //Check whether holiday is already present in calendar or not
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (grdHoliday.Rows.Count == 0)
                    {
                        Holidays hDays = new Holidays();

                        // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
                        Holiday holiday1 = new Holiday(dtpicker.Value, txtDescription.Text, cMarketOff.Checked, cSettlementOff.Checked);
                        hDays.Add(holiday1);

                        grdHoliday.DataSource = hDays;
                        grdHoliday.DataBind();

                        showColumns(grdHoliday);

                        count = true;
                        _flag = true;

                    }

                    if (count == false)
                    {
                        if (flag == false)
                        {
                            //Add next holiday to the grid
                            Holidays holidays = ((Holidays)grdHoliday.DataSource);
                            Holiday holiday1 = new Holiday(dtpicker.Value, txtDescription.Text, cMarketOff.Checked, cSettlementOff.Checked);
                            holidays.Add(holiday1);

                            this.grdHoliday.DataSource = null;
                            this.grdHoliday.DataSource = holidays;
                            grdHoliday.DataBind();

                            showColumns(grdHoliday);
                            _flag = true;
                        }
                        else
                        {
                            MessageBox.Show("Date " + dtpicker.Value.ToShortDateString() + " already exists in the List!", "Warning");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void grdHoliday_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void grdHoliday_MouseDown(object sender, MouseEventArgs e)
        {
            //To add delete row functionality to the grid rows
            if (e.Button == MouseButtons.Right)
            {
                Point mousePoint = new Point(e.X, e.Y);
                UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                if (element is TextUIElement)
                {
                    UltraGridRow row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;
                    if (row != null)
                    {
                        row.Activate();
                    }
                    MenuItem mnuDelete = new MenuItem("Delete");
                    mnuDelete.Click += new EventHandler(mnuDelete_Click);
                    ContextMenu cmnu = new ContextMenu(new MenuItem[] { mnuDelete });
                    grdHoliday.ContextMenu = cmnu;
                }
            }
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            if (grdHoliday.ActiveRow != null)
            {
                grdHoliday.ActiveRow.Delete();
                _flag = true;
            }
            else
            {
                toolStripStatusLabel2.Text = "No rows to delete";
            }
        }

        private Calendars GetCalendars(int year)
        {
            Calendars calendars = new Calendars();
            foreach (Calendar calendar in _calcollection)
            {
                if (calendar.CalendarYear.Equals(year))
                    calendars.Add(calendar);
            }
            return calendars;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel2.Text = "";
                UltraTreeNode nodeCalendar;

                if ((_previous) && (trvCalendar.Nodes.Count > 0) && (trvCalendar.SelectedNodes.Count > 0))
                {
                    nodeCalendar = (UltraTreeNode)trvCalendar.SelectedNodes.All[0];
                    _previous = false;
                }
                else
                {
                    nodeCalendar = trvCalendar.ActiveNode;
                }
                if (nodeCalendar != null)
                {
                    string calendarName = txtCalendar.Text;
                    int year = int.MinValue;
                    int ID = int.Parse(nodeCalendar.Key);

                    if (nodeCalendar.Parent != null)
                    {
                        if (calendarName != string.Empty)
                        {
                            AUECs auecs = null;

                            year = int.Parse(nodeCalendar.Parent.Key);

                            if (_textchanged)
                            {
                                int count = int.MinValue;
                                int index = int.MinValue;
                                Calendar c = null;
                                year = int.Parse(nodeCalendar.Parent.Key);

                                Calendars calendars = GetCalendars(year); //Year Wise Calendars...

                                foreach (Calendar calendar in calendars)
                                {
                                    //check for same calendarname
                                    if (calendar.CalendarName == calendarName)
                                    {
                                        count++;
                                    }
                                    if (calendar.CalendarID.Equals(ID))
                                    {
                                        c = calendar;
                                        index = _calcollection.IndexOf(calendar);
                                    }
                                }
                                if (count == int.MinValue)
                                {
                                    if (nodeCalendar.Text != txtCalendar.Text)
                                    {
                                        //Remove that calendar from that collection that needs to be updated..
                                        _calcollection.Remove(c);

                                        //Update the CalendarName
                                        Calendar calendar = new Calendar(ID, calendarName, year);
                                        AUECManager.SaveCalendar(calendar);
                                        toolStripStatusLabel2.Text = "Calendar renamed to " + calendarName;
                                        //Update The Calendar Collection
                                        _calcollection.Insert(index, calendar);

                                        _textchanged = false;
                                        RefreshTree(calendarName, year.ToString());
                                    }
                                }
                                else
                                {
                                    if (nodeCalendar.Text != txtCalendar.Text)
                                    {
                                        DialogResult result = MessageBox.Show("Calendar with same name already exists! Change Calendar Name!", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                                        if (result.Equals(DialogResult.Cancel))
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            txtCalendar.Focus();
                                            trvCalendar.ActiveNode = nodeCalendar;
                                            return;
                                        }
                                    }
                                }
                            }

                            if (_flag)
                            {
                                Holidays newHolidays = ((Holidays)grdHoliday.DataSource);

                                //Associated AUECs with calendar
                                auecs = AUECManager.auecAssociated(calendarName, year);

                                //Update T_AuecHolidays & T_CalendarHolidays in Database.
                                AUECManager.UpdateCalendarHolidays(newHolidays, ID);
                                AUECManager.UpdateAuecHolidays(newHolidays, year, auecs);

                                toolStripStatusLabel2.Text = "Calendar Saved at " + DateTime.Now.ToString();
                            }

                        }
                        _flag = false;
                        if (calendarName.Equals(string.Empty))
                        {
                            toolStripStatusLabel2.Text = "Enter Calendar Name";
                        }
                    }



                    if (nodeCalendar.Parent == null) // selected node is year node
                    {
                        if (calendarName.Equals(string.Empty))
                        {
                            toolStripStatusLabel2.Text = "Enter Calendar Name";
                        }
                        else
                        {
                            int count = int.MinValue;
                            year = int.Parse(nodeCalendar.Key);

                            Calendars calendars = GetCalendars(year);
                            foreach (Calendar calendar in calendars)
                            {
                                if (calendar.CalendarName.ToLowerInvariant() == calendarName.ToLowerInvariant())
                                {
                                    count++;
                                }
                            }

                            //No such calendar exists in that Year
                            if (count == int.MinValue)
                            {
                                //Save New Calendar in T_Calendar
                                Calendar calendar = new Calendar(int.MinValue, calendarName, year);
                                AUECManager.SaveCalendar(calendar);

                                //Update the Calendar Collection
                                //_calcollection.Add(calendar);
                                _calcollection.Clear();
                                _calcollection = AUECManager.GetCalendar();
                                _textchanged = false;

                                //Save the calendar holidays in T_CalendarHolidays Table
                                if (grdHoliday.Rows.Count != 0)
                                {
                                    Holidays holidays = ((Holidays)grdHoliday.DataSource);
                                    AUECManager.SaveCalendarHolidays(holidays);

                                    toolStripStatusLabel2.Text = "Calendar Saved at " + DateTime.Now.ToString();

                                }
                                _flag = false;
                                RefreshTree(calendarName, year.ToString());
                            }

                            //CalendarName already exists in that year
                            if (count != int.MinValue)
                            {
                                MessageBox.Show("Calendar Already exists");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            _flag = false;
        }

        private void RefreshTree(string name, string year)
        {
            //Refresh the tree by adding or updating nodes in trvcalendar
            BindTree();
            BindCalendar();
            trvCalendar.Update();
            trvCalendar.Refresh();
            ResetTreeAfterSave(name, year);
        }

        private void btnAddCalendar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_flag || _textchanged)
                {
                    //Unsaved Data check
                    DialogResult result = MessageBox.Show("Do you want to save the unsaved data?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        _previous = true;
                        btnSave_Click(this.btnSave, e);
                    }
                    else
                    {
                        _flag = false;
                        _textchanged = false;
                    }
                }
                UltraTreeNode nodeYear = (UltraTreeNode)trvCalendar.ActiveNode;
                if (nodeYear != null)
                {
                    if (nodeYear.Parent != null)
                    {
                        nodeYear = nodeYear.Parent;
                        trvCalendar.ActiveNode = nodeYear;
                    }

                    ControlsEnabled(true);
                    txtCalendar.Text = "";
                    lblAuecAssociated.Text = "";

                    BindColumns();
                    if (nodeYear.Parent != null)
                    {
                        BindDateRange(int.Parse(nodeYear.Parent.Key));
                        txtYearValue.Text = nodeYear.Parent.Key;
                    }
                    else
                    {
                        BindDateRange(int.Parse(nodeYear.Key));
                        txtYearValue.Text = nodeYear.Key;
                    }
                    dtpicker.Value = dtpicker.MinDate;
                    _flag = true;
                    txtCalendar.Focus();
                }
                else
                {
                    toolStripStatusLabel2.Text = "Please click Add Year before adding calendar";
                    btnAddYear.Focus();
                }
            }

            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            if (_flag || _textchanged)
            {
                //Unsaved Data check
                DialogResult result = MessageBox.Show("Do you want to save the unsaved Data?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    btnSave_Click(this.btnSave, e);
                    this.Close();
                }

                if (result == DialogResult.No)
                {
                    _flag = false;
                    _textchanged = false;
                    //this.Close();
                }
                else
                {
                    FormClosingEventArgs formClosing = new FormClosingEventArgs(CloseReason.UserClosing, true);
                }
            }
            else
            {
                this.Close();
            }
        }


        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = ofdCalendar.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string fileName = ofdCalendar.FileName;

                    Holidays holidays = new Holidays();

                    DataTable dt = GetDataTableFromDifferentFileFormats(fileName);
                    Holidays holidaysList = new Holidays();
                    if (dt != null)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Holiday holiday = new Holiday();
                            string holidayDate = row.ItemArray[0].ToString();

                            string holidayDescription = row.ItemArray[1].ToString();

                            if (!string.IsNullOrEmpty(holidayDate))
                            {
                                string[] splitDateFieldSlash = holidayDate.Split('/');
                                if (splitDateFieldSlash.Length == 1)
                                {
                                    string[] splitDateFieldWithDash = holidayDate.Split('-');
                                    if (splitDateFieldWithDash.Length == 1)
                                    {
                                        bool blnIsTrue;
                                        double value;
                                        blnIsTrue = double.TryParse(holidayDate, out value);
                                        if (blnIsTrue)
                                        {
                                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(holidayDate));//.ParseExact(positionMaster.PositionStartDate, "yyyyMMdd", null);
                                            holiday.Date = dtn;
                                        }
                                    }
                                }
                            }

                            holiday.Description = row.ItemArray[1].ToString();
                            if (holiday.Date != Prana.BusinessObjects.DateTimeConstants.MinValue && holiday.Description != string.Empty)
                            {
                                holidaysList.Add(holiday);
                            }
                        }
                        //BindColumns();


                        //Error Strings..
                        string differentCalendarHolidays = string.Empty;
                        string duplicateHolidays = string.Empty;

                        if (holidaysList.Count > 0)
                        {
                            string calendarYear = txtYearValue.Text;

                            Holidays removedHolidays = new Holidays();

                            Holidays newHolidays = new Holidays();
                            foreach (Holiday hol in holidaysList)
                            {
                                newHolidays.Add(hol);
                            }
                            foreach (Holiday holiday in holidaysList)
                            {

                                if (!holiday.Date.Year.ToString().Equals(calendarYear))
                                {
                                    holiday.DateString = "Calendar year is different";
                                    removedHolidays.Add(holiday);
                                    continue;
                                }
                                if (newHolidays.Count > 1)
                                {
                                    // to compare holidays are from the respective year for which calendar is to be made.
                                    newHolidays.Remove(holiday);

                                    foreach (Holiday hol in newHolidays)
                                    {
                                        if (hol.Date.Equals(holiday.Date))
                                        {
                                            holiday.DateString = "Duplicate holiday";
                                            removedHolidays.Add(holiday);
                                        }
                                    }
                                }
                            }

                            foreach (Holiday holiday in removedHolidays)
                            {
                                holidaysList.Remove(holiday);
                                if (holiday.DateString.Equals("Duplicate holiday"))
                                {
                                    duplicateHolidays += holiday.Date.ToString() + "\n";
                                }
                                else
                                {
                                    differentCalendarHolidays += holiday.Date.ToString() + "\n";
                                }
                            }

                            string errorMsg = string.Empty;
                            if (duplicateHolidays != string.Empty)
                            {
                                errorMsg += ("Duplicate Holidays : \n" + duplicateHolidays + "\n");
                            }
                            if (differentCalendarHolidays != string.Empty)
                            {
                                errorMsg += ("Invalid  holidays for this calendar : \n" + differentCalendarHolidays);
                            }
                            if (errorMsg != string.Empty)
                            {
                                DialogResult res = MessageBox.Show("Do you still want to upload? \n" + errorMsg, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                                if (res == DialogResult.Yes && holidaysList.Count > 0)
                                {
                                    UploadCalendar(grdHoliday, holidaysList);
                                }
                            }
                            else
                            {
                                if (holidaysList.Count > 0)
                                {
                                    UploadCalendar(grdHoliday, holidaysList);
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }


        private void UploadCalendar(UltraGrid grdHoliday, Holidays holidaysList)
        {
            try
            {
                holidaysList.Sort();
                grdHoliday.DataSource = holidaysList;
                foreach (UltraGridColumn col in grdHoliday.DisplayLayout.Bands[0].Columns)
                {
                    col.Hidden = true;
                    if (col.Key.Equals("Date") || col.Key.Equals("Description"))
                    {
                        col.Hidden = false;
                    }
                }
                _flag = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private DataTable GetDataTableFromDifferentFileFormats(string fileName)
        {
            DataTable datasourceData = null;
            try
            {
                string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

                switch (fileFormat.ToUpperInvariant())
                {
                    case "CSV":
                        datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    case "XLS":
                        datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    default:
                        datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Default).GetDataTableFromUploadedDataFile(fileName);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return datasourceData;
        }

        private void ResetTreeAfterSave(string name, string year)
        {
            //Set the current node as selected node
            int yearcounter = 0;
            foreach (UltraTreeNode node in trvCalendar.Nodes)
            {
                foreach (UltraTreeNode subNode in trvCalendar.Nodes[yearcounter].Nodes)
                {
                    if ((subNode.Text == name) && (subNode.Parent.Key == year))
                    {
                        trvCalendar.ActiveNode = subNode;
                    }
                }
                yearcounter++;
            }
        }

        private void CalendarHolidays_FormClosed(object sender, FormClosedEventArgs e)
        {
            _cHolidays = null;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            UltraTreeNode nodeCalendar = (UltraTreeNode)trvCalendar.ActiveNode;
            if (nodeCalendar != null)
            {
                if (nodeCalendar.Parent != null)
                {
                    string calendarName = nodeCalendar.Text;
                    int year = int.Parse(nodeCalendar.Parent.Key);
                    int ID = int.Parse(nodeCalendar.Key);

                    string associatedAuecs = AuecAssociated(calendarName, year);
                    DialogResult result = MessageBox.Show("Do you really want to delete?\n" + associatedAuecs, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result.Equals(DialogResult.Yes))
                    {
                        try
                        {
                            DefaultSetUp(nodeCalendar.Parent);

                            AUECs auecs = AUECManager.auecAssociated(calendarName, year);
                            Calendar calendar = new Calendar(ID, calendarName, year);

                            _flag = false;
                            _textchanged = false;

                            //Update the collection & T_calendar,T_calendarHolidays & T_CalendarAUEC
                            _calcollection.Remove(calendar);
                            AUECManager.DeleteCalendar(calendarName, year, auecs);

                            if (trvCalendar.Nodes.Count > 0)
                            {
                                trvCalendar.ActiveNode = (UltraTreeNode)nodeCalendar.Parent;
                                trvCalendar.ActiveNode.Selected = true;
                            }
                            nodeCalendar.Remove();
                            trvCalendar.Update();
                            trvCalendar.Refresh();

                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                            if (rethrow)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
        }

        private void grdHoliday_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {

        }

        private void txtCalendar_TextChanged(object sender, EventArgs e)
        {
            _textchanged = !trvCalendar.ActiveNode.Text.Equals(txtCalendar.Text) ? true : false;
        }

        private void btnAddYear_Click(object sender, EventArgs e)
        {
            Prana.AdminForms.NewYear newYear = Prana.AdminForms.NewYear.GetInstance();
            newYear.ShowDialog(this);
            try
            {
                if (newYear.SaveYear != null)
                {
                    bool count = false;

                    //Returns Year selected from the NewYear Form DateTimePicker
                    string selectedYear = newYear.SaveYear.Year.ToString();

                    UltraTreeNode nodeYear = new UltraTreeNode(selectedYear, selectedYear);

                    if (trvCalendar.Nodes.Count != 0)
                    {
                        foreach (UltraTreeNode node in trvCalendar.Nodes)
                        {
                            if (node.Key.Equals(nodeYear.Key))
                            {
                                count = true;
                            }
                        }
                    }

                    if (!count)
                    {
                        toolStripStatusLabel2.Text = string.Empty;
                        trvCalendar.Nodes.Add(nodeYear);
                        trvCalendar.Update();
                        trvCalendar.Refresh();
                        trvCalendar.ActiveNode = nodeYear;
                        btnAddCalendar_Click(this.btnAddCalendar, e);
                    }
                    else
                    {
                        toolStripStatusLabel2.Text = "Year Already Exists";
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void trvCalendar_BeforeSelect(object sender, BeforeSelectEventArgs e)
        {
            if (_flag || _textchanged)
            {
                DialogResult result = MessageBox.Show("Do you want to save the unsaved data?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    _previous = true;
                    btnSave_Click(this.btnSave, e);
                }
                else
                {
                    _flag = false;
                    _textchanged = false;
                }
            }
        }

        private void grdHoliday_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (e.Cell.Column.Header.Caption.Equals("Date"))
            {
                foreach (UltraGridRow row in grdHoliday.Rows)
                {
                    if (!e.Cell.Row.Index.Equals(row.Index))
                    {
                        if (row.Cells["Date"].Value.ToString().Equals(e.Cell.Value.ToString()))
                        {
                            _flag = false;
                            MessageBox.Show("Date Already Exists");
                            e.Cell.Value = e.Cell.OriginalValue;
                        }
                    }
                }
                if ((DateTime.Parse(e.Cell.Value.ToString()).Year > int.Parse(txtYearValue.Text)) || (DateTime.Parse(e.Cell.Value.ToString()).Year < int.Parse(txtYearValue.Text)))
                {
                    _flag = false;
                    MessageBox.Show("Enter Year Specific Value");
                    e.Cell.Value = e.Cell.OriginalValue;
                }
                else
                {
                    _flag = true;
                }
            }

            if (e.Cell.Column.Header.Caption.Equals("Description"))
            {
                if (e.Cell.Value.ToString().Equals(string.Empty))
                {
                    _flag = false;
                    MessageBox.Show("Enter Description");
                    e.Cell.Value = e.Cell.OriginalValue;
                }
                else
                {
                    _flag = true;
                }
            }

            // Modified by bhavana on 31 March,2014 for additional columns MarketOff and SettlementOff
            if (e.Cell.Column.Header.Caption.ToLower().Equals("MarketOff".ToLower()) || e.Cell.Column.Header.Caption.ToLower().Equals("settlementoff".ToLower()))
            {
                _flag = true;
            }
        }

        private void lblDescription_Click(object sender, EventArgs e)
        {

        }

        private void CalendarHolidays_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                lock (locker)
                {
                    if (_flag || _textchanged)
                    {
                        DialogResult result = MessageBox.Show("Do you want to save the unsaved Data?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            btnSave_Click(this.btnSave, e);
                        }
                        if (result == DialogResult.No)
                        {
                            _flag = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void grdHoliday_Error(object sender, ErrorEventArgs e)
        {
            e.ErrorText = "Date and Description can't be left blank!!";
        }

        private void trvCalendar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (trvCalendar.ActiveNode != null)
                {
                    MenuItem mnuDeleteCalendar = new MenuItem("Delete");
                    mnuDeleteCalendar.Click += new EventHandler(mnuDeleteCalendar_Click);
                    ContextMenu cmnu = new ContextMenu(new MenuItem[] { mnuDeleteCalendar });
                    trvCalendar.ContextMenu = cmnu;
                }
            }
        }

        void mnuDeleteCalendar_Click(object sender, EventArgs e)
        {
            if (trvCalendar.ActiveNode != null)
            {
                this.btnDelete_Click(this.btnDelete, e);
            }
        }
    }
}