<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Space">
		<xsl:param name="number"/>
		<xsl:if test="$number &gt;0">
			<xsl:variable name="blank" select="''"/>
			<xsl:value-of select="concat($blank,' ')"/>
			<xsl:call-template name="Space">
				<xsl:with-param name="number" select="$number - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
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

			<xsl:variable name="Filler">
				<xsl:call-template name="Space">
					<xsl:with-param name="number" select="961"/>
				</xsl:call-template>
			</xsl:variable>

			<HeaderTag>
				<xsl:value-of select="concat('HEADER','NMUN','PWEOD.TRADES','   ',$Year,$Month,$Day,substring-after(DateAndTime,':'),$Filler)"/>
			</HeaderTag>		
			

		</ThirdPartyFlatFileHeader>
	</xsl:template>
</xsl:stylesheet>
