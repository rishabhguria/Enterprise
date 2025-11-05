<%@ page import="com.cameronsystems.fix.processor.breakout.BrokenOutMessageData,
                 com.cameronsystems.fix.message.SmartMessageFactory,
                 com.cameronsystems.fix.message.Message,
                 com.cameronsystems.fix.configuration.Constants,
                 com.cameronsystems.fix.processor.breakout.IBreakOutReason,
                 java.util.Map,
                 java.util.Iterator,
                 com.cameronsystems.util.logger.ILogger,
                 com.cameronsystems.util.logger.LoggerManager,
                 com.cameronsystems.fix.processor.breakout.BrokenOutMessageData,
                 com.cameronsystems.fix.message.ISmartMessageFactory,
                 com.cameronsystems.util.market.IMarketMirror,
                 com.cameronsystems.util.collection.ITagStringPairs,
                 com.cameronsystems.util.market.IOrder,
                 com.cameronsystems.fix.processor.breakout.MessageAlreadyActionedException,
                 java.net.URLEncoder"
         errorPage="ErrorPage.jsp"  %>

<%! private ILogger logger = LoggerManager.getLogger(this.getClass()); %>

<jsp:useBean id="exceptMgr" scope="session" class="com.cameronsystems.fix.exceptionmanager.ExceptionManagerAccessorBean"/>

<%--
  Update based on data passed in on request.
--%>
<%
  if(logger.isDebugEnabled()) {
    Map params = request.getParameterMap();
    for (Iterator i = params.entrySet().iterator(); i.hasNext();) {
      Map.Entry ent = (Map.Entry) i.next();
      StringBuffer buf = new StringBuffer();
      buf.append(ent.getKey());
      buf.append(" ");
      String[] values = (String[]) ent.getValue();
      for (int j = 0; j < values.length; j++) {
        buf.append(values[j]).append(", ");
      }
      logger.debug("Parameters" + buf.toString());
    }
  }
%>

<%
  String action = request.getParameter("action");

  String selections[] = (String[]) request.getParameterMap().get("selection");
  if(selections == null || selections.length == 0) {
    response.sendRedirect("ExceptionView.jsp?error=No items selected&active=1");
    return;
  }

  int alreadyActioned = 0;

  String comment = request.getParameter("comment");

  for (int i = 0; i < selections.length; i++) {
    int id = Integer.parseInt(selections[i]);
    BrokenOutMessageData data = exceptMgr.getMessageById(request, id);

    logger.info(action + "ing "+ id + " " + data.toString());

    try {
      if("accept".equalsIgnoreCase(action)) {
        exceptMgr.onAccept(request, data, comment);
      } else if("reject".equalsIgnoreCase(action)) {
        ISmartMessageFactory factory = exceptMgr.getSmartMessageFactory(data); // @todo get FIX version for connection point.
        Message mess = data.getMessage();
        Message rejectMess;
        String reason;
        if(!data.getReasons().isEmpty()) {
          reason = ((IBreakOutReason) data.getReasons().get(0)).getReason();
        } else {
          reason = "None";
        }
        String msgType = mess.getMsgType();
        if(Constants.MSGOrder.equals(msgType)) {
          String orderId = exceptMgr.nextOrderId();
          rejectMess = factory.createOrderReject(mess, Constants.ORDREJREASON_BrokerOption, reason, "EM" + data.getId());
          rejectMess.setStringFieldValue(Constants.TAGiOrderID, orderId);
          data.putPolicyProperty(Constants.TAGOrderID, orderId);
        } else if(Constants.MSGOrderCancelReplaceRequest.equals(msgType)) {
          IMarketMirror mirror = exceptMgr.getMarketMirror(data);
          IOrder order = null;
          String origClOrdId = mess.getStringFieldValue(Constants.TAGiOrigClOrdID);
          if(origClOrdId != null) {
            order = mirror.getOrder(origClOrdId);
          }
          if(order == null) {
            logger.warn("Can't find order '" + origClOrdId + "' while rejecting message: " + mess.toString(Message.FORMAT_VERBOSE));
          }
          rejectMess = factory.createCancelReject(mess, order, Constants.CXLREJREASON_BrokerOption, reason);
        } else {
          // @todo only do this for > 4.2, otherwise send Reject.
          rejectMess = factory.createBusinessReject(mess, Constants.BUSINESSREJECTREASON_Other, reason);
        }
        exceptMgr.onReject(request, data, rejectMess, comment);
      } else {
        logger.warn("Unknown action: " + action);
      }
    } catch (MessageAlreadyActionedException e) {
      alreadyActioned++;
    }

    // @todo handle errors sensibly
  }

  String warning = "";
  if(alreadyActioned > 0) {
    warning = "&warning=" +
            URLEncoder.encode(String.valueOf(alreadyActioned) +
                              " message" + (alreadyActioned != 1 ? "s have" : " has") +
                              " already been " + action + "ed");
  }

  response.sendRedirect("ExceptionView.jsp?active=1" + warning);
  //@todo report success like we report errors.  e.g. ?success=10 messages rejected, use a jsp:redirect directive.
%>

