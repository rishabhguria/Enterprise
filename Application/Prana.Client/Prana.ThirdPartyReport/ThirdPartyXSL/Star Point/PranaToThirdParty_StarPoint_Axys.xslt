<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<PortfolioCode>
						<xsl:value-of select="FundMappedName"/>
					</PortfolioCode>
					<!--   Side     -->
					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open'">
							<TranCode>
								<xsl:value-of select="'by'"/>
							</TranCode>
						</xsl:when>
						<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
							<TranCode>
								<xsl:value-of select="'cs'"/>
							</TranCode>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<TranCode>
								<xsl:value-of select="'sl'"/>
							</TranCode>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open'">
							<TranCode>
								<xsl:value-of select="'ss'"/>
							</TranCode>
						</xsl:when>
						<xsl:otherwise>
							<TranCode>
								<xsl:value-of select="''"/>
							</TranCode>
						</xsl:otherwise>
					</xsl:choose>

					<!--   Side End    -->
					<Comment>
						<xsl:value-of select ="''"/>
					</Comment>

					<xsl:variable name ="varAssetType">
						<xsl:choose>
							<xsl:when test ="AssetID = '1'">
								<xsl:value-of select ="'cs'"/>"
							</xsl:when>
							<xsl:when test ="AssetID = '2'">
								<xsl:value-of select ="'op'"/>"
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="'cs'"/>"
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name ="varCurrencySymbol">
						<xsl:choose>
							<xsl:when test ="CurrencySymbol = 'USD'">
								<xsl:value-of select ="'us'"/>"
							</xsl:when>
							<xsl:when test ="CurrencySymbol='CAD'">
								<xsl:value-of select ="'ca'"/>"
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="'us'"/>"
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SecType>
						<xsl:value-of select ="'csus'"/>
					</SecType>

					<SecuritySymbol>
						<xsl:value-of select="translate(Symbol, $vUppercaseChars_CONST , $vLowercaseChars_CONST)"/>
					</SecuritySymbol>

					<!-- Month Code-->
					<xsl:variable name ="varMonth">
						<xsl:value-of select ="substring(TradeDate,1,2)"/>
					</xsl:variable>

					<!-- Day Code-->
					<xsl:variable name ="varDay">
						<xsl:value-of select ="substring(TradeDate,4,2)"/>
					</xsl:variable>

					<!-- Year Code-->
					<xsl:variable name ="varYear">
						<xsl:value-of select ="substring(TradeDate,7,4)"/>
					</xsl:variable>
					<TradeDate>
						<xsl:value-of select ="concat($varMonth,$varDay,$varYear)"/>
					</TradeDate>

					<!--column 7 -->

					<!-- Month Code-->
					<xsl:variable name ="varMonthSettle">
						<xsl:value-of select ="substring(SettlementDate,1,2)"/>
					</xsl:variable>

					<!-- Day Code-->
					<xsl:variable name ="varDaySettle">
						<xsl:value-of select ="substring(SettlementDate,4,2)"/>
					</xsl:variable>

					<!-- Year Code-->
					<xsl:variable name ="varYearSettle">
						<xsl:value-of select ="substring(SettlementDate,7,4)"/>
					</xsl:variable>

					<SettleDate>
						<xsl:value-of select ="concat($varMonthSettle,$varDaySettle,$varYearSettle)"/>
					</SettleDate>

					<OriginalCostDate>
						<xsl:value-of select ="''"/>
					</OriginalCostDate>

					<Quantity>
						<xsl:value-of select="AllocatedQty"/>
					</Quantity>

					<CloseMath>
						<xsl:value-of select ="''"/>
					</CloseMath>

					<VersusDate>
						<xsl:value-of select="''"/>
					</VersusDate>

					<SourceType>
						<xsl:value-of select ="'csus'"/>
					</SourceType>

					<!-- Column 13-->
					<SourceSymbol>
						<xsl:value-of select ="'cash'"/>
					</SourceSymbol>

					<TradeDateFXRate>
						<xsl:value-of select ="''"/>
					</TradeDateFXRate>

					<SettleDateFXRate>
						<xsl:value-of select="''"/>
					</SettleDateFXRate>

					<OriginalFXRate>
						<xsl:value-of select ="''"/>
					</OriginalFXRate>

					<MarkToMarket>
						<xsl:value-of select ="'n'"/>
					</MarkToMarket>

					<TradeAmount>
						<xsl:value-of select="concat('@',AveragePrice)"/>
					</TradeAmount>

					<OriginalCost>
						<xsl:value-of select ="''"/>
					</OriginalCost>

					<!--column 20 -->

					<Comment1>
						<xsl:value-of select ="''"/>
					</Comment1>

					<WithholdingTax>
						<xsl:value-of select ="''"/>
					</WithholdingTax>

					<Exchnage>
						<xsl:value-of select ="'1'"/>
					</Exchnage>

					<xsl:variable name ="varSellSide">
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'n'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="'y'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SellSide>
						<xsl:value-of select ="$varSellSide"/>
					</SellSide>

					<commission>
						<xsl:value-of select="CommissionCharged + TaxOnCommissions + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee"/>
					</commission>

					<Broker>
						<xsl:value-of select="CounterParty"/>
					</Broker>

					<ImpliedComm>
						<xsl:value-of select ="'n'"/>
					</ImpliedComm>

					<OtherFees>
						<xsl:value-of select ="''"/>
					</OtherFees>

					<CommPurpose>
						<xsl:value-of select ="''"/>
					</CommPurpose>

					<!-- Column 29-->

					<Pledge>
						<xsl:value-of select ="'n'"/>
					</Pledge>

					<LotLocation>
						<xsl:value-of select ="253"/>
					</LotLocation>

					<DestPledge>
						<xsl:value-of select ="''"/>
					</DestPledge>

					<DestLotLocation>
						<xsl:value-of select ="''"/>
					</DestLotLocation>

					<OriginalFace>
						<xsl:value-of select ="''"/>
					</OriginalFace>

					<YieldOnCost>
						<xsl:value-of select ="''"/>
					</YieldOnCost>

					<!-- column 35-->

					<DurationOnCost>
						<xsl:value-of select ="''"/>
					</DurationOnCost>

					<UserDef1>
						<xsl:value-of select ="''"/>
					</UserDef1>

					<UserDef2>
						<xsl:value-of select="''"/>
					</UserDef2>

					<UserDef3>
						<xsl:value-of select="''"/>
					</UserDef3>

					<TranID>
						<xsl:value-of select ="''"/>
					</TranID>

					<IPCounter>
						<xsl:value-of select ="''"/>
					</IPCounter>

					<Repl>
						<xsl:value-of select="'n'"/>
					</Repl>

					<!-- column 42-->

					<Source>
						<xsl:value-of select ="''"/>
					</Source>

					<Comment2>
						<xsl:value-of select ="''"/>
					</Comment2>

					<OmniAcct>
						<xsl:value-of select ="''"/>
					</OmniAcct>

					<Recon>
						<xsl:value-of select="''"/>
					</Recon>

					<Post>
						<xsl:value-of select ="'y'"/>
					</Post>

					<LabelName>
						<xsl:value-of select="''"/>
					</LabelName>

					<LabelDefinition>
						<xsl:value-of select ="''"/>
					</LabelDefinition>

					<LabelDefinition_Date>
						<xsl:value-of select="''"/>
					</LabelDefinition_Date>

					<!-- column 50-->

					<LabelDefinition_String>
						<xsl:value-of select="''"/>
					</LabelDefinition_String>

					<Comment3>
						<xsl:value-of select ="''"/>
					</Comment3>

					<xsl:variable name ="varSideCode">
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell short' or Side='Sell to Open'">
								<xsl:value-of select="'A'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="'C'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<!-- Year Code-->
					<xsl:variable name ="varYr">
						<xsl:value-of select ="substring(TradeDate,9,2)"/>
					</xsl:variable>


					<!-- Counter Code-->
					<xsl:variable name ="varCount" select ="position()" />

					<xsl:choose>
						<xsl:when test ="string-length($varCount) = 1">
							<Code>
								<xsl:value-of select ="concat($varSideCode,$varYr,$varMonth,$varDay,'00',$varCount)"/>
							</Code>
						</xsl:when>
						<xsl:when test ="string-length($varCount) = 2">
							<Code>
								<xsl:value-of select ="concat($varSideCode,$varYr,$varMonth,$varDay,'0',$varCount)"/>
							</Code>
						</xsl:when>
						<xsl:otherwise>
							<Code>
								<xsl:value-of select ="concat($varSideCode,$varYr,$varMonth,$varDay,$varCount)"/>
							</Code>
						</xsl:otherwise>
					</xsl:choose>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
