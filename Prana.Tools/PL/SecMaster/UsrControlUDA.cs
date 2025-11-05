using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class UserControlUDA : UserControl
    {
        public delegate void DelegateSaveUDAData(Dictionary<String, Dictionary<String, object>> udaData);
        internal event DelegateSaveUDAData EventHandlerSaveUDAData;

        private Dictionary<string, Dictionary<int, string>> _dictUDAAttributes = null;

        public Dictionary<string, Dictionary<int, string>> DictUDAAttributes
        {
            get { return _dictUDAAttributes; }
            set { _dictUDAAttributes = value; }
        }


        //private Dictionary<string, List<int>> _dictInUsedUDAs = null;
        //public Dictionary<string, List<int>> DictInUsedUDAs
        //{
        //    get { return _dictInUsedUDAs; }
        //    set { _dictInUsedUDAs = value; }
        //}



        public UserControlUDA()
        {
            try
            {
                InitializeComponent();
                SetupSnapshotControl();
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

        private void SetupSnapshotControl()
        {
            try
            {
                this.btnScreenShot = SnapShotManager.GetInstance().ultraButton;
                this.btnScreenShot.Anchor = System.Windows.Forms.AnchorStyles.Top;
                this.btnScreenShot.Location = new System.Drawing.Point(450, 3);
                this.btnScreenShot.Name = "btnScreenShot";
                this.btnScreenShot.Size = new System.Drawing.Size(65, 23);
                this.btnScreenShot.Click += new System.EventHandler(this.btnScreenShot_Click);
                this.Controls.Add(this.btnScreenShot);

                btnScreenShot.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnScreenShot.ForeColor = System.Drawing.Color.White;
                btnScreenShot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnScreenShot.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnScreenShot.UseAppStyling = false;
                btnScreenShot.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
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
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveChanges();
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
        private void btnScreenShot_Click(object sender, EventArgs e)
        {
            try
            {
                SnapShotManager.GetInstance().TakeSnapshot(this.ParentForm);
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

        public void LoadData()
        {
            try
            {
                CentralDataManager.Clear();
                if (_dictUDAAttributes != null)
                {
                    if (_dictUDAAttributes.ContainsKey(SecMasterConstants.CONST_UDAAsset))
                    {
                        UDACollection udaCol = CentralDataManager.GetCollection(_dictUDAAttributes[SecMasterConstants.CONST_UDAAsset]);
                        udaAssetClass.SetUp(Prana.BusinessObjects.SecMasterConstants.UDATypes.AssetClass.ToString(), udaCol, SecMasterConstants.CONST_DELETE_UDAASSET_SP, SecMasterConstants.CONST_INSERT_UDAASSET_SP);
                    }

                    if (_dictUDAAttributes.ContainsKey(SecMasterConstants.CONST_UDACountry))
                    {
                        UDACollection udaCol = CentralDataManager.GetCollection(_dictUDAAttributes[SecMasterConstants.CONST_UDACountry]);
                        udaCountry.SetUp(Prana.BusinessObjects.SecMasterConstants.UDATypes.Country.ToString(), udaCol, SecMasterConstants.CONST_DELETE_UDACountries_SP, SecMasterConstants.CONST_INSERT_UDACountries_SP);
                    }
                    if (_dictUDAAttributes.ContainsKey(SecMasterConstants.CONST_UDASector))
                    {
                        UDACollection udaCol = CentralDataManager.GetCollection(_dictUDAAttributes[SecMasterConstants.CONST_UDASector]);
                        udaSector.SetUp(Prana.BusinessObjects.SecMasterConstants.UDATypes.Sector.ToString(), udaCol, SecMasterConstants.CONST_DELETE_UDASectors_SP, SecMasterConstants.CONST_INSERT_UDASectors_SP);
                    }
                    if (_dictUDAAttributes.ContainsKey(SecMasterConstants.CONST_UDASubSector))
                    {
                        UDACollection udaCol = CentralDataManager.GetCollection(_dictUDAAttributes[SecMasterConstants.CONST_UDASubSector]);
                        udaSubSector.SetUp(Prana.BusinessObjects.SecMasterConstants.UDATypes.SubSector.ToString(), udaCol, SecMasterConstants.CONST_DELETE_UDASubSectors_SP, SecMasterConstants.CONST_INSERT_UDASubSectors_SP);
                    }
                    if (_dictUDAAttributes.ContainsKey(SecMasterConstants.CONST_UDASecurityType))
                    {
                        UDACollection udaCol = CentralDataManager.GetCollection(_dictUDAAttributes[SecMasterConstants.CONST_UDASecurityType]);
                        udaSecurityType.SetUp(Prana.BusinessObjects.SecMasterConstants.UDATypes.SecurityType.ToString(), udaCol, SecMasterConstants.CONST_DELETE_UDASecurityTypes_SP, SecMasterConstants.CONST_INSERT_UDASecurityTypes_SP);
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
        /// <summary>
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void SaveChanges()
        {
            try
            {

                //bool changesDone = false;
                bool isTrue = false;
                Dictionary<String, Dictionary<String, object>> totalDataToSave = new Dictionary<string, Dictionary<string, object>>();
                foreach (UDAControl udactrl in CentralDataManager.UDACtrlCollection)
                {
                    isTrue = udactrl.UpdateCollection();
                    if (isTrue)
                    {

                        Dictionary<String, object> dataToSave = new Dictionary<string, object>();

                        if (udactrl.DeletedIDs.Count > 0)
                        {
                            if (!dataToSave.ContainsKey(udactrl.SP_DeleteName))
                                dataToSave.Add(udactrl.SP_DeleteName, udactrl.DeletedIDs);
                        }
                        if (udactrl.AddedUDACollection.Count > 0)
                        {
                            if (!dataToSave.ContainsKey(udactrl.SP_InsertName))
                                dataToSave.Add(udactrl.SP_InsertName, udactrl.AddedUDACollection);
                        }

                        if (dataToSave.Count > 0)
                            totalDataToSave.Add(udactrl.UDAType, dataToSave);


                    }
                }
                if (EventHandlerSaveUDAData != null && totalDataToSave.Count > 0)
                {
                    EventHandlerSaveUDAData(totalDataToSave);
                    //https://jira.nirvanasolutions.com:8443/browse/PRANA-24410
                    foreach (UDAControl udactrl in CentralDataManager.UDACtrlCollection)
                    {
                        udactrl.DeletedIDs.Clear();
                        udactrl.AddedUDACollection.Clear();
                    }
                    // MessageBox.Show("UDA Information Saved!", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    MessageBox.Show("No information to save!", "Nirvana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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
                excelAndPrintUtilities.ExportToExcel(lstUdaGrids, "UDA", true);
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



        internal void UndoAllChanges()
        {
            try
            {
                foreach (UDAControl udactrl in CentralDataManager.UDACtrlCollection)
                {
                    udactrl.UndoChanges();
                }
            }
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
        }

        /// <summary>
        /// Load InUsed UDAs Data 
        /// </summary>
        /// <param name="inUsedUDAsDict"></param>
        internal void LoadDataInUsedUDAs(Dictionary<string, Dictionary<int, string>> inUsedUDAsDict)
        {
            try
            {
                if (inUsedUDAsDict != null)
                {
                    if (inUsedUDAsDict.ContainsKey(SecMasterConstants.CONST_UDAAsset))
                    {
                        udaAssetClass.UDAsInUse = new List<int>();
                        foreach (int id in inUsedUDAsDict[SecMasterConstants.CONST_UDAAsset].Keys)
                        {
                            udaAssetClass.UDAsInUse.Add(id);
                        }
                    }
                    if (inUsedUDAsDict.ContainsKey(SecMasterConstants.CONST_UDACountry))
                    {
                        udaCountry.UDAsInUse = new List<int>();
                        foreach (int id in inUsedUDAsDict[SecMasterConstants.CONST_UDACountry].Keys)
                        {
                            udaCountry.UDAsInUse.Add(id);
                        }
                    }
                    if (inUsedUDAsDict.ContainsKey(SecMasterConstants.CONST_UDASector))
                    {
                        udaSector.UDAsInUse = new List<int>();
                        foreach (int id in inUsedUDAsDict[SecMasterConstants.CONST_UDASector].Keys)
                        {
                            udaSector.UDAsInUse.Add(id);
                        }
                    }
                    if (inUsedUDAsDict.ContainsKey(SecMasterConstants.CONST_UDASubSector))
                    {
                        udaSubSector.UDAsInUse = new List<int>();
                        foreach (int id in inUsedUDAsDict[SecMasterConstants.CONST_UDASubSector].Keys)
                        {
                            udaSubSector.UDAsInUse.Add(id);
                        }
                    }
                    if (inUsedUDAsDict.ContainsKey(SecMasterConstants.CONST_UDASecurityType))
                    {
                        udaSecurityType.UDAsInUse = new List<int>();
                        foreach (int id in inUsedUDAsDict[SecMasterConstants.CONST_UDASecurityType].Keys)
                        {
                            udaSecurityType.UDAsInUse.Add(id);
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

        private void UsrControlUDA_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExcelExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExcelExport.ForeColor = System.Drawing.Color.White;
                btnExcelExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExcelExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExcelExport.UseAppStyling = false;
                btnExcelExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnScreenShot.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnScreenShot.ForeColor = System.Drawing.Color.White;
                btnScreenShot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnScreenShot.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnScreenShot.UseAppStyling = false;
                btnScreenShot.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
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
        }
    }
}
