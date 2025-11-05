<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string DateNow()
		{
		return(System.DateTime.Now.ToString("yyMMddhhmmss"));
		}
	</msxsl:script>
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

      <xsl:variable name="varRecordCount">
        <xsl:choose>
          <xsl:when test="(string-length(RecordCount)='1')">
            <xsl:value-of select="concat('T','0000',RecordCount)"/>
          </xsl:when>
          <xsl:when test="(string-length(RecordCount)='2')">
            <xsl:value-of select="concat('T','000',RecordCount)"/>
          </xsl:when>
          <xsl:when test="(string-length(RecordCount)='3')">
            <xsl:value-of select="concat('T','00',RecordCount)"/>
          </xsl:when>
          <xsl:when test="(string-length(RecordCount)='4')">
            <xsl:value-of select="concat('T','0',RecordCount)"/>
          </xsl:when>
          <xsl:when test="(string-length(RecordCount)='5')">
            <xsl:value-of select="concat('T',RecordCount)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="concat('T',RecordCount)"/>
          </xsl:otherwise>
        </xsl:choose> 
        
      </xsl:variable>

      <Trailer1>
        <xsl:value-of select ="$varRecordCount"/>
      </Trailer1>

      
		</ThirdPartyFlatFileFooter>
	</xsl:template>
</xsl:stylesheet>
