<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

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


					<AccountName>
						<xsl:value-of select="AccountName"/>
					</AccountName>


					<Symbol>
						<xsl:value-of select="Symbol"/>
					</Symbol>

					<Side>
						<xsl:value-of select="Side"/>
					</Side>

					<AveragePrice>
						<xsl:value-of select="AveragePrice"/>
					</AveragePrice>
					
					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>
          
          <State>
          <xsl:value-of select="TaxLotState"/>
          </State>

					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>

					<Commission>
						<xsl:value-of select="CommissionCharged"/>
					</Commission>

					<NetAmount>
						<xsl:value-of select="NetAmount"/>
					</NetAmount>

					<AssetName>
						<xsl:value-of select="Asset"/>
					</AssetName>
					
					<FXRate>
						<xsl:value-of select="FXRate_Taxlot"/>
					</FXRate>

					<Broker>
						<xsl:value-of select="CounterParty"/>
					</Broker>

					<TradeAttribute1>
						<xsl:value-of select="TradeAttribute1"/>
					</TradeAttribute1>


					<TradeAttribute2>
						<xsl:value-of select="TradeAttribute2"/>
					</TradeAttribute2>

					<TradeAttribute3>
						<xsl:value-of select="TradeAttribute3"/>
					</TradeAttribute3>

					<TradeAttribute4>
						<xsl:value-of select="TradeAttribute4"/>
					</TradeAttribute4>

					<TradeAttribute5>
						<xsl:value-of select="TradeAttribute5"/>
					</TradeAttribute5>

					<TradeAttribute6>
						<xsl:value-of select="TradeAttribute6"/>
					</TradeAttribute6>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>


				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>