<html>
<!--
  Input: Post properties from Session configuration screen.
  Processing: If data does not validate, display error screen

              If it does validate, update configuration with session bean.
              This replaces any bean with same connection point, adds new
              bean if connection point unknown.
              Then forward on to Session summary screen.
-->
<head>
<title>
CameronFIX Check Session Configuration
</title>
</head>
<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />
<jsp:useBean id="sess" scope="page" class="com.cameronsystems.fix.SessionBean" />

<body>

<!--
  This copies all fields from posted form to matching properties in SessionBean.
-->
<jsp:setProperty name="sess" property="*" />

<!--
  Could validate fields here and return an error screen in case of problems.
-->

<!--
  Unless we cancelled, update configuration with sess.
  Session is identified by its original connection point string (which may
  have changed if the user has modified any CompID etc fields).
-->
<%
  String cancel = request.getParameter( "cancel" );
  if (cancel == null) {
    app.putSession( request.getParameter( "origcp" ), sess );
  }
%>

<!--
  Forward on to SessionSummary page for redisplay.
-->
<%
  response.sendRedirect("SessionSummary.jsp");
%>

</body>
</html>
