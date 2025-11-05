using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.ShortLocate.Classes;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.TradingTicket.Forms
{
    public partial class ShortLocateListPopUp : Form
    {
        public ShortLocateListPopUp()
        {
            InitializeComponent();
            if (CustomThemeHelper.ApplyTheme)
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_DAILY_PM_CLIENTUI);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Short Locate List", CustomThemeHelper.UsedFont);
            }
            btnSubmit.ButtonStyle = UIElementButtonStyle.Button3D;
            btnSubmit.BackColor = Color.FromArgb(104, 156, 46);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.UseAppStyling = false;
            btnSubmit.UseOsThemes = DefaultableBoolean.False;

            btnCheckBorrow.ButtonStyle = UIElementButtonStyle.Button3D;
            btnCheckBorrow.BackColor = Color.FromArgb(104, 156, 46);
            btnCheckBorrow.ForeColor = Color.White;
            btnCheckBorrow.UseAppStyling = false;
            btnCheckBorrow.UseOsThemes = DefaultableBoolean.False;

        }

        private ShortLocateListParameter _selectedDataShortLocateParameter;

        public ShortLocateListParameter SelectedDataShortLocateParameter
        {
            set { _selectedDataShortLocateParameter = value; }
            get { return _selectedDataShortLocateParameter; }
        }

        public void Bind(string symbol, string companyMasterFund)
        {
            Dictionary<string, BindingList<ShortLocateListParameter>> symbolWiseShortLocateParam = ShortLocateDataManager.GetInstance.GetSymbolWiseShortLocateParameter(companyMasterFund);
            BindingList<ShortLocateListParameter> NewList = new BindingList<ShortLocateListParameter>();
            if (symbolWiseShortLocateParam.ContainsKey(symbol))
                NewList = new BindingList<ShortLocateListParameter>(symbolWiseShortLocateParam[symbol].Where(x => x.BorrowSharesAvailable > 0).ToList());
            shortLocateListGrid.BindData(NewList);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var row = shortLocateListGrid.GetSelectedRow();
            if (row == null)
                return;
            SelectedDataShortLocateParameter = row;
            this.FindForm().Close();

        }
    }
}
