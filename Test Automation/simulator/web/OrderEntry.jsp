<html>

<head>
<title>
CameronFIX Order Entry
</title>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />

<%@page import="com.cameronsystems.fix.configuration.Constants,
                java.util.Set,
                com.cameronsystems.fix.IConnectionPoint" %>
<%@page import="com.cameronsystems.util.collection.ITagStringPairs" %>
<%@page import="java.util.Iterator" %>
<jsp:useBean id="mmp" scope="session" class="com.cameronsystems.fix.processor.MarketMirrorPopulatorAccessor" />
<jsp:useBean id="auth" scope="session" class="com.cameronsystems.fix.processor.AuthorizationBean" />

<link href="cameron.css" rel="stylesheet" media="all">
<script src="validate.js" type="text/javascript"></script>

</head>

<%
  if(!auth.isLoggedIn()) {
    response.sendRedirect("Logon.jsp?target=OrderEntry.jsp");
  }
%>

<%!
  boolean isFirstWord( String word, String s ) {
    return s.startsWith( word + "\t" ) || s.startsWith( word + " " );
  }
%>

<%
  String id = request.getParameter( "id" );
  ITagStringPairs order = mmp.getOrder( id, false );

  boolean replace = id != null;

  String account = "";
  String currency = "";
  String deliverTo = "";
  String exDestination = "";
  String expireTime = "";
  String locateReqd = "";
  String maxFloor = "";
  String minQty = "";
  String ordType = "";
  String originalID = id == null ? "" : id;
  String price = "";
  String processCode = "";
  String securityID = "";
  String securityIDSource = "";
  String shares = "";
  String side = "";
  String symbol = "";
  String text = "";
  String timeInForce = "";
  String url = "";

  if (order != null) {
    String s;

    if ( (s = order.getValue(Constants.TAGiAccount)) != null ) account = s;
    if ( (s = order.getValue(Constants.TAGiCurrency)) != null ) currency = s;
    if ( (s = order.getValue(Constants.TAGiDeliverToCompID)) != null ) deliverTo = s;
    if ( (s = order.getValue(Constants.TAGiExDestination)) != null ) exDestination = s;
    if ( (s = order.getValue(Constants.TAGiExpireTime)) != null ) expireTime = s;
    if ( (s = order.getValue(Constants.TAGiLocateReqd)) != null ) locateReqd = s;
    if ( (s = order.getValue(Constants.TAGiMaxFloor)) != null ) maxFloor = s;
    if ( (s = order.getValue(Constants.TAGiMinQty)) != null ) minQty = s;
    if ( (s = order.getValue(Constants.TAGiOrdType)) != null ) ordType = s;
    if ( (s = order.getValue(Constants.TAGiPrice)) != null ) price = s;
    if ( (s = order.getValue(Constants.TAGiProcessCode)) != null ) processCode = s;
    if ( (s = order.getValue(Constants.TAGiSecurityID)) != null ) securityID = s;
    if ( (s = order.getValue(Constants.TAGiSecurityIDSource)) != null ) securityIDSource = s;
    if ( (s = order.getValue(Constants.TAGiOrderQty)) != null ) shares = s;
    if ( (s = order.getValue(Constants.TAGiSide)) != null ) side = s;
    if ( (s = order.getValue(Constants.TAGiSymbol)) != null ) symbol = s;
    if ( (s = order.getValue(Constants.TAGiText)) != null ) text = s;
    if ( (s = order.getValue(Constants.TAGiTimeInForce)) != null ) timeInForce = s;
    if ( (s = order.getValue(Constants.TAGiURLLink)) != null ) url = s;
  }
%>




<body onload="document.getElementById('54').focus();">

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
CameronFIX Order Entry <%= id == null ? "" : "for Order ID = " + id %>
</h1>

