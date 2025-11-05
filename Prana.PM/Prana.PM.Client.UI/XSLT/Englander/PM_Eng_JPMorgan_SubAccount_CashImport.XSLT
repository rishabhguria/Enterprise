<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="http://www.contoso.com">

	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:if test="substring(COL1,9,1)='1'">

					<PositionMaster>
						<!--FundNameSection -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(substring(COL1,4,5))"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ATRADING']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>


						<FundName>
							<xsl:value-of select='$PRANA_FUND_NAME'/>
						</FundName>

						<xsl:variable name="PRANA_FUND_ID">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ATRADING']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFundID"/>
						</xsl:variable>

						<FundID>
							<xsl:value-of select="$PRANA_FUND_ID"/>
						</FundID>

						<!--Sub account Section-->
						<xsl:variable name = "PB_SUBAC_NAME" >
							<xsl:value-of select="'SubAccount0'"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SUBAC_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SubAccountMapping.xml')/SubAccountMapping/PB[@Name='ATRADING']/SubAccountData[@PBSubAccountCode=$PB_SUBAC_NAME]/@PranaSubAccount"/>
						</xsl:variable>
						<ActivityType>
							<xsl:value-of select="'Cash Activity'"/>
						</ActivityType>
						
						
						<PBDesc>
							<xsl:value-of select="substring(COL1,34,30)"/>
						</PBDesc>

						<SubACName>
							<xsl:value-of select="$PB_SUBAC_NAME"/>
						</SubACName>

						<xsl:variable name="PRANA_SUBAC_ID">
							<xsl:value-of select="document('../ReconMappingXml/SubAccountMapping.xml')/SubAccountMapping/PB[@Name='ATRADING']/SubAccountData[@PBSubAccountCode=$PB_SUBAC_NAME]/@PranaSubAccountID"/>
						</xsl:variable>

						<SubACID>
							<xsl:value-of select="$PRANA_SUBAC_ID"/>
						</SubACID>

						<!--Settlement date-->
						<xsl:choose>
							<xsl:when test="normalize-space(substring(COL1,72,8))!=''">
								<SettlementDate>
									<xsl:value-of select="concat(substring(COL1,72,2),'/',substring(COL1,74,2),'/','20',substring(COL1,78,2))"/>
								</SettlementDate>
							</xsl:when>
							<xsl:otherwise>
								<SettlementDate>
									<xsl:value-of select="''"/>
								</SettlementDate>
							</xsl:otherwise>
						</xsl:choose>
						
						

						<!--Average Price Section-->

						<xsl:variable name ="varPrice">
							<xsl:value-of select ="substring(COL1,86,17)"/>
						</xsl:variable>

						<xsl:variable name ="varPriceInt">
							<xsl:value-of select ="number(substring($varPrice,1,9))"/>
						</xsl:variable>

						<xsl:variable name ="varPriceFrac">
							<xsl:value-of select ="number(substring($varPrice,10,7))"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="boolean(number($varPrice))">
								<CashValue>
									<xsl:value-of select="concat($varPriceInt,'.',$varPriceFrac)"/>
								</CashValue>
							</xsl:when>
							<xsl:otherwise>
								<CashValue>
									<xsl:value-of select="0"/>
								</CashValue>
							</xsl:otherwise>
						</xsl:choose>
						
											
						<!--Average Price Section-->
						
						
						
						<Currency>
							<xsl:value-of select="'USD'"/>
						</Currency>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
