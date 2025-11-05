<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL7)">
					<!--<xsl:if test="number(COL9)"></xsl:if>-->

					<PositionMaster>

						<xsl:variable name="varPBName">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<!--Put Account/Fund here-->
						<FundName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL1"/>
								</xsl:otherwise>
							</xsl:choose>
							
						</FundName>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<!--Put Syol/Ticker here-->
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL!=''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL3"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<!--<SEDOL>
							
						</SEDOL>-->

						<!--Put Description here-->
						<PBSymbol>
							<xsl:value-of select="COL13"/>
						</PBSymbol>

						<!--Put Average Price here-->
						<CostBasis>
							<xsl:choose>
								<xsl:when test="COL8='B'">
									<xsl:value-of select=" number(COL19 - (number(COL29) div number(COL7)))"/>
								</xsl:when>
								<xsl:when test="COL8='S'">
									<xsl:value-of select=" number(COL19 +  (number(COL28) + number(COL33) div number(COL7)))"/>
								</xsl:when>
							</xsl:choose>
							
						</CostBasis>


						<!--Put Trade Date here-->
						<PositionStartDate>
							<!--<xsl:value-of select="COL8"/>-->
						</PositionStartDate>

						<!--Put Quantity here-->
						<NetPosition>
							<xsl:choose>
								<xsl:when test ="COL7 &lt; 0">
									<xsl:value-of select ="COL7*(-1)"/>
								</xsl:when>
								<xsl:when test ="COL7 &gt; 0">
									<xsl:value-of select ="COL7"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>
						
						<Commission>
							<xsl:choose>
								<xsl:when test ="COL29 &lt; 0">
									<xsl:value-of select ="COL29*(-1)"/>
								</xsl:when>
								<xsl:when test ="COL29 &gt; 0">
									<xsl:value-of select ="COL29"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:value-of select="COL29"/>-->
						</Commission>

						<!--Put Side here , Value should be 1 in case of 'Buy', 2 in case of 'Sell' , 5 in case of 'Sell short' , A in case of 'Buy to Open' , B in case of 'Buy to Close' , C in case of 'Sell to Open' and D in case of 'Sell to Close'-->
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="(COL9='O' and COL8='B') or (COL9='C' and COL8='B')">
									<xsl:value-of select= "B" />
								</xsl:when>
								<!--<xsl:when test="(COL8='B' or COL9='C')">
									<xsl:value-of select="B"/>
								</xsl:when>-->
								<xsl:when test="(COL9='O' and COL8='S') or (COL9='C' and COL8='S') ">
									<xsl:value-of select= "2" />
								<!--</xsl:when>-->
								<!--<xsl:when test="(COL8='S' or COL9='C')">
									<xsl:value-of select= "2" />-->
									<!--<xsl:choose>
										<xsl:when test="COL5='sell to open'">
											<Side>
												<xsl:value-of select="'C'"/>
											</Side>
										</xsl:when>
										<xsl:when test="COL5='Buy to open'">
											<Side>
												<xsl:value-of select="'A'"/>
											</Side>
										</xsl:when>


										<xsl:when test="COL5='BUY'">
											<Side>
												<xsl:value-of select="'1'"/>
											</Side>
										</xsl:when>
										<xsl:when test="COL5='SELL'">
											<Side>
												<xsl:value-of select="'2'"/>
											</Side>
										</xsl:when>

										<xsl:otherwise>
											<Side>
												<xsl:value-of select="''"/>
											</Side>
										</xsl:otherwise>
									</xsl:choose>-->
								</xsl:when>
							</xsl:choose>
						</SideTagValue>




					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>