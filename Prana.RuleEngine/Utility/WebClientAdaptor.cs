using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System.Threading;
using System.IO;

namespace Prana.RuleEngine.Utility
{
    internal class WebClientAdaptor
    {
        /// <summary>
        /// send request to url to get data
        /// </summary>
        /// <param name="url"></param>
        /// <returns>data return from URL</returns>
        internal static String GetDataUsingWebClient(String url, String MediaType)
        {
            String data = "";
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("accept",MediaType);
                webClient.Credentials = new NetworkCredential("admin", "admin");
                HttpStatusCode httpStatusCodeTemp = HttpStatusCode.NotFound;
                //TODO find out some standard way to handel "Not found(404) error"
                int requestAttemptCount = 1;
                while (requestAttemptCount <= 3)
                {
                    requestAttemptCount++;
                    try
                    {
                        webClient.Headers.Add("accept", MediaType);
                        data = webClient.DownloadString(url);
                        break;

                    }
                    catch (System.Net.WebException ex)
                    {
                        httpStatusCodeTemp = ((HttpWebResponse)(ex.Response)).StatusCode;
                        if ((httpStatusCodeTemp != HttpStatusCode.NotFound))
                        { 
                            throw;
                        }
                       // Thread.Sleep(2000);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
               
            }
            return data;
        }
        internal static bool DeleteUsingWebClient(String url)
        {
            bool isRuleDeleted = false;
            try
            {
                WebClient webClient = new WebClient();
                webClient.Credentials = new NetworkCredential(Constants.CLIENT_NAME, Constants.CLIENT_PASSWORD);
                byte[] s = new byte[1];
                byte[] response = webClient.UploadData(url, "DELETE", s);
                isRuleDeleted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Asset Not Exists!!" + ex.Message);
            }
            return isRuleDeleted;
        }
        
        internal static  String UpdateRuleUsingWebClient(string url, string source)
        {
            String returnString = String.Empty;
            try
            {
                string str = source;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] arr = encoding.GetBytes(str);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "PUT";
                request.ContentType = "text/plain";
                request.ContentLength = arr.Length;
                request.Credentials = new NetworkCredential("admin", "admin");
                request.KeepAlive = true;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(arr, 0, arr.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                returnString = response.StatusCode.ToString();

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Asset Not Exists!!" + ex.Message);
                throw;
            }
            return returnString;
        }

        internal static String UpdateRuleMetaDataWebClient(string url, string source)
        {
            String returnString = String.Empty;
            try
            {
                string str = source;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] arr = encoding.GetBytes(str);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "PUT";
                request.ContentType = "application/xml";
                request.ContentLength = arr.Length;
                request.Credentials = new NetworkCredential("admin", "admin");
                request.KeepAlive = true;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(arr, 0, arr.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                returnString = response.StatusCode.ToString();

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Asset Not Exists!!" + ex.Message);
                throw;
            }
            return returnString;
        }
        
    }
}