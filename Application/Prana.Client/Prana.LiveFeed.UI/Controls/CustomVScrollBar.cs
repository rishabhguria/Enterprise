using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.LiveFeed.UI
{
    /// <summary>
    /// Summary description for VScrollBar.
    /// </summary>
    public class CustomVScrollBar : VScrollBar
    {
        protected int valLargeChange;

        public int ValLargeChange
        {
            get { return valLargeChange; }
            set { valLargeChange = value; }
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == 8469)
                {
                    switch ((short)m.WParam)
                    {
                        case 0:
                        case 2:
                            if (this.Value - this.ValLargeChange > 0)
                            {
                                this.Value -= this.ValLargeChange;
                            }
                            else
                            {
                                this.Value = 0;
                            }
                            break;

                        case 1:
                        case 3:
                            if (this.Value + this.ValLargeChange + LargeChange < this.Maximum)
                            {
                                this.Value += this.ValLargeChange;
                            }
                            else
                            {
                                this.Value = this.Maximum - LargeChange;
                            }
                            break;

                        default:
                            base.WndProc(ref m);
                            break;
                    }
                }
                else
                {
                    base.WndProc(ref m);
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
