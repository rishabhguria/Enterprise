using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Prana.Utilities.UI
{
    public partial class ctrlTesting : UserControl
    {

        TestingParameters _testingParameters = null;
        string _layout;
        string _LayoutDirectoryPath;
        string _LayoutFilePath;
        public ctrlTesting()
        {
            InitializeComponent();
        }

        public void SetUp(TestingParameters testingParameters)
        {
            try
            {
                _testingParameters = testingParameters;
                ControlMoverOrResizer.WorkType = ControlMoverOrResizer.MoveOrResize.MoveAndResize;
                _testingParameters.ParameterAdded += _testingParameters_ParameterAdded;

                foreach (KeyValuePair<string, object> kvp in _testingParameters.Data)
                {
                    AddControls(kvp.Key);
                }

                _LayoutDirectoryPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + "TestingForms";
                _LayoutFilePath = _LayoutDirectoryPath + @"\" + testingParameters.FormName + ".xml";

                LoadLayout();
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        void _testingParameters_ParameterAdded(object sender, EventArgs<string> e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => _testingParameters_ParameterAdded(sender, e)));
                }
                else if (!this.IsDisposed)
                {
                    AddControls(e.Value);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void AddControls(string Key)
        {
            try
            {
                if (_testingParameters.Data.ContainsKey(Key))
                {
                    if (this.ultraPanel1.ClientArea.Controls.ContainsKey("ultraGroupBox" + Key))
                    {
                        UltraGrid grid = (UltraGrid)this.ultraPanel1.ClientArea.Controls["ultraGroupBox" + Key].Controls["grid" + Key];
                        grid.DataSource = _testingParameters.Data[Key];
                    }
                    else
                    {
                        UltraGroupBox ultraGroupBox = new UltraGroupBox();
                        ultraGroupBox.Name = "ultraGroupBox" + Key;
                        ultraGroupBox.Text = Key;
                        UltraGrid grid = new UltraGrid();
                        grid.DataSource = _testingParameters.Data[Key];
                        grid.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                        ultraGroupBox.Controls.Add(grid);
                        grid.Dock = DockStyle.Fill;
                        grid.Name = "grid" + Key;

                        grid.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortSingle;
                        grid.DisplayLayout.Override.SelectTypeCol = SelectType.SingleAutoDrag;
                        //ControlMoverOrResizer.Init(grid, ultraGroupBox);
                        ControlMoverOrResizer.Init(ultraGroupBox);
                        ultraPanel1.ClientArea.Controls.Add(ultraGroupBox);
                    }
                }
                else
                {
                    if (this.Controls.ContainsKey("ultraGroupBox" + Key))
                    {
                        this.Controls.RemoveByKey("ultraGroupBox" + Key);
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void LoadLayout()
        {
            try
            {

                if (File.Exists(_LayoutFilePath))
                {
                    _layout = File.ReadAllText(_LayoutFilePath);
                }
                if (!string.IsNullOrWhiteSpace(_layout))
                {
                    ControlMoverOrResizer.SetSizeAndPositionOfControlsFromString(this, _layout);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _layout = ControlMoverOrResizer.GetSizeAndPositionOfControlsToString(this);
                if (!Directory.Exists(Path.GetDirectoryName(_LayoutFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_LayoutFilePath));
                }
                File.WriteAllText(_LayoutFilePath, _layout);
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }
    }
}
