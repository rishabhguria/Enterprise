<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >

	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
	
	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	

	<xsl:template match="/">
		<ThirdPartyFlatFileDetailCollection>

			<ThirdPartyFlatFileDetail>
				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<!--1-->
				<BatchNum>
					<xsl:value-of select="'BatchNum'"/>
				</BatchNum>

				<!--2-->
				<MasterReference>
					<xsl:value-of select="'Master Reference'"/>
				</MasterReference>

				<!--3-->
				<ClientAllocationReference>
					<xsl:value-of select="'ClientAllocationReference'"/>
				</ClientAllocationReference>

				<!--4-->
				<L2MatchingProfileName>
					<xsl:value-of select="'L2MatchingProfileName'"/>
				</L2MatchingProfileName>

				<!--5-->
				<QuantityOfTheBlockTrade>
					<xsl:value-of select="'QuantityOfTheBlockTrade'"/>
				</QuantityOfTheBlockTrade>

				<!--6-->
				<ExecutingBroker>
					<xsl:value-of select="'ExecutingBroker'"/>
				</ExecutingBroker>

				<!--7-->
				<OriginatorOfMessage>
					<xsl:value-of select="'OriginatorOfMessage'"/>
				</OriginatorOfMessage>

				<!--8-->
				<TradeDateTime>
					<xsl:value-of select="'TradeDate'"/>
				</TradeDateTime>

				<!--9-->
				<Time>
					<xsl:value-of select="'TradeTime'"/>
				</Time>


				<!--10-->
				<BuySellIndicator>
					<xsl:value-of select="'BuySellIndicator'"/>
				</BuySellIndicator>

				<!--11-->
				<DealPrice>
					<xsl:value-of select="'DealPrice'"/>
				</DealPrice>

				<!--12-->
				<IdentificationOfASecurity>
					<xsl:value-of select="'IdentificationOfASecurity'"/>
				</IdentificationOfASecurity>

				<!--13-->
				<DescriptionOfTheSecurity>
					<xsl:value-of select="'DescriptionOfTheSecurity'"/>
				</DescriptionOfTheSecurity>

				<!--14-->
				<SettlementDate>
					<xsl:value-of select="'SettlementDate'"/>
				</SettlementDate>


				<!--15-->
				<CurrencyCode>
					<xsl:value-of select="'CurrencyCode'"/>
				</CurrencyCode>

				<!--16-->
				<TradeTransactionConditionIndicator>
					<xsl:value-of select="'TradeTransactionConditionIndicator'"/>
				</TradeTransactionConditionIndicator>

				<!--17-->
				<TotalTradeAmount>
					<xsl:value-of select="'TotalTradeAmount'"/>
				</TotalTradeAmount>

				<!--18-->
				<PartyCapacityIndicator>
					<xsl:value-of select="'PartyCapacityIndicator'"/>
				</PartyCapacityIndicator>

				<!--19-->
				<TradeCommAmt>
					<xsl:value-of select="'TradeCommAmt'"/>
				</TradeCommAmt>

				<!--20-->
				<PlaceOfSafekeepingValue>
					<xsl:value-of select="'PlaceOfSafekeepingValue'"/>
				</PlaceOfSafekeepingValue>

				<!--21-->
				<PlaceOfSafekeepingPlace>
					<xsl:value-of select="'PlaceOfSafekeepingPlace'"/>
				</PlaceOfSafekeepingPlace>

				<!--22-->
				<TypeOfPriceIndicator>
					<xsl:value-of select="'Net Price Indicator'"/>
				</TypeOfPriceIndicator>

				<!--23-->
				<TypeOfPriceIndicator1>
					<xsl:value-of select="'Average Price Indicator'"/>
				</TypeOfPriceIndicator1>

				<!--24-->
				<ContSettCurr>
					<xsl:value-of select="'ContSettCurr'"/>
				</ContSettCurr>

				<!--25-->
				<ExchRate>
					<xsl:value-of select="'ExchRate'"/>
				</ExchRate>

				<!--26-->
				<CommissionSharingTypeIndicator>
					<xsl:value-of select="'HardSoftDir'"/>
				</CommissionSharingTypeIndicator>

				<!--27-->
				<SettlementInstructionsSourceIndicator>
					<xsl:value-of select="'SSI Indicator'"/>
				</SettlementInstructionsSourceIndicator>

				<!--28-->
				<PartyFundName>
					<xsl:value-of select="'AcctName'"/>
				</PartyFundName>

				<!--29-->
				<AccountID>
					<xsl:value-of select="'Account Number'"/>
				</AccountID>

				<!--30-->
				<AlertCountryCode>
					<xsl:value-of select="'Alert Country Code'"/>
				</AlertCountryCode>

				<!--31-->
				<AlertSecurityType>
					<xsl:value-of select="'Alert Security Type'"/>
				</AlertSecurityType>

				<!--32-->
				<AlertMethodType>
					<xsl:value-of select="'Alert Clearing Method'"/>
				</AlertMethodType>

				<!--33-->
				<SettlementInstructionsProcessingNarrative>
					<xsl:value-of select="'AlDelivText'"/>
				</SettlementInstructionsProcessingNarrative>

				<!--34-->
				<QuantityAllocated>
					<xsl:value-of select="'Allocation Quantity'"/>
				</QuantityAllocated>

				<!--35-->
				<NetCashAmount>
					<xsl:value-of select="'Allocation Net Amount'"/>
				</NetCashAmount>

				<!--36-->
				<ChargeAmount>
					<xsl:value-of select="'Other Amount'"/>
				</ChargeAmount>

				<!--37-->
				<Commission>
					<xsl:value-of select="'Total Allocation Commission'"/>
				</Commission>

				<!--38-->
				<TradeAmount>
					<xsl:value-of select="'Allocation Gross Amount'"/>
				</TradeAmount>

				<!--39-->
				<Charges>
					<xsl:value-of select="'Charges/Fee'"/>
				</Charges>

				<!--40-->
				<Transaction>
					<xsl:value-of select="'Transaction Taxes'"/>
				</Transaction>

				<!--41-->
				<Local>
					<xsl:value-of select="'Local Taxes'"/>
				</Local>

				<!--42-->
				<AccruedInterestAmount>
					<xsl:value-of select="'AccrInt'"/>
				</AccruedInterestAmount>

				<!--43-->
				<UserDefined>
					<xsl:value-of select="'UserDefined'"/>
				</UserDefined>

				<!--44-->
				<BrokerOfCredit>
					<xsl:value-of select="'Other Broker of Credit'"/>
				</BrokerOfCredit>

				<!--45-->
				<FixAccrInt>
					<xsl:value-of select="'FixAccrInt'"/>
				</FixAccrInt>

				<!--46-->
				<FixAccrCurr>
					<xsl:value-of select="'FixAccrCurr'"/>
				</FixAccrCurr>

				<!--47-->
				<NumberOfDaysAccrued>
					<xsl:value-of select="'FixDaysInt'"/>
				</NumberOfDaysAccrued>

				<!--48-->
				<MaturityDate>
					<xsl:value-of select="'FixMatDate'"/>
				</MaturityDate>

				<!--49-->
				<CouponRate>
					<xsl:value-of select="'FixCoupInt'"/>
				</CouponRate>

				<!--50-->
				<OriginalFaceAmount>
					<xsl:value-of select="'FixOrigFv'"/>
				</OriginalFaceAmount>

				<!--51-->
				<FixCurrFv>
					<xsl:value-of select="'FixCurrFv'"/>
				</FixCurrFv>

				<!--52-->
				<CurrentFactor>
					<xsl:value-of select="'FixFactor'"/>
				</CurrentFactor>

				<!--53-->
				<Yield>
					<xsl:value-of select="'FixCurrentYld'"/>
				</Yield>

				<!--54-->
				<FixYldToMat>
					<xsl:value-of select="'FixYldToMat'"/>
				</FixYldToMat>

				<!--55-->
				<FixRepYld>
					<xsl:value-of select="'FixRepYld'"/>
				</FixRepYld>

				<!--56-->
				<CallType>
					<xsl:value-of select="'FixTypeOfCall'"/>
				</CallType>

				<!--57-->
				<FixYldToCall>
					<xsl:value-of select="'FixYldToCall'"/>
				</FixYldToCall>

				<!--58-->
				<CallPrice>
					<xsl:value-of select="'CallPrice'"/>
				</CallPrice>

				<!--59-->
				<CallDate>
					<xsl:value-of select="'FixCallDate'"/>
				</CallDate>

				<!--60-->
				<DatedDate>
					<xsl:value-of select="'FixDatedDate'"/>
				</DatedDate>

				<!--61-->
				<FixOdd1stCouponDate>
					<xsl:value-of select="'FixOdd1stCouponDate'"/>
				</FixOdd1stCouponDate>

				<!--62-->
				<BookEntry>
					<xsl:value-of select="'FixBookEntry'"/>
				</BookEntry>

				<!--63-->
				<Issuer>
					<xsl:value-of select="'FixIssuer'"/>
				</Issuer>

				<!--64-->
				<FixMoody>
					<xsl:value-of select="'FixMoody'"/>
				</FixMoody>

				<!--65-->
				<FixSP>
					<xsl:value-of select="'FixSP'"/>
				</FixSP>

				<!--66-->
				<FixFedTax>
					<xsl:value-of select="'FixFedTax'"/>
				</FixFedTax>

				<!--67-->
				<FixAltMinTax>
					<xsl:value-of select="'FixAltMinTax'"/>
				</FixAltMinTax>

				<!--68-->
				<LotSize>
					<xsl:value-of select="'LotSize'"/>
				</LotSize>

				<!--69-->
				<FunctionOfMsg>
					<xsl:value-of select="'FunctionOfMsg'"/>
				</FunctionOfMsg>

				<!--70-->
				<Version>
					<xsl:value-of select="'Version'"/>
				</Version>

				<!--71-->
				<CancellationIndicator>
					<xsl:value-of select="'CancellationIndicator'"/>
				</CancellationIndicator>

				<!--72-->
				<PlaceCode>
					<xsl:value-of select="'Place of Trade Code'"/>
				</PlaceCode>

				<!--73-->
				<PlaceNarrative>
					<xsl:value-of select="'Place of Trade Narrative*'"/>
				</PlaceNarrative>

				<!--74-->
				<SettlementCurrency>
					<xsl:value-of select="'SettlementCurrency'"/>
				</SettlementCurrency>

				<!--75-->
				<SettlementAmount>
					<xsl:value-of select="'SettlementAmount'"/>
				</SettlementAmount>

				<!--76-->
				<SettlementTransactionConditionIndicator>
					<xsl:value-of select="'SettlementTransactionConditionIndicator'"/>
				</SettlementTransactionConditionIndicator>

				<!--77-->
				<CommissionType1>
					<xsl:value-of select="'CommissionType1'"/>
				</CommissionType1>

				<!--78-->
				<CommissionAmount1>
					<xsl:value-of select="'CommissionAmount1'"/>
				</CommissionAmount1>

				<!--79-->
				<CommissionType2>
					<xsl:value-of select="'CommissionType2'"/>
				</CommissionType2>

				<!--80-->
				<CommissionAmount2>
					<xsl:value-of select="'CommissionAmount2'"/>
				</CommissionAmount2>

				<!--81-->
				<CommissionType3>
					<xsl:value-of select="'CommissionType3'"/>
				</CommissionType3>

				<!--82-->
				<CommissionAmount3>
					<xsl:value-of select="'CommissionAmount3'"/>
				</CommissionAmount3>

				<!--83-->
				<TotalFeeAmount>
					<xsl:value-of select="'Total Fee Amount'"/>
				</TotalFeeAmount>

				<!--84-->
				<ChargeTaxType1>
					<xsl:value-of select="'ChargeTaxType1'"/>
				</ChargeTaxType1>

				<!--85-->
				<ChargeAmount1>
					<xsl:value-of select="'ChargeAmount1'"/>
				</ChargeAmount1>

				<!--86-->
				<ChargeTaxType2>
					<xsl:value-of select="'ChargeTaxType2'"/>
				</ChargeTaxType2>

				<!--87-->
				<ChargeAmount2>
					<xsl:value-of select="'ChargeAmount2'"/>
				</ChargeAmount2>

				<!--88-->
				<ChargeTaxType3>
					<xsl:value-of select="'ChargeTaxType3'"/>
				</ChargeTaxType3>

				<!--89-->
				<ChargeAmount3>
					<xsl:value-of select="'ChargeAmount3'"/>
				</ChargeAmount3>

				<!--90-->
				<ChargeTaxType4>
					<xsl:value-of select="'ChargeTaxType4'"/>
				</ChargeTaxType4>

				<!--91-->
				<ChargeAmount4>
					<xsl:value-of select="'ChargeAmount4'"/>
				</ChargeAmount4>

				<!--92-->
				<ChargeTaxType5>
					<xsl:value-of select="'ChargeTaxType5'"/>
				</ChargeTaxType5>

				<!--93-->
				<ChargeAmount5>
					<xsl:value-of select="'ChargeAmount5'"/>
				</ChargeAmount5>

				<!--94-->
				<CurrentFaceValue>
					<xsl:value-of select="'CurrentFaceValue'"/>
				</CurrentFaceValue>

				<!--95-->
				<SecurityTypeIndicator>
					<xsl:value-of select="'SecurityTypeIndicator'"/>
				</SecurityTypeIndicator>

				<!--96-->
				<ClearingBroker>
					<xsl:value-of select="'ClearingBroker'"/>
				</ClearingBroker>

				<!--97-->
				<TypeOfFinancialInstrument>
					<xsl:value-of select="'TypeOfFinancialInstrument'"/>
				</TypeOfFinancialInstrument>

				<!--98-->
				<OTCIndicator>
					<xsl:value-of select="'OTCIndicator'"/>
				</OTCIndicator>

				<!--99-->
				<OrderType>
					<xsl:value-of select="'OrderType'"/>
				</OrderType>

				<!--100-->
				<TradeAgreementMethod>
					<xsl:value-of select="'TradeAgreementMethod'"/>
				</TradeAgreementMethod>

				<!--101-->
				<AccountAtClearingBroker>
					<xsl:value-of select="'AccountAtClearingBroker'"/>
				</AccountAtClearingBroker>

				<!--102-->
				<PriceTypeCode>
					<xsl:value-of select="'PriceTypeCode'"/>
				</PriceTypeCode>

				<!--Test-->
				<!--103-->
				<FuturesUnderlyingAsset>
					<xsl:value-of select="'FuturesUnderlyingAsset'"/>
				</FuturesUnderlyingAsset>

				<!--104-->
				<FuturesUnderlyingAssetDescription>
					<xsl:value-of select="'FuturesUnderlyingAssetDescription'"/>
				</FuturesUnderlyingAssetDescription>

				<!--105-->
				<FuturesDeliveryType>
					<xsl:value-of select="'FuturesDeliveryType'"/>
				</FuturesDeliveryType>

				<!--106-->
				<FuturesTickSize>
					<xsl:value-of select="'FuturesTickSize'"/>
				</FuturesTickSize>

				<!--107-->
				<FuturesTickValue>
					<xsl:value-of select="'FuturesTickValue'"/>
				</FuturesTickValue>

				<!--108-->
				<FuturesMultiLegReportingType>
					<xsl:value-of select="'FuturesMultiLegReportingType'"/>
				</FuturesMultiLegReportingType>

				<!--109-->
				<FuturesIntMarginPrTypCode>
					<xsl:value-of select="'FuturesIntMarginPrTypCode'"/>
				</FuturesIntMarginPrTypCode>

				<!--110-->
				<FuturesIntMarginAmnt>
					<xsl:value-of select="'FuturesIntMarginAmnt'"/>
				</FuturesIntMarginAmnt>

				<!--111-->
				<FuturesDeliveryDate>
					<xsl:value-of select="'FuturesDeliveryDate'"/>
				</FuturesDeliveryDate>

				<!--112-->
				<OptionsUnderlyingAsset>
					<xsl:value-of select="'OptionsUnderlyingAsset'"/>
				</OptionsUnderlyingAsset>

				<!--113-->
				<OptionsUnderlyingAssetDescription>
					<xsl:value-of select="'OptionsUnderlyingAssetDescription'"/>
				</OptionsUnderlyingAssetDescription>

				<!--114-->
				<OptionsDeliveryType>
					<xsl:value-of select="'OptionsDeliveryType'"/>
				</OptionsDeliveryType>

				<!--115-->
				<OptionsTickSize>
					<xsl:value-of select="'OptionsTickSize'"/>
				</OptionsTickSize>

				<!--116-->
				<OptionsTickValue>
					<xsl:value-of select="'OptionsTickValue'"/>
				</OptionsTickValue>

				<!--117-->
				<OptionsMultiLegReportingType>
					<xsl:value-of select="'OptionsMultiLegReportingType'"/>
				</OptionsMultiLegReportingType>

				<!--118-->
				<OptionsExpirationDate>
					<xsl:value-of select="'OptionsExpirationDate'"/>
				</OptionsExpirationDate>

				<!--119-->
				<OptionsPutCallindicator>
					<xsl:value-of select="'OptionsPutCallindicator'"/>
				</OptionsPutCallindicator>

				<!--120-->
				<OptionStyle>
					<xsl:value-of select="'OptionStyle'"/>
				</OptionStyle>

				<!--121-->
				<OptionsStrikePrice>
					<xsl:value-of select="'OptionsStrikePrice'"/>
				</OptionsStrikePrice>

				<!--122-->
				<OptionsPremiumAmount>
					<xsl:value-of select="'OptionsPremiumAmount'"/>
				</OptionsPremiumAmount>

				<!--123-->
				<EqtSwapsTerminationDate>
					<xsl:value-of select="'EqtSwapsTerminationDate'"/>
				</EqtSwapsTerminationDate>

				<!--124-->
				<EqtSwapsEffectiveDate>
					<xsl:value-of select="'EqtSwapsEffectiveDate'"/>
				</EqtSwapsEffectiveDate>

				<!--125-->
				<EqtSwapsOnMarginIndicator>
					<xsl:value-of select="'EqtSwapsOnMarginIndicator'"/>
				</EqtSwapsOnMarginIndicator>

				<!--126-->
				<EqtSwapsUnderlyingAsset>
					<xsl:value-of select="'EqtSwapsUnderlyingAsset'"/>
				</EqtSwapsUnderlyingAsset>

				<!--127-->
				<EqtSwapsUnderlyingAssetDescription>
					<xsl:value-of select="'EqtSwapsUnderlyingAssetDescription'"/>
				</EqtSwapsUnderlyingAssetDescription>

				<!--128-->
				<EqtSwapsIntMarginPrTypCode>
					<xsl:value-of select="'EqtSwapsIntMarginPrTypCode'"/>
				</EqtSwapsIntMarginPrTypCode>

				<!--129-->
				<EqtSwapsIntMarginAmnt>
					<xsl:value-of select="'EqtSwapsIntMarginAmnt'"/>
				</EqtSwapsIntMarginAmnt>

				<!--130-->
				<EqtSwapsReturnLegResetDate>
					<xsl:value-of select="'EqtSwapsReturnLegResetDate'"/>
				</EqtSwapsReturnLegResetDate>

				<!--131-->
				<EqtSwapsReturnLegPaymentDate>
					<xsl:value-of select="'EqtSwapsReturnLegPaymentDate'"/>
				</EqtSwapsReturnLegPaymentDate>

				<!--132-->
				<EqtSwapsReturnLegNotionalAmount>
					<xsl:value-of select="'EqtSwapsReturnLegNotionalAmount'"/>
				</EqtSwapsReturnLegNotionalAmount>

				<!--133-->
				<EqtSwapsReturnLegInitialPrice>
					<xsl:value-of select="'EqtSwapsReturnLegInitialPrice'"/>
				</EqtSwapsReturnLegInitialPrice>

				<!--134-->
				<EqtSwapsReturnLegTypeofReturn>
					<xsl:value-of select="'EqtSwapsReturnLegTypeofReturn'"/>
				</EqtSwapsReturnLegTypeofReturn>

				<!--135-->
				<EqtSwapsReturnLegDividendPrTypCode>
					<xsl:value-of select="'EqtSwapsReturnLegDividendPrTypCode'"/>
				</EqtSwapsReturnLegDividendPrTypCode>

				<!--136-->
				<EqtSwapsReturnLegDividendAmnt>
					<xsl:value-of select="'EqtSwapsReturnLegDividendAmnt'"/>
				</EqtSwapsReturnLegDividendAmnt>

				<!--137-->
				<EqtSwapsInterestLegResetDate>
					<xsl:value-of select="'EqtSwapsInterestLegResetDate'"/>
				</EqtSwapsInterestLegResetDate>

				<!--138-->
				<EqtSwapsInterestLegPaymentDate>
					<xsl:value-of select="'EqtSwapsInterestLegPaymentDate'"/>
				</EqtSwapsInterestLegPaymentDate>

				<!--139-->
				<EqtSwapsInterestLegNotionalAmount>
					<xsl:value-of select="'EqtSwapsInterestLegNotionalAmount'"/>
				</EqtSwapsInterestLegNotionalAmount>

				<!--140-->
				<EqtSwapsInterestLegBenchmarkType>
					<xsl:value-of select="'EqtSwapsInterestLegBenchmarkType'"/>
				</EqtSwapsInterestLegBenchmarkType>

				<!--141-->
				<EqtSwapsInterestLegBenchmarkRate>
					<xsl:value-of select="'EqtSwapsInterestLegBenchmarkRate'"/>
				</EqtSwapsInterestLegBenchmarkRate>

				<!--142-->
				<EqtSwapsInterestLegDayCountBasis>
					<xsl:value-of select="'EqtSwapsInterestLegDayCountBasis'"/>
				</EqtSwapsInterestLegDayCountBasis>

				<!--143-->
				<EqtSwapsInterestLegSpread>
					<xsl:value-of select="'EqtSwapsInterestLegSpread'"/>
				</EqtSwapsInterestLegSpread>

				<!--144-->
				<EqtSwapsInterestLegIndexTenor>
					<xsl:value-of select="'EqtSwapsInterestLegIndexTenor'"/>
				</EqtSwapsInterestLegIndexTenor>

				<!--145-->
				<EqtSwapsInterestLegFixedRate>
					<xsl:value-of select="'EqtSwapsInterestLegFixedRate'"/>
				</EqtSwapsInterestLegFixedRate>


				<!--Test-->



				<!--146-->
				<OmniExpected>
					<xsl:value-of select="'OmniExpected'"/>
				</OmniExpected>

				<!--147-->
				<TradeDetailReferenceType>
					<xsl:value-of select="'TradeDetailReferenceType'"/>
				</TradeDetailReferenceType>

				<!--148-->
				<TradeDetailReferenceValue>
					<xsl:value-of select="'TradeDetailReferenceValue'"/>
				</TradeDetailReferenceValue>

				<!--149-->
				<IMCptySSISource>
					<xsl:value-of select="'IMCptySSISource'"/>
				</IMCptySSISource>

				<!--150-->
				<IMCptyAlertCountry>
					<xsl:value-of select="'IMCptyAlertCountry'"/>
				</IMCptyAlertCountry>

				<!--151-->
				<IMCptyAlertSecurity>
					<xsl:value-of select="'IMCptyAlertSecurity'"/>
				</IMCptyAlertSecurity>

				<!--152-->
				<IMCptyAlertClearMethod>
					<xsl:value-of select="'IMCptyAlertClearMethod'"/>
				</IMCptyAlertClearMethod>

				<!--153-->
				<IMCptyAlertSettlementModelName>
					<xsl:value-of select="'IMCptyAlertSettlementModelName'"/>
				</IMCptyAlertSettlementModelName>

				<!--154-->
				<IMCptyAlDelivText>
					<xsl:value-of select="'IMCptyAlDelivText'"/>
				</IMCptyAlDelivText>

				<!--155-->
				<ClearingBrkPartyType>
					<xsl:value-of select="'ClearingBrkPartyType'"/>
				</ClearingBrkPartyType>

				<!--156-->
				<ThirdPartyToTrade>
					<xsl:value-of select="'PrimeBroker'"/>
				</ThirdPartyToTrade>

				<!--157-->
				<PartyType>
					<xsl:value-of select="'PrimeBrkPartyType'"/>
				</PartyType>

				<!--158-->
				<PlaceOfClearing>
					<xsl:value-of select="'PlaceOfClearing'"/>
				</PlaceOfClearing>


				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
				
				

			</ThirdPartyFlatFileDetail>
			
			
			
			
			<xsl:variable name="subTotals"> 
				<xsl:for-each  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[(FundName = 'SAPDB_CASH' or FundName = 'S1DB_CASH' or FundName = 'SP29DB_CASH' or FundName = 'S2DB_CASH' or FundName = 'SAPGS_CASH' or FundName = 'S1GS_CASH' or FundName = 'SP29GS_CASH' or FundName = 'S2GS_CASH' or FundName = 'SAPCS_CASH' or FundName = 'S1CS_CASH' or FundName = 'SP29CS_CASH' or FundName = 'S2CS_CASH' or FundName = 'SP29MS_ALL')]">
				<xsl:sort select="BBCode"/>
					<xsl:sort select="Side"/>
					<xsl:sort select="CounterParty"/>
				</xsl:for-each>
			</xsl:variable>
			
				
				
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[(FundName = 'SAPDB_CASH' or FundName = 'S1DB_CASH' or FundName = 'SP29DB_CASH' or FundName = 'S2DB_CASH' or FundName = 'SAPGS_CASH' or FundName = 'S1GS_CASH' or FundName = 'SP29GS_CASH' or FundName = 'S2GS_CASH' or FundName = 'SAPCS_CASH' or FundName = 'S1CS_CASH' or FundName = 'S2CS_CASH' or FundName = 'SP29CS_CASH' or FundName = 'SP29MS_ALL')
			and (CurrencySymbol = 'HKD' or CurrencySymbol = 'JPY' or CurrencySymbol = 'SGD' or CurrencySymbol = 'IDR' or CurrencySymbol = 'NZD' or CurrencySymbol = 'MYR' or CurrencySymbol = 'PHP' or CurrencySymbol = 'THB')]">
				<!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
				
				<!--<xsl:sort select="normalize-space(concat(BBCode, ' ', Side, ' ', CounterParty))"/>-->
				
					<xsl:sort select="UnderlyingSymbol"/>
					<xsl:sort select="Side"/>
					<xsl:sort select="CounterParty"/>
				
				<xsl:if test="(position()=1 or ((preceding-sibling::*[1]/UnderlyingSymbol != UnderlyingSymbol) or (preceding-sibling::*[1]/Side != Side) or (preceding-sibling::*[1]/CounterParty != CounterParty)))">
					<!-- ...buid a Group for this node_id -->
					<xsl:call-template name="TaxLotIDBuilder">
						
						<xsl:with-param name="I_UnderlyingSymbol">
							<xsl:value-of select="UnderlyingSymbol" />
						</xsl:with-param>
						
						<xsl:with-param name="I_Side">
							<xsl:value-of select="Side" />
						</xsl:with-param>
						
						<xsl:with-param name="I_CounterParty">
							<xsl:value-of select="CounterParty" />
						</xsl:with-param>
						
						<xsl:with-param name="LowestBlockID">
							<xsl:value-of select="PBUniqueID" />
						</xsl:with-param>
						
						
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

	<xsl:template name="TaxLotIDBuilder">
		
		<xsl:param name="I_UnderlyingSymbol" />
		<xsl:param name="I_Side" />
		<xsl:param name="I_CounterParty" />
		<xsl:param name="LowestBlockID" />
				
		
		<xsl:variable name="CommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[(UnderlyingSymbol=$I_UnderlyingSymbol) and (Side=$I_Side) and (CounterParty=$I_CounterParty)][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>
		
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[(UnderlyingSymbol=$I_UnderlyingSymbol) and (Side=$I_Side) and (CounterParty=$I_CounterParty)][TaxLotState != 'Deleted']/ExecutedQty)"/>
		</xsl:variable>
	
		<xsl:variable name ="varRowHeaderValue">
			<xsl:value-of select="'false'"/>
		</xsl:variable>

		<xsl:variable name ="varIsCaptionChangeReqValue">
			<xsl:value-of select="true"/>
		</xsl:variable>

		<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[((UnderlyingSymbol=$I_UnderlyingSymbol) and (Side=$I_Side) and (CounterParty=$I_CounterParty)) and (FundName = 'SAPDB_CASH' or FundName = 'S1DB_CASH' or FundName = 'SP29DB_CASH' or FundName = 'S2DB_CASH' or FundName = 'SAPGS_CASH' or FundName = 'S1GS_CASH' or FundName = 'SP29GS_CASH' or FundName = 'S2GS_CASH' or FundName = 'SAPCS_CASH' or FundName = 'S1CS_CASH' or FundName = 'SP29CS_CASH' or FundName = 'S2CS_CASH' or FundName = 'SP29MS_ALL')
		and (CurrencySymbol = 'HKD' or CurrencySymbol = 'JPY' or CurrencySymbol = 'SGD' or CurrencySymbol = 'IDR' or CurrencySymbol = 'NZD' or CurrencySymbol = 'MYR' or CurrencySymbol = 'PHP' or CurrencySymbol = 'THB')]">

			<ThirdPartyFlatFileDetail>
				<!--for system internal use-->
				<RowHeader>
					<xsl:value-of select ="$varRowHeaderValue"/>
				</RowHeader>

				<!--for system use only-->
				<IsCaptionChangeRequired>
					<xsl:value-of select ="$varIsCaptionChangeReqValue"/>
				</IsCaptionChangeRequired>

				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="TaxLotState"/>
				</TaxLotState>

				<xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
				<xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

				<!--1-->
				<BatchNum>
					<xsl:value-of select="'1'"/>
				</BatchNum>

				<!--2-->
				<MasterReference>
					<xsl:value-of select="$LowestBlockID"/>
				</MasterReference>

				<!--3-->
				<ClientAllocationReference>
					<xsl:value-of select="TradeRefID"/>
				</ClientAllocationReference>

				<!--4-->
				<L2MatchingProfileName>
					<xsl:value-of select="''"/>
				</L2MatchingProfileName>

				<!--5-->
				<QuantityOfTheBlockTrade>
					<xsl:value-of select="$QtySum"/>
				</QuantityOfTheBlockTrade>

				<!--6-->
				<xsl:variable name="varCounterParty">
						<xsl:value-of select="CounterParty"/>
					</xsl:variable>
					
					<xsl:variable name="varEB">
						<xsl:value-of select="document('../ReconMappingXml/Omgeo_EBBroker.xml')/BrokerMapping/PB[@Name='SENSATO']/BrokerData[@PranaBroker = $varCounterParty]/@PBBroker"/>
					</xsl:variable>
					
					<ExecutingBroker>
						<xsl:choose>
							<xsl:when test="$varEB != ''">
								<xsl:value-of select ="$varEB"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CounterParty"/>
							</xsl:otherwise>
						</xsl:choose>
					</ExecutingBroker>
				
					
					<!-- For Testing with Omgeo -->
					<!--<ExecutingBroker>
						<xsl:value-of select="'BKMQPUSH'"/>
					</ExecutingBroker>-->

				<!--7-->
				<OriginatorOfMessage>
					<xsl:value-of select="'SEVTUS61'"/>
				</OriginatorOfMessage>

				<!--8-->
				<TradeDateTime>
					<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'))"/>
					<!--	<xsl:value-of select="TradeDateTime"/>  -->
				</TradeDateTime>

				<!--9-->
				<Time>
					<xsl:value-of select="'00000'"/>
				</Time>

				<!--10-->
				<xsl:variable name="varPosType">
					<xsl:choose>
						<xsl:when test="Side='Buy to Open' or Side='Buy'">
							<xsl:value-of select="'B'"/>
						</xsl:when>
						<xsl:when test=" Side='Sell' or Side='Sell to Close'">
							<xsl:value-of select="'S'"/>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open'">
							<xsl:value-of select="'S'"/>
						</xsl:when>
						<xsl:when test="Side='Buy to Close'">
							<xsl:value-of select="'B'"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>

				<BuySellIndicator>
					<xsl:value-of select="$varPosType"/>
				</BuySellIndicator>

				<!--11-->
				<DealPrice>
					<xsl:choose>
						<!-- GS Funds-->
						<xsl:when test ="($varEB = 'GSCO')">
							<xsl:value-of select='format-number(AveragePrice, "###.0000")'/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select='format-number(AveragePrice, "###.0000")'/>
						</xsl:otherwise>
					</xsl:choose>
				</DealPrice>

				<!--12-->
				<IdentificationOfASecurity>
					<xsl:choose>
						<xsl:when test ="Asset='Equity' and (FundName != 'SAPDB_CASH' and FundName != 'S1DB_CASH' and FundName != 'SP29DB_CASH' and FundName = 'S2DB_CASH' and FundName != 'SAPGS_CASH' and FundName != 'S1GS_CASH' and FundName != 'SP29GS_CASH' and FundName != 'S2GS_CASH' and FundName != 'SAPCS_CASH' and FundName != 'S1CS_CASH' and FundName != 'SP29CS_CASH' and FundName = 'S2CS_CASH' and FundName != 'SP29MS_ALL')">
							<xsl:value-of select="concat(SEDOL,'.CFD')"/>
						</xsl:when>

						<xsl:when test ="Asset='Equity'">
							<xsl:value-of select="SEDOL"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="Symbol"/>
						</xsl:otherwise>
					</xsl:choose>
				</IdentificationOfASecurity>

				<!--13-->
				<DescriptionOfTheSecurity>
					<xsl:value-of select="FullSecurityName"/>
				</DescriptionOfTheSecurity>

				<!--14-->
				<SettlementDate>
					<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),'-',substring-before(SettlementDate,'/'),'-',substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					<!--<xsl:value-of select="SettlementDate"/>-->
				</SettlementDate>

				<!--15-->
				<CurrencyCode>
					<xsl:value-of select ="CurrencySymbol"/>
				</CurrencyCode>

				<!--16-->
				<TradeTransactionConditionIndicator>
					<xsl:value-of select ="''"/>
				</TradeTransactionConditionIndicator>

				<!--17-->
				<TotalTradeAmount>
					<!--format-number($QtySum * AveragePrice, "###") '$QtySum * AveragePrice'-->
					<!--<xsl:value-of select='format-number($QtySum * AveragePrice, "###.00")'/>-->
					<xsl:value-of select='format-number($QtySum * format-number(AveragePrice, "###.0000"), "###.00")'/>
				</TotalTradeAmount>

				<!--18-->
				<PartyCapacityIndicator>
					<xsl:value-of select ="'A'"/>
				</PartyCapacityIndicator>

				<!--19-->
				<TradeCommAmt>
					<xsl:value-of select ="''"/>
				</TradeCommAmt>

				<!--20-->
				<PlaceOfSafekeepingValue>
					<xsl:value-of select ="''"/>
				</PlaceOfSafekeepingValue>

				<!--21-->
				<PlaceOfSafekeepingPlace>
					<xsl:value-of select ="''"/>
				</PlaceOfSafekeepingPlace>

				<!--22-->
				<TypeOfPriceIndicator>
					<xsl:value-of select ="''"/>
				</TypeOfPriceIndicator>

				<!--23-->
				<TypeOfPriceIndicator1>
					<xsl:value-of select ="''"/>
				</TypeOfPriceIndicator1>

				<!--24-->
				<ContSettCurr>
					<xsl:value-of select ="CurrencySymbol"/>
				</ContSettCurr>

				<!--25-->
				<!--<ExchRate>
					<xsl:value-of select ="1 div ForexRate"/>
				</ExchRate>-->
				
				<ExchRate>
					<xsl:value-of select ="''"/>
				</ExchRate>

				<!--26-->
				<CommissionSharingTypeIndicator>
					<xsl:value-of select ="'H'"/>
				</CommissionSharingTypeIndicator>

				<!--27  Need to change it to A as we move into production-->
				<SettlementInstructionsSourceIndicator>
					<xsl:value-of select="'A'"/>
				</SettlementInstructionsSourceIndicator>

				<!--28-->
				<PartyFundName>
					<xsl:value-of select ="FundName"/>
				</PartyFundName>

				<!--29-->
				
				<xsl:variable name="varFundCode">
						<xsl:value-of select="FundName"/>
					</xsl:variable>
				
				<xsl:variable name="varAccountNo">
						<xsl:value-of select="document('../ReconMappingXml/Omgeo_AccountNumberMapping.xml')/FundMapping/PB[@Name='SENSATO']/FundData[@PBFundCode = $varFundCode]/@PranaFund"/>
					</xsl:variable>
				
				
				<AccountID>
					<xsl:value-of select ="$varAccountNo"/>
				</AccountID>
				
				<!--<AccountID>
					<xsl:value-of select ="FundAccountNo"/>
				</AccountID>-->

				<!--30-->
				
				<xsl:variable name="varCurrencySymbol">
						<xsl:value-of select="CurrencySymbol"/>
					</xsl:variable>
				
				<xsl:variable name="varAlertCountryCode">
						<xsl:value-of select="document('../ReconMappingXml/Omgeo_AlertCountryCodeMapping.xml')/AlertCountryMapping/PB[@Name='SENSATO']/CurrencySymbol[@PranaSymbol = $varCurrencySymbol]/@AlertCountryCode"/>
					</xsl:variable>
					
					<xsl:variable name="varAlertMethodCode">
						<xsl:value-of select="document('../ReconMappingXml/Omgeo_AlertMethodMapping.xml')/AlertMethodMapping/PB[@Name='SENSATO']/CurrencySymbol[@PranaSymbol = $varCurrencySymbol]/@AlertMethodCode"/>
					</xsl:variable>
				
				<AlertCountryCode>
					<xsl:value-of select="$varAlertCountryCode"/>
				</AlertCountryCode>

				<!--31-->
				<AlertSecurityType>
					<xsl:value-of select="'EQU'"/>
				</AlertSecurityType>

				<!--32-->
				<AlertMethodType>
					<xsl:choose>
							<xsl:when test="$varAlertMethodCode != ''">
								<xsl:value-of select ="$varAlertMethodCode"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'CASH'"/>
							</xsl:otherwise>
						</xsl:choose>
				</AlertMethodType>

				<!--33-->
				<SettlementInstructionsProcessingNarrative>
					<xsl:value-of select="''"/>
				</SettlementInstructionsProcessingNarrative>

				<!--34-->
				<QuantityAllocated>
					<xsl:value-of select="AllocatedQty"/>
				</QuantityAllocated>

				<!--35-->
				<!--<NetCashAmount>
					<xsl:choose>
						<xsl:when test="CurrencySymbol != 'USD'">
							<xsl:value-of select='format-number((NetAmount * 1), "###.00")'/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select='format-number((NetAmount * 1), "###.00")'/>
						</xsl:otherwise>
					</xsl:choose>
					--><!--<xsl:value-of select='NetAmount'/>--><!--
				</NetCashAmount>-->


			


				<xsl:variable name="Commission">

					<xsl:choose>
						<xsl:when test ="CurrencySymbol = 'JPY'">
							<xsl:value-of select='format-number(CommissionCharged, "###")'/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select='format-number(CommissionCharged, "###.00")'/>
						</xsl:otherwise>
					</xsl:choose>
				
				
				</xsl:variable>


				<NetCashAmount>
					<xsl:choose>
						<xsl:when test ="NetAmount != 0">
							<xsl:choose>
								<xsl:when test ="CurrencySymbol = 'JPY'">
									<xsl:value-of select='format-number(NetAmount, "###")'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='format-number(NetAmount, "###.00")'/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="NetAmount"/>
						</xsl:otherwise>
					</xsl:choose>
				</NetCashAmount>
				
				<!--36-->
				<ChargeAmount>
					<xsl:value-of select="''"/>
				</ChargeAmount>

				<!--37-->

				<Commission>
					<xsl:choose>
						<xsl:when test ="CurrencySymbol = 'JPY'">
							<xsl:value-of select='format-number(CommissionCharged, "###")'/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select='format-number(CommissionCharged, "###.00")'/>
						</xsl:otherwise>
					</xsl:choose>
				</Commission>
					
				<!--38-->
				<TradeAmount>
					<xsl:value-of select='format-number(AllocatedQty * format-number(AveragePrice, "###.0000"),"###.00")'/>
				</TradeAmount>

				<!--39-->
				<!--<Charges>
					<xsl:value-of select="(ClearingFee + TransactionLevy + StampDuty + TaxOnCommissions + MiscFees)"/>
				</Charges-->

				<xsl:variable name="varTotalOtherChanges">
					<xsl:value-of select="(ClearingFee + TransactionLevy + StampDuty + TaxOnCommissions + MiscFees)"/>
				</xsl:variable>

				<xsl:variable name="TradeAmount">
					<xsl:value-of select='AllocatedQty * format-number(AveragePrice, "###.0000")'/>
				</xsl:variable>
				
				<Charges>
					<xsl:choose>
						<xsl:when test ="$varTotalOtherChanges != 0">
							<xsl:choose>
								<xsl:when test ="CurrencySymbol = 'JPY'">
									<xsl:value-of select='format-number($varTotalOtherChanges, "###")'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='format-number(($varTotalOtherChanges), "###.00")'/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="$varTotalOtherChanges"/>
						</xsl:otherwise>
					</xsl:choose>
				</Charges>

				<!--40-->
				<Transaction>
					<xsl:value-of select="''"/>
				</Transaction>

				<!--41-->
				<Local>
					<xsl:value-of select="''"/>
				</Local>

				<!--42-->
				<AccruedInterestAmount>
					<xsl:value-of select="''"/>
				</AccruedInterestAmount>

				<!--43-->
				<UserDefined>
					<xsl:value-of select="''"/>
				</UserDefined>

				<!--44-->
				<BrokerOfCredit>
					<xsl:value-of select="''"/>
				</BrokerOfCredit>

				<!--45-->
				<FixAccrInt>
					<xsl:value-of select="''"/>
				</FixAccrInt>

				<!--46-->
				<FixAccrCurr>
					<xsl:value-of select="''"/>
				</FixAccrCurr>

				<!--47-->
				<NumberOfDaysAccrued>
					<xsl:value-of select="''"/>
				</NumberOfDaysAccrued>

				<!--48-->
				<MaturityDate>
					<xsl:value-of select="''"/>
				</MaturityDate>

				<!--49-->
				<CouponRate>
					<xsl:value-of select="''"/>
				</CouponRate>

				<!--50-->
				<OriginalFaceAmount>
					<xsl:value-of select="''"/>
				</OriginalFaceAmount>

				<!--51-->
				<FixCurrFv>
					<xsl:value-of select="''"/>
				</FixCurrFv>

				<!--52-->
				<CurrentFactor>
					<xsl:value-of select="''"/>
				</CurrentFactor>

				<!--53-->
				<Yield>
					<xsl:value-of select="''"/>
				</Yield>

				<!--54-->
				<FixYldToMat>
					<xsl:value-of select="''"/>
				</FixYldToMat>

				<!--55-->
				<FixRepYld>
					<xsl:value-of select="''"/>
				</FixRepYld>

				<!--56-->
				<CallType>
					<xsl:value-of select="''"/>
				</CallType>

				<!--57-->
				<FixYldToCall>
					<xsl:value-of select="''"/>
				</FixYldToCall>

				<!--58-->
				<CallPrice>
					<xsl:value-of select="''"/>
				</CallPrice>

				<!--59-->
				<CallDate>
					<xsl:value-of select="''"/>
				</CallDate>

				<!--60-->
				<DatedDate>
					<xsl:value-of select="''"/>
				</DatedDate>

				<!--61-->
				<FixOdd1stCouponDate>
					<xsl:value-of select="''"/>
				</FixOdd1stCouponDate>

				<!--62-->
				<BookEntry>
					<xsl:value-of select="''"/>
				</BookEntry>

				<!--63-->
				<Issuer>
					<xsl:value-of select="''"/>
				</Issuer>

				<!--64-->
				<FixMoody>
					<xsl:value-of select="''"/>
				</FixMoody>

				<!--65-->
				<FixSP>
					<xsl:value-of select="''"/>
				</FixSP>

				<!--66-->
				<FixFedTax>
					<xsl:value-of select="''"/>
				</FixFedTax>

				<!--67-->
				<FixAltMinTax>
					<xsl:value-of select="''"/>
				</FixAltMinTax>

				<!--68-->
				<LotSize>
					<xsl:value-of select="''"/>
				</LotSize>

				<!--69-->
				<FunctionOfMsg>
					<xsl:value-of select="''"/>
				</FunctionOfMsg>

				<!--70-->
				<Version>
					<xsl:value-of select="''"/>
				</Version>

				<!--71-->
				<CancellationIndicator>
					<xsl:value-of select="''"/>
				</CancellationIndicator>

				<!--72-->
				<PlaceCode>
					<xsl:value-of select="''"/>
				</PlaceCode>

				<!--73-->
				<PlaceNarrative>
					<xsl:value-of select="''"/>
				</PlaceNarrative>

				<!--74-->
				<SettlementCurrency>
					<xsl:value-of select="''"/>
					<!--<xsl:choose>
						<xsl:when test ="(FundName = 'SAPGS_CASH' or FundName = 'SAPDB_CASH' or FundName = 'S1DB_CASH' or FundName = 'S1GS_CASH' or FundName = 'SAPCS_CASH' or FundName = 'S1CS_CASH' or FundName = 'SP29CS_CASH' or FundName = 'SP29DB_CASH' or FundName = 'SP29GS_CASH')">
							<xsl:value-of select="CurrencySymbol"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="'USD'"/>
						</xsl:otherwise>
					</xsl:choose>-->
					<!--<xsl:value-of select="''"/>-->
				</SettlementCurrency>

				<!--75-->
				<SettlementAmount>
					<xsl:value-of select="''"/>
					<!--<xsl:choose>
						<xsl:when test="CurrencySymbol != 'USD'">
							<xsl:value-of select='format-number((NetAmount * (1 div ForexRate)), "###.00")'/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select='format-number((NetAmount * 1), "###.00")'/>
						</xsl:otherwise>
					</xsl:choose>-->
					<!--<xsl:value-of select="''"/>-->
				</SettlementAmount>

				<!--76-->
				<SettlementTransactionConditionIndicator>
					<xsl:value-of select="''"/>
				</SettlementTransactionConditionIndicator>

				<!--77-->
				<CommissionType1>
					<xsl:value-of select="''"/>
				</CommissionType1>

				<!--78-->
				<CommissionAmount1>
					<xsl:value-of select="''"/>
				</CommissionAmount1>

				<!--79-->
				<CommissionType2>
					<xsl:value-of select="''"/>
				</CommissionType2>

				<!--80-->
				<CommissionAmount2>
					<xsl:value-of select="''"/>
				</CommissionAmount2>

				<!--81-->
				<CommissionType3>
					<xsl:value-of select="''"/>
				</CommissionType3>

				<!--82-->
				<CommissionAmount3>
					<xsl:value-of select="''"/>
				</CommissionAmount3>

				<!--83-->
				<TotalFeeAmount>
					<xsl:value-of select="''"/>
				</TotalFeeAmount>

				<!--84-->
				<ChargeTaxType1>
					<xsl:value-of select="''"/>
				</ChargeTaxType1>

				<!--85-->
				<ChargeAmount1>
					<xsl:value-of select="''"/>
				</ChargeAmount1>

				<!--86-->
				<ChargeTaxType2>
					<xsl:value-of select="''"/>
				</ChargeTaxType2>

				<!--87-->
				<ChargeAmount2>
					<xsl:value-of select="''"/>
				</ChargeAmount2>

				<!--88-->
				<ChargeTaxType3>
					<xsl:value-of select="''"/>
				</ChargeTaxType3>

				<!--89-->
				<ChargeAmount3>
					<xsl:value-of select="''"/>
				</ChargeAmount3>

				<!--90-->
				<ChargeTaxType4>
					<xsl:value-of select="''"/>
				</ChargeTaxType4>

				<!--91-->
				<ChargeAmount4>
					<xsl:value-of select="''"/>
				</ChargeAmount4>

				<!--92-->
				<ChargeTaxType5>
					<xsl:value-of select="''"/>
				</ChargeTaxType5>

				<!--93-->
				<ChargeAmount5>
					<xsl:value-of select="''"/>
				</ChargeAmount5>

				<!--94-->
				<CurrentFaceValue>
					<xsl:value-of select="''"/>
				</CurrentFaceValue>

				<!--95-->
				<SecurityTypeIndicator>
					<xsl:value-of select="''"/>
				</SecurityTypeIndicator>

				<!--96-->
				<ClearingBroker>
					<xsl:value-of select="''"/>
				</ClearingBroker>

				<!--97-->
				<TypeOfFinancialInstrument>
					<xsl:value-of select="''"/>
				</TypeOfFinancialInstrument>

				<!--98-->
				<OTCIndicator>
					<xsl:value-of select="''"/>
				</OTCIndicator>

				<!--99-->
				<OrderType>
					<xsl:value-of select="''"/>
				</OrderType>

				<!--100-->
				<TradeAgreementMethod>
					<xsl:value-of select="''"/>
				</TradeAgreementMethod>

				<!--101-->
				<AccountAtClearingBroker>
					<xsl:value-of select="''"/>
				</AccountAtClearingBroker>

				<!--102-->
				<PriceTypeCode>
					<xsl:value-of select="''"/>
				</PriceTypeCode>

				<!--Test-->
				<!--103-->
				<FuturesUnderlyingAsset>
					<xsl:value-of select="''"/>
				</FuturesUnderlyingAsset>

				<!--104-->
				<FuturesUnderlyingAssetDescription>
					<xsl:value-of select="''"/>
				</FuturesUnderlyingAssetDescription>

				<!--105-->
				<FuturesDeliveryType>
					<xsl:value-of select="''"/>
				</FuturesDeliveryType>

				<!--106-->
				<FuturesTickSize>
					<xsl:value-of select="''"/>
				</FuturesTickSize>

				<!--107-->
				<FuturesTickValue>
					<xsl:value-of select="''"/>
				</FuturesTickValue>

				<!--108-->
				<FuturesMultiLegReportingType>
					<xsl:value-of select="''"/>
				</FuturesMultiLegReportingType>

				<!--109-->
				<FuturesIntMarginPrTypCode>
					<xsl:value-of select="''"/>
				</FuturesIntMarginPrTypCode>

				<!--110-->
				<FuturesIntMarginAmnt>
					<xsl:value-of select="''"/>
				</FuturesIntMarginAmnt>

				<!--111-->
				<FuturesDeliveryDate>
					<xsl:value-of select="''"/>
				</FuturesDeliveryDate>

				<!--112-->
				<OptionsUnderlyingAsset>
					<xsl:value-of select="''"/>
				</OptionsUnderlyingAsset>

				<!--113-->
				<OptionsUnderlyingAssetDescription>
					<xsl:value-of select="''"/>
				</OptionsUnderlyingAssetDescription>

				<!--114-->
				<OptionsDeliveryType>
					<xsl:value-of select="''"/>
				</OptionsDeliveryType>

				<!--115-->
				<OptionsTickSize>
					<xsl:value-of select="''"/>
				</OptionsTickSize>

				<!--116-->
				<OptionsTickValue>
					<xsl:value-of select="''"/>
				</OptionsTickValue>

				<!--117-->
				<OptionsMultiLegReportingType>
					<xsl:value-of select="''"/>
				</OptionsMultiLegReportingType>

				<!--118-->
				<OptionsExpirationDate>
					<xsl:value-of select="''"/>
				</OptionsExpirationDate>

				<!--119-->
				<OptionsPutCallindicator>
					<xsl:value-of select="''"/>
				</OptionsPutCallindicator>

				<!--120-->
				<OptionStyle>
					<xsl:value-of select="''"/>
				</OptionStyle>

				<!--121-->
				<OptionsStrikePrice>
					<xsl:value-of select="''"/>
				</OptionsStrikePrice>

				<!--122-->
				<OptionsPremiumAmount>
					<xsl:value-of select="''"/>
				</OptionsPremiumAmount>

				<!--123-->
				<EqtSwapsTerminationDate>
					<xsl:value-of select="''"/>
				</EqtSwapsTerminationDate>

				<!--124-->
				<EqtSwapsEffectiveDate>
					<xsl:value-of select="''"/>
				</EqtSwapsEffectiveDate>

				<!--125-->
				<EqtSwapsOnMarginIndicator>
					<xsl:value-of select="''"/>
				</EqtSwapsOnMarginIndicator>

				<!--126-->
				<EqtSwapsUnderlyingAsset>
					<xsl:value-of select="''"/>
				</EqtSwapsUnderlyingAsset>

				<!--127-->
				<EqtSwapsUnderlyingAssetDescription>
					<xsl:value-of select="''"/>
				</EqtSwapsUnderlyingAssetDescription>

				<!--128-->
				<EqtSwapsIntMarginPrTypCode>
					<xsl:value-of select="''"/>
				</EqtSwapsIntMarginPrTypCode>

				<!--129-->
				<EqtSwapsIntMarginAmnt>
					<xsl:value-of select="''"/>
				</EqtSwapsIntMarginAmnt>

				<!--130-->
				<EqtSwapsReturnLegResetDate>
					<xsl:value-of select="''"/>
				</EqtSwapsReturnLegResetDate>

				<!--131-->
				<EqtSwapsReturnLegPaymentDate>
					<xsl:value-of select="''"/>
				</EqtSwapsReturnLegPaymentDate>

				<!--132-->
				<EqtSwapsReturnLegNotionalAmount>
					<xsl:value-of select="''"/>
				</EqtSwapsReturnLegNotionalAmount>

				<!--133-->
				<EqtSwapsReturnLegInitialPrice>
					<xsl:value-of select="''"/>
				</EqtSwapsReturnLegInitialPrice>

				<!--134-->
				<EqtSwapsReturnLegTypeofReturn>
					<xsl:value-of select="''"/>
				</EqtSwapsReturnLegTypeofReturn>

				<!--135-->
				<EqtSwapsReturnLegDividendPrTypCode>
					<xsl:value-of select="''"/>
				</EqtSwapsReturnLegDividendPrTypCode>

				<!--136-->
				<EqtSwapsReturnLegDividendAmnt>
					<xsl:value-of select="''"/>
				</EqtSwapsReturnLegDividendAmnt>

				<!--137-->
				<EqtSwapsInterestLegResetDate>
					<xsl:value-of select="''"/>
				</EqtSwapsInterestLegResetDate>

				<!--138-->
				<EqtSwapsInterestLegPaymentDate>
					<xsl:value-of select="''"/>
				</EqtSwapsInterestLegPaymentDate>

				<!--139-->
				<EqtSwapsInterestLegNotionalAmount>
					<xsl:value-of select="''"/>
				</EqtSwapsInterestLegNotionalAmount>

				<!--140-->
				<EqtSwapsInterestLegBenchmarkType>
					<xsl:value-of select="''"/>
				</EqtSwapsInterestLegBenchmarkType>

				<!--141-->
				<EqtSwapsInterestLegBenchmarkRate>
					<xsl:value-of select="''"/>
				</EqtSwapsInterestLegBenchmarkRate>

				<!--142-->
				<EqtSwapsInterestLegDayCountBasis>
					<xsl:value-of select="''"/>
				</EqtSwapsInterestLegDayCountBasis>

				<!--143-->
				<EqtSwapsInterestLegSpread>
					<xsl:value-of select="''"/>
				</EqtSwapsInterestLegSpread>

				<!--144-->
				<EqtSwapsInterestLegIndexTenor>
					<xsl:value-of select="''"/>
				</EqtSwapsInterestLegIndexTenor>

				<!--145-->
				<EqtSwapsInterestLegFixedRate>
					<xsl:value-of select="''"/>
				</EqtSwapsInterestLegFixedRate>
				<!--Test-->

				<!--146-->
				<OmniExpected>
					<xsl:value-of select="''"/>
				</OmniExpected>

				<!--147-->
				<TradeDetailReferenceType>
					<xsl:value-of select="''"/>
				</TradeDetailReferenceType>

				<!--148-->
				<TradeDetailReferenceValue>
					<xsl:value-of select="''"/>
				</TradeDetailReferenceValue>

				<!--149-->
				<IMCptySSISource>
					<xsl:value-of select="''"/>
				</IMCptySSISource>

				<!--150-->
				<IMCptyAlertCountry>
					<xsl:value-of select="''"/>
				</IMCptyAlertCountry>

				<!--151-->
				<IMCptyAlertSecurity>
					<xsl:value-of select="''"/>
				</IMCptyAlertSecurity>

				<!--152-->
				<IMCptyAlertClearMethod>
					<xsl:value-of select="''"/>
				</IMCptyAlertClearMethod>

				<!--153-->
				<IMCptyAlertSettlementModelName>
					<xsl:value-of select="''"/>
				</IMCptyAlertSettlementModelName>

				<!--154-->
				<IMCptyAlDelivText>
					<xsl:value-of select="''"/>
				</IMCptyAlDelivText>

				<!--155-->
				<ClearingBrkPartyType>
					<xsl:value-of select="''"/>
				</ClearingBrkPartyType>

				<!--156-->
				<xsl:variable name="varFundName">
					<xsl:value-of select="FundName"/>
				</xsl:variable>

				<xsl:variable name="varPB">
					<xsl:value-of select="document('../ReconMappingXml/Omgeo_BrokerMatch.xml')/BrokerMapping/PB[@Name='SENSATO']/BrokerData[@PranaFundName = $varFundName]/@PB"/>
				</xsl:variable>

				<xsl:variable name="varBIC">
					<xsl:value-of select="document('../ReconMappingXml/BIC_Omgeo.xml')/BICMapping/PB[@Name='SENSATO']/BrokerData[@PBName = $varPB]/@BIC"/>
				</xsl:variable>

				<ThirdPartyToTrade>


						<xsl:value-of select="''"/>
					
				</ThirdPartyToTrade>

				<!--157-->
				<PartyType>

					<xsl:value-of select="''"/>
				</PartyType>

				<!--158-->
				<PlaceOfClearing>
					<xsl:value-of select="''"/>
				</PlaceOfClearing>

				<!-- for system use only-->
				<EntityID>
					<xsl:value-of select ="EntityID"/>
				</EntityID>


			</ThirdPartyFlatFileDetail>
		</xsl:for-each>

	</xsl:template>
</xsl:stylesheet>