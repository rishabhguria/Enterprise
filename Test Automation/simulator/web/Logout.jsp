<html>
<head>
<title>
CameronFIX Logout
</title>
</head>
<jsp:useBean id="auth" scope="session" class="com.cameronsystems.fix.processor.AuthorizationBean" />

<body>

<%
  auth.logout();
  session.invalidate();
  response.sendRedirect("Logon.jsp");
%>

</body>
</html>
