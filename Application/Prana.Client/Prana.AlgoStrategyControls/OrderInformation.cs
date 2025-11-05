using Prana.Utilities.UI.UIUtilities;
using System.Data;
using System.Windows.Forms;

namespace Prana.AlgoStrategyControls
{
    public partial class OrderInformation : Form
    {
        public OrderInformation()
        {
            InitializeComponent();

        }

        public void ShowData(DataTable datatable)
        {
            CustomThemeHelper.SetThemeProperties(FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);

            if (CustomThemeHelper.ApplyTheme)
            {
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
            }

            // InitializeComponent();
            grdAlgo.DataSource = null;
            grdAlgo.DataSource = datatable;
            grdAlgo.DataBind();

        }
    }
}