<html>

<head>
<title>
CameronFIX Advertisement (AT) Entry
</title>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />

<%@page import="com.cameronsystems.fix.configuration.Constants,
                com.cameronsystems.fix.IConnectionPoint,
                java.util.Set" %>
<%@page import="com.cameronsystems.util.collection.ITagStringPairs" %>
<%@page import="java.util.Iterator" %>
<jsp:useBean id="mmp" scope="session" class="com.cameronsystems.fix.processor.MarketMirrorPopulatorAccessor" />

<link href="cameron.css" rel="stylesheet" media="all">
<script src="validate.js" type="text/javascript"></script>

</head>

<jsp:useBean id="auth" scope="session" class="com.cameronsystems.fix.processor.AuthorizationBean" />

<%
  if(!auth.isLoggedIn()) {
    response.sendRedirect("Logon.jsp?target=Ats.jsp");
  }
%>


<%!
  boolean isFirstWord( String word, String s ) {
    return s.startsWith( word + "\t" ) || s.startsWith( word + " " );
  }
%>

<%
  String id = request.getParameter( "id" );
  ITagStringPairs at = mmp.getAdvertisement( id, false );

  boolean replace = id != null;

  String currency = "";
  String date = "";
  String deliverTo = "";
  String exchange = "";
  String originalID = id == null ? "" : id;
  String price = "";
  String securityID = "";
  String securityIDSource = "";
  String shares = "";
  String side = "";
  String symbol = "";
  String text = "";
  String time = "";
  String url = "";

  if (at != null) {
    String s;

    if ( (s = at.getValue(Constants.TAGiCurrency)) != null ) currency = s;
    if ( (s = at.getValue(Constants.TAGiTradeDate)) != null ) date = s;
    if ( (s = at.getValue(Constants.TAGiDeliverToCompID)) != null ) deliverTo = s;
    if ( (s = at.getValue(Constants.TAGiLastMkt)) != null ) exchange = s;
    if ( (s = at.getValue(Constants.TAGiPrice)) != null ) price = s;
    if ( (s = at.getValue(Constants.TAGiSecurityID)) != null ) securityID = s;
    if ( (s = at.getValue(Constants.TAGiSecurityIDSource)) != null ) securityIDSource = s;
    if ( (s = at.getValue(Constants.TAGiShares)) != null ) shares = s;
    if ( (s = at.getValue(Constants.TAGiAdvSide)) != null ) side = s;
    if ( (s = at.getValue(Constants.TAGiSymbol)) != null ) symbol = s;
    if ( (s = at.getValue(Constants.TAGiText)) != null ) text = s;
    if ( (s = at.getValue(Constants.TAGiTransactTime)) != null ) time = s;
    if ( (s = at.getValue(Constants.TAGiURLLink)) != null ) url = s;
  }
%>

<body onload="document.getElementById('55').focus();">

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
CameronFIX Advertisement (AT) Entry <%= id == null ? "" : "for ID = " + id %>
</h1>

