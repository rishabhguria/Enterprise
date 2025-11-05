<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >

  <xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />

  <xsl:template match="/">
    <Groups>
      <!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
      <!-- let's build a Group node for each different EntityID by   -->
      <!-- looping trough all the records...                         -->
      <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail">
        <!-- ...and, if it is the first node OR this EntityID is != from
the previous... -->
        <xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
          <!-- ...buid a Group for this node_id -->
          <xsl:call-template name="TaxLotIDBuilder">
            <xsl:with-param name="I_GroupID">
              <xsl:value-of
select="PBUniqueID" />
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
      </xsl:for-each>
    </Groups>
  </xsl:template>


  <xsl:template name="TaxLotIDBuilder">
    <xsl:param name="I_GroupID" />
    <xsl:variable name="AllocatedQty" />
    <!-- Building a Group with the EntityID $I_GroupID... -->
    <xsl:variable name="QtySum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/AllocatedQty)"/>
    </xsl:variable>
    <xsl:variable name="AveragePrice">
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/AveragePrice"/>
    </xsl:variable>
    <xsl:variable name="tempSideVar">
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
    </xsl:variable>

    <xsl:variable name="tempCPVar">
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CounterParty"/>
    </xsl:variable>

    <xsl:variable name="Sidevar">
      <xsl:choose>
        <xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">BL</xsl:when>
        <xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">BC</xsl:when>
        <xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">SL</xsl:when>
        <xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">SS</xsl:when>
        <xsl:otherwise> </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="CPVar">
      <xsl:choose>
        <xsl:when test ="$tempCPVar='WEED'">WEEE</xsl:when>
        <xsl:when test ="$tempCPVar='CUTTONE' or $tempCPVar='CUTN'">CUTE</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$tempCPVar"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <Group IMP_ID="{position()}" ExecAccountID="38325502" SecurityID="{Symbol}" TransCode="{$Sidevar}"
			TradeDate="{TradeDate}" SettleDate="{SettlementDate}"	SettlementCurrency="{CurrencySymbol}"	
			Price="{$AveragePrice}"	BrokerCode="{$CPVar}"	Custodian="MSCO"	Quantity="{$QtySum}"	
			CommissionType="C"	Commission="{CommissionCharged}" AssetType="{Asset}"	PutCall="{PutOrCall}"	
			SecurityIDType="TICKER" EntityID="{EntityID}" TaxLotState="{TaxLotState}" />
  </xsl:template>
</xsl:stylesheet>
