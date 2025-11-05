<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				<xsl:if test ="number(COL48) and COL10='VMOB'">
					
					<Comparision>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Test'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL11"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_SUFFIX_NAME" >
							<xsl:value-of select ="substring-after(COL27,'.')"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<Symbol>

							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>

									<xsl:choose>

										<xsl:when test ="COL27!='*'">
											<xsl:choose>
												<xsl:when test ="$PRANA_SUFFIX_NAME !=''">
													<xsl:value-of select ="concat(substring-before(COL27,'.'),$PRANA_SUFFIX_NAME)"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select ="substring-before(normalize-space(COL27),'.')"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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


						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name ="varMarkPrice">
							<xsl:value-of select ="number(COL44)"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="$varMarkPrice &lt;0">
									<xsl:value-of select ="$varMarkPrice*-1"/>
								</xsl:when>

								<xsl:when test ="$varMarkPrice &gt;0">
									<xsl:value-of select ="$varMarkPrice"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<TradeDate>
							<xsl:value-of select ="concat(substring-before(substring-after(COL8,'/'),'/'),'/',substring-before(COL8,'/'),'/',substring-after(substring-after(COL8,'/'),'/'))"/>
						</TradeDate>

						<xsl:variable name ="varMarketValue">
							<xsl:value-of select ="number(COL46)"/>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test ="$varMarketValue &lt;0">
									<xsl:value-of select ="$varMarketValue*-1"/>
								</xsl:when>

								<xsl:when test ="$varMarketValue &gt;0">
									<xsl:value-of select ="$varMarketValue"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<xsl:variable name ="varQuantity">
							<xsl:value-of select ="number(COL48)"/>
						</xsl:variable>
						
						<Quantity>
							<xsl:choose>
								<xsl:when test ="$varQuantity &lt;0">
									<xsl:value-of select ="$varQuantity*-1"/>
								</xsl:when>

								<xsl:when test ="$varQuantity &gt;0">
									<xsl:value-of select ="$varQuantity"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<Side>
							<xsl:choose>
								<xsl:when test ="$varQuantity &lt;0">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>

								<xsl:when test ="$varQuantity &gt;0">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>
						
						
						
						

					</Comparision>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
