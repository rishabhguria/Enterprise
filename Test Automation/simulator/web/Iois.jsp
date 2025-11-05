<html>
<head>
<title>
CameronFIX IOI Summary
</title>

<!-- This refreshes the page every 30 seconds-->
<meta http-equiv="REFRESH" content="30;Iois.jsp" />

<%@page import="com.cameronsystems.util.collection.ITagStringPairs" %>
<%@page import="com.cameronsystems.fix.configuration.Constants" %>

<link href="cameron.css" rel="stylesheet" media="all">

</head>
<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />
<jsp:useBean id="mmp" scope="session" class="com.cameronsystems.fix.processor.MarketMirrorPopulatorAccessor" />
<jsp:useBean id="auth" scope="session" class="com.cameronsystems.fix.processor.AuthorizationBean" />

<%! boolean hideTheDead = true;

  String hideNull(String str) {
    if(str == null || str.equals("null")) {
      str = "";
    }
    return str;
  }
%>

<%
  if(!auth.isLoggedIn()) {
    response.sendRedirect("Logon.jsp?target=Iois.jsp");
  }

  //Determine whether we want a buy side or sell side view.
  boolean buySide = mmp.isBuySideView();

  String hideRequest = request.getParameter( "hi" );
  if (hideRequest != null) {
    hideTheDead = hideRequest.equals("1");
  }
%>

<body>

<!--NAVIGATION-->
<div id="navwrapper">
<ul id="nav">
  <li>               <a class="current">IOIs</a></li>
  <li>               <a href="Ats.jsp">Advertisements (ATs)</a></li>
  <li>               <a href="Orders.jsp">Orders</a></li>
  <li class="lastli"><a href="Logout.jsp">Logout</a></li>
</ul>
</div>
<!--END NAVIGATION-->

<h1>
CameronFIX "<%= app.getName() %>" IOI Summary
</h1>

<!--
  Menu - could have multiple entries.
-->
<table>
  <tr>

  <% if (!buySide) {%>
    <td>
    <a href="IoiEntry.jsp"> Add new IOI </a>
    </td>
  <%}%>

  <td>
    <% if (hideTheDead) {%>
      <a href="Iois.jsp?hi=0"> Show cancelled or expired IOIs </a>
    <%} else {%>
      <a href="Iois.jsp?hi=1"> Hide cancelled or expired IOIs </a>
    <%}%>
  </td>

  </tr>
</table>

<fieldset>
  <legend> IOIs </legend>

  <!--
    This is the table of IOIs.
  -->
  <table>
  <tr>
  <th title="Operations that you can perform on each IOI"> Action </th>
  <th> <a href="Iois.jsp?si=<%= Constants.TAGiIOITransType%>"
          title="Last action performed - click here to sort by this column">Status</a> </th>
  <th> <a href="Iois.jsp?si=<%= Constants.TAGiSymbol%>"
          title="Symbol - click here to sort by this column">Symbol</a> </th>
  <th> <a href="Iois.jsp?si=<%= Constants.TAGiSide%>"
          title="Buy/Sell etc - click here to sort by this column">Side</a> </th>
  <th> <a href="Iois.jsp?si=<%= Constants.TAGiIOIShares%>"
          title="IOI quantity - click here to sort by this column">Size</a> </th>
  <th title="IOI price"> Price </th>
  <th> <a href="Iois.jsp?si=<%= Constants.TAGiSenderCompID%>"
          title="Senders ID - click here to sort by this column">From</a> </th>
  <th> <a href="Iois.jsp?si=<%= Constants.TAGiTransactTime%>"
          title="Time of last change - click here to sort by this column">Entered</a> </th>
  <th> <a href="Iois.jsp?si=<%= Constants.TAGiValidUntilTime%>"
          title="Time that IOI expires - click here to sort by this column">Until</a> </th>
  <th> Block </th>
  <th title="Notes associated with IOI"> Comment </th>
  </tr>
  <%! int sort = 0; %>
  <%
    String requestedSort = request.getParameter( "si" );
    if (requestedSort != null) {
      int newSort = Integer.parseInt( requestedSort );
      sort = newSort;
    }

    String trClass = "plain0";

    ITagStringPairs[] iois = mmp.getIois( sort );
    for (int i=0; i < iois.length; i++) {

      if (i % 2 == 0) {
        trClass = "plain0";
      } else {
        trClass = "plain1";
      }

      ITagStringPairs ioi = iois[i];
      String ioiID = ioi.getValue( Constants.TAGiIOIid );
      String transType = ioi.getValue( Constants.TAGiIOITransType);

      //Display reject text if any.
      String text = mmp.getIoiResponseText( ioiID );
      if (text == null) {
        //Otherwise just display IOI text.
        text = ioi.getValue( Constants.TAGiText );
      }

      String validUntil = ioi.getValue( Constants.TAGiValidUntilTime);

      boolean cancelled = transType.startsWith( "Cancel" );

      boolean expired = false;
      if (validUntil != null) {
        long togo = mmp.computeMsecsFromNow( validUntil );
        expired = togo <= 0;
      }

      if (!(hideTheDead && (cancelled || expired))) {
  %>
      <tr class="<%= trClass %>">
      <td>
        <% if (buySide) {%>
          <!-- @todo Create order from IOI-->
        <%} else if (!cancelled && !expired) {%>
          <a href="IoiUpdate.jsp?op=cancel&id=<%= ioiID %>"
             title="Cancel this IOI - it is no longer valid"> Cancel </a> |
          <a href="IoiEntry.jsp?id=<%= ioiID %>"
             title="Change the details of this IOI"> Replace </a>
        <%}%>
      </td>
      <td> <%= transType %> </td>
          <td> <%= hideNull(ioi.getValue(Constants.TAGiSymbol)) %> </td>
          <td> <%= hideNull(ioi.getValue(Constants.TAGiSide)) %> </td>
          <td> <%= hideNull(ioi.getValue(Constants.TAGiIOIShares)) %> </td>
          <td> <%= hideNull(ioi.getValue(Constants.TAGiPrice)) %> </td>
          <td> <%= hideNull(ioi.getValue(Constants.TAGiSenderCompID)) %> </td>
          <td> <%= hideNull(ioi.getValue(Constants.TAGiTransactTime)) %> </td>
          <td> <%= hideNull(validUntil) %> </td>
          <td> </td>
          <td> <%= hideNull(text) %> </td>
      </tr>
  <%
      } //if not hidden
    } //for each
  %>
  </table>
</fieldset>
</body>
</html>
