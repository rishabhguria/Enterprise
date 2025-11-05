using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Installer.Library;

namespace Middleware.Installer
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class frmMain : Form
    {
        /// <summary>
        /// Gets or sets the active grid view.
        /// </summary>
        /// <value>The active grid view.</value>
        /// <remarks></remarks>
        DataGridView ActiveGridView { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        /// <remarks></remarks>
        string DatabaseName { get; set; }

        public static Dictionary<string, long> sqlScriptVersion { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Forms.Form"/> class.
        /// </summary>
        /// <remarks></remarks>
        public frmMain()
        {
            InitializeComponent();
            LoadScripts();
            sqlScriptVersion = GetDBRevision.getSQLRevsionsFromFile(System.Windows.Forms.Application.StartupPath + "\\ScriptVersion.csv");
        }

        /// <summary>
        /// Loads the scripts.
        /// </summary>
        /// <remarks></remarks>
        public void LoadScriptsFromFile(long revision)
        {
            //ListDirectoryFromFile("?_MW_*", dgm, revision, ".\\Scripts\\Middleware");
            ListDirectoryFromFile("?_W_*", dgt, revision, ".\\Scripts\\Touch");
            ListDirectoryFromFile("?_W_*", dgg, revision, ".\\Scripts\\Gateway");
            ListDirectoryFromFile("?_NT_*", dgNT, revision, ".\\Scripts\\NewTouch");
            ListDirectoryFromFile("*.Job.*", dgj, revision, ".\\Scripts\\Middleware", ".\\Scripts\\Touch", ".\\Scripts\\Gateway");
            //ListDirectoryFromFile("*.linkedserver.*", dgl, revision, ".\\Scripts\\Middleware", ".\\Scripts\\Touch", ".\\Scripts\\Gateway");
        }

        /// <summary>
        /// Lists the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="view">The view.</param>
        /// <remarks></remarks>
        private static void ListDirectoryFromFile(string filter, DataGridView view, long revision, params string[] paths)
        {
            List<string> Orders = new List<string> { "LinkedServers", "Tables", "TableDelta", "Functions", "Procedures", "Views", "Data", "Jobs" };

            List<ScriptItem> Items = new List<ScriptItem>();

            foreach (string path in paths)
            {
                List<string> files = Directory.GetFiles(path, filter, SearchOption.AllDirectories).ToList();

                foreach (var file in files)
                {

                    string name = Path.GetFileNameWithoutExtension(file);
                    string folder = Path.GetDirectoryName(file).Replace(path + "\\", "");
                    string pathFolder = Path.GetFileName(path);
                    int order = 0;

                    for (order = 0; order < 10; order++)
                    {
                        if (folder.Contains(Orders[order]))
                            break;
                    }
                    if (sqlScriptVersion.ContainsKey(pathFolder + "-" + folder + "-" + name))
                    {
                        if (sqlScriptVersion[pathFolder + "-" + folder + "-" + name] > revision)
                        {
                            if (folder.Contains(Orders[1]) || folder.Contains(Orders[2]))
                                Items.Add(new ScriptItem() { Execute = true, DropExisting = false, ScriptName = name, ScriptType = Orders[order], Order = order, FullPathName = file });
                            else
                                Items.Add(new ScriptItem() { Execute = true, DropExisting = true, ScriptName = name, ScriptType = Orders[order], Order = order, FullPathName = file });
                        }
                        else
                            Items.Add(new ScriptItem() { Execute = false, DropExisting = false, ScriptName = name, ScriptType = Orders[order], Order = order, FullPathName = file });
                    }
                    else
                        Items.Add(new ScriptItem() { Execute = false, DropExisting = false, ScriptName = name, ScriptType = Orders[order], Order = order, FullPathName = file });
                    VerifyScript(Items.Last());
                }
            }

            Items.Sort(new OrderByNaturalSort());
            Items.Sort(new OrderByDependency());

            view.DataSource = Items;
            view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            view.Columns["Order"].Visible = false;
            view.Columns["FullPathName"].Visible = false;

            view.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }


        /// <summary>
        /// Loads the scripts.
        /// </summary>
        /// <remarks></remarks>
        public void LoadScripts()
        {
            //ListDirectory("?_MW_*", dgm, ".\\Scripts\\Middleware");
            ListDirectory("?_W_*", dgt, ".\\Scripts\\Touch");
            ListDirectory("?_W_*", dgg, ".\\Scripts\\Gateway");
            ListDirectory("?_NT_*", dgNT, ".\\Scripts\\NewTouch");

            ListDirectory("*.Job.*", dgj, ".\\Scripts\\Touch", ".\\Scripts\\Gateway");

           // ListDirectory("*.linkedserver.*", dgl,  ".\\Scripts\\Touch", ".\\Scripts\\Gateway");
        }

        /// <summary>
        /// Lists the directory.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="path">The path.</param>
        /// <remarks></remarks>
        private static void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();

            var stack = new Stack<TreeNode>();
            var rootDirectory = new DirectoryInfo(path);
            var node = new TreeNode(rootDirectory.Name) { Tag = rootDirectory };
            stack.Push(node);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                var directoryInfo = (DirectoryInfo)currentNode.Tag;
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    var childDirectoryNode = new TreeNode(directory.Name) { Tag = directory };
                    currentNode.Nodes.Add(childDirectoryNode);
                    stack.Push(childDirectoryNode);
                }
                foreach (var file in directoryInfo.GetFiles())
                    currentNode.Nodes.Add(new TreeNode(file.Name, 1, 2));
            }

            treeView.Nodes.Add(node);
        }

        /// <summary>
        /// Lists the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="view">The view.</param>
        /// <remarks></remarks>
        private static void ListDirectory(string filter, DataGridView view, params string[] paths)
        {
            List<string> Orders = new List<string> { "LinkedServers", "Tables", "TableDelta", "Functions", "Procedures", "Views", "Data", "Jobs" };

            List<ScriptItem> Items = new List<ScriptItem>();

            foreach (string path in paths)
            {

                List<string> files = Directory.GetFiles(path, filter, SearchOption.AllDirectories).ToList();

                foreach (var file in files)
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    string folder = Path.GetDirectoryName(file).Replace(path + "\\", "");
                    string pathFolder = Path.GetFileName(path);

                    int order = 0;

                    for (order = 0; order < 10; order++)
                    {
                        if (folder.Contains(Orders[order]))
                            break;
                    }

                    Items.Add(new ScriptItem() { Execute = true, DropExisting = true, ScriptName = name, ScriptType = Orders[order], Order = order, FullPathName = file });
                    VerifyScript(Items.Last());
                }
            }

            Items.Sort(new OrderByNaturalSort());
            Items.Sort(new OrderByDependency());

            view.DataSource = Items;
            view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            view.Columns["Order"].Visible = false;
            view.Columns["FullPathName"].Visible = false;

            view.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        static void VerifyScript(ScriptItem item)
        {
            try
            {
                string buffer = File.ReadAllText(item.FullPathName).ToUpper();
                int create;
                if (String.Compare(item.ScriptType, "Tables", true) != 0)
                    create = buffer.IndexOf(string.Format("CREATE {0} [", item.ScriptType.ToUpper()));
                else
                    create = buffer.IndexOf("Create Table [");

                if (create > -1)
                {
                    if (buffer.Substring(0, create).IndexOf("USE [") > 0)
                    {
                        MessageBox.Show(String.Format("The sql contains a USE statement. Please verify script {0} before running the installer", item.ScriptName), item.FullPathName);
                    }
                }


                //if (buffer.IndexOf(string.Format("ALTER {0} [", item.ScriptType.ToUpper())) > -1)            
                //{                
                //    MessageBox.Show(String.Format("The sql contains a ALTER statement. Please verify script {0} before running the installer", item.ScriptName), item.FullPathName);                
                //}
            }
            catch (Exception)
            {
                return;
            }
        }
        /// <summary>
        /// Handles the Load event of the frmMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void frmMain_Load(object sender, EventArgs e)
        {
            SQLInstaller.ConsoleMessage += ConsoleMessage;
            GetDBRevision.ConsoleMessage += ConsoleMessage;
        }

        /// <summary>
        /// Consoles the message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Installer.Library.MessageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        void ConsoleMessage(object sender, MessageEventArgs e)
        {
            EventHandler<MessageEventArgs> handler = ConsoleMessage;

            if (this.InvokeRequired)
            {
                Invoke(handler, sender, e);
                return;
            }
            txtLog.Text += e.Message + Environment.NewLine;

            LogMessage(sender, e);

            
            //pbar.Value = GetCompleted(dgj, dgm, dgt);
            //RefreshViews(dgj, dgt, dgm);

            //TODO need to check why others tab not refershing
            pbar.Value = GetCompleted(dgj, dgt);
            RefreshViews(dgj, dgt);

            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();

        }

        void LogMessage(object sender, MessageEventArgs e)
        {
            EventHandler<MessageEventArgs> handler = LogMessage;

            if (this.InvokeRequired)
            {
                Invoke(handler, sender, e);
                return;
            }

            File.AppendAllText(".\\Installer.log", String.Format("{0}\t{1}\n", DateTime.Now, e.Message));

        }

        /// <summary>
        /// Gets the completed.
        /// </summary>
        /// <param name="views">The views.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int GetCompleted(params DataGridView[] views)
        {
            int done = 0;
            foreach (var view in views)
            {
                var items = (List<ScriptItem>)view.DataSource;
                done += items.Where(w => string.IsNullOrEmpty(w.Status) == false && w.Execute == true).Count();

            }
            return done;
        }
        /// <summary>
        /// Adds the ranges.
        /// </summary>
        /// <param name="views">The views.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<ScriptItem> AddRanges(params DataGridView[] views)
        {
            List<ScriptItem> Items = new List<ScriptItem>();
            foreach (var view in views)
            {
                var items = (List<ScriptItem>)view.DataSource;
                Items.AddRange(items);
            }
            return Items;
        }
        /// <summary>
        /// Refreshes the views.
        /// </summary>
        /// <param name="views">The views.</param>
        /// <remarks></remarks>
        public static void RefreshViews(params DataGridView[] views)
        {
            foreach (var view in views)
            {
                view.Refresh();
            }
        }
        /// <summary>
        /// Handles the Click event of the btnExecute control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnExecute_Click(object sender, EventArgs e)
        {

            dgj.Update();
            //dgl.Update();
           // dgm.Update();
            dgt.Update();
            dgg.Update();
            dgNT.Update();

            if (MessageBox.Show("Are you sure you wish to upgrade the following database?", "Upgrade",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                //List<ScriptItem> Items = AddRanges(dgl, dgj, dgm, dgt, dgg, dgNT);
                List<ScriptItem> Items = AddRanges(dgj, dgt, dgg, dgNT);
                Items.Sort(new OrderByNaturalSort());
                Items.Sort(new OrderByDependency());

                pbar.Visible = true;
                pbar.Maximum = Items.Where(w => w.Execute == true).Count() + 1;


                File.WriteAllText(".\\Installer.log", "");

                Task.Factory.StartNew(() => ExecuteAsync(this, Items));

                //pbar.Value = 1;
                //pbar.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the toolStripButton1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Executes the async.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="Items">The items.</param>
        /// <remarks></remarks>
        private static void ExecuteAsync(frmMain app, List<ScriptItem> Items)
        {
            app.ConsoleMessage(null, new MessageEventArgs() { Message = "Setup will install scripts for the following connection\n" });


            if (global::Middleware.Installer.Properties.Settings.Default.UseSQLInstaller)
                SQLInstaller.Execute(app.txtConnectionString.Text, Items);
            else
                SMOInstaller.Execute(app.txtConnectionString.Text);

            if (app.InvokeRequired)
            {
                EventHandler<EventArgs> handler = OnComplete;
                app.Invoke(handler, app, EventArgs.Empty);
            }

            MessageBox.Show("Setup complete");
        }

        /// <summary>
        /// Called when [complete].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        public static void OnComplete(object sender, EventArgs e)
        {
            frmMain main = sender as frmMain;
            main.pbar.Value = 0;
            RefreshViews(main.dgj,  main.dgt);
            //RefreshViews(main.dgj, main.dgm, main.dgt);
        }
        /// <summary>
        /// Handles the Click event of the btnConnect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            using (frmLogin login = new frmLogin())
            {
                if (login.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtConnectionString.Text = login.ConnectString;
                    DatabaseName = login.cbDatabase.Text;

                    Text = string.Format("Installer - [{0}].[{1}]", login.cbServer.Text, login.cbDatabase.Text);
                    btnExecute.Visible = true;
                    //btnInstallRuntime.Visible = true;
                    if (sqlScriptVersion.Count > 0)
                    {
                        long revision = GetDBRevision.GetDBVersion(login.ConnectString);
                        if (revision != 0)
                        {
                            LoadScriptsFromFile(revision);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Installs the runtime.
        /// </summary>
        /// <remarks></remarks>
        private void InstallRuntime()
        {
            string root = "C:\\Nirvana";
            string dbPath = string.Format("{0}\\{1}", root, DatabaseName);
            string path = string.Format("{0}\\{1}\\Middleware", root, DatabaseName);
            try
            {
                if (Directory.Exists(dbPath) == false)
                    Directory.CreateDirectory(root);


                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                else if (Directory.Exists(path))
                {
                    if (MessageBox.Show("Warning the Runtime folder {0} already exists. Do you wish to overwrite files and configurations settings?", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                        return;

                }

                string config = File.ReadAllText(".\\runtime\\Nirvana.Middleware.Template.config");

                config = string.Format(config, txtConnectionString.Text);
                File.WriteAllText(string.Format("{0}\\CSBatch.exe.config", path), config);
                File.WriteAllText(string.Format("{0}\\CSBatchUI.exe.config", path), config);

                var files = Directory.GetFiles(".\\runtime").ToList();

                foreach (var file in files)
                {
                    string dest = string.Format("{0}\\{1}", path, Path.GetFileName(file));
                    File.Copy(file, dest, true);
                }

                if (File.Exists(".\\runtime\\RollNightly.cmd"))
                {
                    string value = File.ReadAllText(".\\runtime\\RollNightly.cmd");

                    value = value.Replace("{@database@}", DatabaseName);
                    File.WriteAllText(string.Format("{0}\\RollNightly.cmd", path), value);

                }
                MessageBox.Show("Setup complete", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Can't copy runtime files", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Handles the Click event of the btnInstallRuntime control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnInstallRuntime_Click(object sender, EventArgs e)
        {
            InstallRuntime();
        }

        /// <summary>
        /// Handles the Click event of the toolStripButton2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Handles the Click event of the exitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Handles the Resize event of the frmMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void frmMain_Resize(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Click event of the tabControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void tabControl1_Click(object sender, EventArgs e)
        {
            //if (tabControl1.TabPages[tabControl1.SelectedIndex].Text == "Middleware")
            //    ActiveGridView = dgm;
            //else 
            if (tabControl1.TabPages[tabControl1.SelectedIndex].Text == "Touch")
                ActiveGridView = dgt;
            else if (tabControl1.TabPages[tabControl1.SelectedIndex].Text == "New Touch")
                ActiveGridView = dgNT;
            else if (tabControl1.TabPages[tabControl1.SelectedIndex].Text == "Gateway")
                ActiveGridView = dgg;
           // else if (tabControl1.TabPages[tabControl1.SelectedIndex].Text == "Linked Servers")
              //  ActiveGridView = dgl;
            else if (tabControl1.TabPages[tabControl1.SelectedIndex].Text == "SQL Jobs")
                ActiveGridView = dgj;

            statusStrip1.Items[0].Text = tabControl1.TabPages[tabControl1.SelectedIndex].Text;
        }

        /// <summary>
        /// Handles the Click event of the toggleMiddlewareToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void toggleMiddlewareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveGridView == null) ActiveGridView = dgt;
            List<ScriptItem> Items = ActiveGridView.DataSource as List<ScriptItem>;

            foreach (var item in Items)
            {
                item.Execute = true;
            }
            ActiveGridView.Refresh();
        }

        /// <summary>
        /// Handles the Click event of the toggleTouchToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void toggleTouchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveGridView == null) ActiveGridView = dgt;
            List<ScriptItem> Items = ActiveGridView.DataSource as List<ScriptItem>;

            foreach (var item in Items)
            {
                item.Execute = false;
            }
            ActiveGridView.Refresh();
        }

        /// <summary>
        /// Handles the Click event of the checkSelectionToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void checkSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveGridView == null) ActiveGridView = dgt;
            List<ScriptItem> Items = ActiveGridView.DataSource as List<ScriptItem>;

            foreach (DataGridViewRow row in ActiveGridView.SelectedRows)
            {
                row.Cells["Execute"].Value = false;
            }
            ActiveGridView.Refresh();
        }

        /// <summary>
        /// Handles the Click event of the unCheckSelectionToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void unCheckSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveGridView == null) ActiveGridView = dgt;
            List<ScriptItem> Items = ActiveGridView.DataSource as List<ScriptItem>;

            foreach (DataGridViewRow row in ActiveGridView.SelectedRows)
            {
                row.Cells["Execute"].Value = false;
            }
            ActiveGridView.Refresh();
        }

        private void GridView_MouseLeave(object sender, EventArgs e)
        {
            DataGridView view = sender as DataGridView;


            view.EndEdit();
        }

        private void GridVIew_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView view = sender as DataGridView;

            string s = view.Columns[e.ColumnIndex].Name;

            if (s.Equals("Execute") || s.Equals("DropExisting"))
            {
                return;
            }
            e.Cancel = true;
        }


    }

}
