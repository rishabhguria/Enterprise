<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileFooter">
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>
			<Trailer>
				<xsl:value-of select ="'Trading Broker'"/>
			</Trailer>
			<RecordCount>
				<xsl:value-of select ="'JPM Securities LLC'"/>
			</RecordCount>			
			
		</ThirdPartyFlatFileFooter>

		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>
			<Trailer>
				<xsl:value-of select ="'Trade Settlement'"/>
			</Trailer>
			<RecordCount>
				<xsl:value-of select ="'DTC 352'"/>
			</RecordCount>
			
		</ThirdPartyFlatFileFooter>
		
		
		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>
			<Trailer>
				<xsl:value-of select ="'Account Name:'"/>
			</Trailer>
			<RecordCount>
				<xsl:value-of select ="'Harbrook Limited'"/>
			</RecordCount>
		
		</ThirdPartyFlatFileFooter>

		<ThirdPartyFlatFileFooter>
			<RowHeader>
				<xsl:value-of select ="'false'"/>
			</RowHeader>
			<Trailer>
				<xsl:value-of select ="'Account Number:'"/>
			</Trailer>
			<RecordCount>
				<xsl:value-of select ="'SS1840'"/>
			</RecordCount>

		</ThirdPartyFlatFileFooter>
	</xsl:template>
</xsl:stylesheet>
