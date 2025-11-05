using Prana.Utilities.UI.UIUtilities;

namespace Prana.ClientCommon
{
    partial class PranaOptionCtrl
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
            this.components = new System.ComponentModel.Container();
            this.lblUnderlyingSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.lblStrikePrice = new Infragistics.Win.Misc.UltraLabel();
            this.lblOptionType = new Infragistics.Win.Misc.UltraLabel();
            this.cmbOptionType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.dtExpirationDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblExpirationDate = new Infragistics.Win.Misc.UltraLabel();
            this.tmValidate = new System.Windows.Forms.Timer(this.components);
            this.txtUnderlyingSymbol = new Prana.Utilities.UI.UIUtilities.PranaSymbolCtrl();
            this.txtStrikePrice = new Spinner();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOptionType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtExpirationDate)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUnderlyingSymbol
            // 
            this.lblUnderlyingSymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnderlyingSymbol.Location = new System.Drawing.Point(1, -1);
            this.lblUnderlyingSymbol.Name = "lblUnderlyingSymbol";
            this.lblUnderlyingSymbol.Size = new System.Drawing.Size(106, 16);
            this.lblUnderlyingSymbol.TabIndex = 1;
            this.lblUnderlyingSymbol.Text = "Underlying Symbol";
            // 
            // lblStrikePrice
            // 
            this.lblStrikePrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStrikePrice.Location = new System.Drawing.Point(107, 0);
            this.lblStrikePrice.Name = "lblStrikePrice";
            this.lblStrikePrice.Size = new System.Drawing.Size(62, 15);
            this.lblStrikePrice.TabIndex = 3;
            this.lblStrikePrice.Text = "Strike Price";
            // 
            // lblOptionType
            // 
            this.lblOptionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptionType.Location = new System.Drawing.Point(3, 41);
            this.lblOptionType.Name = "lblOptionType";
            this.lblOptionType.Size = new System.Drawing.Size(70, 16);
            this.lblOptionType.TabIndex = 5;
            this.lblOptionType.Text = "Option Type";
            // 
            // cmbOptionType
            // 
            this.cmbOptionType.AutoSize = false;
            this.cmbOptionType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbOptionType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbOptionType.Location = new System.Drawing.Point(3, 57);
            this.cmbOptionType.Name = "cmbOptionType";
            this.cmbOptionType.Size = new System.Drawing.Size(70, 21);
            this.cmbOptionType.TabIndex = 6;
            this.cmbOptionType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbOptionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.cmbOptionType.ValueChanged += new System.EventHandler(this.cmbOptionType_ValueChanged);
            this.cmbOptionType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.event_KeyUp);
            this.cmbOptionType.Leave += new System.EventHandler(this.event_Leave);
            // 
            // dtExpirationDate
            // 
            this.dtExpirationDate.AutoSize = false;
            this.dtExpirationDate.DateTime = new System.DateTime(2015, 1, 26, 0, 0, 0, 0);
            this.dtExpirationDate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dtExpirationDate.Location = new System.Drawing.Point(96, 59);
            this.dtExpirationDate.MinDate = new System.DateTime(1800, 1, 1, 0, 0, 0, 0);
            this.dtExpirationDate.Name = "dtExpirationDate";
            this.dtExpirationDate.Size = new System.Drawing.Size(90, 21);
            this.dtExpirationDate.TabIndex = 8;
            this.dtExpirationDate.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextSection;
            this.dtExpirationDate.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.dtExpirationDate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.dtExpirationDate.Value = new System.DateTime(2015, 1, 26, 0, 0, 0, 0);
            this.dtExpirationDate.ValueChanged += new System.EventHandler(this.dtExpirationDate_ValueChanged);
            this.dtExpirationDate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.event_KeyUp);
            this.dtExpirationDate.Leave += new System.EventHandler(this.event_Leave);
            // 
            // lblExpirationDate
            // 
            this.lblExpirationDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExpirationDate.Location = new System.Drawing.Point(95, 41);
            this.lblExpirationDate.Margin = new System.Windows.Forms.Padding(0);
            this.lblExpirationDate.Name = "lblExpirationDate";
            this.lblExpirationDate.Size = new System.Drawing.Size(91, 16);
            this.lblExpirationDate.TabIndex = 7;
            this.lblExpirationDate.Text = "Expiration Date";
            // 
            // tmValidate
            // 
            this.tmValidate.Interval = 500;
            this.tmValidate.Tick += new System.EventHandler(this.tmValidate_Tick);
            // 
            // txtUnderlyingSymbol
            // 
            this.txtUnderlyingSymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUnderlyingSymbol.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtUnderlyingSymbol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtUnderlyingSymbol.Location = new System.Drawing.Point(3, 15);
            this.txtUnderlyingSymbol.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtUnderlyingSymbol.MaxLength = 32767;
            this.txtUnderlyingSymbol.Name = "txtUnderlyingSymbol";
            this.txtUnderlyingSymbol.PrevSymbolEntered = "";
            this.txtUnderlyingSymbol.Size = new System.Drawing.Size(81, 21);
            this.txtUnderlyingSymbol.TabIndex = 2;
            this.txtUnderlyingSymbol.SymbolEntered += new System.EventHandler<Prana.Global.EventArgs<string, string>>(this.txtUnderlyingSymbol_SymbolEntered);
            // 
            // txtStrikePrice
            // 
            this.txtStrikePrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtStrikePrice.DataType = DataTypes.Numeric;
            this.txtStrikePrice.DecimalEntered = false;
            this.txtStrikePrice.DecimalPoints = 2147483647;
            this.txtStrikePrice.Increment = 0.01D;
            this.txtStrikePrice.Location = new System.Drawing.Point(107, 15);
            this.txtStrikePrice.MaxValue = 9999999D;
            this.txtStrikePrice.MinValue = -9999999D;
            this.txtStrikePrice.Name = "txtStrikePrice";
            this.txtStrikePrice.Size = new System.Drawing.Size(79, 20);
            this.txtStrikePrice.TabIndex = 4;
            this.txtStrikePrice.Value = 0D;
            this.txtStrikePrice.ValueChanged += new System.EventHandler(this.txtStrikePrice_ValueChanged);
            this.txtStrikePrice.KeyUp += new System.Windows.Forms.KeyEventHandler(this.event_KeyUp);
            this.txtStrikePrice.Leave += new System.EventHandler(this.event_Leave);
            // 
            // PranaOptionCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this.Controls.Add(this.txtUnderlyingSymbol);
            this.Controls.Add(this.lblExpirationDate);
            this.Controls.Add(this.dtExpirationDate);
            this.Controls.Add(this.txtStrikePrice);
            this.Controls.Add(this.cmbOptionType);
            this.Controls.Add(this.lblOptionType);
            this.Controls.Add(this.lblStrikePrice);
            this.Controls.Add(this.lblUnderlyingSymbol);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PranaOptionCtrl";
            this.Size = new System.Drawing.Size(193, 85);
            ((System.ComponentModel.ISupportInitialize)(this.cmbOptionType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtExpirationDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblUnderlyingSymbol;
        private Infragistics.Win.Misc.UltraLabel lblStrikePrice;
        private Infragistics.Win.Misc.UltraLabel lblOptionType;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbOptionType;
        private Spinner txtStrikePrice;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtExpirationDate;
        private Infragistics.Win.Misc.UltraLabel lblExpirationDate;
        private PranaSymbolCtrl txtUnderlyingSymbol;
        private System.Windows.Forms.Timer tmValidate;
    }
}
