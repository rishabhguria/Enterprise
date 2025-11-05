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


	<xsl:template name="varCurrency">
		<xsl:param name="Currency"/>
		<xsl:choose>
			<xsl:when test="$Currency='United States dollar'">
				<xsl:value-of select="'USD'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="varMonth">
		<xsl:param name="MonthName"/>
		<xsl:choose>
			<xsl:when test="$MonthName='June'">
				<xsl:value-of select="6"/>
			</xsl:when>
			<xsl:when test="$MonthName='Feb'">
				<xsl:value-of select="2"/>
			</xsl:when>
			<xsl:when test="$MonthName='Mar'">
				<xsl:value-of select="3"/>
			</xsl:when>
			<xsl:when test="$MonthName='Apr'">
				<xsl:value-of select="4"/>
			</xsl:when>
			<xsl:when test="$MonthName='May'">
				<xsl:value-of select="5"/>
			</xsl:when>
			<xsl:when test="$MonthName='Jun'">
				<xsl:value-of select="6"/>
			</xsl:when>
			<xsl:when test="$MonthName='Jul'">
				<xsl:value-of select="7"/>
			</xsl:when>
			<xsl:when test="$MonthName='Aug'">
				<xsl:value-of select="8"/>
			</xsl:when>
			<xsl:when test="$MonthName='Sep'">
				<xsl:value-of select="9"/>
			</xsl:when>
			<xsl:when test="$MonthName='Oct'">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$MonthName='Nov'">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$MonthName='Dec'">
				<xsl:value-of select="12"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>


		
			<xsl:for-each select ="//Comparision">
				<xsl:variable name="FundName" select="substring-before(//Comparision[contains(COL1,'PCW FUND')]/COL1, '|')"/>
				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="translate(COL3,'$','')"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Cash) and contains(COL1,'Cash')">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'SEI'"/>
							</xsl:variable>

							<xsl:variable name="PB_FUND_NAME" select="normalize-space($FundName)"/>

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


							<Currency>
								<xsl:value-of select="'USD'"/>
							</Currency>


							<OpeningBalanceDR>
								<xsl:choose>
									<xsl:when test="number($Cash)">
										<xsl:value-of select="$Cash"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OpeningBalanceDR>

							<xsl:variable name="OpeningBalanceCR">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<OpeningBalanceCR>
								<xsl:choose>
									<xsl:when test="number($OpeningBalanceCR)">
										<xsl:value-of select="$OpeningBalanceCR"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OpeningBalanceCR>

							<xsl:variable name="MonthNo">
								<xsl:call-template name="varMonth">
									<xsl:with-param name="MonthName" select="substring-before(substring-after($varDate,'-'),'-')"/>
								</xsl:call-template>
							</xsl:variable>

							<xsl:variable name="vardateCode" select="substring-before($varDate,'-')"/>

							<xsl:variable name="varYearCode" select="number(substring-after(substring-after($varDate,'-'),'-'))"/>

							<TradeDate>
								<xsl:value-of select="concat($MonthNo,'/',$vardateCode,'/',$varYearCode)"/>
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