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
				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL27"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Cash) and (contains(COL51,'CASH')='true' or contains(COL51,'MONEY MARKET BALANCE')='true')">
						<PositionMaster>

							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'MS'"/>
							</xsl:variable>

							<xsl:variable name = "PB_SYMBOL_NAME" >
								<xsl:value-of select="'COL2'"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>


							<xsl:variable name="PB_FUND_NAME" select="COL2"/>
							<xsl:variable name ="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>

							<PortfolioAccount>
								<xsl:choose>

									<xsl:when test ="$PRANA_FUND_NAME!=''">
										<xsl:value-of select ="$PRANA_FUND_NAME"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="$PB_FUND_NAME"/>
									</xsl:otherwise>

								</xsl:choose>
							</PortfolioAccount>

							<xsl:variable name="varCurrency">
								<xsl:choose>
									<xsl:when test ="normalize-space(COL2)='U.S. Dollars'">
										<xsl:value-of select ="'USD'"/>
									</xsl:when>
									<xsl:when test ="normalize-space(COL2)='Canadian Dollar'">
										<xsl:value-of select ="'CAD'"/>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>

							<Currency>
								<xsl:value-of select ="COL44"/>
							</Currency>

							

							<xsl:variable name="varCashValueDR">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL32"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="varCash">
								<xsl:choose>
									<xsl:when test="COL51='MONEY MARKET BALANCE'">
										<xsl:value-of select="$varCashValueDR"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$Cash"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<OpeningBalanceDR>
								<xsl:choose>
									<xsl:when test="number($varCash)">
										<xsl:value-of select="$varCash"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OpeningBalanceDR>

							<xsl:variable name="varCashValueCr">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL33"/>
								</xsl:call-template>
							</xsl:variable>
							<OpeningBalanceCR>
								<xsl:choose>
									<xsl:when test="number($varCashValueCr)">
										<xsl:value-of select="$varCashValueCr"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OpeningBalanceCR>



							<xsl:variable name="Year">
								<xsl:value-of select="substring(COL4,1,2)"/>
							</xsl:variable>
							<xsl:variable name="Month">
								<xsl:value-of select="substring(COL4,3,2)"/>
							</xsl:variable>
							<xsl:variable name="Day">
								<xsl:value-of select="substring(COL4,5,2)"/>
							</xsl:variable>
							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>



						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>

							<PortfolioAccount>
								<xsl:value-of select="''"/>
							</PortfolioAccount>


							<Currency>
								<xsl:value-of select="''"/>
							</Currency>


							<OpeningBalanceDR>
								<xsl:value-of select="0"/>
							</OpeningBalanceDR>

							<OpeningBalanceCR>
								<xsl:value-of select="0"/>
							</OpeningBalanceCR>



							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>


						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>