<html>
<!--
  Input: ConnectionPoint string - may be null.
  Processing: Look up session from connection point, or create new session.
              Use session to initialize fields.

  @todo The protocol fields such as Connect/Listen, hostname, port, heartbeat
        dialects, do not come from session bean.
-->

<head>
<title>
CameronFIX Session Configuration
</title>
<%@page import="com.cameronsystems.fix.ISessionBean" %>

<link href="cameron.css" rel="stylesheet" media="all">

</head>

<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />

<%
  String origcp = request.getParameter( "cp" );
  ISessionBean sess = app.getSessionBean( origcp );
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
CameronFIX Session Configuration
</h1>

<form action="CheckSessionConfig.jsp" method="post" name="sessionForm" id="sessionForm">
  <fieldset>
  <legend> Configure FIX Session </legend>

    <input type="hidden" name="origcp" id="origcp" value="<%= origcp%>"/>

    <table>
    <tr>
    <td>
      <fieldset>
        <legend> TargetID </legend>
        <table>
          <tr>
            <td title="This is the name of your counterparty"
                align="right" > Comp ID </td>
            <td> <input type="text" name="targetCompID" id="targetCompID"
                        title="This is the name of your counterparty"
                        value="<%= sess.getTargetCompID()%>" />
            </td>
          </tr>

          <tr>
            <td title="This is an optional part of your counterparty's name"
                align="right" > Sub ID </td>
            <td> <input type="text" name="targetSubID" id="targetSubID"
                        title="This is an optional part of your counterparty's name"
                        value="<%= sess.getTargetSubID()%>" />
            </td>
          </tr>

          <tr>
            <td title="This is an optional part of your counterparty's name"
                align="right" > Location ID </td>
            <td> <input type="text" name="targetLocationID" id="targetLocationID"
                        title="This is an optional part of your counterparty's name"
                        value="<%= sess.getTargetLocationID()%>" />
            </td>
          </tr>
        </table>
      </fieldset>
    </td>

    <td>
      <fieldset>
        <legend> SenderID </legend>
        <table>
          <tr>
            <td title="This is the name by which you are known to your counterparty"
                align="right" > Comp ID </td>
            <td> <input type="text" name="senderCompID" id="senderCompID"
                        title="This is the name by which you are known to your counterparty"
                        value="<%= sess.getSenderCompID()%>" />
            </td>
          </tr>

          <tr>
            <td title="This is an optional part of your name"
                align="right" > Sub ID </td>
            <td> <input type="text" name="senderSubID" id="senderSubID"
                        title="This is an optional part of your name"
                        value="<%= sess.getSenderSubID()%>" />
            </td>
          </tr>

          <tr>
            <td title="This is an optional part of your name"
                align="right" > Location ID </td>
            <td> <input type="text" name="senderLocationID" id="senderLocationID"
                        title="This is an optional part of your name"
                        value="<%= sess.getSenderLocationID()%>" />
            </td>
          </tr>
        </table>
      </fieldset>
    </td>
    </tr>
    </table>

    <fieldset>
      <legend> Protocol </legend>
      <table>
        <tr>
          <td title="This is the FIX version that you have agreed to use with the other party. Note that both sides must be using the same FIX version."
              align="right">
              FIX Version </td>
          <td title="This is the FIX version that you have agreed to use with the other party. Note that both sides must be using the same FIX version.">
            <select name="fixVersion" id="fixVersion">
              <%
                String myver = sess.getFixVersion();
                for (int i=0; i < 5; i++) {
                  String ver = "4." + i;
                  out.println( "<option" +
                               (ver.equals(myver) ? " selected=\"selected\"" : "") +
                               "> " + ver + " </option>"  );
                }
              %>
            </select>
          </td>

          <td title="This is the type of connection you have with your counterparty. Either you connect to them or they connect to you."
              align="right">
              Socket Type </td>
          <td title="This is the type of connection you have with your counterparty. Either you connect to them or they connect to you.">
            <select name="socketType" id="socketType">
              <option selected="selected"> Connect </option>
              <option> Listen </option>
            </select>
          </td>
        </tr>

        <tr>
          <td title="Every period of this number of seconds a message should be sent or received. Otherwise an error is reported."
              align="right">
              Heartbeat (seconds) </td>
          <td title="Every period of this number of seconds a message should be sent or received. Otherwise an error is reported.">
            <select name="heartbeat" id="heartbeat">
              <option> 10 </option>
              <option> 20 </option>
              <option selected="selected"> 30 </option>
              <option> 45 </option>
              <option> 60 </option>
              <option> 90 </option>
            </select>
          </td>

          <td title="Network name or address of your counterparty's computer."
              align="right">
              Host name </td>
          <td title="Network name or address of your counterparty's computer.">
            <input type="text" name="hostname" id="hostname" /> </td>
        </tr>

        <tr>
          <td title="Buy side sends orders and receives trades. Sell side receives orders and sends trades. Which are you?"
              align="right">
              Side </td>
          <td title="Buy side sends orders and receives trades. Sell side receives orders and sends trades. Which are you?">
            <select name="side" id="side">
              <option selected="selected"> Buy </option>
              <option> Sell </option>
            </select>
          </td>

          <td title="If you connect to your counterparty, this is the network port to use. If they connect to you, this is the port you will listen on."
              align="right">
              Port </td>
          <td title="If you connect to your counterparty, this is the network port to use. If they connect to you, this is the port you will listen on.">
            <input type="text" name="port" id="port" /> </td>
        </tr>
      </table>

      <fieldset>
        <legend> Protocol Dialects </legend>
        <table>
          <tr>
            <td>
              <input type="checkbox" name="1" id="1" /> Ignore ID fields in logon response <br/>
              <input type="checkbox" name="2" id="2" /> Process skipped messages <br/>
              <input type="checkbox" name="3" id="3" /> Original sending time in gap fill <br/>
            </td>

            <td>
              <input type="checkbox" name="4" id="4" /> Auto logout response <br/>
              <input type="checkbox" name="5" id="5" /> Allow no length on raw data <br/>
            </td>
          </tr>
        </table>
      </fieldset>
    </fieldset>

    <div align="center">
      <input type="submit" name="submit" id="submit" value="Save"
             onclick="alert('Sorry. Save is not supported yet.');"/> &nbsp
      <input type="submit" name="cancel" id="cancel" value="Cancel"/>
    </div>
  </fieldset>
</form>




</body>
</html>