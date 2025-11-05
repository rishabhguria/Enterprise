<html>
<head>
<title>
CameronFIX Exception Manager
</title>

<!-- This refreshes the page every 30 seconds-->
<meta http-equiv="REFRESH" content="30;ExceptionView.jsp" />

<%@page session="true"
        import="com.cameronsystems.fix.ApplicationBean,
                javax.naming.NameNotFoundException,
                com.cameronsystems.fix.exceptionmanager.ExceptionManagerAccessorBean,
                com.cameronsystems.fix.configuration.Constants,
                java.util.List,
                java.util.Iterator,
                com.cameronsystems.fix.processor.breakout.BrokenOutMessageData,
                com.cameronsystems.fix.processor.breakout.IBreakOutReason,
                com.cameronsystems.fix.message.Message,
                java.util.Date,
                java.text.DateFormat,
                com.cameronsystems.fix.exceptionmanager.ExceptionManagerSecurityPolicy,
                com.cameronsystems.fix.processor.breakout.IBreakOutReason,
                com.cameronsystems.fix.message.Field,
                com.cameronsystems.fix.configuration.Constants,
                com.cameronsystems.fix.configuration.MessageInfos,
                com.cameronsystems.fix.dictionary.IMessageInfo"
        errorPage="ErrorPage.jsp" %>

<link href="cameron.css" rel="stylesheet" media="all">

<script type="text/javascript">
<!--
function displayPopup() {

  var numMessagesElem = document.getElementById("numMessages")
  var prevNumMessagesElem = document.getElementById("prevNumMessages")

  if (numMessagesElem != null && prevNumMessagesElem != null) {
    var numMessages = parseInt(numMessagesElem.value)
    var prevNumMessages = parseInt(prevNumMessagesElem.value)

    if (numMessages > prevNumMessages) {
      this.focus()
      alert( "There are " + numMessages + " exceptions awaiting your attention." )
    }
  }
}

function selectAll() {
  var tableElem = document.getElementById("exceptionTable")

  var selectAllCBElem = document.getElementById("selectAllCB")

  var chk = selectAllCBElem.checked

  var rows = tableElem.getElementsByTagName("tr")

  for(var i = 0; i < rows.length; i++) {
    var rowElem = rows.item(i)
    var rowInputElements = rowElem.getElementsByTagName("input")
    if(rowInputElements.length > 0 && rowInputElements.item(0).name == "selection") {
      var checkBoxElem = rowInputElements.item(0)
      if(!checkBoxElem.disabled) {
        checkBoxElem.checked = chk
      } else {
        checkBoxElem.checked = 0
      }
    }
  }

  activateButtons()
}

function activateButtons() {
  var tableElem = document.getElementById("exceptionTable")

  var rows = tableElem.getElementsByTagName("tr")

  var anyChecked = 0
  var allChecked = 1
  var nRows = 0

  for(var i = 0; i < rows.length; i++) {
    var rowElem = rows.item(i)
    var rowInputElements = rowElem.getElementsByTagName("input")
    if(rowInputElements.length > 0 && rowInputElements.item(0).name == "selection") {
      var checkBoxElem = rowInputElements.item(0)
      var checked = checkBoxElem.checked & !checkBoxElem.disabled
      anyChecked |= checked
      allChecked &= checked
      nRows++
    }
  }

  enableActionButtons("actionRowTop", anyChecked)
  enableActionButtons("actionRowBottom", anyChecked)

  var selectAllCBElem = document.getElementById("selectAllCB")
  selectAllCBElem.checked = allChecked && nRows > 0
}

function enableActionButtons(rowId, anyChecked) {
  var actionRow = document.getElementById(rowId)
  var buttons = actionRow.getElementsByTagName("input")
  for(var j = 0; j < buttons.length; j++) {
    var buttonElem = buttons.item(j)
    buttonElem.disabled = !anyChecked;
  }
}
-->
</script>

</head>
<jsp:useBean id="app" scope="session" class="com.cameronsystems.fix.ApplicationBean" />
<jsp:useBean id="exceptMgr" scope="session" class="com.cameronsystems.fix.exceptionmanager.ExceptionManagerAccessorBean">
  <jsp:setProperty name="exceptMgr" property="breakOutBoxServiceName" value="exceptionManagerBreakOutBox"/>
</jsp:useBean>


