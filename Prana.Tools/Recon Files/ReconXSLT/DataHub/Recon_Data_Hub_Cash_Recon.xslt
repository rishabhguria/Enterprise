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
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Cash)">
					<PositionMaster>
						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'DataHub'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL4"/>
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

							<xsl:value-of select="'ABC'"/>
						</PortfolioAccount>

						<MasterAccount>
							<xsl:value-of select="COL1"/>
						</MasterAccount>


						<Account>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Account>

						<AccountType>
							<xsl:value-of select="COL2"/>
						</AccountType>

						<SubAccount>
							<xsl:value-of select="COL3"/>
						</SubAccount>


						<NetOpeningBalanceBase>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetOpeningBalanceBase>

						<xsl:variable name="CashValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>

						<NetClosingBalanceBase>
							<xsl:choose>
								<xsl:when test="number($CashValueBase)">
									<xsl:value-of select="$CashValueBase"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetClosingBalanceBase>


						<ActivityDR>
							<xsl:value-of select="COL6"/>
						</ActivityDR>


						<BaseCurrecny>
							<xsl:value-of select="'USD'"/>
						</BaseCurrecny>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>