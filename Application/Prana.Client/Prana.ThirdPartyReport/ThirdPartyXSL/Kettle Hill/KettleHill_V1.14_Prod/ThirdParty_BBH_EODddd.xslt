<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="DateFormat">
		<xsl:param name="Date"/>
		<xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
	</xsl:template>

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			
			
			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>
				<TaxLotState>
					<xsl:value-of select="'Allocated'"/>
				</TaxLotState>
				
				<FunctionofInstruction>
					<xsl:value-of select="'Function of Instruction'"/>
				</FunctionofInstruction>
				<ClientReferenceNumber>
					<xsl:value-of select="'Client Reference Number'"/>
				</ClientReferenceNumber>
				<PreviousReferenceNumber>
					<xsl:value-of select="'Previous Reference Number'"/>
				</PreviousReferenceNumber>
				
				<AccountNumber>
					<xsl:value-of select="'Account Number'"/>
				</AccountNumber>
				<TransactionType>
					<xsl:value-of select="'Transaction Type'"/>
				</TransactionType>
				<PlaceofSettlementCountry>
					<xsl:value-of select="'Place of Settlement/Country'"/>
				</PlaceofSettlementCountry>
				<PlaceofSafekeeping>
					<xsl:value-of select="'Place of Safekeeping'"/>
				</PlaceofSafekeeping>
				
				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>
				<SettlementDate>
					<xsl:value-of select="'Settlement Date'"/>
				</SettlementDate>
				<SecurityID>
					<xsl:value-of select="'Security ID'"/>
				</SecurityID>
				<SecurityDescription>
					<xsl:value-of select="'Security Description'"/>
				</SecurityDescription>
				<UnitOriginalFaceAmount>
					<xsl:value-of select="'Unit / Original Face Amount'"/>
				</UnitOriginalFaceAmount>
				<Currency>
					<xsl:value-of select="'Currency'"/>
				</Currency>
				<UnitPriceAmount>
					<xsl:value-of select="'Unit Price Amount'"/>
				</UnitPriceAmount>
				<NetAmount>					
					<xsl:value-of select="'Net Amount'"/>					
				</NetAmount>
				
				<TradingBrokerTypeID>
					<xsl:value-of select="'Trading Broker Type/ID'"/>
				</TradingBrokerTypeID>
				
				<TradingBrokerDescription>
					<xsl:value-of select="'Trading Broker Description'"/>
				</TradingBrokerDescription>
				<BeneficiaryofSecuritiesAccount>
					<xsl:value-of select="'Beneficiary of Securities Account'"/>
				</BeneficiaryofSecuritiesAccount>
				
				<ClearingBrokerIDType>
					<xsl:value-of select="'Clearing Broker ID / Type'"/>
				</ClearingBrokerIDType>
				<ClearingBrokerDescription>
					<xsl:value-of select="'Clearing Broker Description'"/>
				</ClearingBrokerDescription>
				<ClearingAgentAccount>
					<xsl:value-of select="'Clearing Agent Account'"/>
				</ClearingAgentAccount>
				<StampDutyCode>
					<xsl:value-of select="'Stamp Duty Code'"/>
				</StampDutyCode>
				<StampDutyAmount>
					<xsl:value-of select="'Stamp Duty Amount'"/>
				</StampDutyAmount>
				<SpecialSettlementType>
					<xsl:value-of select="'Special Settlement Type'"/>
				</SpecialSettlementType>
				<SpecialIndicator1>
					<xsl:value-of select="'Special Indicator #1'"/>
				</SpecialIndicator1>
				<SpecialIndicator2>
					<xsl:value-of select="'Special Indicator #2'"/>
				</SpecialIndicator2>
				<RegistrationDetails>
					<xsl:value-of select="'Registration Details'"/>
				</RegistrationDetails>
				<SpecialInstruction>
					<xsl:value-of select="'Special Instruction'"/>
				</SpecialInstruction>
				<OriginatorofMessage>
					<xsl:value-of select="'Originator of Message'"/>
				</OriginatorofMessage>
				<CurrentFaceAmortizeValue>
					<xsl:value-of select="'Current Face/Amortize Value'"/>
				</CurrentFaceAmortizeValue>
				<PrincipalAmount>
					<xsl:value-of select="'Principal Amount'"/>
				</PrincipalAmount>
				<InterestAmount>
					<xsl:value-of select="'Interest Amount'"/>
				</InterestAmount>
				
				<OtherFeesAmount>					
					<xsl:value-of select="'Other Fees Amount'"/>
					
				</OtherFeesAmount>
			
				<CommissionAmount>
					<xsl:value-of select="'Commission Amount'"/>
				</CommissionAmount>
				<SECFeesAmount>
					<xsl:value-of select="'SEC Fees Amount'"/>
				</SECFeesAmount>
				<TransactionTaxAmount>
					<xsl:value-of select="'Transaction Tax Amount'"/>
				</TransactionTaxAmount>
				<WithholdingTaxAmount>
					<xsl:value-of select="'Withholding Tax Amount'"/>
				</WithholdingTaxAmount>
				<ExchangeRate>
					<xsl:value-of select="'Exchange Rate'"/>
				</ExchangeRate>
				<ResultingCurrency>
					<xsl:value-of select="'Resulting Currency'"/>
				</ResultingCurrency>
				<ResultingAmount>
					<xsl:value-of select="'Resulting Amount'"/>
				</ResultingAmount>
				<FXCurrency>
					<xsl:value-of select="'FX Currency'"/>
				</FXCurrency>
				<PoolReferenceNumber>
					<xsl:value-of select="'Pool Reference Number'"/>
				</PoolReferenceNumber>
				<TotalGroupNumber>
					<xsl:value-of select="'Total Group Number'"/>
				</TotalGroupNumber>
				<TradeNumber>
					<xsl:value-of select="'Trade Number'"/>
				</TradeNumber>
				<RepoTermDateREPOonly>
					<xsl:value-of select="'Repo Term Date (REPO only)'"/>
				</RepoTermDateREPOonly>
				<RepoAmountREPOonly>
					<xsl:value-of select="'Repo Amount (REPO only)'"/>
				</RepoAmountREPOonly>
				<RepoReferenceNumberREPOonly>
					<xsl:value-of select="'Repo Reference Number (REPO only)'"/>
				</RepoReferenceNumberREPOonly>
				<RepoRateREPOOnly>
					<xsl:value-of select="'Repo Rate (REPO Only)'"/>
				</RepoRateREPOOnly>
				<TickerCPFandCRFOnly>
					<xsl:value-of select="'Ticker (CPF and CRF Only)'"/>
				</TickerCPFandCRFOnly>
				<StrikePriceCPFandCRFOny>
					<xsl:value-of select="'Strike Price (CPF and CRFOny)'"/>
				</StrikePriceCPFandCRFOny>
				<ExpirationDateCPFandCRFOnly>
					<xsl:value-of select="'Expiration Date (CPF and CRF Only)'"/>
				</ExpirationDateCPFandCRFOnly>
				<BrokerNumberCPFandCRFOnly>
					<xsl:value-of select="'Broker Number (CPF and CRF Only)'"/>
				</BrokerNumberCPFandCRFOnly>
				<BrokerAccountCPFandCRFOnly>
					<xsl:value-of select="'Broker Account (CPF and CRF Only)'"/>
				</BrokerAccountCPFandCRFOnly>
				<ContractSizeOptionContractandFutureContractOnly>
					<xsl:value-of select="'Contract Size (Option Contract and Future Contract Only)'"/>
				</ContractSizeOptionContractandFutureContractOnly>
				<PlaceofTradeNarrative>
					<xsl:value-of select="'Place of Trade Narrative'"/>
				</PlaceofTradeNarrative>
				<CommonReference>
					<xsl:value-of select="'Common Reference'"/>
				</CommonReference>
				<PartialSettlementAllowed>
					<xsl:value-of select="'Partial Settlement Allowed'"/>
				</PartialSettlementAllowed>
				<PartialSettlementTolerance>
					<xsl:value-of select="'Partial Settlement Tolerance'"/>
				</PartialSettlementTolerance>
				<NoAutomaticMarketClaim>
					<xsl:value-of select="'No Automatic Market Claim'"/>
				</NoAutomaticMarketClaim>
				<CorporateActionCouponOption>
					<xsl:value-of select="'Corporate Action Coupon Option'"/>
				</CorporateActionCouponOption>
				<TripartyCollateralSegregation>
					<xsl:value-of select="'Triparty Collateral Segregation'"/>
				</TripartyCollateralSegregation>
				<FXCancelForCANCinstructionsonly>
					<xsl:value-of select="'FX Cancel For - CANC instructions only'"/>
				</FXCancelForCANCinstructionsonly>
				<FundAccountingOnlyTradeRPTO>
					<xsl:value-of select="'Fund Accounting Only Trade (RPTO)'"/>
				</FundAccountingOnlyTradeRPTO>
				<CustodyOnlyTradeNACT>
					<xsl:value-of select="'Custody Only Trade (NACT)'"/>
				</CustodyOnlyTradeNACT>

				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>
			
			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<xsl:variable name="varNetamount">
					<xsl:choose>
						<xsl:when test="contains(Side,'Buy')">
							<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
						</xsl:when>
						<xsl:when test="contains(Side,'Sell')">
							<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test ="TaxLotState!='Amended'">
						<ThirdPartyFlatFileDetail>

							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<FunctionofInstruction>
								<xsl:choose>
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'NEWM'"/>
									</xsl:when>

									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'CANC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="'SENT'"/>
									</xsl:otherwise>
								</xsl:choose>
							</FunctionofInstruction>

							<ClientReferenceNumber>
								<xsl:value-of select="EntityID"/>
							</ClientReferenceNumber>

							<PreviousReferenceNumber>
								<xsl:value-of select="''"/>
							</PreviousReferenceNumber>
							<xsl:variable name="PB_NAME" select="''"/>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="'CFFW'"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>


							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<AccountNumber>
								<xsl:value-of select="$varAccountName"/>
							</AccountNumber>

							<TransactionType>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'RVP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'DVP'"/>
									</xsl:when>
								</xsl:choose>								
							</TransactionType>

							<PlaceofSettlementCountry>
								<xsl:value-of select="''"/>
							</PlaceofSettlementCountry>

							<PlaceofSafekeeping>
								<xsl:value-of select="''"/>
							</PlaceofSafekeeping>


							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>

							<TradeDate>
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</TradeDate>


							<SettlementDate>
								<xsl:value-of select="concat(substring-before($SettlementDate,'/'),'/',substring-before(substring-after($SettlementDate,'/'),'/'),'/',substring-after(substring-after($SettlementDate,'/'),'/'))"/>
							</SettlementDate>
							

							<SecurityID>
								<xsl:value-of select="Symbol"/>
							</SecurityID>

							<SecurityDescription>
								<xsl:value-of select="CompanyName"/>
							</SecurityDescription>

							<UnitOriginalFaceAmount>
								<xsl:choose>
									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="OrderQty"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</UnitOriginalFaceAmount>
												

							<Currency>
								<xsl:value-of select="CurrencySymbol"/>
							</Currency>

							<UnitPriceAmount>
								<xsl:choose>
									<xsl:when test="number(AvgPrice)">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</UnitPriceAmount>

							<NetAmount>
								<xsl:choose>
									<xsl:when test="number($varNetamount)">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetAmount>

							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<TradingBrokerTypeID>								
										<xsl:choose>
											<xsl:when test="$THIRDPARTY_BROKER!= ''">
												<xsl:value-of select="$THIRDPARTY_BROKER"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
											</xsl:otherwise>
										</xsl:choose>									
							</TradingBrokerTypeID>

						
							<TradingBrokerDescription>
								<xsl:value-of select="''"/>
							</TradingBrokerDescription>

							<BeneficiaryofSecuritiesAccount>
								<xsl:value-of select="''"/>
							</BeneficiaryofSecuritiesAccount>

							<xsl:variable name="THIRDPARTY_COUNTERPARTY">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
							</xsl:variable>
							<xsl:variable name="BrokerName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_COUNTERPARTY!=''">
										<xsl:value-of select="$THIRDPARTY_COUNTERPARTY"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ClearingBrokerIDType>
								<xsl:value-of select="CounterPartyID"/>
							</ClearingBrokerIDType>

							<ClearingBrokerDescription>
								<xsl:value-of select="''"/>
							</ClearingBrokerDescription>

							<ClearingAgentAccount>
								<xsl:value-of select="''"/>
							</ClearingAgentAccount>


							<StampDutyCode>
								<xsl:value-of select="''"/>
							</StampDutyCode>

							<StampDutyAmount>
								<xsl:value-of select="''"/>
							</StampDutyAmount>

							<SpecialSettlementType>
								<xsl:value-of select="''"/>
							</SpecialSettlementType>

							<SpecialIndicator1>
								<xsl:value-of select="''"/>
							</SpecialIndicator1>

							<SpecialIndicator2>
								<xsl:value-of select="''"/>
							</SpecialIndicator2>


							<RegistrationDetails>
								<xsl:value-of select="''"/>
							</RegistrationDetails>

							<SpecialInstruction>
								<xsl:value-of select="''"/>
							</SpecialInstruction>

							<OriginatorofMessage>
								<xsl:value-of select="''"/>
							</OriginatorofMessage>

							<CurrentFaceAmortizeValue>
								<xsl:value-of select="''"/>
							</CurrentFaceAmortizeValue>
							<xsl:variable name="Principal" select="OrderQty * AvgPrice * AssetMultiplier"/>
							<PrincipalAmount>
								<xsl:value-of select="''"/>
							</PrincipalAmount>

							<InterestAmount>
								<xsl:value-of select="''"/>
							</InterestAmount>

							<xsl:variable name="OtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
							</xsl:variable>
							<OtherFeesAmount>
								<xsl:choose>
									<xsl:when test="number($OtherFees)">
										<xsl:value-of select="format-number($OtherFees,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OtherFeesAmount>
							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>
							<CommissionAmount>
								<xsl:value-of select="''"/>
							</CommissionAmount>

							<SECFeesAmount>
								<xsl:value-of select="''"/>
							</SECFeesAmount>


							<TransactionTaxAmount>
								<xsl:value-of select="''"/>
							</TransactionTaxAmount>

							<WithholdingTaxAmount>
								<xsl:value-of select="''"/>
							</WithholdingTaxAmount>

							<ExchangeRate>
								<xsl:value-of select="''"/>
							</ExchangeRate>

							<ResultingCurrency>
								<xsl:value-of select="''"/>
							</ResultingCurrency>

							<ResultingAmount>
								<xsl:value-of select="''"/>
							</ResultingAmount>
						

							<FXCurrency>
								<xsl:value-of select="''"/>
							</FXCurrency>

							<PoolReferenceNumber>
								<xsl:value-of select="''"/>
							</PoolReferenceNumber>

							<TotalGroupNumber>
								<xsl:value-of select="''"/>
							</TotalGroupNumber>


							<TradeNumber>
								<xsl:value-of select="''"/>
							</TradeNumber>

							<RepoTermDateREPOonly>
								<xsl:value-of select="''"/>
							</RepoTermDateREPOonly>

							<RepoAmountREPOonly>
								<xsl:value-of select="''"/>
							</RepoAmountREPOonly>


							<RepoReferenceNumberREPOonly>
								<xsl:value-of select="''"/>
							</RepoReferenceNumberREPOonly>

							<RepoRateREPOOnly>
								<xsl:value-of select="''"/>
							</RepoRateREPOOnly>
							
							<TickerCPFandCRFOnly>
								<xsl:value-of select="''"/>
							</TickerCPFandCRFOnly>

							<StrikePriceCPFandCRFOny>
								<xsl:value-of select="''"/>
							</StrikePriceCPFandCRFOny>

							<ExpirationDateCPFandCRFOnly>
								<xsl:value-of select="''"/>
							</ExpirationDateCPFandCRFOnly>

							<BrokerNumberCPFandCRFOnly>
								<xsl:value-of select="''"/>
							</BrokerNumberCPFandCRFOnly>

							<BrokerAccountCPFandCRFOnly>
								<xsl:value-of select="''"/>
							</BrokerAccountCPFandCRFOnly>

							<ContractSizeOptionContractandFutureContractOnly>
								<xsl:value-of select="''"/>
							</ContractSizeOptionContractandFutureContractOnly>

							<PlaceofTradeNarrative>
								<xsl:value-of select="''"/>
							</PlaceofTradeNarrative>

							<CommonReference>
								<xsl:value-of select="''"/>
							</CommonReference>

							<PartialSettlementAllowed>
								<xsl:value-of select="''"/>
							</PartialSettlementAllowed>

							<PartialSettlementTolerance>
								<xsl:value-of select="''"/>
							</PartialSettlementTolerance>

							<NoAutomaticMarketClaim>
								<xsl:value-of select="''"/>
							</NoAutomaticMarketClaim>

							<CorporateActionCouponOption>
								<xsl:value-of select="''"/>
							</CorporateActionCouponOption>

							<TripartyCollateralSegregation>
								<xsl:value-of select="''"/>
							</TripartyCollateralSegregation>

							<FXCancelForCANCinstructionsonly>
								<xsl:value-of select="''"/>
							</FXCancelForCANCinstructionsonly>

							<FundAccountingOnlyTradeRPTO>
								<xsl:value-of select="''"/>
							</FundAccountingOnlyTradeRPTO>

							<CustodyOnlyTradeNACT>
								<xsl:value-of select="''"/>
							</CustodyOnlyTradeNACT>

							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>
						</ThirdPartyFlatFileDetail>
					</xsl:when>

					<xsl:otherwise>
						<xsl:if test ="number(OldExecutedQuantity)">
							<ThirdPartyFlatFileDetail>

								<RowHeader>
									<xsl:value-of select ="'false'"/>
								</RowHeader>

								<TaxLotState>
									<xsl:value-of select="'Deleted'"/>
								</TaxLotState>
							

								<FunctionofInstruction>									
											<xsl:value-of select ="'CANC'"/>									
								</FunctionofInstruction>

								<ClientReferenceNumber>
									<xsl:value-of select="concat(substring(EntityID,2,15),'E')"/>
								</ClientReferenceNumber>

								<PreviousReferenceNumber>
									<xsl:value-of select="''"/>
								</PreviousReferenceNumber>
								<xsl:variable name="PB_NAME" select="''"/>

								<xsl:variable name = "PRANA_FUND_NAME">
									<xsl:value-of select="AccountName"/>
								</xsl:variable>

								<xsl:variable name ="THIRDPARTY_FUND_CODE">
									<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
								</xsl:variable>


								<xsl:variable name="varAccountName">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
											<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_FUND_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<AccountNumber>
									<xsl:value-of select="$varAccountName"/>
								</AccountNumber>

								<TransactionType>
									<xsl:choose>
										<xsl:when test="OldSide='Buy'">
											<xsl:value-of select="'RVP'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell'">
											<xsl:value-of select="'DVP'"/>
										</xsl:when>
									</xsl:choose>
								</TransactionType>

								<PlaceofSettlementCountry>
									<xsl:value-of select="''"/>
								</PlaceofSettlementCountry>

								<PlaceofSafekeeping>
									<xsl:value-of select="''"/>
								</PlaceofSafekeeping>

								<xsl:variable name="varOldTradeDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldTradeDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>
								<xsl:variable name="varOldSettleDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldSettlementDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>
								<TradeDate>
									<xsl:value-of select="concat(substring-before($varOldTradeDate,'/'),'/',substring-before(substring-after($varOldTradeDate,'/'),'/'),'/',substring-after(substring-after($varOldTradeDate,'/'),'/'))"/>
								</TradeDate>
								<SettlementDate>
									<xsl:value-of select="concat(substring-before($varOldSettleDate,'/'),'/',substring-before(substring-after($varOldSettleDate,'/'),'/'),'/',substring-after(substring-after($varOldSettleDate,'/'),'/'))"/>
								</SettlementDate>
								<SecurityID>
									<xsl:value-of select="Symbol"/>
								</SecurityID>
								<SecurityDescription>
									<xsl:value-of select="CompanyName"/>
								</SecurityDescription>
								<UnitOriginalFaceAmount>
									<xsl:choose>
										<xsl:when test="number(OldExecutedQuantity)">
											<xsl:value-of select="OldExecutedQuantity"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</UnitOriginalFaceAmount>
								<Currency>
									<xsl:value-of select="CurrencySymbol"/>
								</Currency>
								<UnitPriceAmount>
									<xsl:choose>
										<xsl:when test="number(OldAvgPrice)">
											<xsl:value-of select="format-number(OldAvgPrice,'0.######')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</UnitPriceAmount>
								<xsl:variable name="varOldNetAmount">
									<xsl:choose>
										<xsl:when test="contains(Side,'Buy')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
										</xsl:when>
										<xsl:when test="contains(Side,'Sell')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
										</xsl:when>
									</xsl:choose>
								</xsl:variable>
								<NetAmount>
									<xsl:choose>
										<xsl:when test="number($varOldNetAmount)">
											<xsl:value-of select="$varOldNetAmount"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</NetAmount>
								<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
								<xsl:variable name="THIRDPARTY_BROKER">
									<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
								</xsl:variable>
								<TradingBrokerTypeID>
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_BROKER!= ''">
											<xsl:value-of select="$THIRDPARTY_BROKER"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</TradingBrokerTypeID>
								<TradingBrokerDescription>
									<xsl:value-of select="''"/>
								</TradingBrokerDescription>
								<BeneficiaryofSecuritiesAccount>
									<xsl:value-of select="''"/>
								</BeneficiaryofSecuritiesAccount>
								<xsl:variable name="THIRDPARTY_COUNTERPARTY">
									<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
								</xsl:variable>
								<xsl:variable name="BrokerName">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_COUNTERPARTY!=''">
											<xsl:value-of select="$THIRDPARTY_COUNTERPARTY"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<ClearingBrokerIDType>
									<xsl:value-of select="CounterPartyID"/>
								</ClearingBrokerIDType>
								<ClearingBrokerDescription>
									<xsl:value-of select="''"/>
								</ClearingBrokerDescription>
								<ClearingAgentAccount>
									<xsl:value-of select="''"/>
								</ClearingAgentAccount>
								<StampDutyCode>
									<xsl:value-of select="''"/>
								</StampDutyCode>
								<StampDutyAmount>
									<xsl:value-of select="''"/>
								</StampDutyAmount>
								<SpecialSettlementType>
									<xsl:value-of select="''"/>
								</SpecialSettlementType>
								<SpecialIndicator1>
									<xsl:value-of select="''"/>
								</SpecialIndicator1>
								<SpecialIndicator2>
									<xsl:value-of select="''"/>
								</SpecialIndicator2>
								<RegistrationDetails>
									<xsl:value-of select="''"/>
								</RegistrationDetails>
								<SpecialInstruction>
									<xsl:value-of select="''"/>
								</SpecialInstruction>
								<OriginatorofMessage>
									<xsl:value-of select="''"/>
								</OriginatorofMessage>
								<CurrentFaceAmortizeValue>
									<xsl:value-of select="''"/>
								</CurrentFaceAmortizeValue>						
								<PrincipalAmount>
									<xsl:value-of select="''"/>
								</PrincipalAmount>
								<InterestAmount>
									<xsl:value-of select="''"/>
								</InterestAmount>
								<xsl:variable name="varOldOtherFees">
									<xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + TaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
								</xsl:variable>
								<OtherFeesAmount>
									<xsl:choose>
										<xsl:when test="number($varOldOtherFees)">
											<xsl:value-of select="format-number($varOldOtherFees,'0.##')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</OtherFeesAmount>								
								<CommissionAmount>
									<xsl:value-of select="''"/>
								</CommissionAmount>
								<SECFeesAmount>
									<xsl:value-of select="''"/>
								</SECFeesAmount>
								<TransactionTaxAmount>
									<xsl:value-of select="''"/>
								</TransactionTaxAmount>
								<WithholdingTaxAmount>
									<xsl:value-of select="''"/>
								</WithholdingTaxAmount>
								<ExchangeRate>
									<xsl:value-of select="''"/>
								</ExchangeRate>
								<ResultingCurrency>
									<xsl:value-of select="''"/>
								</ResultingCurrency>
								<ResultingAmount>
									<xsl:value-of select="''"/>
								</ResultingAmount>
								<FXCurrency>
									<xsl:value-of select="''"/>
								</FXCurrency>
								<PoolReferenceNumber>
									<xsl:value-of select="''"/>
								</PoolReferenceNumber>
								<TotalGroupNumber>
									<xsl:value-of select="''"/>
								</TotalGroupNumber>
								<TradeNumber>
									<xsl:value-of select="''"/>
								</TradeNumber>
								<RepoTermDateREPOonly>
									<xsl:value-of select="''"/>
								</RepoTermDateREPOonly>
								<RepoAmountREPOonly>
									<xsl:value-of select="''"/>
								</RepoAmountREPOonly>
								<RepoReferenceNumberREPOonly>
									<xsl:value-of select="''"/>
								</RepoReferenceNumberREPOonly>
								<RepoRateREPOOnly>
									<xsl:value-of select="''"/>
								</RepoRateREPOOnly>
								<TickerCPFandCRFOnly>
									<xsl:value-of select="''"/>
								</TickerCPFandCRFOnly>
								<StrikePriceCPFandCRFOny>
									<xsl:value-of select="''"/>
								</StrikePriceCPFandCRFOny>
								<ExpirationDateCPFandCRFOnly>
									<xsl:value-of select="''"/>
								</ExpirationDateCPFandCRFOnly>
								<BrokerNumberCPFandCRFOnly>
									<xsl:value-of select="''"/>
								</BrokerNumberCPFandCRFOnly>
								<BrokerAccountCPFandCRFOnly>
									<xsl:value-of select="''"/>
								</BrokerAccountCPFandCRFOnly>
								<ContractSizeOptionContractandFutureContractOnly>
									<xsl:value-of select="''"/>
								</ContractSizeOptionContractandFutureContractOnly>
								<PlaceofTradeNarrative>
									<xsl:value-of select="''"/>
								</PlaceofTradeNarrative>
								<CommonReference>
									<xsl:value-of select="''"/>
								</CommonReference>
								<PartialSettlementAllowed>
									<xsl:value-of select="''"/>
								</PartialSettlementAllowed>
								<PartialSettlementTolerance>
									<xsl:value-of select="''"/>
								</PartialSettlementTolerance>
								<NoAutomaticMarketClaim>
									<xsl:value-of select="''"/>
								</NoAutomaticMarketClaim>
								<CorporateActionCouponOption>
									<xsl:value-of select="''"/>
								</CorporateActionCouponOption>
								<TripartyCollateralSegregation>
									<xsl:value-of select="''"/>
								</TripartyCollateralSegregation>
								<FXCancelForCANCinstructionsonly>
									<xsl:value-of select="''"/>
								</FXCancelForCANCinstructionsonly>
								<FundAccountingOnlyTradeRPTO>
									<xsl:value-of select="''"/>
								</FundAccountingOnlyTradeRPTO>
								<CustodyOnlyTradeNACT>
									<xsl:value-of select="''"/>
								</CustodyOnlyTradeNACT>																											
								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>
						<ThirdPartyFlatFileDetail>
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>
							<TaxLotState>
								<xsl:value-of select="'Allocated'"/>
							</TaxLotState>
							<FunctionofInstruction>
								<xsl:value-of select="'NEWM'"/>
							</FunctionofInstruction>
							<ClientReferenceNumber>
								<xsl:value-of select="EntityID"/>
							</ClientReferenceNumber>
							<PreviousReferenceNumber>
								<xsl:value-of select="''"/>
							</PreviousReferenceNumber>
							<xsl:variable name="PB_NAME" select="''"/>
							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="'CFFW'"/>
							</xsl:variable>
							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>

							<xsl:variable name="varAccountName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
										<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<AccountNumber>
								<xsl:value-of select="$varAccountName"/>
							</AccountNumber>
							<TransactionType>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'RVP'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'DVP'"/>
									</xsl:when>
								</xsl:choose>
							</TransactionType>
							<PlaceofSettlementCountry>
								<xsl:value-of select="''"/>
							</PlaceofSettlementCountry>
							<PlaceofSafekeeping>
								<xsl:value-of select="''"/>
							</PlaceofSafekeeping>

							<xsl:variable name="TradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<xsl:variable name="SettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<TradeDate>
								<xsl:value-of select="concat(substring-before($TradeDate,'/'),'/',substring-before(substring-after($TradeDate,'/'),'/'),'/',substring-after(substring-after($TradeDate,'/'),'/'))"/>
							</TradeDate>
							<SettlementDate>
								<xsl:value-of select="concat(substring-before($SettlementDate,'/'),'/',substring-before(substring-after($SettlementDate,'/'),'/'),'/',substring-after(substring-after($SettlementDate,'/'),'/'))"/>
							</SettlementDate>
							<SecurityID>
								<xsl:value-of select="Symbol"/>
							</SecurityID>
							<SecurityDescription>
								<xsl:value-of select="CompanyName"/>
							</SecurityDescription>
							<UnitOriginalFaceAmount>
								<xsl:choose>
									<xsl:when test="number(OrderQty)">
										<xsl:value-of select="OrderQty"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</UnitOriginalFaceAmount>
							<Currency>
								<xsl:value-of select="CurrencySymbol"/>
							</Currency>
							<UnitPriceAmount>
								<xsl:choose>
									<xsl:when test="number(AvgPrice)">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</UnitPriceAmount>
							<NetAmount>
								<xsl:choose>
									<xsl:when test="number($varNetamount)">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetAmount>
							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<TradingBrokerTypeID>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER!= ''">
										<xsl:value-of select="$THIRDPARTY_BROKER"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</TradingBrokerTypeID>
							<TradingBrokerDescription>
								<xsl:value-of select="''"/>
							</TradingBrokerDescription>
							<BeneficiaryofSecuritiesAccount>
								<xsl:value-of select="''"/>
							</BeneficiaryofSecuritiesAccount>

							<xsl:variable name="THIRDPARTY_COUNTERPARTY">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
							</xsl:variable>
							<xsl:variable name="BrokerName">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_COUNTERPARTY!=''">
										<xsl:value-of select="$THIRDPARTY_COUNTERPARTY"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ClearingBrokerIDType>
								<xsl:value-of select="CounterPartyID"/>
							</ClearingBrokerIDType>
							<ClearingBrokerDescription>
								<xsl:value-of select="''"/>
							</ClearingBrokerDescription>
							<ClearingAgentAccount>
								<xsl:value-of select="''"/>
							</ClearingAgentAccount>
							<StampDutyCode>
								<xsl:value-of select="''"/>
							</StampDutyCode>
							<StampDutyAmount>
								<xsl:value-of select="''"/>
							</StampDutyAmount>
							<SpecialSettlementType>
								<xsl:value-of select="''"/>
							</SpecialSettlementType>
							<SpecialIndicator1>
								<xsl:value-of select="''"/>
							</SpecialIndicator1>
							<SpecialIndicator2>
								<xsl:value-of select="''"/>
							</SpecialIndicator2>
							<RegistrationDetails>
								<xsl:value-of select="''"/>
							</RegistrationDetails>
							<SpecialInstruction>
								<xsl:value-of select="''"/>
							</SpecialInstruction>
							<OriginatorofMessage>
								<xsl:value-of select="''"/>
							</OriginatorofMessage>
							<CurrentFaceAmortizeValue>
								<xsl:value-of select="''"/>
							</CurrentFaceAmortizeValue>							
							<PrincipalAmount>
								<xsl:value-of select="''"/>
							</PrincipalAmount>
							<InterestAmount>
								<xsl:value-of select="''"/>
							</InterestAmount>
							<xsl:variable name="OtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
							</xsl:variable>
							<OtherFeesAmount>
								<xsl:choose>
									<xsl:when test="number($OtherFees)">
										<xsl:value-of select="format-number($OtherFees,'0.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OtherFeesAmount>
							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>
							<CommissionAmount>
								<xsl:value-of select="''"/>
							</CommissionAmount>
							<SECFeesAmount>
								<xsl:value-of select="''"/>
							</SECFeesAmount>
							<TransactionTaxAmount>
								<xsl:value-of select="''"/>
							</TransactionTaxAmount>
							<WithholdingTaxAmount>
								<xsl:value-of select="''"/>
							</WithholdingTaxAmount>
							<ExchangeRate>
								<xsl:value-of select="''"/>
							</ExchangeRate>
							<ResultingCurrency>
								<xsl:value-of select="''"/>
							</ResultingCurrency>
							<ResultingAmount>
								<xsl:value-of select="''"/>
							</ResultingAmount>
							<FXCurrency>
								<xsl:value-of select="''"/>
							</FXCurrency>
							<PoolReferenceNumber>
								<xsl:value-of select="''"/>
							</PoolReferenceNumber>
							<TotalGroupNumber>
								<xsl:value-of select="''"/>
							</TotalGroupNumber>
							<TradeNumber>
								<xsl:value-of select="''"/>
							</TradeNumber>
							<RepoTermDateREPOonly>
								<xsl:value-of select="''"/>
							</RepoTermDateREPOonly>
							<RepoAmountREPOonly>
								<xsl:value-of select="''"/>
							</RepoAmountREPOonly>
							<RepoReferenceNumberREPOonly>
								<xsl:value-of select="''"/>
							</RepoReferenceNumberREPOonly>
							<RepoRateREPOOnly>
								<xsl:value-of select="''"/>
							</RepoRateREPOOnly>
							<TickerCPFandCRFOnly>
								<xsl:value-of select="''"/>
							</TickerCPFandCRFOnly>
							<StrikePriceCPFandCRFOny>
								<xsl:value-of select="''"/>
							</StrikePriceCPFandCRFOny>
							<ExpirationDateCPFandCRFOnly>
								<xsl:value-of select="''"/>
							</ExpirationDateCPFandCRFOnly>
							<BrokerNumberCPFandCRFOnly>
								<xsl:value-of select="''"/>
							</BrokerNumberCPFandCRFOnly>
							<BrokerAccountCPFandCRFOnly>
								<xsl:value-of select="''"/>
							</BrokerAccountCPFandCRFOnly>
							<ContractSizeOptionContractandFutureContractOnly>
								<xsl:value-of select="''"/>
							</ContractSizeOptionContractandFutureContractOnly>
							<PlaceofTradeNarrative>
								<xsl:value-of select="''"/>
							</PlaceofTradeNarrative>
							<CommonReference>
								<xsl:value-of select="''"/>
							</CommonReference>
							<PartialSettlementAllowed>
								<xsl:value-of select="''"/>
							</PartialSettlementAllowed>
							<PartialSettlementTolerance>
								<xsl:value-of select="''"/>
							</PartialSettlementTolerance>
							<NoAutomaticMarketClaim>
								<xsl:value-of select="''"/>
							</NoAutomaticMarketClaim>
							<CorporateActionCouponOption>
								<xsl:value-of select="''"/>
							</CorporateActionCouponOption>
							<TripartyCollateralSegregation>
								<xsl:value-of select="''"/>
							</TripartyCollateralSegregation>
							<FXCancelForCANCinstructionsonly>
								<xsl:value-of select="''"/>
							</FXCancelForCANCinstructionsonly>
							<FundAccountingOnlyTradeRPTO>
								<xsl:value-of select="''"/>
							</FundAccountingOnlyTradeRPTO>
							<CustodyOnlyTradeNACT>
								<xsl:value-of select="''"/>
							</CustodyOnlyTradeNACT>
							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>
						</ThirdPartyFlatFileDetail>

					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
