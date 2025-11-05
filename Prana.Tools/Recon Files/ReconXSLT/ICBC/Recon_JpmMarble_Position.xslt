<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				<xsl:if test="number(COL11)">
					<PositionMaster>

						<xsl:variable name="PB_FUND_NAME">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.Xml')/FundMapping/PB[@Name='JPM']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name="PBSuffixCode">
							<xsl:value-of select = "COL5"/>
						</xsl:variable>


						<xsl:variable name="PB_ExchangeCODE">
							<xsl:value-of select="substring-after(COL5, ' ')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Exchange">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='JPM']/SymbolData[@PBSuffixCode=$PB_ExchangeCODE]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="PB_SymbolName">
							<xsl:value-of select="substring-before(COL5,' ')"/>
						</xsl:variable>
						<xsl:variable name="PB_Symbol" select="COL5"/>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name= 'JPM']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>
						

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME !=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$PRANA_Exchange!='' or $PB_ExchangeCODE='US' ">
											<xsl:value-of select="translate(concat($PB_SymbolName,$PRANA_Exchange),'/','.')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="COL5"/>


										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>




						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>


						<Quantity>
							<xsl:choose>
								<xsl:when test="number(COL11)">
									<xsl:value-of select="COL11"/>
								</xsl:when>
								<!--<xsl:when test="number(COL9) &lt; 0">
									<xsl:value-of select="COL9*-1"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>


						<!--<PBSymbol>
							<xsl:value-of select="COL22"/>
						</PBSymbol>-->

						<xsl:variable name="varMarkPrice">
							<xsl:value-of select="COL14"/>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="number($varMarkPrice) ">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<!--<xsl:when test="number($varMarkPrice) &lt; 0">
									<xsl:value-of select="$varMarkPrice*-1"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<Side>
							<xsl:choose>
								<xsl:when test="COL12 = 'L'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="COL12 = 'S'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<MarketValue>
							<xsl:choose>
								<xsl:when test ="number(COL54)">
									<xsl:value-of select="number(COL54)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test ="number(COL69)">
									<xsl:value-of select="number(COL69)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
