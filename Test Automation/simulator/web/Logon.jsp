<html>

<head>
  <title> CameronFIX Logon </title>
  <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />

  <link href="cameron.css" rel="stylesheet" media="all">
  <script src="validate.js" type="text/javascript"></script>

  <jsp:useBean id="auth" scope="session" class="com.cameronsystems.fix.processor.AuthorizationBean" />
  <jsp:useBean id="mmp" scope="session" class="com.cameronsystems.fix.processor.MarketMirrorPopulatorAccessor" />

  <%
    if(!auth.hasAuthorizationSource()) {
      auth.setAuthorizationSource(mmp.getAuthorizationSource());
    }
  %>

  <%
    // Not null we got here because the OK button was pushed
    String ok = request.getParameter("logonOK");

    // The pages to go to if the logon is successful
    String target = request.getParameter("target");

    if (auth.isLoggedIn()) {
      if(target == null) {
        // By default we go to the orders page
        target = "Orders.jsp";
      }
      response.sendRedirect( target );
    }

    // Error message that is displayed if something goes wrong.
    String errorMessage = null;

    if(ok != null) {
      // The button was pushed.  Hopefully the username and password will be ok,
      // and we can redirect to some other page.
      String username = request.getParameter("username");
      String password = request.getParameter("password");

      if(username != null && password != null) {
        auth.setUsername(username);
        auth.setPassword(password);

        if(auth.logon()) {
          if(target == null) {
            // By default we go to the orders page
            target = "Orders.jsp";
          }
          response.sendRedirect(target);
        } else {
          errorMessage = "Logon failed.  Please try again.";
        }
      } else {
        errorMessage = "You must supply a username and password";
      }

    } else {
      // The button was not pushed.  Continue on and display the page.
    }
  %>

</head>
<body onload="document.getElementById('username').focus();">
  <h1> CameronFIX Logon </h1>

  <form action="Logon.jsp" method="post" name="logonForm" id="logonForm">

    <% if(target != null) { %>
      <input type="hidden" name="target" value="<%= target %>">
    <% } %>

    <fieldset>
      <legend> Logon </legend>



      <table align="center">
        <% if(errorMessage != null) { %>
          <tr>
            <td colspan="2">
              <div class="errorMessage"> <%= errorMessage %> </div>
            </td>
          </tr>
        <% } %>

        <tr>
          <td><b>Username</b></td>
          <td><input type="text" name="username" id="username"></td>
        </tr>

        <tr>
          <td><b>Password</b></td>
          <td><input type="password" name="password" id="password"></td>
        </tr>

        <tr>
          <td></td>
          <td><input type="submit" name="logonOK" id="logonOK" value="OK"></td>
        </tr>
      </table>

    </fieldset>

  </form>

</body>

</html>