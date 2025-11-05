<%@ page errorPage="IoiError.jsp" %>

<html>
<head>
<title>
CameronFIX IOI Update
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
  if ( "cancel".equals(op) ) {
    mmp.ioiCancel( id, auth);
  }

  //This will be set if we came here after pressing the Send button on the
  //data entry form.
  String send = request.getParameter( "send" );
  if ( send != null ) {
    mmp.send( request.getParameterMap(), auth);
  }
%>

<!--
  Forward on to IOIs page for redisplay.
-->
<%
  response.sendRedirect("Iois.jsp");
%>

</body>
</html>
