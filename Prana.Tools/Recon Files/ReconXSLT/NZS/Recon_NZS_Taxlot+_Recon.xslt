<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
	<xsl:output method="xml" indent="yes" />
	<xsl:template name="Translate">
		<xsl:param name="Number" />
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
		<xsl:template name="FormatDate">
		<xsl:param name="DateTime" />
		<!-- converts date time double number to 18/12/2009 -->

		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019" />
		</xsl:variable>

		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))" />
		</xsl:variable>

		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
		</xsl:variable>

		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
		</xsl:variable>

		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
		</xsl:variable>

		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))" />
		</xsl:variable>

		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
		</xsl:variable>

		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))" />
		</xsl:variable>

		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))" />
		</xsl:variable>

		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
		</xsl:variable>

		<xsl:variable name ="varMonthUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nMonth) = 1">
					<xsl:value-of select ="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="nDayUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nDay) = 1">
					<xsl:value-of select ="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nDay"/>
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
			<xsl:for-each select="//Comparision">
				
				<xsl:variable name="varNetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL13)"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:if test="number($varNetPosition) and COL20 ='Equities' and COL27 !=' '">
					
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						
						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL23)"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varSedol">
				   <xsl:value-of select="substring(COL26,2)"/>
			  </xsl:variable>

			  <Symbol>
               <xsl:choose>
                 <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                   <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                 </xsl:when>
				  <xsl:when test="$varSedol !=''">
				   <xsl:value-of select="''"/>
				  </xsl:when>
                 <xsl:when test="$varSymbol!='*'">
                   <xsl:value-of select="$varSymbol"/>
                 </xsl:when>
                 <xsl:otherwise>
                   <xsl:value-of select="$PB_SYMBOL_NAME"/>
                 </xsl:otherwise>
               </xsl:choose>
            </Symbol>

			<SEDOL>
			 <xsl:choose>
			  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
				  <xsl:value-of select="''"/>
			  </xsl:when>
			  <xsl:when test="$varSedol !=''">
				  <xsl:value-of select="$varSedol"/>
			  </xsl:when>
			  <xsl:when test="$varSymbol !=''">
				  <xsl:value-of select="''"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="''"/>
			  </xsl:otherwise>
			 </xsl:choose>
			</SEDOL>
						
						<xsl:variable name="PB_FUND_NAME" select="COL2" />
						
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
						
						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>
						
						<Side>
							<xsl:choose>
								<xsl:when test="$varNetPosition &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$varNetPosition &lt; 0">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>
						
						<xsl:variable name="varBaseCurrency">
							<xsl:value-of select="normalize-space(COL15)"/>
						</xsl:variable>
						
						<BaseCurrency>
							<xsl:value-of select="$varBaseCurrency"/>
						</BaseCurrency>
						
						<xsl:variable name="varSettlementCurrency">
							<xsl:value-of select="normalize-space(COL14)"/>
						</xsl:variable>
						
						<SettlCurrency>
							<xsl:value-of select="$varSettlementCurrency"/>
						</SettlCurrency>
						
						<xsl:variable name="varOriginalPurchaseDate">
							<xsl:call-template name="FormatDate">
								<xsl:with-param name="DateTime" select="COL27"/>
							</xsl:call-template>
						</xsl:variable>
						
						<!--<xsl:variable name="varOriginalPurchaseDate">
							<xsl:value-of select="COL27"/>
						</xsl:variable>-->
						
						<OriginalPurchaseDate>
							<xsl:value-of select="$varOriginalPurchaseDate"/>
						</OriginalPurchaseDate>

						<Quantity>
							<xsl:choose>
								<xsl:when test="$varNetPosition &gt; 0">
									<xsl:value-of select="$varNetPosition"/>
								</xsl:when>
								<xsl:when test="$varNetPosition &lt; 0">
									<xsl:value-of select="$varNetPosition * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="varCostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$varCostBasis &gt; 0">
									<xsl:value-of select="$varCostBasis"/>
								</xsl:when>
								<xsl:when test="$varCostBasis &lt; 0">
									<xsl:value-of select="$varCostBasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>	
						
					<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL5"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="number($MarkPrice)">
									<xsl:value-of select="$MarkPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>						

						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test="number($MarketValue)">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<xsl:variable name="varMarketValuebase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="$varMarketValuebase &gt; 0">
									<xsl:value-of select="$varMarketValuebase"/>
								</xsl:when>
								<xsl:when test="$varMarketValuebase &lt; 0">
									<xsl:value-of select="$varMarketValuebase * (-1)"/>
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