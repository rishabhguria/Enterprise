<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>
				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>
				<!-- for system use only-->
				<FileHeader>
					<xsl:value-of select ="'true'"/>
				</FileHeader>
				<!-- for system use only-->
				<FileFooter>
					<xsl:value-of select ="'true'"/>
				</FileFooter>
				<!-- for system use only-->
				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>
				<!-- for system use only-->
				<TaxlotState>
					<xsl:value-of select ="'Allocated'"/>
				</TaxlotState>

				<AssetClass>
					<xsl:value-of select="'Asset Class'"/>
				</AssetClass>

				<InvestmentType>
					<xsl:value-of select ="'Investment Type'"/>
				</InvestmentType>

				<RecordAction>
					<xsl:value-of select ="'Record Action'"/>
				</RecordAction>

				<TradeID>
					<xsl:value-of select ="'Trade ID'"/>
				</TradeID>

				<Fund>
					<xsl:value-of select ="'Fund'"/>
				</Fund>

				<TradeDate>
					<xsl:value-of select ="'Trade Date'"/>
				</TradeDate>

				<SettleDate>
					<xsl:value-of select ="'Settle/Value Date'"/>
				</SettleDate>

				<EffectiveDate>
					<xsl:value-of select ="'Effective Date'"/>
				</EffectiveDate>

				<Broker>
					<xsl:value-of select ="'Broker'"/>
				</Broker>

				<CustodianAccount>
					<xsl:value-of select ="'Custodian Account'"/>
				</CustodianAccount>

				<InstrumentCodeType>
					<xsl:value-of select ="'Instrument Code Type'"/>
				</InstrumentCodeType>

				<InstrumentCode>
					<xsl:value-of select ="'Instrument Code'"/>
				</InstrumentCode>

				<ClientInstrumentID>
					<xsl:value-of select ="'Client Instrument Id'"/>
				</ClientInstrumentID>

				<InstrumentDescription>
					<xsl:value-of select ="'Instrument Description'"/>
				</InstrumentDescription>

				<StrikePrice>
					<xsl:value-of select ="'Strike Price'"/>
				</StrikePrice>

				<Put_Call_Indicator>
					<xsl:value-of select ="'Put/Call Indicator'"/>
				</Put_Call_Indicator>

				<Action>
					<xsl:value-of select ="'Action'"/>
				</Action>

				<Quantity>
					<xsl:value-of select ="'Quantity'"/>
				</Quantity>

				<TradePrice>
					<xsl:value-of select ="'Trade/All-In-Price'"/>
				</TradePrice>

				<TradeCurrency>
					<xsl:value-of select ="'Trade Currency'"/>
				</TradeCurrency>

				<SettlementCurrency>
					<xsl:value-of select ="'Settlement Currency'"/>
				</SettlementCurrency>

				<Rate>
					<xsl:value-of select ="'Yield/Rate'"/>
				</Rate>

				<Principal>
					<xsl:value-of select ="'Principal'"/>
				</Principal>

				<Premium>
					<xsl:value-of select ="'Premium'"/>
				</Premium>

				<Commission>
					<xsl:value-of select ="'Commission'"/>
				</Commission>

				<MiscCharges>
					<xsl:value-of select ="'Misc. Charges'"/>
				</MiscCharges>

				<LocalTax>
					<xsl:value-of select ="'Local Tax'"/>
				</LocalTax>

				<ExchangeTax>
					<xsl:value-of select ="'Exchange Tax'"/>
				</ExchangeTax>

				<AccruedInterest>
					<xsl:value-of select="'Accrued Interest'"/>
				</AccruedInterest>

				<SettlementAmount>
					<xsl:value-of select ="'Settlement Amount'"/>
				</SettlementAmount>

				<SettlementCCYFXrate>
					<xsl:value-of select ="'Settlement CCY FX rate'"/>
				</SettlementCCYFXrate>

				<Repo_AccruedInterest>
					<xsl:value-of select ="'Repo/Accrued Interest'"/>
				</Repo_AccruedInterest>

				<Strategy>
					<xsl:value-of select ="'Strategy'"/>
				</Strategy>

				<Sector>
					<xsl:value-of select ="'Sector'"/>
				</Sector>

				<CurrentFace>
					<xsl:value-of select ="'Current Face'"/>
				</CurrentFace>

				<CurrentFector>
					<xsl:value-of select ="'Current Factor'"/>
				</CurrentFector>

				<GroupId>
					<xsl:value-of select ="'Group Id'"/>
				</GroupId>

				<HaircutRate>
					<xsl:value-of select ="'Haircut Rate'"/>
				</HaircutRate>

				<MarginPercentage>
					<xsl:value-of select ="'Margin Percentage'"/>
				</MarginPercentage>

				<MarginAmount>
					<xsl:value-of select ="'Margin Amount'"/>
				</MarginAmount>

				<OptionExerciseType>
					<xsl:value-of select ="'Option Exercise Type'"/>
				</OptionExerciseType>

				<ExpiryDate>
					<xsl:value-of select ="'Expiry Date'"/>
				</ExpiryDate>

				<FirstResetDate>
					<xsl:value-of select ="'First Reset Date'"/>
				</FirstResetDate>

				<ResetOptions>
					<xsl:value-of select ="'Reset Options'"/>
				</ResetOptions>

				<DayCount>
					<xsl:value-of select ="'Day Count'"/>
				</DayCount>

				<PaymentFreq>
					<xsl:value-of select ="'Payment Freq'"/>
				</PaymentFreq>

				<TypeofFinancingLeg>
					<xsl:value-of select ="'Type of Financing Leg'"/>
				</TypeofFinancingLeg>

				<ReferenceIndex>
					<xsl:value-of select ="'Reference Index'"/>
				</ReferenceIndex>

				<IndexMultiplier>
					<xsl:value-of select ="'Index Multiplier'"/>
				</IndexMultiplier>

				<Spread>
					<xsl:value-of select ="'Spread'"/>
				</Spread>

				<Cap>
					<xsl:value-of select ="'Cap'"/>
				</Cap>

				<Floor>
					<xsl:value-of select ="'Floor'"/>
				</Floor>

				<DividendEntitlementPercentage>
					<xsl:value-of select ="'Dividend Entitlement %'"/>
				</DividendEntitlementPercentage>

				<PayCommissiononSwapOpen>
					<xsl:value-of select ="'Pay Commission on Swap Open'"/>
				</PayCommissiononSwapOpen>

				<CorporateActionsonPaydate>
					<xsl:value-of select ="'Corporate Actions on Pay date'"/>
				</CorporateActionsonPaydate>

				<UnderlyingSecurityIDType>
					<xsl:value-of select ="'Underlying Security ID Type'"/>
				</UnderlyingSecurityIDType>

				<UnderlyingSecurityID>
					<xsl:value-of select ="'Underlying Security ID'"/>
				</UnderlyingSecurityID>

				<!--for system internal use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test ="FundName = 'HSBC 10385' or FundName = 'Pershing SCLP' or FundName = 'JEFF SCLP'">
					<!--<xsl:if test="FundName!="-->

					<ThirdPartyFlatFileDetail>
						<!--for system use only-->
						<IsCaptionChangeRequired>
							<xsl:value-of select ="'true'"/>
						</IsCaptionChangeRequired>
						<!-- for system use only-->
						<FileHeader>
							<xsl:value-of select ="'true'"/>
						</FileHeader>
						<!-- for system use only-->
						<FileFooter>
							<xsl:value-of select ="'true'"/>
						</FileFooter>
						<!-- for system use only-->
						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>
						<!-- for system use only-->
						<TaxlotState>
							<xsl:value-of select ="TaxLotState"/>
						</TaxlotState>


						<xsl:choose>
							<xsl:when test ="Asset='Equity'">
								<AssetClass>
									<xsl:value-of select="'EQ'"/>
								</AssetClass>
							</xsl:when>
							<xsl:when test ="Asset='EquityOption'">
								<AssetClass>
									<xsl:value-of select="'OP'"/>
								</AssetClass>
							</xsl:when>
							<xsl:when test ="Asset='Future'">
								<AssetClass>
									<xsl:value-of select="'FU'"/>
								</AssetClass>
							</xsl:when>
							<xsl:when test ="Asset='FutureOption'">
								<AssetClass>
									<xsl:value-of select="'FO'"/>
								</AssetClass>
							</xsl:when>
							<xsl:when test ="Asset= ('PrivateEquity' or Asset='FixedIncome')">
								<AssetClass>
									<xsl:value-of select="'FI'"/>
								</AssetClass>
							</xsl:when>
							<xsl:otherwise>
								<AssetClass>
									<xsl:value-of select="''"/>
								</AssetClass>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test ="Asset='Equity'">
								<InvestmentType>
									<xsl:value-of select ="'Equity Unit'"/>
								</InvestmentType>
							</xsl:when>
							<xsl:when test ="Asset='EquityOption'">
								<InvestmentType>
									<xsl:value-of select ="'Equity Option'"/>
								</InvestmentType>
							</xsl:when>
							<xsl:when test ="Asset='Future'">
								<InvestmentType>
									<xsl:value-of select ="'Equity Future'"/>
								</InvestmentType>
							</xsl:when>
							<xsl:when test ="Asset='FutureOption'">
								<InvestmentType>
									<xsl:value-of select ="'Equity Future Option'"/>
								</InvestmentType>
							</xsl:when>
							<xsl:when test ="(Asset='PrivateEquity' or Asset='FixedIncome')">
								<InvestmentType>
									<xsl:value-of select ="'Fixed Income'"/>
								</InvestmentType>
							</xsl:when>
							<xsl:otherwise>
								<InvestmentType>
									<xsl:value-of select ="''"/>
								</InvestmentType>
							</xsl:otherwise>
						</xsl:choose>



						<xsl:choose>
							<xsl:when test="TaxLotState='Allocated'">
								<RecordAction>
									<xsl:value-of select ="'N'"/>
								</RecordAction>
							</xsl:when>
							<xsl:when test ="TaxlotState='Amemded'">
								<RecordAction>
									<xsl:value-of select ="'C'"/>
								</RecordAction>
							</xsl:when>
							<xsl:when test="TaxLotState='Deleted'">
								<RecordAction>
									<xsl:value-of select ="'D'"/>
								</RecordAction>
							</xsl:when>
							<xsl:otherwise>
								<RecordAction>
									<xsl:value-of select ="'N'"/>
								</RecordAction>
							</xsl:otherwise>
						</xsl:choose>


						<TradeID>
							<xsl:value-of select ="TradeRefID"/>
						</TradeID>


						<Fund>
							<xsl:choose>
								<xsl:when test ="FundName = 'HSBC 10385' or FundName = 'Pershing SCLP' or FundName = 'JEFF SCLP'">
									<xsl:value-of select ="'SONOMALP'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fund>


					

						<TradeDate>
							<xsl:value-of select ="concat(substring(TradeDate,7,4),substring(TradeDate,1,2),substring(TradeDate,4,2))"/>
						</TradeDate>

						<SettleDate>
							<xsl:value-of select ="concat(substring(SettlementDate,7,4),substring(SettlementDate,1,2),substring(SettlementDate,4,2))"/>
						</SettleDate>

						<EffectiveDate>
							<xsl:value-of select ="''"/>
						</EffectiveDate>

						<Broker>
							<xsl:value-of select ="CounterParty"/>
						</Broker>


						<xsl:choose>				

							<xsl:when test ="FundName = 'JEFF SCLP'">
								<CustodianAccount>
									<xsl:value-of select ="'43000101'"/>
								</CustodianAccount>
							</xsl:when>							
							<xsl:when test ="FundName = 'HSBC 10385'">
								<CustodianAccount>
									<xsl:value-of select ="'HSBC 10385'"/>
								</CustodianAccount>
							</xsl:when>					
														
							<xsl:when test ="FundName = 'Pershing SCLP'">
								<CustodianAccount>
									<xsl:value-of select ="'Pershing T63001498'"/>
								</CustodianAccount>
							</xsl:when>
							<xsl:otherwise>
								<CustodianAccount>
									<xsl:value-of select ="''"/>
								</CustodianAccount>
							</xsl:otherwise>
						</xsl:choose>

						
						<InstrumentCodeType>
							<xsl:value-of select ="'T'"/>
						</InstrumentCodeType>

						<xsl:variable name ="varEqtSymbol">
							<xsl:choose>
								<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/W') != ''">
									<xsl:value-of select ="concat(translate(Symbol,'.','/'),'S')"/>
								</xsl:when >
								<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/W') = ''">
									<xsl:value-of select ="translate(Symbol,'.','/')"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name ="varCheckSymbolUnderlying">
							<xsl:value-of select ="substring-before(Symbol,'-')"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="Symbol='BMO/PV-TC'">
								<InstrumentCode>
									<xsl:value-of select ="'BMO.PR.V'"/>
								</InstrumentCode>
							</xsl:when>

							<xsl:when test="Symbol='RY/PH-TC'">
								<InstrumentCode>
									<xsl:value-of select ="'RY.PFD'"/>
								</InstrumentCode>
							</xsl:when>



							<xsl:when test="Symbol='CM/PI-TC'">
								<InstrumentCode>
									<xsl:value-of select ="'CM.PR.I'"/>
								</InstrumentCode>
							</xsl:when>

							<xsl:when test="Symbol='DIL-NZX'">
								<InstrumentCode>
									<xsl:value-of select ="'DIL-NZX'"/>
								</InstrumentCode>



							</xsl:when>
							<xsl:when test="Asset = 'Equity' and $varCheckSymbolUnderlying!='' ">
								<InstrumentCode>
									<xsl:value-of select ="SEDOL"/>
								</InstrumentCode>
							</xsl:when>
							<xsl:when test="Asset ='Equity' and (Symbol='PE/PA')">
								<InstrumentCode>
									<xsl:value-of select ="'PE-A'"/>
								</InstrumentCode>
							</xsl:when>
							<xsl:when test ="Asset='EquityOption'">
								<InstrumentCode>
									<xsl:value-of select ="OSIOptionSymbol"/>
								</InstrumentCode>
							</xsl:when >
							<xsl:when test ="Asset='Equity'">
								<InstrumentCode>
									<!--<xsl:value-of select ="translate(Symbol,'.','/')"/>-->
									<xsl:value-of select ="$varEqtSymbol"/>
								</InstrumentCode>
							</xsl:when >
							<xsl:when test ="Asset='PrivateEquity'">
								<InstrumentCode>
									<xsl:value-of select ="translate(Symbol,'.','/')"/>
								</InstrumentCode>
							</xsl:when >
							<xsl:otherwise>
								<InstrumentCode>
									<xsl:value-of select ="Symbol"/>
								</InstrumentCode>
							</xsl:otherwise>
						</xsl:choose >

						<ClientInstrumentID>
							<xsl:value-of select ="''"/>
						</ClientInstrumentID>

						<InstrumentDescription>
							<xsl:value-of select ="translate(FullSecurityName,',','')"/>
						</InstrumentDescription>


						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<StrikePrice>
									<xsl:value-of select ="StrikePrice"/>
								</StrikePrice>
							</xsl:when>
							<xsl:otherwise>
								<StrikePrice>
									<xsl:value-of select ="''"/>
								</StrikePrice>
							</xsl:otherwise>
						</xsl:choose>

						<!--<StrikePrice>
            <xsl:value-of select ="StrikePrice"/>
          </StrikePrice>-->

						<xsl:choose>
							<xsl:when test ="PutOrCall = 'CALL'">
								<Put_Call_Indicator>
									<xsl:value-of select ="'C'"/>
								</Put_Call_Indicator>
							</xsl:when>
							<xsl:when test ="PutOrCall = 'PUT'">
								<Put_Call_Indicator>
									<xsl:value-of select ="'P'"/>
								</Put_Call_Indicator>
							</xsl:when>
							<xsl:otherwise>
								<Put_Call_Indicator>
									<xsl:value-of select ="''"/>
								</Put_Call_Indicator>
							</xsl:otherwise>
						</xsl:choose>

						<!--   Side     -->
						<xsl:choose>
							<xsl:when test="Side='Buy' or Side='Buy to Open'">
								<Action>
									<xsl:value-of select="'B'"/>
								</Action>
							</xsl:when>
							<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
								<Action>
									<xsl:value-of select="'BC'"/>
								</Action>
							</xsl:when>
							<xsl:when test="Side='Sell' or Side='Sell to Close'">
								<Action>
									<xsl:value-of select="'S'"/>
								</Action>
							</xsl:when>
							<xsl:when test="Side='Sell short' or Side='Sell to Open'">
								<Action>
									<xsl:value-of select="'SS'"/>
								</Action>
							</xsl:when>
							<xsl:otherwise>
								<Action>
									<xsl:value-of select="''"/>
								</Action>
							</xsl:otherwise>
						</xsl:choose>

						<Quantity>
							<xsl:value-of select="AllocatedQty"/>
						</Quantity>


						<xsl:choose>
							<xsl:when test ="AveragePrice != 0 ">
								<TradePrice>
									<xsl:value-of select = 'format-number(AveragePrice, "###.000000")'/>
								</TradePrice>
							</xsl:when>
							<xsl:otherwise>
								<TradePrice>
									<xsl:value-of select ="0"/>
								</TradePrice>
							</xsl:otherwise>
						</xsl:choose>

						<TradeCurrency>
							<xsl:value-of select ="CurrencySymbol"/>
						</TradeCurrency>

						<SettlementCurrency>
							<xsl:value-of select ="CurrencySymbol"/>
						</SettlementCurrency>

						<Rate>
							<xsl:value-of select ="''"/>
						</Rate>

						<Principal>
							<xsl:choose>
								<xsl:when test="Asset = 'FixedIncome'">
									<xsl:value-of select ="GrossAmount div 100"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="GrossAmount"/>
								</xsl:otherwise>
							</xsl:choose>
						</Principal>

						<Premium>
							<xsl:value-of select ="''"/>
						</Premium>

						<xsl:choose>
							<xsl:when test ="CommissionCharged != 0">
								<Commission>
									<xsl:value-of select = 'format-number(CommissionCharged, "###.0000")'/>
								</Commission>
							</xsl:when>
							<xsl:otherwise>
								<Commission>
									<xsl:value-of select ="0"/>
								</Commission>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name ="varMiscCharges">
							<xsl:choose>
								<xsl:when test ="TransactionLevy != 0">
									<xsl:value-of select ="format-number( TransactionLevy,  '###.0000')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varMiscCharge">
							<xsl:choose>
								<xsl:when test ="(FundName = 'Pershing SCLP' or FundName = 'JEFF SCLP') and CounterParty = 'JDMA'">
									<xsl:value-of select ="($varMiscCharges)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="($varMiscCharges + 15.00)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<MiscCharges>
							
							<xsl:value-of select ="($varMiscCharge)"/>
						</MiscCharges>


						<LocalTax>
							<xsl:value-of select ="''"/>
						</LocalTax>

						<xsl:choose>
							<xsl:when test ="StampDuty != 0">
								<ExchangeTax>
								
									<xsl:value-of select = 'format-number(StampDuty, "###.0000")'/>
								</ExchangeTax>
							</xsl:when>
							<xsl:otherwise>
								<ExchangeTax>
									<xsl:value-of select ="0"/>
								</ExchangeTax>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name="AccruedInterest">
							<xsl:choose>
								<xsl:when test="AllocatedQty &lt; TotalQty">
									<xsl:choose>
										<xsl:when test="number((AccruedInterest * AllocatedQty) div TotalQty)">
											<xsl:value-of select="format-number(((AccruedInterest * AllocatedQty) div TotalQty),'#.###')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="number(AccruedInterest)">
									<xsl:value-of select="AccruedInterest"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<AccruedInterest>
							<xsl:value-of select="$AccruedInterest"/>
						</AccruedInterest>

						<xsl:variable name ="SettAmount">
							<xsl:choose>

								<xsl:when test =" (Asset='PrivateEquity' or Asset='FixedIncome') and (contains(Side,'Sell')!=false)">
									<xsl:value-of select = 'format-number((AllocatedQty * AveragePrice )+ OtherBrokerFee +StampDuty + CommissionCharged + TransactionLevy, "###.0000000")'/>
								</xsl:when>
								<xsl:when test =" (Asset='PrivateEquity' or Asset='FixedIncome') and (contains(Side,'Buy')!=false)">
									<xsl:value-of select = 'format-number((AllocatedQty * AveragePrice )+ OtherBrokerFee +StampDuty + CommissionCharged + TransactionLevy, "###.0000000")'/>
								</xsl:when>

								<xsl:when test ="NetAmount != 0 and Asset != 'PrivateEquity' and Asset != 'FixedIncome' and (contains(Side,'Buy'))">
									<xsl:value-of select = 'format-number(NetAmount + $varMiscCharge, "###.0000000")'/>
								</xsl:when>
								<xsl:when test ="NetAmount != 0 and Asset != 'PrivateEquity' and Asset != 'FixedIncome' and (contains(Side,'Sell'))">
									<xsl:value-of select = 'format-number(NetAmount - $varMiscCharge, "###.0000000")'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<SettlementAmount>
							<xsl:choose>
								<xsl:when test="Asset = 'FixedIncome'">
									<xsl:choose>
										<xsl:when test ="FundName = 'HSBC 10385' or FundName = 'Pershing SCLP' or FundName = 'JEFF SCLP'">
											<xsl:value-of select ="$SettAmount div 100"/>
										</xsl:when>
										<xsl:when test ="(FundName = 'HSBC 10385' or FundName = 'Pershing SCLP' or FundName = 'JEFF SCLP') and (contains(Side,'Sell')!=false)">
											<xsl:value-of select ="($SettAmount div 100)+ $AccruedInterest - 15"/>
										</xsl:when>
										<xsl:when test ="(FundName = 'HSBC 10385' or FundName = 'Pershing SCLP' or FundName = 'JEFF SCLP') and (contains(Side,'Buy')!=false)">
											<xsl:value-of select ="($SettAmount div 100)+$AccruedInterest + 15"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="$SettAmount div 100"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>

						
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="(FundName = 'HSBC 10385' or FundName = 'Pershing SCLP' or FundName = 'JEFF SCLP')">
											<xsl:value-of select ="$SettAmount"/>
										</xsl:when>
										<!--<xsl:when test ="(FundName = 'HSBC 10385' or FundName = 'Pershing SCLP' or FundName = 'JEFF SCLP') and (contains(Side,'Sell')!=false)">
											<xsl:value-of select ="$SettAmount - 15"/>
										</xsl:when>
										<xsl:when test ="(FundName = 'HSBC 10385' or FundName = 'Pershing SCLP' or FundName = 'JEFF SCLP') and (contains(Side,'Buy')!=false)">
											<xsl:value-of select ="$SettAmount + 15"/>
										</xsl:when>-->
										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SettlementAmount>

						<SettlementCCYFXrate>
							<xsl:value-of select ="''"/>
						</SettlementCCYFXrate>

						<xsl:choose>
							<xsl:when test ="OtherBrokerFee != 0">
								<Repo_AccruedInterest>
									<xsl:value-of select = 'format-number(OtherBrokerFee, "###.0000000")'/>
								</Repo_AccruedInterest>
							</xsl:when>
							<xsl:otherwise>
								<Repo_AccruedInterest>
									<xsl:value-of select ="0"/>
								</Repo_AccruedInterest>
							</xsl:otherwise>
						</xsl:choose>

						<Strategy>
							<xsl:value-of select ="''"/>
						</Strategy>

						<Sector>
							<xsl:value-of select ="''"/>
						</Sector>

						<CurrentFace>
							<xsl:value-of select ="''"/>
						</CurrentFace>

						<CurrentFector>
							<xsl:value-of select ="''"/>
						</CurrentFector>

						<GroupId>
							<xsl:value-of select ="''"/>
						</GroupId>

						<HaircutRate>
							<xsl:value-of select ="''"/>
						</HaircutRate>

						<MarginPercentage>
							<xsl:value-of select ="''"/>
						</MarginPercentage>

						<MarginAmount>
							<xsl:value-of select ="''"/>
						</MarginAmount>

						<OptionExerciseType>
							<xsl:value-of select ="''"/>
						</OptionExerciseType>

						<xsl:choose>
							<xsl:when test ="Asset='EquityOption'">
								<ExpiryDate>
									<xsl:value-of select ="concat(substring(ExpirationDate,7,4),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2))"/>
								</ExpiryDate>
							</xsl:when>
							<xsl:otherwise>
								<ExpiryDate>
									<xsl:value-of select ="''"/>
								</ExpiryDate>
							</xsl:otherwise>
						</xsl:choose>


						<FirstResetDate>
							<xsl:value-of select ="''"/>
						</FirstResetDate>

						<ResetOptions>
							<xsl:value-of select ="''"/>
						</ResetOptions>

						<DayCount>
							<xsl:value-of select ="''"/>
						</DayCount>

						<PaymentFreq>
							<xsl:value-of select ="''"/>
						</PaymentFreq>

						<TypeofFinancingLeg>
							<xsl:value-of select ="''"/>
						</TypeofFinancingLeg>

						<ReferenceIndex>
							<xsl:value-of select ="''"/>
						</ReferenceIndex>

						<IndexMultiplier>
							<xsl:value-of select ="''"/>
						</IndexMultiplier>

						<Spread>
							<xsl:value-of select ="''"/>
						</Spread>

						<Cap>
							<xsl:value-of select ="''"/>
						</Cap>

						<Floor>
							<xsl:value-of select ="''"/>
						</Floor>

						<DividendEntitlementPercentage>
							<xsl:value-of select ="''"/>
						</DividendEntitlementPercentage>

						<PayCommissiononSwapOpen>
							<xsl:value-of select ="''"/>
						</PayCommissiononSwapOpen>

						<CorporateActionsonPaydate>
							<xsl:value-of select ="''"/>
						</CorporateActionsonPaydate>

						<UnderlyingSecurityIDType>
							<xsl:value-of select ="'T'"/>
						</UnderlyingSecurityIDType>

						<UnderlyingSecurityID>
							<xsl:value-of select ="UnderlyingSymbol"/>
						</UnderlyingSecurityID>

						<!-- system inetrnal use-->
						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>
					</ThirdPartyFlatFileDetail>
				</xsl:if>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

</xsl:stylesheet>
