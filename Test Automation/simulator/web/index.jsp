<html>
<head>
<title>
CameronFIX Status
</title>

<link href="cameron.css" rel="stylesheet" media="all">

</head>

<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />
<body>

<!--NAVIGATION-->
<div id="navwrapper">
<ul id="nav"><!--do not add white space-->
  <li><a class="current">Home</a></li>
  <li><a href="CommandLine.jsp">Command Line</a></li>
  <li class="lastli"><a href="SessionSummary.jsp">Session Summary</a></li>
</ul>
</div>
<!--END NAVIGATION-->

<h1>
CameronFIX Universal Server "<%= app.getName()%>"
</h1>

<table>

<tr>
<th>Link</th><th>Description</th>
<tr>

<tr>
<td><a href="CommandLine.jsp">Command Line</a></td>
<td>
Command line interface for this Universal Server (<%= app.getName()%>).
You can interrogate the current status of the server as well as controlling
its operation using this command line interface.
</td>
</tr>

<tr>
<td><a href="SessionSummary.jsp">Session Summary</a></td>
<td>
Summary status information on all FIX sessions running on this
Universal Server (<%= app.getName()%>).
</td>
</tr>

<tr>
<td><a href="../doc/README_FIRST.html">Documentation</a></td>
<td>
CameronFIX Documentation including overviews of all options and detailed
JavaDoc of all CameronFIX classes.
</td>
</tr>

<tr>
<td><a href="http://www.cameronsystems.com">Cameron Systems Website</a></td>
<td>
The Cameron Systems website. A source for all the latest news and information
in the FIX and CameronFIX world.
</td>
</tr>

<tr>
<td><a href="http://www.fixprotocol.org">FIX Protocol Website</a></td>
<td>
The official website of the FIX protocol. The ultimate source for information on
the FIX protocol.
</td>
</tr>

</table>

</body>
</html>
