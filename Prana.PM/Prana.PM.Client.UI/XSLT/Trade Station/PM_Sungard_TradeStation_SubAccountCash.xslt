<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if  test="normalize-space(substring(COL1,17,5))!=''">

					<PositionMaster>
						<!--FundNameSection -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="substring(COL1,3,8)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SUNGARD']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<FundName>
							<xsl:value-of select='$PRANA_FUND_NAME'/>
						</FundName>

						<xsl:variable name="PRANA_FUND_ID">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SUNGARD']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFundID"/>
						</xsl:variable>

						<FundID>
							<xsl:value-of select="$PRANA_FUND_ID"/>
						</FundID>

						<!--Sub account Section-->
						<xsl:variable name = "PB_SUBAC_NAME" >
							<xsl:value-of select="concat(substring(COL1,45,1),substring(COL1,116,2),substring(COL1,190,2),substring(COL1,287,6))"/>
						</xsl:variable>
						<!--<xsl:variable name = "PB_SUBAC_NAME" >
							<xsl:value-of select=""/>
						</xsl:variable>-->

						<xsl:variable name="PRANA_SUBAC_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SubAccountMapping.xml')/SubAccountMapping/PB[@Name='SUNGARD']/SubAccountData[@PBSubAccountCode=$PB_SUBAC_NAME]/@PranaSubAccount"/>
						</xsl:variable>
						<ActivityType>
							<xsl:value-of select="$PB_SUBAC_NAME"/>
						</ActivityType>
						<PBDesc>
							<xsl:value-of select="substring(COL1,74,30)"/>
						</PBDesc>

						<SubACName>
							<xsl:value-of select='$PRANA_SUBAC_NAME'/>
						</SubACName>

						<xsl:variable name="PRANA_SUBAC_ID">
							<xsl:value-of select="document('../ReconMappingXml/SubAccountMapping.xml')/SubAccountMapping/PB[@Name='SUNGARD']/SubAccountData[@PBSubAccountCode=$PB_SUBAC_NAME]/@PranaSubAccountID"/>
						</xsl:variable>

						<SubACID>
							<xsl:value-of select="$PRANA_SUBAC_ID"/>
						</SubACID>

						<!--Settlement date-->
						<SettlementDate>
							<xsl:value-of select="substring(COL1,15,8)"/>
						</SettlementDate>

						<xsl:choose>
							<xsl:when test="substring(COL1,65,1)='+'">
								<CashValue>
									<xsl:value-of select="number(substring(COL1,49,16))"/>
								</CashValue>
							</xsl:when>
							<xsl:when test="substring(COL1,65,1)='-'">
								<CashValue>
									<xsl:value-of select="number(substring(COL1,49,16)) * (-1)"/>
								</CashValue>
							</xsl:when>
							<xsl:otherwise>
								<CashValue>
									<xsl:value-of select="0"/>
								</CashValue>
							</xsl:otherwise>

						</xsl:choose>


						<Currency>
							<xsl:value-of select="substring(COL1,11,3)"/>
						</Currency>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
