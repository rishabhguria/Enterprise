using Infragistics.Win.Misc;
using System;

namespace Prana.WashSale.Controls
{
    partial class WashSaleTradesFiltersUC
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblAccount = new Infragistics.Win.Misc.UltraLabel();
            this.lblAssetClass = new Infragistics.Win.Misc.UltraLabel();
            this.lblCurrency = new Infragistics.Win.Misc.UltraLabel();
            this.multiSelectDropDownAccount = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.multiSelectDropDownAssetClass = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.multiSelectDropDownCurrency = new Prana.Utilities.UI.UIUtilities.MultiSelectDropDown();
            this.btnGetData = new Infragistics.Win.Misc.UltraButton();
            this.filterLabel = new Infragistics.Win.Misc.UltraLabel();
            this.SuspendLayout();
            // 
            // lblAccount
            // 
            this.lblAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccount.Location = new System.Drawing.Point(3, 22);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(51, 18);
            this.lblAccount.TabIndex = 0;
            this.lblAccount.Text = "Account";
            // 
            // lblAssetClass
            // 
            this.lblAssetClass.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAssetClass.Location = new System.Drawing.Point(232, 22);
            this.lblAssetClass.Name = "lblAssetClass";
            this.lblAssetClass.Size = new System.Drawing.Size(65, 18);
            this.lblAssetClass.TabIndex = 1;
            this.lblAssetClass.Text = "Asset Class";
            // 
            // lblCurrency
            // 
            this.lblCurrency.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrency.Location = new System.Drawing.Point(468, 22);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(62, 18);
            this.lblCurrency.TabIndex = 2;
            this.lblCurrency.Text = "Currency";
            // 
            // multiSelectDropDownAccount
            // 
            this.multiSelectDropDownAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multiSelectDropDownAccount.Location = new System.Drawing.Point(65, 22);
            this.multiSelectDropDownAccount.Margin = new System.Windows.Forms.Padding(4);
            this.multiSelectDropDownAccount.Name = "multiSelectDropDownAccount";
            this.multiSelectDropDownAccount.Size = new System.Drawing.Size(153, 27);
            this.multiSelectDropDownAccount.TabIndex = 3;
            this.multiSelectDropDownAccount.TitleText = "";
            this.multiSelectDropDownAccount.CheckStateChanged += new EventHandler<System.Windows.Forms.ItemCheckEventArgs>(this.SelectDropDownAccount);
            // 
            // multiSelectDropDownAssetClass
            // 
            this.multiSelectDropDownAssetClass.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multiSelectDropDownAssetClass.Location = new System.Drawing.Point(304, 22);
            this.multiSelectDropDownAssetClass.Margin = new System.Windows.Forms.Padding(4);
            this.multiSelectDropDownAssetClass.Name = "multiSelectDropDownAssetClass";
            this.multiSelectDropDownAssetClass.Size = new System.Drawing.Size(153, 27);
            this.multiSelectDropDownAssetClass.TabIndex = 4;
            this.multiSelectDropDownAssetClass.TitleText = "";
            this.multiSelectDropDownAssetClass.CheckStateChanged += new EventHandler<System.Windows.Forms.ItemCheckEventArgs>(this.SelectDropDownAccount);
            // 
            // multiSelectDropDownCurrency
            // 
            this.multiSelectDropDownCurrency.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multiSelectDropDownCurrency.Location = new System.Drawing.Point(531, 22);
            this.multiSelectDropDownCurrency.Margin = new System.Windows.Forms.Padding(4);
            this.multiSelectDropDownCurrency.Name = "multiSelectDropDownCurrency";
            this.multiSelectDropDownCurrency.Size = new System.Drawing.Size(153, 27);
            this.multiSelectDropDownCurrency.TabIndex = 5;
            this.multiSelectDropDownCurrency.TitleText = "";
            this.multiSelectDropDownCurrency.CheckStateChanged += new EventHandler<System.Windows.Forms.ItemCheckEventArgs>(this.SelectDropDownAccount);
            // 
            // btnGetData
            // 
            this.btnGetData.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(156)))), ((int)(((byte)(46)))));
            this.btnGetData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
            this.btnGetData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetData.ForeColor = System.Drawing.Color.White;
            this.btnGetData.Location = new System.Drawing.Point(304, 56);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(96, 23);
            this.btnGetData.TabIndex = 6;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.UseAppStyling = false;
            this.btnGetData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // filterLabel
            // 
            this.filterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.filterLabel.Location = new System.Drawing.Point(332, 85);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(40, 20);
            this.filterLabel.TabIndex = 7;
            this.filterLabel.Text = "Filter";
            // 
            // WashSaleTradesFiltersUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.filterLabel);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.multiSelectDropDownCurrency);
            this.Controls.Add(this.multiSelectDropDownAssetClass);
            this.Controls.Add(this.multiSelectDropDownAccount);
            this.Controls.Add(this.lblCurrency);
            this.Controls.Add(this.lblAssetClass);
            this.Controls.Add(this.lblAccount);
            this.Name = "WashSaleTradesFiltersUC";
            this.Size = new System.Drawing.Size(708, 106);
            this.Load += new System.EventHandler(this.WashSaleTradesFiltersUC_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UltraLabel lblAccount;
        private UltraLabel lblAssetClass;
        private UltraLabel lblCurrency;
        private Utilities.UI.UIUtilities.MultiSelectDropDown multiSelectDropDownAccount;
        private Utilities.UI.UIUtilities.MultiSelectDropDown multiSelectDropDownAssetClass;
        private Utilities.UI.UIUtilities.MultiSelectDropDown multiSelectDropDownCurrency;
        private UltraButton btnGetData;
        private UltraLabel filterLabel;
    }
}
