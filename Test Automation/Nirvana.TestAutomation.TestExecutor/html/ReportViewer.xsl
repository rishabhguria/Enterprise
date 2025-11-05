<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <head>
        <title>Nirvana Test Report : Prana</title>
        <link rel="stylesheet" href="https://bootswatch.com/yeti/bootstrap.css" media="screen" />
        <script type="text/javascript" src="gstatic_charts.js"></script>
		<script src="http://www.kryogenix.org/code/browser/sorttable/sorttable.js"></script>
        <script type="text/javascript" >
          google.charts.load('current', {'packages':['corechart']});
          google.charts.setOnLoadCallback(drawChart);
          function drawChart() {

          var data = google.visualization.arrayToDataTable([
          ['Result', 'Numbers of Test Cases'],
          ['Passed', <xsl:value-of select="count(/LogEntries/LogEntry[@Result='Pass'])"/>],
          ['Failed', <xsl:value-of select="count(/LogEntries/LogEntry[@Result='Fail'])"/>]
          ]);

          var options = {
          title: ''
          };

          var chart = new google.visualization.PieChart(document.getElementById('piechart'));
          chart.draw(data, options);
          }
        </script>
      </head>
      <body>

        <nav class="navbar navbar-default">
          <div class="container">
            <img src="http://www.nirvanasolutions.com/images/logo.png" />
          </div>
        </nav>
        <div class="container">

          <div class="panel panel-default">
            <div class="panel-heading">
              <h5>Test Summary</h5>
            </div>
            <div class="panel-body">
              <div id="piechart" style="margin:0 auto;" />
            </div>
          </div>
          <table class="table table-striped table-hover sortable">
            <thead>
              <tr>
                <th class="col-md-1">Test Case</th>
                <th class="col-md-4">Description</th>
				<th class="col-md-1">Category</th>
                <th class="col-md-1">Result</th>
                <th class="col-md-5">Error</th>
              </tr>
            </thead>
            <tbody>
              <xsl:for-each select="LogEntries/LogEntry">
                <xsl:if test="@Result = 'Pass'">
                  <tr>
                    <td>
                      <xsl:value-of select="@TestCase" />
                    </td>
                    <td>
                      <xsl:value-of select="@Description" />
                    </td>
					 <td>
                      <xsl:value-of select="@Category" />
                    </td>
                    <td>
                      <font color="#006400">
                        <xsl:value-of select="@Result" />
                      </font>
                    </td>
                    <td>
                      <xsl:value-of select="@Error" />
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="@Result = 'Fail'">
                  <tr class="danger">
                    <td>
                      <xsl:value-of select="@TestCase" />
                    </td>
                    <td>
                      <xsl:value-of select="@Description" />
                    </td>
                    <td>
                      <xsl:value-of select="@Category" />
                    </td>
					<td>
                      <font color="#ff0000">
                        <xsl:value-of select="@Result" />
                      </font>
                    </td>
                    <td>
                      <xsl:value-of select="@Error" />
                    </td>
                  </tr>
                </xsl:if>
              </xsl:for-each>
            </tbody>
          </table>
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>