using System;
using System.Windows.Forms;

namespace Prana.AdminForms
{
    public partial class NewYear : Form
    {
        private static NewYear _newYear = null;
        private static object locker = new object();
        Prana.AdminForms.CalendarHolidays _savedcholidays = null;

        public NewYear()
        {
            InitializeComponent();
        }

        public static NewYear GetInstance()
        {
            lock (locker)
            {
                if (_newYear == null)
                {
                    _newYear = new NewYear();
                }
                return _newYear;
            }
        }

        private void NewYear_FormClosed(object sender, FormClosedEventArgs e)
        {
            _newYear = null;
        }

        private void NewYear_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CalendarHolidays cholidays = new CalendarHolidays();
            cholidays.Year = dtYearPicker.Value.Year;
            _savedcholidays = cholidays;
            this.Hide();
        }

        public Prana.AdminForms.CalendarHolidays SaveYear
        {
            get { return _savedcholidays; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}