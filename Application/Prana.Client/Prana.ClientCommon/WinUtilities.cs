using System;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public static class SafeNativeMethods
    {
        //http://stackoverflow.com/questions/487661/how-do-i-suspend-painting-for-a-control-and-its-children?rq=1
        public static class ControlDrawing
        {
            public static void SuspendDrawing(Control parent)
            {
                Prana.Utilities.Win32Utilities.SafeNativeMethods.SendMessageB(parent.Handle.ToInt32(), 11, Convert.ToInt32(false), 0);
            }

            public static void ResumeDrawing(Control parent)
            {
                Prana.Utilities.Win32Utilities.SafeNativeMethods.SendMessageB(parent.Handle.ToInt32(), 11, Convert.ToInt32(true), 0);
                parent.Refresh();
            }
        }
    }
}
