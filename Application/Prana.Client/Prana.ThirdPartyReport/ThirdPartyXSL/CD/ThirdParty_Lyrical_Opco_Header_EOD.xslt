<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileHeader">
		<ThirdPartyFlatFileHeader>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<xsl:variable name="Year">
				<xsl:value-of select="substring-after(substring-after(Date,'/'),'/')"/>
			</xsl:variable>

			<xsl:variable name="Month">
				<xsl:choose>
					<xsl:when test="string-length(substring-before(Date,'/'))=1">
						<xsl:value-of select="concat('0',substring-before(Date,'/'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring-before(Date,'/')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:variable name ="Day">
				<xsl:choose>
					<xsl:when test="string-length(substring-before(substring-after(Date,'/'),'/'))=1">
						<xsl:value-of select="concat('0',substring-before(substring-after(Date,'/'),'/'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring-before(substring-after(Date,'/'),'/')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			
			<HeaderTag>
				<xsl:value-of select="'HR'"/>
			</HeaderTag>
			<HeaderTag2>
				<xsl:value-of select="concat($Year,$Month,$Day)"/>
			</HeaderTag2>

		</ThirdPartyFlatFileHeader>
	</xsl:template>
</xsl:stylesheet>
