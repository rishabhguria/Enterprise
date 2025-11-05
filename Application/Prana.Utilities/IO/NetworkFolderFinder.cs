using Prana.LogManager;
using System;
using System.IO;
using System.Net.NetworkInformation;

namespace Prana.Utilities
{
    public class NetworkFolderFinder
    {

        private delegate bool DirectoryExistsDelegate(string folder);


        public static bool MachineExist(string path, int millisecondsTimeout)
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    if (new Uri(path).IsUnc)
                    {
                        string[] temp = path.Split('\\');

                        string comp = temp[2];
                        //string dum;
                        PingReply reply = ping.Send(comp, millisecondsTimeout);
                        if (reply.Status == IPStatus.Success)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                return false;
            }
        }

        // the is canbe of type \\192.168.1.18\Test

        public static bool DirectoryExist(string path, int millisecondsTimeout)
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    if (new Uri(path).IsUnc)
                    {
                        string[] temp = path.Split('\\');
                        string comp = temp[2];
                        //string dum;
                        PingReply reply = ping.Send(comp, millisecondsTimeout);

                        if (reply.Status == IPStatus.Success)
                        {
                            if (DirectoryExistsTimeout(path, millisecondsTimeout)) // need to enter path here
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (DirectoryExistsTimeout(path, millisecondsTimeout)) // need to enter path here
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
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
                return false;
            }
        }

        static bool DirectoryExistsTimeout(string path, int millisecondsTimeout)
        {
            try
            {
                DirectoryExistsDelegate callback = new DirectoryExistsDelegate(Directory.Exists);
                IAsyncResult result = callback.BeginInvoke(path, null, null);

                if (result.AsyncWaitHandle.WaitOne(millisecondsTimeout, false))
                {
                    return callback.EndInvoke(result);
                }
                else
                {
                    callback.EndInvoke(result);  // Needed to terminate thread?
                    return false;
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
            return false;
        }
    }
}
