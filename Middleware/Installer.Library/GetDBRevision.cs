using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace Installer.Library
{
    public class GetDBRevision
    {
        public static EventHandler<MessageEventArgs> ConsoleMessage;
        public static System.Data.DataTable dataTable { get; set; }

        /// <summary>
        /// Gets the dictionary of sqlfilename-revsions from the text file
        /// </summary>
        /// <param name="filePath">Complete absolute path of the file</param>
        /// <returns>dictionary of type string,long</returns>
        public static Dictionary<string, long> getSQLRevsionsFromFile(string filePath)
        {
            Dictionary<string, long> sqlScriptRevisions = new Dictionary<string, long>();
            try
            {
                if (File.Exists(filePath))
                {
                    string[] scriptRevisions = File.ReadAllLines(filePath);
                    foreach (string s in scriptRevisions)
                    {
                        if (!string.IsNullOrWhiteSpace(s.Trim()))
                        {
                            long rev;
                            if (long.TryParse(s.Trim().Split(',')[1], out rev))
                                sqlScriptRevisions.Add(s.Trim().Split(',')[0], long.Parse(s.Trim().Split(',')[1]));
                            else
                            {
                                ConsoleWriteLine("The file ScriptVersion.csv contains invalid revisions or data which can not be parsed.");
                                sqlScriptRevisions.Clear();
                                return sqlScriptRevisions;
                            }
                        }
                    }
                }
                return sqlScriptRevisions;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ConsoleWriteLine(ex.Message);
                return sqlScriptRevisions;
            }
        }

        public static long GetDBVersion(string connect)
        {
            try
            {
                SqlConnection db = Connect(connect);
                if (db == null) return long.MaxValue;
                ConnectionStringParser parser = new ConnectionStringParser(connect);
                string query = "select * from T_DBVersion";
                SqlCommand cmd = new SqlCommand(query, db);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dataTable = new DataTable();
                try
                {
                    da.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    ConsoleWriteLine("The table T_DBVersion does not exists in db please review");
                    return long.MaxValue;
                }
                db.Close();
                da.Dispose();
                long rev;
                if (dataTable.Rows.Count != 0)
                {
                    for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
                    {
                        if ((bool)dataTable.Rows[i][1] == true)
                        {
                            if (long.TryParse(dataTable.Rows[i][0].ToString(), out rev))
                                return rev;
                            else
                            {
                                ConsoleWriteLine("The table T_DBVersion contains values in revision column which are not number");
                                return long.MaxValue;
                            }
                        }
                    }
                }
                ConsoleWriteLine("No revision in table T_DBVersion with middleware installation true");
                return long.MaxValue;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ConsoleWriteLine(ex.Message);
                return long.MaxValue;
            }
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





        
    }
}
