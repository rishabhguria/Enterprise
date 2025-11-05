using System;
using System.IO;
using System.Net;
using System.Text;

namespace Prana.OptionCalculator.CalculationComponent
{

    class IDCDataManager
    {
        const string IDCUserNameKey = "IDCUserName";
        const string IDCPWDKey = "IDCPWD";

        public IDCDataManager()
        {
        }
        public string GetIDCData(IDCDataRequest idr)
        {
            try
            {
                string Query = idr.Query;
                string strBaseURL = "http://rplus.intdata.com/cgi/nph-rplus";
                CredentialCache Credentialcache = new CredentialCache();
                //CHMW-2560	CLONE -Apply Microsoft Managed Recommended Rules in Prana.OptionCalculator.CalculationComponent project
                Credentialcache.Add(new Uri(strBaseURL),
                                    "Basic",
                                    new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings[IDCUserNameKey],
                                                          System.Configuration.ConfigurationManager.AppSettings[IDCPWDKey])
                                    );

                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(strBaseURL);
                httpWebRequest.Credentials = Credentialcache;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = Query.Length;
                Stream RequestStream = httpWebRequest.GetRequestStream();
                RequestStream.Write(Encoding.ASCII.GetBytes(Query), 0, Query.Length);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream ResponseStream = httpWebResponse.GetResponseStream();
                StreamReader ResponseStreamReader = new StreamReader(ResponseStream);
                string StringResponse = ResponseStreamReader.ReadToEnd();
                ResponseStreamReader.Close();
                //CA2202	Do not dispose objects multiple times	
                //Object 'ResponseStream' can be disposed more than once in method 'IDCDataManager.GetIDCData(IDCDataRequest)'
                //To avoid generating a System.ObjectDisposedException you should not call Dispose more than one time on an object.
                //ResponseStream.Close();
                httpWebResponse.Close();

                return StringResponse;
            }
            catch (Exception ex)
            {
                if (ex.Message == "The remote server " +
                               "returned an error: (404) Not Found.")
                    throw new Exception("File not found");
                else if (ex.Message == "The remote server" +
                        " returned an error: (401) Unauthorized.")
                    throw new Exception("Unauthorized access");

                return "";
            }
        }

    }
}
