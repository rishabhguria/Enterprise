<html>
<!--
  Input: ConnectionPoint string.
  Processing: Look up session from connection point and display all sorts of
              details about just that session.

  @todo Lots more details need to be displayed. Maybe even latest messages -
        maybe than should come only with monitoring option.
-->
<head>
<title>
CameronFIX Session Details.
</title>
<%@page import="com.cameronsystems.fix.ISessionBean" %>

<link href="cameron.css" rel="stylesheet" media="all">

</head>

<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />

<%
  ISessionBean sess = app.getSessionBean( request.getParameter( "cp" ) );
%>

<body>

<!--NAVIGATION-->
<div id="navwrapper">
<ul id="nav"><!--do not add white space-->
  <li><a href="index.jsp">Home</a></li>
  <li><a href="CommandLine.jsp">Command Line</a></li>
  <li class="lastli"><a href="SessionSummary.jsp">Session Summary</a></li>
</ul>
</div>
<!--END NAVIGATION-->

<h1>
CameronFIX Session Detail for <%= sess.getConnectionPoint()%>
</h1>

  <fieldset>
    <legend> Session Details </legend>
    <ul>
      <li> Session: <%= sess.getConnectionPoint()%> </li>
      <li> Version: <%= sess.getFixVersion()%> </li>
    </ul>
    To do: Lots more details.
  </fieldset>
</body>
</html>