using Prana.DatabaseManager;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Middleware
{
    /// <summary>
    /// Class frmMiddleware
    /// </summary>
    public partial class frmMiddleware : Form, IPluggableTools, ILoginUser
    {
        //TODO:  <add key="MiddlewareManager" value="Tools~Middleware Manager~Modules\Prana.Middleware.dll~Prana.Middleware.frmMiddleware~" />
        //TODO:  copy "$(SolutionDir)Application\Prana.Client\Prana.Middleware\$(OutDir)Prana.Middleware.dll" "Modules\."
        //TODO:  copy "$(SolutionDir)Application\Prana.Client\Prana.Middleware\$(OutDir)Prana.Middleware.pdb" "Modules\."

        bool _isFormClosed = false;
        /// <summary>
        /// Middleware Jobs
        /// </summary>
        class JobInfo
        {
            /// <summary>
            /// SQL Job Id
            /// </summary>
            public Guid JobId;
            /// <summary>
            /// SQL Job Name
            /// </summary>
            public string JobName;
            /// <summary>
            /// Description
            /// </summary>
            public string Description;
            /// <summary>
            /// Group Identity
            /// </summary>
            public int GroupId;

            public JobInfo(Guid jobId, string jobName, string description, int groupId)
            {
                JobId = jobId;
                JobName = jobName;
                Description = description;
                GroupId = groupId;
            }

        }

        JobInfo SelectedJob = null;

        /// <summary>
        /// List of SQL Jobs
        /// </summary>
        List<JobInfo> Jobs = new List<JobInfo>();
        /// <summary>
        /// Current User
        /// </summary>
        private Prana.BusinessObjects.CompanyUser user;

        /// <summary>
        /// Class JobStatusEventArgs
        /// </summary>
        public class JobStatusEventArgs : EventArgs
        {
            /// <summary>
            /// JobName
            /// </summary>
            public string Name;
            /// <summary>
            /// The status
            /// </summary>
            public string Status;
            /// <summary>
            /// The state
            /// </summary>
            public string State;
            /// <summary>
            /// The last run
            /// </summary>
            public DateTime? LastRun;

            /// <summary>
            /// Initializes a new instance of the <see cref="JobStatusEventArgs"/> class.
            /// </summary>
            /// <param name="status">The status.</param>
            /// <param name="state">The state.</param>
            /// <param name="lastrun">The lastrun.</param>
            public JobStatusEventArgs(string name, string status, string state, DateTime? lastrun)
            {
                Name = name;
                Status = status;
                State = state;
                LastRun = lastrun;
            }
            public string[] Items
            {
                get
                {
                    string items = string.Format("{0},{1},{2},{3}", Name, Status, State, LastRun);
                    return items.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }

        /// <summary>
        /// Find a Job by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static Predicate<JobInfo> FindJobByName(string name)
        {
            return delegate (JobInfo jobInfo)
            {
                return jobInfo.JobName == name;
            };
        }

        static Predicate<JobInfo> FindJobByDescription(string name)
        {
            return delegate (JobInfo jobInfo)
            {
                return jobInfo.Description == name;
            };
        }

        /// <summary>
        /// Check to see if any jobs are running for the current group
        /// </summary>
        /// <returns></returns>
        bool IsJobRunning()
        {
            foreach (ListViewItem item in listJobs.Items)
            {
                if (item.SubItems[1].Text == "Running")
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="frmMiddleware"/> class.
        /// </summary>
        public frmMiddleware()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the job id.
        /// </summary>
        /// <param name="job_name">The job_name.</param>
        /// <returns>Guid.</returns>
        private Guid GetJobId(string job_name)
        {
            QueryData queryData = new QueryData();
            queryData.Query = "Select job_id from [msdb].[dbo].[sysjobs_view] where name =@job_name";
            queryData.DictionaryDatabaseParameter.Add("@job_name", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@job_name",
                ParameterType = DbType.String,
                ParameterValue = job_name
            });

            object obj = DatabaseManager.DatabaseManager.ExecuteScalar(queryData);

            if (obj != null)
                return new Guid(obj.ToString());

            return Guid.Empty;
        }

        /// <summary>
        /// Job Status Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JobStatusEvent(object sender, JobStatusEventArgs e)
        {
            // string[] Status ={ "Failed", "Succeeded", "Running", "Canceled", "Cancelled" };
            //int[] ImageIndexes = { 144, 230, 229, 118, 118};

            try
            {
                EventHandler<JobStatusEventArgs> handler = JobStatusEvent;
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        this.BeginInvoke(handler, sender, e);
                        return;
                    }

                    ListViewItem[] items = listJobs.Items.Find(e.Name, true);
                    if (items.Length == 0)
                    {
                        ListViewItem item = new ListViewItem(e.Items, -1);
                        item.Name = e.Name;
                        listJobs.Items.Add(item);
                    }
                    else
                    {

                        foreach (ListViewItem item in items)
                        {
                            if (item.SubItems[1].Text == "Succeeded")
                            {
                                item.ImageIndex = 230;
                            }
                            else if (item.SubItems[1].Text == "Failed")
                            {
                                item.ImageIndex = 144;
                            }
                            else if (item.SubItems[1].Text == "Running")
                            {
                                item.ImageIndex = 229;
                            }
                            else if (item.SubItems[1].Text == "Canceled")
                            {
                                item.ImageIndex = 118;
                            }
                            else if (item.SubItems[1].Text == "Unknown")
                            {
                                item.ImageIndex = 182;
                            }
                            else
                            {
                                item.ImageIndex = -1;
                            }

                            item.SubItems[1].Text = e.Status;
                            item.SubItems[2].Text = e.State;
                            item.SubItems[3].Text = e.LastRun.ToString();
                        }
                    }

                    //obsolete
                    lblLastRun.Text = e.LastRun.ToString();
                    lblJobStatus.Text = e.Status;
                    lblState.Text = e.State;
                    pb.Style = e.Status.Equals("Running") ?
                    ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
                }
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// Gets the job async.
        /// </summary>
        private void GetJobAsync()
        {
            try
            {
                //JobStatusEvent(this, null);

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "sp_JobActivityMonitor";

                IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData);
                while (reader.Read())
                {
                    JobInfo Current = null;
                    if ((Current = Jobs.Find(FindJobByName(reader.GetString(0)))) != null)
                    {
                        JobStatusEventArgs e = new JobStatusEventArgs(
                            Current.Description,
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetDateTime(3));

                        JobStatusEvent(null, e);
                    }
                }
            }
            catch (Exception)
            {
                // don't want to log this every nth so we just ignore it
                //Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }

        }

        /// <summary>
        /// Runs the job async.
        /// </summary>
        private void RunJobAsync()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "msdb.dbo.sp_start_job";
                queryData.DictionaryDatabaseParameter.Add("@job_name", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@job_name",
                    ParameterType = DbType.String,
                    ParameterValue = SelectedJob.JobName
                });

                DatabaseManager.DatabaseManager.ExecuteScalar(queryData);
            }
            catch (Exception ex)
            {
                Exception e = new Exception(string.Format("User: {0}\r\n, Error: {1}\r\n", user.CompanyUserID, ex.Message));
                Logger.HandleException(e, LoggingConstants.POLICY_LOGONLY);
                return;
            }
        }

        /// <summary>
        /// Load defined Jobs
        /// </summary>
        private void LoadJobs()
        {
            try
            {
                string query = "select JobName, Description, GroupId from T_MW_Jobs where ABS([Enabled]) = 1";

                QueryData queryData = new QueryData();
                queryData.Query = query;

                IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData);
                while (reader.Read())
                {
                    Guid JobId = GetJobId(reader.GetString(0));
                    JobInfo job = new JobInfo(JobId, reader.GetString(0), reader.GetString(1), reader.GetInt32(2));
                    Jobs.Add(job);
                }
            }
            catch (Exception)
            {
                return;
            }

        }

        /// <summary>
        /// Check to see if there is a Job Definitiona Table
        /// </summary>
        /// <returns></returns>
        private bool JobTableExists()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.Query = "select count(*) from T_MW_Jobs";

                DatabaseManager.DatabaseManager.ExecuteScalar(queryData);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Check to see if Job Activity Monitor Proc Exists
        /// </summary>
        /// <returns></returns>
        private bool JobActivityMonitorExists()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.Query = "select count(*) from sysobjects where type='P' and name='sp_JobActivityMonitor'";
                DatabaseManager.DatabaseManager.ExecuteScalar(queryData);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Check to see if Job Table exists
        /// </summary>
        private void TableCheck()
        {
            try
            {
                if (JobTableExists()) return;

                Assembly assembly = Assembly.GetAssembly(typeof(frmMiddleware));
                Stream stream = assembly.GetManifestResourceStream("Prana.Middleware.Scripts.T_MW_Jobs.sql");

                using (StreamReader reader = new StreamReader(stream))
                {
                    string sql = reader.ReadToEnd();
                    ExecuteScript(null, sql);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Procedure Check
        /// </summary>
        private void ProcedureCheck()
        {
            try
            {
                if (JobActivityMonitorExists()) return;

                Assembly assembly = Assembly.GetAssembly(typeof(frmMiddleware));
                Stream stream = assembly.GetManifestResourceStream("Prana.Middleware.Scripts.spJobActivityMonitor.sql");

                using (StreamReader reader = new StreamReader(stream))
                {
                    string sql = reader.ReadToEnd();
                    ExecuteScript(null, sql);
                }
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// Handles the Load event of the frmMiddleware control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void frmMiddleware_Load(object sender, EventArgs e)
        {
            try
            {
                TableCheck();
                ProcedureCheck();

                LoadJobs();

                //job_id = GetJobId(job_name);
                StatusThread.RunWorkerAsync();
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_MIDDLEWARE_MANAGER);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void SetButtonsColor()
        {
            try
            {
                btnStart.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnStart.ForeColor = System.Drawing.Color.White;
                btnStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnStart.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnStart.UseAppStyling = false;
                btnStart.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClose.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnClose.ForeColor = System.Drawing.Color.White;
                btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClose.UseAppStyling = false;
                btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        /// <summary>
        /// Handles the Click event of the btnStart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (IsJobRunning())
            {
                MessageBox.Show("Only one job can be run at a time.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (worker.IsBusy) return;
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Handles the DoWork event of the worker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            RunJobAsync();
        }

        /// <summary>
        /// Handles the DoWork event of the Status control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void Status_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int refreshInterval;
                int.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey("MiddlewareManagerRefreshInterval"), out refreshInterval);
                if (refreshInterval == 0)
                {
                    refreshInterval = 1000;              //value is defaulted to 1 second
                }
                while (true)
                {
                    if (_isFormClosed)
                    {
                        break;
                    }
                    GetJobAsync();
                    System.Threading.Thread.Sleep(refreshInterval);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Form Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Form Closing Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMiddleware_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, e);
                }
                StatusThread.CancelAsync();
                _isFormClosed = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Creates the tables.
        /// </summary>
        /// <remarks></remarks>
        public void ExecuteScripts(string filter)
        {
            string[] tables = Directory.GetFiles(".\\Scripts", filter);

            foreach (string file in tables)
            {
                ExecuteScript(Path.GetFileName(file), string.Empty);
            }
        }
        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <remarks></remarks>
        public void ExecuteScript(string script, string sql)
        {
            if (sql == string.Empty)
            {
                sql = File.ReadAllText(string.Format(".\\Scripts\\{0}", script));
            }
            string[] datasets = Regex.Split(sql, @"\b" + "GO" + @"\b", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            foreach (string dataset in datasets)
            {
                try
                {
                    QueryData queryData = new QueryData();
                    queryData.Query = dataset;

                    DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void listJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListViewItem item = listJobs.SelectedItems[0];

                SelectedJob = Jobs.Find(FindJobByDescription(item.Name));
                btnStart.Enabled = true;
                lblName.Text = item.Name;
            }
            catch (Exception)
            {
                SelectedJob = null;
                btnStart.Enabled = false;
                lblName.Text = "";
            }
        }

        #region IPluggableTools Members

        /// <summary>
        /// NA
        /// </summary>
        public void SetUP() { }

        /// <summary>
        /// Form Reference (this)
        /// </summary>
        /// <returns></returns>
        public Form Reference()
        {
            return this;
        }

        /// <summary>
        /// Event for Caller to handle close
        /// </summary>
        public event EventHandler PluggableToolsClosed;

        /// <summary>
        /// NA
        /// </summary>
        public ISecurityMasterServices SecurityMaster
        {
            set {; }
        }

        /// <summary>
        /// NA
        /// </summary>
        public IPostTradeServices PostTradeServices
        {
            set {; }
        }

        /// <summary>
        /// NA
        /// </summary>
        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion

        #region ILoginUser Members

        /// <summary>
        /// Current User Info
        /// </summary>
        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            get
            {
                if (user == null)
                    return new Prana.BusinessObjects.CompanyUser();
                else
                    return user;
            }
            set
            {
                user = value;
            }
        }

        #endregion


    }
}