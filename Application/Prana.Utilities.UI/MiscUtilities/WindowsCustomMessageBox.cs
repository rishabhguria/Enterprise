using Infragistics.Win.Misc;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace Prana.Utilities.UI.MiscUtilities
{
    public partial class WindowsCustomMessageBox : Form
    {
        private static DialogResult _buttonResult = new DialogResult();
        private static WindowsCustomMessageBox _msgBox;
        public WindowsCustomMessageBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="Yes"></param>
        /// <param name="No"></param>
        /// <param name="Cancel"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult Show(string message, string title, Buttons buttons, string Yes = "Yes", string No = "No", string Cancel = "Cancel", Icon icon = Icon.Info)
        {
            _msgBox = new WindowsCustomMessageBox();
            InitMessage(message, title);
            InitIcon(icon);
            InitButton(buttons, Yes, No, Cancel);
            _msgBox.ShowDialog();
            return _buttonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="Yes"></param>
        /// <param name="No"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult Show(string message, string title, Buttons buttons, string Yes = "Yes", string No = "No", Icon icon = Icon.Info)
        {
            _msgBox = new WindowsCustomMessageBox();
            InitMessage(message, title);
            InitIcon(icon);
            InitButton(buttons, "Text1", Yes, No);
            _msgBox.ShowDialog();
            return _buttonResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="Abort"></param>
        /// <param name="Retry"></param>
        /// <param name="Ignore"></param>
        /// <returns></returns>
        public static DialogResult Show(string message, string title, Buttons buttons, string Abort = "Abort", string Retry = "Retry", string Ignore = "Ignore")
        {
            _msgBox = new WindowsCustomMessageBox();
            InitMessage(message, title);
            InitIcon(Icon.Exclamation);
            InitButton(buttons, Abort, Retry, Ignore);
            _msgBox.ShowDialog();
            return _buttonResult;
        }

        private static void InitMessage(string message, string title)
        {
            _msgBox.lblMessage.Text = message;
            _msgBox.Text = title;
            _msgBox.AutoSize = true;
            _msgBox.lblMessage.ForeColor = Color.Black;
            _msgBox.lblMessage.Font = new System.Drawing.Font("Arial", 10);
            _msgBox.lblMessage.BackColor = Color.Transparent;
            _msgBox.lblMessage.Appearance.TextVAlign = Infragistics.Win.VAlign.Middle;
        }


        private static void ButtonClick(object sender, EventArgs e)
        {
            UltraButton btn = (UltraButton)sender;
            _buttonResult = btn.DialogResult;
            _msgBox.Dispose();
        }


        private static void InitIcon(Icon icon)
        {
            switch (icon)
            {
                case WindowsCustomMessageBox.Icon.Application:
                    _msgBox.pictureBox1.Image = SystemIcons.Application.ToBitmap();
                    break;

                case WindowsCustomMessageBox.Icon.Exclamation:
                    _msgBox.pictureBox1.Image = SystemIcons.Exclamation.ToBitmap();
                    break;

                case WindowsCustomMessageBox.Icon.Error:
                    _msgBox.pictureBox1.Image = SystemIcons.Error.ToBitmap();
                    break;

                case WindowsCustomMessageBox.Icon.Info:
                    _msgBox.pictureBox1.Image = SystemIcons.Information.ToBitmap();
                    break;

                case WindowsCustomMessageBox.Icon.Question:
                    _msgBox.pictureBox1.Image = SystemIcons.Question.ToBitmap();
                    break;

                case WindowsCustomMessageBox.Icon.Shield:
                    _msgBox.pictureBox1.Image = SystemIcons.Shield.ToBitmap();
                    break;

                case WindowsCustomMessageBox.Icon.Warning:
                    _msgBox.pictureBox1.Image = SystemIcons.Warning.ToBitmap();
                    break;
            }

        }


        private static void InitButton(Buttons buttonType, string button1text = "Text1", string button2text = "Text2", string buuton3text = "Text3")
        {
            _msgBox.ultraButton1.Text = button1text;
            _msgBox.ultraButton1.AutoSize = true;
            _msgBox.ultraButton1.Click += ButtonClick;
            _msgBox.ultraButton1.Location = new System.Drawing.Point(39, 99);

            _msgBox.ultraButton2.Text = button2text;
            _msgBox.ultraButton2.AutoSize = true;
            _msgBox.ultraButton2.Click += ButtonClick;
            _msgBox.ultraButton2.Location = new System.Drawing.Point(_msgBox.ultraButton1.Location.X + _msgBox.ultraButton1.Width + 20, 99);

            _msgBox.ultraButton3.Text = buuton3text;
            _msgBox.ultraButton3.AutoSize = true;
            _msgBox.ultraButton3.Click += ButtonClick;
            _msgBox.ultraButton3.Location = new System.Drawing.Point(_msgBox.ultraButton2.Location.X + _msgBox.ultraButton2.Width + 20, 99);

            //To do: implement rest button type
            switch (buttonType)
            {
                case Buttons.YesNoCancel:
                    _msgBox.ultraButton1.Visible = true;
                    _msgBox.ultraButton1.DialogResult = System.Windows.Forms.DialogResult.Yes;

                    _msgBox.ultraButton2.Visible = true;
                    _msgBox.ultraButton2.DialogResult = System.Windows.Forms.DialogResult.No;

                    _msgBox.ultraButton3.Visible = true;
                    _msgBox.ultraButton3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
                case Buttons.YesNo:
                    _msgBox.ultraButton1.Visible = false;

                    _msgBox.ultraButton2.Visible = true;
                    _msgBox.ultraButton2.DialogResult = System.Windows.Forms.DialogResult.Yes;

                    _msgBox.ultraButton3.Visible = true;
                    _msgBox.ultraButton3.DialogResult = System.Windows.Forms.DialogResult.No;
                    break;
                case Buttons.AbortRetryIgnore:
                    _msgBox.ultraButton1.Visible = true;
                    _msgBox.ultraButton1.DialogResult = System.Windows.Forms.DialogResult.Abort;

                    _msgBox.ultraButton2.Visible = true;
                    _msgBox.ultraButton2.DialogResult = System.Windows.Forms.DialogResult.Retry;

                    _msgBox.ultraButton3.Visible = true;
                    _msgBox.ultraButton3.DialogResult = System.Windows.Forms.DialogResult.Ignore;
                    break;
            }
        }

        public enum Buttons
        {
            AbortRetryIgnore = 1,
            OK = 2,
            OKCancel = 3,
            RetryCancel = 4,
            YesNo = 5,
            YesNoCancel = 6
        }

        public new enum Icon
        {
            Application = 1,
            Exclamation = 2,
            Error = 3,
            Warning = 4,
            Info = 5,
            Question = 6,
            Shield = 7,
            Search = 8
        }
    }
}
