<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

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
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Quantitiy">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL7"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Quantitiy)">
					<PositionMaster>
						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'DataHub'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<PortfolioAccount>
							<!--<xsl:choose>
								<xsl:when test ="$PRANA_PORTFOLUIO_NAME!=''">
									<xsl:value-of select ="$PRANA_PORTFOLUIO_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_PORTFOLIO_NAME"/>
								</xsl:otherwise>

							</xsl:choose>-->

							<xsl:value-of select="'XYZ partners'"/>
						</PortfolioAccount>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL5"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Symbol">
							<xsl:value-of select="normalize-space(COL4)"/>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>




						<Quantitiy>
							<xsl:choose>
								<xsl:when test="number($Quantitiy)">
									<xsl:value-of select="$Quantitiy"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantitiy>
						
						<xsl:variable name="UnitCostBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
							</xsl:call-template>
						</xsl:variable>
						<UnitCostBase>
							<xsl:choose>
								<xsl:when test="number($UnitCostBase)">
									<xsl:value-of select="$UnitCostBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</UnitCostBase>

						<xsl:variable name="EndingPriceLocal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>
						<EndingPriceBase>
							<xsl:choose>
								<xsl:when test="number($EndingPriceLocal)">
									<xsl:value-of select="$EndingPriceLocal"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</EndingPriceBase>

						<xsl:variable name="TotalCost_Local">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL11"/>
							</xsl:call-template>
						</xsl:variable>
						<TotalCost_Local>
							<xsl:choose>
								<xsl:when test="number($TotalCost_Local)">
									<xsl:value-of select="$TotalCost_Local"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalCost_Local>

						<xsl:variable name="EndingMarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL17"/>
							</xsl:call-template>
						</xsl:variable>
						<EndingMarketValueBase>
							<xsl:choose>
								<xsl:when test="number($EndingMarketValueBase)">
									<xsl:value-of select="$EndingMarketValueBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</EndingMarketValueBase>


						<xsl:variable name="TotalCost_Base">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>
						<TotalCost_Base>
							<xsl:choose>
								<xsl:when test="number($TotalCost_Base)">
									<xsl:value-of select="$TotalCost_Base"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalCost_Base>

						<xsl:variable name="UnrealizedpnlBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL18"/>
							</xsl:call-template>
						</xsl:variable>
						<UnrealizedpnlBase>
							<xsl:choose>
								<xsl:when test="number($UnrealizedpnlBase)">
									<xsl:value-of select="$UnrealizedpnlBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</UnrealizedpnlBase>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>