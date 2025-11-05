using System;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Forms
{
    public partial class ScrollabletPopUpMessage : Form
    {
        public ScrollabletPopUpMessage()
        {
            InitializeComponent();
        }

        private string _title;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _messageDescription;

        /// <summary>
        /// Gets or sets the message description.
        /// </summary>
        /// <value>The message description.</value>
        public string MessageDescription
        {
            get { return _messageDescription; }
            set { _messageDescription = value; }
        }


        private void ScrollabletPopUpMessage_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Text = this._title;
            richTextBoxMessage.Text = this._messageDescription;
        }
    }
}