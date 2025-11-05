using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.BusinessObjects;
using Prana.Utilities.UIUtilities;
using Prana.CommonDataCache;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.AllocationNew.Allocation.UI
{
    public partial class CheckSide : Form
    {
        public CheckSide()
        {
            InitializeComponent();
        }

        //private static CheckSide _singletonInstance = null;
       //private static object _locker = new object();

        //public static CheckSide GetInstance()
        //{
        //    if (_singletonInstance == null)
        //    {
        //        lock (_locker)
        //        {
        //            if (_singletonInstance == null)
        //            {
        //                _singletonInstance = new CheckSide();
        //            }
        //        }
        //    }
        //    return _singletonInstance;
        //}


        private List<TaxLot> _taxlots = null;

        public List<TaxLot> TaxlotsList
        {
            get { return _taxlots; }
            set 
            { 
                _taxlots = value;
                ctrlCheckSide1.BindGrid(_taxlots);
            }
        }

        private void CheckSide_Load(object sender, EventArgs e)
        {
            try
            {
                if (CachedDataManager.GetInstance.GetPranaReleaseViewType() == PranaReleaseViewType.CHMiddleWare)
                {
                    CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                }
                else
                {
                    CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_ALLOCATION_MAIN);
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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
    }
}