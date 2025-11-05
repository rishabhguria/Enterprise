using Prana.AlgoStrategyControls;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.TradingTicket.Forms
{
    public partial class AlgoControlPopUp : Form
    {

        public AlgoControlPopUp()
        {
            InitializeComponent();

            if (CustomThemeHelper.ApplyTheme)
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                this.BackColor = System.Drawing.Color.FromArgb(209, 210, 212);
            }

        }

        public EventHandler<AlgoStrategyControl> OnOkevent;
        public EventHandler<Dictionary<string, string>> OnOkEvent1;
        public bool IsBulkMTT { get; set; }
        #region fields
        private string broker = string.Empty;
        int auecID;
        private string underlyingText = string.Empty;
        private Dictionary<string, string> tagValueDictionary = new Dictionary<string, string>();
        private Dictionary<string, string> algoPropertiesDictionary;
        private Dictionary<string, string> algoSelectedValues;
        string parent = null;

        #endregion

        /// <summary>
        /// Binds the datasource to strategyControl2
        /// </summary>
        /// <param name="CounterPartyId"></param>
        /// <param name="UnderlyingText"></param>
        /// <param name="defaultAlgoType"></param>
        /// <param name="brokerValue"></param>
        public void Bind(string CounterPartyId, string UnderlyingText, int defaultAlgoType, string brokerValue, Dictionary<string, string> algoSelectedCtrlsValues, int auecId, string algoParent = null)
        {
            underlyingText = UnderlyingText;
            broker = brokerValue;
            auecID = auecId;
            algoSelectedValues = algoSelectedCtrlsValues;
            parent = algoParent;
            strategyControl2.StrategyValueChanged += strategyControl2_StrategyValueChanged;
            algoStrategyControl1.ClickedSendButtonEvent += algoStrategyControl_OkButtonClicked;
            strategyControl2.SetStrategies(CounterPartyId, UnderlyingText);
            strategyControl2.SetStrategyID(defaultAlgoType.ToString());
            if (!string.IsNullOrEmpty(parent))
            {
                btnOk.Visible = true;
            }
        }
        /// <summary>
        /// Hits the event when ok button on the form is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void algoStrategyControl_OkButtonClicked(object sender, EventArgs<string> e)
        {
            try
            {
                algoPropertiesDictionary = new Dictionary<string, string>();
                tagValueDictionary = algoStrategyControl1.GetSelectedStrategyFixTagValues();
                string error = string.Empty;
                error = algoStrategyControl1.ValidateAlgoStrategy(null, tagValueDictionary);
                if (error != string.Empty)
                {
                    MessageBox.Show(this, error, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    algoPropertiesDictionary = tagValueDictionary;
                    if (OnOkevent != null)
                    {
                        OnOkevent(this, algoStrategyControl1);
                    }
                    if (OnOkEvent1 != null)
                    {
                        OnOkEvent1(this, algoPropertiesDictionary);
                    }
                    this.Close();
                }


            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Hits the event when the strategy value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void strategyControl2_StrategyValueChanged(object sender, EventArgs<string> e)
        {
            try
            {
                AlgoStrategyChanged(e.Value, strategyControl2.GetStrategyName());
                AdjustAlgoControlPopUpSizeDynamically(strategyControl2.GetStrategyName());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Adjust the pop up size accordingly
        /// </summary>
        /// <param name="strategyName"></param>
        private void AdjustAlgoControlPopUpSizeDynamically(string strategyName)
        {
            if (strategyName != "Algo Type" && string.IsNullOrEmpty(parent))
            {
                this.Width = algoStrategyControl1.MaxPanelWidth + 20;
                this.Height = 310;
                int algoPanelHeight = algoStrategyControl1.MaxPanelHeight;

                if ((algoPanelHeight - 120) >= 0)
                {
                    this.Height = (this.Height + (algoPanelHeight - 120)) + 60;
                }
                algoStrategyControl1.MaxPanelHeight = 120;
                algoStrategyControl1.MaxPanelWidth = 400;
                if (!string.IsNullOrEmpty(algoStrategyControl1.CustomMessage))
                {
                    this.lblAlgoMessage.Location = new Point(8, this.Height - (lblAlgoMessage.Size.Height + 7));
                    this.lblAlgoMessage.Size = new Size(this.Width - 17, this.lblAlgoMessage.Height);
                    lblAlgoMessage.Visible = true;
                    lblAlgoMessage.Text = algoStrategyControl1.CustomMessage;
                }
                else if (string.IsNullOrEmpty(algoStrategyControl1.CustomMessage))
                {
                    lblAlgoMessage.Visible = false;
                }
                algoStrategyControl1.Show();
            }
            else
            {
                this.Height = 200;
                this.Width = 250;
                lblAlgoMessage.Visible = false;
            }
        }

        private void BtnOk_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (OnOkevent != null)
                {
                    OnOkevent(this, algoStrategyControl1);
                }
                if (IsBulkMTT)
                    this.Close();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Algoes the strategy changed.
        /// </summary>
        /// <param name="strategyID">The strategy identifier.</param>
        internal void AlgoStrategyChanged(string strategyID, string strategyName)
        {
            try
            {
                algoStrategyControl1.SetStrategyNameAndId(strategyID, strategyName);
                if (string.IsNullOrEmpty(parent))
                {
                    if (strategyID != int.MinValue.ToString())
                    {
                        algoStrategyControl1.Hide();
                        algoStrategyControl1.BackColor = this.BackColor;
                        algoStrategyControl1.CreateAlgoControls(broker, strategyID, underlyingText, true, true);
                    }
                    else if (strategyID == int.MinValue.ToString())
                    {
                        algoStrategyControl1.Hide();
                        algoStrategyControl1.CustomMessage = string.Empty;

                    }
                    if (algoSelectedValues != null)
                    {
                        algoStrategyControl1.SetSelectedStrategyFixTagValues(null, algoSelectedValues, auecID);
                        algoSelectedValues = null;
                    }
                    OrderSingle orderAlgoDetails = new OrderSingle { AUECID = auecID };
                    if (auecID != int.MinValue)
                        algoStrategyControl1.SetAlgoDetails(orderAlgoDetails);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
