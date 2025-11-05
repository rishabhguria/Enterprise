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
				
				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:choose>
					<xsl:when test="number($Quantity) and normalize-space(COL23) !='Cash'">
						
						<PositionMaster>
							
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>
							
							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="normalize-space(COL7)"/>
							</xsl:variable>
							
							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>
							
							<xsl:variable name="varAsset">
								<xsl:value-of select="normalize-space(COL6)"/>
							</xsl:variable>
							
						
							
							<xsl:variable name="varSymbol">
								<xsl:value-of select="normalize-space(COL6)"/>
							</xsl:variable>
							
							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									
									<xsl:when test="$varSymbol !=''">
										<xsl:value-of select="$varSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>
							
							<!--<xsl:variable name="PB_FUND_NAME" select="'BT Global - Direct Holdings'"/>-->
							<xsl:variable name="PB_FUND_NAME" select="COL2"/>
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
							
							<Side>
								<xsl:choose>
									<xsl:when test="$Quantity &gt;0">
										<xsl:value-of select="'L'"/>
									</xsl:when>
									<xsl:when test="$Quantity &lt;0">
										<xsl:value-of select="'S'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>
							
							<Quantity>
								<xsl:choose>
									<xsl:when test="number($Quantity)">
										<xsl:value-of select="$Quantity"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>
							
							<xsl:variable name="AvgPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL11)"/>
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
							
							<xsl:variable name="varMarkPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL9)"/>
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
									<xsl:with-param name="Number" select="normalize-space(COL12)"/>
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
														
							<Currency>
								<xsl:value-of select="''"/>
							</Currency>
							
							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>
							
							<SettlementDate>
								<xsl:value-of select="''"/>
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

							<Side>
								<xsl:value-of select="''"/>
							</Side>

							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>

							<MarkPrice>
								<xsl:value-of select="0"/>
							</MarkPrice>

							<MarketValue>
								<xsl:value-of select="0"/>
							</MarketValue>

							<Currency>
								<xsl:value-of select="''"/>
							</Currency>

							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<SettlementDate>
								<xsl:value-of select="''"/>
							</SettlementDate>

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