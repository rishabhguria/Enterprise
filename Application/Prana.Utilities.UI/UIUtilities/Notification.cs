using Infragistics.Win.Misc;
using Prana.BusinessObjects.Classes;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class Notification : Form
    {
        Dictionary<String, UltraDesktopAlertShowWindowInfo> _infoItemList = new Dictionary<String, UltraDesktopAlertShowWindowInfo>();

        public Notification()
        {
            try
            {
                InitializeComponent();
                Infragistics.Win.Misc.Resources.Customizer.SetCustomizedString("DesktopAlertLinkUIElement_Caption_ToolTipText_Office2007", string.Empty);
                Infragistics.Win.Misc.Resources.Customizer.SetCustomizedString("DesktopAlertLinkUIElement_Text_ToolTipText_Office2007", string.Empty);

                this.Show();
                this.Visible = false;
                ShowMessage("Nirvana Notification Service", "Nirvana Notification Service Started", "NirvanaNotificationService", Severity.Info);
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
                throw;
            }
        }

        delegate void MainThreadDelegate(String title, String message, String key, Severity iconType);
        public void ShowMessage(String title, String message, String key, Severity iconType)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MainThreadDelegate del = this.ShowMessage;
                        this.BeginInvoke(del, new object[] { title, message, key, iconType });
                    }
                    else
                    {
                        lock (_infoItemList)
                        {
                            message = message.Replace("\n", "<br/>");
                            //bool isPinned = false;
                            if (_infoItemList.ContainsKey(key))
                            {
                                _infoItemList[key].Text = message;
                                //isPinned = _infoItemList[key].Pinned;
                                if (this.notifyIcon.IsOpen(key))
                                    this.notifyIcon.Close(key);

                            }
                            else
                            {
                                UltraDesktopAlertShowWindowInfo infoItem = new UltraDesktopAlertShowWindowInfo();
                                infoItem.Key = key;
                                infoItem.Text = message;
                                infoItem.Caption = title;
                                //infoItem.PinButtonVisible = true;
                                _infoItemList.Add(key, infoItem);
                            }
                            if (iconType == Severity.Critical)
                            {
                                _infoItemList[key].Image = Resource1.images;
                                _infoItemList[key].Sound = Resource1.ErrorSound;
                            }
                            else if (iconType == Severity.Info)
                            {
                                _infoItemList[key].Image = Resource1.info;
                                _infoItemList[key].Sound = Resource1.NotifySound;
                            }
                            else
                                _infoItemList[key].Sound = Resource1.WarningSound;

                            //_infoItemList[key].Pinned = isPinned;
                            this.notifyIcon.Show(_infoItemList[key]);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
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