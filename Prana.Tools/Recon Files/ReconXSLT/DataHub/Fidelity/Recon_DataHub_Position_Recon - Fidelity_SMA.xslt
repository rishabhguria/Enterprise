<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="MonthsCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth=01">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth=02">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth=03">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth=04">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth=05">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth=06">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth=07">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth=08">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth=09">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth=10">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth=11">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth=12">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(substring(COL1,656,1),C) or contains(substring(COL1,656,1),P)">

			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring(COL1,657,3)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(COL1,664,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(COL1,662,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(COL1,660,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(COL1,656,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(COL1,667,3),'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="$ExpiryMonth"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//Comparision">


				<xsl:variable name ="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL2"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Fidelity'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL17"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Symbol" select="COL1"/>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="COL28='EquityOption'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>

								<xsl:when test="COL28='FixedIncome'">
									<xsl:value-of select="'FixedIncome'"/>
								</xsl:when>

								<xsl:when test="COL28='PrivateEquity'">
									<xsl:value-of select="'PrivateEquity'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Asset>
							<xsl:value-of select="$Asset"/>
						</Asset>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>


								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="COL19"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<Multiplier>
							<xsl:value-of select="COL8"/>
						</Multiplier>


						<CurrencySymbol>
							<xsl:value-of select="COL9"/>
						</CurrencySymbol>


						<ExpirationDate>
							<xsl:value-of select="COL13"/>
						</ExpirationDate>

						<PutOrCall>
							<xsl:value-of select="COL14"/>
						</PutOrCall>

						<xsl:variable name="varFxRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>
						<FXRate>

							<xsl:choose>
								<xsl:when test="number($varFxRate)">
									<xsl:value-of select="$varFxRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>

						</FXRate>

						<StrikePrice>
							<xsl:value-of select="COL16"/>
						</StrikePrice>



						<UnderlyingSymbol>
							<xsl:value-of select="COL18"/>
						</UnderlyingSymbol>




						<IDCO>
							<xsl:value-of select="COL20"/>
						</IDCO>

						<SEDOL>
							<xsl:value-of select="COL21"/>
						</SEDOL>

						<OSI>
							<xsl:value-of select="COL22"/>
						</OSI>

						<CUSIP>
							<xsl:value-of select="COL23"/>
						</CUSIP>

						<Bloomberg>
							<xsl:value-of select="COL24"/>
						</Bloomberg>

						<ISINSymbol>
							<xsl:value-of select="COL25"/>
						</ISINSymbol>

						<CounterParty>
							<xsl:value-of select="COL29"/>
						</CounterParty>

						<!--<GrossNotionalValue>
							<xsl:value-of select="COL30"/>
						</GrossNotionalValue>

						<GrossNotionalValueBase>
							<xsl:value-of select="COL9"/>
						</GrossNotionalValueBase>-->

						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL32"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue "/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name="NetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL33"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$NetNotionalValueBase &gt; 0">
									<xsl:value-of select="$NetNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValueBase &lt; 0">
									<xsl:value-of select="$NetNotionalValueBase "/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetNotionalValueBase>




						<xsl:variable name="UnitCost">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL35"/>
							</xsl:call-template>
						</xsl:variable>
						<UnitCost>
							<xsl:choose>
								<xsl:when test="number($UnitCost)">
									<xsl:value-of select="$UnitCost"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</UnitCost>



						<BaseCurrency>
							<xsl:value-of select="COL36"/>
						</BaseCurrency>


						<SettlCurrency>
							<xsl:value-of select="COL37"/>
						</SettlCurrency>


						<xsl:variable name="SettlCurrAmt">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL38"/>
							</xsl:call-template>
						</xsl:variable>
						<SettlCurrAmt>

							<xsl:choose>
								<xsl:when test="number($SettlCurrAmt)">
									<xsl:value-of select="$SettlCurrAmt"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SettlCurrAmt>


						<xsl:variable name="SettlCurrFxRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL39"/>
							</xsl:call-template>
						</xsl:variable>
						<SettlCurrFxRate>

							<xsl:choose>
								<xsl:when test="number($SettlCurrFxRate)">
									<xsl:value-of select="$SettlCurrFxRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SettlCurrFxRate>


						<xsl:variable name="SettlPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL40"/>
							</xsl:call-template>
						</xsl:variable>
						<SettlPrice>

							<xsl:choose>
								<xsl:when test="number($SettlPrice)">
									<xsl:value-of select="$SettlPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SettlPrice>



						<xsl:variable name="SettlementCurrencyMarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL41"/>
							</xsl:call-template>
						</xsl:variable>
						<SettlementCurrencyMarkPrice>

							<xsl:choose>
								<xsl:when test="number($SettlPrice)">
									<xsl:value-of select="$SettlPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SettlementCurrencyMarkPrice>


						<xsl:variable name="SettlementCurrencyCostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL42"/>
							</xsl:call-template>
						</xsl:variable>
						<SettlementCurrencyCostBasis>

							<xsl:choose>
								<xsl:when test="number($SettlementCurrencyCostBasis)">
									<xsl:value-of select="$SettlementCurrencyCostBasis"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SettlementCurrencyCostBasis>

						<xsl:variable name="SettlementCurrencyMarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL43"/>
							</xsl:call-template>
						</xsl:variable>
						<SettlementCurrencyMarketValue>

							<xsl:choose>
								<xsl:when test="number($SettlementCurrencyMarketValue)">
									<xsl:value-of select="$SettlementCurrencyMarketValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SettlementCurrencyMarketValue>


						<xsl:variable name="SettlementCurrencyTotalCost">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL44"/>
							</xsl:call-template>
						</xsl:variable>
						<SettlementCurrencyTotalCost>

							<xsl:choose>
								<xsl:when test="number($SettlementCurrencyTotalCost)">
									<xsl:value-of select="$SettlementCurrencyTotalCost"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</SettlementCurrencyTotalCost>




						<xsl:variable name="Quantity">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL2"/>
							</xsl:call-template>
						</xsl:variable>
						<Quantity>
							<xsl:choose>
								<xsl:when  test="number($Quantity)">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>



						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValue>
							<xsl:choose>
								<xsl:when test="$MarketValue &gt; 0">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>
								<xsl:when test="$MarketValue &lt; 0">
									<xsl:value-of select="$MarketValue "/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValue>

						<xsl:variable name="MarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="$MarketValueBase &gt; 0">
									<xsl:value-of select="$MarketValueBase"/>
								</xsl:when>
								<xsl:when test="$MarketValueBase &lt; 0">
									<xsl:value-of select="$MarketValueBase "/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValueBase>


						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL4"/>
							</xsl:call-template>
						</xsl:variable>
						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>

								</xsl:when>
								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>


						<xsl:variable name="MarkPriceBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>
						<MarkPriceBase>
							<xsl:choose>
								<xsl:when test="$MarkPriceBase &gt; 0">
									<xsl:value-of select="$MarkPriceBase"/>

								</xsl:when>
								<xsl:when test="$MarkPriceBase &lt; 0">
									<xsl:value-of select="$MarkPriceBase * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPriceBase>

						<xsl:variable name="AvgPX">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL3"/>
							</xsl:call-template>
						</xsl:variable>
						<AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPX &gt; 0">
									<xsl:value-of select="$AvgPX"/>

								</xsl:when>
								<xsl:when test="$AvgPX &lt; 0">
									<xsl:value-of select="$AvgPX * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>


						<xsl:variable name="varSide">
							<xsl:value-of select="COL27"/>
						</xsl:variable>
						<Side>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when  test="$varSide='Buy'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when  test="$varSide='Sell'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>
										<xsl:when  test="$varSide='Buy to Close'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>
										<xsl:when  test="$varSide='Sell short'">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when  test="$varSide='Buy'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when  test="$varSide='Sell'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:when  test="$varSide='Buy to Close'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>
										<xsl:when  test="$varSide='Sell short'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<OriginalPurchaseDate>
							<xsl:value-of select="COL10"/>
						</OriginalPurchaseDate>

						<TradeDate>
							<xsl:value-of select="COL11"/>
						</TradeDate>

						<CompanyName>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</CompanyName>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

