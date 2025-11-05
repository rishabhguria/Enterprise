using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinGrid;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Utilities.MiscUtilities;

namespace Prana.UDATool
{
    public partial class UsrControlUDA : UserControl
    {
        public UsrControlUDA()
        {
            InitializeComponent();
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }
        private void btnScreenShot_Click(object sender, EventArgs e)
        {
            SnapShotManager.GetInstance().TakeSnapshot(this.ParentForm);            
        }

        public  void LoadData()
        {
            try
            {
                CentralDataManager.Clear();
                udaAssetClass.SetUp(UDATypes.AssetClass.ToString(), "P_UDAGetAllAssets", "P_DeleteUDAssets", "P_InsertUDAAsset", "P_GetInUseUDAAsset");
                udaSecurityType.SetUp(UDATypes.SecurityType.ToString(), "P_UDAGetAllSecurityType", "P_DeleteUDASecurityType", "P_InsertUDASecurityType", "P_GetInUseUDASecurityType");
                udaSector.SetUp(UDATypes.Sector.ToString(), "P_UDAGetAllSectors", "P_DeleteUDASector", "P_InsertUDASector", "P_GetInUseUDASector");
                udaSubSector.SetUp(UDATypes.SubSector.ToString(), "P_UDAGetAllSubSector", "P_DeleteUDASubSector", "P_InsertUDASubSector", "P_GetInUseUDASubSector");
                udaCountry.SetUp(UDATypes.Country.ToString(), "P_UDAGetAllCountry", "P_DeleteUDACountry", "P_InsertUDACountry", "P_GetInUseUDACountry");
                CentralDataManager.FillInUseUDAIDsList();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void SaveChanges()
        {
            try
            {
                bool changesDone = false;
                bool isTrue = false;
                foreach (UDAControl udactrl in CentralDataManager.UDACtrlCollection)
                {
                     changesDone = udactrl.SaveChanges();
                     if (changesDone)
                     {
                         isTrue = true;
                     }  
                }
                if (isTrue)
                {
                    MessageBox.Show("UDA Information Saved!");
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnExcelExport_Click(object sender, EventArgs e)
        {
          try
            {
                List<UltraGrid> lstUdaGrids = new List<UltraGrid>();

                lstUdaGrids.Add(udaAssetClass.Grid);
                lstUdaGrids.Add(udaSecurityType.Grid);
                lstUdaGrids.Add(udaSector.Grid);
                lstUdaGrids.Add(udaSubSector.Grid);
                lstUdaGrids.Add(udaCountry.Grid);

                ExcelAndPrintUtilities excelAndPrintUtilities = new ExcelAndPrintUtilities();
                excelAndPrintUtilities.ExportToExcel(lstUdaGrids,"UDA",true);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        internal void UndoAllChanges()
        {
            foreach (UDAControl udactrl in CentralDataManager.UDACtrlCollection)
            {
                udactrl.UndoChanges();
            }
        }
    }
}
