<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
  <xsl:template match="/">
    <Groups>
      <!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
      <!-- let's build a Group node for each different EntityID by   -->
      <!-- looping trough all the records...                         -->
      <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[CounterParty='PERS']">
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

  <xsl:template name="GetMonth">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month = 1" >
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month = 2" >
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month = 3" >
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month = 4" >
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month = 5" >
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month = 6" >
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month = 7" >
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month = 8" >
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month = 9" >
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month = 10" >
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month = 11" >
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month = 12" >
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="TaxLotIDBuilder">
    <xsl:param name="I_GroupID" />



    <xsl:variable name="AllocatedQty" />
    <!-- Building a Group with the EntityID $I_GroupID... -->
    <xsl:variable name="QtySum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
    </xsl:variable>
    <xsl:variable name="GroupNetAmt">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/NetAmount)"/>
    </xsl:variable>
    <xsl:variable name="CommissionSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
    </xsl:variable>
    <xsl:variable name="TaxOnCommissionSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/TaxOnCommissions)"/>
    </xsl:variable>
    <xsl:variable name="SecFeeSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/SecFee)"/>
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
    <xsl:variable name="OrfFeeSum">
      <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/OrfFee)"/>
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

    <xsl:variable name="PB_NAME" select="'MS'"/>

    <xsl:variable name="PB_COUNTERPARTY_NAME">
      <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$tempCPVar]/@ThirdPartyBrokerID"/>
    </xsl:variable>


    <xsl:variable name="LOCALREF">
		<!--<xsl:value-of select="concat('PHW',substring(substring-before(TradeDate,'/'),2,1),substring(PBUniqueID,string-length(PBUniqueID)-3),substring(CompanyFundID,string-length(CompanyFundID)-1))"/>-->
		<xsl:value-of select ="concat('PHW',PBUniqueID)"/>
	</xsl:variable>
    <xsl:variable name="CFID">
		<!--<xsl:value-of select="concat('PHW',substring(substring-before(TradeDate,'/'),2,1),substring(PBUniqueID,string-length(PBUniqueID)-3),substring(CompanyFundID,string-length(CompanyFundID)-1))"/>-->
		<xsl:value-of select="$LOCALREF"/>
    </xsl:variable>
	  
    <xsl:variable name="TIRORDERID">
      <xsl:value-of select="concat('BLK', PBUniqueID)"/>
    </xsl:variable>
    <xsl:variable name="SECIDTYPE">
      <xsl:choose>
        <xsl:when test="CUSIP!=''">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="Symbol!=''">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="ISIN!=''">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="'O'"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="SECURITYID">
      <xsl:choose>
        <xsl:when test="CUSIP!=''">
          <xsl:value-of select="CUSIP"/>
        </xsl:when>
        <xsl:when test="Symbol!=''">
          <xsl:value-of select="Symbol"/>
        </xsl:when>
        <xsl:when test="ISIN!=''">
          <xsl:value-of select="ISIN"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="OSIOptionSymbol"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="DESCRIPTION">
      <xsl:value-of select="concat('PB',' ',CounterParty)"/>
    </xsl:variable>

	  <xsl:variable name="DESCRIPTION1">
		  <xsl:value-of select="CounterParty"/>
	  </xsl:variable>

    <xsl:variable name="varTradeMonth">
      <xsl:call-template name="GetMonth">
        <xsl:with-param name="Month" select="substring(TradeDate,1,2)"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="varSettleMonth">
      <xsl:call-template name="GetMonth">
        <xsl:with-param name="Month" select="substring(SettlementDate,1,2)"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="TRADEDATE">
      <xsl:value-of select="concat(substring(TradeDate,4,2),'-',$varTradeMonth,'-',substring(TradeDate,9))"/>
    </xsl:variable>

    <xsl:variable name="SETLDATE">
      <xsl:value-of select="concat(substring(SettlementDate,4,2),'-',$varSettleMonth,'-',substring(SettlementDate,9))"/>
    </xsl:variable>

    <xsl:variable name="CashAccount">
      <!--<xsl:value-of select="concat(FundAccountNo,'2')"/>-->
      <xsl:value-of select="'PHW8971232'"/>
    </xsl:variable>

    <xsl:variable name="SECACCOUNT">

      <xsl:value-of select="'3DT8226900'"/>

    </xsl:variable>

    <xsl:variable name="BSIND">
      <xsl:choose>
        <xsl:when test="contains(Side,'Buy')">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="contains(Side,'Sell')">
          <xsl:value-of select="'B'"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="INSTTYP">
      <xsl:choose>
        <xsl:when test="TaxLotState = 'Allocated'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="'Y'"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    
    <Group
        LOCALREF = "{$LOCALREF}" CFID="{$CFID}" ROUTECD="PSHG" TIRORDERID="{$TIRORDERID}" TIRPIECE=""
        TIRSEQ="" SECIDTYPE="{$SECIDTYPE}" SECURITYID="{$SECURITYID}" DESCRIPTION1="{$DESCRIPTION}" DESCRIPTION2=""
        DESCRIPTION3="" DESCRIPTION4="" TRADEDATE="{$TRADEDATE}" SETLDATE="{$SETLDATE}" QUANTITY="{$QtySum}" QUANTITYDESC=""
        NETMONEY=""	CASHACCOUNT="{$CashAccount}" SECACCOUNT="{$SECACCOUNT}" 	TRADECURRID="USD"	SETLCURRID="USD" BSIND="{$BSIND}"
        INSTTYP="{$INSTTYP}" PRICE="{AveragePrice}"
        COMMISSION="{$CommissionSum}" STAMPTAX=""
        LOCALCHGS="" INTEREST="" PRINCIPAL="" SECFEE="" EXECBROKER="" BROKEROS="" TRAILERCD1="" TRAILERCD2=""
        TRAILERCD3="" BLOTTERCD="49" CLRNGHSE="Y" CLRAGNTCD="{$DESCRIPTION1}" CLRAGNT1="" CLRAGNT2="" CLRAGNT3="" CLRAGNT4="" CNTRPRTYCD="" CNTRPTY1="" CNTRPTY2="" CNTRPTY3="" CNTRPTY4=""
        INSTRUCT="" CEDELAKV="" ORIGLOCALREF="{$LOCALREF}" NOTES="" FILLER1="" FILLER2="" RR="" SETLCOUNTRYCD="US" INSTRUMENTTYPE="" COMMISSIONRATE="" COMPANYNO="" Filler3="" Filler4=""
        Filler5="" Filler6="" Filler7="" GPF2IDCode="" GPF2Amount="" GPF2CurrencyCode="" GPF2AddSubtract="" GPF3IDCode="" GPF3Amount="" GPF3CurrencyCode="" GPF3AddSubtract=""
        GPF4IDCode="" GPF4Amount="" GPF4CurrencyCode="" GPF4AddSubtract="" GPF5IDCode="" GPF5Amount="" GPF5CurrencyCode="" GPF5AddSubtract=""
        EntityID="{EntityID}" TaxLotState="{TaxLotState}" RowHeader="true">


      <!-- ...selecting all the records for this Group... -->
      <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
        <!-- ...and building a ThirdPartyFlatFileDetail for each -->
        <!--<xsl:if test="TaxLotState!='Deleted'">-->

        <xsl:variable name="taxLotIDVar" select="EntityID"/>

        <xsl:variable name="varTaxlotState">
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
              <xsl:value-of select ="''"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="TaxlotGrsAmt">
          <xsl:value-of select="AllocatedQty * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
        </xsl:variable>

        <xsl:variable name="TaxlotSideMultiplier">
          <xsl:choose>
            <xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open' or $tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">
              <xsl:value-of select="1"/>
            </xsl:when>
            <xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close' or $tempSideVar='Sell short' or $tempSideVar='Sell to Open'">
              <xsl:value-of select="-1"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="0"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="TaxlotNetAmt">
          <xsl:value-of select="$TaxlotGrsAmt + ((CommissionCharged + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions) * $TaxlotSideMultiplier)"/>
        </xsl:variable>

        <xsl:variable name="PRANA_FUND_NAME" select="FundName"/>

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

        <xsl:variable name="Stamp">
          <xsl:value-of select="GrossAmount * 0.0000184"/>
        </xsl:variable>

        <xsl:variable name="Orf">
          <xsl:value-of select="AllocatedQty * 0.04"/>
        </xsl:variable>

        <xsl:variable name="TaxFees">
          <xsl:choose>
            <xsl:when test="number(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee) &gt; 0">
              <xsl:value-of select="StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee"/>
            </xsl:when>
            <xsl:when test="number(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee) &lt; 0">
              <xsl:value-of select="(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee) * -1"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="0"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="BSIND1">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="'B'"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="'S'"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>

		  <xsl:variable name="LOCALREF_Allo">
			  <xsl:value-of select="concat('PHW',substring(substring-before(TradeDate,'/'),2,1),substring(PBUniqueID,string-length(PBUniqueID)-3),substring(CompanyFundID,string-length(CompanyFundID)-1))"/>
		  </xsl:variable>

		  <xsl:variable name="SECACCOUNT_Allo">
			  <xsl:value-of select="concat(FundAccountNo,'2')"/>
		  </xsl:variable>


        <xsl:variable name="amp">N</xsl:variable>

        <ThirdPartyFlatFileDetail
        TaxLotState="{TaxLotState}" LOCALREF = "{$LOCALREF_Allo}" CFID="{$LOCALREF_Allo}" ROUTECD="PSHG" TIRORDERID="{$TIRORDERID}" TIRPIECE=""
        TIRSEQ="" SECIDTYPE="{$SECIDTYPE}" SECURITYID="{$SECURITYID}" DESCRIPTION1="{$DESCRIPTION}" DESCRIPTION2=""
        DESCRIPTION3="" DESCRIPTION4="" TRADEDATE="{$TRADEDATE}" SETLDATE="{$SETLDATE}" QUANTITY="{AllocatedQty}" QUANTITYDESC=""
        NETMONEY=""	CASHACCOUNT="{$CashAccount}" SECACCOUNT="{$SECACCOUNT_Allo}" 	TRADECURRID="USD"	SETLCURRID="USD" BSIND="{$BSIND1}"
        INSTTYP="{$INSTTYP}" PRICE="{AveragePrice}"
        COMMISSION="{CommissionCharged}" STAMPTAX=""
        LOCALCHGS="" INTEREST="" PRINCIPAL="" SECFEE="" EXECBROKER="" BROKEROS="" TRAILERCD1="" TRAILERCD2=""
        TRAILERCD3="" BLOTTERCD="49" CLRNGHSE="Y" CLRAGNTCD="{$DESCRIPTION1}" CLRAGNT1="" CLRAGNT2="" CLRAGNT3="" CLRAGNT4="" CNTRPRTYCD="" CNTRPTY1="" CNTRPTY2="" CNTRPTY3="" CNTRPTY4=""
        INSTRUCT="" CEDELAKV="" ORIGLOCALREF="{$LOCALREF_Allo}" NOTES="" FILLER1="" FILLER2="" RR="" SETLCOUNTRYCD="US" INSTRUMENTTYPE="" COMMISSIONRATE="" COMPANYNO="" Filler3="" Filler4=""
        Filler5="" Filler6="" Filler7="" GPF2IDCode="" GPF2Amount="" GPF2CurrencyCode="" GPF2AddSubtract="" GPF3IDCode="" GPF3Amount="" GPF3CurrencyCode="" GPF3AddSubtract=""
        GPF4IDCode="" GPF4Amount="" GPF4CurrencyCode="" GPF4AddSubtract="" GPF5IDCode="" GPF5Amount="" GPF5CurrencyCode="" GPF5AddSubtract=""
        EntityID="{EntityID}" RowHeader="true"/>
        <!--</xsl:if>-->
      </xsl:for-each>

    </Group>









  </xsl:template>
</xsl:stylesheet>