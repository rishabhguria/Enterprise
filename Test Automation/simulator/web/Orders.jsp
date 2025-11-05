<html>
<head>
<title>
CameronFIX Orders Summary
</title>

<!-- This refreshes the page every 30 seconds-->
<meta http-equiv="REFRESH" content="30;Orders.jsp" />

<%@page import="com.cameronsystems.util.collection.ITagStringPairs" %>
<%@page import="com.cameronsystems.fix.configuration.Constants" %>

<link href="cameron.css" rel="stylesheet" media="all">

<script type="text/javascript">
<!--
function checkUnacked() {
  field = document.getElementById("unacked");
  if (field != null) {
    s = field.value;
    if (s.length > 0) {
      this.focus();
      alert( "Order " + s + " is waiting to be accepted." );
    }
  }
}
-->
</script>

</head>
<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />
<jsp:useBean id="mmp" scope="session" class="com.cameronsystems.fix.processor.MarketMirrorPopulatorAccessor" />
<jsp:useBean id="auth" scope="session" class="com.cameronsystems.fix.processor.AuthorizationBean" />

<%!
  boolean hideTheDead = true;

  String hideNull(String str) {
    if(str == null) {
      str = "";
    }
    return str;
  }
%>

<%
  if(!auth.isLoggedIn()) {
    response.sendRedirect("Logon.jsp?target=Orders.jsp");
  }

  //Determine whether we want a buy side or sell side view.
  boolean buySide = mmp.isBuySideView();

  String hideRequest = request.getParameter( "hi" );
  if (hideRequest != null) {
    hideTheDead = hideRequest.equals("1");
  }
%>

<body
<% if (!buySide) {%>
onload="checkUnacked();"
<%}%>
>

<!--NAVIGATION-->
<div id="navwrapper">
<ul id="nav">
  <li>               <a href="Iois.jsp">IOIs</a></li>
  <li>               <a href="Ats.jsp">Advertisements (ATs)</a></li>
  <li>               <a class="current">Orders</a></li>
  <li class="lastli"><a href="Logout.jsp">Logout</a></li>
</ul>
</div>
<!--END NAVIGATION-->

<h1>
CameronFIX "<%= app.getName() %>" Order Summary
</h1>

<!--
  Menu - could have multiple entries.
-->
<table>
  <tr>

  <!-- Only buy sides can create an order-->
  <%if (buySide) {%>
  <td>
  <a href="OrderEntry.jsp"> Add new order </a>
  </td>
  <%}%>

  <td>
    <% if (hideTheDead) {%>
      <a href="Orders.jsp?hi=0"> Show cancelled, done, rejected or filled orders </a>
    <%} else {%>
      <a href="Orders.jsp?hi=1"> Hide cancelled, done, rejected or filled orders </a>
    <%}%>
  </td>

  </tr>
</table>

