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
  ILogger logger = LoggerManager.getLogger("NetTrader.FillError");

  String requestUri = (String) request.getAttribute("javax.servlet.error.request_uri");


  // Attempt to figure out whether we were sending and IOI or cancelling an IOI.
  // This doesn't always work since the request_uri attribute is not always set.
  String action = "sending or cancelling";
  if(requestUri != null) {
    action = requestUri.indexOf("op=cancel") >= 0 ? "cancelling" : "sending";
  }

  logger.error("Error " + action + " an Fill: " + requestUri, exception);
%>

<p>
An Error occured while <%= action %> the Fill.  The
message may not have reached its destination.
</p>

<p>
Go <a href="Orders.jsp"> back</a> to the Orders page.
</p>

</body>
</html>