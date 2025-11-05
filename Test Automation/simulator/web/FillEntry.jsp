<html>

<head>
<title>
CameronFIX Fill Entry
</title>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />

<%@page import="com.cameronsystems.fix.configuration.Constants" %>
<%@page import="com.cameronsystems.util.collection.ITagStringPairs" %>
<%@page import="java.util.Iterator" %>
<jsp:useBean id="mmp" scope="session" class="com.cameronsystems.fix.processor.MarketMirrorPopulatorAccessor" />
<jsp:useBean id="auth" scope="session" class="com.cameronsystems.fix.processor.AuthorizationBean" />



<link href="cameron.css" rel="stylesheet" media="all">
<script src="validate.js" type="text/javascript"></script>

</head>

<%
  if(!auth.isLoggedIn()) {
    response.sendRedirect("Logon.jsp?target=Orders.jsp");
    return;
  }
%>

<%! String sort;

  boolean isFirstWord( String word, String s ) {
    return s.startsWith( word + "\t" ) || s.startsWith( word + " " );
  }
%>

<%
  //Determine whether we want a buy side or sell side view.
  boolean buySide = mmp.isBuySideView();

  String id = request.getParameter( "id" );
  ITagStringPairs order = mmp.getOrder( id );

  String mode = request.getParameter( "mode" );
  boolean historyMode = mode != null && "history".equals( mode );

  String requestedSort = request.getParameter( "si" );
  if (requestedSort != null) {
    sort = requestedSort;
  }
  ITagStringPairs[] fills = mmp.getFills( id, sort );

  String shares = "";
  String price = "";
  String exchange = "";
  String capacity = "";
  String text = "";
  String currency = "";

  String leavesQtyStr = order.getValue( Constants.TAGiLeavesQty );
  if( leavesQtyStr == null ) {
    leavesQtyStr = "ERROR";
  }
%>

<body
<% if (!historyMode) {%>
  onload="document.getElementById('32').focus();"
<%}%>
>

<!--NAVIGATION-->
<div id="navwrapper">
<ul id="nav">
  <li>               <a href="Iois.jsp">IOIs</a></li>
  <li>               <a href="Ats.jsp">Advertisements (ATs)</a></li>
  <li>               <a href="Orders.jsp">Orders</a></li>
  <li class="lastli"><a href="Logout.jsp">Logout</a></li>
</ul>
</div>
<!--END NAVIGATION-->

<h1>
CameronFIX Fill Entry <%= id == null ? "" : "for Order " + id %>
</h1>
<fieldset>
  <legend> Order and its Fills </legend>

  <!--
    This is the order detail.
  -->
  <table>
    <tr>
      <th> Action </th>
      <th> ID </th>
      <th> Symbol </th>
      <th> Side </th>
      <th> Size </th>
      <th> Price </th>
      <th> From </th>
      <th> Order Status </th>
      <th> Modified </th>
      <th> Leaves </th>
      <th> Avg Px </th>
      <th> Comment </th>
      <th> <a title="Reason that fill was rejected by the other side (aka DKReason)"> Reject Reason </a></th>
    </tr>
    <tr>
      <td></td>
      <td> <%= id %> </td>
      <td> <%= order.getValue( Constants.TAGiSymbol) %> </td>
      <td> <%= order.getValue( Constants.TAGiSide) %> </td>
      <td> <%= order.getValue( Constants.TAGiOrderQty) %> </td>
      <td> <%= order.getValue( Constants.TAGiPrice) %> </td>
      <td> <%= order.getValue( Constants.TAGiSenderCompID) %> </td>
      <td> <%= order.getValue( Constants.TAGiOrdStatus) %> </td>
      <td> <%= order.getValue( Constants.TAGiTransactTime) %> </td>
      <td> <%= leavesQtyStr %> </td>
      <td> <%= order.getValue( Constants.TAGiAvgPx) %> </td>
      <td> <%= order.getValue( Constants.TAGiText) %> </td>
      <td>&nbsp;</td>
    </tr>

    <!--
      Now do fills
    -->
    <%
      for (int i=0; i < fills.length; i++) {
        ITagStringPairs fill = fills[i];
        String execid = fill.getValue( Constants.TAGiExecID );
        String qty = fill.getValue( Constants.TAGiLastShares);
        String dkReason = fill.getValue( Constants.TAGiDKReason );
        //Bit of a kludge to detect cancelled trades.
        //See @todo in FixMarketMirror.onFillCancelCorrect
        boolean cancelled;
        if (buySide) {
          cancelled = dkReason != null;
        } else {
          cancelled = qty == null || qty.equals( "0" );
        }
        dkReason = dkReason == null ? "" : dkReason;
    %>
        <tr>
        <td>
          <% if (!cancelled) {%>
            <% if (buySide) {%>
              <a href="OrderUpdate.jsp?op=dk&id=<%= execid %>"
                 title="Reject this trade as a Don't Know trade"> Reject </a>
            <%} else {%>
              <a href="OrderUpdate.jsp?op=bust&id=<%= execid %>"
                 title="Cancel (bust) this trade"> Cancel </a>
            <%}%>
          <%}%>
        </td>
        <td> <%= execid %> </td>
        <td></td>
        <td></td>
        <td> <%= qty %> </td>
        <td> <%= fill.getValue( Constants.TAGiLastPx) %> </td>
        <td></td>
        <td></td>
        <td> <%= fill.getValue( Constants.TAGiTransactTime) %> </td>
        <td></td>
        <td></td>
        <td> <%= fill.getValue( Constants.TAGiText) %> </td>
        <td> <%= dkReason %> </td>
        </tr>
    <%
      }
    %>

  </table>
