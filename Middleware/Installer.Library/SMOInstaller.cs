using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.SqlServer.Management.Smo;

namespace Installer.Library
{   
    /// <summary>
    ///  SMO Installer
    /// </summary>
    /// <remarks></remarks>
    public class SMOInstaller
    {
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
        private static Database Connect(string connect)
        {
            try
            {
                ConnectionStringParser parser = new ConnectionStringParser(connect);

                Microsoft.SqlServer.Management.Common.ServerConnection cn =
                       new Microsoft.SqlServer.Management.Common.ServerConnection { ConnectionString = connect };

                Server server = new Server(cn);
                cn.ServerMessage += cn_ServerMessage;
                cn.InfoMessage += cn_InfoMessage;
                
                ConsoleWriteLine(parser.ToString());

                return server.Databases[parser.Database];
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                ConsoleWriteLine(ex.Message);
                return null;
            }
        }
      
        /// <summary>
        /// Installs the specified connect.
        /// </summary>
        /// <param name="connect">The connect.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool Execute(string connect)
        {
            try
            {
                InitialSession();
           
                Database db = Connect(connect);
                if (db == null) Environment.Exit(-1);

                if (File.Exists(".\\Scripts\\Presetup.sql"))
                {
                    ExecuteNonQuery(File.ReadAllText(".\\Scripts\\PreSetup.sql"), db);
                }
                string[] Items = { "Tables", "TableDelta", "Views", "Procedures", "Functions", "Data", "Jobs" };
                foreach (var item in Items)
                {
                    string path = string.Format(".\\Scripts\\{0}", item);
                    if (Directory.Exists(path))
                    {
                        string[] files = Directory.GetFiles(path, "*.sql");
                        files.ToList().ForEach(query => ExecuteNonQuery(File.ReadAllText(query), db));
                    }
                }

                if (File.Exists(".\\Scripts\\PostSetup.sql"))
                {
                    ExecuteNonQuery(File.ReadAllText(".\\Scripts\\PostSetup.sql"), db);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
        /// Handles the ServerMessage event of the cn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.SqlServer.Management.Common.ServerMessageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        static void cn_ServerMessage(object sender, Microsoft.SqlServer.Management.Common.ServerMessageEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(string.Format("{0}\n", e.Error));
            ConsoleWriteLine(string.Format("{0}\n", e.Error));
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="db">The db.</param>
        /// <remarks></remarks>
        static void ExecuteNonQuery(string sql, Database db)
        {
            try
            {
                sql = sql.Replace("{@database@}", db.Name);
                string[] buffers = db.Name.Split(new string[] { "V1.", "V2.", "V3." }, StringSplitOptions.RemoveEmptyEntries);
                sql = sql.Replace("{@client@}", buffers[0]);                                         
                db.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Exception inner = ex;
                while (inner.InnerException != null)
                {
                    inner = inner.InnerException;
                }
             
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(inner.Message);

                ConsoleWriteLine(inner.Message);
                return;
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
                File.AppendAllText(".\\Installer.log", string.Format("{0}", value));
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