<form action="OrderUpdate.jsp" method="post" name="orderForm" id="orderForm"
      onsubmit="return validate([
                       ['cp', 'selected', 'Broker is required'],
                       ['55', 'selected', 'Symbol is required'],
                       ['38', 'notempty', 'Order quantity is required'],
                       ['38', 'number', 'Order quantity must be a number'],
                       ['22', 'securityIDSource', 'SecurityIDSource must be one of 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F, G or a number > 100'],
                       ['126', 'time', 'Expire time must be a time (HH:MM:SS) or date/time (YYYYMMDD-HH:MM:SS)'],
                       ['44', 'number', 'Price must be a number']
                       ]) && validateSecurityID();" >

  <!-- Hidden entry containing New order or Replace order message type
       depending on whether an existing entry was passed in. -->
  <input type="hidden" name="35" id="35"
    value="<%= replace ? Constants.MSGOrderCancelReplaceRequest :
                         Constants.MSGOrder %>"/>

  <!-- Hidden entry containing automatically generated ClOrdID id -->
  <input type="hidden" name="11" id="11" value="<%= mmp.getNewId() %>"/>

  <!-- Hidden entry only used for replace, containing original id -->
  <input type="hidden" name="41" id="41" value="<%= originalID %>"/>

  <%
  Iterator currencies = mmp.getCurrencies() == null ? null : mmp.getCurrencies().iterator();
  Iterator exchanges = mmp.getExchanges() == null ? null : mmp.getExchanges().iterator();
  Iterator symbols = mmp.getSymbols() == null ? null : mmp.getSymbols().iterator();
  %>

  <fieldset>
  <legend> Enter Order </legend>

    <fieldset>
    <legend> Basic </legend>
      <table>
        <tr>

          <td title="This indicates whether you are buying, selling and other variants."
              align="right">
              <strong>Side</strong> </td>
          <td title="This indicates whether you are buying, selling and other variants.">
            <select name="54" id="54">
              <option <% if (side.equals(""))%> selected="selected" <%;%> />

              <option value="1" <% if (side.equals("1"))%> selected="selected" <%;%>>
                Buy
              </option>

              <option value="2" <% if (side.equals("2"))%> selected="selected" <%;%>>
                Sell
              </option>

              <option value="3" <% if (side.equals("3"))%> selected="selected" <%;%>>
                Buy Minus
              </option>

              <option value="4" <% if (side.equals("4"))%> selected="selected" <%;%>>
                Sell Plus
              </option>

              <option value="5" <% if (side.equals("5"))%> selected="selected" <%;%>>
                Sell Short
              </option>

              <option value="6" <% if (side.equals("6"))%> selected="selected" <%;%>>
                Sell Short Exempt
              </option>

              <option value="8" <% if (side.equals("8"))%> selected="selected" <%;%>>
                Cross
              </option>

              <option value="9" <% if (side.equals("9"))%> selected="selected" <%;%>>
                Cross Short
              </option>

              <option value="A" <% if (side.equals("A"))%> selected="selected" <%;%>>
                Cross Short Exempt
              </option>

              <option value="D" <% if (side.equals("D"))%> selected="selected" <%;%>>
                Subscribe
              </option>

              <option value="E" <% if (side.equals("E"))%> selected="selected" <%;%>>
                Redeem
              </option>

              <option value="F" <% if (side.equals("F"))%> selected="selected" <%;%>>
                Lend
              </option>

              <option value="G" <% if (side.equals("G"))%> selected="selected" <%;%>>
                Borrow
              </option>
            </select>
          </td>

          <td title="Number of shares"
              align="right">
              <strong>Order Quantity</strong> </td>
          <td title="Number of shares">
            <input type="text" name="38" id="38" value="<%= shares %>" />
          </td>

          <td title="Symbol associated with this Order."
              align="right">
              <strong>Symbol</strong> </td>
          <td title="Symbol associated with this Order.">

            <% if (symbols == null) {%>
              <input type="text" name="55" id="55" value="<%= symbol %>" />
            <% } else { %>
              <select name="55" id="55">
                <%
                  boolean noSelection = symbol.equals("");
                  if (noSelection) {
                    out.println("<option selected=\"selected\"/>");
                  } else {
                    out.println("<option/>");
                  }

                  while (symbols.hasNext()) {
                    String s = (String) symbols.next();

                    String opt;
                    if (noSelection || !isFirstWord( symbol, s )) {
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

        <tr>
          <td title="Type of order such as Limit, Market, Stop etc."
              align="right">
              <strong>Order Type</strong> </td>
          <td title="Type of order such as Limit, Market, Stop etc.">
            <select name="40" id="40">
              <option <% if (ordType.equals(""))%> selected="selected" <%;%> />

              <option value="1" <% if (ordType.equals("1"))%> selected="selected" <%;%>>
                Market
              </option>

              <option value="2" <% if (ordType.equals("2"))%> selected="selected" <%;%>>
                Limit
              </option>

              <option value="3" <% if (ordType.equals("3"))%> selected="selected" <%;%>>
                Stop
              </option>

              <option value="4" <% if (ordType.equals("4"))%> selected="selected" <%;%>>
                Stop Limit
              </option>

              <option value="5" <% if (ordType.equals("5"))%> selected="selected" <%;%>>
                Market on Close
              </option>

              <option value="6" <% if (ordType.equals("6"))%> selected="selected" <%;%>>
                With or Without
              </option>

              <option value="7" <% if (ordType.equals("7"))%> selected="selected" <%;%>>
                Limit or Better
              </option>

              <option value="8" <% if (ordType.equals("8"))%> selected="selected" <%;%>>
                Limit With or Without
              </option>

              <option value="9" <% if (ordType.equals("9"))%> selected="selected" <%;%>>
                On Basis
              </option>

              <option value="A" <% if (ordType.equals("A"))%> selected="selected" <%;%>>
                On Close
              </option>

              <option value="B" <% if (ordType.equals("B"))%> selected="selected" <%;%>>
                Limit On Close
              </option>

              <option value="C" <% if (ordType.equals("C"))%> selected="selected" <%;%>>
                Forex - Market
              </option>

              <option value="D" <% if (ordType.equals("D"))%> selected="selected" <%;%>>
                Previously Quoted
              </option>

              <option value="E" <% if (ordType.equals("E"))%> selected="selected" <%;%>>
                Previously Indicated
              </option>

              <option value="F" <% if (ordType.equals("F"))%> selected="selected" <%;%>>
                Forex - Limit
              </option>

              <option value="G" <% if (ordType.equals("G"))%> selected="selected" <%;%>>
                Forex Swap
              </option>

              <option value="H" <% if (ordType.equals("H"))%> selected="selected" <%;%>>
                Forex - Previously Quoted
              </option>

              <option value="I" <% if (ordType.equals("I"))%> selected="selected" <%;%>>
                Funari
              </option>

              <option value="J" <% if (ordType.equals("J"))%> selected="selected" <%;%>>
                Market If Touched
              </option>

              <option value="K" <% if (ordType.equals("K"))%> selected="selected" <%;%>>
                Market with Leftover as Limit
              </option>

              <option value="L" <% if (ordType.equals("L"))%> selected="selected" <%;%>>
                Previous Fund Valuation Point
              </option>

              <option value="M" <% if (ordType.equals("M"))%> selected="selected" <%;%>>
                Next Fund Valuation Point
              </option>

              <option value="P" <% if (ordType.equals("P"))%> selected="selected" <%;%>>
                Pegged
              </option>
            </select>
          </td>

          <td title="Price - Optional price associated with Order."
              align="right">
              Price </td>
          <td title="Price - Optional price associated with Order.">
            <input type="text" name="44" id="44" value="<%= price %>" />
          </td>

          <td title="Comment - any text to be associated with Order."
              align="right">
              Comment </td>
          <td title="Comment - any text to be associated with Order.">
            <input type="text" name="58" id="58" size="40" value="<%= text %>" />
          </td>
        </tr>


        <%
          //It does not make sense to change the counterparties on a replace.
          //The replace should be sent to whoever the original message was sent to.
          if (!replace) {%>
          <tr>
            <td title="The broker (or other counterparty) to whom you want to send the order - only currently connected brokers are enabled"
                align="right">
                <strong>Broker</strong> </td>
            <td title="The broker (or other counterparty) to whom you want to send the order - only currently connected brokers are enabled">
              <select name="cp" id="cp">
                <option selected="selected" />

                <%
                  Set cps = mmp.getConnectionPoints();
                  for (Iterator i = cps.iterator(); i.hasNext();) {
                    IConnectionPoint cp = (IConnectionPoint) i.next();
                    boolean connected = mmp.isConnected(cp);
                %>
                    <option value="<%= cp.toFileString() %>"
                            <% if (!connected) {%> disabled="disabled" <%}%>>
                      <%= cp.getExternalParty() %>
                    </option>
                <%
                  }
                %>
              </select>
            </td>
          </tr>
        <%}%>

      </table>

      <fieldset>
      <legend> Handling Instructions </legend>

        <input type="radio" name="21" id="21" value="2" checked="checked"/>
        Automated execution order, public, broker intervention OK
        <br/>

        <input type="radio" name="21" id="21" value="1"/>
        Automated execution order, private, no broker intervention
        <br/>

        <input type="radio" name="21" id="21" value="3"/>
        Manual order, best execution

      </fieldset>

    </fieldset>

    <div align="center">
      <input type="submit" name="sendOrder" id="sendOrder" value="Send"/> &nbsp
    </div>

    <fieldset>
    <legend> Advanced </legend>

      <fieldset>
      <legend> Execution Instructions </legend>
        <table>
          <tr>
            <td>
              <input type="checkbox" name="18" id="18" value="1"/> Not held
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="2" /> Work
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="3" /> Go along
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="4" /> Over the day
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="5" /> Held
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="6" /> Participate don't initiate
            </td>
          </tr>
          <tr>
            <td>
              <input type="checkbox" name="18" id="18" value="7" /> Strict scale
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="8" /> Try to scale
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="9" /> Stay on bidside
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="0" /> Stay on offerside
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="A" /> No cross (forbidden)
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="B" /> OK to cross
            </td>
          </tr>
          <tr>
            <td>
              <input type="checkbox" name="18" id="18" value="C" /> Call first
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="D" /> Percent of volume
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="E" /> Do not increase
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="F" /> Do not reduce
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="G" /> All or none
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="I" /> Institutions only
            </td>
          </tr>
          <tr>
            <td>
              <input type="checkbox" name="18" id="18" value="N" /> Non negotiable
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="U" /> Customer display instruction
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="V" /> Netting
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="X" /> Trade along
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="Y" /> Try to stop
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="Z" /> Cancel if not best
            </td>
          </tr>
          <tr>
            <td>
              <input type="checkbox" name="18" id="18" value="b" /> Strict limit
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="c" /> Ignore price validity checks
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="e" /> Work to target strategy
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="S" /> Suspend
            </td>
          </tr>
          <tr>
            <td>
              <input type="checkbox" name="18" id="18" value="O" /> Opening peg
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="P" /> Market peg
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="R" /> Primary peg
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="L" /> Last peg (last sale)
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="M" /> Midprice peg
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="T" /> Fixed peg
            </td>
          </tr>
          <tr>
            <td>
              <input type="checkbox" name="18" id="18" value="W" /> Peg to VWAP
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="a" /> Trailing stop peg
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="d" /> Peg to limit price
            </td>
          </tr>
          <tr>
            <td>
              <input type="checkbox" name="18" id="18" value="H" /> Reinstate on system failure
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="Q" /> Cancel on system failure
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="J" /> Reinstate on trading halt
            </td>
            <td>
              <input type="checkbox" name="18" id="18" value="K" /> Cancel on trading halt
            </td>
          </tr>
        </table>
      </fieldset>

      <table>
        <tr>

          <td title="Account mnemonic as agreed between broker and institution"
              align="right">
              Account </td>
          <td title="Account mnemonic as agreed between broker and institution">
            <input type="text" name="1" id="1" value="<%= account %>" />
          </td>

          <td title="Maximum number of shares within an order to be shown on the exchange floor at any given time"
              align="right">
              Max Floor </td>
          <td title="Maximum number of shares within an order to be shown on the exchange floor at any given time">
            <input type="text" name="111" id="111" value="<%= maxFloor %>" />
          </td>

          <td title="Minimum quantity of an order to be executed"
              align="right">
              Min Quantity </td>
          <td title="Minimum quantity of an order to be executed">
            <input type="text" name="110" id="110" value="<%= minQty %>" />
          </td>
        </tr>

        <tr>

          <td title="Execution destination as defined by institution when order is entered"
              align="right">
              Exchange </td>
          <td title="Execution destination as defined by institution when order is entered">

            <% if (exchanges == null) {%>
              <input type="text" name="100" id="100" value="<%= exDestination %>">
            <% } else { %>
            <select name="100" id="100">
              <%
                boolean noSelection = exDestination.equals("");
                if (noSelection) {
                  out.println("<option selected=\"selected\"/>");
                } else {
                  out.println("<option/>");
                }

                while (exchanges.hasNext()) {
                  String s = (String) exchanges.next();

                  String opt;
                  if (noSelection || !isFirstWord( exDestination, s )) {
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

          <td title="Specifies how long the order remains in effect - if not specified DAY is assumed"
              align="right">
              Time In Force </td>
          <td title="Specifies how long the order remains in effect - if not specified DAY is assumed">
            <select name="59" id="59">
              <option <% if (timeInForce.equals(""))%> selected="selected" <%;%> />

              <option value="0" <% if (timeInForce.equals("0"))%> selected="selected" <%;%>>
                Day
              </option>

              <option value="1" <% if (timeInForce.equals("1"))%> selected="selected" <%;%>>
                Good till cancel
              </option>

              <option value="2" <% if (timeInForce.equals("2"))%> selected="selected" <%;%>>
                At the opening
              </option>

              <option value="3" <% if (timeInForce.equals("3"))%> selected="selected" <%;%>>
                Immediate or cancel
              </option>

              <option value="4" <% if (timeInForce.equals("4"))%> selected="selected" <%;%>>
                Fill or kill
              </option>

              <option value="5" <% if (timeInForce.equals("5"))%> selected="selected" <%;%>>
                Good till crossing
              </option>

              <option value="6" <% if (timeInForce.equals("6"))%> selected="selected" <%;%>>
                Good till date
              </option>

              <option value="7" <% if (timeInForce.equals("7"))%> selected="selected" <%;%>>
                At the close
              </option>
            </select>
          </td>

          <td title="Time/date of order expiration"
              align="right">
              Expiry Date/Time </td>
          <td title="Time/date of order expiration">
            <input type="text" name="126" id="126" value="<%= expireTime %>" />
          </td>

        </tr>

        <tr>

          <td title="Currency used for price."
              align="right">
              Currency </td>
          <td title="Currency used for price.">

            <% if (currencies == null) {%>
              <input type="text" name="15" id="15">
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

          <td title="URL to link to additional information."
              align="right">
              URL </td>
          <td title="URL to link to additional information.">
            <input type="text" name="149" id="149" value="<%= url %>" />
          </td>

          <td title="Processing code for sub-account - Regular is assumed if not specified"
              align="right">
              Process code </td>
          <td title="Processing code for sub-account - Regular is assumed if not specified">
            <select name="81" id="81">
              <option <% if (processCode.equals(""))%> selected="selected" <%;%> />

              <option value="0" <% if (processCode.equals("0"))%> selected="selected" <%;%>>
                Regular
              </option>

              <option value="1" <% if (processCode.equals("1"))%> selected="selected" <%;%>>
                Soft dollar
              </option>

              <option value="2" <% if (processCode.equals("2"))%> selected="selected" <%;%>>
                Step in
              </option>

              <option value="3" <% if (processCode.equals("3"))%> selected="selected" <%;%>>
                Step out
              </option>

              <option value="4" <% if (processCode.equals("4"))%> selected="selected" <%;%>>
                Soft dollar step in
              </option>

              <option value="5" <% if (processCode.equals("5"))%> selected="selected" <%;%>>
                Soft dollar step out
              </option>

              <option value="6" <% if (processCode.equals("6"))%> selected="selected" <%;%>>
                Plan sponsor
              </option>
            </select>
          </td>
        </tr>

        <tr>
          <td title="SecurityID - an additional identifier for this security"
              align="right">
            SecurityID
          </td>
          <td title="SecurityID - an additional identifier for this security">
            <input type="text" name="48" id="48" value="<%= securityID %>" />
          </td>

          <td title="SecurityIDSource (aka IDSource) - the source of the SecurityID"
              align="right">
            SecurityIDSource
          </td>
          <td title="SecurityIDSource (aka IDSource) - the source of the SecurityID">
            <input type="text" name="22" id="22" value="<%= securityIDSource %>" />
          </td>
        </tr>

        <tr>
          <td title="Indicates whether the broker is to locate the stock in conjunction with a short sell order"
              align="right">
            Locate Required
          </td>
          <td title="Indicates whether the broker is to locate the stock in conjunction with a short sell order">
            <select name="114" id="114">
              <option <% if (locateReqd.equals(""))%> selected="selected" <%;%>/>

              <option value="Y" <% if (locateReqd.equals("Y"))%> selected="selected" <%;%>>
                The broker must locate the stock
              </option>

              <option value="N" <% if (locateReqd.equals("N"))%> selected="selected" <%;%>>
                The broker is not required to locate the stock
              </option>
            </select>
          </td>
        </tr>

      </table>
    </fieldset>
    <div align="center">
      <input type="submit" name="sendOrder" id="sendOrder" value="Send"/> &nbsp
    </div>
  </fieldset>
</form>

</body>
</html>