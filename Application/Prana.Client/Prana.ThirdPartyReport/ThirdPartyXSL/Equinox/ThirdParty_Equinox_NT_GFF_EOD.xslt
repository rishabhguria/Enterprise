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


				<RecordType>
					<xsl:value-of select="'Record Type'"/>
				</RecordType>

				<GFFVersion>
					<xsl:value-of select="'GFF Version'"/>
				</GFFVersion>


				<GeneralInfo>
					<xsl:value-of select="'General Info'"/>
				</GeneralInfo>


				<SenderId>
					<xsl:value-of select="'Sender Id'"/>
				</SenderId>

				<Destination>
					<xsl:value-of select="'Destination'"/>
				</Destination>

				<SendersReference>
					<xsl:value-of select="'Senders Reference'"/>
				</SendersReference>

				<Functionofthemessage>
					<xsl:value-of select="'Function of the message'"/>
				</Functionofthemessage>

				<ReferenceInfo>
					<xsl:value-of select="'Reference Info'"/>
				</ReferenceInfo>

				<CountCurrentinstruction>
					<xsl:value-of select="'Count - Current instruction'"/>
				</CountCurrentinstruction>

				<CountTotalNoInstructions>
					<xsl:value-of select="'Count - Total No Instructions'"/>
				</CountTotalNoInstructions>

				<PreviousReference1>
					<xsl:value-of select="'Previous Reference 1'"/>
				</PreviousReference1>

				<PreviousReference2>
					<xsl:value-of select="'Previous Reference 2'"/>
				</PreviousReference2>

				<PreviousReference3>
					<xsl:value-of select="'Previous Reference 3'"/>
				</PreviousReference3>

				<PreviousReference4>
					<xsl:value-of select="'PreviousReference 4'"/>
				</PreviousReference4>

				<PreviousReference5>
					<xsl:value-of select="'Previous Reference 5'"/>
				</PreviousReference5>

				<PreadviseReference>
					<xsl:value-of select="'Preadvise Reference'"/>
				</PreadviseReference>


				<PoolReference>
					<xsl:value-of select="'Pool Reference'"/>
				</PoolReference>

				<DealReference>
					<xsl:value-of select="'Deal Reference'"/>
				</DealReference>

				<TradeInfo>
					<xsl:value-of select="'Trade Info'"/>
				</TradeInfo>



				<TradeDate>
					<xsl:value-of select="'Trade Date'"/>
				</TradeDate>

				<PlaceofTrade>
					<xsl:value-of select="'Place of Trade'"/>
				</PlaceofTrade>

				<InstructionType>
					<xsl:value-of select="'Instruction Type'"/>
				</InstructionType>


				<ProductType>
					<xsl:value-of select="'Product Type'"/>
				</ProductType>


				<SettlementDate>
					<xsl:value-of select="'Settlement Date'"/>
				</SettlementDate>

				<QuantityCVOriginalFace>
					<xsl:value-of select="'Quantity/CV/Original Face'"/>
				</QuantityCVOriginalFace>


				<SafekeepingAccount>
					<xsl:value-of select="'Safekeeping Account'"/>
				</SafekeepingAccount>

				<TradeType>
					<xsl:value-of select="'Trade Type'"/>
				</TradeType>

				<PlaceofSettlementBIC>
					<xsl:value-of select="'Place of Settlement - BIC'"/>
				</PlaceofSettlementBIC>

				<PlaceofSettlementISO>
					<xsl:value-of select="'Place of Settlement - ISO'"/>
				</PlaceofSettlementISO>

				<PlaceofSafekeeping>
					<xsl:value-of select="'Place of Safekeeping'"/>
				</PlaceofSafekeeping>

				<SecurityInfo>
					<xsl:value-of select="'Security Info'"/>
				</SecurityInfo>


				<SecurityIDType>
					<xsl:value-of select="'Security ID - Type'"/>
				</SecurityIDType>

				<SecurityID>
					<xsl:value-of select="'Security ID'"/>
				</SecurityID>

				<SecurityDescription>
					<xsl:value-of select="'Security Description'"/>
				</SecurityDescription>

				<SecurityType>
					<xsl:value-of select="'Security Type'"/>
				</SecurityType>

				<PlaceofListing>
					<xsl:value-of select="'Place of Listing'"/>
				</PlaceofListing>

				<BrokerInfo>
					<xsl:value-of select="'Broker Info'"/>
				</BrokerInfo>


				<ExecutingBrokerBIC>
					<xsl:value-of select="'Executing Broker - BIC'"/>
				</ExecutingBrokerBIC>

				<ExecutingBrokerLocalCode>
					<xsl:value-of select="'Executing Broker - Local Code'"/>
				</ExecutingBrokerLocalCode>

				<ExecutingBrokerName>
					<xsl:value-of select="'Executing Broker - Name'"/>
				</ExecutingBrokerName>

				<ClearingBrokerBIC>
					<xsl:value-of select="'Clearing Broker - BIC'"/>
				</ClearingBrokerBIC>

				<ClearingBrokerLocalCode>
					<xsl:value-of select="'Clearing Broker - Local Code'"/>
				</ClearingBrokerLocalCode>

				<ClearingBrokerName>
					<xsl:value-of select="'Clearing Broker - Name'"/>
				</ClearingBrokerName>

				<ExecBrokersAccount>
					<xsl:value-of select="'Exec Brokers Account'"/>
				</ExecBrokersAccount>

				<ClearingBrokersAccountFedThirdParty>
					<xsl:value-of select="'Clearing Brokers Account Fed Third Party'"/>
				</ClearingBrokersAccountFedThirdParty>

				<IntermediateBrokerBIC>
					<xsl:value-of select="'IntermediateBrokerBIC'"/>
				</IntermediateBrokerBIC>

				<IntermediateBrokerLocalCode>
					<xsl:value-of select="'Intermediate Broker - Local Code'"/>
				</IntermediateBrokerLocalCode>

				<IntermediateBrokerName>
					<xsl:value-of select="'Intermediate Broker - Name'"/>
				</IntermediateBrokerName>

				<IntermediateBrokerAcct>
					<xsl:value-of select="'Intermediate Broker Acct'"/>
				</IntermediateBrokerAcct>

				<SettlementInfo>
					<xsl:value-of select="'Settlement Info'"/>
				</SettlementInfo>


				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<TradingCurrency>
					<xsl:value-of select="'Trading Currency'"/>
				</TradingCurrency>

				<TradeAmount>
					<xsl:value-of select="'Trade Amount'"/>
				</TradeAmount>

				<SettlementCurrency>
					<xsl:value-of select="'Settlement Currency'"/>
				</SettlementCurrency>

				<SettlementAmount>
					<xsl:value-of select="'Settlement Amount'"/>
				</SettlementAmount>

				<ExecutingBrokersAmount>
					<xsl:value-of select="'Executing Brokers Amount'"/>
				</ExecutingBrokersAmount>

				<TaxCost>
					<xsl:value-of select="'Tax Cost'"/>
				</TaxCost>

				<NetGainLossAmount>
					<xsl:value-of select="'Net Gain/Loss Amount'"/>
				</NetGainLossAmount>

				<ChargesFees>
					<xsl:value-of select="'Charges/Fees'"/>
				</ChargesFees>

				<ConsumptionTax>
					<xsl:value-of select="'Consumption Tax'"/>
				</ConsumptionTax>

				 <CountryNationalFederalTax>
					<xsl:value-of select="'Country National Federal Tax'"/>
				</CountryNationalFederalTax>

			

				<IssueDiscountAllowance>
					<xsl:value-of select="'Issue Discount/ Allowance'"/>
				</IssueDiscountAllowance>

				<Reserved1>
					<xsl:value-of select="'-Reserved-'"/>
				</Reserved1>

				<LocalTax>
					<xsl:value-of select="'Local Tax'"/>
				</LocalTax>

				<LocalBrokersCommission>
					<xsl:value-of select="'Local Brokers Commission'"/>
				</LocalBrokersCommission>

				<Reserved2>
					<xsl:value-of select="'-Reserved-'"/>
				</Reserved2>

				<OtherAmount>
					<xsl:value-of select="'-Reserved-'"/>
				</OtherAmount>

				<Reserved3>
					<xsl:value-of select="'-Reserved-'"/>
				</Reserved3>

				<Reserved4>
					<xsl:value-of select="'-Reserved-'"/>
				</Reserved4>

				<ShippingAmount>
					<xsl:value-of select="'Shipping Amount'"/>
				</ShippingAmount>

				<SpecialConcessionsAmount>
					<xsl:value-of select="'Special Concessions Amount'"/>
				</SpecialConcessionsAmount>

				<StampDuty>
					<xsl:value-of select="'Stamp Duty'"/>
				</StampDuty>

				<StockExchangeTax>
					<xsl:value-of select="'Stock Exchange Tax'"/>
				</StockExchangeTax>


				<TransferTax>
					<xsl:value-of select="'Transfer Tax'"/>
				</TransferTax>

				<TransactionTax>
					<xsl:value-of select="'Transaction Tax'"/>
				</TransactionTax>

				<ValueAddedTax>
					<xsl:value-of select="'Value-Added Tax'"/>
				</ValueAddedTax>

				<WithholdingTax>
					<xsl:value-of select="'Withholding Tax'"/>
				</WithholdingTax>

				<ResultingAmount>
					<xsl:value-of select="'Resulting Amount'"/>
				</ResultingAmount>

				<WireAccountwithInstitutionBIC>
					<xsl:value-of select="'Wire Account with Institution - BIC'"/>
				</WireAccountwithInstitutionBIC>

				<WireAccountwithInstitutionLocalCodeType>
					<xsl:value-of select="'Wire Account with Institution-Local Code Type'"/>
				</WireAccountwithInstitutionLocalCodeType>

				<WireAccountwithInstitution-LocalCode>
					<xsl:value-of select="'Wire Account with Institution-Local Code'"/>
				</WireAccountwithInstitution-LocalCode>

				<WireAccountwithInstitution-Name>
					<xsl:value-of select="'Wire Account with Institution-Name'"/>
				</WireAccountwithInstitution-Name>

				<WireBeneficiary-BIC>
					<xsl:value-of select="'Wire Beneficiary-BIC'"/>
				</WireBeneficiary-BIC>

				<WireBeneficiaryLocalCodeType>
					<xsl:value-of select="'Wire Bene ficiary-Local Code-Type'"/>
				</WireBeneficiaryLocalCodeType>

				<WireBeneficiaryLocalCode>
					<xsl:value-of select="'Wire Beneficiary - Local Code'"/>
				</WireBeneficiaryLocalCode>

				<WireBeneficiaryName>
					<xsl:value-of select="'Wire Beneficiary - Name'"/>
				</WireBeneficiaryName>

				<WireBeneficiaryAccount>
					<xsl:value-of select="'Wire Beneficiary Account'"/>
				</WireBeneficiaryAccount>

				<MiscellaneousInfo>
					<xsl:value-of select="'Miscellaneous Info'"/>
				</MiscellaneousInfo>


				<Narrative>
					<xsl:value-of select="'Narrative'"/>
				</Narrative>

				<ShortSaleBuytoCover>
					<xsl:value-of select="'Short Sale Buy to Cover'"/>
				</ShortSaleBuytoCover>

				<Taxable>
					<xsl:value-of select="'Taxable'"/>
				</Taxable>

				<FreeCleanSettlement>
					<xsl:value-of select="'Free Clean Settlement'"/>
				</FreeCleanSettlement>

				<Physical>
					<xsl:value-of select="'Physical'"/>
				</Physical>

				<SpecialDelivery>
					<xsl:value-of select="'Special Delivery'"/>
				</SpecialDelivery>

				<SplitSettlement>
					<xsl:value-of select="'Split Settlement'"/>
				</SplitSettlement>

				<StampDutyCode>
					<xsl:value-of select="'Stamp Duty Code'"/>
				</StampDutyCode>

				<RealTimeGrossSettlement>
					<xsl:value-of select="'Real Time Gross Settlement'"/>
				</RealTimeGrossSettlement>

				<Beneficiary>
					<xsl:value-of select="'Beneficiary'"/>
				</Beneficiary>

				<FXSSI>
					<xsl:value-of select="'FX SSI'"/>
				</FXSSI>


				<BlockSettlement>
					<xsl:value-of select="'Block Settlement'"/>

				</BlockSettlement>

				<Tracking>
					<xsl:value-of select="'Tracking'"/>
				</Tracking>

				<FXInstruction>
					<xsl:value-of select="'FX Instruction'"/>
				</FXInstruction>

				<DebtFields>
					<xsl:value-of select="'Debt Fields'"/>
				</DebtFields>

				<LateDeliveryDate>
					<xsl:value-of select="'Late Delivery Date'"/>
				</LateDeliveryDate>

				<CurrentFace>
					<xsl:value-of select="'Current Face'"/>
				</CurrentFace>

				<MaturityDate>
					<xsl:value-of select="'Maturity Date'"/>
				</MaturityDate>

				<InterestRate>
					<xsl:value-of select="'Interest Rate'"/>
				</InterestRate>

				<AccruedInterestAmount>
					<xsl:value-of select="'Accrued Interest Amount'"/>
				</AccruedInterestAmount>

				<FXFields>
					<xsl:value-of select="'FX Fields'"/>
				</FXFields>


				<ExchangeRate>
					<xsl:value-of select="'Exchange Rate'"/>
				</ExchangeRate>

				<CurrencyBought>
					<xsl:value-of select="'Currency Bought'"/>
				</CurrencyBought>

				<AmountBought>
					<xsl:value-of select="'Amount Bought'"/>
				</AmountBought>

				<CurrencySold>
					<xsl:value-of select="'Currency Sold'"/>
				</CurrencySold>

				<AmountSold>
					<xsl:value-of select="'Amount Sold'"/>
				</AmountSold>

				<NetIndicator>
					<xsl:value-of select="'NetIndicator'"/>
				</NetIndicator>

				<ExecutingBrokerBIC1>
					<xsl:value-of select="'Executing Broker - BIC'"/>
				</ExecutingBrokerBIC1>

				<ExecutingBrokerName1>
					<xsl:value-of select="'Executing Broker - Name'"/>
				</ExecutingBrokerName1>

				<DeliveryAgentBIC>
					<xsl:value-of select="'Delivery Agent BIC'"/>
				</DeliveryAgentBIC>

				<DeliveryAgentName>
					<xsl:value-of select="'Delivery Agent Name'"/>
				</DeliveryAgentName>

				<IntermediaryBIC>
					<xsl:value-of select="'Intermediary BIC'"/>
				</IntermediaryBIC>

				<IntermediaryName>
					<xsl:value-of select="'Intermediary Name'"/>
				</IntermediaryName> 

				<IntermediaryClearingcode>
					<xsl:value-of select="'Intermediary Clearing code'"/>
				</IntermediaryClearingcode>

				<IntermediaryFedId>
					<xsl:value-of select="'Intermediary FedId'"/>
				</IntermediaryFedId>

				<ReceivingAgentBIC>
					<xsl:value-of select="'Receiving Agent BIC'"/>
				</ReceivingAgentBIC>

				<Receivingagentname>
					<xsl:value-of select="'Receiving agent name'"/>
				</Receivingagentname>

				<ReceivingagentAcct>
					<xsl:value-of select="'Receiving agent Acct'"/>
				</ReceivingagentAcct>



				<ReceivingAgentSortCode>
					<xsl:value-of select="'Receiving Agent Sort Code'"/>
				</ReceivingAgentSortCode>

				<ReceivingAgentClearingcode>
					<xsl:value-of select="'Receiving Agent Clearing code'"/>
				</ReceivingAgentClearingcode>

				<ReceivingagentFedID>
					<xsl:value-of select="'Receiving agent Fed ID'"/>
				</ReceivingagentFedID>

				<BeneficiaryInstitutionBIC>
					<xsl:value-of select="'Beneficiary Institution BIC'"/>
				</BeneficiaryInstitutionBIC>

				<BeneficiaryInstitutionName>
					<xsl:value-of select="'Beneficiary Institution Name'"/>
				</BeneficiaryInstitutionName>

				<BeneficiaryInstitutionAccountID>
					<xsl:value-of select="'Beneficiary Institution AccountID'"/>
				</BeneficiaryInstitutionAccountID>


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
					<xsl:when test ="TaxLotState!='Amemded'">
						<ThirdPartyFlatFileDetail>


							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<TaxLotState>
								<xsl:value-of select="TaxLotState"/>
							</TaxLotState>

							<RecordType>
								<xsl:value-of select="'TET'"/>
							</RecordType>

							<GFFVersion>
								<xsl:value-of select="'1'"/>
							</GFFVersion>


							<GeneralInfo>
								<xsl:value-of select="'GeneralInfo'"/>
							</GeneralInfo>


							<SenderId>
								<xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/> 
							</SenderId>

							<Destination>
								<xsl:value-of select="'NTC'"/>
							</Destination>

							<SendersReference>
								<xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/>
							</SendersReference>

							<Functionofthemessage>
								<xsl:choose>
									<xsl:when test="TaxLotState='Allocated'">
										<xsl:value-of select ="'N'"/>
									</xsl:when>

									<xsl:when test="TaxLotState='Deleted'">
										<xsl:value-of select ="'C'"/>
									</xsl:when>
									<xsl:when test="TaxLotState='Amemded'">
										<xsl:value-of select ="'A'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Functionofthemessage>

							<ReferenceInfo>
								<xsl:value-of select="'ReferenceInfo'"/>
							</ReferenceInfo>

							<CountCurrentinstruction>
								<xsl:value-of select="''"/>
							</CountCurrentinstruction>

							<CountTotalNoInstructions>
								<xsl:value-of select="''"/>
							</CountTotalNoInstructions>

							<PreviousReference1>
								<xsl:value-of select="''"/>
							</PreviousReference1>

							<PreviousReference2>
								<xsl:value-of select="''"/>
							</PreviousReference2>

							<PreviousReference3>
								<xsl:value-of select="''"/>
							</PreviousReference3>

							<PreviousReference4>
								<xsl:value-of select="''"/>
							</PreviousReference4>

							<PreviousReference5>
								<xsl:value-of select="''"/>
							</PreviousReference5>

							<PreadviseReference>
								<xsl:value-of select="''"/>
							</PreadviseReference>


							<PoolReference>
								<xsl:value-of select="''"/>
							</PoolReference>

							<DealReference>
								<xsl:value-of select="''"/>
							</DealReference>

							<TradeInfo>
								<xsl:value-of select="'TradeInfo'"/>
							</TradeInfo>


							<xsl:variable name="varTradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<TradeDate>
								<xsl:value-of select="concat(substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'),substring-after(substring-after($varTradeDate,'/'),'/'))"/>
							</TradeDate>
							
							<PlaceofTrade>
								<xsl:value-of select="''"/>
							</PlaceofTrade>

							<InstructionType>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'P'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>								
							</InstructionType>


							<ProductType>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'EQ'"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'DB'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ProductType>

							<xsl:variable name="varSettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<SettlementDate>
								<xsl:value-of select="concat(substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'),substring-after(substring-after($varSettlementDate,'/'),'/'))"/>
							</SettlementDate>
							

							<QuantityCVOriginalFace>
								<xsl:value-of select="OrderQty"/>
							</QuantityCVOriginalFace>

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
							<SafekeepingAccount>
								<xsl:value-of select="$varAccountName"/>
							</SafekeepingAccount>

							<TradeType>
								<xsl:value-of select="'TR'"/>
							</TradeType>

							<PlaceofSettlementBIC>
								<xsl:value-of select="''"/>
							</PlaceofSettlementBIC>

							<PlaceofSettlementISO>
								<xsl:value-of select="substring(SettlCurrency,1,2)"/>
							</PlaceofSettlementISO>

							<PlaceofSafekeeping>
								<xsl:value-of select="''"/>
							</PlaceofSafekeeping>

							<SecurityInfo>
								<xsl:value-of select="'SecurityInfo'"/>
							</SecurityInfo>


							<SecurityIDType>
								<xsl:choose>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="'CU'"/>
									</xsl:when>
									<xsl:when test="ISIN!=''">
										<xsl:value-of select="'IS'"/>
									</xsl:when>
									<xsl:when test="SEDOL!=''">
										<xsl:value-of select="'SD'"/>
									</xsl:when>
									<xsl:when test="Symbol!=''">
										<xsl:value-of select="'TC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityIDType>

							<SecurityID>
								<xsl:value-of select="''"/>
							</SecurityID>

							<SecurityDescription>
								<xsl:value-of select="FullSecurityName"/>
							</SecurityDescription>

							<SecurityType>
								<xsl:value-of select="''"/>
							</SecurityType>

							<PlaceofListing>
								<xsl:value-of select="''"/>
							</PlaceofListing>

							<BrokerInfo>
								<xsl:value-of select="'BrokerInfo'"/>
							</BrokerInfo>
							
							<ExecutingBrokerBIC>
								<xsl:value-of select="''"/>
							</ExecutingBrokerBIC>
							
							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<ExecutingBrokerLocalCode>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER!= ''">
										<xsl:value-of select="$THIRDPARTY_BROKER"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</ExecutingBrokerLocalCode>
							

							<ExecutingBrokerName>
								<xsl:value-of select="''"/>
							</ExecutingBrokerName>

							<ClearingBrokerBIC>
								<xsl:value-of select="''"/>
							</ClearingBrokerBIC>

							<ClearingBrokerLocalCode>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER!= ''">
										<xsl:value-of select="$THIRDPARTY_BROKER"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</ClearingBrokerLocalCode>

							<ClearingBrokerName>
								<xsl:value-of select="''"/>
							</ClearingBrokerName>

							<ExecBrokersAccount>
								<xsl:value-of select="''"/>
							</ExecBrokersAccount>

							<ClearingBrokersAccountFedThirdParty>
								<xsl:value-of select="''"/>
							</ClearingBrokersAccountFedThirdParty>

							<IntermediateBrokerBIC>
								<xsl:value-of select="''"/>
							</IntermediateBrokerBIC>

							<IntermediateBrokerLocalCode>
								<xsl:value-of select="''"/>
							</IntermediateBrokerLocalCode>

							<IntermediateBrokerName>
								<xsl:value-of select="''"/>
							</IntermediateBrokerName>

							<IntermediateBrokerAcct>
								<xsl:value-of select="''"/>
							</IntermediateBrokerAcct>

							<SettlementInfo>
								<xsl:value-of select="'SettlementInfo'"/>
							</SettlementInfo>

							<xsl:variable name="AvgPrice1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="AvgPrice * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="AvgPrice div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<Price>
								<xsl:value-of select="format-number($AvgPrice1,'0.#######')"/>
							</Price>
							
							<TradingCurrency>
								<xsl:value-of select="CurrencySymbol"/>
							</TradingCurrency>

							<TradeAmount>
								<xsl:value-of select="$varNetamount"/>
							</TradeAmount>

							<SettlementCurrency>
								<xsl:value-of select="''"/>
							</SettlementCurrency>
							
							<xsl:variable name="varSettlementAmount">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varNetamount * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varNetamount div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<SettlementAmount>
								<xsl:value-of select="$varSettlementAmount"/>
							</SettlementAmount>

							<xsl:variable name="varOtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
							</xsl:variable>

							<xsl:variable name="varOtherFees1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varOtherFees"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varOtherFees * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varOtherFees div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ExecutingBrokersAmount>
								<xsl:value-of select="$varOtherFees1"/>
							</ExecutingBrokersAmount>

							<TaxCost>
								<xsl:value-of select="''"/>
							</TaxCost>

							<NetGainLossAmount>
								<xsl:value-of select="''"/>
							</NetGainLossAmount>

							<ChargesFees>
								<xsl:value-of select="''"/>
							</ChargesFees>

							<ConsumptionTax>
								<xsl:value-of select="''"/>
							</ConsumptionTax>

							<CountryNationalFederalTax>
								<xsl:value-of select="''"/>
							</CountryNationalFederalTax>

						

							<IssueDiscountAllowance>
								<xsl:value-of select="''"/>
							</IssueDiscountAllowance>

							<Reserved1>
								<xsl:value-of select="''"/>
							</Reserved1>

							<LocalTax>
								<xsl:value-of select="''"/>
							</LocalTax>

							<LocalBrokersCommission>
								<xsl:value-of select="''"/>
							</LocalBrokersCommission>

							<Reserved2>
								<xsl:value-of select="''"/>
							</Reserved2>

							<OtherAmount>
								<xsl:value-of select="''"/>
							</OtherAmount>

							<Reserved3>
								<xsl:value-of select="''"/>
							</Reserved3>

							<Reserved4>
								<xsl:value-of select="''"/>
							</Reserved4>

							<ShippingAmount>
								<xsl:value-of select="''"/>
							</ShippingAmount>

							<SpecialConcessionsAmount>
								<xsl:value-of select="''"/>
							</SpecialConcessionsAmount>

							<StampDuty>
								<xsl:value-of select="''"/>
							</StampDuty>

							<StockExchangeTax>
								<xsl:value-of select="''"/>
							</StockExchangeTax>


							<TransferTax>
								<xsl:value-of select="''"/>
							</TransferTax>

							<TransactionTax>
								<xsl:value-of select="''"/>
							</TransactionTax>

							<ValueAddedTax>
								<xsl:value-of select="''"/>
							</ValueAddedTax>

							<WithholdingTax>
								<xsl:value-of select="''"/>
							</WithholdingTax>

							<ResultingAmount>
								<xsl:value-of select="''"/>
							</ResultingAmount>

							<WireAccountwithInstitutionBIC>
								<xsl:value-of select="''"/>
							</WireAccountwithInstitutionBIC>

							<WireAccountwithInstitutionLocalCodeType>
								<xsl:value-of select="''"/>
							</WireAccountwithInstitutionLocalCodeType>

							<WireAccountwithInstitution-LocalCode>
								<xsl:value-of select="''"/>
							</WireAccountwithInstitution-LocalCode>

							<WireAccountwithInstitution-Name>
								<xsl:value-of select="''"/>
							</WireAccountwithInstitution-Name>

							<WireBeneficiary-BIC>
								<xsl:value-of select="''"/>
							</WireBeneficiary-BIC>

							<WireBeneficiaryLocalCodeType>
								<xsl:value-of select="''"/>
							</WireBeneficiaryLocalCodeType>

							<WireBeneficiaryLocalCode>
								<xsl:value-of select="''"/>
							</WireBeneficiaryLocalCode>

							<WireBeneficiaryName>
								<xsl:value-of select="''"/>
							</WireBeneficiaryName>

							<WireBeneficiaryAccount>
								<xsl:value-of select="''"/>
							</WireBeneficiaryAccount>

							<MiscellaneousInfo>
								<xsl:value-of select="'MiscellaneousInfo'"/>
							</MiscellaneousInfo>


							<Narrative>
								<xsl:value-of select="''"/>
							</Narrative>

							<ShortSaleBuytoCover>
								<xsl:value-of select="''"/>
							</ShortSaleBuytoCover>

							<Taxable>
								<xsl:value-of select="''"/>
							</Taxable>

							<FreeCleanSettlement>
								<xsl:value-of select="''"/>
							</FreeCleanSettlement>

							<Physical>
								<xsl:value-of select="''"/>
							</Physical>

							<SpecialDelivery>
								<xsl:value-of select="''"/>
							</SpecialDelivery>

							<SplitSettlement>
								<xsl:value-of select="''"/>
							</SplitSettlement>

							<StampDutyCode>
								<xsl:value-of select="''"/>
							</StampDutyCode>

							<RealTimeGrossSettlement>
								<xsl:value-of select="''"/>
							</RealTimeGrossSettlement>

							<Beneficiary>
								<xsl:value-of select="''"/>
							</Beneficiary>

							<FXSSI>
								<xsl:value-of select="''"/>
							</FXSSI>


							<BlockSettlement>
								<xsl:value-of select="''"/>
								
							</BlockSettlement>

							<Tracking>
								<xsl:value-of select="''"/>
							</Tracking>

							<FXInstruction>
								<xsl:value-of select="''"/>
							</FXInstruction>

							<DebtFields>
								<xsl:value-of select="'DebtFields'"/>
							</DebtFields>

							<LateDeliveryDate>
								<xsl:value-of select="''"/>
							</LateDeliveryDate>

							<CurrentFace>
								<xsl:value-of select="''"/>
							</CurrentFace>

							<MaturityDate>
								<xsl:value-of select="''"/>
							</MaturityDate>

							<InterestRate>
								<xsl:value-of select="''"/>
							</InterestRate>

							<AccruedInterestAmount>
								<xsl:value-of select="AccruedInterest"/>
							</AccruedInterestAmount>

							<FXFields>
								<xsl:value-of select="''"/>
							</FXFields>


							<ExchangeRate>
								<xsl:value-of select="''"/>
							</ExchangeRate>

							<CurrencyBought>
								<xsl:value-of select="''"/>
							</CurrencyBought>

							<AmountBought>
								<xsl:value-of select="''"/>
							</AmountBought>

							<CurrencySold>
								<xsl:value-of select="''"/>
							</CurrencySold>

							<AmountSold>
								<xsl:value-of select="''"/>
							</AmountSold>

							<NetIndicator>
								<xsl:value-of select="''"/>
							</NetIndicator>

							<ExecutingBrokerBIC1>
								<xsl:value-of select="''"/>
							</ExecutingBrokerBIC1>

							<ExecutingBrokerName1>
								<xsl:value-of select="''"/>
							</ExecutingBrokerName1>

							<DeliveryAgentBIC>
								<xsl:value-of select="''"/>
							</DeliveryAgentBIC>

							<DeliveryAgentName>
								<xsl:value-of select="''"/>
							</DeliveryAgentName>

							<IntermediaryBIC>
								<xsl:value-of select="''"/>
							</IntermediaryBIC>

							<IntermediaryName>
								<xsl:value-of select="''"/>
							</IntermediaryName>

							<IntermediaryClearingcode>
								<xsl:value-of select="''"/>
							</IntermediaryClearingcode>

							<IntermediaryFedId>
								<xsl:value-of select="''"/>
							</IntermediaryFedId>

							<ReceivingAgentBIC>
								<xsl:value-of select="''"/>
							</ReceivingAgentBIC>

							<Receivingagentname>
								<xsl:value-of select="''"/>
							</Receivingagentname>

							<ReceivingagentAcct>
								<xsl:value-of select="''"/>
							</ReceivingagentAcct>
							
							
							
							<ReceivingAgentSortCode>
								<xsl:value-of select="''"/>
							</ReceivingAgentSortCode>

							<ReceivingAgentClearingcode>
								<xsl:value-of select="''"/>
							</ReceivingAgentClearingcode>

							<ReceivingagentFedID>
								<xsl:value-of select="''"/>
							</ReceivingagentFedID>

							<BeneficiaryInstitutionBIC>
								<xsl:value-of select="''"/>
							</BeneficiaryInstitutionBIC>

							<BeneficiaryInstitutionName>
								<xsl:value-of select="''"/>
							</BeneficiaryInstitutionName>

							<BeneficiaryInstitutionAccountID>
								<xsl:value-of select="''"/>
							</BeneficiaryInstitutionAccountID> 



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
								
								<RecordType>
									<xsl:value-of select="'TET'"/>
								</RecordType>

								<GFFVersion>
									<xsl:value-of select="'1'"/>
								</GFFVersion>


								<GeneralInfo>
									<xsl:value-of select="'GeneralInfo'"/>
								</GeneralInfo>


								<SenderId>
									<xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/>
								</SenderId>

								<Destination>
									<xsl:value-of select="'NTC'"/>
								</Destination>

								<SendersReference>
									<xsl:value-of select="substring(AmendTaxLotId1,string-length(AmendTaxLotId1)-7,string-length(AmendTaxLotId1))"/>
								</SendersReference>

								<Functionofthemessage>							
											<xsl:value-of select ="'C'"/>				
										
								</Functionofthemessage>

								<ReferenceInfo>
									<xsl:value-of select="'ReferenceInfo'"/>
								</ReferenceInfo>

								<CountCurrentinstruction>
									<xsl:value-of select="''"/>
								</CountCurrentinstruction>

								<CountTotalNoInstructions>
									<xsl:value-of select="''"/>
								</CountTotalNoInstructions>

								<PreviousReference1>
									<xsl:value-of select="''"/>
								</PreviousReference1>

								<PreviousReference2>
									<xsl:value-of select="''"/>
								</PreviousReference2>

								<PreviousReference3>
									<xsl:value-of select="''"/>
								</PreviousReference3>

								<PreviousReference4>
									<xsl:value-of select="''"/>
								</PreviousReference4>

								<PreviousReference5>
									<xsl:value-of select="''"/>
								</PreviousReference5>

								<PreadviseReference>
									<xsl:value-of select="''"/>
								</PreadviseReference>


								<PoolReference>
									<xsl:value-of select="''"/>
								</PoolReference>

								<DealReference>
									<xsl:value-of select="''"/>
								</DealReference>

								<TradeInfo>
									<xsl:value-of select="'TradeInfo'"/>
								</TradeInfo>


								<xsl:variable name="varTradeDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldTradeDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>
								<TradeDate>
									<xsl:value-of select="concat(substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'),substring-after(substring-after($varTradeDate,'/'),'/'))"/>
								</TradeDate>

								<PlaceofTrade>
									<xsl:value-of select="''"/>
								</PlaceofTrade>

								<InstructionType>
									<xsl:choose>
										<xsl:when test="OldSide='Buy'">
											<xsl:value-of select="'P'"/>
										</xsl:when>
										<xsl:when test="OldSide='Sell'">
											<xsl:value-of select="'S'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</InstructionType>


								<ProductType>
									<xsl:choose>
										<xsl:when test="Asset='Equity'">
											<xsl:value-of select="'EQ'"/>
										</xsl:when>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="'DB'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</ProductType>

								<xsl:variable name="varSettlementDate">
									<xsl:call-template name="DateFormat">
										<xsl:with-param name="Date" select="OldSettlementDate">
										</xsl:with-param>
									</xsl:call-template>
								</xsl:variable>
								<SettlementDate>
									<xsl:value-of select="concat(substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'),substring-after(substring-after($varSettlementDate,'/'),'/'))"/>
								</SettlementDate>


								<QuantityCVOriginalFace>
									<xsl:value-of select="OldExecutedQuantity"/>
								</QuantityCVOriginalFace>

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
								<SafekeepingAccount>
									<xsl:value-of select="$varAccountName"/>
								</SafekeepingAccount>

								<TradeType>
									<xsl:value-of select="'TR'"/>
								</TradeType>

								<PlaceofSettlementBIC>
									<xsl:value-of select="''"/>
								</PlaceofSettlementBIC>

								<PlaceofSettlementISO>
									<xsl:value-of select="substring(OldSettlCurrency,1,2)"/>
								</PlaceofSettlementISO>

								<PlaceofSafekeeping>
									<xsl:value-of select="''"/>
								</PlaceofSafekeeping>

								<SecurityInfo>
									<xsl:value-of select="'SecurityInfo'"/>
								</SecurityInfo>


								<SecurityIDType>
									<xsl:choose>
										<xsl:when test="CUSIP!=''">
											<xsl:value-of select="'CU'"/>
										</xsl:when>
										<xsl:when test="ISIN!=''">
											<xsl:value-of select="'IS'"/>
										</xsl:when>
										<xsl:when test="SEDOL!=''">
											<xsl:value-of select="'SD'"/>
										</xsl:when>
										<xsl:when test="Symbol!=''">
											<xsl:value-of select="'TC'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SecurityIDType>

								<SecurityID>
									<xsl:value-of select="''"/>
								</SecurityID>

								<SecurityDescription>
									<xsl:value-of select="CompanyName"/>
								</SecurityDescription>

								<SecurityType>
									<xsl:value-of select="''"/>
								</SecurityType>

								<PlaceofListing>
									<xsl:value-of select="''"/>
								</PlaceofListing>

								<BrokerInfo>
									<xsl:value-of select="'BrokerInfo'"/>
								</BrokerInfo>

								<ExecutingBrokerBIC>
									<xsl:value-of select="''"/>
								</ExecutingBrokerBIC>

								<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
								<xsl:variable name="THIRDPARTY_BROKER">
									<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
								</xsl:variable>
								<ExecutingBrokerLocalCode>
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_BROKER!= ''">
											<xsl:value-of select="$THIRDPARTY_BROKER"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</ExecutingBrokerLocalCode>


								<ExecutingBrokerName>
									<xsl:value-of select="''"/>
								</ExecutingBrokerName>

								<ClearingBrokerBIC>
									<xsl:value-of select="''"/>
								</ClearingBrokerBIC>

								<ClearingBrokerLocalCode>
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_BROKER!= ''">
											<xsl:value-of select="$THIRDPARTY_BROKER"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</ClearingBrokerLocalCode>

								<ClearingBrokerName>
									<xsl:value-of select="''"/>
								</ClearingBrokerName>

								<ExecBrokersAccount>
									<xsl:value-of select="''"/>
								</ExecBrokersAccount>

								<ClearingBrokersAccountFedThirdParty>
									<xsl:value-of select="''"/>
								</ClearingBrokersAccountFedThirdParty>

								<IntermediateBrokerBIC>
									<xsl:value-of select="''"/>
								</IntermediateBrokerBIC>

								<IntermediateBrokerLocalCode>
									<xsl:value-of select="''"/>
								</IntermediateBrokerLocalCode>

								<IntermediateBrokerName>
									<xsl:value-of select="''"/>
								</IntermediateBrokerName>

								<IntermediateBrokerAcct>
									<xsl:value-of select="''"/>
								</IntermediateBrokerAcct>

								<SettlementInfo>
									<xsl:value-of select="'SettlementInfo'"/>
								</SettlementInfo>

								<xsl:variable name="AvgPrice1">
									<xsl:choose>
										<xsl:when test="SettlCurrFxRate=0">
											<xsl:value-of select="OldAvgPrice"/>
										</xsl:when>
										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
											<xsl:value-of select="OldAvgPrice * SettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
											<xsl:value-of select="OldAvgPrice div SettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<Price>
									<xsl:value-of select="format-number($AvgPrice1,'0.#######')"/>
								</Price>

								<TradingCurrency>
									<xsl:value-of select="CurrencySymbol"/>
								</TradingCurrency>
								
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
								<TradeAmount>
									<xsl:value-of select="$varOldNetAmount"/>
								</TradeAmount>

								<SettlementCurrency>
									<xsl:value-of select="''"/>
								</SettlementCurrency>

								<xsl:variable name="varSettlementAmount">
									<xsl:choose>
										<xsl:when test="SettlCurrFxRate=0">
											<xsl:value-of select="$varOldNetAmount"/>
										</xsl:when>
										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
											<xsl:value-of select="$varOldNetAmount * SettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
											<xsl:value-of select="$varOldNetAmount div SettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<SettlementAmount>
									<xsl:value-of select="$varSettlementAmount"/>
								</SettlementAmount>

								<xsl:variable name="varOldOtherFees">
									<xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + TaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
								</xsl:variable>

								<xsl:variable name="varOtherFees1">
									<xsl:choose>
										<xsl:when test="SettlCurrFxRate=0">
											<xsl:value-of select="$varOldOtherFees"/>
										</xsl:when>
										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
											<xsl:value-of select="$varOldOtherFees * SettlCurrFxRate"/>
										</xsl:when>

										<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
											<xsl:value-of select="$varOldOtherFees div SettlCurrFxRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<ExecutingBrokersAmount>
									<xsl:value-of select="$varOtherFees1"/>
								</ExecutingBrokersAmount>

								<TaxCost>
									<xsl:value-of select="''"/>
								</TaxCost>

								<NetGainLossAmount>
									<xsl:value-of select="''"/>
								</NetGainLossAmount>

								<ChargesFees>
									<xsl:value-of select="''"/>
								</ChargesFees>

								<ConsumptionTax>
									<xsl:value-of select="''"/>
								</ConsumptionTax>

								<CountryNationalFederalTax>
									<xsl:value-of select="''"/>
								</CountryNationalFederalTax>

							

								<IssueDiscountAllowance>
									<xsl:value-of select="''"/>
								</IssueDiscountAllowance>

								<Reserved1>
									<xsl:value-of select="''"/>
								</Reserved1>

								<LocalTax>
									<xsl:value-of select="''"/>
								</LocalTax>

								<LocalBrokersCommission>
									<xsl:value-of select="''"/>
								</LocalBrokersCommission>

								<Reserved2>
									<xsl:value-of select="''"/>
								</Reserved2>

								<OtherAmount>
									<xsl:value-of select="''"/>
								</OtherAmount>

								<Reserved3>
									<xsl:value-of select="''"/>
								</Reserved3>

								<Reserved4>
									<xsl:value-of select="''"/>
								</Reserved4>

								<ShippingAmount>
									<xsl:value-of select="''"/>
								</ShippingAmount>

								<SpecialConcessionsAmount>
									<xsl:value-of select="''"/>
								</SpecialConcessionsAmount>

								<StampDuty>
									<xsl:value-of select="''"/>
								</StampDuty>

								<StockExchangeTax>
									<xsl:value-of select="''"/>
								</StockExchangeTax>


								<TransferTax>
									<xsl:value-of select="''"/>
								</TransferTax>

								<TransactionTax>
									<xsl:value-of select="''"/>
								</TransactionTax>

								<ValueAddedTax>
									<xsl:value-of select="''"/>
								</ValueAddedTax>

								<WithholdingTax>
									<xsl:value-of select="''"/>
								</WithholdingTax>

								<ResultingAmount>
									<xsl:value-of select="''"/>
								</ResultingAmount>

								<WireAccountwithInstitutionBIC>
									<xsl:value-of select="''"/>
								</WireAccountwithInstitutionBIC>

								<WireAccountwithInstitutionLocalCodeType>
									<xsl:value-of select="''"/>
								</WireAccountwithInstitutionLocalCodeType>

								<WireAccountwithInstitution-LocalCode>
									<xsl:value-of select="''"/>
								</WireAccountwithInstitution-LocalCode>

								<WireAccountwithInstitution-Name>
									<xsl:value-of select="''"/>
								</WireAccountwithInstitution-Name>

								<WireBeneficiary-BIC>
									<xsl:value-of select="''"/>
								</WireBeneficiary-BIC>

								<WireBeneficiaryLocalCodeType>
									<xsl:value-of select="''"/>
								</WireBeneficiaryLocalCodeType>

								<WireBeneficiaryLocalCode>
									<xsl:value-of select="''"/>
								</WireBeneficiaryLocalCode>

								<WireBeneficiaryName>
									<xsl:value-of select="''"/>
								</WireBeneficiaryName>

								<WireBeneficiaryAccount>
									<xsl:value-of select="''"/>
								</WireBeneficiaryAccount>

								<MiscellaneousInfo>
									<xsl:value-of select="'MiscellaneousInfo'"/>
								</MiscellaneousInfo>


								<Narrative>
									<xsl:value-of select="''"/>
								</Narrative>

								<ShortSaleBuytoCover>
									<xsl:value-of select="''"/>
								</ShortSaleBuytoCover>

								<Taxable>
									<xsl:value-of select="''"/>
								</Taxable>

								<FreeCleanSettlement>
									<xsl:value-of select="''"/>
								</FreeCleanSettlement>

								<Physical>
									<xsl:value-of select="''"/>
								</Physical>

								<SpecialDelivery>
									<xsl:value-of select="''"/>
								</SpecialDelivery>

								<SplitSettlement>
									<xsl:value-of select="''"/>
								</SplitSettlement>

								<StampDutyCode>
									<xsl:value-of select="''"/>
								</StampDutyCode>

								<RealTimeGrossSettlement>
									<xsl:value-of select="''"/>
								</RealTimeGrossSettlement>

								<Beneficiary>
									<xsl:value-of select="''"/>
								</Beneficiary>

								<FXSSI>
									<xsl:value-of select="''"/>
								</FXSSI>


								<BlockSettlement>
									<xsl:value-of select="''"/>

								</BlockSettlement>

								<Tracking>
									<xsl:value-of select="''"/>
								</Tracking>

								<FXInstruction>
									<xsl:value-of select="''"/>
								</FXInstruction>

								<DebtFields>
									<xsl:value-of select="'DebtFields'"/>
								</DebtFields>

								<LateDeliveryDate>
									<xsl:value-of select="''"/>
								</LateDeliveryDate>

								<CurrentFace>
									<xsl:value-of select="''"/>
								</CurrentFace>

								<MaturityDate>
									<xsl:value-of select="''"/>
								</MaturityDate>

								<InterestRate>
									<xsl:value-of select="''"/>
								</InterestRate>

								<AccruedInterestAmount>
									<xsl:value-of select="OldAccruedInterest"/>
								</AccruedInterestAmount>

								<FXFields>
									<xsl:value-of select="''"/>
								</FXFields>


								<ExchangeRate>
									<xsl:value-of select="OldFXRate"/>
								</ExchangeRate>

								<CurrencyBought>
									<xsl:value-of select="''"/>
								</CurrencyBought>

								<AmountBought>
									<xsl:value-of select="''"/>
								</AmountBought>

								<CurrencySold>
									<xsl:value-of select="''"/>
								</CurrencySold>

								<AmountSold>
									<xsl:value-of select="''"/>
								</AmountSold>

								<NetIndicator>
									<xsl:value-of select="''"/>
								</NetIndicator>

								<ExecutingBrokerBIC1>
									<xsl:value-of select="''"/>
								</ExecutingBrokerBIC1>

								<ExecutingBrokerName1>
									<xsl:value-of select="''"/>
								</ExecutingBrokerName1>

								<DeliveryAgentBIC>
									<xsl:value-of select="''"/>
								</DeliveryAgentBIC>

								<DeliveryAgentName>
									<xsl:value-of select="''"/>
								</DeliveryAgentName>

								<IntermediaryBIC>
									<xsl:value-of select="''"/>
								</IntermediaryBIC>

								<IntermediaryName>
									<xsl:value-of select="''"/>
								</IntermediaryName>

								<IntermediaryClearingcode>
									<xsl:value-of select="''"/>
								</IntermediaryClearingcode>

								<IntermediaryFedId>
									<xsl:value-of select="''"/>
								</IntermediaryFedId>

								<ReceivingAgentBIC>
									<xsl:value-of select="''"/>
								</ReceivingAgentBIC>

								<Receivingagentname>
									<xsl:value-of select="''"/>
								</Receivingagentname>

								<ReceivingagentAcct>
									<xsl:value-of select="''"/>
								</ReceivingagentAcct>



								<ReceivingAgentSortCode>
									<xsl:value-of select="''"/>
								</ReceivingAgentSortCode>

								<ReceivingAgentClearingcode>
									<xsl:value-of select="''"/>
								</ReceivingAgentClearingcode>

								<ReceivingagentFedID>
									<xsl:value-of select="''"/>
								</ReceivingagentFedID>

								<BeneficiaryInstitutionBIC>
									<xsl:value-of select="''"/>
								</BeneficiaryInstitutionBIC>

								<BeneficiaryInstitutionName>
									<xsl:value-of select="''"/>
								</BeneficiaryInstitutionName>

								<BeneficiaryInstitutionAccountID>
									<xsl:value-of select="''"/>
								</BeneficiaryInstitutionAccountID>

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


							<RecordType>
								<xsl:value-of select="'TET'"/>
							</RecordType>

							<GFFVersion>
								<xsl:value-of select="'1'"/>
							</GFFVersion>


							<GeneralInfo>
								<xsl:value-of select="'GeneralInfo'"/>
							</GeneralInfo>


							<SenderId>
								<xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/>
							</SenderId>

							<Destination>
								<xsl:value-of select="'NTC'"/>
							</Destination>

							<SendersReference>
								<xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/>
							</SendersReference>

							<Functionofthemessage>						
									
										<xsl:value-of select ="'N'"/>								
									
							</Functionofthemessage>

							<ReferenceInfo>
								<xsl:value-of select="'ReferenceInfo'"/>
							</ReferenceInfo>

							<CountCurrentinstruction>
								<xsl:value-of select="''"/>
							</CountCurrentinstruction>

							<CountTotalNoInstructions>
								<xsl:value-of select="''"/>
							</CountTotalNoInstructions>

							<PreviousReference1>
								<xsl:value-of select="''"/>
							</PreviousReference1>

							<PreviousReference2>
								<xsl:value-of select="''"/>
							</PreviousReference2>

							<PreviousReference3>
								<xsl:value-of select="''"/>
							</PreviousReference3>

							<PreviousReference4>
								<xsl:value-of select="''"/>
							</PreviousReference4>

							<PreviousReference5>
								<xsl:value-of select="''"/>
							</PreviousReference5>

							<PreadviseReference>
								<xsl:value-of select="''"/>
							</PreadviseReference>


							<PoolReference>
								<xsl:value-of select="''"/>
							</PoolReference>

							<DealReference>
								<xsl:value-of select="''"/>
							</DealReference>

							<TradeInfo>
								<xsl:value-of select="'TradeInfo'"/>
							</TradeInfo>


							<xsl:variable name="varTradeDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="TradeDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<TradeDate>
								<xsl:value-of select="concat(substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'),substring-after(substring-after($varTradeDate,'/'),'/'))"/>
							</TradeDate>

							<PlaceofTrade>
								<xsl:value-of select="''"/>
							</PlaceofTrade>

							<InstructionType>
								<xsl:choose>
									<xsl:when test="Side='Buy'">
										<xsl:value-of select="'P'"/>
									</xsl:when>
									<xsl:when test="Side='Sell'">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</InstructionType>


							<ProductType>
								<xsl:choose>
									<xsl:when test="Asset='Equity'">
										<xsl:value-of select="'EQ'"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="'DB'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ProductType>

							<xsl:variable name="varSettlementDate">
								<xsl:call-template name="DateFormat">
									<xsl:with-param name="Date" select="SettlementDate">
									</xsl:with-param>
								</xsl:call-template>
							</xsl:variable>
							<SettlementDate>
								<xsl:value-of select="concat(substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'),substring-after(substring-after($varSettlementDate,'/'),'/'))"/>
							</SettlementDate>


							<QuantityCVOriginalFace>
								<xsl:value-of select="OrderQty"/>
							</QuantityCVOriginalFace>

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
							<SafekeepingAccount>
								<xsl:value-of select="$varAccountName"/>
							</SafekeepingAccount>

							<TradeType>
								<xsl:value-of select="'TR'"/>
							</TradeType>

							<PlaceofSettlementBIC>
								<xsl:value-of select="''"/>
							</PlaceofSettlementBIC>

							<PlaceofSettlementISO>
								<xsl:value-of select="substring(SettlCurrency,1,2)"/>
							</PlaceofSettlementISO>

							<PlaceofSafekeeping>
								<xsl:value-of select="''"/>
							</PlaceofSafekeeping>

							<SecurityInfo>
								<xsl:value-of select="'SecurityInfo'"/>
							</SecurityInfo>


							<SecurityIDType>
								<xsl:choose>
									<xsl:when test="CUSIP!=''">
										<xsl:value-of select="'CU'"/>
									</xsl:when>
									<xsl:when test="ISIN!=''">
										<xsl:value-of select="'IS'"/>
									</xsl:when>
									<xsl:when test="SEDOL!=''">
										<xsl:value-of select="'SD'"/>
									</xsl:when>
									<xsl:when test="Symbol!=''">
										<xsl:value-of select="'TC'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityIDType>

							<SecurityID>
								<xsl:value-of select="''"/>
							</SecurityID>

							<SecurityDescription>
								<xsl:value-of select="FullSecurityName"/>
							</SecurityDescription>

							<SecurityType>
								<xsl:value-of select="''"/>
							</SecurityType>

							<PlaceofListing>
								<xsl:value-of select="''"/>
							</PlaceofListing>

							<BrokerInfo>
								<xsl:value-of select="'BrokerInfo'"/>
							</BrokerInfo>

							<ExecutingBrokerBIC>
								<xsl:value-of select="''"/>
							</ExecutingBrokerBIC>

							<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
							<xsl:variable name="THIRDPARTY_BROKER">
								<xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
							</xsl:variable>
							<ExecutingBrokerLocalCode>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER!= ''">
										<xsl:value-of select="$THIRDPARTY_BROKER"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</ExecutingBrokerLocalCode>


							<ExecutingBrokerName>
								<xsl:value-of select="''"/>
							</ExecutingBrokerName>

							<ClearingBrokerBIC>
								<xsl:value-of select="''"/>
							</ClearingBrokerBIC>

							<ClearingBrokerLocalCode>
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER!= ''">
										<xsl:value-of select="$THIRDPARTY_BROKER"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</ClearingBrokerLocalCode>

							<ClearingBrokerName>
								<xsl:value-of select="''"/>
							</ClearingBrokerName>

							<ExecBrokersAccount>
								<xsl:value-of select="''"/>
							</ExecBrokersAccount>

							<ClearingBrokersAccountFedThirdParty>
								<xsl:value-of select="''"/>
							</ClearingBrokersAccountFedThirdParty>

							<IntermediateBrokerBIC>
								<xsl:value-of select="''"/>
							</IntermediateBrokerBIC>

							<IntermediateBrokerLocalCode>
								<xsl:value-of select="''"/>
							</IntermediateBrokerLocalCode>

							<IntermediateBrokerName>
								<xsl:value-of select="''"/>
							</IntermediateBrokerName>

							<IntermediateBrokerAcct>
								<xsl:value-of select="''"/>
							</IntermediateBrokerAcct>

							<SettlementInfo>
								<xsl:value-of select="'SettlementInfo'"/>
							</SettlementInfo>

							<xsl:variable name="AvgPrice1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="AvgPrice"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="AvgPrice * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="AvgPrice div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<Price>
								<xsl:value-of select="format-number($AvgPrice1,'0.#######')"/>
							</Price>

							<TradingCurrency>
								<xsl:value-of select="CurrencySymbol"/>
							</TradingCurrency>

							<TradeAmount>
								<xsl:value-of select="$varNetamount"/>
							</TradeAmount>

							<SettlementCurrency>
								<xsl:value-of select="''"/>
							</SettlementCurrency>

							<xsl:variable name="varSettlementAmount">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varNetamount"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varNetamount * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varNetamount div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<SettlementAmount>
								<xsl:value-of select="$varSettlementAmount"/>
							</SettlementAmount>

							<xsl:variable name="varOtherFees">
								<xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
							</xsl:variable>

							<xsl:variable name="varOtherFees1">
								<xsl:choose>
									<xsl:when test="SettlCurrFxRate=0">
										<xsl:value-of select="$varOtherFees"/>
									</xsl:when>
									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='M'">
										<xsl:value-of select="$varOtherFees * SettlCurrFxRate"/>
									</xsl:when>

									<xsl:when test="SettlCurrFxRate!=0 and SettlCurrFxRateCalc='D'">
										<xsl:value-of select="$varOtherFees div SettlCurrFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ExecutingBrokersAmount>
								<xsl:value-of select="$varOtherFees1"/>
							</ExecutingBrokersAmount>

							<TaxCost>
								<xsl:value-of select="''"/>
							</TaxCost>

							<NetGainLossAmount>
								<xsl:value-of select="''"/>
							</NetGainLossAmount>

							<ChargesFees>
								<xsl:value-of select="''"/>
							</ChargesFees>

							<ConsumptionTax>
								<xsl:value-of select="''"/>
							</ConsumptionTax>

							<CountryNationalFederalTax>
								<xsl:value-of select="''"/>
							</CountryNationalFederalTax>

						

							<IssueDiscountAllowance>
								<xsl:value-of select="''"/>
							</IssueDiscountAllowance>

							<Reserved1>
								<xsl:value-of select="''"/>
							</Reserved1>

							<LocalTax>
								<xsl:value-of select="''"/>
							</LocalTax>

							<LocalBrokersCommission>
								<xsl:value-of select="''"/>
							</LocalBrokersCommission>

							<Reserved2>
								<xsl:value-of select="''"/>
							</Reserved2>

							<OtherAmount>
								<xsl:value-of select="''"/>
							</OtherAmount>

							<Reserved3>
								<xsl:value-of select="''"/>
							</Reserved3>

							<Reserved4>
								<xsl:value-of select="''"/>
							</Reserved4>

							<ShippingAmount>
								<xsl:value-of select="''"/>
							</ShippingAmount>

							<SpecialConcessionsAmount>
								<xsl:value-of select="''"/>
							</SpecialConcessionsAmount>

							<StampDuty>
								<xsl:value-of select="''"/>
							</StampDuty>

							<StockExchangeTax>
								<xsl:value-of select="''"/>
							</StockExchangeTax>


							<TransferTax>
								<xsl:value-of select="''"/>
							</TransferTax>

							<TransactionTax>
								<xsl:value-of select="''"/>
							</TransactionTax>

							<ValueAddedTax>
								<xsl:value-of select="''"/>
							</ValueAddedTax>

							<WithholdingTax>
								<xsl:value-of select="''"/>
							</WithholdingTax>

							<ResultingAmount>
								<xsl:value-of select="''"/>
							</ResultingAmount>

							<WireAccountwithInstitutionBIC>
								<xsl:value-of select="''"/>
							</WireAccountwithInstitutionBIC>

							<WireAccountwithInstitutionLocalCodeType>
								<xsl:value-of select="''"/>
							</WireAccountwithInstitutionLocalCodeType>

							<WireAccountwithInstitution-LocalCode>
								<xsl:value-of select="''"/>
							</WireAccountwithInstitution-LocalCode>

							<WireAccountwithInstitution-Name>
								<xsl:value-of select="''"/>
							</WireAccountwithInstitution-Name>

							<WireBeneficiary-BIC>
								<xsl:value-of select="''"/>
							</WireBeneficiary-BIC>

							<WireBeneficiaryLocalCodeType>
								<xsl:value-of select="''"/>
							</WireBeneficiaryLocalCodeType>

							<WireBeneficiaryLocalCode>
								<xsl:value-of select="''"/>
							</WireBeneficiaryLocalCode>

							<WireBeneficiaryName>
								<xsl:value-of select="''"/>
							</WireBeneficiaryName>

							<WireBeneficiaryAccount>
								<xsl:value-of select="''"/>
							</WireBeneficiaryAccount>

							<MiscellaneousInfo>
								<xsl:value-of select="'MiscellaneousInfo'"/>
							</MiscellaneousInfo>


							<Narrative>
								<xsl:value-of select="''"/>
							</Narrative>

							<ShortSaleBuytoCover>
								<xsl:value-of select="''"/>
							</ShortSaleBuytoCover>

							<Taxable>
								<xsl:value-of select="''"/>
							</Taxable>

							<FreeCleanSettlement>
								<xsl:value-of select="''"/>
							</FreeCleanSettlement>

							<Physical>
								<xsl:value-of select="''"/>
							</Physical>

							<SpecialDelivery>
								<xsl:value-of select="''"/>
							</SpecialDelivery>

							<SplitSettlement>
								<xsl:value-of select="''"/>
							</SplitSettlement>

							<StampDutyCode>
								<xsl:value-of select="''"/>
							</StampDutyCode>

							<RealTimeGrossSettlement>
								<xsl:value-of select="''"/>
							</RealTimeGrossSettlement>

							<Beneficiary>
								<xsl:value-of select="''"/>
							</Beneficiary>

							<FXSSI>
								<xsl:value-of select="''"/>
							</FXSSI>


							<BlockSettlement>
								<xsl:value-of select="''"/>

							</BlockSettlement>

							<Tracking>
								<xsl:value-of select="''"/>
							</Tracking>

							<FXInstruction>
								<xsl:value-of select="''"/>
							</FXInstruction>

							<DebtFields>
								<xsl:value-of select="'DebtFields'"/>
							</DebtFields>

							<LateDeliveryDate>
								<xsl:value-of select="''"/>
							</LateDeliveryDate>

							<CurrentFace>
								<xsl:value-of select="''"/>
							</CurrentFace>

							<MaturityDate>
								<xsl:value-of select="''"/>
							</MaturityDate>

							<InterestRate>
								<xsl:value-of select="''"/>
							</InterestRate>

							<AccruedInterestAmount>
								<xsl:value-of select="AccruedInterest"/>
							</AccruedInterestAmount>

							<FXFields>
								<xsl:value-of select="''"/>
							</FXFields>


							<ExchangeRate>
								<xsl:value-of select="FXRate"/>
							</ExchangeRate>

							<CurrencyBought>
								<xsl:value-of select="''"/>
							</CurrencyBought>

							<AmountBought>
								<xsl:value-of select="''"/>
							</AmountBought>

							<CurrencySold>
								<xsl:value-of select="''"/>
							</CurrencySold>

							<AmountSold>
								<xsl:value-of select="''"/>
							</AmountSold>

							<NetIndicator>
								<xsl:value-of select="''"/>
							</NetIndicator>

							<ExecutingBrokerBIC1>
								<xsl:value-of select="''"/>
							</ExecutingBrokerBIC1>

							<ExecutingBrokerName1>
								<xsl:value-of select="''"/>
							</ExecutingBrokerName1>

							<DeliveryAgentBIC>
								<xsl:value-of select="''"/>
							</DeliveryAgentBIC>

							<DeliveryAgentName>
								<xsl:value-of select="''"/>
							</DeliveryAgentName>

							<IntermediaryBIC>
								<xsl:value-of select="''"/>
							</IntermediaryBIC>

							<IntermediaryName>
								<xsl:value-of select="''"/>
							</IntermediaryName>

							<IntermediaryClearingcode>
								<xsl:value-of select="''"/>
							</IntermediaryClearingcode>

							<IntermediaryFedId>
								<xsl:value-of select="''"/>
							</IntermediaryFedId>

							<ReceivingAgentBIC>
								<xsl:value-of select="''"/>
							</ReceivingAgentBIC>

							<Receivingagentname>
								<xsl:value-of select="''"/>
							</Receivingagentname>

							<ReceivingagentAcct>
								<xsl:value-of select="''"/>
							</ReceivingagentAcct>



							<ReceivingAgentSortCode>
								<xsl:value-of select="''"/>
							</ReceivingAgentSortCode>

							<ReceivingAgentClearingcode>
								<xsl:value-of select="''"/>
							</ReceivingAgentClearingcode>

							<ReceivingagentFedID>
								<xsl:value-of select="''"/>
							</ReceivingagentFedID>

							<BeneficiaryInstitutionBIC>
								<xsl:value-of select="''"/>
							</BeneficiaryInstitutionBIC>

							<BeneficiaryInstitutionName>
								<xsl:value-of select="''"/>
							</BeneficiaryInstitutionName>

							<BeneficiaryInstitutionAccountID>
								<xsl:value-of select="''"/>
							</BeneficiaryInstitutionAccountID>

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