<form action="AtUpdate.jsp" method="post" name="atForm" id="atForm"
      onsubmit="return validate([
                       ['55', 'selected', 'Symbol is required'],
                       ['4', 'selected', 'Side is required'],
                       ['53', 'notempty', 'Shares is required'],
                       ['22', 'securityIDSource', 'SecurityIDSource must be one of 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F, G or a number > 100'],
                       ['53', 'number', 'Shares must be a number'],
                       ['44', 'number', 'Price must be a number'],
                       ['75', 'date', 'Invalid trade date. Format is YYYYMMDD, eg 20041109'],
                       ['60', 'time', 'Invalid trade time. Format is HH:MM:SS, eg 09:50:07']
                       ]) && validateSecurityID();" >


  <!-- Hidden entry containing Advertisement message type -->
  <input type="hidden" name="35" id="35" value="<%= Constants.MSGAdvertisement%>"/>

  <!-- Hidden entry containing AdvTransType of New or Replace depending on
       whether an existing entry was passed in. -->
  <input type="hidden" name="5" id="5"
    value="<%= replace ? Constants.ADVTRANSTYPE_Replace :
                         Constants.ADVTRANSTYPE_New %>"/>

  <!-- Hidden entry containing automatically generated AdvId -->
  <input type="hidden" name="2" id="2" value="<%= mmp.getNewId()%>"/>

  <!-- Hidden entry only used for replace, containing original id -->
  <input type="hidden" name="3" id="3" value="<%= originalID %>"/>

  <%
  Iterator currencies = mmp.getCurrencies() == null ? null : mmp.getCurrencies().iterator();
  Iterator exchanges = mmp.getExchanges() == null ? null : mmp.getExchanges().iterator();
  Iterator symbols = mmp.getSymbols() == null ? null : mmp.getSymbols().iterator();
  %>

  <fieldset>
  <legend> Enter Advertisement (AT) </legend>

    <fieldset>
    <legend> Basic </legend>
      <table>
        <tr>
          <td title="Symbol associated with this Advertisement."
              align="right">
              <strong>Symbol</strong> </td>
          <td title="Symbol associated with this Advertisement.">

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

          <td title="This indicates the side of your advertised trade."
              align="right">
              <strong>Side</strong> </td>
          <td title="This indicates the side of your advertised trade.">
            <select name="4" id="4">
              <option <% if (side.equals("B"))%> selected="selected" <%;%>></option>
              <option value="B" <% if (side.equals("B"))%> selected="selected" <%;%>>
                Buy
              </option>
              <option value="S" <% if (side.equals("S"))%> selected="selected" <%;%>>
                Sell
              </option>
              <option value="X" <% if (side.equals("X"))%> selected="selected" <%;%>>
                Cross
              </option>
              <option value="T" <% if (side.equals("T"))%> selected="selected" <%;%>>
                Trade
              </option>
            </select>
          </td>

          <td title="Number of shares"
              align="right">
              <strong>Shares</strong> </td>
          <td title="Number of shares">
            <input type="text" name="53" id="53" value="<%= shares %>" />
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
          <td title="Price - Optional price associated with advertisement."
              align="right">
              Price </td>
          <td title="Price - Optional price associated with advertisement.">
            <input type="text" name="44" id="44" value="<%= price %>" />
          </td>

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
        </tr>
        <tr>
          <td title="Date of trade (YYYYMMDD) - if ommitted assumed to be today."
              align="right">
              Trade Date </td>
          <td title="Date of trade - if ommitted assumed to be today.">
            <input type="text" name="75" id="75" value="<%= date %>" />
          </td>

          <td title="Time of trade (HH:MM:SS or YYYYMMDD-HH:MM:SS)."
              align="right">
              Trade Time </td>
          <td title="Time of trade.">
            <input type="text" name="60" id="60" value="<%= time %>" />
          </td>
        </tr>
        <tr>
          <td title="Comment - any text to be associated with IOI."
              align="right">
              Comment
          </td>
          <td title="Comment - any text to be associated with IOI.">
            <input type="text" name="58" id="58" size="30" value="<%= text %>" />
          </td>
        </tr>
      </table>

    </fieldset>

    <fieldset>
    <legend> Advanced </legend>
      <table>
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
        </tr>
      </table>
    </fieldset>

    <fieldset>
    <legend> Counterparties </legend>
      <table>
        <tr>

        <%
          Set cps;

          //If replace, just show connection points already associated with this
          //id. Otherwise, for new messages, display all connection points.
          if (replace) {
            cps = mmp.getConnectionPoints( id );
          } else {
            cps = mmp.getConnectionPoints();
          }

          int nPerRow = 6;
          int c = 0;
          for (Iterator i = cps.iterator(); i.hasNext(); c++) {
            IConnectionPoint cp = (IConnectionPoint) i.next();

            if ( c != 0 && c % nPerRow == 0) out.println( "</tr><tr>" );

            boolean connected = mmp.isConnected(cp);
        %>
          <td>
            <%//Only allow input on new messages (ie not replaces).
              if (!replace) { %>
              <input type="checkbox" name="cp" id="cp"
                     value="<%= cp.toFileString() %>"
                     <%= connected ? "checked=\"checked\"" : "disabled=\"disabled\"" %> />
            <% } %>

            <%= cp.getExternalParty() %>
          </td>
        <%
          }
        %>
        </tr>
      </table>
    </fieldset>

    <fieldset>
    <legend> Special AUTEX routing </legend>
      <table>
        <tr>
          <td title="Autex list name - for example, L1 for institutions only, L3 for brokers and institutions."
              align="right">
            Autex List Name
          </td>
          <td title="Autex list name - for example, L1 for institutions only, L3 for brokers and institutions.">

            <%//Cannot change Autex list on a replace - just display value.
              if (replace) {%>
              <input type="text" name="128" id="128" value="<%= deliverTo%>"
                     readonly/>
            <% } else { %>
              <input type="text" name="128" id="128" />
            <% } %>
          </td>

          <% if (!replace) {%>
            <td>
              <input type="button" value="Institutions Only"
                     title="Press this button to set L1 as the Autex list name."
                     onclick="document.getElementById(128).value='L1';"/>
            </td>

            <td>
              <input type="button" value="Brokers and Institutions"
                     title="Press this button to set L3 as the Autex list name."
                     onclick="document.getElementById(128).value='L3';"/>
            </td>
          <% } %>

        </tr>
      </table>
    </fieldset>

    <div align="center">
      <input type="submit" name="send" id="send" value="Send"/> &nbsp
    </div>
  </fieldset>
</form>

</body>
</html>