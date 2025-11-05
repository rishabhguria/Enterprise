<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
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
	
	<xsl:template match="/">
		
		<DocumentElement>
			
			
			<xsl:for-each select="//Comparision">
				
				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL14"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:choose>	
					
					<xsl:when test="number($varQuantity) and normalize-space(COL10)='Equity'">
							
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
																		
						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL11)"/>
						</xsl:variable>
						
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL12)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>						
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
								<xsl:variable name="PB_FUND_NAME" select="'Boothbay - Wells Fargo'"/>
						<!--<xsl:variable name="PB_FUND_NAME" select="COL2"/>-->

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<FundName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</FundName>

						<Currency>
							<xsl:value-of select="COL13"/>
						</Currency>
						
						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="varSide">
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="$varSide='Buy'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:when test="$varSide='CoverShort'">
									<xsl:value-of select="'Buy to Close'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varQuantity) &gt;0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt;0">
									<xsl:value-of select="$varQuantity * (-1)"/>
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
								<xsl:when test="$AvgPrice &gt;0">
									<xsl:value-of select="$AvgPrice"/>
								</xsl:when>
								<xsl:when test="$AvgPrice &lt;0">
									<xsl:value-of select="$AvgPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>
						
						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
							</xsl:call-template>
						</xsl:variable>
						
						<NetAmount>
							<xsl:choose>
								<xsl:when test="$NetNotionalValue &gt;0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValue &lt;0">
									<xsl:value-of select="$NetNotionalValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetAmount>
						<xsl:variable name="NetNotionalValueB">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL17"/>
							</xsl:call-template>
						</xsl:variable>

						<NetAmountBase>
							<xsl:choose>
								<xsl:when test="$NetNotionalValueB &gt;0">
									<xsl:value-of select="$NetNotionalValueB"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValueB &lt;0">
									<xsl:value-of select="$NetNotionalValueB * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetAmountBase>

						<xsl:variable name="varMarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL15)"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$varMarkPrice &gt;0">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<xsl:when test="$varMarkPrice &lt;0">
									<xsl:value-of select="$varMarkPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="varMarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test="$varMarketValue &gt;0">
									<xsl:value-of select="$varMarketValue"/>
								</xsl:when>
								<xsl:when test="$varMarketValue &lt;0">
									<xsl:value-of select="$varMarketValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<xsl:variable name="varMarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="$varMarketValueBase &gt;0">
									<xsl:value-of select="$varMarketValueBase"/>
								</xsl:when>
								<xsl:when test="$varMarketValueBase &lt;0">
									<xsl:value-of select="$varMarketValueBase * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>

						<xsl:variable name="varCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL18)"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &gt;0">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt;0">
									<xsl:value-of select="$varCommission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varSecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL21)"/>
							</xsl:call-template>
						</xsl:variable>

						<SecFee>
							<xsl:choose>
								<xsl:when test="$varSecFee &gt;0">
									<xsl:value-of select="$varSecFee"/>
								</xsl:when>
								<xsl:when test="$varSecFee &lt;0">
									<xsl:value-of select="$varSecFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>
						
						<TradeDate>
							<xsl:value-of select="COL4"/>
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select="COL5"/>
						</SettlementDate>

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

							<FundName>
								<xsl:value-of select="''"/>
							</FundName>

							<Currency>
								<xsl:value-of select="''"/>
							</Currency>

							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>

							<Side>
								<xsl:value-of select="''"/>
							</Side>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select="''"/>
							</SettlementDate>

							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>

							<NetAmount>
								<xsl:value-of select="0"/>
							</NetAmount>
							<NetAmountBase>
								<xsl:value-of select="0"/>
							</NetAmountBase>

							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>
							
							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<MarketValueBase>
								<xsl:value-of select="0"/>
							</MarketValueBase>

							<Commission>
								<xsl:value-of select="0"/>
							</Commission>

							<SecFee>
								<xsl:value-of select="0"/>
							</SecFee>

							<SMRequest>
								<xsl:value-of select="'true'"/>
							</SMRequest>
							
						</PositionMaster>
					</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>