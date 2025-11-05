using Infragistics.Win.Misc;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace Prana
{
    public partial class UsrCtrlCounterPartyStatusDetails : UltraPanel
    {
        Dictionary<int, UsrCtrlConnectionStatus> _dictUsrCtrlConnectionStatus = new Dictionary<int, UsrCtrlConnectionStatus>();

        public UsrCtrlCounterPartyStatusDetails()
        {
            InitializeComponent();
        }

        public void SetUp(CounterPartyCollection counterPartyCollection)
        {
            try
            {
                Disposed += UsrCtrlCounterPartyStatusDetails_Disposed;
                panelCounterPartiesStatus.ClientArea.Controls.Clear();
                _dictUsrCtrlConnectionStatus.Clear();

                int totalHeight = 0;
                foreach (BusinessObjects.CounterParty counterParty in counterPartyCollection)
                {
                    UsrCtrlConnectionStatus usrCtrlConnectionStatus = new UsrCtrlConnectionStatus(counterParty.Name);
                    panelCounterPartiesStatus.ClientArea.Controls.Add(usrCtrlConnectionStatus);
                    usrCtrlConnectionStatus.Location = new Point(0, totalHeight + 1);
                    totalHeight += usrCtrlConnectionStatus.Height;
                    _dictUsrCtrlConnectionStatus.Add(counterParty.CounterPartyID, usrCtrlConnectionStatus);
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

        private void UsrCtrlCounterPartyStatusDetails_Disposed(object sender, EventArgs e)
        {
            try
            {
                TradeManagerExtension.GetInstance().CounterPartyStatusUpdate -= new TradeManagerExtension.CounterPartyConnectHandler(communicationManager_CounterPartyStatusUpdate);
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

        public void BindCounterPartyStatusUpdate()
        {
            try
            {
                TradeManagerExtension.GetInstance().CounterPartyStatusUpdate += new TradeManagerExtension.CounterPartyConnectHandler(communicationManager_CounterPartyStatusUpdate);
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

        void communicationManager_CounterPartyStatusUpdate(object sender, EventArgs<CounterPartyDetails> e)
        {
            try
            {
                if (e.Value != null && e.Value.OriginatorType != PranaServerConstants.OriginatorType.Allocation && _dictUsrCtrlConnectionStatus.ContainsKey(e.Value.CounterPartyID))
                {
                    _dictUsrCtrlConnectionStatus[e.Value.CounterPartyID].UserCtrlConnectionStatusUpdate(e.Value.ConnStatus);
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
    }
}
