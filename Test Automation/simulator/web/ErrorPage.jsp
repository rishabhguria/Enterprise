<%@ page import="java.io.StringWriter,
                 java.io.PrintWriter,
                 java.util.Map,
                 java.util.ArrayList,
                 java.util.Collections,
                 java.util.Iterator"%>
<%@ page isErrorPage="true" %>

<html>

<head>
  <title> Error </title>
  <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />

  <link href="cameron.css" rel="stylesheet" media="all">

</head>
<body>

<h1> Error </h1>

<p>
An error occured occured while processing the last request.
</p>

<p>
Please send a copy of the following text to
<a href="mailto:support@cameronsystems.com"><code>support@cameronsystems.com</code></a>
along with a description of what you were doing immeadiately prior to this error:
</p>

<div class="errorDump">

<strong>Url:</strong> <%=request.getAttribute("javax.servlet.error.request_uri")%> <br/>
<strong>Parameters;</strong> </br>
<%
  Map params = request.getParameterMap();
  ArrayList keys = new ArrayList(params.keySet());
  Collections.sort(keys);

  for (Iterator i = keys.iterator(); i.hasNext();) {
    String key = (String) i.next();
    String[] value = (String[]) params.get(key);

    StringBuffer valueStr = new StringBuffer();
    for(int j = 0; j < value.length; j++) {
      valueStr.append(value[j]);
      if(j + 1 < value.length) {
        valueStr.append(", ");
      }
    }

%>
    <code><%= key %>:&nbsp;<%= valueStr %></code><br/>
<%
  }


  String stackTrace = "";
  if(exception != null) {
    StringWriter writer;
    writer = new StringWriter(4096);
    exception.printStackTrace(new PrintWriter(writer));
    stackTrace = writer.toString();
  }
%>
<strong>Stack trace:</strong><br/>
<pre> <%= stackTrace %> </pre>
</div>

</body>
</html>