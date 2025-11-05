<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}

	</msxsl:script>

	

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
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



	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL70"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity) and COL64!='Cash'">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'BNY'"/>
							</xsl:variable>

							<xsl:variable name = "PB_SYMBOL_NAME" >
								<xsl:value-of select ="COL67"/>
							</xsl:variable>


							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>


							<xsl:variable name = "PB_SUFFIX_CODE" >
								<xsl:value-of select ="''"/>
							</xsl:variable>

							<xsl:variable name ="PRANA_SUFFIX_NAME">
								<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
							</xsl:variable>

							<xsl:variable name="Asset">
								<xsl:choose>
									<xsl:when test="COL64 ='Option'">
										<xsl:value-of select="'EquityOption'"/>
									</xsl:when>

									<xsl:when test="COL64 ='Futures'">
										<xsl:value-of select="'Future'"/>
									</xsl:when>
									<xsl:when test="COL64 ='Synthetic Equity'">
										<xsl:value-of select="'EquitySwap'"/>
									</xsl:when>
									<xsl:when test="normalize-space(COL64) ='ForwardFX'">
										<xsl:value-of select="'ForwardFX'"/>
									</xsl:when>
									<xsl:when test="COL64 ='Debt Instruments'">
										<xsl:value-of select="'DebtInstruments'"/>
									</xsl:when>

									<xsl:when test="COL64 ='Mutual Fund'">
										<xsl:value-of select="'MutualFund'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'Equity'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							
							
							
							
							<xsl:variable name="varFWD">
								<xsl:value-of select="substring-before(COL86,'-')"/>
							</xsl:variable>
							<xsl:variable name="varmonth">
								<xsl:value-of select="substring(substring-after(COL86,'-'),1,2)"/>
							</xsl:variable>
							<xsl:variable name="varday">
								<xsl:value-of select="substring(substring-after(COL86,'-'),3,2)"/>
							</xsl:variable>
							<xsl:variable name="varyear">
								<xsl:value-of select="substring(substring-after(COL86,'-'),5,4)"/>
							</xsl:variable>
							<xsl:variable name="varFWDSymbol">
								<xsl:value-of select="concat($varFWD,'Fwd','  ',$varmonth,'/',$varday,'/',$varyear)"/>
							</xsl:variable>
							
							


							<xsl:variable name="varSymbol">
								<xsl:choose>
									<xsl:when test="$Asset ='EquityOption'">
										<xsl:value-of select="COL86"/>
									</xsl:when>
									<xsl:when test="$Asset ='ForwardFX'">
										<xsl:value-of select="$varFWDSymbol"/>
									</xsl:when>
									<xsl:when test="$Asset ='Future'">
										<xsl:value-of select="COL90"/>
									</xsl:when>
									<xsl:when test="$Asset ='EquitySwap'">
										<xsl:value-of select="COL86"/>
									</xsl:when>
									
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="$Asset ='Equity'">
												<xsl:choose>
													
													<xsl:when test="COL82 ='USD'and $Asset ='Equity'">
														<xsl:value-of select="concat(substring-before(COL90,' '),' ','US')"/>
													</xsl:when>
													
													<xsl:when test="COL82 ='USD'and $Asset ='Equity'">
														<xsl:value-of select="concat(COL86,' ','US')"/>
													</xsl:when>

													<xsl:when test="COL82 ='AUD'and $Asset ='Equity'">
														<xsl:value-of select="concat(substring-before(COL86,' '),' ','AU')"/>
													</xsl:when>

													<xsl:when test="COL82 ='JPY'and $Asset ='Equity'">
														<xsl:value-of select="concat(substring-before(COL86,' '),' ','JP')"/>
													</xsl:when>

													<xsl:when test="COL82 ='CAD'and $Asset ='Equity'">
														<xsl:value-of select="concat(substring-before(COL86,' '),' ','CN')"/>
													</xsl:when>												
													
													<xsl:when test="contains(COL86,' ')">
														<xsl:value-of select="concat(substring-before(COL86,' '),' ',COL83)"/>
													</xsl:when>
													
													<xsl:otherwise>
														<xsl:value-of select="COL67"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:when>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="Symbol" select="COL86"/>
							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>


									<xsl:when test="$varSymbol!=''">
										<xsl:value-of select="$varSymbol"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="COL67"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>

							


							<xsl:variable name="PB_FUND_NAME" select="COL62"/>
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>
							<FundName>
								<xsl:choose>
									<xsl:when test ="$PRANA_FUND_NAME!=''">
										<xsl:value-of select ="$PRANA_FUND_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="$PB_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</FundName>


							<xsl:variable name="varSide">
								<xsl:value-of select="COL6"/>
							</xsl:variable>

							<Side>
								<xsl:choose>
									<xsl:when test="$Asset ='EquityOption'">
										<xsl:choose>

											<xsl:when test="$Quantity &gt; 0">
												<xsl:value-of select="'Buy to Open'"/>
											</xsl:when>

											<xsl:when test="$Quantity &lt; 0">
												<xsl:value-of select="'Sell to Open'"/>
											</xsl:when>

											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>

										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>

											<xsl:when test="$Quantity &gt; 0">
												<xsl:value-of select="'Buy'"/>
											</xsl:when>

											<xsl:when test="$Quantity &lt; 0">
												<xsl:value-of select="'Sell short'"/>
											</xsl:when>

											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>

										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>

							</Side>

							<CurrencySymbol>
								<xsl:value-of select="COL82"/>
							</CurrencySymbol>

							<xsl:variable name="varSettQuantity">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL44"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varBuyQuantity">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL42"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varQuantity">
								<xsl:choose>
									<xsl:when test="COL64= 'ForwardFX' and COL41 ='USD'">
										<xsl:value-of select="$varSettQuantity * -1"/>
									</xsl:when>
									<xsl:when test="COL64= 'ForwardFX' and COL41 !='USD'">
										<xsl:value-of select="$varBuyQuantity"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$Quantity"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<Quantity>
								<xsl:choose>
									<xsl:when test="number($varQuantity)">
										<xsl:value-of select="$varQuantity"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>

							<xsl:variable name="AvgPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<AvgPX>
								<xsl:choose>
									<xsl:when test="$AvgPrice &gt; 0">
										<xsl:value-of select="$AvgPrice"/>

									</xsl:when>
									<xsl:when test="$AvgPrice &lt; 0">
										<xsl:value-of select="$AvgPrice * (-1)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>

								</xsl:choose>
							</AvgPX>


							<xsl:variable name="NetNotionalValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<NetNotionalValue>
								<xsl:choose>
									<xsl:when test="$NetNotionalValue &gt; 0">
										<xsl:value-of select="$NetNotionalValue"/>
									</xsl:when>
									<xsl:when test="$NetNotionalValue &lt; 0">
										<xsl:value-of select="$NetNotionalValue * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValue>

							<xsl:variable name="NetNotionalValueBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<NetNotionalValueBase>
								<xsl:choose>
									<xsl:when test="$NetNotionalValueBase &gt; 0">
										<xsl:value-of select="$NetNotionalValueBase"/>
									</xsl:when>
									<xsl:when test="$NetNotionalValueBase &lt; 0">
										<xsl:value-of select="$NetNotionalValueBase * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValueBase>

							<xsl:variable name="MarketValueBase">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL14"/>
								</xsl:call-template>
							</xsl:variable>

							<MarketValueBase>
								<xsl:choose>
									<xsl:when test="number($MarketValueBase)">
										<xsl:value-of select="$MarketValueBase"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValueBase>

							<xsl:variable name="MarketValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL73"/>
								</xsl:call-template>
							</xsl:variable>

							<MarketValue>
								<xsl:choose>
									<xsl:when test="number($MarketValue)">
										<xsl:value-of select="$MarketValue"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarketValue>


							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>


							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>



							<xsl:variable name="MarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL77"/>
								</xsl:call-template>
							</xsl:variable>
							<MarkPrice>
								<xsl:choose>
									<xsl:when test="$MarkPrice &gt; 0">
										<xsl:value-of select="$MarkPrice"/>

									</xsl:when>
									<xsl:when test="$MarkPrice &lt; 0">
										<xsl:value-of select="$MarkPrice * (-1)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>

								</xsl:choose>
							</MarkPrice>


							<xsl:variable name="varMarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<MarkPriceBase>
								<xsl:choose>
									<xsl:when test="$varMarkPrice &gt; 0">
										<xsl:value-of select="$varMarkPrice"/>
									</xsl:when>
									<xsl:when test="$varMarkPrice &lt; 0">
										<xsl:value-of select="$varMarkPrice * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</MarkPriceBase>



							<xsl:variable name="UnitCost">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<UnitCost>
								<xsl:choose>
									<xsl:when test="$UnitCost &gt; 0">
										<xsl:value-of select="$UnitCost"/>
									</xsl:when>
									<xsl:when test="$UnitCost &lt; 0">
										<xsl:value-of select="$UnitCost * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</UnitCost>

							<xsl:variable name ="FXRate">
								<xsl:value-of select ="''"/>
							</xsl:variable>

							<FXRate>
								<xsl:choose>
									<xsl:when test ="$FXRate ">
										<xsl:value-of select ="$FXRate"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</FXRate>

							<CompanyName>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</CompanyName>


							<SMRequest>
								<xsl:value-of select="'true'"/>
							</SMRequest>
						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>

							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>


							<CUSIP>
								<xsl:value-of select="''"/>
							</CUSIP>

							<ISIN>
								<xsl:value-of select="''"/>
							</ISIN>

							<IDCOOptionSymbol>
								<xsl:value-of select="''"/>
							</IDCOOptionSymbol>

							<FundName>
								<xsl:value-of select="''"/>
							</FundName>

							<FXRate>
								<xsl:value-of select="'0'"/>
							</FXRate>

							<Side>
								<xsl:value-of select="''"/>
							</Side>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>


							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>

							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>


							<NetNotionalValueBase>
								<xsl:value-of select="0"/>
							</NetNotionalValueBase>


							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>
							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select ="''"/>
							</SettlementDate>

							<MarkPriceBase>
								<xsl:value-of select="0"/>
							</MarkPriceBase>
							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>


							<UnitCost>
								<xsl:value-of select="0"/>
							</UnitCost>


							<CompanyName>
								<xsl:value-of select="''"/>
							</CompanyName>
							<SMRequest>
								<xsl:value-of select="''"/>
							</SMRequest>
						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


