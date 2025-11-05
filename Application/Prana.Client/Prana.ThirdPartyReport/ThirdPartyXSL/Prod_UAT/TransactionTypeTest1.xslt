<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<!--<xsl:template match="/ThirdPartyFlatFileDetailCollection">-->
	<xsl:template match="/NewDataSet">

		<ThirdPartyFlatFileDetailCollection>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>
						<!--for system internal use-->
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						
						<TickerSymbol>
							<xsl:value-of select="Symbol"/>
						</TickerSymbol>

						<IsGrouped>
							<xsl:value-of select ="IsGrouped"/>
						</IsGrouped>
						
						<fromDeleted>
							<xsl:value-of select = "FromDeleted"/>
						</fromDeleted>

						<Issuer>
							<xsl:value-of select="Issuer"/>
						</Issuer>

						<CountryOfRisk>
							<xsl:value-of select="CountryOfRisk"/>
						</CountryOfRisk>
						
						<CustomUDA1>
							<xsl:value-of select="CustomUDA1"/>
						</CustomUDA1>
						
						<CustomUDA2>
							<xsl:value-of select="CustomUDA2"/>
						</CustomUDA2>
						
						<CustomUDA3>
							<xsl:value-of select="CustomUDA3"/>
						</CustomUDA3>
						
						<CustomUDA4>
							<xsl:value-of select="CustomUDA4"/>
						</CustomUDA4>
						
						<CustomUDA5>
							<xsl:value-of select="CustomUDA5"/>
						</CustomUDA5>
						
						<CustomUDA6>
							<xsl:value-of select="CustomUDA6"/>
						</CustomUDA6>
						
						<CustomUDA7>
							<xsl:value-of select="CustomUDA7"/>
						</CustomUDA7>
						
						<Region>
							<xsl:value-of select="Region"/>
						</Region>
						
						<MarketCap>
							<xsl:value-of select="MarketCap"/>
						</MarketCap>
						
						<LiquidTag>
							<xsl:value-of select="LiquidTag"/>
						</LiquidTag>
						
						<RiskCurrency>
							<xsl:value-of select="RiskCurrency"/>
						</RiskCurrency>
						
						<UCITSEligibleTag>
							<xsl:value-of select="UCITSEligibleTag"/>
						</UCITSEligibleTag>
						
						<ThirdPartyCheck>
							<xsl:value-of select="ThirdPartyCheck"/>
						</ThirdPartyCheck>
					
						<TaxLotFIXAckState>
							<xsl:value-of select="TaxLotFIXAckStateID"/>
						</TaxLotFIXAckState>

						<SettlCurrAmt>
							<xsl:value-of select="SettlCurrAmt"/>
						</SettlCurrAmt>

						<SettlCurrFxRate>
							<xsl:value-of select="SettlCurrFxRate"/>
						</SettlCurrFxRate>

						<SettlCurrFxRateCalc>
							<xsl:value-of select="SettlCurrFxRateCalc"/>
						</SettlCurrFxRateCalc>

						<SettlCurrency>
							<xsl:value-of select="SettlCurrency"/>
						</SettlCurrency>
						
						<SettlFxRate>
							<xsl:value-of select="SettlCurrFxRate"/>
						</SettlFxRate>
						
						<FxRate>
							<xsl:value-of select="FXRate_Taxlot"/>
						</FxRate>
						
						<AccountName>
							<xsl:value-of select="AccountName"/>
						</AccountName>
						
						<Broker>
							<xsl:value-of select="CounterParty"/>
						</Broker>
						
						<Commission>
							<xsl:value-of select="CommissionCharged"/>
						</Commission>
						
						<SoftCommission>
							<xsl:value-of select="SoftCommissionCharged"/>
						</SoftCommission>

						<Side>
							<xsl:value-of select="Side"/>
						</Side>

						<Quantity>
							<xsl:value-of select="ExecutedQty"/>
						</Quantity>

						<Price>
							<xsl:value-of select="AveragePrice"/>
						</Price>						

						<TradeDate>
							<xsl:value-of select="TradeDate"/>
						</TradeDate>

						<!--<FXPrice>
							<xsl:value-of select="format-number(ForexRate,'#.#####')"/>
						</FXPrice>-->

					<ChangeType>
						<xsl:value-of select="ChangeType"/>
					</ChangeType>
					
						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
			
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>