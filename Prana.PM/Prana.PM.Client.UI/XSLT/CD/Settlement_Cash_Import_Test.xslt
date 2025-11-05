<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="SetlementCashLocal" select="COL4"/>

				<xsl:if test="number($SetlementCashLocal) and  COL1 != 'Fund' and COL2 != '*'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'UBS'"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>

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

						<xsl:variable name="PB_CURRENCY_NAME" select="normalize-space(COL3)"/>

						<!--<xsl:variable name="PRANA_CURRENCY_NAME">
							<xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@PBCurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyID"/>
						</xsl:variable>-->

						<SettlementDateLocalCurrency>
							<!--<xsl:choose>

								<xsl:when test ="$PRANA_CURRENCY_NAME!=''">
									<xsl:value-of select ="$PRANA_CURRENCY_NAME"/>
								</xsl:when>

								<xsl:otherwise>-->
									<xsl:value-of select ="$PB_CURRENCY_NAME"/>
								<!--</xsl:otherwise>

							</xsl:choose>-->
						</SettlementDateLocalCurrency>

						<SettlementDateBaseCurrency>
							<xsl:value-of select="'USD'"/>
						</SettlementDateBaseCurrency>

						<SettlementDateCashValueLocal>
							<xsl:value-of select="$SetlementCashLocal"/>
						</SettlementDateCashValueLocal>

						<SettlementDateCashValueBase>
							<xsl:value-of select="0"/>
						</SettlementDateCashValueBase>

						<SettlementDate>
							<xsl:value-of select="COL1"/>
						</SettlementDate>

						<!--<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>-->

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>