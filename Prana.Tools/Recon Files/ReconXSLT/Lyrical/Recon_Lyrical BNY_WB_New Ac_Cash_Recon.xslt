<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Cash" select="COL11"/>

				<xsl:if test="number($Cash) and (contains(COL7,'FEDERATED TREASURY')='true' or normalize-space(COL7)='TRADE DATE CASH')">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'BNY_WB'"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>

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

						<Symbol>
							<xsl:value-of select="COL14"/>
						</Symbol>

						<EndingQuantity>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>		
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>							
						</EndingQuantity>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>