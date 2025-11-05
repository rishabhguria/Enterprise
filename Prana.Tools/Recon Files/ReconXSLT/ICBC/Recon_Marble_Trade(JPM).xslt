<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<NewDataSet>

			<xsl:for-each select ="//Comparision">
				
				<xsl:if test ="number(COL12)">

					<Comparision>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL17"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='JPM']/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name ="PB_EXCHANGE_CODE">
							<xsl:value-of select ="substring-after(COL17,' ')"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_SUFFIX_CODE">
							<xsl:value-of select ="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='JPM']/SymbolData[@PBSuffixCode=$PB_EXCHANGE_CODE]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='JPM']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name ="NetPosition">

							<xsl:value-of select ="number(COL23)"/>

						</xsl:variable>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select ="number(COL45)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test ="$PRANA_SYMBOL_NAME !=''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:otherwise>

									<xsl:choose>

										<xsl:when test ="$PRANA_SUFFIX_CODE != ''">
											<xsl:value-of select ="concat(translate(substring-before(COL17,' '), '/', '.'),$PRANA_SUFFIX_CODE)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="COL17"/>
										</xsl:otherwise>

									</xsl:choose>

								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

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

						<TradeDate>
							<xsl:value-of select ="COL12"/>
						</TradeDate>

						<Side>

							<xsl:choose>
								<xsl:when test ="COL22='B'">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>

								<xsl:when test ="COL22='S'">
									<xsl:value-of select ="'Sell'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>

						</Side>

						<Quantity>
							<xsl:choose>
								<xsl:when test ="$NetPosition &gt; 0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>
								<xsl:when test ="$NetPosition &lt; 0">
									<xsl:value-of select ="$NetPosition*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<AvgPX>
							<xsl:choose>
								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

					</Comparision>
					
				</xsl:if>
				
			</xsl:for-each>
			
		</NewDataSet>
		
	</xsl:template>

</xsl:stylesheet>
