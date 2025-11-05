<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
	<xsl:output method="xml" indent="yes"/>

	<xsl:template match="/">
		
		<DocumentElement>
			
			<xsl:for-each select="//PositionMaster">
				
				<xsl:variable name="Cash" select="COL8"/>
				
				<xsl:if test="number($Cash)">
					
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Pershing'"/>
						</xsl:variable>
						
						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>
						
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<CashValueBase>
							<xsl:value-of select="0"/>
						</CashValueBase>
						
						<CashValueLocal>
							<xsl:choose>
								<xsl:when test="number($Cash)">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CashValueLocal>
						
						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>
						
						<LocalCurrency>
							<xsl:value-of select="'USD'"/>
						</LocalCurrency>

						<PositionType>
							<xsl:value-of select="'Cash'"/>
						</PositionType>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>
</xsl:stylesheet>
