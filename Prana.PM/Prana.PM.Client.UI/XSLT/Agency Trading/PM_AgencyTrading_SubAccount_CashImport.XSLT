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

				<xsl:variable name="varSecurity_Type" select="normalize-space(substring(COL1,25,2))"/>
				<xsl:variable name="varTransaction_type" select="normalize-space(substring(COL1,15,2))"/>
				<xsl:if test="($varTransaction_type='ai' or $varTransaction_type='dp' or $varTransaction_type='in' or $varTransaction_type='rc'or $varTransaction_type='wd') and ($varSecurity_Type='ba' or $varSecurity_Type='ca' or $varSecurity_Type='cd' or $varSecurity_Type='cm'or $varSecurity_Type='ep'or $varSecurity_Type='ex'or $varSecurity_Type='fc'or $varSecurity_Type='FF'or $varSecurity_Type='fh'or $varSecurity_Type='fm'or $varSecurity_Type='gm' or $varSecurity_Type='gp'or $varSecurity_Type='ic'or $varSecurity_Type='if'or $varSecurity_Type='ip'or $varSecurity_Type='pa'or $varSecurity_Type='to'or $varSecurity_Type='tp'or $varSecurity_Type='ut'or $varSecurity_Type='VF')">

		
						<PositionMaster>
						<!--FundNameSection -->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(substring(COL1,1,14))"/>
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
							<xsl:value-of select="'Cash'"/>
						</ActivityType>
						<PBDesc>
							<xsl:value-of select="'Description'"/>
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
						<SettlementDate>
							<xsl:value-of select="concat('20',substring(COL1,91,2),'/',substring(COL1,87,2),'/',substring(COL1,89,2))"/>
						</SettlementDate>

						<!--Average Price Section-->
						<xsl:choose>
							<xsl:when  test="boolean(number(normalize-space(substring(COL1,37,11))))">
								<CashValue>
									<xsl:value-of select="normalize-space(substring(COL1,37,11)) "/>
								</CashValue>
							</xsl:when >
							<xsl:otherwise>
								<CashValue>
									<xsl:value-of select="0"/>
								</CashValue>
							</xsl:otherwise>
						</xsl:choose >
						<!--Average Price Section-->
						
						
						
						<Currency>
							<xsl:value-of select="'USD'"/>
						</Currency>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
