<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	
	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Cash" select="COL41"/>

				<xsl:if test="number($Cash) and (normalize-space(COL4)='JPMORGAN PRIME MMKT FUND CAPITAL SHARES - FUND 3605' or COL4='US DOLLAR'or COL4='JPMORGAN US GOVT MMKT FUND INST SHARES - FUND 3915')">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JPM_Mail'"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL7)"/>

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
							<xsl:value-of select="COL38"/>
						</Symbol>						

						<EndingQuantity>
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
						</EndingQuantity>

						<TradeDate>
							<xsl:value-of select="''"/>
						</TradeDate>
					
				</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>
		
	</xsl:template>

</xsl:stylesheet>