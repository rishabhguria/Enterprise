<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<NewDataSet>

			<xsl:for-each select ="//Comparision">
				<xsl:if test ="number(COL18)">

					<Comparision>

						<xsl:variable name ="varPBName">
							<xsl:value-of select ="'UBS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL24"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL_NAME !=''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="COL32!='' and COL32!=' '">
											<xsl:value-of select ="COL32"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="$PB_SYMBOL_NAME"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>


						</Symbol>

						<xsl:variable name ="varAvgPx">
							<xsl:value-of select ="number(COL20)"/>
						</xsl:variable>


						<AvgPX>
							<xsl:choose>
								<xsl:when test ="$varAvgPx &lt;0">
									<xsl:value-of select ="$varAvgPx*-1"/>
								</xsl:when>

								<xsl:when test ="$varAvgPx &gt;0">
									<xsl:value-of select ="$varAvgPx"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>



						<TradeDate>
							<xsl:value-of select="concat(substring-before(substring-after(COL5,'/'),'/'),'/',substring-before(COL5,'/'),'/',substring-after(substring-after(COL5,'/'),'/'))"/>
						</TradeDate>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>




						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>


						<Quantity>
							<xsl:choose>
								<xsl:when test ="number(COL19)">
									<xsl:value-of select ="COL19"/>		
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'0'"/>
								</xsl:otherwise> 
							</xsl:choose>
							
						</Quantity>


						<Side>
							<xsl:choose>

								<xsl:when test ="COL11='S'">
									<xsl:value-of select ="'Sell'"/>
								</xsl:when>

								<xsl:when test ="COL11='L'">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>

							</xsl:choose>
						</Side>

						<xsl:variable name ="varComission">
							<xsl:value-of select ="COL25"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>

								<xsl:when test ="$varComission &lt;0">
									<xsl:value-of select ="$varComission*-1"/>
								</xsl:when>

								<xsl:when test ="$varComission &gt;0">
									<xsl:value-of select ="$varComission"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>

						<xsl:variable name ="varNetNotional">
							<xsl:value-of select ="number(COL21)"/>
						</xsl:variable>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test ="$varNetNotional &lt;0">
									<xsl:value-of select ="$varNetNotional*-1"/>
								</xsl:when>

								<xsl:when test ="$varNetNotional &gt;0">
									<xsl:value-of select ="$varNetNotional"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name ="varSecFees">
							<xsl:value-of select ="number(COL23)"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test ="$varSecFees &lt;0">
									<xsl:value-of select ="$varSecFees*-1"/>
								</xsl:when>

								<xsl:when test ="$varSecFees &gt;0">
									<xsl:value-of select ="$varSecFees"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>

						<xsl:variable name ="varOtherFees">
							<xsl:value-of select ="number(COL20)"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ="$varOtherFees &lt;0">
									<xsl:value-of select ="$varOtherFees*-1"/>
								</xsl:when>

								<xsl:when test ="$varOtherFees &gt;0">
									<xsl:value-of select ="$varOtherFees"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<CurrencySymbol>
							<xsl:value-of select ="COL3"/>
						</CurrencySymbol>



					</Comparision>
				</xsl:if>
			</xsl:for-each>

		</NewDataSet>
	</xsl:template>

</xsl:stylesheet>
