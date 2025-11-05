<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileFooter">
    <ThirdPartyFlatFileFooter>
      <RowHeader>
        <xsl:value-of select ="'false'"/>
      </RowHeader>

		<xsl:variable name="newline">
			<xsl:text>&#13;</xsl:text>
		</xsl:variable>
		<xsl:variable name="lastline">
			<xsl:text>&#10;</xsl:text>
		</xsl:variable>

      <RecordType>
        <xsl:value-of select="'TLR'"/>
      </RecordType>

      <RecordCount>
        <xsl:value-of select ="concat(RecordCount,$newline,$lastline)"/>
      </RecordCount>

    </ThirdPartyFlatFileFooter>
  </xsl:template>
</xsl:stylesheet>
