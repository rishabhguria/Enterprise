using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana
{
    public partial class CounterPartyDownForm : Form
    {
        object locker = new object();
        OrderCollection _queuedOrders = new OrderCollection();
        static CounterPartyDownForm _counterPartyDownForm = null;
        private CounterPartyDownForm()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnClose.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClose.ForeColor = System.Drawing.Color.White;
                btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClose.UseAppStyling = false;
                btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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
        public static CounterPartyDownForm GetInstance
        {
            get
            {
                if (_counterPartyDownForm == null)
                {
                    _counterPartyDownForm = new CounterPartyDownForm();
                }
                return _counterPartyDownForm;
            }
        }
        private void CounterPartyDownForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }
        public void SetUp()
        {
            try
            {
                grid.DataSource = null;
                grid.DataSource = _queuedOrders;
                grid.DataBind();
                ShowDefaultColumns();
                this.Visible = false;


            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void AddQueuedOrders(OrderCollection newOrders)
        {
            foreach (Order order in newOrders)
            {
                lock (locker)
                    _queuedOrders.Add(order);
            }
            if (!this.Visible)
                this.Visible = true;
        }
        public void AddQueuedOrder(Order newOrder)
        {
            lock (locker)
                _queuedOrders.Add(newOrder);
            if (!this.Visible)
                this.Visible = true;
        }
        public void RemoveOrder(Order newOrder)
        {
            try
            {
                Order cpDownorder = null;
                lock (locker)
                {
                    foreach (Order tempOrder in _queuedOrders)
                    {
                        if (tempOrder.ClOrderID == newOrder.ClOrderID)
                        {
                            cpDownorder = tempOrder;
                        }
                    }
                    if (cpDownorder != null)
                    {
                        _queuedOrders.Remove(cpDownorder);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }
        public OrderCollection QueuedOrders
        {
            get { return _queuedOrders; }
        }

        private void ShowDefaultColumns()
        {
            try
            {
                string[] defaultColumns = Enum.GetNames(typeof(OrderFields.CounterPartyUpGridGridColumns));
                ColumnsCollection columnCollection = grid.DisplayLayout.Bands[0].Columns;
                // int defaultColumnsLength = defaultColumns.Length;

                foreach (UltraGridColumn column in columnCollection)
                {
                    column.Hidden = true;
                }
                int i = 0;
                foreach (string columnName in defaultColumns)
                {
                    UltraGridColumn column = columnCollection[columnName];
                    column.Hidden = false;
                    column.Header.VisiblePosition = i;
                    i++;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;

            }



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }


    }
}