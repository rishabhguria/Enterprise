<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />

  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
    <Groups>
      <!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
      <!-- let's build a Group node for each different EntityID by   -->
      <!-- looping trough all the records...                         -->
      <xsl:for-each select="/NewDataSet/ThirdPartyFlatFileDetail[IsSwapped='1' and contains(AccountName,'CLVD')]">
        <!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
        <xsl:if test="(1=position()) or(preceding-sibling::*[1]/TaxLotID != TaxLotID)">
          <!-- ...buid a Group for this node_id -->

          <xsl:call-template name="TaxLotIDBuilder">
            <xsl:with-param name="I_TaxLotID">
              <xsl:value-of select="TaxLotID" />
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>

      </xsl:for-each>
    </Groups>
  </xsl:template>


  <xsl:template name="TaxLotIDBuilder">
    <xsl:param name="I_TaxLotID" />

    <xsl:variable name="AllocatedQty" />
    <!-- Building a Group with the EntityID $I_TaxLotID... -->

    <!--Total Quantity-->
    <xsl:variable name="QtySum">
      <xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/ClosedQty)"/>
    </xsl:variable>

    <xsl:variable name="QtySum1">
      <xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/ClosedQty)"/>
    </xsl:variable>

    <!--Total Commission-->
    <xsl:variable name="VarCommissionSum">
      <xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/CommissionCharged)"/>
    </xsl:variable>
    <!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID]/Symbol"/>
		</xsl:variable>-->
    <xsl:variable name="tempSideVar">
      <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID]/Side"/>
    </xsl:variable>
   
    <xsl:variable name="tempTaxlotStateVar">
      <xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotStateID>1]/TaxLotState"/>
    </xsl:variable>

    <xsl:variable name="varTransactionType">
      <xsl:choose>
        <xsl:when test ="Side='Sell' or Side='Buy to Close'">
          <xsl:value-of select="'Termination'"/>
        </xsl:when>
        <xsl:when test ="Side='Buy'">
          <xsl:value-of select="'Buy'"/>
        </xsl:when>
        <xsl:when test ="Side='Sell short' ">
          <xsl:value-of select="'Short'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varinvestment">
      <xsl:choose>
        <xsl:when test ="contains(BBCode,'Equity') or contains(BBCode,'EQUITY')">
          <xsl:value-of select="BBCode"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="concat(BBCode,' EQUITY')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

   

    <xsl:variable name="varInstruction">
      <xsl:choose>
        <xsl:when test="TaxLotState='Allocated'">
          <xsl:value-of select ="'NEW'"/>
        </xsl:when>
        <xsl:when test="TaxLotState='Amemded'">
          <xsl:value-of select ="'Amend'"/>
        </xsl:when>
        <xsl:when test="TaxLotState='Deleted'">
          <xsl:value-of select ="'CAN'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="'SENT'"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

  
    <xsl:variable name="varStrategy">
      <xsl:choose>
        <xsl:when test="Level2ID=0">
          <xsl:value-of select="'Strategy Unallocated'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varSpotRate">     
        <xsl:choose>
          <xsl:when test="CurrencySymbol ='AUD' or CurrencySymbol ='EUR' or CurrencySymbol ='GBP' or CurrencySymbol ='NZD'">
            <xsl:value-of select="format-number(FXRate,'0.########')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="format-number(1 div FXRate,'0.########')"/>
          </xsl:otherwise>
        </xsl:choose>     
    </xsl:variable>

    <xsl:variable name="varUserTranId1">
      <xsl:value-of select="concat('A',UserTranId)"/>
    </xsl:variable>

   


    <xsl:variable name ="varSwaptransationCode">
      <xsl:choose>
        <xsl:when test ="IsSwapped='1'">
          <xsl:value-of select="'EquitySwap'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="'Equity'"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="varCommission">
      <xsl:choose>
        <xsl:when test ="number(CommissionCharged)">
          <xsl:value-of select="format-number(CommissionCharged,'##.##')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of  select="0"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>


    <xsl:variable name="varNetamount">
      <xsl:choose>
        <xsl:when test="contains(Side,'Buy')">
          <xsl:value-of select="(AllocatedQty * AveragePrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFee + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
        </xsl:when>
        <xsl:when test="contains(Side,'Sell')">
          <xsl:value-of select="(AllocatedQty * AveragePrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFee + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

   

    <Group
				RecordAction ="{$varInstruction}" RecordType ="{$varTransactionType}" Portfolio="CLOVET" Investment="{$varinvestment}"
				LocationAccount="{AccountName}" Strategy="{$varStrategy}" Quantity="{AllocatedQty}" Price="{AveragePrice}" Broker="{CounterParty}"
				EventDate = "{TradeDate}" SettleDate = "{SettlementDate}" ActualSettleDate="{SettlementDate}"
        SecFeeAmount="{SecFee}" NetCounterAmount="" BuyAmount="" SellAmount=""
        SpotRate="{$varSpotRate}" NetInvestmentAmount="{format-number($varNetamount,'##.####')}" TotCommission="{$varCommission}" UserTranId1="{$varUserTranId1}"        
        BuyCurrency="" SellCurrency="" CounterFXDenomination="{CurrencySymbol}" TradeFX="{$varSpotRate}" SEDOL="{SEDOLSymbol}" SecurityType="Equity"
        SecurityDescription="{CompanyName}" Currency="{CurrencySymbol}" MaturityDate="" FinancingRate="" FinancingType=""
        SwapTransactionCode="{$varSwaptransationCode}"
        EntityID="{EntityID}" TaxLotState="{TaxLotState}">		
    </Group>
  </xsl:template>
</xsl:stylesheet>
