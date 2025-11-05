<?xml version="1.0" encoding="UTF-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>	
			
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varSecurity_Type" select="normalize-space(substring(COL1,25,2))"/>
				<xsl:if test="$varSecurity_Type='CA'">
					<PositionMaster>
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(substring(COL1,1,14))"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ATRADING']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose >

						<BaseCurrency>
							<xsl:value-of select="'USD'"/>
						</BaseCurrency>

						<LocalCurrency>
							<xsl:value-of select="'USD'"/>
						</LocalCurrency>

						<xsl:choose>
							<xsl:when  test="number(normalize-space(substring(COL1,37,11)))">
								<CashValueBase>
									<xsl:value-of select="normalize-space(substring(COL1,37,11))"/>
								</CashValueBase>
                <CashValueLocal>
                  <xsl:value-of select="normalize-space(substring(COL1,37,11))"/>
                </CashValueLocal>
							</xsl:when >
							<xsl:otherwise>
								<CashValueBase>
									<xsl:value-of select="0"/>
								</CashValueBase>
                <CashValueLocal>
                  <xsl:value-of select="0"/>
                </CashValueLocal>
							</xsl:otherwise>
						</xsl:choose >

						<Date>
							<xsl:value-of select="concat('20',substring(COL1,22,2),'/',substring(COL1,18,2),'/',substring(COL1,20,2))"/>
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
