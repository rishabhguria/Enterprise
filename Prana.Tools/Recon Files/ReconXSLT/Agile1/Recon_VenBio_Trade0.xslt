<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<NewDataSet>

			<xsl:for-each select="//Comparision">

				<xsl:if test ="number(COL18)">

					<Comparision>

						<xsl:variable name="varPB_Name">
							<xsl:value-of select="'UBS'"/>
						</xsl:variable>

						<!--<xsl:variable name = "PB_SYMBOL_SUFFIX" >
							<xsl:value-of select="substring-after(normalize-space(COL12),' ')"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_SUFFIX">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBSuffixCode=$PB_SYMBOL_SUFFIX]/@TickerSuffixCode"/>
						</xsl:variable>-->
						

						<xsl:variable name = "PB_SYMBOL" >
							<xsl:value-of select="normalize-space(COL24)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
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
						

						<Symbol>
							<xsl:choose>
								<!--<xsl:when test="COL11='Equities'">
									<xsl:choose>-->
										<!--<xsl:when test="COL105!=''">
											<xsl:value-of select="COL105"/>
										</xsl:when>-->

										<xsl:when test="$PRANA_SYMBOL!=''">
											<xsl:value-of select="$PRANA_SYMBOL"/>
										</xsl:when>

										<!--<xsl:otherwise>
											--><!--<xsl:choose>

												<xsl:when test ="$PRANA_SYMBOL_SUFFIX !=''">
													<xsl:value-of select ="concat(substring-before(normalize-space(COL12),' '),$PRANA_SYMBOL_SUFFIX)"/>
												</xsl:when>-->

												<xsl:otherwise>
													<xsl:value-of select ="normalize-space(COL13)"/>
												</xsl:otherwise>

											<!--</xsl:choose>-->

										<!--</xsl:otherwise>--><!--
									</xsl:choose>-->
								<!--</xsl:when>-->
								<!--<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>-->
							</xsl:choose>

						</Symbol>



						<Asset>
							<xsl:value-of select ="COL1"/>
						</Asset>


						<CompanyName>
							<xsl:value-of select="normalize-space(COL24)"/>
						</CompanyName>

						<CurrencySymbol>
							<xsl:value-of select ="COL25"/>
						</CurrencySymbol>

						<Quantity>
							<xsl:choose>
								<xsl:when test ="COL18 &gt; 0">
									<xsl:value-of select ="COL18"/>
								</xsl:when>
								<xsl:when test ="COL18 &lt; 0">
									<xsl:value-of select ="COL18*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Quantity>

						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL11)"/>
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="$varSide='Buy'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:when test="$varSide='SellShort'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								<xsl:when test="$varSide='CoverShort'">
									<xsl:value-of select="'Buy to Close'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>



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
							<xsl:value-of select="COL5"/>
						</TradeDate>

						<xsl:variable name="varNotional">
							<xsl:value-of select="COL33"/>
						</xsl:variable>
						
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test ='number($varNotional) &lt; 0'>
									<xsl:value-of select ='$varNotional*-1'/>
								</xsl:when>

								<xsl:when test ='number($varNotional) &gt; 0'>
									<xsl:value-of select ='$varNotional'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name="varCommision">
							<xsl:value-of select="COL31"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($varCommision) &lt; 0'>
									<xsl:value-of select ='$varCommision*-1'/>
								</xsl:when>

								<xsl:when test ='number($varCommision) &gt; 0'>
									<xsl:value-of select ='$varCommision'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varFees">
							<xsl:value-of select="COL19"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ='number($varFees) &lt; 0'>
									<xsl:value-of select ='$varFees*-1'/>
								</xsl:when>

								<xsl:when test ='number($varFees) &gt; 0'>
									<xsl:value-of select ='$varFees'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<xsl:variable name="varStamp">
							<xsl:value-of select="COL21"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test ='number($varStamp) &lt; 0'>
									<xsl:value-of select ='$varStamp*-1'/>
								</xsl:when>

								<xsl:when test ='number($varStamp) &gt; 0'>
									<xsl:value-of select ='$varStamp'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>


						<!--<xsl:variable name="varMarkPrice">
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
								<xsl:when test="$varMarketValue &lt; 0">
									<xsl:value-of select="$varMarketValue *-1"/>
								</xsl:when>
								<xsl:when test="$varMarketValue &gt; 0">
									<xsl:value-of select="$varMarketValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<SMRequest>
							<xsl:value-of select="'TRUE'"/>
						</SMRequest>-->
					</Comparision>

				</xsl:if>

			</xsl:for-each>

		</NewDataSet>

	</xsl:template>

</xsl:stylesheet>
