using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public class ProgressBarHelper
    {
        public static void AddProgressBar(Control control)
        {
            ProgressCircle progressCircle = new ProgressCircle();
            SetDefaultProperties(progressCircle);

            progressCircle.Location = new System.Drawing.Point((control.Location.X + control.Width) / 2, (control.Location.Y + control.Height) / 2);
            control.Controls.Add(progressCircle);
            progressCircle.BringToFront();
        }

        public static void StopProgressBar(Control control)
        {
            try
            {
                if (UIValidation.GetInstance().validate(control))
                {
                    if (control.InvokeRequired)
                    {
                        ControlUpdateHandler controlUpdateHandler = new ControlUpdateHandler(StopProgressBar);
                        control.BeginInvoke(controlUpdateHandler, new Object[] { control });
                    }
                    else
                    {
                        Control[] controlArr = control.Controls.Find("ProgressBar", true);
                        if (controlArr.Length > 0)
                        {
                            ProgressCircle progressCircle = controlArr[0] as ProgressCircle;
                            if (progressCircle != null)
                            {
                                progressCircle.Rotate = false;
                                progressCircle.Visible = false;
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

        private static void SetDefaultProperties(ProgressCircle progressBar1)
        {
            progressBar1.BackColor = System.Drawing.Color.Transparent;
            progressBar1.ForeColor = System.Drawing.Color.Green;
            progressBar1.Interval = 100;
            progressBar1.Location = new System.Drawing.Point(235, 90);
            progressBar1.Name = "ProgressBar";
            progressBar1.RingColor = System.Drawing.Color.White;
            progressBar1.RingThickness = 10;
            progressBar1.Size = new System.Drawing.Size(80, 80);
            progressBar1.Rotate = true;
        }
    }

    public delegate void ControlUpdateHandler(Control control);
}
