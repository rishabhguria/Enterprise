using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prana.TradingTicket
{
    public partial class StrategyControl : UserControl
    {
        public StrategyControl()
        {
            InitializeComponent();

        }
        public delegate void StrategyChanged(Object sender, EventArgs<String> e);
        public event StrategyChanged StrategyValueChanged;
        /// <summary>
        /// Set Strategies
        /// </summary>
        /// <param name="cpID"></param>
        /// <param name="underlyingText"></param>
        public void SetStrategies(string cpID, string underlyingText)
        {
            try
            {
                DataTable dt = AlgoStrategyControls.AlgoControlsDictionary.GetInstance().GetAllowedStrategies(cpID, underlyingText);
                string logoName = AlgoStrategyControls.AlgoControlsDictionary.GetInstance().GetCounterPartyLogoName(cpID);
                cmbbxStrategy.DataSource = null;
                cmbbxStrategy.DataSource = dt;
                cmbbxStrategy.DisplayMember = "StrategyName";
                cmbbxStrategy.ValueMember = "StrategyID";
                cmbbxStrategy.DataBind();
                cmbbxStrategy.DisplayLayout.Bands[0].Columns["StrategyID"].Hidden = true;
                this.cmbbxStrategy.Rows.Band.ColHeadersVisible = false;

                if (File.Exists(System.Windows.Forms.Application.StartupPath + @"\Themes\AlgoIcons\" + logoName))
                    pictureBox1.Image = Image.FromFile(System.Windows.Forms.Application.StartupPath + @"\Themes\AlgoIcons\" + logoName);
                else if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }


                foreach (Button previousButton in hotButtonCollection.Values)
                {
                    previousButton.Visible = false;
                }
                hotButtonCollection.Clear();
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

        private void cmbbxStrategy_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbbxStrategy.Value != null)
                {
                    if (cmbbxStrategy.Value.ToString() == int.MinValue.ToString())
                    {

                    }

                    if (StrategyValueChanged != null)
                    {
                        StrategyValueChanged(this, new EventArgs<string>(cmbbxStrategy.Value.ToString()));
                    }
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

        Hashtable hotButtonCollection = new Hashtable();

        public void SetStrategyID(string strategyID)
        {
            cmbbxStrategy.Value = strategyID;
        }

        public string GetStrategyName()
        {
            return cmbbxStrategy.Text;
        }

        public void DisableControls()
        {
            cmbbxStrategy.Enabled = false;
        }
        public void EnableControls()
        {
            cmbbxStrategy.Enabled = true;
        }


        /// <summary>
        /// Re set
        /// </summary>
        internal void Reset()
        {
            cmbbxStrategy.ValueChanged -= cmbbxStrategy_ValueChanged;
            cmbbxStrategy.Value = int.MinValue;
            cmbbxStrategy.ValueChanged += cmbbxStrategy_ValueChanged;
        }



    }
}
