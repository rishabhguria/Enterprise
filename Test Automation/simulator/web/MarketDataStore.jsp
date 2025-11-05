<html>
<head>
<title>
Cameron Market Data Store View
</title>

<%@page import="com.cameronsystems.util.marketdata.*" %>
<%@page import="java.util.*" %>

<link href="cameron.css" rel="stylesheet" media="all">

</head>

<jsp:useBean id="store" scope="session" class="com.cameronsystems.util.marketdata.MarketDataStoreAccessor" />

<%
  String service = request.getParameter( "service" );
  if (service != null) {
    store.setMarketDataServiceName( service );
  }
%>

<body>

<h1>
Cameron Market Data Store View "<%=store.getMarketDataServiceName()%>"
</h1>
<h4>
COPYRIGHT (c) 1998-2006 Cameron Systems Pty Ltd. All Rights Reserved.
</h4>

<fieldset>


  <legend> Market Data - Top Of Book </legend>

  <%
    IInstrument[] insts = store.getInstruments();

    IFieldInfos infos = store.getFieldInfos();

    String customName = null;
    boolean haveCustom = infos.getMaxFieldID() > FieldInfos.STANDARD_FIELD_MAX;
    if (haveCustom) {
      IFieldInfo info = infos.getFieldInfo( infos.getMaxFieldID() );
      customName = info.getFieldName();
    }
  %>

  <!--
    This is the top of book table.
  -->
  <table>
  <tr>
  <th> Instrument </th>
  <th> Open </th>
  <th> Bid </th>
  <th> Ask </th>
  <th> Last </th>
  <th> High </th>
  <th> Low </th>
  <th> VWAP </th>
  <th> Close </th>
  <%
  if (haveCustom) {
  %>
    <th> <%= customName %> </th>
  <%
  }
  %>
  <th> Status </th>
  <th> CorporateAction </th>
  </tr>

  <%
    IMarketDataValue val;
    for (int i = 0; i < insts.length; i++) {
      IInstrument inst = insts[i];
      String symbol = inst.getSymbol();

      val = inst.getTopOfBookValue( FieldInfos.FIELD_OpeningPrice );
      String open = store.valueToString( val );

      String bid = store.qtyAtPriceToString( inst.getBestQty( true ),
                                             inst.getBestPrice( true ) );
      String ask = store.qtyAtPriceToString( inst.getBestQty( false ),
                                             inst.getBestPrice( false ) );
      String last = store.qtyAtPriceToString( inst.getLastTradeQty(),
                                              inst.getLastTradePrice() );

      val = inst.getTopOfBookValue( FieldInfos.FIELD_HighPrice );
      String high = store.valueToString( val );

      val = inst.getTopOfBookValue( FieldInfos.FIELD_LowPrice );
      String low = store.valueToString( val );

      val = inst.getTopOfBookValue( FieldInfos.FIELD_VWAPPrice );
      String vwap = store.valueToString( val );

      val = inst.getTopOfBookValue( FieldInfos.FIELD_ClosingPrice );
      String close = store.valueToString( val );

      String custom = null;
      if (haveCustom) {
        val = inst.getTopOfBookValue( infos.getMaxFieldID() );
        custom = store.valueToString( val );
      }

      val = inst.getTopOfBookValue( FieldInfos.FIELD_TradingStatus );
      String status = store.valueToString( val );

      val = inst.getTopOfBookValue( FieldInfos.FIELD_CorporateAction );
      String corporateAction = store.valueToString( val );

  %>
      <tr>
      <td> <a href="Detail.jsp?symbol=<%=symbol%>"
              title="Click here to display the depth of market for this instrument"
              target="_blank">
              <%= symbol %> </a> </td>
      <td> <%= open %> </td>
      <td> <%= bid %> </td>
      <td> <%= ask %> </td>
      <td> <%= last %> </td>
      <td> <%= high %> </td>
      <td> <%= low %> </td>
      <td> <%= vwap %> </td>
      <td> <%= close %> </td>
      <%
      if (haveCustom) {
      %>
        <td> <%= custom %> </td>
      <%
      }
      %>
      <td> <%= status %> </td>
      <td> <%= corporateAction %> </td>
      </tr>
  <%
    }
  %>
  </table>
</fieldset>
</body>
</html>
