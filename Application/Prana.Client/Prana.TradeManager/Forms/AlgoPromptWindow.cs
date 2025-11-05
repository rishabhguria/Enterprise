using Prana.BusinessObjects;
using Prana.TradeManager.Extension;
using System;
using System.Windows.Forms;

namespace Prana.TradeManager
{
    public partial class AlgoPromptWindow : Form
    {
        private OrderSingle _algoFixOrder;

        public AlgoPromptWindow(string msg, string windowName, OrderSingle _algoOrder)
        {
            InitializeComponent();
            txtbxMessage.Text = msg;
            this.Text = windowName;
            _algoFixOrder = (OrderSingle)_algoOrder.Clone();
        }

        public AlgoPromptWindow(string msg, string windowName, bool enableContinueButton, bool enableEditButton, OrderSingle _algoOrder)
        {
            InitializeComponent();
            txtbxMessage.Text = msg;
            this.Name = windowName;
            _algoFixOrder = (OrderSingle)_algoOrder.Clone();
            btnContinue.Enabled = enableContinueButton;
            btnAbort.Enabled = enableEditButton;
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (_algoFixOrder != null)
            {
                AlgoReplaceManager.GetInstance().CancelAwaitingOrder(_algoFixOrder.ParentClOrderID);
            }
            this.Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (_algoFixOrder != null)
            {
                TradeManagerExtension.GetInstance().SendValidatedTrades(_algoFixOrder);
            }
            this.Close();
        }
    }
}