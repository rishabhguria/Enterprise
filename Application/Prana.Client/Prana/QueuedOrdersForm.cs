using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana
{
    public partial class QueuedOrdersForm : Form
    {
        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();
        OrderCollection _queuedOrders = new OrderCollection();
        OrderCollection _selectedOrders = new OrderCollection();
        ICommunicationManager _commManager = null;
        static QueuedOrdersForm _queuedOrdersForm = null;
        object locker = new object();
        private QueuedOrdersForm()
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
                btn_Send.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btn_Send.ForeColor = System.Drawing.Color.White;
                btn_Send.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btn_Send.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btn_Send.UseAppStyling = false;
                btn_Send.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCancel.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnCancel.ForeColor = System.Drawing.Color.White;
                btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClose.BackColor = System.Drawing.Color.FromArgb(55, 67, 46);
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


        public static QueuedOrdersForm GetInstance
        {
            get
            {
                if (_queuedOrdersForm == null)
                {
                    _queuedOrdersForm = new QueuedOrdersForm();
                }
                return _queuedOrdersForm;
            }
        }

        public void SetUp(ICommunicationManager commManager)
        {
            try
            {
                _commManager = commManager;
                //grid.DataSource = null;
                grid.DataSource = _queuedOrders;
                grid.DataBind();
                this.Visible = false;
                headerCheckBox._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBox__CLICKED);
                AddCheckBoxinGrid(grid, headerCheckBox);
                ShowDefaultColumns();
                grid.UpdateMode = UpdateMode.OnCellChange;


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

        void headerCheckBox__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {
            try
            {
                _selectedOrders = new OrderCollection();
                if (e.CurrentCheckState == CheckState.Checked)
                {

                    foreach (Order order in _queuedOrders)
                    {
                        _selectedOrders.Add(order);
                    }
                }
                else
                {
                    // do nothing
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }


        }
        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            try
            {
                grid.CreationFilter = headerCheckBox;
                grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "");
                grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);
                grid.DisplayLayout.Bands[0].Columns["checkBox"].CellClickAction = CellClickAction.Edit;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                if (_commManager.ConnectionStatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    BackgroundWorker bgWorkerForSending = new BackgroundWorker();
                    bgWorkerForSending.DoWork += new DoWorkEventHandler(bgWorkerForSending_DoWork);
                    bgWorkerForSending.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerForSending_RunWorkerCompleted);
                    foreach (Order order in _selectedOrders)
                    {
                        lock (locker)
                        {
                            _queuedOrders.Remove(order);
                        }
                    }
                    bgWorkerForSending.RunWorkerAsync(_selectedOrders);
                }
                else
                {
                    MessageBox.Show("Server not connected");
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

        void bgWorkerForSending_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
            {
                MessageBox.Show("All the Messages are sent to Counter parties");
            }
            else
            {

            }
        }

        void bgWorkerForSending_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //foreach (Order order in _selectedOrders)
                //{
                //    //string msg = CustomFIXConstants.MSG_CounterPartyUp + Prana.BusinessObjects.Seperators.SEPERATOR_1 + order.ClOrderID + Prana.BusinessObjects.Seperators.SEPERATOR_1 + "1";

                //    Prana.BusinessObjects.FIX.PranaMessage msg = Prana.Fix.FixDictionary.Transformer.CreatePranaMessageThroughReflection(order);
                //    //msg.MessageType = CustomFIXConstants.MSG_CounterPartyUpQueuedMsg;                    
                //    //QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_CounterPartyUpQueuedMsg, msg);
                //    //_commManager.SendMessage(qMsg);
                //    //_commManager.SendMessage(msg);

                //}
                _selectedOrders = new OrderCollection();
                e.Result = true;
            }
            catch (Exception ex)
            {
                e.Result = false;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
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
                columnCollection["checkBox"].Hidden = false;
                columnCollection["checkBox"].Header.VisiblePosition = i;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void QueuedOrdersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void grid_MouseUp(object sender, MouseEventArgs e)
        {
            grid.Update();
            //grid.UpdateData();
            //grid.UpdateMode = UpdateMode.OnCellChange;
            bool isRowSelected = false;
            if (e.Button.ToString() == "Right")
                return;
            UIElement objUIElement;
            UltraGridCell objUltraGridCell;
            objUIElement = grid.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
            if (objUIElement == null)
                return;
            objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
            if (objUltraGridCell == null)
                return;
            if ((objUltraGridCell.Value.ToString() == "True" || objUltraGridCell.Value.ToString() == "False"))
            {
                if (objUltraGridCell.Row.Cells["checkBox"].Value.ToString().ToUpper() == "TRUE")
                {
                    //objUltraGridCell.Row.Cells["checkBox"].Value=false;
                    isRowSelected = false;

                }
                else
                {
                    //objUltraGridCell.Row.Cells["checkBox"].Value=true;
                    isRowSelected = true;

                }

            }
            else
                return;
            Order order = (Order)objUltraGridCell.Row.ListObject;
            if (isRowSelected)
            {
                if (!_selectedOrders.Contains(order))
                    _selectedOrders.Add(order);

            }
            else
            {
                if (_selectedOrders.Contains(order))
                    _selectedOrders.Remove(order);

            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                BackgroundWorker bgWorkerForCanceling = new BackgroundWorker();
                foreach (Order order in _selectedOrders)
                {
                    // _commManager.SendMessage(msg);
                    lock (locker)
                    {
                        _queuedOrders.Remove(order);
                    }
                }
                bgWorkerForCanceling.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerForCanceling_RunWorkerCompleted);
                bgWorkerForCanceling.DoWork += new DoWorkEventHandler(bgWorkerForCanceling_DoWork);

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

        void bgWorkerForCanceling_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //foreach (Order order in _selectedOrders)
                //{
                //    string msg = CustomFIXConstants.MSG_CounterPartyUp + Seperators.SEPERATOR_1 + order.ClOrderID + Seperators.SEPERATOR_1 + "0";
                //    //_commManager.SendMessage(msg);
                //}
                _selectedOrders = new OrderCollection();
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

        void bgWorkerForCanceling_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
            {
                MessageBox.Show("All the Cancellation Messages are sent to Counter parties");
            }
            else
            {

            }
        }






    }
}