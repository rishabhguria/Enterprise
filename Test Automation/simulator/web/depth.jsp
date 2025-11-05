<html>
<head>
<title>
Cameron Market Data Demo
</title>

<!-- This refreshes the page every 30 seconds-->
<meta http-equiv="REFRESH" content="30;depth.jsp" />

<%@page import="com.cameronsystems.sse.DepthEntry" %>
<%@page import="java.util.*" %>

<link href="cameron.css" rel="stylesheet" media="all">

</head>

<jsp:useBean id="market" scope="session" class="com.cameronsystems.sse.MarketBean" />
<body>

<h1>
Cameron Market Data Demo
</h1>
<h4>
COPYRIGHT (c) 1998-2005 Cameron Systems Pty Ltd. All Rights Reserved.
</h4>

<fieldset>
  <legend> Performance Statistics </legend>
Number of updates processed <%= market.getNUpdates() %>
<p>
Current updates per second <%= market.getUpdatesPerSecond() %>
</fieldset>

<fieldset>
  <legend> Market Depth </legend>

  <!--
    This is the depth table.
  -->
  <table>
  <tr>
  <th> Buy Qty</th>
  <th> Buy Price</th>
  <th> Sell Price</th>
  <th> Sell Qty</th>
  </tr>

  <%

    List buys = market.getDepth( 0, true );
    int nBuys = buys.size();

    List sells = market.getDepth( 0, false );
    int nSells = sells.size();

    int nLines = nBuys;
    if (nSells > nLines) {
      nLines = nSells;
    }

    Iterator iterBuys = buys.iterator();
    Iterator iterSells = sells.iterator();
    for (int i = 0; i < nLines; i++) {
      DepthEntry buyEntry, sellEntry;
      if (iterBuys.hasNext()) {
        buyEntry = (DepthEntry) iterBuys.next();
      } else {
        buyEntry = null;
      }
      if (iterSells.hasNext()) {
        sellEntry = (DepthEntry) iterSells.next();
      } else {
        sellEntry = null;
      }
  %>
      <tr>
      <td> <%= buyEntry == null ? "" : Integer.toString(buyEntry.getQty()) %> </td>
      <td> <%= buyEntry == null ? "" : Integer.toString(buyEntry.getPrice()) %> </td>
      <td> <%= sellEntry == null ? "" : Integer.toString(sellEntry.getPrice()) %> </td>
      <td> <%= sellEntry == null ? "" : Integer.toString(sellEntry.getQty()) %> </td>
      </tr>
  <%
    }
  %>
  </table>
</fieldset>
</body>
</html>
