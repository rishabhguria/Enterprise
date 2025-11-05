<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	
	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="CashInt" select="substring(COL1,63,10)"/>
				<xsl:variable name="CashDeci" select="substring(COL1,73,2)"/>

				<xsl:variable name ="Cash">
					<xsl:value-of select="format-number(concat($CashInt,'.',$CashDeci),'#.00')"/>
				</xsl:variable>

				<xsl:if test="number($Cash)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Pershing'"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="substring(COL1,8,9)"/>

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
							<xsl:value-of select="'USD'"/>
						</Symbol>						

						<EndingQuantity>
							<xsl:choose>

								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="number($Cash)"/>
								</xsl:when>

								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="number($Cash) * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</EndingQuantity>

						<xsl:variable name="Date" select="substring(COL1,55,8)"/>

						<TradeDate>
							<xsl:value-of select="concat(substring($Date,5,2),'/',substring($Date,7,2),'/',substring($Date,1,4))"/>
						</TradeDate>
					
				</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>
		
	</xsl:template>

</xsl:stylesheet>