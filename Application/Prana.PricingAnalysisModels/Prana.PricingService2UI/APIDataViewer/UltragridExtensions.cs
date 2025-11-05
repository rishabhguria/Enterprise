using Infragistics.Win.UltraWinForm;
using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.PricingService2UI.APIDataViewer
{
    public static class UltragridExtensions
    {
        public static void PaintDynamicForm(this Form formTemplate)
        {
            try
            {
                formTemplate.ShowIcon = false;
                UltraFormManager formManager = Infragistics.Win.UltraWinForm.UltraFormManager.FromForm(formTemplate);
                if (formManager == null)
                {
                    formManager = new Infragistics.Win.UltraWinForm.UltraFormManager();
                    formManager.Form = formTemplate;
                    formTemplate.Disposed += delegate (object sen, EventArgs ev)
                    {
                        formManager.Dispose();
                    };
                    // Since FormManager uses form Handle to manipulates the form, if it is destroyed  we should remove and the manager
                    formTemplate.HandleDestroyed += delegate (object se, EventArgs ev)
                    {
                        formManager.Form = null;
                        formManager.Dispose();
                    };
                    //if (CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana") && CustomThemeHelper.ApplyTheme)
                    //{
                    //    CustomThemeHelper.SetThemeProperties(formTemplate, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CUSTOM_FILTER);
                    //    formManager.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    //    formManager.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, formTemplate.Text, CustomThemeHelper.UsedFont);
                    //    formTemplate.MaximizeBox = false;
                    //    formTemplate.MinimizeBox = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static void AddCustomColumnChooser(this Form form, UltraGrid ultraGrid1)
        {
            try
            {
                //TODO: Need to change this check
                if (ultraGrid1.DataSource != null)
                {
                    ColumnChooserDialog dlg = new ColumnChooserDialog();
                    dlg.Owner = form;
                    UltraGridColumnChooser cc = dlg.ColumnChooserControl;
                    cc.SourceGrid = ultraGrid1;
                    cc.CurrentBand = ultraGrid1.DisplayLayout.Bands[0];
                    cc.Style = ColumnChooserStyle.AllColumnsWithCheckBoxes;
                    cc.MultipleBandSupport = MultipleBandSupport.SingleBandOnly;
                    dlg.Size = new Size(290, 410);
                    dlg.ColumnChooserControl.DisplayLayout.Override.FilterUIType = FilterUIType.FilterRow;
                    dlg.Show();
                    (dlg as Form).PaintDynamicForm();
                    var columnChooserGrid = dlg.ColumnChooserControl.Controls["displayGrid"] as UltraGrid;
                    columnChooserGrid.InitializeLayout += columnChooserGrid_InitializeLayout;

                    for (int index = 0; index < cc.Controls.Count; index++)
                    {
                        var control = cc.Controls[index];
                        if (control is ComboBox)
                        {
                            cc.Controls.Remove(control);
                        }

                        if (control is UltraGrid)
                        {
                            var grid = (UltraGrid)control;
                            grid.InitializeRow += Grid_InitializeRow;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static void columnChooserGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridLayout layout = e.Layout;
                UltraGridBand band = layout.Bands[0];
                if (CheckKeyExistence(band, "Value"))
                {
                    band.Columns["Value"].FilterOperandStyle = FilterOperandStyle.Edit;
                    band.Columns["Value"].FilterOperatorDefaultValue = FilterOperatorDefaultValue.Contains;
                    ((UltraGrid)sender).InitializeLayout -= columnChooserGrid_InitializeLayout;
                }

            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static bool CheckKeyExistence(UltraGridBand band, string key)
        {
            foreach (UltraGridColumn col in band.Columns)
            {
                if (col.Key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        private static void Grid_InitializeRow(object sender, InitializeRowEventArgs e)
        {
        }
    }
}