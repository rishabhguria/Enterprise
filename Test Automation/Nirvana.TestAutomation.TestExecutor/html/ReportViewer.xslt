<?xml version="1.0" encoding="UTF-8"?>
<html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<body style="font-family:Arial;font-size:12pt;background-color:#aaaaaa">

<img src="http://www.nirvanasolutions.com/images/logo.png" />
<br />
<br /><br />

<table border="1" style="border-color:black">
	<tr>
		<td style="border-color:black"><b>Test Case</b></td>
		<td style="border-color:black"><b>Description</b></td>
		<td style "border-color:black"><b>Category</b></td>
		<td style="border-color:black"><b>Result</b></td>
	</tr>
	<xsl:for-each select="LogEntries/LogEntry">
		<tr>
			<td style="border-color:black"><xsl:value-of select="@TestCase" /></td>
			<td style="border-color:black"><xsl:value-of select="@Description" /></td>
			<td style="border-color:black"><xsl:value-of select="@Category" /></td>
			<td style="border-color:black"><xsl:value-of select="@Result" /></td>
		</tr>
	</xsl:for-each>
</table>

</body>
</html>