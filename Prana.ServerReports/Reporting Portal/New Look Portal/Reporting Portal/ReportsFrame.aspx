<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportsFrame.aspx.cs" Inherits="ReportsFrame" EnableViewState="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=Settings[sModule]["Report_PageTitle"]%></title>
    <style type="text/css">
        <!--
        body {margin: 0;} 
        .ReportClass{width: 100%; background-color: white; margin:0px; text-align:left}
        .HeaderBannerbg {width: 98%; height: 23px;text-align:right; vertical-align:text-bottom; background-repeat: no-repeat;}
        .ImgClass {border: 0; float: left}
        .LinkFont {font-family: verdana; font-weight: normal; font-size: 12px; color: black;}
        -->
    </style>
</head>
<body>
<div id="divBanner" class="HeaderBannerbg">
<table width="100%" cellpadding="0" cellspacing="0" >
    <tr>
        <td class="LinkFont"><a href="javascript:parent.location='Reports.aspx'" title="Reporting Service" >Home</a></td>
    </tr>
</table>
</div>
<div id="divReport" class="ReportClass">
<form id="form1" runat="server">
    <rsweb:ReportViewer ID="reportVwr" runat="server" Height="100%" Width="100%" ProcessingMode="Remote">
    </rsweb:ReportViewer>
 </form>
</div>
        <script language="javascript" type="text/javascript">
  var x =document.getElementsByTagName("span");
    for (i=0;i<x.length;i++)
    {
        //alert(x[i].innerText);
        if (x[i].innerText == "ghostparam")
         {
            var node = x[i].parentElement;
            if (node)
                node.style.visibility = "hidden";
            node = node.nextSibling;
            if (node)
                node.style.visibility = "hidden";
        }
    }
</script>
</body>
</html>
