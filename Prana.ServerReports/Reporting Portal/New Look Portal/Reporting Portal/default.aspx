<%@Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default"%>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title><%=Settings[sModule]["Report_PageTitle"]%></title>
</head>
<FRAMESET id="fstMain" border="0" frameSpacing="0" rows="*" frameBorder="0" >

    <FRAME name="OnlinePage" marginWidth="0" marginHeight="0" src="index.aspx" frameBorder="0" noResize scrolling="auto" style="background-image: url('images/bkgnd.jpg')">

</FRAMESET>
<body>
    <form id="form1" runat="server">
    <p>This page uses frames, but your browser doesn't support them.</p> 
    </form>
</body>
</html>