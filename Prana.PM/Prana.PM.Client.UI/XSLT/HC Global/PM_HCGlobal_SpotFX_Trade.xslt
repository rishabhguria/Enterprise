<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),'$',''),$SingleQuote,''))"/>
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
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varQty">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>
			
											
				<xsl:if test="number($varQty)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Blotter']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
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
						
			           <xsl:variable name="varLeadingCurrency">
			        		<xsl:choose>
			        			<xsl:when test="COL4='AUD' or COL4='NZD' or COL4='EUR' or COL4='GBP'">
			        				<xsl:value-of select="'1'"/>
			        			</xsl:when>
			        			<xsl:otherwise>
			        				<xsl:value-of select="'0'"/>
			        			</xsl:otherwise>
			        		</xsl:choose>
			        	</xsl:variable>	

						<xsl:variable name="varSymbol">
              <xsl:choose>
                <xsl:when test="$varLeadingCurrency ='1'">
                  <xsl:value-of select="concat(COL4,'/','USD')"/>
                </xsl:when>
                <xsl:when test="$varLeadingCurrency ='0'">
                  <xsl:value-of select="concat('USD','/',COL4)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL != ''">
									<xsl:value-of select ="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL8)"/>
              </xsl:call-template>
            </xsl:variable>
						<CostBasis>
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
						</CostBasis>

						<NetPosition>
              <xsl:choose>
                <xsl:when test="$varQty &gt; 0">
                  <xsl:value-of select="$varQty"/>
                </xsl:when>
                <xsl:when test="$varQty &lt; 0">
                  <xsl:value-of select="$varQty * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
						</NetPosition>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL17)"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <xsl:variable name="varFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL18)"/>
              </xsl:call-template>
            </xsl:variable>
            <Fees>
              <xsl:choose>
                <xsl:when test="$varFees &gt; 0">
                  <xsl:value-of select="$varFees"/>
                </xsl:when>
                <xsl:when test="$varFees &lt; 0">
                  <xsl:value-of select="$varFees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varLeadingCurrency ='1'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varLeadingCurrency ='0'">
									<xsl:value-of select="'2'"/>									
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

            <SettlCurrencyName>
              <xsl:value-of select="COL19"/>
            </SettlCurrencyName>

						<xsl:variable name="varPositionSettlementDate">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="varPositionStartDate">
							<xsl:value-of select="COL1"/>
						</xsl:variable>
						

						<PositionStartDate>
							<xsl:value-of select="$varPositionStartDate"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select="$varPositionSettlementDate"/>
						</PositionSettlementDate>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>

