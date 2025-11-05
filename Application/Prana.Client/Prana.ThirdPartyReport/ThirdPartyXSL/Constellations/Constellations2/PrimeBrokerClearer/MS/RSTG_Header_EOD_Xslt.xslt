<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileHeader">
		<ThirdPartyFlatFileHeader>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<MessageType>
				<xsl:value-of select="'TRADE'"/>
			</MessageType>

			<BatchID>
				<!--<xsl:choose>
					<xsl:when test="string-length(substring-before(DateAndTime,'/'))=1">
						<xsl:value-of select ="concat('0',substring-before(DateAndTime,'/'),substring-before(substring-after(DateAndTime,'/'),'/'),substring-after(DateAndTime,':'))"/>
					</xsl:when>
					<xsl:otherwise>-->
						<!--<xsl:value-of select ="concat(substring-before(DateAndTime,'/'),substring-before(substring-after(DateAndTime,'/'),'/'),substring-after(DateAndTime,':'))"/>-->
					<!--</xsl:otherwise>
				</xsl:choose>-->
				<!--<xsl:value-of select ="TranfRefNumber"/>-->

				<xsl:value-of select ="substring-after(DateAndTime,':')"/>
			</BatchID>

			<xsl:variable name="Date">
				<xsl:value-of select ="substring-before(DateAndTime,':')"/>
			</xsl:variable>

			<SendTimestamp>
				<xsl:value-of select="concat(substring-after(substring-after($Date,'/'),'/'),substring-before($Date,'/'),substring-before(substring-after($Date,'/'),'/'),substring-after(DateAndTime,':'))"/>
			</SendTimestamp>

			<Code>
				<xsl:value-of select="'YRTS'"/>
			</Code>



		</ThirdPartyFlatFileHeader>
	</xsl:template>
</xsl:stylesheet>