<%
  // Parameters are:
  // si -- the column to sort on.
  // offset -- the message to start dsiplaying at.
  // error -- an error message to dsiplay
  // active -- if we came to this page as the result of a user action.  If this
  // is true then the user is probably looking at the screen.

  String si = request.getParameter("si");
  String offsetStr = request.getParameter("offset");

  if(si != null && !si.equals(exceptMgr.getSortColumn())) {
    exceptMgr.setSortColumn(si);
    offsetStr = null;  // always start at offset 0 after changing sort.
  }


  int displayedRows = 20;  // @todo get this from some where (cookie maybe)

  int offset = 0;


  if(offsetStr != null) {
    try {
      offset = Integer.parseInt(offsetStr);
    } catch(NumberFormatException nfe) {
      offset = 0;
    }
  }


  List messages = exceptMgr.getMessages(request);

  String errorMsg = request.getParameter("error");

  String[] infoMsgs = request.getParameterValues("info");

  String warning = request.getParameter("warning");

  Object prevNumMessagesObj = session.getAttribute("numMessages");
  int prevNumMessages = prevNumMessagesObj == null ? 0 : ((Integer) prevNumMessagesObj).intValue();

  session.setAttribute("numMessages", new Integer(messages.size()));

%>

<body onload="displayPopup(); activateButtons()" >

<h1> CameronFIX Exception Manager "<%= app.getName() %>" </h1>

<%
  if(errorMsg != null) {
    %>
    <div class="errorMessage">
      <b>Error</b>: <%= errorMsg %>
    </div>
    <%
  }
%>

<%
  if(warning != null) {
    %>
    <div class="warningMessage">
      <b>Warning</b>: <%= warning %>
    </div>
    <%
  }
%>

<%
  if(infoMsgs != null && infoMsgs.length > 0) {
    %>
    <div class="infoMessage">
    <b>Information:</b>
    <%
      if(infoMsgs.length == 1) {
        %>
        <%= infoMsgs[0] %>
        <%
      } else {
        %>
        <ul>
        <%
          for(int i = 0; i < infoMsgs.length; i++) {
            %>
            <li><%= infoMsgs[i] %></li>
            <%
          }
        %>
        </ul>
        <%
      }
    %>
    </div>
    <%
  }
%>

<div class="viewStatus">Exceptions <%= messages.size() == 0 ? 0 : offset + 1 %> - <%= Math.min(messages.size(), offset + displayedRows) %>
of <%= messages.size()%> |
<%
  if(offset > 0) {
    %>
    <a href="ExceptionView.jsp?active=1&offset=<%= Math.max(0, offset - displayedRows) %>"> Previous</a> |
    <%
  } else {
    %>
    Previous |
    <%
  }
  if(offset + displayedRows < messages.size()) {
    %>
    <a href="ExceptionView.jsp?active=1&offset=<%= offset + displayedRows %>"> Next</a> |
    <%
  } else {
    %>
    Next |
    <%
  }