</fieldset>

<% if(!historyMode) { %>

  <form action="OrderUpdate.jsp" method="post" name="fillForm" id="fillForm"
        onsubmit="return validate([
                         ['32', 'notempty', 'Shares is required'],
                         ['32', 'number', 'Shares must be a number'],
                         ['31', 'notempty', 'Price is required'],
                         ['31', 'number', 'Price must be a number']
                         ]);" >

    <!-- Hidden entry containing Order id -->
    <input type="hidden" name="id" id="id" value="<%= id %>"/>

    <%
    Iterator currencies = mmp.getCurrencies() == null ? null : mmp.getCurrencies().iterator();
    Iterator exchanges = mmp.getExchanges() == null ? null : mmp.getExchanges().iterator();
    %>

    <fieldset>
    <legend> Enter Fill </legend>

      <table>
        <tr>
          <td title="Number of shares in fill"
              align="right">
              <strong>Shares</strong> </td>
          <td title="Number of shares in fill">
            <input type="text" name="32" id="32" value="<%= shares %>" />
          </td>

          <td title="Price - Price at which fill was executed"
              align="right">
              <strong>Price</strong> </td>
          <td title="Price - Price at which fill was executed">
            <input type="text" name="31" id="31" value="<%= price %>" />
          </td>
        </tr>

        <tr>

          <td title="Exchange where trade occurred."
              align="right">
              Exchange </td>
          <td title="Exchange where trade occurred.">

            <% if (exchanges == null) {%>
              <input type="text" name="30" id="30">
            <% } else { %>
              <select name="30" id="30">
                <%
                  boolean noSelection = exchange.equals("");
                  if (noSelection) {
                    out.println("<option selected=\"selected\"/>");
                  } else {
                    out.println("<option/>");
                  }

                  while (exchanges.hasNext()) {
                    String s = (String) exchanges.next();

                    String opt;
                    if (noSelection || !isFirstWord( exchange, s )) {
                      opt = "<option>";
                    } else {
                      opt = "<option selected=\"selected\">";
                    }

                    out.println( opt );
                    out.println( s );
                    out.println( "</option>" );
                  }
                %>
              </select>
            <% } %>

          </td>

          <td title="Broker capacity in execution"
              align="right">
              Capacity </td>
          <td title="Broker capacity in execution">
            <select name="29" id="29">
              <option <% if (capacity.equals(""))%> selected="selected" <%;%> />

              <option value="1" <% if (capacity.equals("1"))%> selected="selected" <%;%>>
                Agent
              </option>

              <option value="2" <% if (capacity.equals("2"))%> selected="selected" <%;%>>
                Cross as agent
              </option>

              <option value="3" <% if (capacity.equals("3"))%> selected="selected" <%;%>>
                Cross as principal
              </option>

              <option value="4" <% if (capacity.equals("4"))%> selected="selected" <%;%>>
                Principal
              </option>
            </select>
          </td>
        </tr>

        <tr>

          <td title="Comment - any text to be associated with this fill"
              align="right">
              Comment </td>
          <td title="Comment - any text to be associated with this fill">
            <input type="text" name="58" id="58" value="<%= text %>" />
          </td>

          <td title="Currency used for price."
              align="right">
              Currency </td>
          <td title="Currency used for price.">

            <% if (currencies == null) {%>
              <input type="text" name="15" id="15" value="<%= currency %>"/>
            <% } else { %>
              <select name="15" id="15">
                <%
                  boolean noSelection = currency.equals("");
                  if (noSelection) {
                    out.println("<option selected=\"selected\"/>");
                  } else {
                    out.println("<option/>");
                  }

                  while (currencies.hasNext()) {
                    String s = (String) currencies.next();

                    String opt;
                    if (noSelection || !isFirstWord( currency, s )) {
                      opt = "<option>";
                    } else {
                      opt = "<option selected=\"selected\">";
                    }

                    out.println( opt );
                    out.println( s );
                    out.println( "</option>" );
                  }
                %>
              </select>
            <% } %>

          </td>
        </tr>
      </table>

      <div align="center">
        <input type="submit" name="sendFill" id="sendFill" value="Send"/> &nbsp
      </div>
    </fieldset>
  </form>
<% } else { // historyMode { %>
  <form action="Orders.jsp" method="get" name="okForm" id="okForm">
      <div align="center">
        <input type="submit" name="ok" id="ok" value="OK"/> &nbsp
      </div>
  </form>
<% } %>

</body>
</html>