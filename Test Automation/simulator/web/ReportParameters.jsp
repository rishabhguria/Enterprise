<%@ page import="java.util.List,
                 java.util.Iterator,
                 com.cameronsystems.fix.reporting.servlet.ReportParameterInfo,
                 com.cameronsystems.fix.reporting.servlet.SharedPageLogic,
                 com.cameronsystems.fix.reporting.servlet.GetReportParametersPageLogic,
                 com.cameronsystems.fix.reporting.servlet.ReportsPageConstants"%>
<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<%
  String report = request.getParameter(ReportsPageConstants.REPORT_PARAM);
  List reportParameterInfos =
          GetReportParametersPageLogic.getReportParameterInfos(session.getServletContext(), report);
  String errorParam = request.getParameter(ReportsPageConstants.ERROR_PARAM_PARAM);
  String errorMsg = request.getParameter(ReportsPageConstants.ERROR_MSG_PARAM);

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
  for (Iterator i = reportParameterInfos.iterator(); i.hasNext();) {
    ReportParameterInfo info = (ReportParameterInfo) i.next();
    String name = info.getName();
    String value = request.getParameter(name);
    value = value != null ? value : "";
%>
<tr>
<td title="<%= info.getDescription() %>">
<strong><%= info.getPrettyName() %></strong>
</td>
<td><input type="text" name="<%= name %>" value="<%= value %>"/><%=info.isRequired() ? "*" : ""%>
<%
    if(name.equals(errorParam)) {
      %>
      <span class="errorMsg">
        <%= errorMsg %>
      </span>
      <%
    }
%>

</td>

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