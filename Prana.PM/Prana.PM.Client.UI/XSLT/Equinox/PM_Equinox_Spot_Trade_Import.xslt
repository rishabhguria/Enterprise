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
				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL17"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($NetPosition)">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Leucadia'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:variable name="varsymbol">
							<xsl:choose>
								<xsl:when test="COL11='GBP' or COL11='AUD' or COL11='NZD' or COL11='EUR' ">
									<xsl:value-of select="concat(COL11,'/','USD')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',COL11)"/>
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
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<PositionStartDate>
							<xsl:value-of select ="COL1"/>
						</PositionStartDate>
						
						
						<xsl:variable name="varNetamountLocal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varNetamountBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL17"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varNetamount">
							<xsl:choose>
								<xsl:when test="COL11='GBP' or COL11='AUD' or COL11='NZD' or COL11='EUR' ">
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

						<xsl:variable name="varSide" select="COL7"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL11 !='GBP' or COL11 !='AUD' or COL11!='NZD' or COL11!='EUR' ">
									<xsl:choose>
										<xsl:when test ="$varSide ='Buy'">
											<xsl:value-of select ="'2'"/>
										</xsl:when>

										<xsl:when test ="$varSide ='Sell'">
											<xsl:value-of select ="'1'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="$varSide ='Buy'">
											<xsl:value-of select ="'1'"/>
										</xsl:when>

										<xsl:when test ="$varSide ='Sell'">
											<xsl:value-of select ="'2'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>						
														
						</SideTagValue>
						
						<xsl:variable name="varCostBasis">
							<xsl:choose>
								<xsl:when test="COL11='GBP' or COL11='AUD' or COL11='NZD' or COL11='EUR' ">
									<xsl:value-of select="$varNetamountLocal div $varNetamountBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varNetamountBase div $varNetamountLocal"/>
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


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
