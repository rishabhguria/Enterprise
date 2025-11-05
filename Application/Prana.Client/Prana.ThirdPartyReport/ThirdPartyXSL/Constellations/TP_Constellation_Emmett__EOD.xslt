<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<TaxLotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxLotState>
				<TradeID>
					<xsl:value-of select="'Trade ID'"/>
				</TradeID>
				<FundCode>
					<xsl:value-of select="'Fund Code'"/>
				</FundCode>
				<TransactionType>
					<xsl:value-of select="'Transaction Type'"/>
				</TransactionType>
				<ProductType>
					<xsl:value-of select="'Product Type'"/>
				</ProductType>
				<BloombergYellowKey>
					<xsl:value-of select="'Bloomberg Yellow Key'"/>
				</BloombergYellowKey>
				<Ticker>
					<xsl:value-of select="'Ticker'"/>
				</Ticker>
				<ISIN>
					<xsl:value-of select="'ISIN'"/>
				</ISIN>
				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>
				<SEDOL>
					<xsl:value-of select="'SEDOL'"/>
				</SEDOL>
				<OCCCode>
					<xsl:value-of select="'OCC Code'"/>
				</OCCCode>
				<ISOCurrencyCode>
					<xsl:value-of select="'ISO Currency Code'"/>
				</ISOCurrencyCode>
				<PrivateAssetID>
					<xsl:value-of select="'Private Asset ID'"/>
				</PrivateAssetID>
				<TradeType>
					<xsl:value-of select="'Trade Type'"/>
				</TradeType>
				<OriginalFace>
					<xsl:value-of select="'OriginalFace'"/>
				</OriginalFace>
				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>
				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>
				<Factor>
					<xsl:value-of select="'Factor'"/>
				</Factor>
				<LocalInterest>
					<xsl:value-of select="'Local Interest'"/>
				</LocalInterest>
				<NetProceeds>
					<xsl:value-of select="'NetProceeds'"/>
				</NetProceeds>
				<Currency>
					<xsl:value-of select="'Currency'"/>
				</Currency>
				<SettleCurrency>
					<xsl:value-of select="'SettleCurrency'"/>
				</SettleCurrency>
				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>
				<SettleDate>
					<xsl:value-of select="'Settle Date'"/>
				</SettleDate>
				<ExecutingBroker>
					<xsl:value-of select="'Executing Broker'"/>
				</ExecutingBroker>
				<ClearingBrokerCustodianFCM>
					<xsl:value-of select="'Clearing Broker / Custodian/ FCM'"/>
				</ClearingBrokerCustodianFCM>
				<Strategy>
					<xsl:value-of select="'Strategy'"/>
				</Strategy>
				<Exchange>
					<xsl:value-of select="'Exchange'"/>
				</Exchange>
				<UnderlyingAssetType>
					<xsl:value-of select="'Underlying Asset Type'"/>
				</UnderlyingAssetType>
				<UnderlyingID>
					<xsl:value-of select="'Underlying ID'"/>
				</UnderlyingID>
				<RepoType>
					<xsl:value-of select="'Repo Type'"/>
				</RepoType>
				<RepoTermDate>
					<xsl:value-of select="'Repo Term Date'"/>
				</RepoTermDate>
				<RepoFixedRate>
					<xsl:value-of select="'Repo Fixed Rate'"/>
				</RepoFixedRate>
				<RepoVariableRateCode>
					<xsl:value-of select="'Repo Variable Rate Code'"/>
				</RepoVariableRateCode>
				<RepoAccrualType>
					<xsl:value-of select="'Repo Accrual Type'"/>
				</RepoAccrualType>
				<RepoPoints>
					<xsl:value-of select="'Repo Points'"/>
				</RepoPoints>
				<RepoHaircut>
					<xsl:value-of select="'Repo Haircut'"/>
				</RepoHaircut>
				<BrokerExecutionCommission>
					<xsl:value-of select="'Broker Execution Commission'"/>
				</BrokerExecutionCommission>
				<BrokerClearingFees>
					<xsl:value-of select="'Broker Clearing Fees'"/>
				</BrokerClearingFees>
				<ExchangeFees>
					<xsl:value-of select="'Exchange Fees'"/>
				</ExchangeFees>
				<StampDuty>
					<xsl:value-of select="'Stamp Duty'"/>
				</StampDuty>
				<LocalLevy>
					<xsl:value-of select="'Local Levy'"/>
				</LocalLevy>
				<SECFeeOverride>
					<xsl:value-of select="'SEC Fee Override'"/>
				</SECFeeOverride>
				<OptionsRegFeeOverride>
					<xsl:value-of select="'Options Reg Fee Override'"/>
				</OptionsRegFeeOverride>
				<OtherCommType1>
					<xsl:value-of select="'Other Comm Type 1'"/>
				</OtherCommType1>
				<OtherCommAmount1>
					<xsl:value-of select="'Other Comm Amount 1'"/>
				</OtherCommAmount1>
				<OtherCommType2>
					<xsl:value-of select="'Other Comm Type 2'"/>
				</OtherCommType2>
				<OtherCommAmount2>
					<xsl:value-of select="'Other Comm Amount 2'"/>
				</OtherCommAmount2>
				<Description>
					<xsl:value-of select="'Description'"/>
				</Description>
				<BeginDate>
					<xsl:value-of select="'Begin Date'"/>
				</BeginDate>
				<EndDate>
					<xsl:value-of select="'End Date'"/>
				</EndDate>
				<Leg1Notional>
					<xsl:value-of select="'Leg 1 Notional'"/>
				</Leg1Notional>
				<Leg1LegType>
					<xsl:value-of select="'Leg 1 Leg Type'"/>
				</Leg1LegType>
				<Leg1FixedRate>
					<xsl:value-of select="'Leg 1 Fixed Rate'"/>
				</Leg1FixedRate>
				<Leg1VariableRateCode>
					<xsl:value-of select="'Leg 1 Variable Rate Code'"/>
				</Leg1VariableRateCode>
				<Leg1StubRate>
					<xsl:value-of select="'Leg 1 Stub Rate'"/>
				</Leg1StubRate>
				<Leg1VariablePoints>
					<xsl:value-of select="'Leg 1 Variable Points'"/>
				</Leg1VariablePoints>
				<Leg1EquityBondAssetID>
					<xsl:value-of select="'Leg 1 Equity\Bond Asset ID'"/>
				</Leg1EquityBondAssetID>
				<Leg1ResetFreq>
					<xsl:value-of select="'Leg 1 Reset Freq'"/>
				</Leg1ResetFreq>
				<Leg1EndofMonthConvention>
					<xsl:value-of select="'Leg 1 End of Month Convention'"/>
				</Leg1EndofMonthConvention>
				<Leg1AccrualConvention>
					<xsl:value-of select="'Leg 1 Accrual Convention'"/>
				</Leg1AccrualConvention>
				<Leg1AccrualType>
					<xsl:value-of select="'Leg 1 Accrual Type'"/>
				</Leg1AccrualType>
				<Leg1PayFreq>
					<xsl:value-of select="'Leg 1 Pay Freq'"/>
				</Leg1PayFreq>
				<Leg1CompoundInterest>
					<xsl:value-of select="'Leg 1 Compound Interest'"/>
				</Leg1CompoundInterest>
				<Leg1CurrencyofLeg>
					<xsl:value-of select="'Leg 1 Currency of Leg'"/>
				</Leg1CurrencyofLeg>
				<Leg1PayRecIndicator>
					<xsl:value-of select="'Leg 1 Pay/Rec Indicator'"/>
				</Leg1PayRecIndicator>
				<Leg1StartDate>
					<xsl:value-of select="'Leg 1 Start Date'"/>
				</Leg1StartDate>
				<Leg1FirstResetDate>
					<xsl:value-of select="'Leg 1 First Reset Date'"/>
				</Leg1FirstResetDate>
				<Leg2Notional>
					<xsl:value-of select="'Leg 2 Notional'"/>
				</Leg2Notional>
				<Leg2LegType>
					<xsl:value-of select="'Leg 2 Leg Type'"/>
				</Leg2LegType>
				<Leg2FixedRate>
					<xsl:value-of select="'Leg 2 Fixed Rate'"/>
				</Leg2FixedRate>
				<Leg2VariableRateCode>
					<xsl:value-of select="'Leg 2 Variable Rate Code'"/>
				</Leg2VariableRateCode>
				<Leg2StubRate>
					<xsl:value-of select="'Leg 2 Stub Rate'"/>
				</Leg2StubRate>
				<Leg2VariablePoints>
					<xsl:value-of select="'Leg 2 Variable Points'"/>
				</Leg2VariablePoints>
				<Leg2EquityBondAssetID>
					<xsl:value-of select="'Leg 2 Equity\Bond Asset ID'"/>
				</Leg2EquityBondAssetID>
				<Leg2Price>
					<xsl:value-of select="'Leg 2 Price'"/>
				</Leg2Price>
				<Leg2ResetFreq>
					<xsl:value-of select="'Leg 2 Reset Freq'"/>
				</Leg2ResetFreq>
				<Leg2EndofMonthConvention>
					<xsl:value-of select="'Leg 2 End of Month Convention'"/>
				</Leg2EndofMonthConvention>
				<Leg2AccrualConvention>
					<xsl:value-of select="'Leg 2 Accrual Convention'"/>
				</Leg2AccrualConvention>
				<Leg2AccrualType>
					<xsl:value-of select="'Leg 2 Accrual Type'"/>
				</Leg2AccrualType>
				<Leg2PayFreq>
					<xsl:value-of select="'Leg 2 Pay Freq'"/>
				</Leg2PayFreq>
				<Leg2CompoundInterest>
					<xsl:value-of select="'Leg 2 Compound Interest'"/>
				</Leg2CompoundInterest>
				<Leg2CurrencyofLeg>
					<xsl:value-of select="'Leg 2 Currency of Leg'"/>
				</Leg2CurrencyofLeg>
				<Leg2PayRec>
					<xsl:value-of select="'Leg 2 Pay/Rec'"/>
				</Leg2PayRec>
				<Leg2StartDate>
					<xsl:value-of select="'Leg 2 Start Date'"/>
				</Leg2StartDate>
				<Leg2FirstResetDate>
					<xsl:value-of select="'Leg 2 First Reset Date'"/>
				</Leg2FirstResetDate>
				<Issuer>
					<xsl:value-of select="'Issuer'"/>
				</Issuer>
				<StrikePrice>
					<xsl:value-of select="'Strike Price'"/>
				</StrikePrice>
				<PutCall>
					<xsl:value-of select="'Put/Call'"/>
				</PutCall>
				<ExpirationDate>
					<xsl:value-of select="'Expiration Date'"/>
				</ExpirationDate>
				<Valueof1Tick>
					<xsl:value-of select="'Value of 1 Tick'"/>
				</Valueof1Tick>
				<UnitsPerLot>
					<xsl:value-of select="'Units Per Lot'"/>
				</UnitsPerLot>
				<ExerciseType>
					<xsl:value-of select="'Exercise Type'"/>
				</ExerciseType>


				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[IsSwapped ='true']">				
					<ThirdPartyFlatFileDetail>
						
						<RowHeader>
							<xsl:value-of select ="'false'"/>
						</RowHeader>

						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>
						<xsl:variable name="varTransactionAction">
							<xsl:choose>
								<xsl:when test="TaxLotState='Allocated'">
									<xsl:value-of select="'New'"/>
								</xsl:when>
								<xsl:when test="TaxLotState='Amended'">
									<xsl:value-of select="'Amend'"/>
								</xsl:when>
								<xsl:when test="TaxLotState='Deleted'">
									<xsl:value-of select="'Cancel'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<TradeID>
							<xsl:value-of select="PBUniqueID"/>
						</TradeID>
						<FundCode>
							<xsl:value-of select="'038CAB8P2'"/>
						</FundCode>
						<TransactionType>
							<xsl:value-of select="$varTransactionAction"/>
						</TransactionType>
						<ProductType>
							<xsl:value-of select="' '"/>
						</ProductType>
						<BloombergYellowKey>
							<xsl:value-of select="BBCode"/>
						</BloombergYellowKey>
						<Ticker>
							<xsl:value-of select="Symbol"/>
						</Ticker>
						<ISIN>
							<xsl:value-of select="ISIN"/>
						</ISIN>
						<CUSIP>
							<xsl:value-of select="CUSIP"/>
						</CUSIP>
						<SEDOL>
							<xsl:value-of select="SEDOL"/>
						</SEDOL>
						<OCCCode>
							<xsl:value-of select="' '"/>
						</OCCCode>
						<ISOCurrencyCode>
							<xsl:value-of select="' '"/>
						</ISOCurrencyCode>
						<PrivateAssetID>
							<xsl:value-of select="' '"/>
						</PrivateAssetID>
						<TradeType>
							<xsl:choose>
								<xsl:when test="Side='Buy' or Side='Buy to Open'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="Side='Sell' or Side='Sell to Open' or Side='Sell to Close'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:when test="Side='Sell short'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								<xsl:when test="(Side='Buy to Cover' or Side='Buy to Close')">
									<xsl:value-of select="'Buy to Cover'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="Side"/>
								</xsl:otherwise>
							</xsl:choose>
						</TradeType>
						<OriginalFace>
							<xsl:value-of select="' '"/>
						</OriginalFace>
						<Quantity>
							<xsl:value-of select="AllocatedQty"/>
						</Quantity>
						<Price>
							<xsl:value-of select="AveragePrice"/>
						</Price>
						<Factor>
							<xsl:value-of select="' '"/>
						</Factor>
						<LocalInterest>
							<xsl:value-of select="' '"/>
						</LocalInterest>
						<NetProceeds>
							<xsl:value-of select="' '"/>
						</NetProceeds>
						<Currency>
							<xsl:value-of select="CurrencySymbol"/>
						</Currency>
						<SettleCurrency>
							<xsl:value-of select="SettlCurrency"/>
						</SettleCurrency>
						<TradeDate>
							<xsl:value-of select="TradeDate"/>
						</TradeDate>
						<SettleDate>
							<xsl:value-of select="SettlementDate"/>
						</SettleDate>
						<xsl:variable name="Pb_name" select="''"/>
						<xsl:variable name="PRANA_COUNTERPARTY">
							<xsl:value-of select="CounterParty"/>
						</xsl:variable>

						<xsl:variable name="PB_COUNTERPARTY">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name = $Pb_name]/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@PBBroker"/>
						</xsl:variable>

						<xsl:variable name="varCounterParty">
							<xsl:choose>
								<xsl:when test="$PB_COUNTERPARTY = ''">
									<xsl:value-of select="$PRANA_COUNTERPARTY"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COUNTERPARTY"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<ExecutingBroker>
							<xsl:value-of select="$varCounterParty"/>
						</ExecutingBroker>
						<ClearingBrokerCustodianFCM>
							<xsl:value-of select="$varCounterParty"/>
						</ClearingBrokerCustodianFCM>
						<Strategy>
							<xsl:value-of select="' '"/>
						</Strategy>
						<Exchange>
							<xsl:value-of select="' '"/>
						</Exchange>
						<UnderlyingAssetType>
							<xsl:value-of select="' '"/>
						</UnderlyingAssetType>
						<UnderlyingID>
							<xsl:value-of select="' '"/>
						</UnderlyingID>
						<RepoType>
							<xsl:value-of select="' '"/>
						</RepoType>
						<RepoTermDate>
							<xsl:value-of select="' '"/>
						</RepoTermDate>
						<RepoFixedRate>
							<xsl:value-of select="' '"/>
						</RepoFixedRate>
						<RepoVariableRateCode>
							<xsl:value-of select="' '"/>
						</RepoVariableRateCode>
						<RepoAccrualType>
							<xsl:value-of select="' '"/>
						</RepoAccrualType>
						<RepoPoints>
							<xsl:value-of select="' '"/>
						</RepoPoints>
						<RepoHaircut>
							<xsl:value-of select="' '"/>
						</RepoHaircut>
						<BrokerExecutionCommission>
							<xsl:value-of select="' '"/>
						</BrokerExecutionCommission>
						<BrokerClearingFees>
							<xsl:value-of select="' '"/>
						</BrokerClearingFees>
						<ExchangeFees>
							<xsl:value-of select="' '"/>
						</ExchangeFees>
						<StampDuty>
							<xsl:value-of select="' '"/>
						</StampDuty>
						<LocalLevy>
							<xsl:value-of select="' '"/>
						</LocalLevy>
						<SECFeeOverride>
							<xsl:value-of select="' '"/>
						</SECFeeOverride>
						<OptionsRegFeeOverride>
							<xsl:value-of select="' '"/>
						</OptionsRegFeeOverride>
						<OtherCommType1>
							<xsl:value-of select="' '"/>
						</OtherCommType1>
						<OtherCommAmount1>
							<xsl:value-of select="' '"/>
						</OtherCommAmount1>
						<OtherCommType2>
							<xsl:value-of select="' '"/>
						</OtherCommType2>
						<OtherCommAmount2>
							<xsl:value-of select="' '"/>
						</OtherCommAmount2>
						<Description>
							<xsl:value-of select="' '"/>
						</Description>
						<BeginDate>
							<xsl:value-of select="' '"/>
						</BeginDate>
						<EndDate>
							<xsl:value-of select="' '"/>
						</EndDate>
						<Leg1Notional>
							<xsl:value-of select="' '"/>
						</Leg1Notional>
						<Leg1LegType>
							<xsl:value-of select="' '"/>
						</Leg1LegType>
						<Leg1FixedRate>
							<xsl:value-of select="' '"/>
						</Leg1FixedRate>
						<Leg1VariableRateCode>
							<xsl:value-of select="' '"/>
						</Leg1VariableRateCode>
						<Leg1StubRate>
							<xsl:value-of select="' '"/>
						</Leg1StubRate>
						<Leg1VariablePoints>
							<xsl:value-of select="' '"/>
						</Leg1VariablePoints>
						<Leg1EquityBondAssetID>
							<xsl:value-of select="' '"/>
						</Leg1EquityBondAssetID>
						<Leg1ResetFreq>
							<xsl:value-of select="' '"/>
						</Leg1ResetFreq>
						<Leg1EndofMonthConvention>
							<xsl:value-of select="' '"/>
						</Leg1EndofMonthConvention>
						<Leg1AccrualConvention>
							<xsl:value-of select="' '"/>
						</Leg1AccrualConvention>
						<Leg1AccrualType>
							<xsl:value-of select="' '"/>
						</Leg1AccrualType>
						<Leg1PayFreq>
							<xsl:value-of select="' '"/>
						</Leg1PayFreq>
						<Leg1CompoundInterest>
							<xsl:value-of select="' '"/>
						</Leg1CompoundInterest>
						<Leg1CurrencyofLeg>
							<xsl:value-of select="' '"/>
						</Leg1CurrencyofLeg>
						<Leg1PayRecIndicator>
							<xsl:value-of select="' '"/>
						</Leg1PayRecIndicator>
						<Leg1StartDate>
							<xsl:value-of select="' '"/>
						</Leg1StartDate>
						<Leg1FirstResetDate>
							<xsl:value-of select="' '"/>
						</Leg1FirstResetDate>
						<Leg2Notional>
							<xsl:value-of select="' '"/>
						</Leg2Notional>
						<Leg2LegType>
							<xsl:value-of select="' '"/>
						</Leg2LegType>
						<Leg2FixedRate>
							<xsl:value-of select="' '"/>
						</Leg2FixedRate>
						<Leg2VariableRateCode>
							<xsl:value-of select="' '"/>
						</Leg2VariableRateCode>
						<Leg2StubRate>
							<xsl:value-of select="' '"/>
						</Leg2StubRate>
						<Leg2VariablePoints>
							<xsl:value-of select="' '"/>
						</Leg2VariablePoints>
						<Leg2EquityBondAssetID>
							<xsl:value-of select="' '"/>
						</Leg2EquityBondAssetID>
						<Leg2Price>
							<xsl:value-of select="' '"/>
						</Leg2Price>
						<Leg2ResetFreq>
							<xsl:value-of select="' '"/>
						</Leg2ResetFreq>
						<Leg2EndofMonthConvention>
							<xsl:value-of select="' '"/>
						</Leg2EndofMonthConvention>
						<Leg2AccrualConvention>
							<xsl:value-of select="' '"/>
						</Leg2AccrualConvention>
						<Leg2AccrualType>
							<xsl:value-of select="' '"/>
						</Leg2AccrualType>
						<Leg2PayFreq>
							<xsl:value-of select="' '"/>
						</Leg2PayFreq>
						<Leg2CompoundInterest>
							<xsl:value-of select="' '"/>
						</Leg2CompoundInterest>
						<Leg2CurrencyofLeg>
							<xsl:value-of select="' '"/>
						</Leg2CurrencyofLeg>
						<Leg2PayRec>
							<xsl:value-of select="' '"/>
						</Leg2PayRec>
						<Leg2StartDate>
							<xsl:value-of select="' '"/>
						</Leg2StartDate>
						<Leg2FirstResetDate>
							<xsl:value-of select="' '"/>
						</Leg2FirstResetDate>
						<Issuer>
							<xsl:value-of select="' '"/>
						</Issuer>
						<StrikePrice>
							<xsl:value-of select="' '"/>
						</StrikePrice>
						<PutCall>
							<xsl:value-of select="' '"/>
						</PutCall>
						<ExpirationDate>
							<xsl:value-of select="' '"/>
						</ExpirationDate>
						<Valueof1Tick>
							<xsl:value-of select="' '"/>
						</Valueof1Tick>
						<UnitsPerLot>
							<xsl:value-of select="' '"/>
						</UnitsPerLot>
						<ExerciseType>
							<xsl:value-of select="' '"/>
						</ExerciseType>



						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
			
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>