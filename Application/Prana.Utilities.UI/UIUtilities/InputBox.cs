using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    /// <summary>
    /// Class to implement the input box, used to take input for general usage.
    /// </summary>
    public class InputBox : System.Windows.Forms.Form
    {
        private Infragistics.Win.UltraWinEditors.UltraTextEditor textBox1;
        private Infragistics.Win.Misc.UltraButton btnOK;
        private Infragistics.Win.Misc.UltraButton btnOKServer;
        bool _formClosed = false;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel InputBox_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _InputBox_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _InputBox_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _InputBox_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _InputBox_UltraFormManager_Dock_Area_Bottom;
        private IContainer components;

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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (textBox1 != null)
                {
                    textBox1.Dispose();
                }

                if (btnOK != null)
                {
                    btnOK.Dispose();
                }

                if (btnOKServer != null)
                {
                    btnOKServer.Dispose();
                }

                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }

                if (InputBox_Fill_Panel != null)
                {
                    InputBox_Fill_Panel.Dispose();
                }

                if (_InputBox_UltraFormManager_Dock_Area_Left != null)
                {
                    _InputBox_UltraFormManager_Dock_Area_Left.Dispose();
                }

                if (_InputBox_UltraFormManager_Dock_Area_Right != null)
                {
                    _InputBox_UltraFormManager_Dock_Area_Right.Dispose();
                }

                if (_InputBox_UltraFormManager_Dock_Area_Top != null)
                {
                    _InputBox_UltraFormManager_Dock_Area_Top.Dispose();
                }

                if (_InputBox_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _InputBox_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
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
            this.textBox1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnOK = new Infragistics.Win.Misc.UltraButton();
            this.btnOKServer = new Infragistics.Win.Misc.UltraButton();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.InputBox_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._InputBox_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._InputBox_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._InputBox_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._InputBox_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.textBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.InputBox_Fill_Panel.ClientArea.SuspendLayout();
            this.InputBox_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox1.Location = new System.Drawing.Point(29, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(152, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // btnOK
            // 
            this.btnOK.BackColorInternal = System.Drawing.SystemColors.Control;
            this.btnOK.Location = new System.Drawing.Point(69, 48);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnOKServer
            // 
            btnOKServer.BackColorInternal = System.Drawing.SystemColors.Control;
            btnOKServer.Location = new System.Drawing.Point(69, 48);
            btnOKServer.Name = "btnOKServer";
            btnOKServer.Size = new System.Drawing.Size(75, 23);
            btnOKServer.TabIndex = 1;
            btnOKServer.Text = "&OK";
            btnOKServer.Click += new System.EventHandler(btnOKServer_Click);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // InputBox_Fill_Panel
            // 
            // 
            // InputBox_Fill_Panel.ClientArea
            // 
            this.InputBox_Fill_Panel.ClientArea.Controls.Add(this.btnOK);
            this.InputBox_Fill_Panel.ClientArea.Controls.Add(this.btnOKServer);
            this.InputBox_Fill_Panel.ClientArea.Controls.Add(this.textBox1);
            this.InputBox_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.InputBox_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputBox_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.InputBox_Fill_Panel.Name = "InputBox_Fill_Panel";
            this.InputBox_Fill_Panel.Size = new System.Drawing.Size(205, 82);
            this.InputBox_Fill_Panel.TabIndex = 0;
            // 
            // _InputBox_UltraFormManager_Dock_Area_Left
            // 
            this._InputBox_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._InputBox_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._InputBox_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._InputBox_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._InputBox_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._InputBox_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._InputBox_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._InputBox_UltraFormManager_Dock_Area_Left.Name = "_InputBox_UltraFormManager_Dock_Area_Left";
            this._InputBox_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 82);
            // 
            // _InputBox_UltraFormManager_Dock_Area_Right
            // 
            this._InputBox_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._InputBox_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._InputBox_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._InputBox_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._InputBox_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._InputBox_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._InputBox_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(209, 27);
            this._InputBox_UltraFormManager_Dock_Area_Right.Name = "_InputBox_UltraFormManager_Dock_Area_Right";
            this._InputBox_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 82);
            // 
            // _InputBox_UltraFormManager_Dock_Area_Top
            // 
            this._InputBox_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._InputBox_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._InputBox_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._InputBox_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._InputBox_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._InputBox_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._InputBox_UltraFormManager_Dock_Area_Top.Name = "_InputBox_UltraFormManager_Dock_Area_Top";
            this._InputBox_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(213, 27);
            // 
            // _InputBox_UltraFormManager_Dock_Area_Bottom
            // 
            this._InputBox_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._InputBox_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._InputBox_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._InputBox_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._InputBox_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._InputBox_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._InputBox_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 109);
            this._InputBox_UltraFormManager_Dock_Area_Bottom.Name = "_InputBox_UltraFormManager_Dock_Area_Bottom";
            this._InputBox_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(213, 4);
            // 
            // InputBox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(213, 113);
            this.Controls.Add(this.InputBox_Fill_Panel);
            this.Controls.Add(this._InputBox_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._InputBox_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._InputBox_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._InputBox_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InputBox_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.textBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.InputBox_Fill_Panel.ClientArea.ResumeLayout(false);
            this.InputBox_Fill_Panel.ClientArea.PerformLayout();
            this.InputBox_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Handle the Enter key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (box.textBox1.Text != string.Empty)
                {
                    _formClosed = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please enter the name in the textbox", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _formClosed = false;
                }
            }
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
            box.Text = header;
            SetTheme();
            box.ShowDialog();
            string output = box.textBox1.Text;
            box.Dispose();
            box = null;
            return output;
        }

        /// <summary>
        /// Shows the input box.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="letterCase">The letter case for input.</param>
        /// <param name="textBoxText">The input text box text.</param>
        /// <returns></returns>
        public static string ShowInputBox(string header, CharacterCasing letterCase, string textBoxText = "")
        {
            try
            {
                box = new InputBox();
                box.Text = header;
                box.textBox1.CharacterCasing = letterCase;
                box.textBox1.Text = textBoxText;
                box.textBox1.SelectAll();
                SetTheme();
                box.ShowDialog();
                string output = box.textBox1.Text;
                box.Dispose();
                box = null;
                return output;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return string.Empty;
            }
        }

        public static string ShowInputBox(string header, out DialogResult result)
        {
            box = new InputBox();
            box.btnOK.Visible = false;
            box.btnOKServer.Visible = true;
            box.Text = header;
            result = box.ShowDialog();
            if (_textBoxText != string.Empty)
            {
                result = DialogResult.OK;
            }
            return _textBoxText;
        }

        static string _textBoxText = string.Empty;
        private static void btnOKServer_Click(object sender, EventArgs e)
        {
            if (box.textBox1.Text.Equals(string.Empty))
            {
                MessageBox.Show("Please enter the OrderID in the textbox", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                box._formClosed = false;
            }
            else
            {
                _textBoxText = box.textBox1.Text;
                box._formClosed = true;
                box.Close();
            }
        }

        private static void SetTheme()
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(box, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
                box.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + box.Text + "</p>";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static string ShowInputBox(string header, string prefilledText, out DialogResult result)
        {
            box = new InputBox();
            box.Text = header;
            SetTheme();
            box.textBox1.CharacterCasing = CharacterCasing.Normal;
            box.textBox1.Text = prefilledText;
            result = box.ShowDialog();
            if (box._formClosed == true) result = DialogResult.OK;
            string output = box.textBox1.Text;
            box.Dispose();
            box = null;
            return output;
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if (box.textBox1.Text.Equals(string.Empty))
            {
                MessageBox.Show("Please enter the name in the textbox", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _formClosed = false;
            }
            else
            {
                _formClosed = true;
                this.Close();
            }
        }

        private void InputBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_formClosed == false)
            {
                box.textBox1.Text = string.Empty;
                _textBoxText = string.Empty;
            }
        }
    }
}
