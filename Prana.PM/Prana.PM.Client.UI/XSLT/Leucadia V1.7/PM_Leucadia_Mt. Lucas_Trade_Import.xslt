<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position" select="COL10"/>

				<xsl:if test="number($Position) and normalize-space(COL17)!='CURRENCY'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JPM'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME">
							<xsl:value-of select ="normalize-space(COL15)"/>
						</xsl:variable>

						<xsl:variable name="Asset" select="normalize-space(COL17)"/>

						<xsl:variable name="Symbol" select="normalize-space(COL2)"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME" select="substring-before(substring-after($Symbol,' '),' ')"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="PB_ROOT_NAME">
							<xsl:choose>

								<xsl:when test="$Asset='FUTURE'">
									<xsl:value-of select="normalize-space(substring($Symbol,1,2))"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="PB_YELLOW_FLAG" select ="substring($Symbol,6)"/>

						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXML/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_ROOT_NAME]/@UnderlyingCode"/>
						</xsl:variable>

						<xsl:variable name="PRANA_EXCHANGE_NAME">
							<xsl:value-of select="document('../ReconMappingXML/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_ROOT_NAME]/@ExchangeName"/>
						</xsl:variable>

						<xsl:variable name="PRANA_PRICE_MULTIPLIER">
							<xsl:value-of select="document('../ReconMappingXML/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_ROOT_NAME]/@PriceMul"/>
						</xsl:variable>

						<xsl:variable name="varUnderlying">
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME!=''">
									<xsl:value-of select="$PRANA_ROOT_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Flag" select="document('../ReconMappingXML/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@BBCode=$PB_ROOT_NAME and translate(@YellowFlag,$vLowercaseChars_CONST,$vUppercaseChars_CONST)=$PB_YELLOW_FLAG]/@ExpFlag"/>

						<xsl:variable name="FutureSymbol">
							<xsl:if test="$varUnderlying!=''">
								<xsl:choose>


									<xsl:when test="$Flag!=''">
										<xsl:value-of select="normalize-space(concat($varUnderlying,' ',substring($Symbol,4,1),substring($Symbol,3,1),$PRANA_EXCHANGE_NAME))"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="normalize-space(concat($varUnderlying,' ',substring($Symbol,3,2),$PRANA_EXCHANGE_NAME))"/>
									</xsl:otherwise>

								</xsl:choose>
							</xsl:if>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='FUTURE'">
									<xsl:choose>
										<xsl:when test="$FutureSymbol!=''">
											<xsl:value-of select="$FutureSymbol"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PB_SYMBOL_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="concat(substring-before($Symbol,' '),$PRANA_SUFFIX_NAME)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="''"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<xsl:variable name="Side" select="normalize-space(COL4)"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Side='Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:when test="$Side='Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'0'"/>
								</xsl:otherwise>

							</xsl:choose>
						</SideTagValue>

						<NetPosition>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetPosition>

						<xsl:variable name="Cost">
							<xsl:choose>
								<xsl:when test="number($PRANA_PRICE_MULTIPLIER)">
									<xsl:value-of select="number(COL11)*$PRANA_PRICE_MULTIPLIER"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="number(COL11)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test="$Cost &gt; 0">
									<xsl:value-of select="$Cost"/>
								</xsl:when>

								<xsl:when test="$Cost &lt; 0">
									<xsl:value-of select="$Cost * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<PositionStartDate>
							<xsl:value-of select="COL5"/>
						</PositionStartDate>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<TradeAttribute1>
							<xsl:choose>
								<xsl:when test="contains(COL17,'SWAP')">
									<xsl:choose>

										<xsl:when test="$PRANA_SYMBOL_NAME!=''">
											<xsl:value-of select="translate(concat($PRANA_SYMBOL_NAME,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:when>

										<xsl:when test="$Asset='FUTURE'">
											<xsl:choose>
												<xsl:when test="$FutureSymbol!=''">
													<xsl:value-of select="translate(concat($FutureSymbol,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="translate(concat($PB_SYMBOL_NAME,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>

										<xsl:when test="$Symbol!='*'">
											<xsl:value-of select="translate(concat(concat(substring-before($Symbol,' '),$PRANA_SUFFIX_NAME),'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="translate(concat($PB_SYMBOL_NAME,'_swap'),$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test="$PRANA_SYMBOL_NAME!=''">
											<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
										</xsl:when>

										<xsl:when test="$Asset='FUTURE'">
											<xsl:choose>
												<xsl:when test="$FutureSymbol!=''">
													<xsl:value-of select="$FutureSymbol"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$PB_SYMBOL_NAME"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>

										<xsl:when test="$Symbol!='*'">
											<xsl:value-of select="concat(substring-before($Symbol,' '),$PRANA_SUFFIX_NAME)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="$PB_SYMBOL_NAME"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</TradeAttribute1>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>