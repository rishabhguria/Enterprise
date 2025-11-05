using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;

public partial class Default : System.Web.UI.Page
{
    internal static string sModule = "Reports";
    internal Dictionary<string, Dictionary<string, string>> Settings = BaseSetting.BaseSettings;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

}