<html>
<head>
<title>
CameronFIX Exception Manager
</title>

<%@page import="com.cameronsystems.fix.ApplicationBean,
                javax.naming.NameNotFoundException,
                com.cameronsystems.fix.exceptionmanager.ExceptionManagerAccessorBean,
                com.cameronsystems.fix.configuration.Constants,
                java.util.List,
                java.util.Iterator,
                com.cameronsystems.fix.processor.breakout.BrokenOutMessageData,
                com.cameronsystems.fix.processor.breakout.IBreakOutReason,
                com.cameronsystems.fix.message.Message,
                java.util.Enumeration,
                com.cameronsystems.fix.message.Field,
                com.cameronsystems.fix.configuration.FieldInfo,
                com.cameronsystems.fix.configuration.ValueInfo,
                com.cameronsystems.fix.processor.breakout.IBreakOutReason"
         errorPage="ErrorPage.jsp" %>



<link href="cameron.css" rel="stylesheet" media="all">

</head>
<jsp:useBean id="exceptMgr" scope="session" class="com.cameronsystems.fix.exceptionmanager.ExceptionManagerAccessorBean"/>



<%
  if(!exceptMgr.isInitialized()) {
    response.sendRedirect("ExceptionView.jsp");
    return;
  }

  String idStr = request.getParameter("id");
  int id = -1;

  try {
    id = Integer.parseInt(idStr);
  } catch(NumberFormatException nfe) {
    id = -1;
  }

  if(id == -1) {
    response.sendRedirect("ExceptionView.jsp");
    return;
  }

  BrokenOutMessageData data = exceptMgr.getMessageById(request,id);

  if(data == null) {
    response.sendRedirect("ExceptionView.jsp");
    return;
  }

  Message mess = data.getMessage();
%>

<body>


<h1> Exception Details </h1>

<fieldset>
<legend>Message</legend>
<table>
  <tr>
    <th>Tag</th>
    <th>Name</th>
    <th>Value</th>
  </tr>

  <%
    int rowCnt = 0;
    for(Enumeration e = mess.getFieldObjects().elements(); e.hasMoreElements(); rowCnt++) {
      Field field = (Field) e.nextElement();
      FieldInfo fieldInfo = field.getFieldInfo();
      ValueInfo valueInfo = field.getValueInfo();

      %>
      <tr class="<%= rowCnt % 2 == 0 ? "plain0" : "plain1" %>">
        <td><%= field.getTag() %></td>
        <td title="<%= fieldInfo.getDescription() %>"><%= fieldInfo.getName() %></td>
        <td><%= field.getStringFieldValue() + (valueInfo != null && valueInfo.getName() != null ? (" <i>[" + valueInfo.getName() + "]</i>") : "") %></td>
        </tr>
      <%
    }
  %>

</table>
</fieldset>

<fieldset>
  <legend> Reasons </legend>
  <ol>
    <%
      List reasons = data.getReasons();
      for (Iterator i = reasons.iterator(); i.hasNext();) {
        IBreakOutReason reason = (IBreakOutReason) i.next();
        %>
        <li><%= reason.getReason() %></li>
        <%
      }
    %>
  </ol>
</fieldset>

</body>
</html>