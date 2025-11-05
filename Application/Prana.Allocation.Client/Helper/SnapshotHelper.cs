using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Prana.Allocation.Client.Helper
{
    internal class SnapshotHelper
    {
        #region Members

        /// <summary>
        /// The _locker print
        /// </summary>
        private static object _lockerPrint = new object();

        /// <summary>
        /// The p document
        /// </summary>
        private static PrintDocument _printDocument = new PrintDocument();

        /// <summary>
        /// The _file path
        /// </summary>
        private static string _filePath = string.Empty;

        #endregion Members

        #region Methods

        /// <summary>
        /// Clicks the snap shot.
        /// </summary>
        internal static void ClickSnapShot(Window allocationClientWindow)
        {
            try
            {
                int top = (allocationClientWindow.WindowState == WindowState.Maximized) ? 0 : Convert.ToInt32(allocationClientWindow.Top);
                int left = (allocationClientWindow.WindowState == WindowState.Maximized) ? 0 : Convert.ToInt32(allocationClientWindow.Left);
                int right = 0;
                int bottom = 0;
                if (allocationClientWindow.WindowState != WindowState.Maximized)
                {
                    right = (Screen.PrimaryScreen.WorkingArea.Right < Convert.ToInt32(allocationClientWindow.RestoreBounds.Right)) ? Screen.PrimaryScreen.WorkingArea.Right : Convert.ToInt32(allocationClientWindow.RestoreBounds.Right);
                    bottom = (Screen.PrimaryScreen.WorkingArea.Bottom < Convert.ToInt32(allocationClientWindow.RestoreBounds.Bottom)) ? Screen.PrimaryScreen.WorkingArea.Bottom : Convert.ToInt32(allocationClientWindow.RestoreBounds.Bottom);
                }
                else
                {
                    right = Screen.PrimaryScreen.WorkingArea.Right;
                    bottom = Screen.PrimaryScreen.WorkingArea.Bottom;
                }
                int width = right - left;
                int height = bottom - top;
                Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                Graphics graphics = Graphics.FromImage(bitmap as Image);
                graphics.CopyFromScreen(left, top, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
                _filePath = System.Windows.Forms.Application.StartupPath + "\\" + "1";
                bitmap.Save(_filePath, ImageFormat.Jpeg);
                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                ((ToolStripButton)((ToolStrip)printPreviewDialog.Controls[1]).Items[9]).Text = "OK";
                ((ToolStripButton)((ToolStrip)printPreviewDialog.Controls[1]).Items[9]).Click += new EventHandler(Print_Click);
                _printDocument.DefaultPageSettings.Landscape = true;
                _printDocument.PrintPage += Print_Preview;
                printPreviewDialog.Document = _printDocument;
                printPreviewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the DoWork event of the bgWorkerObj control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private static void bgWorkerObj_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _printDocument.Print();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the Click event of the Print control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void Print_Click(object sender, EventArgs e)
        {
            try
            {
                lock (_lockerPrint)
                {
                    PrintDialog printDialog = new PrintDialog();
                    printDialog.Document = _printDocument;
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
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the PrintPage event of the pDoc control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PrintPageEventArgs"/> instance containing the event data.</param>
        private static void Print_Preview(object sender, PrintPageEventArgs e)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
                Image image = Image.FromStream(fs);
                int x = e.MarginBounds.X;
                int y = e.MarginBounds.Y;
                Rectangle destRect = new Rectangle(x, y, e.MarginBounds.Width, e.MarginBounds.Height);
                e.Graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                fs.Dispose();
            }
        }

        #endregion Methods
    }
}
