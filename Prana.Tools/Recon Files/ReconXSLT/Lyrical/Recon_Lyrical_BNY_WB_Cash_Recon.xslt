<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">

	<DocumentElement>

		<xsl:for-each select ="//Comparision">

			<xsl:if test="COL50='CASH &amp; CASH EQUIVALENTS'">

				<PositionMaster>

					<Symbol>
						<xsl:value-of select="COL21"/>
					</Symbol>

					<xsl:variable name="PB_NAME">
						<xsl:value-of select="'BNY_WB'"/>
					</xsl:variable>

					<xsl:variable name="PB_FUND_NAME" select="COL1"/>

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

					<TradeDate>
						<xsl:value-of select="COL28"/>
					</TradeDate>

					<xsl:variable name="EndingQuantity" select="number(COL42)"/>

					<EndingQuantity>
						<xsl:choose>

							<xsl:when test="$EndingQuantity &gt; 0">
								<xsl:value-of select="$EndingQuantity"/>
							</xsl:when>

							<xsl:when test="$EndingQuantity &lt; 0">
								<xsl:value-of select="$EndingQuantity * (-1)"/>
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
