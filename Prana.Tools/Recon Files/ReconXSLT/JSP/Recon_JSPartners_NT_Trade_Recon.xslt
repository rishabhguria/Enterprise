<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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

	<xsl:template name="FormatDate">
		<xsl:param name="DateTime"/>
		<!--  converts date time double number to 18/12/2009  -->
		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019"/>
		</xsl:variable>
		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))"/>
		</xsl:variable>
		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))"/>
		</xsl:variable>
		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))"/>
		</xsl:variable>
		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31"/>
		</xsl:variable>
		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))"/>
		</xsl:variable>
		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))"/>
		</xsl:variable>
		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))"/>
		</xsl:variable>
		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))"/>
		</xsl:variable>
		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)"/>
		</xsl:variable>
		<xsl:variable name="varMonthUpdated">
			<xsl:choose>
				<xsl:when test="string-length($nMonth) = 1">
					<xsl:value-of select="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="nDayUpdated">
			<xsl:choose>
				<xsl:when test="string-length($nDay) = 1">
					<xsl:value-of select="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="$varMonthUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nDayUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nYear"/>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL20)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="(normalize-space(COL9) ='Equities') and 
						(COL15 !='OPENING BALANCE' and COL15 !='CLOSING BALANCE') and not(contains(COL15,'INCOME'))">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'NT'"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME" />
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="varSedol">
							<xsl:choose>
								<xsl:when test="substring(normalize-space(COL12),1,1)='S'">
									<xsl:value-of select="substring(normalize-space(COL12),2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varCUSIP">
							<xsl:choose>
								<xsl:when test="substring(normalize-space(COL12),1,1)='C'">
									<xsl:value-of select="substring(normalize-space(COL12),2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSedol!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varCUSIP!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<SEDOL>
							<xsl:choose>
								<xsl:when test="$varSedol!=''">
									<xsl:value-of select="$varSedol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>

						<CUSIP>
							<xsl:choose>
								<xsl:when test="$varCUSIP!=''">
									<xsl:value-of select="$varCUSIP"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>

						<Side>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring-before(substring-after(substring-after(normalize-space(COL15),'PRICE:'),' '),' ')"/>
							</xsl:call-template>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>
								</xsl:when>
								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="substring-before(substring-after(substring-after(normalize-space(COL15),'COMMISSION:'),' '),' ')"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varSecFees">
							<xsl:choose>
								<xsl:when test="contains(COL15,'OTHER CHARGES')">
									<xsl:call-template name="Translate">
										<xsl:with-param name="Number" select="substring-before(substring-after(substring-after(normalize-space(COL15),'OTHER CHARGES:'),' '),' ')"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<SecFee>
							<xsl:choose>
								<xsl:when test="$varSecFees &gt; 0">
									<xsl:value-of select="$varSecFees"/>
								</xsl:when>
								<xsl:when test="$varSecFees &lt; 0">
									<xsl:value-of select="$varSecFees * (1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<Quantity>
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
						</Quantity>

						<xsl:variable name="varPositionStartDate">
							<xsl:call-template name="FormatDate">
								<xsl:with-param name="DateTime" select="normalize-space(COL16)"/>
							</xsl:call-template>
						</xsl:variable>

						<TradeDate>
							<xsl:value-of select="$varPositionStartDate"/>
						</TradeDate>

						<xsl:variable name="varPositionSettlementDate">
							<xsl:call-template name="FormatDate">
								<xsl:with-param name="DateTime" select="normalize-space(COL17)"/>
							</xsl:call-template>
						</xsl:variable>

						<SettlementDate>
							<xsl:value-of select="$varPositionSettlementDate"/>
						</SettlementDate>
						
						<CounterParty>
                            <!-- <xsl:value-of select="normalize-space(substring-after(substring-after(normalize-space(COL15),'BROKER:'),' '))"/> -->
														<xsl:value-of select="'NT'"/>

                        </CounterParty>
			
			<xsl:variable name="NetNotionalValue">
				              <xsl:call-template name="Translate">
				                <xsl:with-param name="Number" select="COL21"/>
				              </xsl:call-template>
				            </xsl:variable>
				
				            <NetNotionalValue>
				
				              <xsl:choose>
				
				                <xsl:when test="$NetNotionalValue &gt; 0">
				                  <xsl:value-of select="$NetNotionalValue"/>
				                </xsl:when>
				
				                <xsl:when test="$NetNotionalValue &lt; 0">
				                  <xsl:value-of select="$NetNotionalValue * (-1)"/>
				                </xsl:when>
				
				
				                <xsl:otherwise>
				                  <xsl:value-of select="0"/>
				                </xsl:otherwise>
				
				              </xsl:choose>
				
				            </NetNotionalValue>
			
			
				</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>