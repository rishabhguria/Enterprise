<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Cash" select="COL12"/>

				<xsl:if test="number($Cash) and normalize-space(COL4)='CASH EQUIVALENTS' and contains(COL1,'41150012870533')!='true'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'US TRUST'"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="substring(COL1,string-length(COL1)-6)"/>

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

						<Date>
							<xsl:value-of select="''"/>
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