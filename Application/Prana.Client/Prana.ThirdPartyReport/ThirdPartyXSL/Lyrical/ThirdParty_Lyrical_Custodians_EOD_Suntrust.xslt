<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

<xsl:template match="/ThirdPartyFlatFileDetailCollection">
<ThirdPartyFlatFileDetailCollection>
<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

<ThirdPartyFlatFileDetail>

<RowHeader>
<xsl:value-of select ="'true'"/>
</RowHeader>

<IsCaptionChangeRequired>
<xsl:value-of select ="'true'"/>
</IsCaptionChangeRequired>

<TaxLotState>
<xsl:value-of select="'TaxLotState'"/>
</TaxLotState>

<CUSIP>
<xsl:value-of select="'CUSIP'"/>
</CUSIP>

<CODE>
<xsl:value-of select="'CODE'"/>
</CODE>

<UNITS>
<xsl:value-of select="'UNITS'"/>
</UNITS>

<PRSHR>
<xsl:value-of select="'PRSHR'"/>
</PRSHR>

<BROKER>
<xsl:value-of select="'BROKER'"/>
</BROKER>

<TRADDT>
<xsl:value-of select="'TRADDT'"/>
</TRADDT>

<CONTDT>
<xsl:value-of select="'CONTDT'"/>
</CONTDT>

<COMMS>
<xsl:value-of select="'COMMS'"/>
</COMMS>

<SECFees>
<xsl:value-of select="'S.E.C. Fees'"/>
</SECFees>

<NET>
<xsl:value-of select="'NET'"/>
</NET>

<TICKER>
<xsl:value-of select="'TICKER'"/>
</TICKER>

<SECURITYDESCRIPTION>
<xsl:value-of select="'SECURITYDESCRIPTION'"/>
</SECURITYDESCRIPTION>

<PrincipalAmount>
<xsl:value-of select ="'Principal Amount '"/>
</PrincipalAmount>

<AccountID>
<xsl:value-of select ="'Account ID'"/>
</AccountID>


<!-- system use only-->
<EntityID>
<xsl:value-of select="'EntityID'"/>
</EntityID>

</ThirdPartyFlatFileDetail>



<xsl:for-each select="ThirdPartyFlatFileDetail">
<ThirdPartyFlatFileDetail>
<RowHeader>
<xsl:value-of select ="'true'"/>
</RowHeader>

<IsCaptionChangeRequired>
<xsl:value-of select ="'true'"/>
</IsCaptionChangeRequired>

<TaxLotState>
<xsl:value-of select="TaxLotState"/>
</TaxLotState>

<!--<xsl:variable name = "PRANA_FUND_NAME">
<xsl:value-of select="FundName"/>
</xsl:variable>

<xsl:variable name ="PB_FUND_CODE">
<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_FundMapping.xml')/FundMapping/PB[@Name='Lyrical']/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
</xsl:variable>-->

<CUSIP>
<xsl:value-of select="concat(concat('=&quot;',CUSIP),'&quot;')"/>
</CUSIP>

<CODE>
<xsl:value-of select="translate(Side,$varSmall,$varCapital)"/>
</CODE>

<UNITS>
<xsl:value-of select="AllocatedQty"/>
</UNITS>

<PRSHR>
<xsl:value-of select="AveragePrice"/>
</PRSHR>

<BROKER>
<xsl:value-of select="CounterParty"/>
</BROKER>

<TRADDT>
<xsl:value-of select="concat(substring(TradeDate,1,6),substring(TradeDate,9,2))"/>
</TRADDT>

<CONTDT>
<xsl:value-of select="concat(substring(SettlementDate,1,6),substring(SettlementDate,9,2))"/>
</CONTDT>

<COMMS>
<xsl:value-of select="CommissionCharged"/>
</COMMS>

<SECFees>
<xsl:value-of select="StampDuty"/>
</SECFees>

<NET>
<xsl:value-of select="NetAmount"/>
</NET>

<TICKER>
<xsl:value-of select="Symbol"/>
</TICKER>

<SECURITYDESCRIPTION>
<xsl:value-of select="FullSecurityName"/>
</SECURITYDESCRIPTION>

<PrincipalAmount>
<xsl:value-of select ="GrossAmount"/>
</PrincipalAmount>

	<xsl:variable name = "PRANA_FUND_NAME">
		<xsl:value-of select="AccountName"/>
	</xsl:variable>

	<xsl:variable name ="PB_FUND_CODE">
		<xsl:value-of select ="document('../ReconMappingXml/EOD_Mapping.xml')/FundMapping/PB[@Name='EOD']/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/>
	</xsl:variable>

	<AccountID>

		<xsl:choose>
			<xsl:when test ="$PB_FUND_CODE!=''">
				<xsl:value-of select ="$PB_FUND_CODE"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>

</AccountID>


<!-- system use only-->
<EntityID>
<xsl:value-of select="EntityID"/>
</EntityID>
</ThirdPartyFlatFileDetail>

</xsl:for-each>
</ThirdPartyFlatFileDetailCollection>
</xsl:template>
<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
