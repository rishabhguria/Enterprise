<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<NewDataSet>

			<xsl:for-each select="//Comparision">

				<xsl:if test ="number(COL22) and COL11!='Cash and Equivalents'">

					<Comparision>

						<xsl:variable name="varPB_Name">
							<xsl:value-of select="'MERLIN'"/>
						</xsl:variable>
						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPB_Name]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name = "PB_SYMBOL_SUFFIX" >
							<xsl:value-of select="substring-after(normalize-space(COL12),' ')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_SUFFIX">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBSuffixCode=$PB_SYMBOL_SUFFIX]/@TickerSuffixCode"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL" >
							<xsl:value-of select="normalize-space(COL13)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL]/@PranaSymbol"/>
						</xsl:variable>

						<!--<SEDOL>
							<xsl:value-of select="COL105"/>
							--><!--<xsl:choose>
								<xsl:when test="COL105=SEDOL">
									<xsl:value-of select="COL105"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
								
							</xsl:choose>--><!--
							
						</SEDOL>-->

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="COL11='Options'">
									<xsl:value-of select="concat(COL12,'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>

						<Symbol>
							<xsl:choose>
								<xsl:when test="COL11='Equities'">
									<xsl:choose>
										<!--<xsl:when test="COL105!=''">
											<xsl:value-of select="COL105"/>
										</xsl:when>-->
										
										<xsl:when test="$PRANA_SYMBOL!=''">
											<xsl:value-of select="$PRANA_SYMBOL"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:choose>

												<xsl:when test ="$PRANA_SYMBOL_SUFFIX !=''">
													<xsl:value-of select ="concat(substring-before(normalize-space(COL12),' '),$PRANA_SYMBOL_SUFFIX)"/>
												</xsl:when>

												<xsl:otherwise>
													<xsl:value-of select ="normalize-space(COL12)"/>
												</xsl:otherwise>

											</xsl:choose>

										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						
						</Symbol>

					

						<!--<Asset>
							<xsl:value-of select ="COL6"/>
						</Asset>-->


						<CompanyName>
							<xsl:value-of select="normalize-space(COL13)"/>
						</CompanyName>

						<CurrencySymbol>
							<xsl:value-of select ="COL26"/>
						</CurrencySymbol>

						<Quantity>
							<xsl:choose>
								<xsl:when test ="number(COL22)">
									<xsl:value-of select ="COL22"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
									
							</xsl:choose> 
						
						</Quantity>

						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="$varSide='Long'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$varSide='Short'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="varMarkPrice">
							<xsl:value-of select="COL38"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$varMarkPrice &lt; 0">
									<xsl:value-of select="$varMarkPrice *-1"/>
								</xsl:when>
								<xsl:when test="$varMarkPrice &gt; 0">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="varMarketValue">
							<xsl:value-of select="COL46"/>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test="$varMarketValue">
									<xsl:value-of select="$varMarketValue"/>
								</xsl:when>
								<!--<xsl:when test="$varMarketValue &gt; 0">
									<xsl:value-of select="$varMarketValue"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<SMRequest>
							<xsl:value-of select="'TRUE'"/>
						</SMRequest>
					</Comparision>

				</xsl:if>

			</xsl:for-each>

		</NewDataSet>

	</xsl:template>

</xsl:stylesheet>
