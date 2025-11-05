using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.Interfaces;
using Infragistics.Win.UltraWinToolTip;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;


namespace Prana.LiveFeedEngine
{
    public partial class LivefeedConnection : UserControl
    {
        LiveFeedSubscriber _liveFeedSubscriber = LiveFeedSubscriber.GetInstance();
        public LivefeedConnection()
        {
            InitializeComponent();
            InitiateLivefeedControl();
            SetDataManagerConnectionStatus();
        }

        private void InitiateLivefeedControl()
        {
            if (_liveFeedSubscriber != null)
            {
                _liveFeedSubscriber.DataManagerConnected += new EventHandler(_liveFeedSubscriber_DataManagerConnected);
                _liveFeedSubscriber.DataManagerDisConnected += new EventHandler(_liveFeedSubscriber_DataManagerDisConnected);
            }

        }

        void _liveFeedSubscriber_DataManagerDisConnected(object sender, EventArgs e)
        {
            DisplayDMConnectionStatus(false);
        }

        void _liveFeedSubscriber_DataManagerConnected(object sender, EventArgs e)
        {
            DisplayDMConnectionStatus(true);
        }

        private void SetDataManagerConnectionStatus()
        {
            try
            {
                if (null != _liveFeedSubscriber && _liveFeedSubscriber.IsDataManagerConnected())
                {
                    DisplayDMConnectionStatus(true);
                }
                else
                {
                    DisplayDMConnectionStatus(false);
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void DisplayDMConnectionStatus(bool isConnected)
        {
            try
            {
                if (isConnected)
                {
                    pbLiveFeedConnectionStatus.Image = Resource1.Connect;
                    ultraToolTipManager1.InitialDelay = 10;
                    // Set the AutoPopDelay to 50 seconds. 
                    this.ultraToolTipManager1.AutoPopDelay = 50000;
                    // Get the ToolTipInfo for Picture Box pbPricingServerConnectionStatus
                    UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(this.pbLiveFeedConnectionStatus);
                    // Set the ToolTipText 
                    toolTipInfo.ToolTipText = "Live Feed Connected.";
                }
                else
                {
                    pbLiveFeedConnectionStatus.Image = Resource1.Disconnect;
                    ultraToolTipManager1.InitialDelay = 10;
                    // Set the AutoPopDelay to 50 seconds.  
                    this.ultraToolTipManager1.AutoPopDelay = 50000;
                    //Get the ToolTipInfo for Picture Box pbPricingServerConnectionStatus
                    UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(this.pbLiveFeedConnectionStatus);
                    // Set the ToolTipText 
                    toolTipInfo.ToolTipText = "Live Feed Disconnected .";
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

       
       
    }
}
