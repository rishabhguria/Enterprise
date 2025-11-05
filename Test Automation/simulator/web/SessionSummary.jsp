<html>
<head>
<title>
CameronFIX Status
</title>

<!-- This refreshes the page every 30 seconds-->
<meta http-equiv="REFRESH" content="30;SessionSummary.jsp" />

<%@page import="com.cameronsystems.fix.ISessionBean" %>

<link href="cameron.css" rel="stylesheet" media="all">

</head>
<!--
  The SessionSummary object is the only input to this form.
-->
<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />
<body>

<!--NAVIGATION-->
<div id="navwrapper">
<ul id="nav"><!--do not add white space-->
  <li><a href="index.jsp">Home</a></li>
  <li><a href="CommandLine.jsp">Command Line</a></li>
  <li class="lastli"><a class="current">Session Summary</a></li>
</ul>
</div>
<!--END NAVIGATION-->

<h1>
CameronFIX Universal Server "<%= app.getName() %>" Session Summary
</h1>

<!--
  Menu - could have multiple entries.
-->
<table>
  <tr>
    <td> <a href="SessionConfig.jsp"> New Session </a> </td>
  </tr>
</table>

<fieldset>
  <legend> FIX Sessions </legend>

  <!--
    This is the table of session summaries.
  -->
  <table>
  <tr>
  <th> Action </th>
  <th> <a href="SessionSummary.jsp?si=1"> Session </a> </th>
  <th> <a href="SessionSummary.jsp?si=3"> Connection </a> </th>
  <th> In </th>
  <th> Out </th>
  <th> <a href="SessionSummary.jsp?si=4"> Blocked </a> </th>
  <th> <a href="SessionSummary.jsp?si=5"> Logged On </a> </th>
  <th> <a href="SessionSummary.jsp?si=6"> Scheduled </a> </th>
  <th> Status </th>
  <th> <a href="SessionSummary.jsp?si=2"> FIX Version </a> </th>
  <th> <a href="SessionSummary.jsp?si=7"> In/sec </a> </th>
  <th> <a href="SessionSummary.jsp?si=8"> Out/sec </a> </th>
  </tr>
  <%! String sort; %>
  <%
    String requestedSort = request.getParameter( "si" );
    if (requestedSort != null) {
      sort = requestedSort;
    }

    ISessionBean[] sessions = app.getSessions( sort );
    for (int i=0; i < sessions.length; i++) {
      ISessionBean sess = sessions[i];
  %>
      <tr>
      <td>
        <a href="SessionConfig.jsp?cp=<%= sess.getConnectionPoint() %>"> Edit </a> |
        <a href="SessionDelete.jsp?cp=<%= sess.getConnectionPoint() %>"
           onclick="alert('Sorry. Session delete is not available yet.');"> Del </a>
      </td>
      <td> <a href="SessionStatus.jsp?cp=<%= sess.getConnectionPoint() %>">
           <%= sess.getConnectionPoint() %> </a>
      </td>
      <td> <%= sess.getConnection() %> </td>
      <td>  <a href="SessionInMessages.jsp?cp=<%= sess.getConnectionPoint() %>">
        <%= sess.getInMsgSeqNum() %>
      </td>
      <td> <a href="SessionOutMessages.jsp?cp=<%= sess.getConnectionPoint() %>">
        <%= sess.getOutMsgSeqNum() %>
      </td>
      <td> <%= sess.getBlocked() %> </td>
      <td> <%= sess.getLoggedOn() %> </td>
      <td> <%= sess.getScheduled() %> </td>
      <td> <%= sess.getTerminationStatus() %> </td>
      <td> <%= sess.getFixVersion() %> </td>
      <td> <%= sess.getInMessagesPerSecond() %> </td>
      <td> <%= sess.getOutMessagesPerSecond() %> </td>
      </tr>
  <%
    }
  %>
  </table>
</fieldset>
</body>
</html>
