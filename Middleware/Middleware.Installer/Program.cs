using System.Windows.Forms;

namespace Middleware.Installer
{
    /// <summary>
    /// Middleware.Installer Main Entry
    /// </summary>
    /// <remarks></remarks>
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());         

        }
        static void MainConsole(string[] args)
        {
            //Console.Title = "Middleware.Installer";

            //string cn = global::Middleware.Installer.Properties.Settings.Default.ConnectionString;

            //ConnectionStringParser parser = new ConnectionStringParser(cn);

            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("Setup will install scripts for the following connection\n");

            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.WriteLine(parser.ToString());

            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine("\n\nPress any key to begin the installation.");
            //Console.ReadKey();
                     

            //Console.ForegroundColor = ConsoleColor.Gray;

            //if (global::Middleware.Installer.Properties.Settings.Default.UseSQLInstaller)
            //    SQLInstaller.Execute(cn);
            //else
            //    SMOInstaller.Execute(cn);
            
            //System.Threading.Thread.Sleep(10000);

        }    
    }
}
