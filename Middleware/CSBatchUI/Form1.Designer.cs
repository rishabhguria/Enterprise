namespace CSBatchUI
{
    partial class Form1
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnRunBatch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.btnCancelBatch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.nCores = new System.Windows.Forms.NumericUpDown();
            this.nStep = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.txtBatch = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nCores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nStep)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(111, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "To";
            // 
            // btnRunBatch
            // 
            this.btnRunBatch.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnRunBatch.Location = new System.Drawing.Point(193, 307);
            this.btnRunBatch.Name = "btnRunBatch";
            this.btnRunBatch.Size = new System.Drawing.Size(75, 23);
            this.btnRunBatch.TabIndex = 6;
            this.btnRunBatch.Text = "Run";
            this.btnRunBatch.UseVisualStyleBackColor = true;
            this.btnRunBatch.Click += new System.EventHandler(this.btnRunBatch_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBatch);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nStep);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nCores);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtFrom);
            this.groupBox1.Controls.Add(this.dtTo);
            this.groupBox1.Location = new System.Drawing.Point(16, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(252, 253);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Run Batch";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(111, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "From";
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(30, 42);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(200, 20);
            this.dtFrom.TabIndex = 0;
            // 
            // dtTo
            // 
            this.dtTo.Location = new System.Drawing.Point(30, 106);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(200, 20);
            this.dtTo.TabIndex = 1;
            // 
            // btnCancelBatch
            // 
            this.btnCancelBatch.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelBatch.Location = new System.Drawing.Point(112, 307);
            this.btnCancelBatch.Name = "btnCancelBatch";
            this.btnCancelBatch.Size = new System.Drawing.Size(75, 23);
            this.btnCancelBatch.TabIndex = 7;
            this.btnCancelBatch.Text = "Cancel";
            this.btnCancelBatch.UseVisualStyleBackColor = true;
            this.btnCancelBatch.Click += new System.EventHandler(this.btnCancelBatch_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "# of CPU\'s";
            // 
            // nCores
            // 
            this.nCores.Location = new System.Drawing.Point(157, 149);
            this.nCores.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nCores.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nCores.Name = "nCores";
            this.nCores.Size = new System.Drawing.Size(72, 20);
            this.nCores.TabIndex = 5;
            this.toolTip1.SetToolTip(this.nCores, "Number of Parallel Thread to run on. Using more than the number of CPU\'s is not r" +
        "ecommended.");
            this.nCores.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nStep
            // 
            this.nStep.Location = new System.Drawing.Point(158, 175);
            this.nStep.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nStep.Name = "nStep";
            this.nStep.Size = new System.Drawing.Size(72, 20);
            this.nStep.TabIndex = 7;
            this.toolTip1.SetToolTip(this.nStep, "Step 1 Runs GenericPNL and Transactions. Step 2:  Only Runs Derived Data and Dail" +
        "y Returns");
            this.nStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(28, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Step";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(28, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Create Batch File ( Name Only)";
            this.toolTip1.SetToolTip(this.label5, "Creates a Batch File based on the above settings");
            // 
            // txtBatch
            // 
            this.txtBatch.Location = new System.Drawing.Point(31, 227);
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Size = new System.Drawing.Size(198, 20);
            this.txtBatch.TabIndex = 9;
            // 
            // btnCreate
            // 
            this.btnCreate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCreate.Location = new System.Drawing.Point(16, 307);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 8;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 362);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnRunBatch);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancelBatch);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Batch Execution";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nCores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nStep)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRunBatch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DateTimePicker dtFrom;
        public System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Button btnCancelBatch;
        private System.Windows.Forms.TextBox txtBatch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown nStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nCores;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCreate;
    }
}

