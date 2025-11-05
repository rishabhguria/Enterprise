<html>
<!--
  Input: ConnectionPoint string.
  Processing: Look up session from connection point and display recently
              received messages.
-->
<head>
<title>
CameronFIX Session Last Messages Sent.
</title>
<%@page import="com.cameronsystems.fix.ISessionBean" %>

<link href="cameron.css" rel="stylesheet" media="all">

</head>

<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />

<%
  ISessionBean sess = app.getSessionBean( request.getParameter( "cp" ) );
%>

<body>

<h1>
CameronFIX recent messages received from <%= sess.getConnectionPoint()%>
</h1>

  <fieldset>
    <legend> Recent Messages Received</legend>

  <%
    String[] messages = sess.getInMessages( 40 );
    for (int i = 0; i < messages.length; i++) {
  %>
      <%= messages[i] %> <br>
  <%
    }
  %>
  </fieldset>
</body>
</html>
