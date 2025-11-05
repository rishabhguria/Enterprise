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

    <xsl:variable name="varAveragePrice">
      <xsl:value-of select="format-number(AveragePrice,'#.0000')"/>
    </xsl:variable>

    <xsl:variable name="varGrossAmount">
      <xsl:value-of select="AllocatedQty*$varAveragePrice*Multiplier"/>
    </xsl:variable>

    <xsl:variable name="varOtherCharges">
      <xsl:value-of select="CommissionCharged+OtherBrokerFee+StampDuty+TransactionLevy+ClearingFee+TaxOnCommissions+MiscFees"/>
    </xsl:variable>

    <xsl:variable name="varSideMul">
      <xsl:choose>
        <xsl:when test="SideTag = '5' or SideTag = 'C' or SideTag = '2' ">
          <xsl:value-of select="-1"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="1"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varNetAmmount">
      <xsl:value-of select="$varGrossAmount + $varOtherCharges*($varSideMul)"/>
    </xsl:variable>
    
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

	  <xsl:variable name="Pb_Name" select="'Mizuho'"/>

	  <xsl:variable name="ThirdParty_CounterParty">
		  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$Pb_Name]/BrokerData[@PranaBrokerName=$tempCPVar]/@PBBrokerName"/>
	  </xsl:variable>

	  <xsl:variable name="CP">
		  <xsl:choose>
			  <xsl:when test ="$ThirdParty_CounterParty != ''">
				  <xsl:value-of select ="$ThirdParty_CounterParty"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="$tempCPVar"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="Side">
		  <xsl:choose>
			  <xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">
				  <xsl:value-of select="'B'"/>
			  </xsl:when>
			  <xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">
				  <xsl:value-of select="'S'"/>
			  </xsl:when>
			  <xsl:when test="$tempSideVar='BuSell short' or $tempSideVar='Sell to Open'">
				  <xsl:value-of select="'SS'"/>
			  </xsl:when>
			  <xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">
				  <xsl:value-of select="'BC'"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="Symbol">
		  <xsl:value-of select="concat(Symbol,' Equity')"/>
	  </xsl:variable>

	  <xsl:variable name="AveragePrice">
		  <xsl:choose>
			  <xsl:when test="AveragePrice &gt; 0">
				  <xsl:value-of select="AveragePrice"/>
			  </xsl:when>
			  <xsl:when test="AveragePrice &lt; 0">
				  <xsl:value-of select="AveragePrice * (-1)"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="'-'"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="GrossAmountSumG">
		  <xsl:choose>
			  <xsl:when test="$GrossAmountSum &gt; 0">
				  <xsl:value-of select="$GrossAmountSum"/>
			  </xsl:when>
			  <xsl:when test="$GrossAmountSum &lt; 0">
				  <xsl:value-of select="$GrossAmountSum * (-1)"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="'-'"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="CommissionSumG">
		  <xsl:choose>
			  <xsl:when test="$CommissionSum &gt; 0">
				  <xsl:value-of select="$CommissionSum"/>
			  </xsl:when>
			  <xsl:when test="$CommissionSum &lt; 0">
				  <xsl:value-of select="$CommissionSum * (-1)"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="'-'"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="StampDutySumG">
		  <xsl:choose>
			  <xsl:when test="$StampDutySum &gt; 0">
				  <xsl:value-of select="$StampDutySum"/>
			  </xsl:when>
			  <xsl:when test="$StampDutySum &lt; 0">
				  <xsl:value-of select="$StampDutySum * (-1)"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="'-'"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="NetAmountSumG">
		  <xsl:choose>
			  <xsl:when test="$NetAmountSum &gt; 0">
				  <xsl:value-of select="$NetAmountSum"/>
			  </xsl:when>
			  <xsl:when test="$NetAmountSum &lt; 0">
				  <xsl:value-of select="$NetAmountSum * (-1)"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="'-'"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <Group 
		RowHeader="true" Tradedate="{TradeDate}" Settledate="{SettlementDate}" Name="{FullSecurityName}" Ticker="{$Symbol}"
		Cusip="{CUSIP}" Side="{$Side}" Shares="{$QtySum}"
		Tradeprice="{concat('$',$AveragePrice)}" Principal="{concat('$',$GrossAmountSumG)}" Commission="{concat('$',$CommissionSumG)}" SECfee="{concat('$',$StampDutySumG)}"
		Netmoney="{concat('$',$NetAmountSumG)}" Broker="{$CP}" DTC="0226"
		EntityID="{EntityID}" TaxLotState="{TaxLotState}">


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

		  <xsl:variable name="PB_NAME" select="'JPM'"/>

		  <xsl:variable name="PRANA_FUND_NAME" select="AccountName"/>

		  <xsl:variable name="THIRDPARTY_FUND_NAME">
			  <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
		  </xsl:variable>

		  <xsl:variable name="AccountId">
			  <xsl:choose>
				  <xsl:when test="$THIRDPARTY_FUND_NAME!=''">
					  <xsl:value-of select="$THIRDPARTY_FUND_NAME"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="$PRANA_FUND_NAME"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </xsl:variable>

		  <xsl:variable name="GrossAmount">
			  <xsl:choose>
				  <xsl:when test="GrossAmount &gt; 0">
					  <xsl:value-of select="GrossAmount"/>
				  </xsl:when>
				  <xsl:when test="GrossAmount &lt; 0">
					  <xsl:value-of select="GrossAmount * (-1)"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="'-'"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </xsl:variable>

		  <xsl:variable name="CommissionCharged">
			  <xsl:choose>
				  <xsl:when test="CommissionCharged &gt; 0">
					  <xsl:value-of select="CommissionCharged"/>
				  </xsl:when>
				  <xsl:when test="CommissionCharged &lt; 0">
					  <xsl:value-of select="CommissionCharged * (-1)"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="'-'"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </xsl:variable>

		  <xsl:variable name="StampDuty">
			  <xsl:choose>
				  <xsl:when test="StampDuty &gt; 0">
					  <xsl:value-of select="StampDuty"/>
				  </xsl:when>
				  <xsl:when test="StampDuty &lt; 0">
					  <xsl:value-of select="StampDuty * (-1)"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="'-'"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </xsl:variable>

		  <xsl:variable name="NetAmount">
			  <xsl:choose>
				  <xsl:when test="NetAmount &gt; 0">
					  <xsl:value-of select="NetAmount"/>
				  </xsl:when>
				  <xsl:when test="NetAmount &lt; 0">
					  <xsl:value-of select="NetAmount * (-1)"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="'-'"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </xsl:variable>

        <ThirdPartyFlatFileDetail
			RowHeader="true" TaxLotState="{TaxLotState}" Tradedate="{TradeDate}" Settledate="{SettlementDate}" Name="{FullSecurityName}" Ticker="{$Symbol}"
			Cusip="{CUSIP}" Side="{$Side}" Shares="{AllocatedQty}"
			Tradeprice="{concat('$',$AveragePrice)}" Principal="{concat('$',$GrossAmount)}" Commission="{concat('$',$CommissionCharged)}" SECfee="{concat('$',$StampDuty)}"
			Netmoney="{concat('$',$NetAmount)}" Broker="{$CP}" DTC="{$AccountId}"
			EntityID="{EntityID}" />
	</xsl:for-each>
</Group>
</xsl:template>
</xsl:stylesheet>
