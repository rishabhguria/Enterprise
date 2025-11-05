<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>

					<!-- this field use internal purpose-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:choose>
						<xsl:when test ="TaxLotState='Amemded'">
							<OrdStatus>
								<xsl:value-of select ="'R'"/>
							</OrdStatus>
						</xsl:when >
						<xsl:when test ="TaxLotState='Deleted'">
							<OrdStatus>
								<xsl:value-of select ="'D'"/>
							</OrdStatus>
						</xsl:when>
						<xsl:otherwise>
							<OrdStatus>
								<xsl:value-of select ="'N'"/>
							</OrdStatus>
						</xsl:otherwise>
					</xsl:choose >

					<xsl:choose>
						<xsl:when test ="TaxLotState='Amemded' or TaxLotState='Deleted'">
							<ExecTransType>
								<xsl:value-of select ="'0'"/>
							</ExecTransType>
						</xsl:when >						
						<xsl:otherwise>
							<ExecTransType>
								<xsl:value-of select ="'2'"/>
							</ExecTransType>
						</xsl:otherwise>
					</xsl:choose >

					<ClientOrderID>
						<xsl:value-of select ="TradeRefID"/>
					</ClientOrderID>

					<FillID>
						<xsl:value-of select ="TradeRefID"/>
					</FillID>

					<IDofOrder>
						<xsl:value-of select ="''"/>
					</IDofOrder>

					<LotNumber>
						<xsl:value-of select ="''"/>
					</LotNumber>

					<Symbol>
						<xsl:value-of select ="Symbol"/>
					</Symbol>

					<SecurityType>
						<xsl:value-of select ="''"/>
					</SecurityType>

					<SecurityCurrency>
						<xsl:value-of select ="CurrencySymbol"/>
					</SecurityCurrency>

					<SecurityDesc>
						<xsl:value-of select ="FullSecurityName"/>
					</SecurityDesc>

					<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open'">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="'B'"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>
						<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="'BC'"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close'">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="'S'"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>
						<xsl:when test="Side='Sell short' or Side='Sell to Open'">
							<Buy_Sell_Sht_Cvr>
								<xsl:value-of select ="'SS'"/>
							</Buy_Sell_Sht_Cvr>
						</xsl:when>
						<xsl:otherwise>
							<ActionCode>
								<xsl:value-of select="''"/>
							</ActionCode>
						</xsl:otherwise>
					</xsl:choose>
				

					<Open_Close>
						<xsl:value-of select="''"/>
					</Open_Close>

					<!-- fix what is available in our system-->
					<IDSource>
						<xsl:value-of select ="'SEDOL'"/>
					</IDSource>

					<SecurityID>
						<xsl:value-of select ="SEDOL"/>
					</SecurityID>

					<ISIN>
						<xsl:value-of select ="ISIN"/>
					</ISIN>

					<CUSIP>
						<xsl:value-of select ="CUSIP"/>
					</CUSIP>

					<SEDOL>
						<xsl:value-of select ="SEDOL"/>
					</SEDOL>

					<Bloomberg>
						<xsl:value-of select ="BBCode"/>
					</Bloomberg>

					<CINS>
						<xsl:value-of select ="''"/>
					</CINS>

					<WhenIssued>
						<xsl:value-of select ="''"/>
					</WhenIssued>

					<IssueDate>
						<xsl:value-of select ="''"/>
					</IssueDate>

					<MaturityDate>
						<xsl:value-of select ="''"/>
					</MaturityDate>

					<Cpn_Percent_Repo_Rate_Percent>
						<xsl:value-of select ="''"/>
					</Cpn_Percent_Repo_Rate_Percent>

					<ExecutionInt_Days>
						<xsl:value-of select ="''"/>
					</ExecutionInt_Days>

					<AccruedInt>
						<xsl:value-of select ="''"/>
					</AccruedInt>

					<FaceValue>
						<xsl:value-of select ="''"/>
					</FaceValue>

					<RepoType>
						<xsl:value-of select ="''"/>
					</RepoType>

					<RepoCurrency>
						<xsl:value-of select ="''"/>
					</RepoCurrency>

					<DayCountFract>
						<xsl:value-of select ="''"/>
					</DayCountFract>

					<RepoLoanAmt>
						<xsl:value-of select ="''"/>
					</RepoLoanAmt>

					<!-- need to be mapped, this field value will be provided by Client-->
					<Trader>
						<xsl:value-of select ="''"/>
					</Trader>

					<OrderQty>
						<xsl:value-of select="AllocatedQty"/>
					</OrderQty>

					<FillQty>
						<xsl:value-of select="AllocatedQty"/>
					</FillQty>

					<CumQty>
						<xsl:value-of select="AllocatedQty"/>
					</CumQty>

					<HairCut>
						<xsl:value-of select="''"/>
					</HairCut>

					<AvgPrice>
						<xsl:value-of select="AveragePrice"/>
					</AvgPrice>

					<FillPrice>
						<xsl:value-of select="AveragePrice"/>
					</FillPrice>

					<xsl:variable name = "varTradeMth" >
						<xsl:value-of select="substring(TradeDate,1,2)"/>
					</xsl:variable>
					<xsl:variable name = "varTradeDay" >
						<xsl:value-of select="substring(TradeDate,4,2)"/>
					</xsl:variable>
					<xsl:variable name = "varTradeYR" >
						<xsl:value-of select="substring(TradeDate,7,4)"/>
					</xsl:variable>
					<TradeDate>
						<xsl:value-of select="concat($varTradeYR,'',$varTradeMth,'',$varTradeDay)"/>
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

					<xsl:variable name = "varSettleMth" >
						<xsl:value-of select="substring(SettlementDate,1,2)"/>
					</xsl:variable>
					<xsl:variable name = "varSettleDay" >
						<xsl:value-of select="substring(SettlementDate,4,2)"/>
					</xsl:variable>
					<xsl:variable name = "varSettleYR" >
						<xsl:value-of select="substring(SettlementDate,7,4)"/>
					</xsl:variable>
					<SettlementDate>
						<xsl:value-of select="concat($varSettleYR,'',$varSettleMth,'',$varSettleDay)"/>
					</SettlementDate>

					<ExecutingUser>
						<xsl:value-of select="''"/>
					</ExecutingUser>

					<OperationsNotes_Comment>
						<xsl:value-of select="''"/>
					</OperationsNotes_Comment>

					<Account>
						<xsl:value-of select="''"/>
					</Account>

					<Fund>
						<xsl:value-of select="FundMappedName"/>
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

					<!-- need broker mapping-->
					<ExecutionBroker>
						<xsl:value-of select ="CounterParty"/>
					</ExecutionBroker>

					<!-- need mapping-->
					<ClearingAgent>
						<xsl:value-of select ="''"/>
					</ClearingAgent>

					<ContractSize>
						<xsl:value-of select ="''"/>
					</ContractSize>

					<!-- only commission-->
					<Commission>
						<xsl:value-of select="CommissionCharged"/>
					</Commission>

					<SpotFXRate>
						<xsl:value-of select ="''"/>
					</SpotFXRate>

					<FWDFXPoints>
						<xsl:value-of select ="''"/>
					</FWDFXPoints>

					<Fee>
						<xsl:value-of select ="''"/>	
					</Fee>

					<CurrencyTraded>
						<xsl:value-of select ="CurrencySymbol"/>
					</CurrencyTraded>

					<!-- company base currency-->
					<SettleCurrency>
						<xsl:value-of select ="'USD'"/>
					</SettleCurrency>

					<FX_BASERate>
						<xsl:value-of select ="''"/>
					</FX_BASERate>

					<BASE_FXRate>
						<xsl:value-of select ="''"/>
					</BASE_FXRate>

					<StrikePrice>
						<xsl:value-of select ="StrikePrice"/>
					</StrikePrice>

					<Put_Call>
						<xsl:value-of select ="PutOrCall"/>
					</Put_Call>

					<DerivativeExpiry>
						<xsl:value-of select ="''"/>
					</DerivativeExpiry>

					<SubStrategy>
						<xsl:value-of select ="''"/>
					</SubStrategy>

					<GroupOrderId>
						<xsl:value-of select ="''"/>
					</GroupOrderId>

					<GroupOrderId>
						<xsl:value-of select ="''"/>
					</GroupOrderId>

					<Penalty>
						<xsl:value-of select ="''"/>
					</Penalty>

					<Commissionturn>
						<xsl:value-of select ="'F'"/>
					</Commissionturn>

					<AllocRule>
						<xsl:value-of select ="''"/>
					</AllocRule>

					<PaymentFreq>
						<xsl:value-of select ="''"/>
					</PaymentFreq>

					<RateSource>
						<xsl:value-of select ="''"/>
					</RateSource>

					<Spread>
						<xsl:value-of select ="''"/>
					</Spread>

					<CurrentFace>
						<xsl:value-of select ="''"/>
					</CurrentFace>

					<CurrentPrincipalFactor>
						<xsl:value-of select ="''"/>
					</CurrentPrincipalFactor>

					<AccrualFactor>
						<xsl:value-of select ="''"/>
					</AccrualFactor>

					<TaxRate>
						<xsl:value-of select ="''"/>
					</TaxRate>

					<Expenses>
						<xsl:value-of select ="''"/>
					</Expenses>

					<Fees>
						<xsl:value-of select ="''"/>
					</Fees>

					<NetConsideration>
						<xsl:value-of select ="''"/>
					</NetConsideration>

					<!-- this is also for internal purpose-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>				

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
