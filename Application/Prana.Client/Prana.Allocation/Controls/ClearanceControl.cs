using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Allocation.BLL;
namespace Prana.Allocation.Controls
{
    public partial class ClearanceControl : UserControl
    {
        private int _updatedrowscount;
        private int[] _clearanceTimeID;
        private DateTime[] _ClearanceTime;
        private int[] _companyUserAUECID;
        private int _userID;

        public ClearanceControl()
        {
            InitializeComponent();
        }


        private Infragistics.Win.UltraWinDataSource.UltraDataSource GetData(bool isBlotterClearance)
        {

            object[] parameter = new object[1];
            
            parameter[0] = this._userID;

            Database db = DatabaseFactory.CreateDatabase();

            string sPName = string.Empty;
            if (isBlotterClearance)
            {
                sPName = "P_GetBlotterClearanceForUser";
            }
            else
            {
                sPName = "P_GetAllocationClearanceForUser";
            }

            using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(sPName, parameter))
            {
                while (reader.Read())
                {


                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    DateTime[] time = new DateTime[3];
                    time[0] = (DateTime)row[1];
                    time[1] = (DateTime)row[2];
                    time[2] = (DateTime)row[3];

                    string[] timestring = new string[3];

                    timestring[0] = time[0].ToShortTimeString();
                    timestring[1] = time[1].ToShortTimeString();
                    timestring[2] = time[2].ToShortTimeString();

                    row[1] = timestring[0];
                    row[2] = timestring[1];
                    row[3] = timestring[2];

                    clearanceDataSource.Rows.Add(row);


                    //clearanceData.Add((row, 0));
                }

                return clearanceDataSource;

            }
        }


