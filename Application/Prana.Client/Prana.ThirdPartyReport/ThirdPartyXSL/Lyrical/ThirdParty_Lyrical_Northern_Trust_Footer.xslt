<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:variable name="GreaterThan" select="'&gt;'"/>
	<xsl:variable name="LessThan" select="'&lt;'"/>
	
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>

			<RecordType>
				<xsl:value-of select="'TLR'"/>
			</RecordType>

			<GFF>
				<xsl:value-of select="'1.0'"/>
			</GFF>

			<GeneralInfo>
				<xsl:value-of select="concat($LessThan,$LessThan,'GeneralInfo',$GreaterThan,$GreaterThan)"/>
			</GeneralInfo>

			<SenderId>
				<xsl:value-of select="'NIRSO1S'"/>
			</SenderId>

			<TradeCount>
				<xsl:value-of select="RecordCount"/>
			</TradeCount>
			
			<TotalSettlementAmount>
				<xsl:value-of select="InternalNetNotional"/>
			</TotalSettlementAmount>

			<TotalNominal>
				<xsl:value-of select="TotalQty"/>
			</TotalNominal>
			
		</ThirdPartyFlatFileFooter>
	</xsl:template>
</xsl:stylesheet>