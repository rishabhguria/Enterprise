using System;
using System.IO;
using System.Web;
using System.Web.Security;

public class FileProtectionHandler : IHttpHandler
{
    public bool IsReusable
    {
        get { return true; }
    }

    public void ProcessRequest(HttpContext context)
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            FormsAuthentication.RedirectToLoginPage();
            return;
        }

        string requestedFile = context.Server.MapPath(context.Request.FilePath);
        SendContentTypeAndFile(context, requestedFile);
    }

    private HttpContext SendContentTypeAndFile(HttpContext context, String strFile)
    {
        context.Response.ContentType = GetContentType(strFile);
        context.Response.TransmitFile(strFile);
        context.Response.End();

        return context;
    }

    private string GetContentType(string filename)
    {
        // used to set the encoding for the reponse stream 
        string res = null;
        FileInfo fileinfo = new FileInfo(filename);
        if (fileinfo.Exists)
        {
            switch (fileinfo.Extension.Remove(0, 1).ToLower())
            {
                case "html":
                    {
                        res = "text/HTML";
                        break;
                    }
                case "htm":
                    {
                        res = "text/HTML";
                        break;
                    }
                case "pdf":
                    {
                        res = "Application/pdf";
                        break;
                    }
            }
            return res;
        }
        return null;
    }
}