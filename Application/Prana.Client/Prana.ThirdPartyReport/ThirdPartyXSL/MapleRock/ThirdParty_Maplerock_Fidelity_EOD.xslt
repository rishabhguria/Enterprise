<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<!--<ThirdPartyFlatFileDetail>

				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<FileHeader>
					<xsl:value-of select ="'true'"/>
				</FileHeader>

				<FileFooter>
					<xsl:value-of select ="'false'"/>
				</FileFooter>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<TaxLotState>
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

				<RecordAction>
					<xsl:value-of select="'RecordAction'"/>
				</RecordAction>

				<RecordType>
					<xsl:value-of select="'RecordType'"/>
				</RecordType>

				<Portfolio>
					<xsl:value-of select="'Asset Class'"/>
				</Portfolio>

				<Investment>
					<xsl:value-of select="'Investment'"/>
				</Investment>

				<LocationAccount>
					<xsl:value-of select="'LocationAccount'"/>
				</LocationAccount>

				<Strategy>
					<xsl:value-of select="'Strategy'"/>
				</Strategy>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<EventDate>
					<xsl:value-of select="'EventDate'"/>
				</EventDate>

				<SettleDate>
					<xsl:value-of select="'SettleDate'"/>
				</SettleDate>

				<ActualSettleDate>
					<xsl:value-of select="'ActualSettleDate'"/>
				</ActualSettleDate>

				<SecFeeAmount>
					<xsl:value-of select="'SecFeeAmount'"/>
				</SecFeeAmount>

				<NetCounterAmount>
					<xsl:value-of select="'NetCounterAmount'"/>
				</NetCounterAmount>

				<NetInvestmentAmount>
					<xsl:value-of select="'NetInvestmentAmount'"/>
				</NetInvestmentAmount>

				<TotCommission>
					<xsl:value-of select="'TotCommission'"/>
				</TotCommission>

				<UserTranId1>
					<xsl:value-of select="'UserTranId1'"/>
				</UserTranId1>

				<PriceDenomination>
					<xsl:value-of select="'PriceDenomination'"/>
				</PriceDenomination>

				<CounterInvestment>
					<xsl:value-of select="'CounterInvestment'"/>
				</CounterInvestment>

				<CounterFXDenomination>
					<xsl:value-of select="'CounterFXDenomination'"/>
				</CounterFXDenomination>

				<TradeFX>
					<xsl:value-of select="'TradeFX'"/>
				</TradeFX>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<UnderlyingSymbol>
					<xsl:value-of select ="'UnderlyingSymbol'"/>
				</UnderlyingSymbol>

				<ExpirationDate>
					<xsl:value-of select ="'ExpirationDate'"/>
				</ExpirationDate>

				<StrikePrice>
					<xsl:value-of select ="'StrikePrice'"/>
				</StrikePrice>

				<Put_Call>
					<xsl:value-of select ="'PutOrCall'"/>
				</Put_Call>

				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>-->
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<!--<xsl:if test="FundName='MousseIndia Fidelity' or 'MousseAround Fidelity'">-->
				<xsl:if test="AccountName='Maple Rock MF: Fidelity' or AccountName='Maple Rock MF: Fidelity Collateral' or AccountName='Maple Rock US: Fidelity' or AccountName='Maple Rock OS: Fidelity' ">
					<ThirdPartyFlatFileDetail>

						<!--for system internal use-->
						<!--<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>-->

						<FileHeader>
							<xsl:value-of select ="'false'"/>
						</FileHeader>

						<FileFooter>
							<xsl:value-of select ="'false'"/>
						</FileFooter>

						<!--for system internal use-->
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<!--for system internal use-->
						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>

						<xsl:variable name="Prana_FundName">
							<xsl:value-of select="AccountName"/>
						</xsl:variable>

						<xsl:variable name="PRANA_MasterFund_Name">
							<xsl:value-of select="document('../ReconMappingXml/MasterFundMapping.xml')/MasterFundMapping/PB[@Name= 'GS']/MasterFundData[@FundName=$Prana_FundName]/@MasterFundName"/>
						</xsl:variable>


						<RecordAction>
							<xsl:choose>
								<xsl:when test ="TaxLotState = 'Allocated'">
									<xsl:value-of select="'New'"/>
								</xsl:when>
								<xsl:when test ="TaxLotState = 'Amemded'">
									<xsl:value-of select="'c'"/>
								</xsl:when>
								<xsl:when test ="TaxLotState = 'Deleted'">
									<xsl:value-of select="'x'"/>
								</xsl:when>
							</xsl:choose>
						</RecordAction>

						<RecordType>
							<xsl:choose>
								<xsl:when test ="Asset = 'EquityOption'">
									<xsl:choose>
										<xsl:when test="Side='Buy to Open'">
											<xsl:value-of select="'BTO'"/>
										</xsl:when>
										<xsl:when test="Side='Sell to Close'">
											<xsl:value-of select="'STC'"/>
										</xsl:when>
										<xsl:when test="Side='Sell to Open'">
											<xsl:value-of select="'STO'"/>
										</xsl:when>
										<xsl:when test="Side='Buy to Close'">
											<xsl:value-of select="'BTC'"/>
										</xsl:when>
									</xsl:choose>									
								</xsl:when>
								<xsl:when test ="Asset = 'Equity' or Asset = 'FixedIncome' or Asset = 'EquitySwap'">
									<xsl:choose>
										<xsl:when test="Side='Buy'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="Side='Sell'">
											<xsl:value-of select="'S'"/>
										</xsl:when>
										<xsl:when test="Side='Sell short'">
											<xsl:value-of select="'SS'"/>
										</xsl:when>
										<xsl:when test="Side='Buy to Close'">
											<xsl:value-of select="'BC'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:when test ="Asset = 'FX'">
									<xsl:choose>
										<xsl:when test="Side='Buy'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="Side='Sell'">
											<xsl:value-of select="'S'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
							
						</RecordType>

						<Portfolio>
							<xsl:value-of select="$PRANA_MasterFund_Name"/>
						</Portfolio>

						<Investment>
							<xsl:choose>
								<xsl:when test ="Asset = 'EquityOption'">
									<xsl:value-of select ="OSIOptionSymbol"/>
								</xsl:when>
								<xsl:when test ="contains(Asset,'FX') or IsSwapped='true'">
									<xsl:value-of select ="Symbol"/>
								</xsl:when>
								<xsl:when test ="contains(Symbol, '-') != false">
									<xsl:value-of select ="SEDOL"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="Symbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Investment>

						<LocationAccount>
							<!--<xsl:value-of select="concat($PRANA_MasterFund_Name, '_', AccountName)"/>-->
							<xsl:choose>
							<xsl:when test="AccountName = 'Maple Rock MF: Fidelity'">
								<xsl:value-of select="'752014280'"/>
							</xsl:when>
								</xsl:choose>
							
						</LocationAccount>

						<Strategy>
							<xsl:value-of select="Strategy"/>
						</Strategy>

						<Quantity>
							<xsl:value-of select="AllocatedQty"/>
						</Quantity>

						<xsl:variable name="varSettFxAmt">
							<xsl:choose>
								<xsl:when test="SettlCurrency != CurrencySymbol">
									<xsl:choose>
										<xsl:when test="FXConversionMethodOperator_Trade ='M'">
											<xsl:value-of select="format-number((AveragePrice * FXRate_Taxlot),'0.######')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number((AveragePrice div FXRate_Taxlot),'0.######')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number(AveragePrice,'0.######')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Price">
							<xsl:choose>
								<xsl:when test="SettlCurrency = CurrencySymbol">
									<xsl:value-of select="format-number(AveragePrice,'0.######')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number($varSettFxAmt,'0.######')"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<Price>
							<xsl:value-of select="$Price"/>

						</Price>


						

						<Broker>
							<xsl:choose>
								<xsl:when test="CounterParty= 'JEFF' or CounterParty= 'ZJEFF'">
									<xsl:value-of select="'JEFF'"/>
								</xsl:when>
								<xsl:when test="CounterParty= 'CITI' or CounterParty= 'ZCITI'">
									<xsl:value-of select="'CITI'"/>
								</xsl:when>
								<xsl:when test="CounterParty= 'JPMS' or CounterParty= 'ZJPMS'">
									<xsl:value-of select="'JPMS'"/>
								</xsl:when>
								<xsl:when test="CounterParty= 'GS' or CounterParty= 'ZGS'">
									<xsl:value-of select="'GS'"/>
								</xsl:when>
								<xsl:when test="CounterParty= 'BERN' or CounterParty= 'ZBERN'">
									<xsl:value-of select="'BERN'"/>
								</xsl:when>
								<xsl:when test="CounterParty= 'CS' or CounterParty= 'ZCS'">
									<xsl:value-of select="'CS'"/>
								</xsl:when>
								<xsl:when test="CounterParty= 'ITGI' or CounterParty= 'ZITGI'">
									<xsl:value-of select="'ITGI'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="CounterParty"/>
								</xsl:otherwise>
							</xsl:choose>
						</Broker>

						<EventDate>
							<xsl:value-of select="TradeDate"/>
						</EventDate>

						<SettleDate>
							<xsl:value-of select="SettlementDate"/>
						</SettleDate>

						<ActualSettleDate>
							<xsl:value-of select="SettlementDate"/>
						</ActualSettleDate>
						
						<xsl:variable name="varSettFxSecFee">
							<xsl:choose>
								<xsl:when test="SettlCurrency != CurrencySymbol">
									<xsl:choose>
										<xsl:when test="FXConversionMethodOperator_Trade ='M'">
											<xsl:value-of select="StampDuty * FXRate_Taxlot"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="StampDuty div FXRate_Taxlot"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="StampDuty"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="StampDuty">
							<xsl:choose>
								<xsl:when test="SettlCurrency = CurrencySymbol">
									<xsl:value-of select="StampDuty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varSettFxSecFee"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>
					

						<SecFeeAmount>
							<xsl:choose>
								<xsl:when test ="Side = 'Sell' or Side = 'Sell short'">
									<xsl:value-of select="$StampDuty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFeeAmount>

						<NetCounterAmount>
							<xsl:value-of select="''"/>
						</NetCounterAmount>

						<xsl:variable name="varSettFxNetAmount">
							<xsl:choose>
								<xsl:when test="SettlCurrency != CurrencySymbol">
									<xsl:choose>
										<xsl:when test="FXConversionMethodOperator_Trade ='M'">
											<xsl:value-of select="NetAmount * FXRate_Taxlot"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="NetAmount div FXRate_Taxlot"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="NetAmount"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="NetAmount">
							<xsl:choose>
								<xsl:when test="SettlCurrency = CurrencySymbol">
									<xsl:value-of select="NetAmount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varSettFxNetAmount"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>
						
						<NetInvestmentAmount>
							<xsl:value-of select="$NetAmount"/>
						</NetInvestmentAmount>

						<xsl:variable name="varSettFxCommissionCharged">
							<xsl:choose>
								<xsl:when test="SettlCurrency != CurrencySymbol">
									<xsl:choose>
										<xsl:when test="FXConversionMethodOperator_Trade ='M'">
											<xsl:value-of select="format-number(CommissionCharged * FXRate_Taxlot,'#.##')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(CommissionCharged div FXRate_Taxlot,'#.##')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number(CommissionCharged,'#.##')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="CommissionCharged">
							<xsl:choose>
								<xsl:when test="SettlCurrency = CurrencySymbol">
									<xsl:value-of select="format-number(CommissionCharged,'0.##')"/>
									<!--<xsl:value-of select="CommissionCharged"/>-->
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number($varSettFxCommissionCharged,'0.##')"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<TotCommission>
							<xsl:value-of select="format-number($CommissionCharged,'0.##')"/>
							<!--<xsl:value-of select="$CommissionCharged"/>-->
						</TotCommission>

						<UserTranId1>
							<xsl:value-of select="EntityID"/>
						</UserTranId1>

						<PriceDenomination>
							<xsl:value-of select="SettlCurrency"/>
						</PriceDenomination>

						<CounterInvestment>
							<xsl:value-of select="SettlCurrency"/>
						</CounterInvestment>

						<CounterFXDenomination>
							<xsl:value-of select="SettlCurrency"/>
						</CounterFXDenomination>

						<TradeFX>
							<xsl:value-of select="ForexRate"/>
						</TradeFX>

						<CUSIP>
							<xsl:value-of select="CUSIP"/>
						</CUSIP>

						<UnderlyingSymbol>
							<xsl:choose>
								<xsl:when test ="Asset = 'EquityOption'">
									<xsl:value-of select ="UnderlyingSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</UnderlyingSymbol>

						<ExpirationDate>
							<xsl:choose>
								<xsl:when test ="Asset = 'EquityOption'">
									<xsl:value-of select ="ExpirationDate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExpirationDate>

						<StrikePrice>
							<xsl:choose>
								<xsl:when test ="number(StrikePrice)">
									<xsl:value-of select ="StrikePrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</StrikePrice>

						<Put_Call>
							<xsl:value-of select ="substring(PutOrCall,1,1)"/>
						</Put_Call>
						
						<DTCStatus>							
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="'NOT DTC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'DTC'"/>
									</xsl:otherwise>
								</xsl:choose>							
						</DTCStatus>
						

						<!-- system use only-->
						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
				</xsl:if>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
