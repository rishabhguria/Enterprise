<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="normalize-space(COL2)!='Trade Date'">
					<PositionMaster>
						<!--   Fund -->
						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>

						<!--<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:variable name = "PB_SymbolTicker_NAME" >
							<xsl:value-of select="COL17"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SymbolTicker_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_SymbolTicker_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_SymbolCurrency_NAME" >
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name = "PB_Currency_NAME" >
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SymbolCurrency_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@CompanyName=$PB_SymbolCurrency_NAME and @Currency=$PB_Currency_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="COL2"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>-->

						<!--<xsl:variable name="varMonthNo" select="substring(COL17,9,2)"/>
						<xsl:variable name="varYearNo" select="substring(COL17,7,2)"/>
						<xsl:variable name="varSymbol" select="normalize-space(substring(COL17,1,6))"/>
						<xsl:variable name="varStrikePrice" select="format-number(concat(substring(COL17,14,5),'.',substring(COL17,19)),'#.00')"/>
						<xsl:variable name="varCallPutCode" select="substring(COL17,13,1)"/>
						<xsl:variable name = "varMonthCode" >
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="$varMonthNo" />
								<xsl:with-param name="varPutCall" select="$varCallPutCode"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="varOptionSymbol">
							<xsl:value-of select="concat('O:',$varSymbol,' ',$varYearNo,$varMonthCode,$varStrikePrice)"/>
						</xsl:variable>-->


						<!--<xsl:choose>
							
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
		
							
						</xsl:choose>-->
						
						
						<Symbol>
							<!--<xsl:choose>-->
								
									<xsl:value-of select="COL3"/>
								
								<!--<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="COL9='USD'">
											<xsl:choose>
												<xsl:when test="$PRANA_SymbolTicker_NAME=''">
													<xsl:value-of select="$PB_SymbolTicker_NAME"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$PRANA_SymbolTicker_NAME"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="$PRANA_SymbolCurrency_NAME=''">
													<xsl:value-of select="$PB_SymbolCurrency_NAME"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$PRANA_SymbolCurrency_NAME"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>-->
							<!--</xsl:choose>-->
						</Symbol>

						<!--<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="string-length(COL5) &gt; 20">
									<xsl:value-of select="concat(COL5, 'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>-->


						<CostBasis>
							<xsl:choose>
								<xsl:when test="number(COL5)">
									<xsl:value-of select="COL5"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="COL4 &gt; 0">
									<xsl:value-of select="COL4"/>
								</xsl:when>
								<xsl:when test="COL4 &lt; 0">
									<xsl:value-of select="COL4*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL4 &gt; 0 ">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL4 &lt; 0">
									<xsl:value-of select="5"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<!--<Description>
							<xsl:value-of select="COL4"/>
						</Description>-->

						<CounterPartyID>
							<xsl:value-of select ="24"/>
						</CounterPartyID>

						<!--<FXRate>
							<xsl:choose>
								<xsl:when test="number(COL10)">
									<xsl:value-of select="number(COL11) div number(COL10)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>-->

						<!--<FXConversionMethodOperator>
							<xsl:value-of select ="'M'"/>
						</FXConversionMethodOperator>-->

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>