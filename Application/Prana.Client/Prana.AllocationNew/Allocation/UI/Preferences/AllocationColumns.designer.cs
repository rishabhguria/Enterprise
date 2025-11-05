namespace Prana.AllocationNew
{
    partial class AllocationColumns
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
            if (uctAllocatedColumns != null)
            {
                uctAllocatedColumns.Dispose();
                uctAllocatedColumns = null;
            }
            if (uctUnAllocatedColumn != null)
            {
                uctUnAllocatedColumn.Dispose();
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
            this.label7 = new System.Windows.Forms.Label();
            this.lbGridType = new System.Windows.Forms.ListBox();
            this.uctAllocatedColumns = new Prana.AllocationNew.ColumnsUserControl();
            this.uctUnAllocatedColumn = new Prana.AllocationNew.ColumnsUserControl();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label7.Location = new System.Drawing.Point(24, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 16);
            this.label7.TabIndex = 60;
            this.label7.Text = "Blotters";
            // 
            // lbGridType
            // 
            this.lbGridType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lbGridType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbGridType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbGridType.Location = new System.Drawing.Point(5, 63);
            this.lbGridType.Name = "lbGridType";
            this.lbGridType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbGridType.Size = new System.Drawing.Size(105, 184);
            this.lbGridType.TabIndex = 58;
            this.lbGridType.SelectedIndexChanged += new System.EventHandler(this.lbGridType_SelectedIndexChanged);
            
            // uctAllocatedColumns
            // 
            this.uctAllocatedColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.uctAllocatedColumns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctAllocatedColumns.Location = new System.Drawing.Point(114, 16);
            this.uctAllocatedColumns.Name = "uctAllocatedColumns";
            this.uctAllocatedColumns.Size = new System.Drawing.Size(342, 263);
            this.uctAllocatedColumns.TabIndex = 62;
            // 
            // uctUnAllocatedColumn
            // 
            this.uctUnAllocatedColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.uctUnAllocatedColumn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctUnAllocatedColumn.Location = new System.Drawing.Point(114, 16);
            this.uctUnAllocatedColumn.Name = "uctUnAllocatedColumn";
            this.uctUnAllocatedColumn.Size = new System.Drawing.Size(342, 263);
            this.uctUnAllocatedColumn.TabIndex = 59;
            // 
            // AllocationColumns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.uctAllocatedColumns);
            this.Controls.Add(this.uctUnAllocatedColumn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbGridType);
            this.Name = "AllocationColumns";
            this.Size = new System.Drawing.Size(454, 289);
            this.ResumeLayout(false);

        }

        #endregion

        private ColumnsUserControl uctUnAllocatedColumn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox lbGridType;
        private ColumnsUserControl uctAllocatedColumns;
    }
}