<fieldset>
  <legend> Orders </legend>

  <!--
    This is the table of Orders.
  -->
  <table>
  <tr>
  <th title="Operations that you can perform on each order"> Action </th>
  <th> <a href="Orders.jsp?si=<%= Constants.TAGiClOrdID%>"
          title="Order ID - click here to sort by this column">ID</a> </th>
  <th> <a href="Orders.jsp?si=<%= Constants.TAGiSymbol%>"
          title="Symbol - click here to sort by this column">Symbol</a> </th>
  <th> <a href="Orders.jsp?si=<%= Constants.TAGiSide%>"
          title="Buy/Sell etc - click here to sort by this column">Side</a> </th>
  <th> <a href="Orders.jsp?si=<%= Constants.TAGiOrderQty%>"
          title="Order quantity - click here to sort by this column">Size</a> </th>
  <th> <a href="Orders.jsp?si=<%= Constants.TAGiOrdType%>"
          title="Type of order - click here to sort by this column">Type</a> </th>
  <th title="Order limit price"> Price </th>
  <th> <a href="Orders.jsp?si=<%= Constants.TAGiSenderCompID%>"
          title="Senders ID - click here to sort by this column">From</a> </th>
  <th> <a href="Orders.jsp?si=<%= Constants.TAGiOrdStatus%>"
          title="Current status of order - click here to sort by this column">Order Status</a> </th>
  <th> <a href="Orders.jsp?si=<%= Constants.TAGiTransactTime%>"
          title="Time of last change - click here to sort by this column">Modified</a> </th>
  <th> <a href="Orders.jsp?si=<%= Constants.TAGiLeavesQty%>"
          title="Quantity left to execute - click here to sort by this column">Leaves</a> </th>
  <th title="Calculated average price of all fills on this order"> Avg Px </th>
  <th title="Client notes sent with order"> Comment </th>
  </tr>
  <%! int sort = 0; %>
  <%
    String requestedSort = request.getParameter( "si" );
    if (requestedSort != null) {
      int newSort = Integer.parseInt( requestedSort );
      sort = newSort;
    }

    String trClass = "plain0";

    String unackedID = "";

    ITagStringPairs[] orders = mmp.getOrders( sort );
    for (int i=0; i < orders.length; i++) {

      if (i % 2 == 0) {
        trClass = "plain0";
      } else {
        trClass = "plain1";
      }

      ITagStringPairs order = orders[i];
      String id = order.getValue( Constants.TAGiClOrdID );

      //Display reject text if any.
      String text = mmp.getOrderResponseText( id );
      if (text == null) {
        //Otherwise just display IOI text.
        text = order.getValue( Constants.TAGiText );
      }

      String ordStatus = order.getValue( Constants.TAGiOrdStatus);
      boolean done = ordStatus.startsWith( "Done" );
      boolean filled = ordStatus.startsWith( "Filled" );
      boolean pending = ordStatus.startsWith( "Pending" );
      boolean rejected = ordStatus.startsWith( "Reject" );
      boolean cancelled = ordStatus.startsWith( "Cancel" );


      if ( pending ) {
        unackedID = id;
        trClass = "pending";
      }

      if (!(hideTheDead && (rejected || filled || done || cancelled))) {
  %>
      <tr class="<%= trClass %>">
      <td>
        <% if (buySide) {%>

          <!-- Buy side actions -->
          <% if (!pending && !filled && !cancelled && !rejected) {%>
            | <a href="OrderEntry.jsp?id=<%= id %>"
               title="Request to change the details of this order">
               Amend </a>
          <%}%>

          <% if (!filled && !cancelled && !rejected) {%>
            | <a href="OrderUpdate.jsp?op=cancel&id=<%= id %>"
               title="Request to cancel this order">
               Cancel </a>
          <%}%>

          <% if (!rejected) {%>
            | <a href="FillEntry.jsp?mode=history&id=<%= id %>"
               title="Show this order's fills - you can also reject fills">
               Fills
               </a>
          <%}%>

        <%} else {%>

          <!-- Sell side actions -->

          <% if (pending) {%>
            | <a href="OrderUpdate.jsp?op=accept&id=<%= id %>"
               title="Acknowledge the outstanding pending order request">
               Accept </a>
            | <a href="OrderUpdate.jsp?op=reject&id=<%= id %>"
               title="Reject the outstanding pending order request">
               Reject </a>
          <%}%>

          <% if (!pending && !filled && !cancelled && !rejected) {%>
            | <a href="FillEntry.jsp?id=<%= id %>"
               title="Add a partial or complete fill to the order">
               Fill </a>
          <%}%>

          <% if (!pending && !done && !cancelled && !rejected) {%>
            | <a href="OrderUpdate.jsp?op=done&id=<%= id %>"
               title="Done for day - Tells client that no more trading will occur today on this order">
               Done </a>
          <%}%>

          | <a href="FillEntry.jsp?mode=history&id=<%= id %>"
             title="Show the history of this order">
             History
             </a>
        <%}%>

      </td>
      <td> <%= id %> </td>
      <td> <%= order.getValue( Constants.TAGiSymbol) %> </td>
      <td> <%= order.getValue( Constants.TAGiSide) %> </td>
      <td> <%= order.getValue( Constants.TAGiOrderQty) %> </td>
      <td> <%= order.getValue( Constants.TAGiOrdType) %> </td>
      <td> <%= hideNull( order.getValue( Constants.TAGiPrice) ) %> </td>
      <td> <%= order.getValue( Constants.TAGiSenderCompID) %> </td>
      <td> <%= ordStatus %> </td>
      <td> <%= order.getValue( Constants.TAGiTransactTime) %> </td>
      <td> <%= hideNull( order.getValue( Constants.TAGiLeavesQty) )%> </td>
      <td> <%= order.getValue( Constants.TAGiAvgPx) %> </td>
      <td> <%= hideNull( text ) %> </td>
      </tr>
  <%
      } //if not hidden
    } //for each order
  %>
  <input type="hidden" name="unacked" id="unacked" value="<%= unackedID %>"/>
  </table>
</fieldset>
</body>
</html>
