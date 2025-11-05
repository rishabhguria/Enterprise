<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string DateNow()
    {
    return(System.DateTime.Now.ToString("MMddhhmm"));
    }
  </msxsl:script>

  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

      <xsl:variable name ="varDateTime">
        <xsl:value-of select ="number(my:DateNow())"/>
      </xsl:variable>

      <xsl:variable name="varZeros">
        <xsl:call-template name="noofzeros">
          <xsl:with-param name="count" select="6-string-length(RecordCount)"/>
        </xsl:call-template>
      </xsl:variable>
      
			<Trailer>
        <xsl:value-of select="concat('XYZ       ABC', $varDateTime,'END', $varZeros, RecordCount,'EOD')"/>
      </Trailer>
			
		</ThirdPartyFlatFileFooter>
	</xsl:template>
</xsl:stylesheet>
