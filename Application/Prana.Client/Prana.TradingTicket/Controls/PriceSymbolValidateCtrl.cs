using Prana.ClientCommon;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;
namespace Prana.TradingTicket
{
    public partial class PriceSymbolValidate : UserControl
    {
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkValidate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkWarning;
        private Spinner txtRiskValue;
        private System.ComponentModel.IContainer components = null;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkLimitPrice;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chbxSetExecutedQtytoZero;

        private Prana.BusinessObjects.PriceSymbolValidation _riskAndValidate = new Prana.BusinessObjects.PriceSymbolValidation();
        public PriceSymbolValidate()
        {
            InitializeComponent();
        }

        #region Component Designer generated code
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.chkValidate = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.chkWarning = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtRiskValue = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.chkLimitPrice = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chbxSetExecutedQtytoZero = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.chkValidate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLimitPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbxSetExecutedQtytoZero)).BeginInit();
            this.SuspendLayout();
            // 
            // chkValidate
            // 
            appearance1.FontData.Name = "Tahoma";
            this.chkValidate.Appearance = appearance1;
            this.chkValidate.Location = new System.Drawing.Point(8, 113);
            this.chkValidate.Name = "chkValidate";
            this.chkValidate.Size = new System.Drawing.Size(110, 30);
            this.chkValidate.TabIndex = 8;
            this.chkValidate.Text = "Validate Symbol";
            this.chkValidate.Visible = false;
            // 
            // ultraLabel1
            // 
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(357, 13);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(126, 20);
            this.ultraLabel1.TabIndex = 7;
            this.ultraLabel1.Text = "% of Last Traded Price";
            // 
            // chkWarning
            // 
            appearance3.FontData.Name = "Tahoma";
            this.chkWarning.Appearance = appearance3;
            this.chkWarning.Location = new System.Drawing.Point(8, 8);
            this.chkWarning.Name = "chkWarning";
            this.chkWarning.Size = new System.Drawing.Size(231, 30);
            this.chkWarning.TabIndex = 5;
            this.chkWarning.Text = "Show Warning if entered price is +/- ";
            // 
            // txtRiskValue
            // 
            this.txtRiskValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtRiskValue.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.PositiveNumeric;
            this.txtRiskValue.DecimalPoints = 2147483647;
            this.txtRiskValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtRiskValue.Increment = 0.01D;
            this.txtRiskValue.Location = new System.Drawing.Point(279, 17);
            this.txtRiskValue.MaxValue = 100D;
            this.txtRiskValue.MinValue = 0D;
            this.txtRiskValue.Name = "txtRiskValue";
            this.txtRiskValue.Size = new System.Drawing.Size(72, 20);
            this.txtRiskValue.TabIndex = 9;
            this.txtRiskValue.Value = 1D;
            // 
            // chkLimitPrice
            // 
            appearance4.FontData.Name = "Tahoma";
            this.chkLimitPrice.Appearance = appearance4;
            this.chkLimitPrice.Location = new System.Drawing.Point(8, 43);
            this.chkLimitPrice.Name = "chkLimitPrice";
            this.chkLimitPrice.Size = new System.Drawing.Size(418, 30);
            this.chkLimitPrice.TabIndex = 10;
            this.chkLimitPrice.Text = "Show Warning if entered limit price is different from parent or sub ";
            // 
            // chbxSetExecutedQtytoZero
            // 
            this.chbxSetExecutedQtytoZero.Location = new System.Drawing.Point(8, 78);
            this.chbxSetExecutedQtytoZero.Name = "chbxSetExecutedQtytoZero";
            this.chbxSetExecutedQtytoZero.Size = new System.Drawing.Size(294, 30);
            this.chbxSetExecutedQtytoZero.TabIndex = 12;
            this.chbxSetExecutedQtytoZero.Text = "Set Manual Executed Quantity to Zero by Default";
            this.chbxSetExecutedQtytoZero.Enabled = false;
            // 
            // PriceSymbolValidate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chbxSetExecutedQtytoZero);
            this.Controls.Add(this.chkLimitPrice);
            this.Controls.Add(this.txtRiskValue);
            this.Controls.Add(this.chkValidate);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.chkWarning);
            this.Name = "PriceSymbolValidate";
            this.Size = new System.Drawing.Size(488, 146);
            ((System.ComponentModel.ISupportInitialize)(this.chkValidate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkLimitPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbxSetExecutedQtytoZero)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Disposing
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (chkValidate != null)
                {
                    chkValidate.Dispose();
                }
                if (ultraLabel1 != null)
                {
                    ultraLabel1.Dispose();
                }
                if (chkWarning != null)
                {
                    chkWarning.Dispose();
                }
                if (txtRiskValue != null)
                {
                    txtRiskValue.Dispose();
                }
                if (chkLimitPrice != null)
                {
                    chkLimitPrice.Dispose();
                }
                if (chbxSetExecutedQtytoZero != null)
                {
                    chbxSetExecutedQtytoZero.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        public void SetValues(Prana.BusinessObjects.PriceSymbolValidation riskCtrlValidateSettings)
        {
            if (riskCtrlValidateSettings.RiskCtrlCheck == true)
            {
                chkWarning.Checked = true;
                txtRiskValue.Value = riskCtrlValidateSettings.RiskValue;
            }

            if (riskCtrlValidateSettings.ValidateSymbolCheck == true)
            {
                chkValidate.Checked = true;
            }
            if (riskCtrlValidateSettings.LimitPriceCheck == true)
            {
                chkLimitPrice.Checked = true;
            }
            if (riskCtrlValidateSettings.SetExecutedQtytoZero == true)
            {
                chbxSetExecutedQtytoZero.Checked = true;
            }
        }

        public Prana.BusinessObjects.PriceSymbolValidation GetValues()
        {
            if (chkWarning.Checked == true)
            {
                _riskAndValidate.RiskCtrlCheck = true;
            }
            else
            {
                _riskAndValidate.RiskCtrlCheck = false;
            }

            if (txtRiskValue.Value != 0.0)
            {
                _riskAndValidate.RiskValue = txtRiskValue.Value;
            }
            if (chkValidate.Checked == true)
            {
                _riskAndValidate.ValidateSymbolCheck = true;
            }
            else
            {
                _riskAndValidate.ValidateSymbolCheck = false;
            }
            if (chkLimitPrice.Checked == true)
            {
                _riskAndValidate.LimitPriceCheck = true;
            }
            else
            {
                _riskAndValidate.LimitPriceCheck = false;
            }

            if (chbxSetExecutedQtytoZero.Checked)
            {
                _riskAndValidate.SetExecutedQtytoZero = true;
            }
            else
            {
                _riskAndValidate.SetExecutedQtytoZero = false;
            }
            return _riskAndValidate;
        }

        /// <summary>
        /// Saves the current selection in db and updates in trade manager
        /// </summary>
        /// <param name="priceSymbolValidation"></param>
        public void SavePriceSymbolValidation(Prana.BusinessObjects.PriceSymbolValidation priceSymbolValidation)
        {
            try
            {
                TradingTktPrefs.SaveRiskValidationPreferences(priceSymbolValidation);
                Prana.TradeManager.TradeManagerCore.UpdateSymbolSettings(priceSymbolValidation);
                Prana.TradeManager.TradeManager.GetNewPriceSymbolSettings(priceSymbolValidation);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
