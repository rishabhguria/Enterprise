using System;
using Prana.PM.BLL;
using Prana.LogManager;

namespace Prana.PM.Client.UI.Controls
{
    partial class CtrlMainConsolidationViewSummary
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }

                _isInitialized = false;


                //if (this._consolidatedInfoManagerInstance != null)
                //{
                //    this._consolidatedInfoManagerInstance.ConsolidatedInfoListChanged -= new ConsolidatedInfoManager.MethodHandler(consolidatedInfoManagerInstance_ConsolidatedInfoListChanged);
                //    this._consolidatedInfoManagerInstance.ConsolidatedInfoSummaryChanged -= new ConsolidatedInfoManager.MethodHandler(consolidatedInfoManagerInstance_ConsolidatedInfoSummaryChanged);
                //}

                //this._consolidatedInfoManagerInstance = null;


                base.Dispose(disposing);
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblDayPNLLongTotal = new Infragistics.Win.Misc.UltraLabel();
            this.label2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblDayPNLShortTotal = new Infragistics.Win.Misc.UltraLabel();
            this.label3 = new Infragistics.Win.Misc.UltraLabel();
            this.lblDayPNLTotal = new Infragistics.Win.Misc.UltraLabel();
            this.label4 = new Infragistics.Win.Misc.UltraLabel();
            this.lblLongExposureTotal = new Infragistics.Win.Misc.UltraLabel();
            this.label5 = new Infragistics.Win.Misc.UltraLabel();
            this.lblShortExposureTotal = new Infragistics.Win.Misc.UltraLabel();
            this.label11 = new Infragistics.Win.Misc.UltraLabel();
            this.lblNetExposureTotal = new Infragistics.Win.Misc.UltraLabel();
            this.label13 = new Infragistics.Win.Misc.UltraLabel();
            this.label15 = new Infragistics.Win.Misc.UltraLabel();
            this.lblLongNotionalValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblNetAssetValue = new Infragistics.Win.Misc.UltraLabel();
            this.label17 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCashProjected = new Infragistics.Win.Misc.UltraLabel();
            this.lblCashValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblPNLContribution = new Infragistics.Win.Misc.UltraLabel();
            this.label19 = new Infragistics.Win.Misc.UltraLabel();
            this.lblCostBasisRealizedPnL = new Infragistics.Win.Misc.UltraLabel();
            this.label22 = new Infragistics.Win.Misc.UltraLabel();
            this.label20 = new Infragistics.Win.Misc.UltraLabel();
            this.label21 = new Infragistics.Win.Misc.UltraLabel();
            this.lblMTDUnrealizedPnL = new Infragistics.Win.Misc.UltraLabel();
            this.lblMTDRealizedPNL = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalMTDPnL = new Infragistics.Win.Misc.UltraLabel();
            this.label24 = new Infragistics.Win.Misc.UltraLabel();
            this.lblNetNotionalValue = new Infragistics.Win.Misc.UltraLabel();
            this.label25 = new Infragistics.Win.Misc.UltraLabel();
            this.tlpConsolidationDashboard = new System.Windows.Forms.TableLayoutPanel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.lblPercentLongExposure = new Infragistics.Win.Misc.UltraLabel();
            this.lblPercentShortExposure = new Infragistics.Win.Misc.UltraLabel();
            this.lblPercentNetExposure = new Infragistics.Win.Misc.UltraLabel();
            this.lblCostBasisPnl = new Infragistics.Win.Misc.UltraLabel();
            this.lblCostBasisPNLValue = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.lblNetMktValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblShortMktValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblLongMktValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblShortNotionalValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblPercentNetMktValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblPercentNetExposureGross = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.lblDayReturnGrossMarketValue = new Infragistics.Win.Misc.UltraLabel();
            this.tlpConsolidationDashboard.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(260, 77);
            this.label1.Margin = new System.Windows.Forms.Padding(2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 15);
            this.label1.TabIndex = 56;
            this.label1.Text = "Day P&&L Long";
            this.label1.Visible = false;
            this.label1.WrapText = false;
            // 
            // lblDayPNLLongTotal
            // 
            this.lblDayPNLLongTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblDayPNLLongTotal.ForeColor = System.Drawing.Color.White;
            this.lblDayPNLLongTotal.Location = new System.Drawing.Point(369, 77);
            this.lblDayPNLLongTotal.Margin = new System.Windows.Forms.Padding(2);
            this.lblDayPNLLongTotal.Name = "lblDayPNLLongTotal";
            this.lblDayPNLLongTotal.Size = new System.Drawing.Size(31, 10);
            this.lblDayPNLLongTotal.TabIndex = 56;
            this.lblDayPNLLongTotal.Visible = false;
            this.lblDayPNLLongTotal.WrapText = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(2, 77);
            this.label2.Margin = new System.Windows.Forms.Padding(2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 15);
            this.label2.TabIndex = 56;
            this.label2.Text = "Day P&&L Short";
            this.label2.Visible = false;
            this.label2.WrapText = false;
            // 
            // lblDayPNLShortTotal
            // 
            this.lblDayPNLShortTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblDayPNLShortTotal.ForeColor = System.Drawing.Color.White;
            this.lblDayPNLShortTotal.Location = new System.Drawing.Point(98, 77);
            this.lblDayPNLShortTotal.Margin = new System.Windows.Forms.Padding(2);
            this.lblDayPNLShortTotal.Name = "lblDayPNLShortTotal";
            this.lblDayPNLShortTotal.Size = new System.Drawing.Size(25, 15);
            this.lblDayPNLShortTotal.TabIndex = 56;
            this.lblDayPNLShortTotal.Visible = false;
            this.lblDayPNLShortTotal.WrapText = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(2, 2);
            this.label3.Margin = new System.Windows.Forms.Padding(2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 15);
            this.label3.TabIndex = 56;
            this.label3.Text = "Day P&&L";
            this.label3.WrapText = false;
            // 
            // lblDayPNLTotal
            // 
            this.lblDayPNLTotal.AutoSize = true;
            this.lblDayPNLTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDayPNLTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblDayPNLTotal.ForeColor = System.Drawing.Color.White;
            this.lblDayPNLTotal.Location = new System.Drawing.Point(98, 2);
            this.lblDayPNLTotal.Margin = new System.Windows.Forms.Padding(2);
            this.lblDayPNLTotal.Name = "lblDayPNLTotal";
            this.lblDayPNLTotal.Size = new System.Drawing.Size(0, 0);
            this.lblDayPNLTotal.TabIndex = 56;
            this.lblDayPNLTotal.WrapText = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(127, 2);
            this.label4.Margin = new System.Windows.Forms.Padding(2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 15);
            this.label4.TabIndex = 56;
            this.label4.Text = "   Long Exposure";
            this.label4.WrapText = false;
            // 
            // lblLongExposureTotal
            // 
            this.lblLongExposureTotal.AutoSize = true;
            this.lblLongExposureTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLongExposureTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblLongExposureTotal.ForeColor = System.Drawing.Color.White;
            this.lblLongExposureTotal.Location = new System.Drawing.Point(223, 2);
            this.lblLongExposureTotal.Margin = new System.Windows.Forms.Padding(2);
            this.lblLongExposureTotal.Name = "lblLongExposureTotal";
            this.lblLongExposureTotal.Size = new System.Drawing.Size(0, 0);
            this.lblLongExposureTotal.TabIndex = 56;
            this.lblLongExposureTotal.WrapText = false;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(127, 27);
            this.label5.Margin = new System.Windows.Forms.Padding(2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 15);
            this.label5.TabIndex = 56;
            this.label5.Text = "   Short Exposure";
            this.label5.WrapText = false;
            // 
            // lblShortExposureTotal
            // 
            this.lblShortExposureTotal.AutoSize = true;
            this.lblShortExposureTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShortExposureTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblShortExposureTotal.ForeColor = System.Drawing.Color.White;
            this.lblShortExposureTotal.Location = new System.Drawing.Point(223, 27);
            this.lblShortExposureTotal.Margin = new System.Windows.Forms.Padding(2);
            this.lblShortExposureTotal.Name = "lblShortExposureTotal";
            this.lblShortExposureTotal.Size = new System.Drawing.Size(0, 0);
            this.lblShortExposureTotal.TabIndex = 56;
            this.lblShortExposureTotal.WrapText = false;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(127, 52);
            this.label11.Margin = new System.Windows.Forms.Padding(2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 15);
            this.label11.TabIndex = 56;
            this.label11.Text = "   Net Exposure";
            this.label11.WrapText = false;
            // 
            // lblNetExposureTotal
            // 
            this.lblNetExposureTotal.AutoSize = true;
            this.lblNetExposureTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetExposureTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblNetExposureTotal.ForeColor = System.Drawing.Color.White;
            this.lblNetExposureTotal.Location = new System.Drawing.Point(223, 52);
            this.lblNetExposureTotal.Margin = new System.Windows.Forms.Padding(2);
            this.lblNetExposureTotal.Name = "lblNetExposureTotal";
            this.lblNetExposureTotal.Size = new System.Drawing.Size(0, 0);
            this.lblNetExposureTotal.TabIndex = 56;
            this.lblNetExposureTotal.WrapText = false;
            // 
            // label13
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.label13.Appearance = appearance1;
            this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(644, 2);
            this.label13.Margin = new System.Windows.Forms.Padding(2);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(107, 15);
            this.label13.TabIndex = 56;
            this.label13.Text = "Long Notional Value";
            this.label13.Visible = false;
            this.label13.WrapText = false;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(644, 27);
            this.label15.Margin = new System.Windows.Forms.Padding(2);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 15);
            this.label15.TabIndex = 56;
            this.label15.Text = "Short Notional Value";
            this.label15.Visible = false;
            this.label15.WrapText = false;
            // 
            // lblLongNotionalValue
            // 
            this.lblLongNotionalValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblLongNotionalValue.ForeColor = System.Drawing.Color.White;
            this.lblLongNotionalValue.Location = new System.Drawing.Point(757, 2);
            this.lblLongNotionalValue.Margin = new System.Windows.Forms.Padding(2);
            this.lblLongNotionalValue.Name = "lblLongNotionalValue";
            this.lblLongNotionalValue.Size = new System.Drawing.Size(44, 19);
            this.lblLongNotionalValue.TabIndex = 56;
            this.lblLongNotionalValue.Visible = false;
            this.lblLongNotionalValue.WrapText = false;
            // 
            // lblNetAssetValue
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.lblNetAssetValue.Appearance = appearance2;
            this.lblNetAssetValue.AutoSize = true;
            this.lblNetAssetValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetAssetValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblNetAssetValue.ForeColor = System.Drawing.Color.White;
            this.lblNetAssetValue.Location = new System.Drawing.Point(619, 27);
            this.lblNetAssetValue.Margin = new System.Windows.Forms.Padding(2);
            this.lblNetAssetValue.Name = "lblNetAssetValue";
            this.lblNetAssetValue.Size = new System.Drawing.Size(0, 0);
            this.lblNetAssetValue.TabIndex = 61;
            this.lblNetAssetValue.WrapText = false;
            // 
            // label17
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.label17.Appearance = appearance3;
            this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(532, 27);
            this.label17.Margin = new System.Windows.Forms.Padding(2);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 15);
            this.label17.TabIndex = 60;
            this.label17.Text = "   Shadow NAV";
            this.label17.WrapText = false;
            // 
            // lblCashProjected
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            this.lblCashProjected.Appearance = appearance4;
            this.lblCashProjected.AutoSize = true;
            this.lblCashProjected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCashProjected.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblCashProjected.ForeColor = System.Drawing.Color.White;
            this.lblCashProjected.Location = new System.Drawing.Point(619, 2);
            this.lblCashProjected.Margin = new System.Windows.Forms.Padding(2);
            this.lblCashProjected.Name = "lblCashProjected";
            this.lblCashProjected.Size = new System.Drawing.Size(0, 0);
            this.lblCashProjected.TabIndex = 63;
            this.lblCashProjected.WrapText = false;
            // 
            // lblCashValue
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.lblCashValue.Appearance = appearance5;
            this.lblCashValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCashValue.ForeColor = System.Drawing.Color.White;
            this.lblCashValue.Location = new System.Drawing.Point(532, 2);
            this.lblCashValue.Margin = new System.Windows.Forms.Padding(2);
            this.lblCashValue.Name = "lblCashValue";
            this.lblCashValue.Size = new System.Drawing.Size(83, 15);
            this.lblCashValue.TabIndex = 62;
            this.lblCashValue.Text = "   Cash Value";
            this.lblCashValue.WrapText = false;
            // 
            // lblPNLContribution
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            this.lblPNLContribution.Appearance = appearance6;
            this.lblPNLContribution.AutoSize = true;
            this.lblPNLContribution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPNLContribution.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblPNLContribution.ForeColor = System.Drawing.Color.White;
            this.lblPNLContribution.Location = new System.Drawing.Point(98, 27);
            this.lblPNLContribution.Margin = new System.Windows.Forms.Padding(2);
            this.lblPNLContribution.Name = "lblPNLContribution";
            this.lblPNLContribution.Size = new System.Drawing.Size(0, 0);
            this.lblPNLContribution.TabIndex = 65;
            this.lblPNLContribution.WrapText = false;
            // 
            // label19
            // 
            appearance8.ForeColor = System.Drawing.Color.White;
            this.label19.Appearance = appearance8;
            this.label19.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(2, 27);
            this.label19.Margin = new System.Windows.Forms.Padding(2);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(92, 15);
            this.label19.TabIndex = 64;
            this.label19.Text = "P&&L Contribution (BP)";
            this.label19.WrapText = false;
            // 
            // lblCostBasisRealizedPnL
            // 
            this.lblCostBasisRealizedPnL.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblCostBasisRealizedPnL.ForeColor = System.Drawing.Color.White;
            this.lblCostBasisRealizedPnL.Location = new System.Drawing.Point(619, 77);
            this.lblCostBasisRealizedPnL.Margin = new System.Windows.Forms.Padding(2);
            this.lblCostBasisRealizedPnL.Name = "lblCostBasisRealizedPnL";
            this.lblCostBasisRealizedPnL.Size = new System.Drawing.Size(21, 15);
            this.lblCostBasisRealizedPnL.TabIndex = 69;
            this.lblCostBasisRealizedPnL.Visible = false;
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(644, 77);
            this.label22.Margin = new System.Windows.Forms.Padding(2);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(77, 15);
            this.label22.TabIndex = 70;
            this.label22.Text = "MTD Realized P&&L";
            this.label22.Visible = false;
            this.label22.WrapText = false;
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(532, 77);
            this.label20.Margin = new System.Windows.Forms.Padding(2);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(44, 15);
            this.label20.TabIndex = 71;
            this.label20.Text = "CB Realized P&&L";
            this.label20.Visible = false;
            this.label20.WrapText = false;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(127, 77);
            this.label21.Margin = new System.Windows.Forms.Padding(2);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(80, 15);
            this.label21.TabIndex = 66;
            this.label21.Text = "MTD Unrealized P&&L";
            this.label21.Visible = false;
            this.label21.WrapText = false;
            // 
            // lblMTDUnrealizedPnL
            // 
            this.lblMTDUnrealizedPnL.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblMTDUnrealizedPnL.ForeColor = System.Drawing.Color.White;
            this.lblMTDUnrealizedPnL.Location = new System.Drawing.Point(223, 77);
            this.lblMTDUnrealizedPnL.Margin = new System.Windows.Forms.Padding(2);
            this.lblMTDUnrealizedPnL.Name = "lblMTDUnrealizedPnL";
            this.lblMTDUnrealizedPnL.Size = new System.Drawing.Size(33, 15);
            this.lblMTDUnrealizedPnL.TabIndex = 67;
            this.lblMTDUnrealizedPnL.Visible = false;
            // 
            // lblMTDRealizedPNL
            // 
            this.lblMTDRealizedPNL.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblMTDRealizedPNL.ForeColor = System.Drawing.Color.White;
            this.lblMTDRealizedPNL.Location = new System.Drawing.Point(757, 77);
            this.lblMTDRealizedPNL.Margin = new System.Windows.Forms.Padding(2);
            this.lblMTDRealizedPNL.Name = "lblMTDRealizedPNL";
            this.lblMTDRealizedPNL.Size = new System.Drawing.Size(35, 15);
            this.lblMTDRealizedPNL.TabIndex = 68;
            this.lblMTDRealizedPNL.Visible = false;
            // 
            // lblTotalMTDPnL
            // 
            this.lblTotalMTDPnL.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTotalMTDPnL.ForeColor = System.Drawing.Color.White;
            this.lblTotalMTDPnL.Location = new System.Drawing.Point(502, 77);
            this.lblTotalMTDPnL.Margin = new System.Windows.Forms.Padding(2);
            this.lblTotalMTDPnL.Name = "lblTotalMTDPnL";
            this.lblTotalMTDPnL.Size = new System.Drawing.Size(26, 15);
            this.lblTotalMTDPnL.TabIndex = 72;
            this.lblTotalMTDPnL.Visible = false;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(404, 77);
            this.label24.Margin = new System.Windows.Forms.Padding(2);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(88, 15);
            this.label24.TabIndex = 75;
            this.label24.Text = "Total MTD P&&L";
            this.label24.Visible = false;
            this.label24.WrapText = false;
            // 
            // lblNetNotionalValue
            // 
            this.lblNetNotionalValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblNetNotionalValue.ForeColor = System.Drawing.Color.White;
            this.lblNetNotionalValue.Location = new System.Drawing.Point(757, 52);
            this.lblNetNotionalValue.Margin = new System.Windows.Forms.Padding(2);
            this.lblNetNotionalValue.Name = "lblNetNotionalValue";
            this.lblNetNotionalValue.Size = new System.Drawing.Size(44, 19);
            this.lblNetNotionalValue.TabIndex = 77;
            this.lblNetNotionalValue.Visible = false;
            this.lblNetNotionalValue.WrapText = false;
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label25.ForeColor = System.Drawing.Color.White;
            this.label25.Location = new System.Drawing.Point(644, 52);
            this.label25.Margin = new System.Windows.Forms.Padding(2);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(109, 19);
            this.label25.TabIndex = 76;
            this.label25.Text = "Net Notional Value";
            this.label25.Visible = false;
            this.label25.WrapText = false;
            // 
            // tlpConsolidationDashboard
            // 
            this.tlpConsolidationDashboard.ColumnCount = 14;
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpConsolidationDashboard.Controls.Add(this.lblMTDUnrealizedPnL, 3, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.label21, 2, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.label4, 2, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.label5, 2, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.label11, 2, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.lblShortExposureTotal, 3, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.lblLongExposureTotal, 3, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.lblNetExposureTotal, 3, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.label24, 6, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.lblTotalMTDPnL, 7, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.label20, 8, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.ultraLabel4, 4, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.ultraLabel5, 4, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.ultraLabel6, 4, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.lblPercentLongExposure, 5, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.lblPercentShortExposure, 5, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.lblPercentNetExposure, 5, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.label1, 4, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.lblDayPNLLongTotal, 5, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.label3, 0, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.lblDayPNLTotal, 1, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.label22, 10, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.label2, 0, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.lblDayPNLShortTotal, 1, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.label19, 0, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.lblPNLContribution, 1, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.lblCostBasisPnl, 0, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.lblCostBasisPNLValue, 1, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.lblMTDRealizedPNL, 11, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.ultraLabel1, 6, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.ultraLabel3, 6, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.ultraLabel2, 6, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.lblCostBasisRealizedPnL, 9, 3);
            this.tlpConsolidationDashboard.Controls.Add(this.lblNetMktValue, 7, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.lblShortMktValue, 7, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.lblLongMktValue, 7, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.lblCashValue, 8, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.label17, 8, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.lblCashProjected, 9, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.lblNetAssetValue, 9, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.label13, 10, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.label15, 10, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.label25, 10, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.lblLongNotionalValue, 11, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.lblShortNotionalValue, 11, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.lblNetNotionalValue, 11, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.lblPercentNetMktValue, 12, 0);
            this.tlpConsolidationDashboard.Controls.Add(this.lblPercentNetExposureGross, 12, 1);
            this.tlpConsolidationDashboard.Controls.Add(this.ultraLabel7, 12, 2);
            this.tlpConsolidationDashboard.Controls.Add(this.lblDayReturnGrossMarketValue, 13, 2);
            this.tlpConsolidationDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpConsolidationDashboard.Location = new System.Drawing.Point(0, 0);
            this.tlpConsolidationDashboard.Name = "tlpConsolidationDashboard";
            this.tlpConsolidationDashboard.RowCount = 4;
            this.tlpConsolidationDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpConsolidationDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpConsolidationDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpConsolidationDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpConsolidationDashboard.Size = new System.Drawing.Size(955, 102);
            this.tlpConsolidationDashboard.TabIndex = 78;
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel4.ForeColor = System.Drawing.Color.White;
            this.ultraLabel4.Location = new System.Drawing.Point(260, 2);
            this.ultraLabel4.Margin = new System.Windows.Forms.Padding(2);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(103, 15);
            this.ultraLabel4.TabIndex = 86;
            this.ultraLabel4.Text = "   % Long Exposure";
            this.ultraLabel4.WrapText = false;
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.AutoSize = true;
            this.ultraLabel5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel5.ForeColor = System.Drawing.Color.White;
            this.ultraLabel5.Location = new System.Drawing.Point(260, 27);
            this.ultraLabel5.Margin = new System.Windows.Forms.Padding(2);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(105, 15);
            this.ultraLabel5.TabIndex = 87;
            this.ultraLabel5.Text = "   % Short Exposure";
            this.ultraLabel5.WrapText = false;
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.AutoSize = true;
            this.ultraLabel6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel6.ForeColor = System.Drawing.Color.White;
            this.ultraLabel6.Location = new System.Drawing.Point(260, 52);
            this.ultraLabel6.Margin = new System.Windows.Forms.Padding(2);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(96, 15);
            this.ultraLabel6.TabIndex = 88;
            this.ultraLabel6.Text = "   % Net Exposure";
            this.ultraLabel6.WrapText = false;
            // 
            // lblPercentLongExposure
            // 
            this.lblPercentLongExposure.AutoSize = true;
            this.lblPercentLongExposure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPercentLongExposure.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblPercentLongExposure.ForeColor = System.Drawing.Color.White;
            this.lblPercentLongExposure.Location = new System.Drawing.Point(369, 2);
            this.lblPercentLongExposure.Margin = new System.Windows.Forms.Padding(2);
            this.lblPercentLongExposure.Name = "lblPercentLongExposure";
            this.lblPercentLongExposure.Size = new System.Drawing.Size(0, 0);
            this.lblPercentLongExposure.TabIndex = 89;
            // 
            // lblPercentShortExposure
            // 
            this.lblPercentShortExposure.AutoSize = true;
            this.lblPercentShortExposure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPercentShortExposure.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblPercentShortExposure.ForeColor = System.Drawing.Color.White;
            this.lblPercentShortExposure.Location = new System.Drawing.Point(369, 27);
            this.lblPercentShortExposure.Margin = new System.Windows.Forms.Padding(2);
            this.lblPercentShortExposure.Name = "lblPercentShortExposure";
            this.lblPercentShortExposure.Size = new System.Drawing.Size(0, 0);
            this.lblPercentShortExposure.TabIndex = 90;
            // 
            // lblPercentNetExposure
            // 
            this.lblPercentNetExposure.AutoSize = true;
            this.lblPercentNetExposure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPercentNetExposure.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblPercentNetExposure.ForeColor = System.Drawing.Color.White;
            this.lblPercentNetExposure.Location = new System.Drawing.Point(369, 52);
            this.lblPercentNetExposure.Margin = new System.Windows.Forms.Padding(2);
            this.lblPercentNetExposure.Name = "lblPercentNetExposure";
            this.lblPercentNetExposure.Size = new System.Drawing.Size(0, 0);
            this.lblPercentNetExposure.TabIndex = 91;
            // 
            // lblCostBasisPnl
            // 
            this.lblCostBasisPnl.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCostBasisPnl.ForeColor = System.Drawing.Color.White;
            this.lblCostBasisPnl.Location = new System.Drawing.Point(2, 52);
            this.lblCostBasisPnl.Margin = new System.Windows.Forms.Padding(2);
            this.lblCostBasisPnl.Name = "lblCostBasisPnl";
            this.lblCostBasisPnl.Size = new System.Drawing.Size(79, 15);
            this.lblCostBasisPnl.TabIndex = 84;
            this.lblCostBasisPnl.Text = "Cost Basis P&&L ";
            this.lblCostBasisPnl.WrapText = false;
            // 
            // lblCostBasisPNLValue
            // 
            this.lblCostBasisPNLValue.AutoSize = true;
            this.lblCostBasisPNLValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCostBasisPNLValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblCostBasisPNLValue.ForeColor = System.Drawing.Color.White;
            this.lblCostBasisPNLValue.Location = new System.Drawing.Point(98, 52);
            this.lblCostBasisPNLValue.Margin = new System.Windows.Forms.Padding(2);
            this.lblCostBasisPNLValue.Name = "lblCostBasisPNLValue";
            this.lblCostBasisPNLValue.Size = new System.Drawing.Size(0, 0);
            this.lblCostBasisPNLValue.TabIndex = 85;
            this.lblCostBasisPNLValue.WrapText = false;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel1.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Location = new System.Drawing.Point(404, 2);
            this.ultraLabel1.Margin = new System.Windows.Forms.Padding(2);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(94, 21);
            this.ultraLabel1.TabIndex = 78;
            this.ultraLabel1.Text = "   Long Mkt Value";
            this.ultraLabel1.WrapText = false;
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel3.Location = new System.Drawing.Point(404, 27);
            this.ultraLabel3.Margin = new System.Windows.Forms.Padding(2);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(94, 21);
            this.ultraLabel3.TabIndex = 80;
            this.ultraLabel3.Text = "   Short Mkt Value";
            this.ultraLabel3.WrapText = false;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraLabel2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel2.Location = new System.Drawing.Point(404, 52);
            this.ultraLabel2.Margin = new System.Windows.Forms.Padding(2);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(94, 21);
            this.ultraLabel2.TabIndex = 79;
            this.ultraLabel2.Text = "   Net Mkt Value";
            this.ultraLabel2.WrapText = false;
            // 
            // lblNetMktValue
            // 
            this.lblNetMktValue.AutoSize = true;
            this.lblNetMktValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetMktValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblNetMktValue.ForeColor = System.Drawing.Color.White;
            this.lblNetMktValue.Location = new System.Drawing.Point(502, 52);
            this.lblNetMktValue.Margin = new System.Windows.Forms.Padding(2);
            this.lblNetMktValue.Name = "lblNetMktValue";
            this.lblNetMktValue.Size = new System.Drawing.Size(0, 0);
            this.lblNetMktValue.TabIndex = 83;
            this.lblNetMktValue.WrapText = false;
            // 
            // lblShortMktValue
            // 
            this.lblShortMktValue.AutoSize = true;
            this.lblShortMktValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShortMktValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblShortMktValue.ForeColor = System.Drawing.Color.White;
            this.lblShortMktValue.Location = new System.Drawing.Point(502, 27);
            this.lblShortMktValue.Margin = new System.Windows.Forms.Padding(2);
            this.lblShortMktValue.Name = "lblShortMktValue";
            this.lblShortMktValue.Size = new System.Drawing.Size(0, 0);
            this.lblShortMktValue.TabIndex = 81;
            this.lblShortMktValue.WrapText = false;
            // 
            // lblLongMktValue
            // 
            this.lblLongMktValue.AutoSize = true;
            this.lblLongMktValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLongMktValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblLongMktValue.ForeColor = System.Drawing.Color.White;
            this.lblLongMktValue.Location = new System.Drawing.Point(502, 2);
            this.lblLongMktValue.Margin = new System.Windows.Forms.Padding(2);
            this.lblLongMktValue.Name = "lblLongMktValue";
            this.lblLongMktValue.Size = new System.Drawing.Size(0, 0);
            this.lblLongMktValue.TabIndex = 82;
            this.lblLongMktValue.WrapText = false;
            // 
            // lblShortNotionalValue
            // 
            this.lblShortNotionalValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblShortNotionalValue.ForeColor = System.Drawing.Color.White;
            this.lblShortNotionalValue.Location = new System.Drawing.Point(757, 27);
            this.lblShortNotionalValue.Margin = new System.Windows.Forms.Padding(2);
            this.lblShortNotionalValue.Name = "lblShortNotionalValue";
            this.lblShortNotionalValue.Size = new System.Drawing.Size(44, 10);
            this.lblShortNotionalValue.TabIndex = 56;
            this.lblShortNotionalValue.Visible = false;
            this.lblShortNotionalValue.WrapText = false;
            // 
            // lblPercentNetMktValue
            // 
            this.lblPercentNetMktValue.Font = new System.Drawing.Font("Tahoma", 6.75F);
            this.lblPercentNetMktValue.Location = new System.Drawing.Point(806, 3);
            this.lblPercentNetMktValue.Name = "lblPercentNetMktValue";
            this.lblPercentNetMktValue.Size = new System.Drawing.Size(65, 19);
            this.lblPercentNetMktValue.TabIndex = 92;
            // 
            // lblPercentNetExposureGross
            // 
            this.lblPercentNetExposureGross.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblPercentNetExposureGross.ForeColor = System.Drawing.Color.White;
            this.lblPercentNetExposureGross.Location = new System.Drawing.Point(806, 27);
            this.lblPercentNetExposureGross.Margin = new System.Windows.Forms.Padding(2);
            this.lblPercentNetExposureGross.Name = "lblShortNotionalValue";
            this.lblPercentNetExposureGross.Size = new System.Drawing.Size(44, 10);
            this.lblPercentNetExposureGross.TabIndex = 56;
            this.lblPercentNetExposureGross.Visible = false;
            this.lblPercentNetExposureGross.WrapText = false;
            // 
            // ultraLabel7
            // 
            this.ultraLabel7.Location = new System.Drawing.Point(806, 53);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(83, 19);
            this.ultraLabel7.TabIndex = 93;
            this.ultraLabel7.Text = "Day Return (Gross Market Value)";
            this.ultraLabel7.Visible = false;
            this.ultraLabel7.WrapText = false;
            // 
            // lblDayReturnGrossMarketValue
            // 
            this.lblDayReturnGrossMarketValue.Location = new System.Drawing.Point(895, 53);
            this.lblDayReturnGrossMarketValue.Name = "lblDayReturnGrossMarketValue";
            this.lblDayReturnGrossMarketValue.Size = new System.Drawing.Size(60, 19);
            this.lblDayReturnGrossMarketValue.TabIndex = 94;
            this.lblDayReturnGrossMarketValue.Visible = false;
            this.lblDayReturnGrossMarketValue.WrapText = false;
            // 
            // CtrlMainConsolidationViewSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.tlpConsolidationDashboard);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CtrlMainConsolidationViewSummary";
            this.Size = new System.Drawing.Size(955, 102);
            this.tlpConsolidationDashboard.ResumeLayout(false);
            this.tlpConsolidationDashboard.PerformLayout();
            this.ResumeLayout(false);

        }

        

        #endregion

        private Infragistics.Win.Misc.UltraLabel label1;
        private Infragistics.Win.Misc.UltraLabel lblDayPNLLongTotal;
        private Infragistics.Win.Misc.UltraLabel label2;
        private Infragistics.Win.Misc.UltraLabel lblDayPNLShortTotal;
        private Infragistics.Win.Misc.UltraLabel label3;
        private Infragistics.Win.Misc.UltraLabel lblDayPNLTotal;
        private Infragistics.Win.Misc.UltraLabel label4;
        private Infragistics.Win.Misc.UltraLabel lblLongExposureTotal;
        private Infragistics.Win.Misc.UltraLabel label5;
        private Infragistics.Win.Misc.UltraLabel lblShortExposureTotal;
        private Infragistics.Win.Misc.UltraLabel label11;
        private Infragistics.Win.Misc.UltraLabel lblNetExposureTotal;
        private Infragistics.Win.Misc.UltraLabel label13;
        private Infragistics.Win.Misc.UltraLabel label15;
        private Infragistics.Win.Misc.UltraLabel lblLongNotionalValue;
        private Infragistics.Win.Misc.UltraLabel lblNetAssetValue;
        private Infragistics.Win.Misc.UltraLabel label17;
        private Infragistics.Win.Misc.UltraLabel lblCashProjected;
        private Infragistics.Win.Misc.UltraLabel lblCashValue;
        private Infragistics.Win.Misc.UltraLabel lblPNLContribution;
        private Infragistics.Win.Misc.UltraLabel label19;
        private Infragistics.Win.Misc.UltraLabel lblCostBasisRealizedPnL;
        private Infragistics.Win.Misc.UltraLabel label22;
        private Infragistics.Win.Misc.UltraLabel label20;
        private Infragistics.Win.Misc.UltraLabel label21;
        private Infragistics.Win.Misc.UltraLabel lblMTDUnrealizedPnL;
        private Infragistics.Win.Misc.UltraLabel lblMTDRealizedPNL;
        private Infragistics.Win.Misc.UltraLabel lblTotalMTDPnL;
        private Infragistics.Win.Misc.UltraLabel label24;
        private Infragistics.Win.Misc.UltraLabel lblNetNotionalValue;
        private Infragistics.Win.Misc.UltraLabel label25;
        private System.Windows.Forms.TableLayoutPanel tlpConsolidationDashboard;
        private Infragistics.Win.Misc.UltraLabel lblShortNotionalValue;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel lblLongMktValue;
        private Infragistics.Win.Misc.UltraLabel lblShortMktValue;
        private Infragistics.Win.Misc.UltraLabel lblNetMktValue;
        private Infragistics.Win.Misc.UltraLabel lblCostBasisPnl;
        private Infragistics.Win.Misc.UltraLabel lblCostBasisPNLValue;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.Misc.UltraLabel lblPercentLongExposure;
        private Infragistics.Win.Misc.UltraLabel lblPercentShortExposure;
        private Infragistics.Win.Misc.UltraLabel lblPercentNetExposure;
        private Infragistics.Win.Misc.UltraLabel lblPercentNetMktValue;
        private Infragistics.Win.Misc.UltraLabel lblPercentNetExposureGross;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.Misc.UltraLabel lblDayReturnGrossMarketValue;
    }
}
