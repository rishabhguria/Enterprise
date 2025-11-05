//using Infragistics.Win.UltraWinGrid;
//using Prana.LogManager;
//using System;
//using System.Collections;
//using System.IO;
//using System.Text;
//using System.Windows.Forms;

//namespace Prana.Utilities.UI.UIUtilities
//{
//    public class TimedExcelWriter : IDisposable
//    {
//        int _counter = 1;
//        Timer _t;
//        UltraGrid _gridToWriteExcel;
//        string _rootDir;


//        public int Interval
//        {
//            set
//            {
//                _t.Interval = value;
//            }
//        }

//        private int _NumberOfFilesToKeep = 10;

//        public int NumberOfFilesToKeep
//        {
//            get { return _NumberOfFilesToKeep; }
//            set
//            {
//                if (value > 3)
//                {
//                    _NumberOfFilesToKeep = value;
//                }
//            }
//        }


//        public TimedExcelWriter()
//        {
//            try
//            {
//                _t = new Timer();
//                _t.Tick += new EventHandler(_t_Tick);
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }


//        public TimedExcelWriter(UltraGrid gridToWrite, string RootDirectory, int interval, bool start)
//            : base()
//        {
//            try
//            {
//                _t = new Timer();
//                _t.Tick += new EventHandler(_t_Tick);
//                if (start)
//                {
//                    _t.Interval = interval;
//                    this.GridToWrite(gridToWrite, RootDirectory);

//                    StartWriting();
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }
//        private delegate StringBuilder UIThreadMarshaller(UltraGrid grdConsolidation);
//        VisiblePosComparer comparer = new VisiblePosComparer();

//        private void WriteData(StringBuilder fileContent, string filepath)
//        {
//            //StreamWriter sw = File.CreateText(filepath);
//            using (StreamWriter sw = File.CreateText(filepath))
//            {
//                if (fileContent != null)
//                {
//                    sw.WriteLine(fileContent.ToString());
//                }
//            }
//        }

//        //StringBuilder _file = null;
//        private StringBuilder GetFileContent(UltraGrid grdToExport)
//        {
//            StringBuilder s = new StringBuilder();
//            // StreamWriter sw = File.CreateText(fileName);
//            try
//            {
//                if (UIValidation.GetInstance().validate(grdToExport))
//                {
//                    if (grdToExport.InvokeRequired)
//                    {
//                        UIThreadMarshaller mi = new UIThreadMarshaller(GetFileContent);
//                        grdToExport.BeginInvoke(mi, new object[] { grdToExport });
//                    }
//                    // StreamWriter sw = File.CreateText(fileName);
//                    else
//                    {
//                        //if (!String.IsNullOrEmpty(fileName))
//                        //{

//                        string groupByColCaption = string.Empty;

//                        if (grdToExport.DisplayLayout.Bands.Count > 0)
//                        {
//                            UltraGridBand band = grdToExport.DisplayLayout.Bands[0];

//                            if (band.SortedColumns != null && band.SortedColumns.Count > 0)
//                            {
//                                foreach (UltraGridColumn col in band.SortedColumns)
//                                {
//                                    if (col.IsGroupByColumn)
//                                    {
//                                        groupByColCaption = band.SortedColumns[0].Header.Caption;
//                                        break;
//                                    }
//                                }
//                            }


//                            UltraGridColumn[] colArr = new UltraGridColumn[band.Columns.All.Length];

//                            band.Columns.All.CopyTo(colArr, 0);
//                            //Sort the array according to the visible position of column in grid
//                            Array.Sort(colArr, comparer);

//                            foreach (UltraGridColumn col in colArr)
//                            {
//                                if (!col.Hidden)
//                                {
//                                    s.Append(col.Header.Caption).Append(",");
//                                }
//                            }

//                            //TODO: check the code for not writing while building the string, and write only in the end.
//                            // sw.WriteLine(s.ToString().TrimEnd(','));
//                            // WriteData(sw, s);
//                            //this is done as length = 0 will initialize the string pointed to by the string builder.
//                            //Another inefficient way would be to call s= new stringbuilder();
//                            s.Append(Environment.NewLine);

