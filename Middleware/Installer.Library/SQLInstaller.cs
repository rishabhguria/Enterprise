using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace Installer.Library
{
    /// <summary>
    /// SQL Installer
    /// </summary>
    /// <remarks></remarks>
    public class SQLInstaller
    {
        public static EventHandler<SqlInfoMessageEventArgs> InfoMessage;
        public static EventHandler<MessageEventArgs> ConsoleMessage;

        public static bool CreateJobs = true;
        public static bool DropExisting = true;
        public static bool CreateTables = true;
        public static bool CreateViews = true;
        public static bool CreateProcedures = true;
        public static bool CreateFunctions = true;

        /// <summary>
        /// Global Transaction
        /// </summary>
        //static SqlTransaction g_trans = null;

        /// <summary>
        /// Initials the session.
        /// </summary>
        /// <remarks></remarks>
        private static void InitialSession()
        {
            try
            {
                File.WriteAllText(".\\Installer.log", "");
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Connects the specified database.
        /// </summary>
        /// <param name="connect">The connect.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static SqlConnection Connect(string connect)
        {
            try
            {
                SqlConnection cn = new SqlConnection(connect);
                cn.Open();

                //g_trans = cn.BeginTransaction();

                cn.InfoMessage += cn_InfoMessage;

                ConnectionStringParser parser = new ConnectionStringParser(connect);
                ConsoleWriteLine("Connected to " + parser.ToString());

                return cn;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ConsoleWriteLine(ex.Message);
                return null;
            }
        }

        private static bool AllowRun(string item)
        {
            if (item == "Tables" && CreateTables) return true;
            if (item == "Procedures" && CreateProcedures) return true;
            if (item == "Views" && CreateViews) return true;
            if (item == "Functions" && CreateFunctions) return true;
            if (item == "Jobs" && CreateJobs) return true;

            return false;
        }
        private static string GetDropStatment(ScriptItem item, string name)
        {
            if (item.DropExisting == false || string.IsNullOrEmpty(item.ScriptName) || item.ScriptType == "TableDelta" || item.ScriptType == "Data") return string.Empty;

            if (String.Compare(item.ScriptType, "Tables", true) == 0)
            {
                string header = string.Format("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]'))\n", name);
                string droptable = string.Format("{0}\nDrop Table {1} GO ", header, name);
                string dropconstraints = "Declare @Sql nvarchar(Max)\n";
                dropconstraints += string.Format("Select @Sql = (Select N'Alter Table ' + Object_Name(Parent_Object_Id) + ' Drop Constraint ' + Object_Name(Constraint_Object_Id) + ';' From Sys.Foreign_Key_Columns Where Object_Name(Referenced_Object_Id) = '{0}' For Xml Path(''))\n", name);
                dropconstraints += "Exec sp_executesql @Sql\n";
                droptable = dropconstraints + droptable;
                return droptable;
            }
            else if (string.Compare(item.ScriptType, "Jobs", true) == 0)
            {
                string itemname = name.Replace(".Job", "");
                string dropjob = "Declare @ExisingJob_Id uniqueidentifier\n";
                dropjob += "Select @ExisingJob_Id = Job_Id From msdb.dbo.sysjobs_view Where [Name] = N'{@item@}.{@database@}'\n";
                dropjob = dropjob.Replace("{@item@}", itemname);
                dropjob += "If @ExisingJob_Id Is Not Null Exec msdb.dbo.sp_delete_job @job_id=@ExisingJob_Id, @delete_unused_schedule=1";
                dropjob += " Go ";
                return dropjob;
            }
            else if (string.Compare(item.ScriptType, "LinkedServers", true) == 0)
            {
                string itemname = name.Replace(".linkedserver", "");
                string dropserver = "IF EXISTS (SELECT srv.name FROM sys.servers srv WHERE srv.server_id != 0 AND srv.name = N'{@item@}')EXEC master.dbo.sp_dropserver @server=N'{@item@}', @droplogins='droplogins'";
                dropserver = dropserver.Replace("{@item@}", itemname);
                dropserver += " Go ";
                return dropserver;
            }
            else if (String.Compare(item.ScriptType, "Functions", true) == 0)
            {
                string header = string.Format("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]'))\n", name);
                return string.Format("{0}\nDrop Function {1} GO ", header, name);
            }
            else if (String.Compare(item.ScriptType, "Procedures", true) == 0)
            {
                string header = string.Format("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]'))\n", name);
                return string.Format("{0}\nDrop Procedure {1} GO ", header, name);
            }
            else if (String.Compare(item.ScriptType, "Views", true) == 0)
            {
                string header = string.Format("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]'))\n", name);
                return string.Format("{0}\nDrop View {1} GO ", header, name);
            }
            else
            {
                string header = string.Format("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]'))\n", name);
                return string.Empty;
            }
        }

        public static bool Execute(string connect, List<ScriptItem> Items)
        {
            ScriptItem Current = null;

            try
            {
                InitialSession();

                SqlConnection db = Connect(connect);
                if (db == null) Environment.Exit(-1);

                ConnectionStringParser parser = new ConnectionStringParser(connect);


                foreach (var item in Items)
                {
                    if (item.Execute == false) continue;
                    Current = item;

                    string path = item.FullPathName; // string.Format(".\\Scripts{0}\\{1}.sql", item.ScriptType, item.ScriptName);

                    if (File.Exists(path))
                    {


                        ExecuteNonQuery(item, Path.GetFileNameWithoutExtension(path), File.ReadAllText(path), db, parser.Database);


                        //string[] files = Directory.GetFiles(path, "*.sql");
                        //files.ToList().ForEach(query => );
                    }

                }

                return true;


            }
            catch (Exception ex)
            {
                Current.Status = ex.Message;
                ConsoleWriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Handles the InfoMessage event of the cn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Data.SqlClient.SqlInfoMessageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        static void cn_InfoMessage(object sender, System.Data.SqlClient.SqlInfoMessageEventArgs e)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            ConsoleWriteLine(e.Message);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="cmdText">The SQL.</param>
        /// <param name="cn">The cn.</param>
        /// <param name="database">The database.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        static bool ExecuteNonQuery(ScriptItem item, string name, string cmdText, SqlConnection cn, string database)
        {
            try
            {
                string[] buffers = database.Split(new string[] { "V1.", "V2.", "V3." }, StringSplitOptions.RemoveEmptyEntries);
                ConnectionStringParser parser = new ConnectionStringParser(cn.ConnectionString);

                Console.ForegroundColor = ConsoleColor.Gray;
                ConsoleWriteLine(string.Format("Executing {0}", name));

                if (string.IsNullOrEmpty(cmdText.Trim())) return false;

                string cmdWhole = GetDropStatment(item, name) + cmdText;
                cmdWhole = cmdWhole.Replace("{@client@}", buffers[0]);
                cmdWhole = cmdWhole.Replace("{@server@}", parser.DataSource);
                cmdWhole = cmdWhole.Replace("{@database@}", database);

                string[] sqls = Regex.Split(cmdWhole, @"\b" + "GO" + @"\b", RegexOptions.Singleline | RegexOptions.IgnoreCase);

                if (cn.State == System.Data.ConnectionState.Closed)
                    cn.Open();

                using (SqlCommand db = new SqlCommand(cmdText, cn))
                {
                    db.StatementCompleted += db_StatementCompleted;

                    foreach (string sql in sqls)
                    {
                        if (string.IsNullOrEmpty(sql)) continue;
                        db.CommandTimeout = 360;
                        db.CommandText = sql;

                        db.ExecuteNonQuery();
                    }
                    db.StatementCompleted -= db_StatementCompleted;
                }

                item.Status = "Complete";
                item.Execute = false;

                return true;

            }
            catch (Exception ex)
            {
                Exception inner = ex;
                while (inner.InnerException != null)
                {
                    inner = inner.InnerException;
                }

                item.Status = inner.Message;

                Console.ForegroundColor = ConsoleColor.Red;
                ConsoleWriteLine(String.Format("{0}  {1}", item.ScriptName, inner.Message));
                return false;
            }
        }

        /// <summary>
        /// Handles the StatementCompleted event of the db control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Data.StatementCompletedEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        static void db_StatementCompleted(object sender, System.Data.StatementCompletedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            ConsoleWriteLine(string.Format("Statement Completed {0} records", e.RecordCount));
        }

        /// <summary>
        /// Consoles the write line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        static void ConsoleWriteLine(string value)
        {
            try
            {
                if (ConsoleMessage != null)
                    ConsoleMessage(null, new MessageEventArgs() { Message = value });

                // File.AppendAllText(".\\Installer.log", string.Format("{0}\t{1}{2}",DateTime.Now, value, Environment.NewLine));
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
