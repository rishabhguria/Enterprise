using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.AlertHistory.UI.UserControls
{
    public partial class AlertHistoryPaging : UserControl
    {
        public event EventHandler<EventArgs<int, int>> PageChanged;  //Event Declaration

        #region Constructor
        public AlertHistoryPaging()
        {
            try
            {
                InitializeComponent();
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
        #endregion

        #region Getting Data and Udating pages
        /// <summary>
        /// For total no. pf pages
        ///  </summary>
        private int _totalPages; //

        /// <summary>
        ///  For current page
        /// </summary>
        private int _currentPage = 1;

        /// <summary>
        /// For page Size
        /// </summary>
        private int _pageSize = 20;

        /// <summary>
        /// Update total number of pages  
        /// </summary>
        /// <param name="totalPages">Taking as one int argument for total pages</param>
        internal void UpdateTotalPages(int totalPages)
        {
            try
            {
                _totalPages = totalPages;
                if (_currentPage > totalPages)
                    _currentPage = totalPages;
                lblRecordStatus.Text = (_currentPage) + " of " + _totalPages;
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

        /// <summary>
        /// Getting Current Page
        /// </summary>
        /// <returns> current page as integer</returns>
        internal int GetCurrentPage()
        {
            try
            {
                return _currentPage;
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
                return 0;
            }
        }

        /// <summary>
        /// Getting Page Size
        /// </summary>
        /// <returns> page size as int</returns>
        internal int GetPageSize()
        {
            try
            {
                _pageSize = Convert.ToInt32(ultraNumericEditorPageSize.Value);
                return _pageSize;
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
                return 0;
            }
        }

        /// <summary>
        /// Updating pageNumber and raising Event
        /// </summary>
        private void UpdatePageNo()
        {
            try
            {
                btnPrevious.Enabled = true;
                btnNext.Enabled = true;
                btnLast.Enabled = true;
                btnFirst.Enabled = true;
                if (_currentPage == 1)
                {
                    btnPrevious.Enabled = false;
                    btnFirst.Enabled = false;
                }
                if (_currentPage == _totalPages)
                {
                    btnNext.Enabled = false;
                    btnLast.Enabled = false;
                }
                lblRecordStatus.Text = _currentPage + " of " + _totalPages;
                if (PageChanged != null)
                    PageChanged(this, new EventArgs<int, int>(_currentPage, _pageSize));

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

        /// <summary>
        /// Reset control Enabled/Disabled Buttons
        /// </summary>
        internal void Reset()
        {
            try
            {
                _currentPage = 1;
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;

                if (_currentPage == _totalPages)
                {
                    btnNext.Enabled = false;
                    btnLast.Enabled = false;
                }
                else
                {
                    btnLast.Enabled = true;
                    btnNext.Enabled = true;
                }
                lblRecordStatus.Text = _currentPage + " of " + _totalPages;
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
        #endregion

        #region Loading Paging Control
        /// <summary>
        /// Updating label of current page and total page on control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlertHistoryPaging_Load(object sender, EventArgs e)
        {
            try
            {
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                if (_currentPage == _totalPages)
                {
                    btnNext.Enabled = false;
                    btnLast.Enabled = false;
                }
                lblRecordStatus.Text = (_currentPage) + " of " + _totalPages;
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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
        #endregion

        private void SetButtonsColor()
        {
            try
            {
                btnLast.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnLast.ForeColor = System.Drawing.Color.White;
                btnLast.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnLast.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnLast.UseAppStyling = false;
                btnLast.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnNext.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnNext.ForeColor = System.Drawing.Color.White;
                btnNext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnNext.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnNext.UseAppStyling = false;
                btnNext.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnPrevious.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnPrevious.ForeColor = System.Drawing.Color.White;
                btnPrevious.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnPrevious.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnPrevious.UseAppStyling = false;
                btnPrevious.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnFirst.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnFirst.ForeColor = System.Drawing.Color.White;
                btnFirst.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnFirst.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnFirst.UseAppStyling = false;
                btnFirst.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        #region Buttons Clicked and Numeric Editor changed value
        /// <summary>
        /// Button First Clicked then Enable/Disable buttons and updating label and raising Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                _currentPage = 1;
                UpdatePageNo();
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

        /// <summary>
        /// Button Previous Clicked then Enable/Disable buttons and updating label and raising Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                _currentPage--;
                UpdatePageNo();
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

        /// <summary>
        /// Button Next Clicked then Enable/Disable buttons and updating label and raising Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                _currentPage++;
                UpdatePageNo();
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

        /// <summary>
        /// Button Last Clicked then Enable/Disable buttons and updating label and raising Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                _currentPage = _totalPages;
                UpdatePageNo();
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

        /// <summary>
        /// Numeric Editor setting page Size and Raising event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraNumericEditorPageSize_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                _pageSize = Convert.ToInt32(ultraNumericEditorPageSize.Value);
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
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
        #endregion
    }
}

