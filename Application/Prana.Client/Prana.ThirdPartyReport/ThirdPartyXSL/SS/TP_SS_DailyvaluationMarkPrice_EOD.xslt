<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>


					<Symbol>
						<xsl:value-of select="Symbol"/>
					</Symbol>

					<Description>
						<xsl:value-of select="CompanyName"/>
					</Description>


					<Currency>
						<xsl:value-of select="TradeCurrency"/>
					</Currency>

					<ClosingPrice>
						<xsl:value-of select="MarkPrice"/>
					</ClosingPrice>

					

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>