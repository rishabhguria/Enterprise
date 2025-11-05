<html>
<head>
<title>
CameronFIX Advertisement (AT) Summary
</title>

<!-- This refreshes the page every 30 seconds-->
<meta http-equiv="REFRESH" content="30;Ats.jsp" />

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
    response.sendRedirect("Logon.jsp?target=Ats.jsp");
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
  <li>               <a href="Iois.jsp">IOIs</a></li>
  <li>               <a class="current">Advertisements (ATs)</a></li>
  <li>               <a href="Orders.jsp">Orders</a></li>
  <li class="lastli"><a href="Logout.jsp">Logout</a></li>
</ul>
</div>
<!--END NAVIGATION-->

<h1>
CameronFIX "<%= app.getName() %>" Advertisement (AT) Summary
</h1>

<!--
  Menu - could have multiple entries.
-->
<table>
  <tr>

  <% if (!buySide) {%>
    <td>
    <a href="AtEntry.jsp"> Add new advertisement (AT) </a>
    </td>
  <%}%>

  <td>
    <% if (hideTheDead) {%>
      <a href="Ats.jsp?hi=0"> Show cancelled advertisements </a>
    <%} else {%>
      <a href="Ats.jsp?hi=1"> Hide cancelled advertisements </a>
    <%}%>
  </td>

  </tr>
</table>

<fieldset>
  <legend> Advertisements (ATs) </legend>

  <!--
    This is the table of ATs.
  -->
  <table>
  <tr>
  <th title="Operations that you can perform on each advertisement"> Action </th>
  <th> <a href="Ats.jsp?si=<%= Constants.TAGiAdvTransType%>"
          title="Last action performed - click here to sort by this column"> Status </th>
  <th> <a href="Ats.jsp?si=<%= Constants.TAGiSymbol%>"
          title="Symbol - click here to sort by this column"> Symbol </th>
  <th> <a href="Ats.jsp?si=<%= Constants.TAGiAdvSide%>"
          title="Buy/Sell/Cross/Trade - click here to sort by this column"> Side </th>
  <th> <a href="Ats.jsp?si=<%= Constants.TAGiShares%>"
          title="Trade quantity - click here to sort by this column"> Size </th>
  <th title="Trade price"> Price </th>
  <th> <a href="Ats.jsp?si=<%= Constants.TAGiTransactTime%>"
          title="Time of trade - click here to sort by this column"> Traded </th>
  <th> <a href="Ats.jsp?si=<%= Constants.TAGiLastMkt%>"
          title="Exchange where trade occurred - click here to sort by this column"> Exchange </th>
  <th title="Notes associated with advertisement"> Comment </th>
  </tr>
  <%! int sort = 0; %>
  <%
    String requestedSort = request.getParameter( "si" );
    if (requestedSort != null) {
      int newSort = Integer.parseInt( requestedSort );
      sort = newSort;
    }

    String trClass = "plain0";

    ITagStringPairs[] ats = mmp.getAdvertisements( sort );
    for (int i=0; i < ats.length; i++) {

      if (i % 2 == 0) {
        trClass = "plain0";
      } else {
        trClass = "plain1";
      }

      ITagStringPairs at = ats[i];
      String advID = at.getValue( Constants.TAGiAdvId );
      String transType = at.getValue( Constants.TAGiAdvTransType);

      //Display reject text if any.
      String text = mmp.getAdvertisementResponseText( advID );
      if (text == null) {
        //Otherwise just display AT text.
        text = at.getValue( Constants.TAGiText );
      }

      boolean cancelled = transType.startsWith( "Cancel" );

      if (!(hideTheDead && cancelled)) {
  %>
      <tr class="<%= trClass %>">
      <td>
        <% if (!buySide && !cancelled) {%>
          <a href="AtUpdate.jsp?op=cancel&id=<%= advID %>"
             title="Cancel this Advertisment (AT) - it is no longer valid"> Cancel </a> |
          <a href="AtEntry.jsp?id=<%= advID %>"
             title="Change the details of this Advertisment (AT)"> Replace </a>
        <%}%>
      </td>
      <td> <%= hideNull(at.getValue( Constants.TAGiAdvTransType)) %> </td>
      <td> <%= hideNull(at.getValue( Constants.TAGiSymbol)) %> </td>
      <td> <%= hideNull(at.getValue( Constants.TAGiAdvSide)) %> </td>
      <td> <%= hideNull(at.getValue( Constants.TAGiShares)) %> </td>
      <td> <%= hideNull(at.getValue( Constants.TAGiPrice)) %> </td>
      <td> <%= hideNull(at.getValue( Constants.TAGiTransactTime)) %> </td>
      <td> <%= hideNull(at.getValue( Constants.TAGiLastMkt)) %> </td>
      <td> <%= hideNull( text ) %> </td>
      </tr>
  <%
      } //if not hidden
    } //for each
  %>
  </table>
</fieldset>
</body>
</html>
