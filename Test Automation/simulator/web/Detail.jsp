<html>
<head>
<title>
Cameron Market Data Instrument Depth
</title>

<%@page import="com.cameronsystems.util.marketdata.*" %>
<%@page import="java.util.*" %>

<link href="cameron.css" rel="stylesheet" media="all">

</head>

<jsp:useBean id="store" scope="session" class="com.cameronsystems.util.marketdata.MarketDataStoreAccessor" />

<%
  String symbol = request.getParameter( "symbol" );
  IInstrument inst = store.getInstrument( symbol );
%>

<body>

<h1>
 Depth of market for <%=symbol%>
</h1>
<h4>
COPYRIGHT (c) 1998-2006 Cameron Systems Pty Ltd. All Rights Reserved.
</h4>

<fieldset>
  <legend> Depth of market for <%=symbol%> </legend>
  <!--
    This is the depth table.
  -->
  <table>
  <tr>
  <th></th> <!-- For bid detail entries -->
  <th> Bid Qty</th>
  <th> Bid Price</th>
  <th> Ask Price</th>
  <th> Ask Qty</th>
  <th></th> <!-- For ask detail entries -->
  </tr>

<%
    IDepthEntry[] bids = inst.getDepth( true, true );
    int nBids = bids.length;

    IDepthEntry[] asks = inst.getDepth( false, true );
    int nAsks = asks.length;

    Iterator bidDetailIter = null;
    Iterator askDetailIter = null;

    boolean showBidDepth = false;
    boolean showAskDepth = false;

    int bidDepthIndex = -1;
    int askDepthIndex = -1;

    boolean finished = nAsks == 0 && nBids == 0;
    while (!finished) {
      IDepthEntry bidEntry = null, askEntry = null;

      if (bidDetailIter == null) {
        bidDepthIndex++;
        if (bidDepthIndex < nBids) {
          bidEntry = bids[ bidDepthIndex ];
          List detail = bidEntry.getDetail();
          if (detail != null) {
            bidDetailIter = detail.iterator();
          } else {
            bidDetailIter = null;
          }
          showBidDepth = true;
        }
      }

      if (askDetailIter == null) {
        askDepthIndex++;
        if (askDepthIndex < nAsks) {
          askEntry = asks[ askDepthIndex ];
          List detail = askEntry.getDetail();
          if (detail != null) {
            askDetailIter = detail.iterator();
          } else {
            askDetailIter = null;
          }
          showAskDepth = true;
        }
      }
%>
      <tr>
<%
      //Bid detail
      String detailStr = "";
//      if (!showBidDepth) {
        if (bidDetailIter != null) {
          if (bidDetailIter.hasNext()) {
            IDetailEntry det = (IDetailEntry)bidDetailIter.next();
            detailStr = det.getQty() + "@" + det.getPrice();
          } else {
            bidDetailIter = null;
          }
        }
//      }
%>
      <td> <%= detailStr %> </td>
<%
      //Bid depth qty and price
      String priceStr = "";
      String qtyStr = "";
      if (showBidDepth) {
        qtyStr = Integer.toString( bidEntry.getQty() );
        priceStr = Integer.toString( bidEntry.getPrice() );
        showBidDepth = false;
      }
%>
      <td> <%= qtyStr %> </td>
      <td> <%= priceStr %> </td>
<%
      //Ask depth price and qty
      priceStr = "";
      qtyStr = "";
      if (showAskDepth) {
        qtyStr = Integer.toString( askEntry.getQty() );
        priceStr = Integer.toString( askEntry.getPrice() );
        showAskDepth = false;
      }
%>
      <td> <%= priceStr %> </td>
      <td> <%= qtyStr %> </td>
<%
      //Ask detail
      detailStr = "";
//      if (!showAskDepth) {
        if (askDetailIter != null) {
          if (askDetailIter.hasNext()) {
            IDetailEntry det = (IDetailEntry)askDetailIter.next();
            detailStr = det.getQty() + "@" + det.getPrice();
          } else {
            askDetailIter = null;
          }
        }
//      }
%>
      <td> <%= detailStr %> </td>
      </tr>
<%
      //Now loop for any other detail entries.
      finished = bidDepthIndex >= nBids && askDepthIndex >= nAsks;
    }
%>
  </table>
</fieldset>
</body>
</html>
