using System;
using System.Windows.Forms;

namespace Prana.BusinessObjects
{
    public class ResizeHandlerEventArgs : EventArgs
    {
        /// <summary>
        /// the control sending raising the event
        /// </summary>
        public Control ThisControl { get; set; }

        /// <summary>
        /// Current sate of the control
        /// </summary>
        public ControlState CurrentControlState { get; set; }

        /// <summary>
        /// Current size of the control
        /// </summary>
        public int CurrentControlSize { get; set; }

        public ResizeHandlerEventArgs(Control thisControl, ControlState currentControlState, int currentControlSize)
        {
            ThisControl = thisControl;
            CurrentControlState = currentControlState;
            CurrentControlSize = currentControlSize;
        }
    }
}