        public void LoadControl(int userId )
        {
            try
            {
                _userID = userId;


                this.clearanceDataSource.Band.Columns.Add("AUEC", typeof(string));
                this.clearanceDataSource.Band.Columns.Add("Start Time", typeof(string));
                this.clearanceDataSource.Band.Columns.Add("End Time", typeof(string));
                this.clearanceDataSource.Band.Columns.Add("Clearance Time", typeof(string));
                this.clearanceDataSource.Band.Columns.Add("Clearance Time ID", typeof(int));
                this.clearanceDataSource.Band.Columns.Add("CompanyUserAUECID", typeof(int));


                //clearanceDataSource.Band.Columns["StarTime"]
                clearanceTable.DataSource = null;
                clearanceTable.DataSource = GetData(false);


                SetGridStyles();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private void SetGridStyles()
        {
            clearanceTable.DisplayLayout.Bands[0].Columns["Clearance Time ID"].Hidden = true;
            clearanceTable.DisplayLayout.Bands[0].Columns["CompanyUserAUECID"].Hidden = true;
            //Set the cell activation to false.
            clearanceTable.DisplayLayout.Bands[0].Columns["AUEC"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            clearanceTable.DisplayLayout.Bands[0].Columns["Start Time"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            clearanceTable.DisplayLayout.Bands[0].Columns["End Time"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            //Set the column styles.
            clearanceTable.DisplayLayout.Bands[0].Columns["Start Time"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
            clearanceTable.DisplayLayout.Bands[0].Columns["End Time"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
            clearanceTable.DisplayLayout.Bands[0].Columns["Clearance Time"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.TimeWithSpin;

            _clearanceTimeID = new int[clearanceDataSource.Rows.Count];
            _ClearanceTime = new DateTime[clearanceDataSource.Rows.Count];
            _companyUserAUECID = new int[clearanceDataSource.Rows.Count];
            _updatedrowscount = 0;
        }

        public void SaveData()
        {
            Database db = DatabaseFactory.CreateDatabase();
           
                if (blotterClearanceChkBx.Checked)
                {
                   
                    foreach (Infragistics.Win.UltraWinDataSource.UltraDataRow row in clearanceDataSource.Rows)
                    {
                        int isSuccess = 0;
                        object[] parameters = new object[3];
                        parameters[0] = Convert.ToInt32(row.GetCellValue("Clearance Time ID"));
                        parameters[1] = Convert.ToDateTime(row.GetCellValue("Clearance Time"));
                        parameters[2] = Convert.ToInt32(row.GetCellValue("CompanyUserAUECID"));

                        isSuccess = db.ExecuteNonQuery("P_UpdateAllocationClearanceTime", parameters);
                    }
                }
                else if (this._updatedrowscount > 0)
                {
                    //allocationClearanceChkBx.Checked = false;

                    for (int counter = 0; counter < this._updatedrowscount; counter++)
                    {
                        int isSuccess = 0;
                        object[] parameters = new object[3];
                        parameters[0] = this._clearanceTimeID[counter];
                        parameters[1] = this._ClearanceTime[counter];
                        parameters[2] = this._companyUserAUECID[counter];

                        isSuccess = db.ExecuteNonQuery("P_UpdateAllocationClearanceTime", parameters);
                    }
                }
            
        }

       

        private void clearanceTable_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            Infragistics.Win.UltraWinGrid.UltraGridRow presentRow = e.Cell.Row;

            DateTime clearanceTime = Convert.ToDateTime(e.Cell.Value);
            DateTime startTime = Convert.ToDateTime(presentRow.Cells["Start Time"].Value);
            DateTime endTime = Convert.ToDateTime(presentRow.Cells["End Time"].Value);

            if (clearanceTime >= startTime && clearanceTime <= endTime)
            {
                MessageBox.Show("Clearance Time cannot be between Trading Start Time and Trading End Time. Change to make it after End Time or before Start Time.");
                //e.Cell.Value = e.Cell.OriginalValue;
            }
            else
            {
                if (e.Cell.Value != e.Cell.OriginalValue)
                {
                    this._updatedrowscount = this._updatedrowscount + 1;
                    this._clearanceTimeID[this._updatedrowscount - 1] = Convert.ToInt32(presentRow.Cells["Clearance Time ID"].Value);
                    this._ClearanceTime[this._updatedrowscount - 1] = clearanceTime;
                    this._companyUserAUECID[this._updatedrowscount - 1] = Convert.ToInt32(presentRow.Cells["CompanyUserAUECID"].Value);
                }
            }


        }

        private void blotterClearanceChkBx_CheckedChanged(object sender, EventArgs e)
        {
            clearanceDataSource.Rows.Clear();

            if (blotterClearanceChkBx.Checked)
            {
                clearanceTable.DataSource = null;
                clearanceTable.DataSource = GetData(true);

                this._updatedrowscount = 0;
                this._clearanceTimeID = new int[clearanceTable.Rows.Count];
                this._ClearanceTime = new DateTime[clearanceTable.Rows.Count];
                this._companyUserAUECID = new int[clearanceTable.Rows.Count];
                foreach (Infragistics.Win.UltraWinDataSource.UltraDataRow row in clearanceDataSource.Rows)
                {
                    this._updatedrowscount = this._updatedrowscount + 1;
                    if (row.GetCellValue("Clearance Time ID") == null)
                    {
                        this._clearanceTimeID[this._updatedrowscount - 1] = 0;
                    }
                    else
                    {
                        this._clearanceTimeID[this._updatedrowscount - 1] = Convert.ToInt32(row.GetCellValue("Clearance Time ID"));
                    }
                    this._ClearanceTime[this._updatedrowscount - 1] = Convert.ToDateTime(row.GetCellValue("Clearance Time"));
                    this._companyUserAUECID[this._updatedrowscount - 1] = Convert.ToInt32(row.GetCellValue("CompanyUserAUECID"));
                }
            }
            else
            {
                clearanceTable.DataSource = null;
                clearanceTable.DataSource = GetData(false);
            }
            SetGridStyles();

        }

        
    }
}
