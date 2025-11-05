using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Global.Utilities
{
    /// <summary>
    /// Static class that returns the list of processes and the ports those processes use.
    /// </summary>
    public static class ProcessPorts
    {
        public static bool IsPortNumberInUse(List<int> ports)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                bool isAvailable = false;

                foreach (var portNumber in ports)
                {
                    isAvailable = IsPortAvailableToUse(portNumber);   //if even one port is unavailable then break
                    if (isAvailable == false)
                        break;
                }

                if (isAvailable)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Port check completed in " + sw.ElapsedMilliseconds + " ms");
                    return false;
                }


                //confirm if the port is already in use by some process and log that process name, and id and protocol.
                List<ProcessPort> portsInUse = GetNetStatPorts();
                foreach (int portNumber in ports)
                {
                    ProcessPort processPort = portsInUse.Find(x => x.PortNumber == portNumber);

                    if (processPort != null && processPort.Status != "TIME_WAIT" && processPort.Status != "CLOSE_WAIT")
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(string.Format("Port ({0}) already used by Process: {1}, ID: {2}, Protocol: {3}", processPort.PortNumber, processPort.ProcessName, processPort.ProcessId, processPort.Protocol));
                        Console.WriteLine(string.Format("Port ({0}) already used by Process: {1}, ID: {2}, Protocol: {3}", processPort.PortNumber, processPort.ProcessName, processPort.ProcessId, processPort.Protocol));

                        return true;
                    }
                }
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Port check completed in " + sw.ElapsedMilliseconds + " ms through GetNetStatPorts");
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

            return false;
        }

        /// <summary>
        /// This method distills the output from netstat -a -n -o into a list of ProcessPorts that provide a mapping between
        /// the process (name and id) and the ports that the process is using.
        /// </summary>
        /// <returns></returns>
        private static List<ProcessPort> GetNetStatPorts()
        {
            var sm = Stopwatch.StartNew();

            List<ProcessPort> ProcessPorts = new List<ProcessPort>();

            try
            {
                using (Process Proc = new Process())
                {
                    ProcessStartInfo StartInfo = new ProcessStartInfo();
                    StartInfo.FileName = "netstat.exe";
                    StartInfo.Arguments = "-a -n -o";
                    //StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    StartInfo.UseShellExecute = false;
                    StartInfo.RedirectStandardInput = true;
                    StartInfo.RedirectStandardOutput = true;
                    StartInfo.RedirectStandardError = true;
                    StartInfo.CreateNoWindow = true;
                    Proc.StartInfo = StartInfo;
                    Proc.Start();

                    StreamReader StandardOutput = Proc.StandardOutput;
                    StreamReader StandardError = Proc.StandardError;

                    string NetStatContent = StandardOutput.ReadToEnd() + StandardError.ReadToEnd();
                    string NetStatExitStatus = Proc.ExitCode.ToString();

                    if (NetStatExitStatus != "0")
                    {
                        Logger.HandleException(new Exception("NetStat command failed.This may require elevated permissions."), LoggingConstants.POLICY_LOGONLY);
                    }

                    string[] NetStatRows = Regex.Split(NetStatContent, "\r\n");

                    foreach (string NetStatRow in NetStatRows)
                    {
                        string[] Tokens = Regex.Split(NetStatRow, "\\s+");
                        if (Tokens.Length > 4 && (Tokens[1].Equals("UDP") || Tokens[1].Equals("TCP")))
                        {
                            string IpAddress = Regex.Replace(Tokens[2], @"\[(.*?)\]", "1.1.1.1");
                            try
                            {
                                ProcessPorts.Add(new ProcessPort(
                                    Tokens[1] == "UDP" ? GetProcessName(Convert.ToInt32(Tokens[4])) : GetProcessName(Convert.ToInt32(Tokens[5])),
                                    Tokens[1] == "UDP" ? Convert.ToInt32(Tokens[4]) : Convert.ToInt32(Tokens[5]),
                                    IpAddress.Contains("1.1.1.1") ? String.Format("{0}v6", Tokens[1]) : String.Format("{0}v4", Tokens[1]),
                                    Convert.ToInt32(IpAddress.Split(':')[1]),
                                     Tokens[1] == "UDP" ? "NoStatus" : Tokens[4].ToString()
                                ));
                            }
                            catch
                            {
                                Logger.HandleException(new Exception("Could not convert the following NetStat row to a Process to Port mapping."), LoggingConstants.POLICY_LOGONLY);
                                Logger.HandleException(new Exception(NetStatRow.ToString()), LoggingConstants.POLICY_LOGONLY);
                            }
                        }
                        else
                        {
                            if (!NetStatRow.Trim().StartsWith("Proto") && !NetStatRow.Trim().StartsWith("Active") && !String.IsNullOrWhiteSpace(NetStatRow))
                            {
                                Logger.HandleException(new Exception("Unrecognized NetStat row to a Process to Port mapping."), LoggingConstants.POLICY_LOGONLY);
                                Logger.HandleException(new Exception(NetStatRow.ToString()), LoggingConstants.POLICY_LOGONLY);
                            }
                        }
                    }
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

            LogAndDisplayOnInformationReporter
                .GetInstance
                .WriteAndDisplayOnInformationReporter("GetNetStatPorts method has been completed in " + sm.ElapsedMilliseconds + " ms");
            return ProcessPorts;
        }

        /// <summary>
        /// Private method that handles pulling the process name (if one exists) from the process id.
        /// </summary>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        private static string GetProcessName(int ProcessId)
        {
            string procName = "UNKNOWN";
            try
            {
                procName = Process.GetProcessById(ProcessId).ProcessName;
            }
            catch (Exception ex)
            {
                // I used LogOnly here as when some process was running earlier(when netstat command gathered all the processes) but stopped after that,
                // so here it will try to get process ID but that will give error "No Process with id ABC is running", which would not affect us in any way,
                // so I am just logging that in spite of showing or throwing that back and returning the process name as "Unknown" in that case.
                Logger.LoggerWrite(ex.Message, LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
            }
            return procName;
        }



        private static bool IsPortAvailableToUse(int port)
        {
            return TryBindTcpV4(port) || TryBindTcpV6(port);   //Because usually we need the port free in at least one family to bind the listener for for WCF, Windows Services, general socket servers) 
        }
        private static bool TryBindTcpV4(int port)
        {
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Server.ExclusiveAddressUse = true;
                listener.Start();
                return true;
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Port {port} is already in use for TcpV4 ");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Port {port} is already in use for TcpV4 in general exception. Ex:" + ex.Message);
                return false;
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

        private static bool TryBindTcpV6(int port)
        {
            Socket s = null;
            try
            {
                s = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp)
                {
                    ExclusiveAddressUse = true,
                    DualMode = true
                };
                s.Bind(new IPEndPoint(IPAddress.IPv6Any, port));
                return true;
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Port {port} is already in use for TcpV6 ");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Port {port} is already in use for TcpV6 in general exception. Ex:" + ex.Message);
                return false;
            }
            finally
            {
                if (s != null)
                    s.Close();
            }
        }
    }

    /// <summary>
    /// A mapping for processes to ports and ports to processes that are being used in the system.
    /// </summary>
    public class ProcessPort
    {
        private string _ProcessName = String.Empty;
        private int _ProcessId = 0;
        private string _Protocol = String.Empty;
        private int _PortNumber = 0;
        private string _status = string.Empty;

        /// <summary>
        /// Internal constructor to initialize the mapping of process to port.
        /// </summary>
        /// <param name="ProcessName">Name of process to be </param>
        /// <param name="ProcessId"></param>
        /// <param name="Protocol"></param>
        /// <param name="PortNumber"></param>
        internal ProcessPort(string ProcessName, int ProcessId, string Protocol, int PortNumber, string status)
        {
            _ProcessName = ProcessName;
            _ProcessId = ProcessId;
            _Protocol = Protocol;
            _PortNumber = PortNumber;
            _status = status;
        }

        public string ProcessPortDescription
        {
            get
            {
                return String.Format("{0} ({1} port {2} pid {3})", _ProcessName, _Protocol, _PortNumber, _ProcessId);
            }
        }
        public string ProcessName
        {
            get { return _ProcessName; }
        }
        public int ProcessId
        {
            get { return _ProcessId; }
        }
        public string Protocol
        {
            get { return _Protocol; }
        }
        public int PortNumber
        {
            get { return _PortNumber; }
        }
        public string Status
        {
            get { return _status; }
        }
    }
}