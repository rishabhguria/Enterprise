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



	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name ="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($varQuantity) and not(contains(COL4,'Cash'))">
						<PositionMaster>

							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'TD Prime Services LLC'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="COL3"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>




							<xsl:variable name = "varSymbol" >
								<xsl:value-of select="normalize-space(COL5)"/>
							</xsl:variable>
							<xsl:variable name = "varCusip" >
								<xsl:value-of select="normalize-space(COL15)"/>
							</xsl:variable>


							<xsl:variable name = "PB_SUFFIX_CODE" >
								<xsl:value-of select ="''"/>
							</xsl:variable>

							<xsl:variable name ="PRANA_SUFFIX_NAME">
								<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
							</xsl:variable>



							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									
									<xsl:when test="contains(COL4,'Corp')">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="$varSymbol!=''">
										<xsl:value-of select="$varSymbol"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>

							<CUSIP>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="''"/>
									</xsl:when>

									<xsl:when test="contains(COL4,'Corp')">
										<xsl:value-of select="$varCusip"/>
									</xsl:when>

									<xsl:when test="$varSymbol!=''">
										<xsl:value-of select="$varSymbol"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</CUSIP>
								


						  <xsl:variable name="PB_FUND_NAME" select="COL1"/>
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
								<xsl:value-of select="''"/>
							</Multiplier>


							<CurrencySymbol>
								<xsl:value-of select="COL8"/>
							</CurrencySymbol>


							<ExpirationDate>
								<xsl:value-of select="''"/>
							</ExpirationDate>

							<PutOrCall>
								<xsl:value-of select="''"/>
							</PutOrCall>

							<xsl:variable name="varFxRate">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<Strategy>

								<xsl:choose>
									<xsl:when test="number($varFxRate)">
										<xsl:value-of select="$varFxRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="1"/>
									</xsl:otherwise>
								</xsl:choose>

							</Strategy>


							<CounterParty>
								<xsl:value-of select="''"/>
							</CounterParty>


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




							<xsl:variable name="UnitCost">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
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






							<xsl:variable name="SettlCurrAmt">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
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
									<xsl:with-param name="Number" select="''"/>
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
									<xsl:with-param name="Number" select="''"/>
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
									<xsl:with-param name="Number" select="''"/>
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
									<xsl:with-param name="Number" select="''"/>
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
									<xsl:with-param name="Number" select="''"/>
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
									<xsl:with-param name="Number" select="''"/>
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
									<xsl:with-param name="Number" select="COL7"/>
								</xsl:call-template>
							</xsl:variable>
							<Quantity>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="$Quantity"/>

									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="$Quantity * (-1)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>

							</Quantity>



							<xsl:variable name="MarketValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL11"/>
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
									<xsl:with-param name="Number" select="COL12"/>
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
									<xsl:with-param name="Number" select="COL9"/>
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
									<xsl:with-param name="Number" select="COL10"/>
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
									<xsl:with-param name="Number" select="''"/>
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
								<xsl:value-of select="''"/>
							</xsl:variable>
							<Side>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="'Sell short'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>

							<OriginalPurchaseDate>
								<xsl:value-of select="''"/>
							</OriginalPurchaseDate>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<CompanyName>
								<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
							</CompanyName>

						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>



							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>

							<SEDOL>
								<xsl:value-of select="''"/>
							</SEDOL>

							<FundName>
								<xsl:value-of select="''"/>
							</FundName>

							<Multiplier>
								<xsl:value-of select="0"/>
							</Multiplier>


							<CurrencySymbol>
								<xsl:value-of select="''"/>
							</CurrencySymbol>


							<ExpirationDate>
								<xsl:value-of select="''"/>
							</ExpirationDate>

							<PutOrCall>
								<xsl:value-of select="''"/>
							</PutOrCall>


							<Strategy>
								<xsl:value-of select="0"/>
							</Strategy>



							<CounterParty>
								<xsl:value-of select="''"/>
							</CounterParty>

							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>


							<NetNotionalValueBase>
								<xsl:value-of select="0"/>
							</NetNotionalValueBase>



							<UnitCost>
								<xsl:value-of select="0"/>
							</UnitCost>



							<BaseCurrency>
								<xsl:value-of select="''"/>>
							</BaseCurrency>


							<SettlCurrency>
								<xsl:value-of select="''"/>
							</SettlCurrency>


							<SettlCurrAmt>
								<xsl:value-of select="0"/>

							</SettlCurrAmt>



							<SettlCurrFxRate>

								<xsl:value-of select="0"/>

							</SettlCurrFxRate>


							<SettlPrice>
								<xsl:value-of select="0"/>

							</SettlPrice>



							<SettlementCurrencyMarkPrice>

								<xsl:value-of select="0"/>

							</SettlementCurrencyMarkPrice>



							<SettlementCurrencyCostBasis>
								<xsl:value-of select="0"/>

							</SettlementCurrencyCostBasis>

							<SettlementCurrencyMarketValue>

								<xsl:value-of select="0"/>

							</SettlementCurrencyMarketValue>


							<SettlementCurrencyTotalCost>
								<xsl:value-of select="0"/>
							</SettlementCurrencyTotalCost>


							<Quantity>
								<xsl:value-of select="0"/>

							</Quantity>

							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>


							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>



							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>



							<MarkPriceBase>
								<xsl:value-of select="0"/>
							</MarkPriceBase>


							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>


							<Side>
								<xsl:value-of select ="''"/>
							</Side>

							<OriginalPurchaseDate>
								<xsl:value-of select ="''"/>
							</OriginalPurchaseDate>

							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>

							<CompanyName>
								<xsl:value-of select ="''"/>
							</CompanyName>
							<SMRequest>
								<xsl:value-of select="'true'"/>
							</SMRequest>
						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

