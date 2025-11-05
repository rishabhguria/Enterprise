<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileHeader">
		<ThirdPartyFlatFileHeader>

			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>
			<RecordTypeCode>
				<xsl:value-of select="'HDR'"/>
			</RecordTypeCode>

			<TradeSourceID>
				<xsl:value-of select="'7731502'"/>
			</TradeSourceID>

			<FileSequenceNumber>
				<xsl:value-of select="'1'"/>
			</FileSequenceNumber>

			<xsl:variable name="Date">
				<xsl:value-of select ="substring-before(DateAndTime,':')"/>
			</xsl:variable>

			

			<TransmissionTime>
				<xsl:value-of select="concat(substring-after(substring-after($Date,'/'),'/'),substring-before($Date,'/'),substring-before(substring-after($Date,'/'),'/'),substring-after($Date,':'))"/>
			</TransmissionTime>

			<xsl:variable name="varDate">
				<xsl:value-of select ="substring-after(DateAndTime,':')"/>
			</xsl:variable>
			<TransmissionDate>
				<xsl:value-of select="concat(substring($varDate,1,2),':',substring($varDate,3,2),':',substring($varDate,3,2))"/>
			</TransmissionDate>

		</ThirdPartyFlatFileHeader>
	</xsl:template>
</xsl:stylesheet>
