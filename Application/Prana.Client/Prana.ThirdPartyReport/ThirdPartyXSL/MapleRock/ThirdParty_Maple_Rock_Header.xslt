<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileHeader">
		<ThirdPartyFlatFileHeader>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>
			<MessageType>
				<xsl:value-of select="'HEADER'"/>
			</MessageType>
			<HeaderTag>
				<xsl:value-of select="'Maple Rock'"/>
			</HeaderTag>
			<xsl:variable name="Date">
				<xsl:value-of select ="substring-before(DateAndTime,':')"/>
			</xsl:variable>
			<SendTimestamp>
				<xsl:value-of select="concat(substring-after(substring-after($Date,'/'),'/'),substring-before($Date,'/'),substring-before(substring-after($Date,'/'),'/'))"/>
			</SendTimestamp>
			<!--<xsl:variable name="DateandTime">
				<xsl:value-of select ="substring-after(DateAndTime,':')"/>
			</xsl:variable>
			<Code>
				<xsl:value-of select="concat(substring($DateandTime,1,2),':',substring($DateandTime,3,2),':',substring($DateandTime,4,2))"/>
			</Code>-->
		</ThirdPartyFlatFileHeader>
	</xsl:template>
</xsl:stylesheet>