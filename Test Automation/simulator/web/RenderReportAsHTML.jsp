
<%@ page errorPage="ErrorPage.jsp" %>
<%@ page import="net.sf.jasperreports.engine.*,
                 org.apache.commons.configuration.DatabaseConfiguration,
                 java.sql.Connection,
                 com.cameronsystems.fix.reporting.server.ReportServer,
                 java.net.URLEncoder,
                 com.cameronsystems.fix.reporting.servlet.RenderReportPageLogic,
                 com.cameronsystems.fix.reporting.servlet.SharedPageLogic,
                 com.cameronsystems.fix.reporting.servlet.GetReportParametersPageLogic,
                 com.cameronsystems.fix.reporting.servlet.convertors.ParameterConversionException" %>
<%@ page import="net.sf.jasperreports.engine.util.*" %>
<%@ page import="net.sf.jasperreports.engine.export.*" %>
<%@ page import="com.cameronsystems.fix.reporting.archiver.DatabaseConnectionManager" %>
<%@ page import="com.cameronsystems.fix.reporting.archiver.DBUtil" %>
<%@ page import="java.util.*" %>
<%@ page import="java.io.*" %>

<%
  DatabaseConnectionManager dcm =
          (DatabaseConnectionManager) session.getServletContext().getAttribute(
                  "com.cameronsystems.fix.reporting.archiver.DatabaseConnectionManager");

  String report = request.getParameter("__report");
  String format = request.getParameter("__format");

  System.out.println("Report: " + report);


  String refreshUrl = RenderReportPageLogic.makeUrl(request, "RenderReportAsHTML.jsp", report, "HTML", null);
  String viewPdfUrl = RenderReportPageLogic.makeUrl(request, "RenderReportAsHTML.jsp", report, "PDF", null);
  List reportParameterInfos = GetReportParametersPageLogic.getReportParameterInfos(session.getServletContext(), report);

  Map reportParameters;

  try {
    reportParameters = RenderReportPageLogic.convertReportParameters(request, reportParameterInfos);
  } catch (ParameterConversionException e) {
    String redirectUrl = RenderReportPageLogic.makeUrl(request, "ReportParameters.jsp", report, format, e);
    response.sendRedirect(redirectUrl);
    return;
  }


  System.out.println("Report: " + report);

  File reportsDir = (File) session.getServletContext().getAttribute(ReportServer.REPORTS_DIR_SERVLET_ATTR);

  File reportFile = new File(reportsDir, report + ".jasper");

  System.out.println("Report file: " + reportFile);
  if (!reportFile.exists())
    throw new JRRuntimeException("Report file " + reportFile + " not found");

  JasperReport jasperReport = (JasperReport)JRLoader.loadObject(reportFile.getPath());


  Connection conn = dcm.getConnection();

  try {
    JasperPrint jasperPrint =
            JasperFillManager.fillReport(
                    jasperReport,
                    reportParameters,
                    conn
            );

    JRHtmlExporter exporter = new JRHtmlExporter();

    Map imagesMap = new HashMap();
    session.setAttribute("IMAGES_MAP", imagesMap);

    exporter.setParameter(JRExporterParameter.JASPER_PRINT, jasperPrint);
    exporter.setParameter(JRExporterParameter.OUTPUT_WRITER, out);
    exporter.setParameter(JRHtmlExporterParameter.IMAGES_MAP, imagesMap);
    exporter.setParameter(JRHtmlExporterParameter.IMAGES_URI, "ReportImage.jsp?image=");
    exporter.setParameter(JRHtmlExporterParameter.HTML_HEADER,
			              "<html>\n"+
                          "<head>\n" +
                          "  <title>Report View</title>\n" +
                          "  <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>\n" +
                          "  <link href=\"reports.css\" rel=\"stylesheet\" media=\"all\">" +
                          "</head>\n" +
                          "<body>\n" +
                          "  <h1 id='title'>Report View</h1>\n" +
                          "  <div id='navwrapper'>\n" +
                          "    [ <a href='Reports.jsp'>Reports</a> | \n" +
                          "      <a href='" + refreshUrl + "'>Refresh</a> | \n" +
                          "      <a href='" + viewPdfUrl + "'>View PDF</a> ]\n" +
                          "  </div>\n");
    exporter.setParameter(JRHtmlExporterParameter.HTML_FOOTER,
                          "  <script language='javascript'>\n" +
                          "    pages = document.getElementsByTagName('table')\n" +
                          "    for(var i = 0; i < pages.length; i++) {\n" +
                          "      tableElem = pages[i]\n" +
                          "      if(tableElem.getAttributeNode('style')) {\n" +
                          "        tableElem.align = 'center'\n" +
                          "        tableElem.style.border = 'solid black 1px'\n" +
                          "      }\n" +
                          "    }\n" +
                          "  </script>\n" +
                          "</body>\n" +
                          "</html>\n");


    exporter.exportReport();
  } finally {
    DBUtil.close(conn);
  }
%>

