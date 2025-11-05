namespace Prana.PMLauncher
{
    partial class ErrorProviderCheck
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label address1Label;
            System.Windows.Forms.Label address2Label;
            System.Windows.Forms.Label countryIdLabel;
            System.Windows.Forms.Label errorLabel;
            System.Windows.Forms.Label faxNumberLabel;
            System.Windows.Forms.Label stateIdLabel;
            System.Windows.Forms.Label workNumberLabel;
            System.Windows.Forms.Label zipLabel;
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.address1TextBox = new System.Windows.Forms.TextBox();
            this.address2TextBox = new System.Windows.Forms.TextBox();
            this.countryIdTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.faxNumberTextBox = new System.Windows.Forms.TextBox();
            this.stateIdTextBox = new System.Windows.Forms.TextBox();
            this.workNumberTextBox = new System.Windows.Forms.TextBox();
            this.zipTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.addressDetailsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button2 = new System.Windows.Forms.Button();
            address1Label = new System.Windows.Forms.Label();
            address2Label = new System.Windows.Forms.Label();
            countryIdLabel = new System.Windows.Forms.Label();
            errorLabel = new System.Windows.Forms.Label();
            faxNumberLabel = new System.Windows.Forms.Label();
            stateIdLabel = new System.Windows.Forms.Label();
            workNumberLabel = new System.Windows.Forms.Label();
            zipLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addressDetailsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // address1Label
            // 
            address1Label.AutoSize = true;
            address1Label.Location = new System.Drawing.Point(77, 31);
            address1Label.Name = "address1Label";
            address1Label.Size = new System.Drawing.Size(54, 13);
            address1Label.TabIndex = 0;
            address1Label.Text = "Address1:";
            // 
            // address2Label
            // 
            address2Label.AutoSize = true;
            address2Label.Location = new System.Drawing.Point(77, 57);
            address2Label.Name = "address2Label";
            address2Label.Size = new System.Drawing.Size(54, 13);
            address2Label.TabIndex = 2;
            address2Label.Text = "Address2:";
            // 
            // countryIdLabel
            // 
            countryIdLabel.AutoSize = true;
            countryIdLabel.Location = new System.Drawing.Point(77, 83);
            countryIdLabel.Name = "countryIdLabel";
            countryIdLabel.Size = new System.Drawing.Size(58, 13);
            countryIdLabel.TabIndex = 4;
            countryIdLabel.Text = "Country Id:";
            // 
            // errorLabel
            // 
            errorLabel.AutoSize = true;
            errorLabel.Location = new System.Drawing.Point(77, 109);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new System.Drawing.Size(32, 13);
            errorLabel.TabIndex = 6;
            errorLabel.Text = "Error:";
            // 
            // faxNumberLabel
            // 
            faxNumberLabel.AutoSize = true;
            faxNumberLabel.Location = new System.Drawing.Point(77, 135);
            faxNumberLabel.Name = "faxNumberLabel";
            faxNumberLabel.Size = new System.Drawing.Size(67, 13);
            faxNumberLabel.TabIndex = 8;
            faxNumberLabel.Text = "Fax Number:";
            // 
            // stateIdLabel
            // 
            stateIdLabel.AutoSize = true;
            stateIdLabel.Location = new System.Drawing.Point(77, 161);
            stateIdLabel.Name = "stateIdLabel";
            stateIdLabel.Size = new System.Drawing.Size(47, 13);
            stateIdLabel.TabIndex = 10;
            stateIdLabel.Text = "State Id:";
            // 
            // workNumberLabel
            // 
            workNumberLabel.AutoSize = true;
            workNumberLabel.Location = new System.Drawing.Point(77, 187);
            workNumberLabel.Name = "workNumberLabel";
            workNumberLabel.Size = new System.Drawing.Size(76, 13);
            workNumberLabel.TabIndex = 12;
            workNumberLabel.Text = "Work Number:";
            // 
            // zipLabel
            // 
            zipLabel.AutoSize = true;
            zipLabel.Location = new System.Drawing.Point(77, 213);
            zipLabel.Name = "zipLabel";
            zipLabel.Size = new System.Drawing.Size(25, 13);
            zipLabel.TabIndex = 14;
            zipLabel.Text = "Zip:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.DataSource = this.addressDetailsBindingSource;
            // 
            // address1TextBox
            // 
            this.address1TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.addressDetailsBindingSource, "Address1", true));
            this.address1TextBox.Location = new System.Drawing.Point(159, 28);
            this.address1TextBox.Name = "address1TextBox";
            this.address1TextBox.Size = new System.Drawing.Size(100, 20);
            this.address1TextBox.TabIndex = 1;
            // 
            // address2TextBox
            // 
            this.address2TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.addressDetailsBindingSource, "Address2", true));
            this.address2TextBox.Location = new System.Drawing.Point(159, 54);
            this.address2TextBox.Name = "address2TextBox";
            this.address2TextBox.Size = new System.Drawing.Size(100, 20);
            this.address2TextBox.TabIndex = 3;
            // 
            // countryIdTextBox
            // 
            this.countryIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.addressDetailsBindingSource, "CountryId", true));
            this.countryIdTextBox.Location = new System.Drawing.Point(159, 80);
            this.countryIdTextBox.Name = "countryIdTextBox";
            this.countryIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.countryIdTextBox.TabIndex = 5;
            // 
            // errorTextBox
            // 
            this.errorTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.addressDetailsBindingSource, "Error", true));
            this.errorTextBox.Location = new System.Drawing.Point(159, 106);
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.Size = new System.Drawing.Size(100, 20);
            this.errorTextBox.TabIndex = 7;
            // 
            // faxNumberTextBox
            // 
            this.faxNumberTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.addressDetailsBindingSource, "FaxNumber", true));
            this.faxNumberTextBox.Location = new System.Drawing.Point(159, 132);
            this.faxNumberTextBox.Name = "faxNumberTextBox";
            this.faxNumberTextBox.Size = new System.Drawing.Size(100, 20);
            this.faxNumberTextBox.TabIndex = 9;
            // 
            // stateIdTextBox
            // 
            this.stateIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.addressDetailsBindingSource, "StateId", true));
            this.stateIdTextBox.Location = new System.Drawing.Point(159, 158);
            this.stateIdTextBox.Name = "stateIdTextBox";
            this.stateIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.stateIdTextBox.TabIndex = 11;
            // 
            // workNumberTextBox
            // 
            this.workNumberTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.addressDetailsBindingSource, "WorkNumber", true));
            this.workNumberTextBox.Location = new System.Drawing.Point(159, 184);
            this.workNumberTextBox.Name = "workNumberTextBox";
            this.workNumberTextBox.Size = new System.Drawing.Size(100, 20);
            this.workNumberTextBox.TabIndex = 13;
            // 
            // zipTextBox
            // 
            this.zipTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.addressDetailsBindingSource, "Zip", true));
            this.zipTextBox.Location = new System.Drawing.Point(159, 210);
            this.zipTextBox.Name = "zipTextBox";
            this.zipTextBox.Size = new System.Drawing.Size(100, 20);
            this.zipTextBox.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(78, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // addressDetailsBindingSource
            // 
            this.addressDetailsBindingSource.DataSource = typeof(Prana.PM.BLL.AddressDetails);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(174, 269);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ErrorProviderCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(324, 340);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(address1Label);
            this.Controls.Add(this.address1TextBox);
            this.Controls.Add(address2Label);
            this.Controls.Add(this.address2TextBox);
            this.Controls.Add(countryIdLabel);
            this.Controls.Add(this.countryIdTextBox);
            this.Controls.Add(errorLabel);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(faxNumberLabel);
            this.Controls.Add(this.faxNumberTextBox);
            this.Controls.Add(stateIdLabel);
            this.Controls.Add(this.stateIdTextBox);
            this.Controls.Add(workNumberLabel);
            this.Controls.Add(this.workNumberTextBox);
            this.Controls.Add(zipLabel);
            this.Controls.Add(this.zipTextBox);
            this.Name = "ErrorProviderCheck";
            this.Text = "ErrorProviderCheck";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addressDetailsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.BindingSource addressDetailsBindingSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox address1TextBox;
        private System.Windows.Forms.TextBox address2TextBox;
        private System.Windows.Forms.TextBox countryIdTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.TextBox faxNumberTextBox;
        private System.Windows.Forms.TextBox stateIdTextBox;
        private System.Windows.Forms.TextBox workNumberTextBox;
        private System.Windows.Forms.TextBox zipTextBox;
        private System.Windows.Forms.Button button2;
    }
}