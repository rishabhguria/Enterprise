<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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
			<xsl:variable name="FundName" select="(//PositionMaster[contains(COL1,'AMEREN LYRICAL LARGE CAP')]/COL2)"/>
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash">
					<xsl:choose>
						<xsl:when test="COL7='STIFS AND MONEY MARKETS Total'">
							<xsl:value-of select="COL14"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>

				<xsl:if test="number($Cash) ">


					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'FMTC'"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME">
							<xsl:value-of select="$FundName"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name="LocalCurrency"/>
						<LocalCurrency>
							<xsl:value-of select="'USD'"/>
						</LocalCurrency>

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<CashValueLocal>
							<xsl:choose>
								<xsl:when test ="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</CashValueLocal>


						<xsl:variable name="Date" select="COL"/>
						<Date>
							<xsl:value-of select="$Date"/>
						</Date>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>