//                            if (!string.IsNullOrEmpty(groupByColCaption))
//                            {
//                                if (grdToExport.Rows != null)
//                                {
//                                    foreach (UltraGridRow row in grdToExport.Rows)
//                                    {
//                                        if (row.IsGroupByRow && row.Hidden.Equals(false))
//                                        {
//                                            SummaryValuesCollection summaryCol = row.ChildBands[0].Rows.SummaryValues;
//                                            bool notInSummary = false;
//                                            foreach (UltraGridColumn col in colArr)
//                                            {
//                                                notInSummary = false;
//                                                if (!col.Hidden)
//                                                {
//                                                    foreach (SummaryValue summary in summaryCol)
//                                                    {
//                                                        if (summary.Key.Equals(col.Key))
//                                                        {
//                                                            if (summary.Value.ToString().Contains(","))
//                                                            {
//                                                                s.Append("\"").Append(summary.Value.ToString()).Append("\"").Append(",");
//                                                            }
//                                                            else
//                                                            {
//                                                                s.Append(summary.Value.ToString()).Append(",");
//                                                            }

//                                                            notInSummary = true;
//                                                            break;
//                                                        }
//                                                    }
//                                                    // if column is visible true and does not exists in the summary, then also add in the export file
//                                                    // so that number of column will be same
//                                                    if (!notInSummary)
//                                                    {
//                                                        s.Append("").Append(",");
//                                                    }
//                                                }
//                                            }
//                                            //s.Remove(s.Length - 1, 1);
//                                            //s.ToString().TrimEnd(',');
//                                            s.Append(Environment.NewLine);
//                                        }
//                                    }
//                                    //s.Remove()
//                                }

//                                //WriteData(sw, s);
//                            }
//                            else
//                            {
//                                if (grdToExport.Rows != null && colArr.Length > 0)
//                                {
//                                    UltraGridRow[] filterednonGropuedRows = grdToExport.Rows.GetFilteredInNonGroupByRows();
//                                    foreach (UltraGridRow row in filterednonGropuedRows)
//                                    {
//                                        foreach (UltraGridColumn col in colArr)
//                                        {
//                                            if (!col.Hidden)
//                                            {
//                                                if (row.Cells[col.Key].Value != null)
//                                                {
//                                                    if (row.Cells[col.Key].Value.ToString().Contains(","))
//                                                    {
//                                                        s.Append("\"").Append(row.Cells[col.Key].Value).Append("\"").Append(",");
//                                                    }
//                                                    else
//                                                    {
//                                                        s.Append(row.Cells[col.Key].Value).Append(",");
//                                                    }
//                                                }
//                                                else
//                                                {
//                                                    s.Append("").Append(",");
//                                                }
//                                            }
//                                        }

//                                        //s.Remove(s.Length - 1, 1);
//                                        //s.ToString().TrimEnd(',');
//                                        //NewLine does the job, and writing is done only once
//                                        s.Append(Environment.NewLine);
//                                    }
//                                }
//                                // WriteData(sw, s);
//                            }
//                            // sw.WriteLine(s.ToString().TrimEnd(','));
//                        }
//                        //}
//                        return s.Remove(s.Length - 1, 1);
//                    }
//                }
//            }

//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//            return null;

//        }

//        bool _isWriting = false;
//        void _t_Tick(object sender, EventArgs e)
//        {
//            //there is no need to check if _isWriting, as the timer being used is windows timer which raises event on UI thread.
//            //this event wont be raised if the thread is already busy writing
//            string dataFileName = string.Empty;
//            string blankFileName = string.Empty;
//            if (_isWriting)
//            {
//                return;
//            }

