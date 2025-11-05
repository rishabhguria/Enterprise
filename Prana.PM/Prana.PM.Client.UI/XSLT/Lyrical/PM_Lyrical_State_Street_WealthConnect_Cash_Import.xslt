<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash" select="number(COL6)"/>

				<xsl:if test="number($Cash) and contains(COL1,'Total')!='true' and (COL2='CM1FOIXX2' or COL2='P2CASH' or COL2='CM1GVVXX3' or COL2='P1CASH')">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Statestreet_WC'"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL4)"/>

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

						<LocalCurrency>
							<xsl:value-of select="'USD'"/>
						</LocalCurrency>

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<CashValueLocal>
							<xsl:choose>

								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="$Cash"/>
								</xsl:when>

								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="$Cash * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CashValueLocal>

						<CashValueBase>
							<xsl:value-of select="0"/>
						</CashValueBase>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>