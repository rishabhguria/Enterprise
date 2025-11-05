using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.LiveFeed.UI
{
    /// <summary>
    /// This class has been created to avoid the flickering problem coming in the custom grid.
    /// </summary>
    public class PaintPanel : System.Windows.Forms.Panel
    {
        public PaintPanel()
        {
            try
            {
                SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
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
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //			base.OnPaintBackground (pevent); 
        }
    }
}