//            try
//            {
//                _isWriting = true;
//                // for sending file to a remote computer
//                //string text = @"\\192.168.1.18\\Test" + @"\" + counter.ToString() + ".xls";
//                //string empty = @"\\192.168.1.18\\Test" + @"\" + (counter + 1).ToString() + ".xls";

//                dataFileName = _rootDir + @"\" + _counter.ToString() + ".csv";
//                blankFileName = _rootDir + @"\" + (_counter + 1).ToString() + ".csv";
//                _counter++;
//                if (this._gridToWriteExcel.Parent != null)
//                {
//                    /// If filename is empty then don't need to write the file
//                    if (!String.IsNullOrEmpty(dataFileName))
//                    {
//                        StringBuilder fileContentSB = GetFileContent(_gridToWriteExcel);
//                        WriteData(fileContentSB, dataFileName);
//                    }

//                    //SetExcelLayoutAndWrite(blankFileName, new UltraGrid());
//                    /// If filename is empty then don't need to write the file
//                    if (!String.IsNullOrEmpty(blankFileName))
//                    {
//                        StringBuilder blankSB = new StringBuilder();
//                        WriteData(blankSB, blankFileName);
//                    }
//                }
//                DeleteOldFiles();

//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//            finally
//            {
//                //_file = null;
//                dataFileName = null;
//                blankFileName = null;
//                _isWriting = false;
//            }
//        }

//        /// <summary>
//        /// deletes files older than a certain count
//        /// </summary>
//        /// <param name="rootDir"></param>
//        private void DeleteOldFiles()
//        {
//            try
//            {
//                DirectoryInfo dir = new DirectoryInfo(_rootDir);
//                FileInfo[] xlsfiles = dir.GetFiles((_counter - _NumberOfFilesToKeep) + ".csv");

//                foreach (FileInfo f in xlsfiles)
//                {
//                    f.Delete();
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }


//        }

//        /// <summary>
//        /// Deletes old files in FTP Folder
//        /// </summary>
//        public void StartWriting()
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(_rootDir) || !NetworkFolderFinder.MachineExist(_rootDir, 100))
//                {

//                    return;
//                }
//                // need to take care that whether we have permission to create folder/ file on the
//                // specifed path or not.
//                DirectoryInfo dir = new DirectoryInfo(_rootDir);
//                if (!NetworkFolderFinder.DirectoryExist(_rootDir, 100))
//                {
//                    dir.Create();
//                }
//                FileInfo[] csvfiles = dir.GetFiles("*.csv");
//                if (csvfiles != null)
//                {
//                    foreach (FileInfo f in csvfiles)
//                    {
//                        f.Delete();
//                    }
//                }
//                _t.Start();
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }


//        /// <summary>
//        /// Stops writing without disposing
//        /// </summary>
//        public void StopWriting()
//        {
//            try
//            {
//                _t.Stop();
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }


//        public void GridToWrite(UltraGrid gridToWriteExcel, string RootDirectory)
//        {
//            try
//            {
//                _gridToWriteExcel = gridToWriteExcel;
//                _rootDir = RootDirectory;
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }



//        public void Close()
//        {
//            try
//            {
//                _t.Stop();
//                _t = null;
//                _gridToWriteExcel = null;
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }


//        }



//        public class VisiblePosComparer : IComparer
//        {
//            public VisiblePosComparer()
//            {
//            }

//            public int Compare(object x, object y)
//            {
//                UltraGridColumn xCol = (UltraGridColumn)x;
//                UltraGridColumn yCol = (UltraGridColumn)y;
//                int val = -1;
//                if ((xCol.Header.VisiblePosition == yCol.Header.VisiblePosition))
//                {
//                    val = 0;
//                }
//                else if ((xCol.Header.VisiblePosition > yCol.Header.VisiblePosition))
//                {
//                    val = 1;
//                }
//                else
//                {
//                    val = -1;
//                }

//                return val;
//            }
//        }
//        #region IDisposable
//        protected virtual void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                // dispose managed resources
//                _t.Dispose();
//            }
//            // free native resources
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }
//        #endregion
//    }
//}
