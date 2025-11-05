<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<html>
  <head>
    <title>CameronFIX Exception Manager</title>

    <link href="cameron.css" rel="stylesheet" media="all">

  </head>
  <body>

  <h1>Enter a comment</h1>

  <%
    String action = request.getParameter("action");

    String selections[] = (String[]) request.getParameterMap().get("selection");
    if(selections == null || selections.length == 0) {
      response.sendRedirect("ExceptionView.jsp?error=No items selected&active=1");
      return;
    }
  %>

  <form action="ExceptionAction.jsp" method="post">
    <p>You are about to <%= action %> <%= selections.length %> message<%=selections.length != 1 ? "s" : "" %>.</p>
    <p>Please enter a reason: <input type="text" name="comment" size="40" >

    <button type="submit" > Continue </button>
    </p>

    <%
      for(int i = 0; i < selections.length; i++) {
        %>
        <input type="hidden" name="selection" value="<%= selections[i]%>" >
        <%
      }
    %>

    <input type="hidden" name="action" value="<%= action %>" >

  </form>

  </body>
</html>