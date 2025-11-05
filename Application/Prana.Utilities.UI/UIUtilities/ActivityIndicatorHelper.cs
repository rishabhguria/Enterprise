using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Prana.Utilities.UI
{
    public static class ActivityIndicatorHelper
    {
        public delegate void ControlUpdateHandler(Control form);

        public static void StopActivityIndicator(Control controlActivityContainer)
        {
            try
            {
                if (UIValidation.GetInstance().validate(controlActivityContainer))
                {
                    if (controlActivityContainer.InvokeRequired)
                    {
                        controlActivityContainer.BeginInvoke(new ControlUpdateHandler(StopActivityIndicator), new object[] { controlActivityContainer });
                    }
                    else
                    {
                        Control[] controlArr = controlActivityContainer.Controls.Find("ActivityIndicator", true);
                        ActivityIndicator activityIndicator = null;
                        if (controlArr.Length > 0)
                        {
                            activityIndicator = controlArr[0] as ActivityIndicator;
                        }
                        else
                        {
                            activityIndicator = controlActivityContainer as ActivityIndicator;
                        }

                        if (activityIndicator != null)
                        {
                            SetDefaultProperties(activityIndicator);
                            activityIndicator.Stop();
                        }
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


        public static void StartActivityIndicator(Control controlActivityContainer)
        {
            try
            {
                if (UIValidation.GetInstance().validate(controlActivityContainer))
                {
                    if (controlActivityContainer.InvokeRequired)
                    {
                        controlActivityContainer.BeginInvoke(new ControlUpdateHandler(StartActivityIndicator), new object[] { controlActivityContainer });
                    }
                    else
                    {
                        Control[] controlArr = controlActivityContainer.Controls.Find("ActivityIndicator", true);

                        ActivityIndicator activityIndicator = null;
                        if (controlArr.Length > 0)
                        {
                            activityIndicator = controlArr[0] as ActivityIndicator;
                            // controlActivityContainer.Text = info.HeaderText;
                        }
                        else
                        {
                            activityIndicator = controlActivityContainer as ActivityIndicator;
                        }

                        if (activityIndicator != null)
                        {
                            SetDefaultProperties(activityIndicator);
                            activityIndicator.Start();
                            activityIndicator.Visible = true;
                        }

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


        public static void UpdateProgress(ProgressInfo info, Control controlActivityContainer)
        {
            try
            {
                if (UIValidation.GetInstance().validate(controlActivityContainer))
                {
                    if (controlActivityContainer.InvokeRequired)
                    {
                        controlActivityContainer.BeginInvoke(new ControlUpdateHandler(StartActivityIndicator));
                    }
                    else
                    {
                        Control[] controlArr = controlActivityContainer.Controls.Find("ActivityIndicator", true);

                        ActivityIndicator activityIndicator = null;
                        if (controlArr.Length > 0)
                        {
                            activityIndicator = controlArr[0] as ActivityIndicator;
                            // controlActivityContainer.Text = info.HeaderText;
                        }
                        else
                        {
                            activityIndicator = controlActivityContainer as ActivityIndicator;
                        }

                        if (activityIndicator != null)
                        {
                            activityIndicator.SetProgressText(info);
                            if (info.IsTaskCompleted)
                            {
                                Thread.Sleep(2000);
                                StopActivityIndicator(controlActivityContainer);
                            }
                        }
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


        private static void SetDefaultProperties(ActivityIndicator controlActivityIndicator)
        {

            controlActivityIndicator.SetViewStyle(Infragistics.Win.UltraActivityIndicator.ActivityIndicatorViewStyle.Aero);

        }



    }
}
