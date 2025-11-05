<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="PositionMaster">
				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(translate(COL11,' ' , ''),'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="$varInstrumentType='50' or $varInstrumentType='60'">
					<PositionMaster>
						<!--   Fund -->
						<!-- Column 1 mapped with Fund-->
						<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL2,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varPortfolioID='11817245' ">
								<FundName>
									<xsl:value-of select="'LETRP.11817245'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='11802079'">
								<FundName>
									<xsl:value-of select="'LETRP.11802079.H'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='11802597'">
								<FundName>
									<xsl:value-of select="'LETRP2.11802597'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='11802852'">
								<FundName>
									<xsl:value-of select="'LETRP2.11802852.H'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='11802555'">
								<FundName>
									<xsl:value-of select="'LETRP.11802555'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='11802079'">
								<FundName>
									<xsl:value-of select="'LETRP.11802079'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='11801907'">
								<FundName>
									<xsl:value-of select="'LETRP.11801907'"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select="' '"/>
								</FundName>
							</xsl:otherwise >
						</xsl:choose >
						<xsl:if test ="COL11='10'">
							<PBAssetType>
								<xsl:value-of select="'Bonds'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='50'">
							<PBAssetType>
								<xsl:value-of select="'Equities'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='55'">
							<PBAssetType>
								<xsl:value-of select="'Private Investments'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='60'">
							<PBAssetType>
								<xsl:value-of select="'Options'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='70'">
							<PBAssetType>
								<xsl:value-of select="'Futures'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='90'">
							<PBAssetType>
								<xsl:value-of select="'Programs'"/>
							</PBAssetType>
						</xsl:if >
						<xsl:if test ="COL11='99'">
							<PBAssetType>
								<xsl:value-of select="'Indexes'"/>
							</PBAssetType>
						</xsl:if >
						<!-- Column 7 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
						<xsl:choose>
							<xsl:when test="COL7 &lt; 0">
								<SideTagValue>
									<xsl:value-of select="'5'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL7*(-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="COL7 &gt; 0">
								<SideTagValue>
									<xsl:value-of select="'1'"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="COL7"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<SideTagValue>
									<xsl:value-of select="''"/>
								</SideTagValue>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>
						<!-- column 8 gives the total cost, so average price comes by this way -->
						<xsl:choose>
							<xsl:when test ="COL7 != 0 and COL8 != 0">
								<CostBasis>
									<xsl:value-of select="COL8 div COL7"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>
						<!--<CUSIP>
							<xsl:value-of select="COL9"/>
						</CUSIP>-->
						<PBSymbol>
							<xsl:value-of select="translate(COL5,'&quot;','')"/>
						</PBSymbol>
						<!--in case of equity we get the eSignal symbol directly so column 5 gives the value-->
						<xsl:if test="$varInstrumentType='50'">
							<Symbol>
								<xsl:value-of select="COL5"/>
							</Symbol>							
						</xsl:if >
						<!-- in case of Options, I get symbol from file like QCHKTX, I remove Q and then CHK TX and make eSignal symbol-->
						<xsl:if test="$varInstrumentType='60'">
							<xsl:variable name="varAfterQ" >
								<xsl:value-of select="substring-after(COL5,'Q')"/>
							</xsl:variable>
							<xsl:variable name = "varLength" >
								<xsl:value-of select="string-length($varAfterQ)"/>
							</xsl:variable>
							<xsl:variable name = "varAfter" >
								<xsl:value-of select="substring($varAfterQ,($varLength)-1,2)"/>
							</xsl:variable>
							<xsl:variable name = "varBefore" >
								<xsl:value-of select="substring($varAfterQ,1,($varLength)-2)"/>
							</xsl:variable>
							<Symbol>
								<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
							</Symbol>							
						</xsl:if >
						<!-- Position Date mapped with the column 4 -->
						<xsl:variable name = "varYR" >
							<xsl:value-of select="translate(substring(COL4,1,4),'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name = "varMth" >
							<xsl:value-of select="translate(substring(COL4,5,2),'&quot;','')"/>
						</xsl:variable>
						<xsl:variable name = "varDt" >
							<xsl:value-of select="translate(substring(COL4,7,2),'&quot;','')"/>
						</xsl:variable>
						<PositionStartDate>
							<xsl:value-of select="translate(concat($varYR,'/',$varMth,'/',$varDt),'&quot;','')"/>
						</PositionStartDate>
            <FXRate>
              <xsl:value-of select="1.0"/>
            </FXRate>
            <FXConversionMethodOperator>
              <xsl:value-of select="'M'"/>
            </FXConversionMethodOperator>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
