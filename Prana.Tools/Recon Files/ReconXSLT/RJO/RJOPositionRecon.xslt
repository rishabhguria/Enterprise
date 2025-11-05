<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com"
				>
	<xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<!-- Put = 0,Call = 1 , Here First call/put code then 2 characters for month code -->
		<!-- Call month Codes e.g. 101 represents 1=Call, 01 = January-->
		<xsl:choose>
			<xsl:when test ="$varMonth=01">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=02">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=03">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=04">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=05">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=06">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=07">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=08">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=09">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=10">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=11">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=12">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>

			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/DocumentElement">
		<DocumentElement>
			<xsl:for-each select="PositionMaster">
				<PositionMaster>

					<xsl:variable name = "VarFutureExpirationMonth" >
						<xsl:value-of select="normalize-space(COL8)"/>
					</xsl:variable>

					<xsl:variable name = "VarFutureRootSymbol" >
						<xsl:value-of select="normalize-space(COL6)"/>
					</xsl:variable>

					<xsl:variable name = "VarFutureExpirationYear">
						<xsl:value-of select="normalize-space(substring(COL9,4,1))"/>
					</xsl:variable>

					<xsl:variable name = "varFutureMonthCode" >
						<xsl:call-template name="MonthCode">
							<xsl:with-param name="varMonth" select="$VarFutureExpirationMonth" />
						</xsl:call-template>
					</xsl:variable>


					<xsl:variable name="PRANA_Root_Symbol">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='RJO']/SymbolData[@PBCompanyName=$VarFutureRootSymbol]/@PranaSymbol"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="$PRANA_Root_Symbol != ''">
							<Symbol>
								<xsl:value-of select="concat($PRANA_Root_Symbol,' ',$varFutureMonthCode,$VarFutureExpirationYear)"/>
							</Symbol>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="concat($VarFutureRootSymbol,' ',$varFutureMonthCode,$VarFutureExpirationYear)"/>
							</Symbol>
						</xsl:otherwise>
					</xsl:choose>


					<PBSymbol>
							<xsl:value-of select="$VarFutureRootSymbol"/>
						</PBSymbol>

					<xsl:variable name = "varQuantity" >
						<xsl:value-of select="number(COL14) + number(COL15)"/>
					</xsl:variable>
		
					<Quantity>
						<xsl:value-of select="number(COL14) + number(COL15)"/>
					</Quantity>
					
						<xsl:choose>
							<xsl:when test="COL14 = 0">
								
								<Side>
									<xsl:value-of select="'Buy'"/>
								</Side>
							</xsl:when>
							<xsl:when test="COL15 = 0">
									<Side>
									<xsl:value-of select="'Sell'"/>
								</Side>
							</xsl:when>
							<xsl:otherwise>
								<Side>
									<xsl:value-of select="''"/>
								</Side>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test ="boolean(number(COL16))">
								<MarkPrice>
									<xsl:value-of select="COL16"/>
								</MarkPrice>
							</xsl:when>
							<xsl:otherwise>
								<MarkPrice>
									<xsl:value-of select="0"/>
								</MarkPrice>
							</xsl:otherwise>
						</xsl:choose>

						<!--<MarketValue>
							<xsl:value-of select="$varQuantity * "/>
						</MarketValue>

						<MarketValueBase>
							<xsl:value-of select="COL33"/>
						</MarketValueBase>-->

						<!--<CompanyName>
							<xsl:value-of select="translate(COL20,' ','')"/>
						</CompanyName>-->

						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='RJO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<!--<SMRequest>
							<xsl:value-of select ="'TRUE'"/>
						</SMRequest>-->

					</PositionMaster>
			
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


</xsl:stylesheet>
