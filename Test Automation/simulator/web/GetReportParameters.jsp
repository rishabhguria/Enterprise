<%@ page import="java.util.List,
                 java.util.Iterator,
                 com.cameronsystems.fix.reporting.servlet.ReportParameterInfo,
                 com.cameronsystems.fix.reporting.servlet.SharedPageLogic"%>
<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<%
  String report = request.getParameter("__report");
  List reportParams = SharedPageLogic.getReportParameters(session.getServletContext(), report);
%>
<html>
<head>
<title>CameronFIX Business Reports: Enter parameters for <%= report %></title>
<link href="cameron.css" rel="stylesheet" media="all">
</head>
<body>
<h1>Enter Parameters for <%= report %></h1>

<form action="RenderReportAsHTML.jsp" method="GET">
<fieldset>
<legend>Parameters</legend>

<input type="hidden" name="__report" value="<%=report %>"/>

<table>
<%
  for (Iterator i = reportParams.iterator(); i.hasNext();) {
    ReportParameterInfo info = (ReportParameterInfo) i.next();
%>
<tr>
<td title="<%= info.getDescription() %>">
<strong><%= info.getPrettyName() %></strong>
</td>
<td><input type="text" name="<%= info.getName() %>"/></td>
</tr>

<% } %>

<tr>
<td> <strong>View as ... </strong> </td>
<td>
HTML <input type="radio" name="__format" value="HTML" checked="checked"/>
PDF <input type="radio" name="__format" value="PDF"/>
</td>
</tr>

<tr>
<td colspan="2">
<input type="submit" value="Run"/>
</td>
</tr>

</table>

</fieldset>

</form>

</body>
</html>