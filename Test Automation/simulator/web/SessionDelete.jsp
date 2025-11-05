<html>
<!--
  Input: ConnectionPoint string.
  Processing: Look up session from connection point and delete it.
-->
<head>
<title>
CameronFIX Session Delete.
</title>
</head>

<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />

<%
  app.removeSession( request.getParameter( "cp" ) );
  response.sendRedirect("SessionSummary.jsp");
%>

<body>
</body>
</html>