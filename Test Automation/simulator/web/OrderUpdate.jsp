<%@ page errorPage="FillError.jsp" %>

<html>
<head>
<title>
CameronFIX Order Update
</title>
</head>
<jsp:useBean id="mmp" scope="session" class="com.cameronsystems.fix.processor.MarketMirrorPopulatorAccessor" />
<jsp:useBean id="auth" scope="session" class="com.cameronsystems.fix.processor.AuthorizationBean" />
<body>

<!--
  Update based on data passed in on request.
-->
<%
  String id = request.getParameter( "id" );
  String op = request.getParameter( "op" );
  if ( "accept".equals(op) ) {
    mmp.orderRequestAck( id, auth);
  } else if ( "reject".equals(op) ) {
    mmp.orderRequestReject( id, auth);
  } else if ( "done".equals(op) ) {
    mmp.orderDone( id, auth );
  } else if ( "cancel".equals(op) ) {
    mmp.orderCancel( id, auth);
  } else if ( "bust".equals(op) ) {
    mmp.fillCancel( id, auth);
  } else if ( "dk".equals(op) ) {
    mmp.fillDontKnow( id, auth);
  }

  //This will be set if we came here after pressing the Send button on the
  //fill entry form.
  String sendFill = request.getParameter( "sendFill" );
  if ( sendFill != null ) {
    mmp.orderFill( id, request.getParameterMap(), auth);
  }

  //This will be set if we came here after pressing the Send button on the
  //order entry form.
  String sendOrder = request.getParameter( "sendOrder" );
  if ( sendOrder != null ) {
    mmp.send( request.getParameterMap(), auth);
  }
%>

<!--
  Forward on to Orders page for redisplay.
-->
<%
  response.sendRedirect("Orders.jsp");
%>

</body>
</html>
