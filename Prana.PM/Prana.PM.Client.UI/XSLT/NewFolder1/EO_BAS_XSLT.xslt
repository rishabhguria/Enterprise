<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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

	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="Position"> <!--<Put Quantity here->-->
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position)">
					
					<PositionMaster>

						<!--Put Account/Fund here-->
						<xsl:variable name ="varPBName">
							<xsl:value-of select ="'GS'"/>
						</xsl:variable>
						<xsl:variable name ="PB_FUND_NAME">
							<xsl:value-of select ="''"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<!--Put Symbol/Ticker here-->
						<xsl:variable name ="Symbol">
							<xsl:value-of select ="COL3"/>
						</xsl:variable>
						<xsl:variable name ="PB_COMPANY">
							<xsl:value-of select ="' '"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL!=''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:when test ="$Symbol!=''">
									<xsl:value-of select ="translate($Symbol,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_COMPANY"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						<!--Put Description here-->
						<PBSymbol>
							
						</PBSymbol>

						<!--Put Average Price here-->


						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test ="$AvgPrice &gt; 0">
									<xsl:value-of select ="$AvgPrice"/>
								</xsl:when>
								<xsl:when test ="$AvgPrice &lt; 0">
									<xsl:value-of select ="$AvgPrice * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>


						<!--Put Trade Date here-->
						<PositionStartDate>
							<xsl:value-of select=" * "/>
						</PositionStartDate>

						<!--Put Quantity here-->

						<NetPosition>
							<xsl:choose>
								<xsl:when test ="$Position &gt; 0">
									<xsl:value-of select ="$Position"/>
								</xsl:when>
								<xsl:when test ="$Position &lt; 0">
									<xsl:value-of select ="$Position * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetPosition>


						

						<!--Put Side here , Value should be 1 in case of 'Buy', 2 in case of 'Sell' , 5 in case of 'Sell short' , A in case of 'Buy to Open' , B in case of 'Buy to Close' , C in case of 'Sell to Open' and D in case of 'Sell to Close'-->
						<SideTagValue>
							<xsl:choose>
								
								<xsl:when test="COL2='Sell Short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="COL2='Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL2='Buy Cover'">
									<xsl:value-of select="'B'"/>
								</xsl:when>

								<xsl:when test="COL2='Sell'">
									<xsl:value-of select="'2'"/>
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