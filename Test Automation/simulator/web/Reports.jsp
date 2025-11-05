<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<%@ page import="com.cameronsystems.fix.reporting.archiver.DatabaseConnectionManager,
                 java.sql.Connection,
                 java.sql.Statement,
                 java.sql.ResultSet,
                 com.cameronsystems.fix.reporting.archiver.DBUtil,
                 java.sql.Timestamp,
                 java.io.File,
                 java.io.FilenameFilter,
                 com.cameronsystems.util.logger.ILogger,
                 com.cameronsystems.util.logger.LoggerManager,
                 com.cameronsystems.fix.reporting.servlet.ReportsPageLogic,
                 java.util.Iterator,
                 com.cameronsystems.fix.reporting.servlet.ReportInfo,
                 java.util.ArrayList,
                 java.net.URLEncoder,
                 java.util.List"%>
<%
//  ILogger logger = LoggerManager.getLogger("reports.Reports.jsp");

  String summaryString = ReportsPageLogic.getDatabaseSummaryString(session.getServletContext());

  List reportInfos = ReportsPageLogic.getReportInfos(session.getServletContext());
%>

<html>
<head>
<title>CameronFIX Business Reports</title>

<link href="cameron.css" rel="stylesheet" media="all">

</head>
<body>
<h1>CameronFIX Business Reports</h1>

<div><%= summaryString %></div>
<table>
<tr>
<th>Report</th>
<th>Description</th>
</tr>

<%
  if(reportInfos.isEmpty()) {
    %>
    <tr><td colspan="2">No reports configured.</td></tr>
    <%
  } else {
    for (int i = 0; i < reportInfos.size(); i++) {
      ReportInfo info = (ReportInfo) reportInfos.get(i);
      %>
      <tr>
      <td><a href="ReportParameters.jsp?__report=<%= URLEncoder.encode(info.getName(), "UTF-8") %>">
            <%= info.getName() %></a></td>
      <td><%= info.getDescription() %></td>
      </tr>
      <%
    }
  }
%>
<tr>

</table>

</body>
</html>