<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

				<!--for system use only-->


				<!--for system internal use-->
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

				<OrdStatus>

					<xsl:value-of select="'OrdStatus'"/>

				</OrdStatus>

				<ExecTransType>

					<xsl:value-of select="'ExecTransType'"/>

				</ExecTransType>

				<ClientOrderID>
					<xsl:value-of select="'ClientOrderID'"/>
				</ClientOrderID>

				<FillID>
					<xsl:value-of select="'FillID'"/>
				</FillID>

				<IDofOrdFill>
					<xsl:value-of select="'IDofOrdFill'"/>
				</IDofOrdFill>

				<LotNumber>
					<xsl:value-of select="'LotNumber'"/>
				</LotNumber>

				<Symbol>

					<xsl:value-of select="'Symbol'"/>

				</Symbol>

				<SecurityType>

					<xsl:value-of select="'SecurityType'"/>

				</SecurityType>

				<SecurityCurrency>
					<xsl:value-of select="'SecurityCurrency'"/>
				</SecurityCurrency>

				<SecurityDesc>
					<xsl:value-of select="'SecurityDesc'"/>
				</SecurityDesc>

				<BuySellShtCvr>

					<xsl:value-of select="'BuySellShtCvr'"/>

				</BuySellShtCvr>

				<OpenClose>
					<xsl:value-of select="'OpenClose'"/>
				</OpenClose>

				<IDSource>


					<xsl:value-of select="'IDSource'"/>

				</IDSource>

				<SecurityID>

					<xsl:value-of select="'SecurityID'"/>

				</SecurityID>


				<ISIN>
					<xsl:value-of select="'ISIN'"/>
				</ISIN>

				<CUSIP>
					<xsl:value-of select="'CUSIP'"/>
				</CUSIP>

				<SEDOL>
					<xsl:value-of select="'SEDOL'"/>
				</SEDOL>

				<Bloomberg>
					<xsl:value-of select="'Bloomberg'"/>
				</Bloomberg>

				<CINS>
					<xsl:value-of select="'CINS'"/>
				</CINS>

				<WhenIssued>
					<xsl:value-of select="'WhenIssued'"/>
				</WhenIssued>

				<IssueDate>
					<xsl:value-of select="'IssueDate'"/>
				</IssueDate>

				<MaturityDate>
					<xsl:value-of select="'MaturityDate'"/>
				</MaturityDate>

				<CpnRepoRate>
					<xsl:value-of select="'CpnRepoRate'"/>
				</CpnRepoRate>

				<ExecutionIntDays>
					<xsl:value-of select="'ExecutionIntDays'"/>
				</ExecutionIntDays>

				<AccruedInt>
					<xsl:value-of select="'AccruedInt'"/>
				</AccruedInt>

				<FaceValue>
					<xsl:value-of select="'FaceValue'"/>
				</FaceValue>

				<RepoType>
					<xsl:value-of select="'RepoType'"/>
				</RepoType>

				<RepoCurrency>
					<xsl:value-of select="'RepoCurrency'"/>

				</RepoCurrency>

				<DayCountFract>
					<xsl:value-of select="'DayCountFract'"/>
				</DayCountFract>

				<RepoLoanAmt>
					<xsl:value-of select="'RepoLoanAmt'"/>
				</RepoLoanAmt>

				<Trader>
					<xsl:value-of select="'Trader'"/>

				</Trader>

				<OrderQty>

					<xsl:value-of select="'OrderQty'"/>

				</OrderQty>

				<FillQty>
					<xsl:value-of select="'FillQty'"/>
				</FillQty>

				<CumQty>
					<xsl:value-of select="'CumQty'"/>
				</CumQty>

				<HairCut>
					<xsl:value-of select="'HairCut'"/>
				</HairCut>

				<AvgPrice>

					<xsl:value-of select="'AvgPrice'"/>

				</AvgPrice>

				<FillPrice>

					<xsl:value-of select="'FillPrice'"/>

				</FillPrice>

				<TradeDate>
					<xsl:value-of select="'TradeDate'"/>
				</TradeDate>

				<TradeTime>
					<xsl:value-of select="'TradeTime'"/>
				</TradeTime>

				<ExecutionDate>
					<xsl:value-of select="'ExecutionDate'"/>
				</ExecutionDate>

				<ExecutionTime>
					<xsl:value-of select="'ExecutionTime'"/>
				</ExecutionTime>

				<SettlementDate>
					<xsl:value-of select="'SettlementDate'"/>

				</SettlementDate>

				<ExecutingUser>
					<xsl:value-of select="'ExecutingUser'"/>
				</ExecutingUser>

				<TradeNotesCommt>
					<xsl:value-of select="'TradeNotesCommt'"/>
				</TradeNotesCommt>

				<Account>
					<xsl:value-of select="'Account'"/>
				</Account>


				<Fund>

					<xsl:value-of select="'Fund'"/>

				</Fund>


				<SubFund>
					<xsl:value-of select="'SubFund'"/>
				</SubFund>

				<AllocationCode>
					<xsl:value-of select="'AllocationCode'"/>
				</AllocationCode>

				<StrategyCode>
					<xsl:value-of select="'StrategyCode'"/>
				</StrategyCode>


				<ExecutionBroker>

					<xsl:value-of select="'ExecutionBroker'"/>

				</ExecutionBroker>



				<ClearingAgent>
					<xsl:value-of select="'ClearingAgent'"/>
				</ClearingAgent>

				<ContractSize>
					<xsl:value-of select="'ContractSize'"/>
				</ContractSize>

				<Commissions>
					<xsl:value-of select="'Commissions'"/>
				</Commissions>

				<SpotFXRate>
					<xsl:value-of select="'SpotFXRate'"/>
				</SpotFXRate>

				<FWDFXPoints>
					<xsl:value-of select="'FWDFXPoints'"/>
				</FWDFXPoints>

				<Fee >
					<xsl:value-of select="'Fee'"/>
				</Fee>

				<CurrencyTraded>
					<xsl:value-of select="'CurrencyTraded'"/>
				</CurrencyTraded>

				<SettleCurrency>
					<xsl:value-of select="'SettlCurrency'"/>
				</SettleCurrency>

				<FXBaseRate>
					<xsl:value-of select="'FXBaseRate'"/>
				</FXBaseRate>


				<BaseFXRate>
					<xsl:value-of select="'BaseFXRate'"/>
				</BaseFXRate>

				<StrikePrice>
					<xsl:value-of select="'StrikePrice'"/>
				</StrikePrice>

				<PutorCall>
					<xsl:value-of select="'PutorCall'"/>
				</PutorCall>

				<DerivativeExpiry>
					<xsl:value-of select="'DerivativeExpiry'"/>
				</DerivativeExpiry>

				<SubStrategy>
					<xsl:value-of select="'SubStrategy'"/>
				</SubStrategy>

				<GroupOrderID>
					<xsl:value-of select="'GroupOrderID'"/>
				</GroupOrderID>

				<Penalty>
					<xsl:value-of select="'Penalty'"/>
				</Penalty>

				<CommissionTurn>
					<xsl:value-of select="'CommissionTurn'"/>
				</CommissionTurn>

				<AllocRule>
					<xsl:value-of select="'AllocRule'"/>
				</AllocRule>

				<PaymentFrequency>
					<xsl:value-of select="'PaymentFrequency'"/>
				</PaymentFrequency>

				<RateSource>
					<xsl:value-of select="'RateSource'"/>
				</RateSource>

				<Spread >
					<xsl:value-of select="'Spread'"/>
				</Spread>

				<CurrentFace>
					<xsl:value-of select="'CurrentFace'"/>
				</CurrentFace>

				<CurrentPrincipalFactor>
					<xsl:value-of select="'CurrentPrincipalFactor'"/>
				</CurrentPrincipalFactor>

				<AccrualFactor>
					<xsl:value-of select="'AccrualFactor'"/>
				</AccrualFactor>

				<TaxRate>
					<xsl:value-of select="'TaxRate'"/>
				</TaxRate>
				<xsl:value-of select="''"/>
				<Expenses>
					<xsl:value-of select="'Expenses'"/>
				</Expenses>

				<Fees>
					<xsl:value-of select="'Fees'"/>
				</Fees>

				<PostCommAndFeesOn>
					<xsl:value-of select="'PostCommAndFeesOn'"/>
				</PostCommAndFeesOn>


				<InitImpliedCommissionFlag>
					<xsl:value-of select="'InitImpliedCommissionFlag'"/>
				</InitImpliedCommissionFlag>

				<TransactionType>
					<xsl:value-of select="'TransactionType'"/>
				</TransactionType>

				<MasterConfirmType>

					<xsl:value-of select="'MasterConfirmType'"/>
				</MasterConfirmType>

				<MatrixTerm>
					<xsl:value-of select="'MatrixTerm'"/>
				</MatrixTerm>
				<EMInternalSeqNo>
					<xsl:value-of select="'EMInternalSeqNo'"/>
				</EMInternalSeqNo>

				<ObjectivePrice>
					<xsl:value-of select="'ObjectivePrice'"/>
				</ObjectivePrice>
				<MarketPrice>
					<xsl:value-of select="'MarketPrice'"/>
				</MarketPrice>

				<StopPrice>
					<xsl:value-of select="'StopPrice'"/>
				</StopPrice>

				<NetConsideration>
					<xsl:value-of select="'NetConsideration'"/>
				</NetConsideration>

				<FixingDate>
					<xsl:value-of select="'FixingDate'"/>
				</FixingDate>


				<DeliveryInstructions>
					<xsl:value-of select="'DeliveryInstructions'"/>
				</DeliveryInstructions>

				<ForceMatchID>
					<xsl:value-of select="'ForceMatchID'"/>
				</ForceMatchID>

				<ForceMatchType>
					<xsl:value-of select="'ForceMatchType'"/>
				</ForceMatchType>

				<ForceMatchNotes>
					<xsl:value-of select="'ForceMatchNotes'"/>
				</ForceMatchNotes>

				<CommissionRateforAllocation>
					<xsl:value-of select="'CommissionRateforAllocation'"/>
				</CommissionRateforAllocation>

				<CommissionAmountforFill>
					<xsl:value-of select="'CommissionAmountforFill'"/>
				</CommissionAmountforFill>

				<ExpenseAmountforFill>
					<xsl:value-of select="'ExpenseAmountforFill'"/>
				</ExpenseAmountforFill>

				<FeeAmountforFill>
					<xsl:value-of select="'FeeAmountforFill'"/>
				</FeeAmountforFill>

				<StandardStrategy>
					<xsl:value-of select="'StandardStrategy'"/>
				</StandardStrategy>

				<StrategyLinkName>
					<xsl:value-of select="'StrategyLinkName'"/>
				</StrategyLinkName>

				<StrategyGroup>
					<xsl:value-of select="'StrategyGroup'"/>
				</StrategyGroup>

				<FillFXSettleAmount>
					<xsl:value-of select="'FillFXSettleAmount'"/>
				</FillFXSettleAmount>



				<Reserved>
					<xsl:value-of select="'Reserved'"/>
				</Reserved>

				<DealAttributes>
					<xsl:value-of select="'DealAttributes'"/>
				</DealAttributes>

				<FinanceLeg>
					<xsl:value-of select="'FinanceLeg'"/>
				</FinanceLeg>

				<PerformanceLeg>
					<xsl:value-of select="'PerformanceLeg'"/>
				</PerformanceLeg>

				<Attributes>
					<xsl:value-of select="'Attributes'"/>
				</Attributes>

				<DealSymbol>
					<xsl:value-of select="'DealSymbol'"/>
				</DealSymbol>

				<InitialMarginType>
					<xsl:value-of select="'InitialMarginType'"/>
				</InitialMarginType>

				<InitialMarginAmount>
					<xsl:value-of select="'InitialMarginAmount'"/>
				</InitialMarginAmount>

				<InitialMarginCurrency>
					<xsl:value-of select="'InitialMarginCurrency'"/>
				</InitialMarginCurrency>

				<ConfirmStatus>
					<xsl:value-of select="'ConfirmStatus'"/>
				</ConfirmStatus>

				<CounterpartyCPAssignBroker>
					<xsl:value-of select="'CounterpartyCPAssignBroker'"/>
				</CounterpartyCPAssignBroker>

				<TraderNotes>
					<xsl:value-of select="'TraderNotes'"/>
				</TraderNotes>

				<ConvertPricetoSettleCCY>
					<xsl:value-of select="'ConvertPricetoSettleCCY'"/>
				</ConvertPricetoSettleCCY>

				<BondCouponType>
					<xsl:value-of select="'BondCouponType'"/>
				</BondCouponType>

				<GenericFeesEnabled>
					<xsl:value-of select="'GenericFeesEnabled'"/>
				</GenericFeesEnabled>

				<GenericFeesListing>
					<xsl:value-of select="'GenericFeesListing'"/>
				</GenericFeesListing>

				<OrderLevelAttributes>
					<xsl:value-of select="'OrderLevelAttributes'"/>
				</OrderLevelAttributes>




				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>


			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<xsl:if test="AccountName = 'OCA Iguana'">

					<ThirdPartyFlatFileDetail>

						<RowHeader>
							<xsl:value-of select ="'true'"/>
						</RowHeader>

						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>


						<OrdStatus>
							<xsl:choose>
								<xsl:when test="TaxLotState='Allocated'">
									<xsl:value-of select="'N'"/>
								</xsl:when>
								<xsl:when test="TaxLotState='Amended' ">
									<xsl:value-of select="'R'"/>
								</xsl:when>
								<xsl:when test="TaxLotState='Deleted' ">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrdStatus>

						<ExecTransType>
							<xsl:choose>
								<xsl:when test="TaxLotState='Deleted' or TaxLotState='Amended' ">
									<xsl:value-of select="'0'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'2'"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExecTransType>

						<ClientOrderID>
							<xsl:value-of select="concat('A',EntityID)"/>
						</ClientOrderID>

						<FillID>
							<xsl:value-of select="concat('A',EntityID)"/>
						</FillID>

						<IDofOrdFill>
							<xsl:value-of select="''"/>
						</IDofOrdFill>

						<LotNumber>
							<xsl:value-of select="''"/>
						</LotNumber>

						<Symbol>
							<!--<xsl:choose>
								<xsl:when test="BBCode!=''">
									<xsl:value-of select="BBCode"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="RIC"/>
								</xsl:otherwise>
							</xsl:choose>-->
							<xsl:value-of select="''"/>
						</Symbol>

						<SecurityType>
							<xsl:choose>
								<xsl:when test="Asset='Equity'">
									<xsl:value-of select="'CS'"/>
								</xsl:when>
								<xsl:when test="Asset='EquityOption'">
									<xsl:value-of select="'OPT'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecurityType>

						<SecurityCurrency>
							<xsl:value-of select="SettlCurrency"/>
						</SecurityCurrency>

						<SecurityDesc>
							<xsl:value-of select="FullSecurityName"/>
						</SecurityDesc>

						<BuySellShtCvr>
							<xsl:choose>
								<xsl:when test="Side='Buy'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="Side='Buy to Close'">
									<xsl:value-of select="'BC'"/>
								</xsl:when>
								<xsl:when test="Side='Sell short'">
									<xsl:value-of select="'SS'"/>
								</xsl:when>
								<xsl:when test="Side='Sell'">
									<xsl:value-of select="'S'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</BuySellShtCvr>

						<OpenClose>
							<xsl:value-of select="''"/>
						</OpenClose>

						<IDSource>
							<xsl:choose>
								<xsl:when test="BBCode!=''">
									<xsl:value-of select="'BLOOMBERG'"/>
								</xsl:when>
								<xsl:when test="SEDOL!=''">
									<xsl:value-of select="'SEDOL'"/>
								</xsl:when>
								<xsl:when test="ISIN!=''">
									<xsl:value-of select="'ISIN'"/>
								</xsl:when>
								<xsl:when test="CUSIP!=''">
									<xsl:value-of select="'CUSIP'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDSource>

						<SecurityID>
							<xsl:choose>
								<xsl:when test="BBCode!=''">
									<xsl:value-of select="BBCode"/>
								</xsl:when>
								<xsl:when test="SEDOL!=''">
									<xsl:value-of select="SEDOL"/>
								</xsl:when>
								<xsl:when test="CUSIP!=''">
									<xsl:value-of select="CUSIP"/>
								</xsl:when>
								<xsl:when test="ISIN!=''">
									<xsl:value-of select="ISIN"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecurityID>


						<ISIN>
							<xsl:value-of select="ISIN"/>
						</ISIN>

						<CUSIP>
							<xsl:value-of select="CUSIP"/>
						</CUSIP>

						<SEDOL>
							<xsl:value-of select="SEDOL"/>
						</SEDOL>

						<Bloomberg>
							<xsl:value-of select="BBCode"/>
						</Bloomberg>

						<CINS>
							<xsl:value-of select="''"/>
						</CINS>

						<WhenIssued>
							<xsl:value-of select="''"/>
						</WhenIssued>

						<IssueDate>
							<xsl:value-of select="''"/>
						</IssueDate>

						<MaturityDate>
							<xsl:value-of select="''"/>
						</MaturityDate>

						<CpnRepoRate>
							<xsl:value-of select="''"/>
						</CpnRepoRate>

						<ExecutionIntDays>
							<xsl:value-of select="''"/>
						</ExecutionIntDays>

						<AccruedInt>
							<xsl:value-of select="''"/>
						</AccruedInt>

						<FaceValue>
							<xsl:value-of select="''"/>
						</FaceValue>

						<RepoType>
							<xsl:value-of select="''"/>
						</RepoType>

						<RepoCurrency>
							<xsl:value-of select="''"/>

						</RepoCurrency>

						<DayCountFract>
							<xsl:value-of select="''"/>
						</DayCountFract>

						<RepoLoanAmt>
							<xsl:value-of select="''"/>
						</RepoLoanAmt>

						<Trader>
							<xsl:value-of select="'IAN'"/>

						</Trader>

						<OrderQty>
							<xsl:choose>
								<xsl:when test="number(AllocatedQty)">
									<xsl:value-of select="AllocatedQty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrderQty>

						<FillQty>
							<xsl:value-of select="''"/>
						</FillQty>

						<CumQty>
							<xsl:value-of select="''"/>
						</CumQty>

						<HairCut>
							<xsl:value-of select="''"/>
						</HairCut>

						<AvgPrice>
							<xsl:choose>
								<xsl:when test="number(AveragePrice)">
									<xsl:value-of select="AveragePrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPrice>

						<FillPrice>
							<xsl:choose>
								<xsl:when test="number(AveragePrice)">
									<xsl:value-of select="AveragePrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</FillPrice>

						<TradeDate>
							<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
						</TradeDate>

						<TradeTime>
							<xsl:value-of select="''"/>
						</TradeTime>

						<ExecutionDate>
							<xsl:value-of select="''"/>
						</ExecutionDate>

						<ExecutionTime>
							<xsl:value-of select="''"/>
						</ExecutionTime>

						<SettlementDate>
							<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>

						</SettlementDate>

						<ExecutingUser>
							<xsl:value-of select="''"/>
						</ExecutingUser>

						<TradeNotesCommt>
							<xsl:value-of select="''"/>
						</TradeNotesCommt>

						<Account>
							<xsl:value-of select="''"/>
						</Account>

						<xsl:variable name="PB_NAME" select="'Iguana'"/>

						<xsl:variable name = "PRANA_FUND_NAME">
							<xsl:value-of select="AccountName"/>
						</xsl:variable>

						<xsl:variable name ="THIRDPARTY_FUND_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
						</xsl:variable>

						<Fund>
							<xsl:choose>
								<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
									<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fund>


						<SubFund>
							<xsl:value-of select="''"/>
						</SubFund>

						<AllocationCode>
							<xsl:value-of select="''"/>
						</AllocationCode>

						<StrategyCode>
							<xsl:value-of select="''"/>
						</StrategyCode>

						<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

						<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
						</xsl:variable>

						<xsl:variable name="Broker">
							<xsl:choose>
								<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
									<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<ExecutionBroker>

							<xsl:value-of select="$Broker"/>

						</ExecutionBroker>

						<xsl:variable name="THIRDPARTY_COUNTERPARTY">
							<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
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


						<ClearingAgent>
							<xsl:value-of select="$BrokerName"/>
						</ClearingAgent>

						<ContractSize>
							<xsl:value-of select="''"/>
						</ContractSize>

						<Commissions>
							<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
						</Commissions>

						<SpotFXRate>
							<xsl:value-of select="''"/>
						</SpotFXRate>

						<FWDFXPoints>
							<xsl:value-of select="''"/>
						</FWDFXPoints>

						<Fee >
							<xsl:value-of select="MiscFees + ClearingFee + OtherBrokerFee+ TaxOnCommissions"/>
						</Fee>

						<CurrencyTraded>
							<xsl:value-of select="''"/>
						</CurrencyTraded>

						<SettleCurrency>
							<xsl:value-of select="SettlCurrency"/>
						</SettleCurrency>

						<FXBaseRate>
							<xsl:value-of select="''"/>
						</FXBaseRate>


						<BaseFXRate>
							<xsl:value-of select="''"/>
						</BaseFXRate>

						<StrikePrice>
							<xsl:value-of select="''"/>
						</StrikePrice>

						<PutorCall>
							<xsl:value-of select="''"/>
						</PutorCall>

						<DerivativeExpiry>
							<xsl:value-of select="''"/>
						</DerivativeExpiry>

						<SubStrategy>
							<xsl:value-of select="''"/>
						</SubStrategy>

						<GroupOrderID>
							<xsl:value-of select="''"/>
						</GroupOrderID>

						<Penalty>
							<xsl:value-of select="''"/>
						</Penalty>

						<CommissionTurn>
							<xsl:value-of select="''"/>
						</CommissionTurn>

						<AllocRule>
							<xsl:value-of select="''"/>
						</AllocRule>

						<PaymentFrequency>
							<xsl:value-of select="''"/>
						</PaymentFrequency>

						<RateSource>
							<xsl:value-of select="''"/>
						</RateSource>

						<Spread >
							<xsl:value-of select="''"/>
						</Spread>

						<CurrentFace>
							<xsl:value-of select="''"/>
						</CurrentFace>

						<CurrentPrincipalFactor>
							<xsl:value-of select="''"/>
						</CurrentPrincipalFactor>

						<AccrualFactor>
							<xsl:value-of select="''"/>
						</AccrualFactor>

						<TaxRate>
							<xsl:value-of select="''"/>
						</TaxRate>
						<xsl:value-of select="''"/>
						<Expenses>
							<xsl:value-of select="''"/>
						</Expenses>

						<Fees>
							<xsl:value-of select="''"/>
						</Fees>

						<PostCommAndFeesOn>
							<xsl:value-of select="''"/>
						</PostCommAndFeesOn>


						<InitImpliedCommissionFlag>
							<xsl:value-of select="''"/>
						</InitImpliedCommissionFlag>

						<TransactionType>
							<xsl:value-of select="''"/>
						</TransactionType>

						<MasterConfirmType>

							<xsl:value-of select="''"/>
						</MasterConfirmType>

						<MatrixTerm>
							<xsl:value-of select="''"/>
						</MatrixTerm>
						<EMInternalSeqNo>
							<xsl:value-of select="''"/>
						</EMInternalSeqNo>

						<ObjectivePrice>
							<xsl:value-of select="''"/>
						</ObjectivePrice>
						<MarketPrice>
							<xsl:value-of select="''"/>
						</MarketPrice>

						<StopPrice>
							<xsl:value-of select="''"/>
						</StopPrice>

						<NetConsideration>
							<xsl:value-of select="''"/>
						</NetConsideration>

						<FixingDate>
							<xsl:value-of select="''"/>
						</FixingDate>


						<DeliveryInstructions>
							<xsl:value-of select="''"/>
						</DeliveryInstructions>

						<ForceMatchID>
							<xsl:value-of select="''"/>
						</ForceMatchID>

						<ForceMatchType>
							<xsl:value-of select="''"/>
						</ForceMatchType>

						<ForceMatchNotes>
							<xsl:value-of select="''"/>
						</ForceMatchNotes>

						<CommissionRateforAllocation>
							<xsl:value-of select="''"/>
						</CommissionRateforAllocation>

						<CommissionAmountforFill>
							<xsl:value-of select="''"/>
						</CommissionAmountforFill>

						<ExpenseAmountforFill>
							<xsl:value-of select="''"/>
						</ExpenseAmountforFill>

						<FeeAmountforFill>
							<xsl:value-of select="''"/>
						</FeeAmountforFill>

						<StandardStrategy>
							<xsl:value-of select="''"/>
						</StandardStrategy>

						<StrategyLinkName>
							<xsl:value-of select="''"/>
						</StrategyLinkName>

						<StrategyGroup>
							<xsl:value-of select="''"/>
						</StrategyGroup>

						<FillFXSettleAmount>
							<xsl:value-of select="''"/>
						</FillFXSettleAmount>

						<Reserved>
							<xsl:value-of select="''"/>
						</Reserved>

						<DealAttributes>
							<xsl:value-of select="''"/>
						</DealAttributes>

						<FinanceLeg>
							<xsl:value-of select="''"/>
						</FinanceLeg>

						<PerformanceLeg>
							<xsl:value-of select="''"/>
						</PerformanceLeg>

						<Attributes>
							<xsl:value-of select="''"/>
						</Attributes>

						<DealSymbol>
							<xsl:value-of select="''"/>
						</DealSymbol>

						<InitialMarginType>
							<xsl:value-of select="''"/>
						</InitialMarginType>

						<InitialMarginAmount>
							<xsl:value-of select="''"/>
						</InitialMarginAmount>

						<InitialMarginCurrency>
							<xsl:value-of select="''"/>
						</InitialMarginCurrency>

						<ConfirmStatus>
							<xsl:value-of select="''"/>
						</ConfirmStatus>

						<CounterpartyCPAssignBroker>
							<xsl:value-of select="''"/>
						</CounterpartyCPAssignBroker>

						<TraderNotes>
							<xsl:value-of select="''"/>
						</TraderNotes>

						<ConvertPricetoSettleCCY>
							<xsl:value-of select="''"/>
						</ConvertPricetoSettleCCY>

						<BondCouponType>
							<xsl:value-of select="''"/>
						</BondCouponType>

						<GenericFeesEnabled>
							<xsl:value-of select="''"/>
						</GenericFeesEnabled>

						<GenericFeesListing>
							<xsl:value-of select="''"/>
						</GenericFeesListing>

						<OrderLevelAttributes>
							<xsl:value-of select="''"/>
						</OrderLevelAttributes>





						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>

					</ThirdPartyFlatFileDetail>
				</xsl:if>
			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>