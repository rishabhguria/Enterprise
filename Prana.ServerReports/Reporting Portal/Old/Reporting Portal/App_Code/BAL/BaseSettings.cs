using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

/// <summary>
/// Summary description for BaseSettings
/// </summary>
public static class BaseSetting
{

    private static Dictionary<string,Dictionary<string,string>> dicBaseSettings = new Dictionary<string,Dictionary<string,string>>();

    public static Dictionary<string,Dictionary<string,string>> BaseSettings
    {
        get { return dicBaseSettings; }
        set { dicBaseSettings = value; }
    }
	

	static  BaseSetting()
	{
        LoadSettings();
	}

    private static void LoadSettings()
    {
        DataTable tblBaseSettings = DataManager.getBaseSettings();
        string tempApplicableTo = string.Empty;
        // For now commented the entry from config file, as it wasn't working on reports. In spite of sending dd/MM/YYYY in en-GB format
        // it was taking MM/dd/YYYY. so for now commented it from config file and hard coded in code, so that we can pick it back up 
        // from config when R&D is to be done.
        //_dateFormate = ConfigurationManager.AppSettings["DateFormat"];
        _dateFormate = "MM/dd/yyyy";
        foreach (DataRow dr in tblBaseSettings.Rows)
        {
            tempApplicableTo = dr["ApplicableTo"].ToString();
            
            if(!dicBaseSettings.ContainsKey(tempApplicableTo))
            {
                dicBaseSettings.Add(tempApplicableTo,new Dictionary<string, string>());
            }
            dicBaseSettings[tempApplicableTo].Add(dr["SettingName"].ToString(),dr["SettingValue"].ToString().Replace("$today",DateTime.Today.Date.ToString(BaseSetting.DateFormat)));
        }

    }
    private static string _dateFormate;

    public static string DateFormat
    {
        get { return _dateFormate; }
        //set { _dateFormate = value; }
    }

}
