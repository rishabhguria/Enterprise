using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public class SnapShotManager : IDisposable
    {
        string _filePath = "";
        PrintDocument _documentToPrint;
        const string _displayName = "Snapshot";
        const string _toolTipText = "Take Snapshot";
        Infragistics.Win.Appearance _appearance = new Infragistics.Win.Appearance();
        private static SnapShotManager _singleton = null;
        private static object _locker = new object();
        private static object _lockerSnapshot = new object();
        private static object _lockerPrint = new object();

        public static SnapShotManager GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new SnapShotManager();
                    }
                }
            }
            return _singleton;
        }
        public SnapShotManager()
        {
            _documentToPrint = new PrintDocument();
            _documentToPrint.PrintPage += new PrintPageEventHandler(documentToPrint_PrintPage);
        }
        public ButtonTool buttonTool
        {
            get
            {
                ButtonTool buttonTool = new ButtonTool("ScreenShot");
                buttonTool.SharedProps.ToolTipText = _toolTipText;
                buttonTool.SharedProps.Caption = _displayName;
                buttonTool.SharedProps.AppearancesSmall.Appearance = new Infragistics.Win.Appearance(); ;
                return buttonTool;
            }
        }
        public UltraButton ultraButton
        {
            get
            {
                UltraButton ultraButton = new UltraButton();
                ultraButton.Text = _displayName;
                return ultraButton;
            }
        }
        public UltraTab ultraTab
        {
            get
            {
                UltraTab ultraTab = new UltraTab();
                ultraTab.Text = _displayName;
                return ultraTab;
            }
        }
        public ToolStripButton toolStripButton
        {
            get
            {
                ToolStripButton toolStripButton = new ToolStripButton();
                toolStripButton.Text = _displayName;
                return toolStripButton;
            }
        }
        public Button button
        {
            get
            {
                Button button = new Button();
                button.Text = _displayName;
                return button;
            }
        }
        public void TakeSnapshot(Form form)
        {
            try
            {
                lock (_lockerSnapshot)
                {
                    Bitmap bmpScreenshot;
                    Graphics gfxScreenshot;
                    bmpScreenshot = new Bitmap(form.Bounds.Width, form.Bounds.Height, PixelFormat.Format32bppArgb);
                    // Create a graphics object from the bitmap
                    gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                    // Take the screenshot from the upper left corner to the right bottom corner
                    gfxScreenshot.CopyFromScreen(form.Bounds.X, form.Bounds.Y, 0, 0, form.Bounds.Size, CopyPixelOperation.SourceCopy);
                    // Save the screenshot to the application startup path
                    _filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + "1";
                    bmpScreenshot.Save(_filePath, ImageFormat.Jpeg);
                    //Code for print preview.
                    PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                    ((Form)printPreviewDialog).WindowState = FormWindowState.Maximized;
                    ((ToolStripButton)((ToolStrip)printPreviewDialog.Controls[1]).Items[9]).Text = "OK";
                    ((ToolStripButton)((ToolStrip)printPreviewDialog.Controls[1]).Items[9]).Click += new EventHandler(Print_Click);
                    // To disable the print icon.
                    ((ToolStripButton)((ToolStrip)printPreviewDialog.Controls[1]).Items[0]).Enabled = false;
                    //page Orientation will be Landscape mode by default
                    _documentToPrint.DefaultPageSettings.Landscape = true;
                    printPreviewDialog.Document = _documentToPrint;
                    printPreviewDialog.ShowDialog();
                    bmpScreenshot.Dispose();
                    gfxScreenshot.Dispose();
                }
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
        void Print_Click(object sender, EventArgs e)
        {
            try
            {
                lock (_lockerPrint)
                {
                    PrintDialog printDialog = new PrintDialog();
                    printDialog.Document = _documentToPrint;
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        BackgroundWorker bgWorkerObj = new BackgroundWorker();
                        bgWorkerObj.DoWork += new DoWorkEventHandler(bgWorkerObj_DoWork);
                        bgWorkerObj.RunWorkerAsync();
                    }
                }
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
        void bgWorkerObj_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _documentToPrint.Print();
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
        private void documentToPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
                System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                int x = e.MarginBounds.X;
                int y = e.MarginBounds.Y;
                System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(x, y, e.MarginBounds.Width, e.MarginBounds.Height);
                e.Graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                fs.Dispose();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.

            // Always use SuppressFinalize() in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Overloaded Implementation of Dispose.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; 
        /// <c>false</c> to release only unmanaged resources.</param>
        /// <remarks>
        /// <list type="bulleted">Dispose(bool isDisposing) executes in two distinct scenarios.
        /// <item>If <paramref name="isDisposing"/> equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.</item>
        /// <item>If <paramref name="isDisposing"/> equals <c>false</c>, the method has been called 
        /// by the runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.</item></list>
        /// </remarks>
        protected virtual void Dispose(bool isDisposing)
        {
            // TODO If you need thread safety, use a lock around these 
            // operations, as well as in your methods that use the resource.

            // Explicitly set root references to null to expressly tell the GarbageCollector
            // that the resources have been disposed of and its ok to release the memory 
            // allocated for them.
            if (isDisposing)
            {
                // Release all managed resources here
                // Need to unregister/detach yourself from the events. Always make sure
                // the object is not null first before trying to unregister/detach them!
                // Failure to unregister can be a BIG source of memory leaks
                _appearance.Dispose();
                _documentToPrint.Dispose();
                // If this is a WinForm/UI control, uncomment this code
                //if (components != null)
                //{
                //    components.Dispose();
                //}
                // Release all unmanaged resources here  
            }
        }
        #endregion
    }
}
