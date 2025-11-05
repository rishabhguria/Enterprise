<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileHeader">
    <ThirdPartyFlatFileHeader>

      <RowHeader>
        <xsl:value-of select ="'false'"/>
      </RowHeader>

      <RedordType>
        <xsl:value-of select="'HDR'"/>
      </RedordType>

		<xsl:variable name="Day">
		<xsl:choose>
			<xsl:when test="string-length(substring-before(substring-after(Date,'/'),'/'))=1">
				<xsl:value-of select="concat('0',substring-before(substring-after(Date,'/'),'/'))"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of  select="substring-before(substring-after(Date,'/'),'/')"/>
			</xsl:otherwise>
		</xsl:choose>
		</xsl:variable>
		<xsl:variable name="Month">
			<xsl:choose>
				<xsl:when test="string-length(substring-before(Date,'/'))=1">
					<xsl:value-of select="concat('0',substring-before(Date,'/'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="substring-before(Date,'/')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="Year" select="substring(substring-after(substring-after(Date,'/'),'/'),3,2)"/>


	
		<Date>
			<xsl:value-of select="concat($Month,$Day,$Year)"/>
			<!--<xsl:value-of select="concat(substring-before(Date,'/'),substring-after(substring-before(Date,'/'),'/'),substring(substring-after(substring-after(Date,'/'),'/'),3,2))"/>-->
		</Date>

		<!--<xsl:text>&#13;</xsl:text>-->

	</ThirdPartyFlatFileHeader>
  </xsl:template>
</xsl:stylesheet>
