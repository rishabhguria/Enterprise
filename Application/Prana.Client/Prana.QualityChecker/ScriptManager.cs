using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.NirvanaQualityChecker
{
    class ScriptManager
    {
        static String _filepath;
        public static string ClientDatabase;
        public static string SmDatabase;
        public static string ConnectionString;
        public static string SmConnectionString;
        public static string Datasource;
        private static readonly Dictionary<int, String> fundsDictionary = new Dictionary<int, string>();
        public static Dictionary<String, List<Script>> Dictionaryofscripts = new Dictionary<string, List<Script>>();
        public static string RootPath;

        public static Dictionary<int, String> FundsDictionary
        {
            get { return fundsDictionary; }
        }

        public static void ListDirectory(TreeView treeView)
        {
            try
            {
                String path = Application.StartupPath + @"\Quality Checker Scripts\Detection Scripts\";
                treeView.Nodes.Clear();
                var rootDirectoryInfo = new DirectoryInfo(path);
                RootPath = rootDirectoryInfo.Parent.FullName;
                if (!RootPath.EndsWith("\\"))
                    RootPath = RootPath + "\\";
                treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
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


        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode();
            try
            {
                directoryNode = new TreeNode(directoryInfo.Name);
                foreach (var directory in directoryInfo.GetDirectories())
                    directoryNode.Nodes.Add(CreateDirectoryNode(directory));

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return directoryNode;
        }

        public static void ReadScriptsOnPath()
        {
            try
            {

                _filepath = Application.StartupPath + @"\Quality Checker Scripts\Detection Scripts\";

                //ModuleWise
                foreach (String module in Directory.GetDirectories(_filepath, "*.*", SearchOption.AllDirectories))
                {
                    var lsScripts = new List<Script>();
                    var modulename = new DirectoryInfo(module);
                    foreach (String script in Directory.GetFiles(module, "*.sql", SearchOption.TopDirectoryOnly))
                    {
                        var file = new FileInfo(script);

                        String description;
                        using (var reader = new StreamReader(file.FullName))
                            description = reader.ReadLine();


                        if (!description.StartsWith("Description:") && !description.StartsWith("description:"))
                        {
                            description = "";
                        }
                        else
                        {
                            description = description.Substring(description.IndexOf("'") + 1, (description.LastIndexOf("'") - description.IndexOf("'") - 1));

                        }

                        lsScripts.Add(new Script(file.Name.Replace(".sql", ""), file.FullName, modulename.Name));//, description));

                    }
                    if (!Dictionaryofscripts.ContainsKey(modulename.FullName))
                        Dictionaryofscripts.Add(modulename.FullName, lsScripts);
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

        internal static void GetCompanyAccounts()
        {
            try
            {
                foreach (var account in CachedDataManager.GetInstance.GetAccountsWithFullName())
                {
                    fundsDictionary.Add(account.Key, account.Value);
                }
            }
            catch (Exception ex)
            {
                QualityCheck.IsConnected = 0;
                Logger.LoggerWrite(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "Table 'T_CompanyFunds' not found. Please make sure you have selected a correct Database.");
                //Log.Error("Table 'T_CompanyFunds' not found. Please make sure you have selected a correct Database.", ex);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Need to load sql query for Quality checker")]
        public static bool ScriptsExecute(List<Script> selectedScripts, DateTime FromDate, DateTime Todate, String commaseperatedSymbol, String commaseperatedFunds)
        {
            try
            {
                string databaseUsed = string.Empty;
                foreach (Script script in selectedScripts)
                {
                    if (script.SelectScript)
                    {
                        var sql = new StringBuilder(File.ReadAllText(script.FullScriptPath));

                        sql.Replace(@"set @FromDate=''", @"set @FromDate='" + FromDate + "'");
                        sql.Replace(@"set @ToDate=''", @"set @ToDate='" + Todate + "'");
                        sql.Replace(@"set @FundIds=''", @"set @FundIds='" + commaseperatedFunds + "'");
                        sql.Replace(@"set @Symbols=''", @"set @Symbols='" + commaseperatedSymbol + "'");
                        sql.Replace(@"set @Smdb=''", @"set @Smdb='" + SmDatabase + "'");

                        SqlConnection conn = null;
                        if (sql.ToString().Contains("--Work on SMDB"))
                        {
                            conn = new SqlConnection(SmConnectionString);
                            databaseUsed = SmDatabase;
                        }
                        else
                        {
                            conn = new SqlConnection(ConnectionString);
                            databaseUsed = ClientDatabase;
                        }

                        conn.Open();
                        using (var cmd = new SqlCommand(sql.ToString(), conn))
                        {
                            cmd.CommandTimeout = 30000;
                            var resultSet = new DataSet();
                            try
                            {
                                using (var da = new SqlDataAdapter(cmd))
                                {
                                    da.Fill(resultSet);
                                    if (resultSet.Tables.Count > 0)
                                    {
                                        script.DatabaseResult = resultSet;
                                    }
                                    else
                                    {
                                        var dt = new DataTable();
                                        dt.Columns.Add("ErrorMsg", typeof(string));
                                        dt.Rows.Add("-");
                                        resultSet.Tables.Add(dt);
                                        script.DatabaseResult = resultSet;
                                    }
                                }
                                Logger.LoggerWrite(script.ErrorMessage, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, string.Empty, new Dictionary<string, object>(){
                                    {"ModuleName", script.ModuleName},
                                    {"ScriptName", script.ScriptName},
                                    {"DatabaseUsed", databaseUsed}
                                });
                            }
                            catch (Exception ex)
                            {
                                Logger.LoggerWrite(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "Script failure. Error running it.", new Dictionary<string, object>(){
                                    {"ModuleName", script.ModuleName},
                                    {"ScriptName", script.ScriptName},
                                    {"DatabaseUsed", databaseUsed}
                                });
                                var dt = new DataTable();
                                dt.Columns.Add("ErrorMsg", typeof(string));
                                dt.Rows.Add("Script failure. Error running it.");
                                resultSet.Tables.Add(dt);
                                script.DatabaseResult = resultSet;
                            }
                            conn.Close();
                        }

                    }
                }


            }
            catch (Exception e)
            {
                bool rethrow = Logger.HandleException(e, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        internal static void SetUpDataSource()
        {
            try
            {
                FundsDictionary.Clear();
                ConnectionString = ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString;
                SmConnectionString = ConfigurationManager.ConnectionStrings["SMConnectionString"].ConnectionString;

                var firstOrDefault = ConnectionString.Split(';').FirstOrDefault();
                if (firstOrDefault != null)
                    ClientDatabase = firstOrDefault.Split('=').LastOrDefault();
                var orDefault = SmConnectionString.Split(';').FirstOrDefault();
                if (orDefault != null)
                    SmDatabase = orDefault.Split('=').LastOrDefault();

                Datasource = ConnectionString.Split(';').FirstOrDefault(s => s.Contains("Server"));
                GetCompanyAccounts();
            }
            catch (Exception e)
            {
                bool rethrow = Logger.HandleException(e, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //private static string GetDataBaseName(string splitAfter, string connectionString)
        //{
        //    string returnString = String.Empty;
        //    try
        //    {
        //        returnString = connectionString.Substring(splitAfter.Length + 1, connectionString.IndexOf(';'));
        //    }
        //    catch (Exception e)
        //    {
        //        bool rethrow = Logger.HandleException(e, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return returnString;
        //}
    }
}
