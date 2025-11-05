//using System;
//using Prana.PM.BLL;
//using Prana.Logging;

//namespace Prana.PM.Client.UI.Controls
//{
//    partial class OptionBookSummaryDashboard
//    {
//        /// <summary> 
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            try
//            {
//                if (disposing && (components != null))
//                {
//                    components.Dispose();
//                }

//                _isInitialized = false;


//                //if (this._consolidatedInfoManagerInstance != null)
//                //{
//                //    this._consolidatedInfoManagerInstance.ConsolidatedInfoListChanged -= new ConsolidatedInfoManager.MethodHandler(consolidatedInfoManagerInstance_ConsolidatedInfoListChanged);
//                //    this._consolidatedInfoManagerInstance.ConsolidatedInfoSummaryChanged -= new ConsolidatedInfoManager.MethodHandler(consolidatedInfoManagerInstance_ConsolidatedInfoSummaryChanged);
//                //}

//                //this._consolidatedInfoManagerInstance = null;


//                base.Dispose(disposing);
//            }
//            catch (Exception ex)
//            {

//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
//            this.label1 = new Infragistics.Win.Misc.UltraLabel();
//            this.lblBookDelta = new Infragistics.Win.Misc.UltraLabel();
//            this.label2 = new Infragistics.Win.Misc.UltraLabel();
//            this.lblBookGamma = new Infragistics.Win.Misc.UltraLabel();
//            this.label3 = new Infragistics.Win.Misc.UltraLabel();
//            this.lblBookTheta = new Infragistics.Win.Misc.UltraLabel();
//            this.label4 = new Infragistics.Win.Misc.UltraLabel();
//            this.lblLongExposureTotal = new Infragistics.Win.Misc.UltraLabel();
//            this.label5 = new Infragistics.Win.Misc.UltraLabel();
//            this.lblShortExposureTotal = new Infragistics.Win.Misc.UltraLabel();
//            this.label11 = new Infragistics.Win.Misc.UltraLabel();
//            this.lblNetExposureTotal = new Infragistics.Win.Misc.UltraLabel();
//            this.label13 = new Infragistics.Win.Misc.UltraLabel();
//            this.label15 = new Infragistics.Win.Misc.UltraLabel();
//            this.lblBookKappa = new Infragistics.Win.Misc.UltraLabel();
//            this.lblBookRho = new Infragistics.Win.Misc.UltraLabel();
//            this.label6 = new Infragistics.Win.Misc.UltraLabel();
//            this.lblNetPNL = new Infragistics.Win.Misc.UltraLabel();
//            this.label17 = new Infragistics.Win.Misc.UltraLabel();
//            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
//            this.tableLayoutPanel1.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // label1
//            // 
//            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.label1.ForeColor = System.Drawing.Color.White;
//            this.label1.Location = new System.Drawing.Point(3, 3);
//            this.label1.Name = "label1";
//            this.label1.Size = new System.Drawing.Size(66, 15);
//            this.label1.TabIndex = 56;
//            this.label1.Text = "Book Delta";
//            // 
//            // lblBookDelta
//            // 
//            this.lblBookDelta.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.lblBookDelta.ForeColor = System.Drawing.Color.White;
//            this.lblBookDelta.Location = new System.Drawing.Point(93, 3);
//            this.lblBookDelta.Name = "lblBookDelta";
//            this.lblBookDelta.Size = new System.Drawing.Size(85, 15);
//            this.lblBookDelta.TabIndex = 56;
//            this.lblBookDelta.Text = "<Book Delta>";
//            // 
//            // label2
//            // 
//            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.label2.ForeColor = System.Drawing.Color.White;
//            this.label2.Location = new System.Drawing.Point(3, 36);
//            this.label2.Name = "label2";
//            this.label2.Size = new System.Drawing.Size(80, 15);
//            this.label2.TabIndex = 56;
//            this.label2.Text = "Book Gamma";
//            // 
//            // lblBookGamma
//            // 
//            this.lblBookGamma.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.lblBookGamma.ForeColor = System.Drawing.Color.White;
//            this.lblBookGamma.Location = new System.Drawing.Point(93, 36);
//            this.lblBookGamma.Name = "lblBookGamma";
//            this.lblBookGamma.Size = new System.Drawing.Size(98, 15);
//            this.lblBookGamma.TabIndex = 56;
//            this.lblBookGamma.Text = "<Book Gamma>";
//            // 
//            // label3
//            // 
//            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.label3.ForeColor = System.Drawing.Color.White;
//            this.label3.Location = new System.Drawing.Point(3, 69);
//            this.label3.Name = "label3";
//            this.label3.Size = new System.Drawing.Size(68, 15);
//            this.label3.TabIndex = 56;
//            this.label3.Text = "Book Theta";
//            // 
//            // lblBookTheta
//            // 
//            this.lblBookTheta.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.lblBookTheta.ForeColor = System.Drawing.Color.White;
//            this.lblBookTheta.Location = new System.Drawing.Point(93, 69);
//            this.lblBookTheta.Name = "lblBookTheta";
//            this.lblBookTheta.Size = new System.Drawing.Size(87, 15);
//            this.lblBookTheta.TabIndex = 56;
//            this.lblBookTheta.Text = "<Book Theta>";
//            // 
//            // label4
//            // 
//            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.label4.ForeColor = System.Drawing.Color.White;
//            this.label4.Location = new System.Drawing.Point(227, 3);
//            this.label4.Name = "label4";
//            this.label4.Size = new System.Drawing.Size(72, 15);
//            this.label4.TabIndex = 56;
//            this.label4.Text = "Book Kappa";
//            // 
//            // lblLongExposureTotal
//            // 
//            this.lblLongExposureTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.lblLongExposureTotal.ForeColor = System.Drawing.Color.White;
//            this.lblLongExposureTotal.Location = new System.Drawing.Point(558, 3);
//            this.lblLongExposureTotal.Name = "lblLongExposureTotal";
//            this.lblLongExposureTotal.Size = new System.Drawing.Size(142, 18);
//            this.lblLongExposureTotal.TabIndex = 56;
//            this.lblLongExposureTotal.Text = "<Long Exposure Total>";
//            // 
//            // label5
//            // 
//            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.label5.ForeColor = System.Drawing.Color.White;
//            this.label5.Location = new System.Drawing.Point(227, 36);
//            this.label5.Name = "label5";
//            this.label5.Size = new System.Drawing.Size(59, 15);
//            this.label5.TabIndex = 56;
//            this.label5.Text = "Book Rho";
//            // 
//            // lblShortExposureTotal
//            // 
//            this.lblShortExposureTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.lblShortExposureTotal.ForeColor = System.Drawing.Color.White;
//            this.lblShortExposureTotal.Location = new System.Drawing.Point(558, 36);
//            this.lblShortExposureTotal.Name = "lblShortExposureTotal";
//            this.lblShortExposureTotal.Size = new System.Drawing.Size(142, 19);
//            this.lblShortExposureTotal.TabIndex = 56;
//            this.lblShortExposureTotal.Text = "<Short Exposure Total>";
//            // 
//            // label11
//            // 
//            this.label11.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.label11.ForeColor = System.Drawing.Color.White;
//            this.label11.Location = new System.Drawing.Point(227, 69);
//            this.label11.Name = "label11";
//            this.label11.Size = new System.Drawing.Size(50, 15);
//            this.label11.TabIndex = 56;
//            this.label11.Text = "Net P&&L";
//            // 
//            // lblNetExposureTotal
//            // 
//            this.lblNetExposureTotal.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.lblNetExposureTotal.ForeColor = System.Drawing.Color.White;
//            this.lblNetExposureTotal.Location = new System.Drawing.Point(558, 69);
//            this.lblNetExposureTotal.Name = "lblNetExposureTotal";
//            this.lblNetExposureTotal.Size = new System.Drawing.Size(142, 20);
//            this.lblNetExposureTotal.TabIndex = 56;
//            this.lblNetExposureTotal.Text = "<Net Exposure Total>";
//            // 
//            // label13
//            // 
//            appearance1.ForeColor = System.Drawing.Color.White;
//            this.label13.Appearance = appearance1;
//            this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.label13.ForeColor = System.Drawing.Color.White;
//            this.label13.Location = new System.Drawing.Point(455, 3);
//            this.label13.Name = "label13";
//            this.label13.Size = new System.Drawing.Size(88, 15);
//            this.label13.TabIndex = 56;
//            this.label13.Text = "Long Exposure";
//            // 
//            // label15
//            // 
//            this.label15.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.label15.ForeColor = System.Drawing.Color.White;
//            this.label15.Location = new System.Drawing.Point(455, 36);
//            this.label15.Name = "label15";
//            this.label15.Size = new System.Drawing.Size(91, 15);
//            this.label15.TabIndex = 56;
//            this.label15.Text = "Short Exposure";
//            // 
//            // lblBookKappa
//            // 
//            this.lblBookKappa.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.lblBookKappa.ForeColor = System.Drawing.Color.White;
//            this.lblBookKappa.Location = new System.Drawing.Point(316, 3);
//            this.lblBookKappa.Name = "lblBookKappa";
//            this.lblBookKappa.Size = new System.Drawing.Size(90, 15);
//            this.lblBookKappa.TabIndex = 56;
//            this.lblBookKappa.Text = "<Book Kappa>";
//            // 
//            // lblBookRho
//            // 
//            this.lblBookRho.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.lblBookRho.ForeColor = System.Drawing.Color.White;
//            this.lblBookRho.Location = new System.Drawing.Point(316, 36);
//            this.lblBookRho.Name = "lblBookRho";
//            this.lblBookRho.Size = new System.Drawing.Size(74, 15);
//            this.lblBookRho.TabIndex = 56;
//            this.lblBookRho.Text = "<BookRho>";
//            // 
//            // label6
//            // 
//            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
//            this.label6.Location = new System.Drawing.Point(373, 17);
//            this.label6.Name = "label6";
//            this.label6.Size = new System.Drawing.Size(0, 0);
//            this.label6.TabIndex = 56;
//            // 
//            // lblNetPNL
//            // 
//            appearance2.ForeColor = System.Drawing.Color.White;
//            this.lblNetPNL.Appearance = appearance2;
//            this.lblNetPNL.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
//            this.lblNetPNL.ForeColor = System.Drawing.Color.White;
//            this.lblNetPNL.Location = new System.Drawing.Point(316, 69);
//            this.lblNetPNL.Name = "lblNetPNL";
//            this.lblNetPNL.Size = new System.Drawing.Size(69, 15);
//            this.lblNetPNL.TabIndex = 61;
//            this.lblNetPNL.Text = "<Net P&&L>";
//            // 
//            // label17
//            // 
//            appearance3.ForeColor = System.Drawing.Color.White;
//            this.label17.Appearance = appearance3;
//            this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.label17.ForeColor = System.Drawing.Color.White;
//            this.label17.Location = new System.Drawing.Point(455, 69);
//            this.label17.Name = "label17";
//            this.label17.Size = new System.Drawing.Size(80, 15);
//            this.label17.TabIndex = 60;
//            this.label17.Text = "Net Exposure";
//            // 
//            // tableLayoutPanel1
//            // 
//            this.tableLayoutPanel1.ColumnCount = 6;
//            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.81296F));
//            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.0407F));
//            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.64535F));
//            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.7349F));
//            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.72754F));
//            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.76583F));
//            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
//            this.tableLayoutPanel1.Controls.Add(this.lblBookDelta, 1, 0);
//            this.tableLayoutPanel1.Controls.Add(this.lblNetExposureTotal, 5, 2);
//            this.tableLayoutPanel1.Controls.Add(this.lblShortExposureTotal, 5, 1);
//            this.tableLayoutPanel1.Controls.Add(this.label17, 4, 2);
//            this.tableLayoutPanel1.Controls.Add(this.lblLongExposureTotal, 5, 0);
//            this.tableLayoutPanel1.Controls.Add(this.lblNetPNL, 3, 2);
//            this.tableLayoutPanel1.Controls.Add(this.label15, 4, 1);
//            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
//            this.tableLayoutPanel1.Controls.Add(this.label13, 4, 0);
//            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
//            this.tableLayoutPanel1.Controls.Add(this.lblBookRho, 3, 1);
//            this.tableLayoutPanel1.Controls.Add(this.lblBookGamma, 1, 1);
//            this.tableLayoutPanel1.Controls.Add(this.lblBookKappa, 3, 0);
//            this.tableLayoutPanel1.Controls.Add(this.lblBookTheta, 1, 2);
//            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
//            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 1);
//            this.tableLayoutPanel1.Controls.Add(this.label11, 2, 2);
//            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
//            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
//            this.tableLayoutPanel1.RowCount = 3;
//            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
//            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
//            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
//            this.tableLayoutPanel1.Size = new System.Drawing.Size(703, 100);
//            this.tableLayoutPanel1.TabIndex = 62;
//            // 
//            // OptionBookSummaryDashboard
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.BackColor = System.Drawing.Color.Black;
//            this.Controls.Add(this.tableLayoutPanel1);
//            this.Controls.Add(this.label6);
//            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
//            this.Name = "OptionBookSummaryDashboard";
//            this.Size = new System.Drawing.Size(706, 108);
//            this.tableLayoutPanel1.ResumeLayout(false);
//            this.ResumeLayout(false);

