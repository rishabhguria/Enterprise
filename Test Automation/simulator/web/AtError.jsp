<%@ page
  import="com.cameronsystems.util.logger.ILogger,
          com.cameronsystems.util.logger.LoggerManager"
  isErrorPage="true"
%>

<html>

<head>
  <title> Error </title>
  <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />

  <link href="cameron.css" rel="stylesheet" media="all">

</head>
<body>
<h1> Error </h1>

<%
  ILogger logger = LoggerManager.getLogger("NetTrader.AtError");

  String requestUri = (String) request.getAttribute("javax.servlet.error.request_uri");

  logger.error("Error sending an AT: " + requestUri, exception);
%>

<p>
An Error occured while sending the AT.  The
message may not have reached its destination.
</p>

<p>
Go <a href="Ats.jsp"> back</a> to the ATs page.
</p>

</body>
</html>