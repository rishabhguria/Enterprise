<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<!-- system inetrnal use-->
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>
					<!-- system inetrnal use-->
					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>
					<xsl:choose>
						<xsl:when test ="AccountName = 'JEFF Offshore'">
							<PortfolioCode>
								<xsl:value-of select ="'43001171'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'JEFF SCLP'">
							<PortfolioCode>
								<xsl:value-of select ="'43000101'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'IB Offshore'">
							<PortfolioCode>
								<xsl:value-of select ="'u743966'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'IB SCLP'">
							<PortfolioCode>
								<xsl:value-of select ="'u743965'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'PERS Offshore'">
							<PortfolioCode>
								<xsl:value-of select ="'jmp001813'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'PERS SCLP'">
							<PortfolioCode>
								<xsl:value-of select ="'jmp001797'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'HSBC SCLP'">
							<PortfolioCode>
								<xsl:value-of select ="'10385'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'HSBC Offshore'">
							<PortfolioCode>
								<xsl:value-of select ="'10443'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'HSBC 25394'">
							<PortfolioCode>
								<xsl:value-of select ="'25394'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'HSBC 25393'">
							<PortfolioCode>
								<xsl:value-of select ="'25393'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'HSBC 26206'">
							<PortfolioCode>
								<xsl:value-of select ="'26206'"/>
							</PortfolioCode>
						</xsl:when>	
						<xsl:when test ="AccountName = 'Schwab SCLP'">
							<PortfolioCode>
								<xsl:value-of select ="'26004389'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'AMTD SCLP'">
							<PortfolioCode>
								<xsl:value-of select ="'amtdsclp'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'JPM SCLP'">
							<PortfolioCode>
								<xsl:value-of select ="'61160271'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'JPM SCO'">
							<PortfolioCode>
								<xsl:value-of select ="'61160272'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'HSBC 10409'">
							<PortfolioCode>
								<xsl:value-of select ="'10409'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'HSBC 9776'">
							<PortfolioCode>
								<xsl:value-of select ="'9776'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'HSBC 10143'">
							<PortfolioCode>
								<xsl:value-of select ="'10413'"/>
							</PortfolioCode>
						</xsl:when>
						<xsl:when test ="AccountName = 'HSBC 9950 DRP'">
							<PortfolioCode>
								<xsl:value-of select ="'9950'"/>
							</PortfolioCode>
						</xsl:when>						
						<!--<xsl:when test ="FundName = 'JPM SCLP Custody'">
							<PortfolioCode>
								<xsl:value-of select ="'42503008'"/>
							</PortfolioCode>
						</xsl:when>-->
						<xsl:otherwise>
							<PortfolioCode>
								<xsl:value-of select ="''"/>
							</PortfolioCode>
						</xsl:otherwise>
					</xsl:choose>

					
					<!--<PortfolioCode>
						<xsl:value-of select="FundName"/>
					</PortfolioCode>-->	
					
					
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

					<xsl:variable name ="varCheckSymbolUnderlying">
						<xsl:value-of select ="substring-before(Symbol,'-')"/>
					</xsl:variable>

					<xsl:variable name ="varSymbol">
						<xsl:value-of select="Symbol"/>
					</xsl:variable>
					<xsl:variable name="PBSecType">
						<xsl:value-of select="document('../ReconMappingXml/MutualFundMapping.xml')/SymbolMapping/PB[@Name='Axys']/SymbolData[@PranaSymbol=$varSymbol]/@SecType"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$PBSecType != ''">
							<SecType>
								<xsl:value-of select ="$PBSecType"/>
							</SecType>
						</xsl:when >
						<xsl:when test="Asset = 'PrivateEquity'">
							<SecType>
								<xsl:value-of select ="'cbus'"/>
							</SecType>
						</xsl:when >
						<xsl:when test="Asset = 'Equity'">
							<SecType>
								<xsl:value-of select ="'csus'"/>
							</SecType>
						</xsl:when >
						<xsl:when test="Asset='EquityOption' and PutOrCall = 'CALL'">
							<SecType>
								<xsl:value-of select ="'clus'"/>
							</SecType>
						</xsl:when >
						<xsl:when test="Asset='EquityOption' and PutOrCall = 'PUT'">
							<SecType>
								<xsl:value-of select ="'ptus'"/>
							</SecType>
						</xsl:when >
						<xsl:otherwise>
							<SecType>
								<xsl:value-of select ="''"/>
							</SecType>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:variable name ="varEqtSymbol">
						<xsl:choose>
							<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/W') != ''">
								<xsl:value-of select ="concat(translate(Symbol,'.','/'),'S')"/>
							</xsl:when>							
							<!--
							Change	Date :23-12-2011 , 
							For  DTE/PA and  DTE/PC
                            Will Display dte a, dte c
							-->
							<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/P') != ''">
								<xsl:value-of select ="concat(substring-before(Symbol,'/P'),' ',substring-after(Symbol,'/P'))"/>
							</xsl:when>
							<!--End Change-->
							<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/W') = ''">
								<xsl:value-of select ="Symbol"/>
							</xsl:when >
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:choose>
						<!--For US Equities use ticker symbol and add ' us' to it. For example MSFT is msft us-->
						<!--varCheckSymbolUnderlying is used to check whether Symbol is US Equity Symbol or international symbol-->
						<!--<xsl:when test ="Asset = 'Equity' and CurrencySymbol = 'USD'">-->
						<xsl:when test ="$varEqtSymbol='BMO V-TC'">
							<SecuritySymbol>
								<xsl:value-of select ="'bmo.pr.v'"/>
							</SecuritySymbol>
						</xsl:when>						
						<xsl:when test ="$varEqtSymbol='CM I-TC'">
							<SecuritySymbol>
								<xsl:value-of select ="'cm.pr.i'"/>
							</SecuritySymbol>
						</xsl:when>
						<!-- Internation Symbol handling, here we send SEDOL Symbol -->
						<!-- http://jira.nirvanasolutions.com:8080/browse/SONOMA-250 -->
						<xsl:when test ="Asset = 'Equity' and $varCheckSymbolUnderlying != ''">
							<SecuritySymbol>								
								<xsl:value-of select="translate(SEDOL,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
							</SecuritySymbol>
						</xsl:when>
						<xsl:when test ="Asset = 'Equity' and $varCheckSymbolUnderlying = '' and ($varEqtSymbol='BRK.A' or $varEqtSymbol ='BRK.B')">
							<SecuritySymbol>
								<xsl:value-of select="translate(translate($varEqtSymbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST),'.','')"/>
							</SecuritySymbol>
						</xsl:when>
						<xsl:when test ="Asset = 'Equity' and $varCheckSymbolUnderlying = '' and $varEqtSymbol='PE/PA'">
							<SecuritySymbol>
								<xsl:value-of select="translate(translate($varEqtSymbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST),'/',' ')"/>
							</SecuritySymbol>
						</xsl:when>
					
						<xsl:when test ="Asset = 'Equity' and $varCheckSymbolUnderlying = ''">
							<SecuritySymbol>
								<!--<xsl:value-of select="concat(translate(Symbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST), ' us')"/>-->
								<xsl:value-of select="translate($varEqtSymbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
							</SecuritySymbol>
						</xsl:when>
						<!--For US Equity Options use ticker symbol and add '+' to it. For example MSFT is msft us-->
						<xsl:when test ="Asset = 'EquityOption'">
							<SecuritySymbol>
								<xsl:value-of select="translate(OSIOptionSymbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
								<!--<xsl:value-of select="Symbol"/>-->
							</SecuritySymbol>
						</xsl:when>
						<!--Otherwise use ticker symbol and translate to lower case. For example MSFT is msft us-->
						<xsl:otherwise>
							<SecuritySymbol>
								<xsl:value-of select="translate(Symbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
							</SecuritySymbol>
						</xsl:otherwise>
					</xsl:choose>

					<TradeDate>
						<xsl:value-of select ="translate(TradeDate,'/','')"/>
					</TradeDate>

					<!--column 7 -->

					<SettleDate>
						<xsl:value-of select ="translate(SettlementDate,'/','')"/>
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
						<xsl:value-of select ="'caus'"/>
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
						<xsl:value-of select ="''"/>
					</MarkToMarket>

					<xsl:variable name ="varAvgPrice">
						<xsl:choose>
							<xsl:when test ="AveragePrice != 0">
								<xsl:value-of select = 'format-number(AveragePrice, "###.0000000")'/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<TradeAmount>
						<xsl:value-of select="concat('@',$varAvgPrice)"/>
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

					<Exchange>
						<xsl:value-of select ="'5'"/>
					</Exchange>

					<!--<ExchangeFee>
						<xsl:value-of select ="MiscFees"/>
					</ExchangeFee>-->

					<xsl:choose>
						<xsl:when test ="StampDuty != 0">
							<ExchangeFee>
								<xsl:value-of select = 'format-number(StampDuty, "###.0000000")'/>
							</ExchangeFee>
						</xsl:when>
						<xsl:otherwise>
							<ExchangeFee>
								<xsl:value-of select ="0"/>
							</ExchangeFee>
						</xsl:otherwise>
					</xsl:choose>

					<!--<commission>
						<xsl:value-of select="CommissionCharged"/>
					</commission>-->

					<xsl:choose>
						<xsl:when test ="CommissionCharged != 0">
							<commission>
								<xsl:value-of select = 'format-number(CommissionCharged, "###.0000000")'/>
							</commission>
						</xsl:when>
						<xsl:otherwise>
							<commission>
								<xsl:value-of select ="0"/>
							</commission>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test ="string-length(CounterParty) &lt; 4">
							<Broker>
								<xsl:value-of select="concat(translate(CounterParty,$vUppercaseChars_CONST,$vLowercaseChars_CONST),'kr')"/>
							</Broker>
						</xsl:when>
						<xsl:otherwise>
							<Broker>
								<xsl:value-of select="translate(CounterParty,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
							</Broker>
						</xsl:otherwise>
					</xsl:choose>

					<ImpliedComm>
						<xsl:value-of select ="'n'"/>
					</ImpliedComm>

					<!--<OtherFees>
						<xsl:value-of select ="OtherBrokerFee"/>
					</OtherFees>-->

					<xsl:variable name ="varOtherFee">
						<xsl:choose>
							<xsl:when test ="Asset = 'PrivateEquity'">
								<xsl:value-of select = "0"/>
							</xsl:when >
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test ="TransactionLevy != 0">
										<xsl:value-of select = 'format-number(TransactionLevy, "###.0000000")'/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose >
					</xsl:variable>


					<OtherFees>
						<xsl:choose>
							<xsl:when test ="(AccountName = 'JEFF SCLP' or AccountName = 'JPM SCLP' or AccountName = 'JPM SCO') ">
								<xsl:value-of select ="format-number(($varOtherFee+15),'###.0000000')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="$varOtherFee"/>
							</xsl:otherwise>
						</xsl:choose>
					</OtherFees>
					<!--<xsl:choose>
						<xsl:when test ="Asset = 'PrivateEquity'">
							<OtherFees>
								<xsl:value-of select = "0"/>
							</OtherFees>
						</xsl:when >
						<xsl:otherwise>
							<xsl:choose>
								<xsl:when test ="TransactionLevy != 0">
									<OtherFees>
										<xsl:value-of select = 'format-number(TransactionLevy, "###.0000000")'/>
									</OtherFees>
								</xsl:when>
								<xsl:otherwise>
									<OtherFees>
										<xsl:value-of select ="0"/>
									</OtherFees>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose >-->
					
					

					<CommPurpose>
						<xsl:value-of select ="''"/>
					</CommPurpose>

					<!-- Column 29-->

					<Pledge>
						<xsl:value-of select ="'n'"/>
					</Pledge>

					<LotLocation>
						<xsl:value-of select ="'253'"/>
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
						<xsl:value-of select="''"/>
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

					<RecordDate>
						<xsl:value-of select ="''"/>
					</RecordDate>

					<ReclaimAmount>
						<xsl:value-of select ="''"/>
					</ReclaimAmount>

					<Strategy>
						<xsl:value-of select ="''"/>
					</Strategy>

					<Comment4>
						<xsl:value-of select ="''"/>
					</Comment4>

					<IncomeAccount>
						<xsl:value-of select ="''"/>
					</IncomeAccount>

					<AccrualAccount>
						<xsl:value-of select ="''"/>
					</AccrualAccount>

					<DivAccrualMethod>
						<xsl:value-of select ="''"/>
					</DivAccrualMethod>

					<PerfContributionOrWithdrawal>
						<xsl:value-of select ="''"/>
					</PerfContributionOrWithdrawal>

					<!-- system inetrnal use-->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>
				<xsl:if test ="Asset='PrivateEquity'">
					<ThirdPartyFlatFileDetail>
						<!-- system inetrnal use-->
						<TaxLotState>
							<xsl:value-of select="TaxLotState"/>
						</TaxLotState>
						<!-- system inetrnal use-->
						<RowHeader>
							<xsl:value-of select ="'false'"/>
						</RowHeader>

						<xsl:choose>
							<xsl:when test ="AccountName = 'JEFF Offshore'">
								<PortfolioCode>
									<xsl:value-of select ="'43001171'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'JEFF SCLP'">
								<PortfolioCode>
									<xsl:value-of select ="'43000101'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'IB Offshore'">
								<PortfolioCode>
									<xsl:value-of select ="'u743966'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'IB SCLP'">
								<PortfolioCode>
									<xsl:value-of select ="'u743965'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'PERS Offshore'">
								<PortfolioCode>
									<xsl:value-of select ="'jmp001813'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'PERS SCLP'">
								<PortfolioCode>
									<xsl:value-of select ="'jmp001797'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'HSBC SCLP'">
								<PortfolioCode>
									<xsl:value-of select ="'10385'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'HSBC Offshore'">
								<PortfolioCode>
									<xsl:value-of select ="'10443'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'HSBC 25394'">
								<PortfolioCode>
									<xsl:value-of select ="'25394'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'HSBC 25393'">
								<PortfolioCode>
									<xsl:value-of select ="'25393'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'HSBC 26206'">
								<PortfolioCode>
									<xsl:value-of select ="'26206'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'Schwab SCLP'">
								<PortfolioCode>
									<xsl:value-of select ="'26004389'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'AMTD SCLP'">
								<PortfolioCode>
									<xsl:value-of select ="'amtdsclp'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'JPM SCLP'">
								<PortfolioCode>
									<xsl:value-of select ="'61160271'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'JPM SCO'">
								<PortfolioCode>
									<xsl:value-of select ="'61160272'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'HSBC 10409'">
								<PortfolioCode>
									<xsl:value-of select ="'10409'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:when test ="AccountName = 'JPM SCLP Custody'">
								<PortfolioCode>
									<xsl:value-of select ="'42503008'"/>
								</PortfolioCode>
							</xsl:when>
							<xsl:otherwise>
								<PortfolioCode>
									<xsl:value-of select ="''"/>
								</PortfolioCode>
							</xsl:otherwise>
						</xsl:choose>


						<!--<PortfolioCode>
						<xsl:value-of select="FundName"/>
					</PortfolioCode>-->


						<!--   Side     -->
						<TranCode>
							<xsl:value-of select="'in'"/>
						</TranCode>

						<!--   Side End    -->
						<Comment>
							<xsl:value-of select ="''"/>
						</Comment>

						<xsl:variable name ="varCheckSymbolUnderlying">
							<xsl:value-of select ="substring-before(Symbol,'-')"/>
						</xsl:variable>

						<xsl:variable name ="varSymbol">
							<xsl:value-of select="Symbol"/>
						</xsl:variable>
						<xsl:variable name="PBSecType">
							<xsl:value-of select="document('../ReconMappingXml/MutualFundMapping.xml')/SymbolMapping/PB[@Name='Axys']/SymbolData[@PranaSymbol=$varSymbol]/@SecType"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PBSecType != ''">
								<SecType>
									<xsl:value-of select ="$PBSecType"/>
								</SecType>
							</xsl:when >
							<xsl:when test="Asset = 'PrivateEquity'">
								<SecType>
									<xsl:value-of select ="'cbus'"/>
								</SecType>
							</xsl:when >
							<xsl:when test="Asset = 'Equity'">
								<SecType>
									<xsl:value-of select ="'csus'"/>
								</SecType>
							</xsl:when >
							<xsl:when test="Asset='EquityOption' and PutOrCall = 'CALL'">
								<SecType>
									<xsl:value-of select ="'clus'"/>
								</SecType>
							</xsl:when >
							<xsl:when test="Asset='EquityOption' and PutOrCall = 'PUT'">
								<SecType>
									<xsl:value-of select ="'ptus'"/>
								</SecType>
							</xsl:when >
							<xsl:otherwise>
								<SecType>
									<xsl:value-of select ="''"/>
								</SecType>
							</xsl:otherwise>
						</xsl:choose>
						
						
						<!--<SecType>
							<xsl:value-of select ="'cvus'"/>
						</SecType>-->					
						

					<xsl:variable name ="varEqtSymbol">
						<xsl:choose>
							<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/W') != ''">
								<xsl:value-of select ="concat(translate(Symbol,'.','/'),'S')"/>
							</xsl:when >
							<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/P') != ''">
								<xsl:value-of select ="concat(substring-before(Symbol,'/P'),' ',substring-after(Symbol,'/P'))"/>
							</xsl:when>
							<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/W') = ''">
								<xsl:value-of select ="Symbol"/>
							</xsl:when >
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:choose>
						<!--For US Equities use ticker symbol and add ' us' to it. For example MSFT is msft us-->
						<!--varCheckSymbolUnderlying is used to check whether Symbol is US Equity Symbol or international symbol-->
						<!--<xsl:when test ="Asset = 'Equity' and CurrencySymbol = 'USD'">-->

						<!--<xsl:when test ="SecuritySymbol = 'CM/PI-TC'">
							<SecuritySymbol>

								<xsl:value-of select='cm.pr.i'/>
							</SecuritySymbol>
						</xsl:when>-->
						
						
						
						
						<xsl:when test ="Asset = 'Equity' and $varCheckSymbolUnderlying != ''">
							<SecuritySymbol>

								<xsl:value-of select="SEDOL"/>
							</SecuritySymbol>
						</xsl:when>
						<xsl:when test ="Asset = 'Equity' and $varCheckSymbolUnderlying = ''">
							<!--<xsl:choose>
								<xsl:when test ="$varEqtSymbol=">
									
								</xsl:when>
							</xsl:choose>-->
							
							
							<SecuritySymbol>
								<!--<xsl:value-of select="concat(translate(Symbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST), ' us')"/>-->
								<xsl:value-of select="translate($varEqtSymbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
								</SecuritySymbol>
							</xsl:when>
							<!--For US Equity Options use ticker symbol and add '+' to it. For example MSFT is msft us-->
							<xsl:when test ="Asset = 'EquityOption'">
								<SecuritySymbol>
									<xsl:value-of select="translate(OSIOptionSymbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
									<!--<xsl:value-of select="Symbol"/>-->
								</SecuritySymbol>
							</xsl:when>
							<!--Otherwise use ticker symbol and translate to lower case. For example MSFT is msft us-->
							<xsl:otherwise>
								<SecuritySymbol>
									<xsl:value-of select="translate(Symbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
								</SecuritySymbol>
							</xsl:otherwise>
						</xsl:choose>

						<TradeDate>
							<xsl:value-of select ="translate(TradeDate,'/','')"/>
						</TradeDate>

						<!--column 7 -->

						<SettleDate>
							<xsl:value-of select ="translate(SettlementDate,'/','')"/>
						</SettleDate>

						<OriginalCostDate>
							<xsl:value-of select ="''"/>
						</OriginalCostDate>

						<Quantity>
							<xsl:value-of select="1"/>
						</Quantity>

						<CloseMath>
							<xsl:value-of select ="''"/>
						</CloseMath>

						<VersusDate>
							<xsl:value-of select="''"/>
						</VersusDate>

						<SourceType>
							<xsl:value-of select ="'caus'"/>
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
							<xsl:value-of select ="''"/>
						</MarkToMarket>

						<xsl:variable name ="varAvgPrice">
							<xsl:choose>
								<xsl:when test ="AveragePrice != 0">
									<xsl:value-of select = 'format-number(AveragePrice, "###.0000000")'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<TradeAmount>
							<xsl:value-of select= 'format-number(TransactionLevy*-1, "###.0000000")'/>
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

						<Exchange>
							<xsl:value-of select ="'5'"/>
						</Exchange>

						<!--<ExchangeFee>
						<xsl:value-of select ="MiscFees"/>
					</ExchangeFee>-->

						<xsl:choose>
							<xsl:when test ="StampDuty != 0">
								<ExchangeFee>
									<xsl:value-of select = 'format-number(StampDuty, "###.0000000")'/>
								</ExchangeFee>
							</xsl:when>
							<xsl:otherwise>
								<ExchangeFee>
									<xsl:value-of select ="0"/>
								</ExchangeFee>
							</xsl:otherwise>
						</xsl:choose>

						<!--<commission>
						<xsl:value-of select="CommissionCharged"/>
					</commission>-->

						<xsl:choose>
							<xsl:when test ="CommissionCharged != 0">
								<commission>
									<xsl:value-of select = 'format-number(CommissionCharged, "###.0000000")'/>
								</commission>
							</xsl:when>
							<xsl:otherwise>
								<commission>
									<xsl:value-of select ="0"/>
								</commission>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="string-length(CounterParty) &lt; 4">
								<Broker>
									<xsl:value-of select="concat(translate(CounterParty,$vUppercaseChars_CONST,$vLowercaseChars_CONST),'kr')"/>
								</Broker>
							</xsl:when>
							<xsl:otherwise>
								<Broker>
									<xsl:value-of select="translate(CounterParty,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
								</Broker>
							</xsl:otherwise>
						</xsl:choose>

						<ImpliedComm>
							<xsl:value-of select ="'n'"/>
						</ImpliedComm>

						<OtherFees>
							<xsl:value-of select ="0"/>
						</OtherFees>

						<CommPurpose>
							<xsl:value-of select ="''"/>
						</CommPurpose>

						<!-- Column 29-->

						<Pledge>
							<xsl:value-of select ="'n'"/>
						</Pledge>

						<LotLocation>
							<xsl:value-of select ="'253'"/>
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
							<xsl:value-of select="''"/>
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

						<RecordDate>
							<xsl:value-of select ="''"/>
						</RecordDate>

						<ReclaimAmount>
							<xsl:value-of select ="''"/>
						</ReclaimAmount>

						<Strategy>
							<xsl:value-of select ="''"/>
						</Strategy>

						<Comment4>
							<xsl:value-of select ="''"/>
						</Comment4>

						<IncomeAccount>
							<xsl:value-of select ="''"/>
						</IncomeAccount>

						<AccrualAccount>
							<xsl:value-of select ="''"/>
						</AccrualAccount>

						<DivAccrualMethod>
							<xsl:value-of select ="''"/>
						</DivAccrualMethod>

						<PerfContributionOrWithdrawal>
							<xsl:value-of select ="''"/>
						</PerfContributionOrWithdrawal>

						<!-- system inetrnal use-->
						<EntityID>
							<xsl:value-of select="EntityID"/>
						</EntityID>
					</ThirdPartyFlatFileDetail>
				</xsl:if>

			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