//        }

        

//        #endregion

//        private Infragistics.Win.Misc.UltraLabel label1;
//        private Infragistics.Win.Misc.UltraLabel lblBookDelta;
//        private Infragistics.Win.Misc.UltraLabel label2;
//        private Infragistics.Win.Misc.UltraLabel lblBookGamma;
//        private Infragistics.Win.Misc.UltraLabel label3;
//        private Infragistics.Win.Misc.UltraLabel lblBookTheta;
//        private Infragistics.Win.Misc.UltraLabel label4;
//        private Infragistics.Win.Misc.UltraLabel lblLongExposureTotal;
//        private Infragistics.Win.Misc.UltraLabel label5;
//        private Infragistics.Win.Misc.UltraLabel lblShortExposureTotal;
//        private Infragistics.Win.Misc.UltraLabel label11;
//        private Infragistics.Win.Misc.UltraLabel lblNetExposureTotal;
//        private Infragistics.Win.Misc.UltraLabel label13;
//        private Infragistics.Win.Misc.UltraLabel label15;
//        private Infragistics.Win.Misc.UltraLabel lblBookKappa;
//        private Infragistics.Win.Misc.UltraLabel lblBookRho;
//        private Infragistics.Win.Misc.UltraLabel label6;
//        private Infragistics.Win.Misc.UltraLabel lblNetPNL;
//        private Infragistics.Win.Misc.UltraLabel label17;
//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
//    }
//}
