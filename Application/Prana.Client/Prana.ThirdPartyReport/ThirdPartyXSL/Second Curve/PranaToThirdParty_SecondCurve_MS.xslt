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
        <!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
        <xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
          <!-- ...buid a Group for this node_id -->
          <xsl:call-template name="TaxLotIDBuilder">
            <xsl:with-param name="I_GroupID">
              <xsl:value-of select="PBUniqueID" />
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
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
    </xsl:variable>
    <xsl:variable name="CommissionSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
    </xsl:variable>
    <xsl:variable name="TaxOnCommissionSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TaxOnCommissions)"/>
    </xsl:variable>
    <xsl:variable name="StampDutySum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/StampDuty)"/>
    </xsl:variable>
    <xsl:variable name="TransactionLevySum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TransactionLevy)"/>
    </xsl:variable>
    <xsl:variable name="ClearingFeeSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/ClearingFee)"/>
    </xsl:variable>
    <xsl:variable name="MiscFeesSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/MiscFees)"/>
    </xsl:variable>
    <xsl:variable name="OtherBrokerFeeSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/OtherBrokerFee)"/>
    </xsl:variable>
    <xsl:variable name="GrossAmountSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/GrossAmount)"/>
    </xsl:variable>
    <xsl:variable name="NetAmountSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/NetAmount)"/>
    </xsl:variable>
    <!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
    <xsl:variable name="tempSideVar">
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
    </xsl:variable>

    <xsl:variable name="tempCPVar">
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CounterParty"/>
    </xsl:variable>

    <xsl:variable name="CPVar">
      <xsl:choose>
        <xsl:when test="$tempCPVar='CUTTONE' or $tempCPVar='CUTN'">CUTE</xsl:when>
        <xsl:when test="$tempCPVar='SAND'">SDLR</xsl:when>
        <xsl:when test="$tempCPVar='ISGI'">INTS</xsl:when>
        <xsl:otherwise>
          <xsl:value-of  select="$tempCPVar"/>
        </xsl:otherwise>
      </xsl:choose>
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
    <xsl:variable name="tempTaxlotStateVar">
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotStateID>1]/TaxLotState"/>
    </xsl:variable>

    <xsl:variable name="varTaxlotStateGrp">
      <xsl:choose>
        <xsl:when test="$tempTaxlotStateVar != ''">COR</xsl:when>
        <!--<xsl:when test="$tempTaxlotStateVar='Amemded' or $tempTaxlotStateVar='Deleted'">COR</xsl:when>-->
        <xsl:otherwise>NEW</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varSymbol">
      <xsl:choose>
        <xsl:when test="Asset='EquityOption'">
          <xsl:variable name ="varSymbolBef" select ="substring-before(Symbol,' ')"/>
          <xsl:variable name ="varSymbolAft" select ="substring-after(Symbol,' ')"/>
          <xsl:value-of select="concat($varSymbolBef,'/',$varSymbolAft)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="Symbol"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <Group 
			transType = "TR001" TransStatus="{$varTaxlotStateGrp}" BuySell="" LongShort="" PosType="{$Sidevar}"
			translevel="B" ClientRef="{PBUniqueID}" Associated="{PBUniqueID}" ExecAccount="038325502" CustAccount="038325502"
			ExecBkr="{$CPVar}" SecType="T" SecID="{$varSymbol}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
			CCY="{CurrencySymbol}"	ExCode="" qty="{$QtySum}" 	Price="{format-number(AveragePrice,'###.0000000')}"	type="G" prin="{$GrossAmountSum}" 
			comm="{$CommissionSum + $TaxOnCommissionSum}" comtype="F" Othercharges="0" Taxfees="{$StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum}"
			feesind="F" interest="0" interestindicator="" netamount="{$NetAmountSum}" hsyind="" custbkr="" mmgr="" bookid=""
			dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="" acqdate="" instx=""
			EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="">


      <!-- ...selecting all the records for this Group... -->
      <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
        <!-- ...and building a ThirdPartyFlatFileDetail for each -->
        <xsl:variable name="taxLotIDVar" select="EntityID"/>

        <xsl:variable name="varTaxlotStateTx">
          <xsl:choose>
            <xsl:when test="TaxLotState='Allocated'">
              <xsl:value-of select ="'NEW'"/>
            </xsl:when>
            <xsl:when test="TaxLotState='Amemded'">
              <xsl:value-of select ="'COR'"/>
            </xsl:when>
            <xsl:when test="TaxLotState='Deleted'">
              <xsl:value-of select ="'CAN'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select ="'NEW'"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <ThirdPartyFlatFileDetail
					 Group_Id=""  transType = "TR001" TransStatus="{$varTaxlotStateTx}" BuySell="" LongShort="" PosType="{$Sidevar}"
					translevel="A" ClientRef="{TradeRefID}" Associated="{PBUniqueID}" ExecAccount="038325502" CustAccount="{FundMappedName}"
					ExecBkr="{CounterParty}" SecType="T" SecID="{$varSymbol}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
					CCY="{CurrencySymbol}"	ExCode="" qty="{AllocatedQty}" Price="{format-number(AveragePrice,'###.0000000')}" type="G"	prin="{GrossAmount}" 				
					comm="{CommissionCharged + TaxOnCommissions}" comtype="F" Othercharges="0" Taxfees="{StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee}"
					feesind="F" interest="0" interestindicator="" netamount="{NetAmount}" hsyind="" custbkr="" mmgr="" bookid=""
					dealid="" taxlotid="" taxdate="" taxprice="" closeoutmethod="" exrate="" acqdate="" instx=""
					EntityID="{EntityID}" TaxLotState="{TaxLotState}"/>
      </xsl:for-each>
    </Group>
  </xsl:template>
</xsl:stylesheet>
