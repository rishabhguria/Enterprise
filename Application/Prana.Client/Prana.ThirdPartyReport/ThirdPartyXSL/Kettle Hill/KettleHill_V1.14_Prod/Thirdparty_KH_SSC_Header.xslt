<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileHeader">
		<ThirdPartyFlatFileHeader>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<MessageType>
				<xsl:value-of select="'KettleSSC'"/>
			</MessageType>

			<!--<BatchID>
				--><!--<xsl:value-of select="translate(translate(translate(DateAndTime,'/',''),' ',''),':','')"/>--><!--
				<xsl:choose>
					<xsl:when test="string-length(substring-before(DateAndTime,'/'))=1">
						<xsl:value-of select ="concat('0',substring-before(DateAndTime,'/'),substring-before(substring-after(DateAndTime,'/'),'/'),substring-after(DateAndTime,':'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="concat(substring-before(DateAndTime,'/'),substring-before(substring-after(DateAndTime,'/'),'/'),substring-after(DateAndTime,':'))"/>
					</xsl:otherwise>
				</xsl:choose>

			</BatchID>

			<xsl:variable name="Date">
				<xsl:value-of select ="substring-before(DateAndTime,':')"/>
			</xsl:variable>

			<SendTimestamp>
				<xsl:value-of select="concat(substring-after(substring-after($Date,'/'),'/'),substring-before($Date,'/'),substring-before(substring-after($Date,'/'),'/'),substring-after(DateAndTime,':'))"/>
			</SendTimestamp>-->

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

			<HeaderTag2>
				<!--<xsl:value-of select="concat($Year,$Month,$Day)"/>-->
				<xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
			</HeaderTag2>

			<Code>
				<xsl:value-of select="'C'"/>
			</Code>



		</ThirdPartyFlatFileHeader>
	</xsl:template>
</xsl:stylesheet>