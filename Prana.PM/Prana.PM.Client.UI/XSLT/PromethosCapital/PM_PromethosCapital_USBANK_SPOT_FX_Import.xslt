<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
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

			<xsl:for-each select ="//PositionMaster">
				
				<xsl:variable name="varNetamountLocal">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL22"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="varNetamountBase">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL20"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:if test ="number($varNetamountLocal) and contains(COL9, 'SPOT')">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varCURRENCY">
							<xsl:choose>
								<xsl:when test="normalize-space(COL9)='HONGKONG DOLLAR SPOT'">
									<xsl:value-of select="'HKD'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='AUSTRALIAN DOLLAR SPOT'">
									<xsl:value-of select="'AUD'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='BRAZILIAN REAL SPOT'">
									<xsl:value-of select="'BRL'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='CANADIAN DOLLAR SPOT'">
									<xsl:value-of select="'CAD'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='EURO SPOT'">
									<xsl:value-of select="'EUR'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='SINGAPORE DOLLAR SPOT'">
									<xsl:value-of select="'SGD'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='JAPANESE YEN SPOT'">
									<xsl:value-of select="'JPY'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='SWISS FRANC SPOT'">
									<xsl:value-of select="'CHF'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='GR BRITISH POUND SPOT'">
									<xsl:value-of select="'GBP'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL9)='SOUTH AFRICAN RAND SPOT'">
									<xsl:value-of select="'ZAR'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varsymbol">
							<xsl:choose>
								<xsl:when test="$varCURRENCY='GBP' or $varCURRENCY='AUD' or $varCURRENCY='NZD' or $varCURRENCY='EUR' ">
									<xsl:value-of select="concat($varCURRENCY,'/','USD')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',$varCURRENCY)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varsymbol!=''">
									<xsl:value-of select="$varsymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

	<!--If symbol used as GBP/USD then cash amount column will pick in pay receive and Units as Quantity. = QTY = T , AVG = Average price = column 22 dividend by column 20
	If symbol used as USD/CHF then cash amount will pick as Quantity and column T will pack as Pay receive.  QTY = V Average price = column 20 divided by column 22-->

						<xsl:variable name="varNetamount">
							<xsl:choose>
								<xsl:when test="$varCURRENCY='GBP' or $varCURRENCY='AUD' or $varCURRENCY='NZD' or $varCURRENCY='EUR' ">
									<xsl:value-of select="$varNetamountLocal"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varNetamountBase"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<NetPosition>
							<xsl:choose>
								<xsl:when test ="$varNetamount &lt;0">
									<xsl:value-of select ="$varNetamount*-1"/>
								</xsl:when>
								<xsl:when test ="$varNetamount &gt;0">
									<xsl:value-of select ="$varNetamount"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>


						<SideTagValue>
							<xsl:choose>
								<xsl:when test ="$varNetamount &lt;0">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test ="$varNetamount &gt;0">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name="varCostBasis">
							<xsl:choose>
								<xsl:when test="$varCURRENCY='GBP' or $varCURRENCY='AUD' or $varCURRENCY='NZD' or $varCURRENCY='EUR' ">
									<xsl:value-of select="$varNetamountBase div $varNetamountLocal"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varNetamountLocal div $varNetamountBase"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>
								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<PositionStartDate>
							<xsl:value-of select ="COL2"/>
						</PositionStartDate>
						
						<PositionSettlementDate>
							<xsl:value-of select ="COL2"/>
						</PositionSettlementDate>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