%>
</div>
<form action="ExceptionComment.jsp" method="post" name="exceptionView">
<table id="exceptionTable">

  <tr class="actionRow" id="actionRowTop">
    <td colspan="11">
      <input type="submit" name="action" value="Accept" />
      <input type="submit" name="action" value="Reject" />
    </td>
  </tr>

  <tr>
    <th rowspan="2" title="Select All"><input type="checkbox" id="selectAllCB" name="selectall" value="t" onclick="selectAll()" /></th>
    <th rowspan="2" title="Operations that you can perform on this message" > Actions </th>
    <th rowspan="2"><a href="ExceptionView.jsp?active=1&si=<%= ExceptionManagerAccessorBean.SORT_COLUMN_DATE %>"
        title="Time that the exception occured. Click here to sort by this column."> Time </a></th>
    <th rowspan="2"><a href="ExceptionView.jsp?active=1&si=<%= ExceptionManagerAccessorBean.SORT_COLUMN_SENDER %>"
        title="Sender - The party that sent this message.  Click here to sort by this column."> Sender </a></th>
    <th rowspan="2" >Type</th>
    <th rowspan="1" colspan="5" title="Message - the message"> Message </th>
    <th rowspan="2" title="The reasons that this message breached"> Breach Reasons </th>
  </tr>
  <tr>
    <th>Sec</th>
    <th>B/S</th>
    <th>Price</th>
    <th>Qty</th>
    <th>ClOrdID</th>
  </tr>

  <%
    long now = System.currentTimeMillis();

    int msgCnt = 0;

    for(Iterator i = messages.listIterator(offset); i.hasNext() && msgCnt < displayedRows; msgCnt++) {
      BrokenOutMessageData data = (BrokenOutMessageData) i.next();
      Message mess = data.getMessage();
      Date date = data.getDate();
      final long TWENTY_FOUR_HOURS = 24 * 60 * 60 * 1000;
      DateFormat dateFmt;
      if(now - date.getTime() >= TWENTY_FOUR_HOURS) {
        dateFmt = DateFormat.getDateTimeInstance();  // @todo feed in viewers locale, should be able to get this from headers
      } else {
        dateFmt = DateFormat.getTimeInstance();  // @todo ditto
      }
      String timeStr = dateFmt.format(date);
  %>
  <tr class="<%= msgCnt % 2 == 0 ? "plain0" : "plain1" %>">
     <%
      boolean canAction = ExceptionManagerSecurityPolicy.canAction(request, data);

      String disabledCheckBoxAttr = "";
      if(!canAction) {
        disabledCheckBoxAttr = "disabled='disabled'";
      }
     %>
     <td>
       <input type="checkbox" name="selection"  value="<%= data.getId() %>"  <%= disabledCheckBoxAttr %> onclick="activateButtons()" />
     </td>
     <td>
       <%
        if(canAction) {
          %>
          <a class="actionAccept" href="ExceptionComment.jsp?action=accept&selection=<%= data.getId() %>"> Accept </a>
          <hr/>
          <a class="actionReject" href="ExceptionComment.jsp?action=reject&selection=<%= data.getId() %>"> Reject </a>
          <%
        } else {
          %>
          <span class="greyedOut">
          Accept
          <hr/>
          Reject
          </span>
          <%
          }
       %>

     </td>
     <td>
       <%= timeStr %>
     </td>

     <td>
       <%= data.getConnectionPoint().getExternalParty() %>
     </td>

     <td>
       <%
        String msgType = mess.getMsgType();
        IMessageInfo info = (IMessageInfo) MessageInfos.getInstance().get(msgType);
        String msgName = "Unknown";
        if(info != null) {
          msgName = info.getMsgName();
        }
        if(Constants.MSGOrderCancelReplaceRequest.equals(msgType)) {
          msgName = "Amend";
        } else if(Constants.MSGOrder.equals(msgType)) {
          msgName = "Order";
        }
       %>
       <%= msgName %>
     </td>

     <%
      if(Constants.MSGOrder.equals(msgType) || Constants.MSGOrderCancelReplaceRequest.equals(msgType)) {
        String symbol = mess.getStringFieldValue(Constants.TAGiSymbol);
        String price;
        if(Constants.ORDTYPE_Market.equals(mess.getStringFieldValue(Constants.TAGiOrdType))) {
          price = "Mkt";
        } else {
          price = mess.getStringFieldValue(Constants.TAGiPrice);
        }
        String orderQty = mess.getStringFieldValue(Constants.TAGiOrderQty);
        String side = "";
        String clOrdId = mess.getStringFieldValue(Constants.TAGiClOrdID);
        Field sideField = mess.getField(Constants.TAGiSide);
        if(sideField != null) {
          if(sideField.getValueInfo() != null) {
            side = sideField.getValueInfo().getName();
          } else {
            side = sideField.getStringFieldValue();
          }
        }

        %>
        <td><%= symbol %></td>
        <td><%= side %></td>
        <td><%= price %></td>
        <td><%= orderQty %></td>
        <td align="center"><a href="ExceptionDetailView.jsp?id=<%= data.getId() %>">
          <%= clOrdId %>
        </a></td>

     <% } else { %>
       <td colspan="5"><a href="ExceptionDetailView.jsp?id=<%= data.getId() %>">
         <%= mess.toString(Message.FORMAT_SUMMARY_VERBOSE) %>
       </a></td>
     <% } %>


     <td>
     <%
      List reasons = data.getReasons();
      if(reasons.size() == 1) {
        IBreakOutReason reason = (IBreakOutReason) reasons.get(0);
     %>
        <%= reason.getReason() %>
        <%
      } else {
        %>
        <ol>
        <%
        for(Iterator j = reasons.iterator(); j.hasNext();) {
          IBreakOutReason reason = (IBreakOutReason) j.next();
        %>
        <li><%= reason.getReason() %></li>
          <%
        }
          %>
          </ol>
        <%
      }

        %>
        </td>
  </tr>
  <%
    } // End main loop
  %>

  <tr class="actionRow" id="actionRowBottom">
    <td colspan="11">
      <input type="submit" name="action" value="Accept" />
      <input type="submit" name="action" value="Reject" />
    </td>
  </tr>
</table>
<input type="hidden" name="numMessages" id="numMessages" value="<%= messages.size() %>"/>
<input type="hidden" name="prevNumMessages" id="prevNumMessages" value="<%= prevNumMessages %>"/>

</form>

</body>
</html>