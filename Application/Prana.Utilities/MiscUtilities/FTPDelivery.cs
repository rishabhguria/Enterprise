using Prana.Interfaces;
using System.IO;
using System.Net;

namespace Prana.Utilities.MiscUtilities
{
    public class FTPDelivery : IDelivery
    {
        #region IDeliveryClass Members
        public bool SendFile(object objDetails)
        {
            return true;
        }

        public void ftpfile(string ftpfilepath, string inputfilepath)
        {
            string ftphost = "127.0.0.1";
            //here correct hostname or IP of the ftp server to be given   

            string ftpfullpath = "ftp://" + ftphost + ftpfilepath;
            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
            ftp.Credentials = new NetworkCredential("userid", "password");
            //userid and password for the ftp server to given   

            ftp.KeepAlive = true;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;
            FileStream fs = File.OpenRead(inputfilepath);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            Stream ftpstream = ftp.GetRequestStream();
            ftpstream.Write(buffer, 0, buffer.Length);
            ftpstream.Close();
        }
        #endregion
    }
}
