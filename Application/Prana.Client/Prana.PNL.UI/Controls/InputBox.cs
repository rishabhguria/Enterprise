using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.PNL.UI.Controls
{
	/// <summary>
	/// Class to implement the input box, used to take various inputs in WatchList
	/// </summary>
	public class InputBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InputBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(29, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(152, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.Control;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(69, 48);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // InputBox
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(210, 82);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Handle the Enter key
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void textBox1_KeyDown(object sender,System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
				this.Close();
		}

        static InputBox box = null;
		/// <summary>
		/// Displays the input box and returns it's value
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static string ShowInputBox(string header)
		{
			box = new InputBox();
			box.Text= header;
			box.ShowDialog();
			return box.textBox1.Text;
			
		}

        public static string ShowInputBox(string header, string prefilledText)
        {
            box = new InputBox();
            box.Text = header;
            box.textBox1.Text = prefilledText;
            box.ShowDialog();
            return box.textBox1.Text;

        }

        public static string ShowInputBox(string header, string prefilledText,out DialogResult result)
        {
            box = new InputBox();
            box.Text = header;
            box.textBox1.Text = prefilledText;
            result = box.ShowDialog();
            return box.textBox1.Text;
        }

        public static string ShowInputBox(string header, out DialogResult result)
        {
            box = new InputBox();
            box.Text = header;
            result = box.ShowDialog();
            return box.textBox1.Text;

        }

		private void btnOK_Click(object sender, System.EventArgs e)
		{
            if (box.textBox1.Text.Equals(string.Empty))
                MessageBox.Show("Please enter the name in the textbox", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
			    this.Close();
		}   
		
	}
}
