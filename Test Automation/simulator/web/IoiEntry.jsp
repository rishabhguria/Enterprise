<html>

<head>
<title>
CameronFIX IOI Entry
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
    response.sendRedirect("Logon.jsp?target=IoiEntry.jsp");
  }
%>

<%!
  boolean isFirstWord( String word, String s ) {
    return s.startsWith( word + "\t" ) || s.startsWith( word + " " );
  }
%>

<%
  String id = request.getParameter( "id" );
  ITagStringPairs ioi = mmp.getIoi( id, false );

  boolean replace = id != null;

  String currency = "";
  String deliverTo = "";
  String duration = "";
  String natural = "";
  String originalID = id == null ? "" : id;
  String price = "";
  String quality = "";
  String securityID = "";
  String securityIDSource = "";
  String shares = "";
  String side = Constants.SIDE_Undisclosed;
  String symbol = "";
  String text = "";
  String url = "";

  if (ioi != null) {
    String s;

    if ( (s = ioi.getValue(Constants.TAGiCurrency)) != null ) currency = s;
    if ( (s = ioi.getValue(Constants.TAGiDeliverToCompID)) != null ) deliverTo = s;
    if ( (s = ioi.getValue(Constants.TAGiValidUntilTime)) != null ) duration = s;
    if ( (s = ioi.getValue(Constants.TAGiIOINaturalFlag)) != null ) natural = s;
    if ( (s = ioi.getValue(Constants.TAGiPrice)) != null ) price = s;
    if ( (s = ioi.getValue(Constants.TAGiIOIQltyInd)) != null ) quality = s;
    if ( (s = ioi.getValue(Constants.TAGiSecurityID)) != null ) securityID = s;
    if ( (s = ioi.getValue(Constants.TAGiSecurityIDSource)) != null ) securityIDSource = s;
    if ( (s = ioi.getValue(Constants.TAGiIOIShares)) != null ) shares = s;
    if ( (s = ioi.getValue(Constants.TAGiSide)) != null ) side = s;
    if ( (s = ioi.getValue(Constants.TAGiSymbol)) != null ) symbol = s;
    if ( (s = ioi.getValue(Constants.TAGiText)) != null ) text = s;
    if ( (s = ioi.getValue(Constants.TAGiURLLink)) != null ) url = s;

    //Compute duration.
    if (duration.length() > 0) {
      long togo = mmp.computeMsecsFromNow( duration );
      if (togo < 0) {
        duration = "0";
      } else {
        //Convert to minutes.
        duration = Long.toString( togo / (60*1000) );
      }
    }
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
CameronFIX IOI Entry <%= id == null ? "" : "for IOI ID = " + id %>
</h1>

<form action="IoiUpdate.jsp" method="post" name="ioiForm" id="ioiForm"
      onsubmit="return validate([
                       ['55', 'selected', 'Symbol is required'],
                       ['27', 'notempty', 'Shares is required'],
                       ['27', 'ioisize', 'Shares must be a number or S, M or L'],
                       ['22', 'securityIDSource', 'SecurityIDSource must be one of 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F, G or a number > 100'],
                       ['44', 'number', 'Price must be a number'],
                       ['duration', 'number', 'Duration must be a number']
                       ]) && validateSecurityID();" >

  <!-- Hidden entry containing IOI message type -->
  <input type="hidden" name="35" id="35" value="<%= Constants.MSGIOI%>"/>

  <!-- Hidden entry containing IOITransType of New or Replace depending on
       whether an existing entry was passed in. -->
  <input type="hidden" name="28" id="28"
    value="<%= replace ? Constants.IOITRANSTYPE_Replace :
                         Constants.IOITRANSTYPE_New %>"/>

  <!-- Hidden entry containing automatically generated IOI id -->
  <input type="hidden" name="23" id="23" value="<%= mmp.getNewId() %>"/>

  <!-- Hidden entry only used for replace, containing original id -->
  <input type="hidden" name="26" id="26" value="<%= originalID %>"/>

  <%
  Iterator currencies = mmp.getCurrencies() == null ? null : mmp.getCurrencies().iterator();
  Iterator symbols = mmp.getSymbols() == null ? null : mmp.getSymbols().iterator();
  %>

  <fieldset>
  <legend> Enter IOI </legend>

    <fieldset>
    <legend> Basic </legend>
      <table>
        <tr>
          <td title="Symbol associated with this IOI."
              align="right">
              <strong>Symbol</strong> </td>
          <td title="Symbol associated with this IOI.">

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

          <td title="This indicates whether you are buying, selling or not revealing your intention."
              align="right">
              <strong>Side</strong> </td>
          <td title="This indicates whether you are buying, selling or not revealing your intention.">
            <select name="54" id="54">
              <option value="1" <% if (side.equals("1"))%> selected="selected" <%;%>>
                Buy
              </option>

              <option value="2" <% if (side.equals("2"))%> selected="selected" <%;%>>
                Sell
              </option>

              <option value="7" <% if (side.equals("7"))%> selected="selected" <%;%>>
                Undisclosed
              </option>
            </select>
          </td>

          <td title="Number of shares - a number or S for small, M for medium or L for large"
              align="right">
              <strong>Shares</strong> </td>
          <td title="Number of shares - a number or S for small, M for medium or L for large">
            <input type="text" name="27" id="27" value="<%= shares %>" />
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
          <td title="Price - Optional price associated with IOI."
              align="right">
              Price </td>
          <td title="Price - Optional price associated with IOI.">
            <input type="text" name="44" id="44" value="<%= price %>" />
          </td>

          <td title="Duration in minutes used to calculate when the IOI expires."
              align="right">
              Duration (minutes) </td>
          <td title="Duration in minutes used to calculate when the IOI expires.">
            <input type="text" name="duration" id="duration" value="<%= duration %>"/>
          </td>

          <td title="Comment - any text to be associated with IOI."
              align="right">
              Comment </td>
          <td title="Comment - any text to be associated with IOI.">
            <input type="text" name="58" id="58" value="<%= text %>" />
          </td>
        </tr>
      </table>

      <fieldset>
      <legend> Qualifiers </legend>
        <table>
          <tr>
            <td>
              <input type="checkbox" name="104" id="104" value="A"/> All or none
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="C" /> At the close
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="I" /> In touch with
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="L" /> Limit
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="M" /> More behind
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="O" /> At the open
            </td>
          </tr>
          <tr>
            <td>
              <input type="checkbox" name="104" id="104" value="P" /> Taking a position
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="Q" /> At the market
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="R" /> Ready to trade
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="S" /> Portfolio show
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="T" /> Through the day
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="V" /> Versus
            </td>
          </tr>
          <tr>
            <td>
              <input type="checkbox" name="104" id="104" value="W" /> Indication - Working away
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="X" /> Crossing opportunity
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="Y" /> At the midpoint
            </td>
            <td>
              <input type="checkbox" name="104" id="104" value="Z" /> Pre-open
            </td>
          </tr>
        </table>
      </fieldset>

    </fieldset>

    <fieldset>
    <legend> Advanced </legend>
      <table>
        <tr>
          <td title="Relative quality of indication."
              align="right">
              Quality </td>
          <td title="Relative quality of indication.">
            <select name="25" id="25">
              <option <% if (quality.equals(""))%> selected="selected" <%;%> />

              <option value="L" <% if (quality.equals("L"))%> selected="selected" <%;%>>
                Low
              </option>

              <option value="M" <% if (quality.equals("M"))%> selected="selected" <%;%>>
                Medium
              </option>

              <option value="H" <% if (quality.equals("H"))%> selected="selected" <%;%>>
                High
              </option>
            </select>
          </td>

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

          <td title="Indicates that IOI is the result of an existing agency order or a facilitation position resulting from an agency order, not from principal trading or order solicitation activity."
              align="right">
              Natural </td>
          <td title="Indicates that IOI is the result of an existing agency order or a facilitation position resulting from an agency order, not from principal trading or order solicitation activity.">
            <select name="130" id="130">
              <option <% if (natural.equals(""))%> selected="selected" <%;%>/>

              <option value="Y" <% if (natural.equals("Y"))%> selected="selected" <%;%>>
                Natural
              </option>

              <option value="N" <% if (natural.equals("N"))%> selected="selected" <%;%>>
                Not natural
              </option>
            </select>
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