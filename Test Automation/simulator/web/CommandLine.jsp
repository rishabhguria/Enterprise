<html>

<head>
<title>
CameronFIX Command Line
</title>

<link href="cameron.css" rel="stylesheet" media="all">

</head>

<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />

<body onload="document.commandForm.command.focus(); document.commandForm.command.select();">

<!--NAVIGATION-->
<div id="navwrapper">
<ul id="nav"><!--do not add white space-->
  <li><a href="index.jsp">Home</a></li>
  <li><a class="current">Command Line</a></li>
  <li class="lastli"><a href="SessionSummary.jsp">Session Summary</a></li>
</ul>
</div>
<!--END NAVIGATION-->

<h1>
CameronFIX Universal Server "<%= app.getName() %>"
</h1>

<%
  String command = request.getParameter( "command" );
  if (command == null) {
    command = "?";
  }
%>

<form type=POST action=CommandLine.jsp name="commandForm" id="commandForm">
<fieldset>
  <legend> Command Line Interface </legend>
<br/>
<label> Command:
  <input type="text" name="command" id="command" size="80"
         value="<%= command%>" title="This is the command to send" />
</label>
<br/><br/>
</fieldset>
<br> <br>

</form>

<p>
<pre>
<%= app.processCommand( command ) %>
</pre>

</body>
</html>
