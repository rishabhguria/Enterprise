<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL17)">
					<!--<xsl:if test="number(COL9)"></xsl:if>-->

					<PositionMaster>

						<!--Put Account/Fund here-->
						<xsl:variable name ="varPBName">
							<xsl:value-of select ="'GS'"/>
						</xsl:variable>
						<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="COL5"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<FundName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</FundName>
						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="COL6"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>
						<!--Put Symbol/Ticker here-->
						<xsl:variable name ="Symbol">
			             	<xsl:choose>
								<xsl:when test ="contains(COL6,'-')">
									<xsl:value-of select ="substring-before(COL6,'-')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL6"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL!=''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test ="COL6!=''">
									<xsl:value-of select ="$Symbol"/>
									</xsl:when>
							</xsl:choose>
							
						</Symbol>

						
						<!--Put Description here-->
						<PBSymbol>
							<xsl:value-of select="COL7"/>
						</PBSymbol>

						<!--Put Average Price here-->
						<xsl:variable name ="varCost">
							<xsl:choose>
								<xsl:when test ="number(COL24)">
									<xsl:value-of select ="COL24"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test ="$varCost &gt; 0">
									<xsl:value-of select ="$varCost"/>
								</xsl:when>
								<xsl:when test ="$varCost &lt; 0">
									<xsl:value-of select ="$varCost * -1"/>
								</xsl:when>
							</xsl:choose>
						</CostBasis>


						<!--Put Trade Date here-->
						<xsl:variable name ="Date">
							<xsl:choose>
								<xsl:when test ="contains(COL45, ' ' )">
									<xsl:value-of select ="substring-before(COL45, ' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL45"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<PositionStartDate>
							<xsl:choose>
								<xsl:when test ="COL45!=''">
									<xsl:value-of select ="$Date"/>
								</xsl:when>
							</xsl:choose>
						</PositionStartDate>

						<!--Put Quantity here-->
						<xsl:variable name ="varQuantity">
							<xsl:choose>
								<xsl:when test ="number(COL17)">
									<xsl:value-of select ="COL17"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>
						<NetPosition>
							<xsl:choose>
								<xsl:when test ="$varQuantity &gt; 0">
									<xsl:value-of select ="$varQuantity"/>
								</xsl:when>
								<xsl:when test ="$varQuantity &lt; 0">
									<xsl:value-of select ="$varQuantity * -1"/>
								</xsl:when>
							</xsl:choose>
						</NetPosition>

						<!--Put Side here , Value should be 1 in case of 'Buy', 2 in case of 'Sell' , 5 in case of 'Sell short' , A in case of 'Buy to Open' , B in case of 'Buy to Close' , C in case of 'Sell to Open' and D in case of 'Sell to Close'-->
						<SideTagValue>
							
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