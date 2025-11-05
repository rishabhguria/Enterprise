<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
  <xsl:template match="/">
    <xsl:variable name="varValidAccount">

      <xsl:choose>
        <xsl:when test="AccountName='CRPR-MS-SWAP'">
          <xsl:value-of select ="'true'"/>
        </xsl:when>
        <xsl:when test="AccountName='CRPL-MS-SWAP'">
          <xsl:value-of select ="'true'"/>
        </xsl:when>
        <xsl:when test="AccountName='CRPI-MS-SWAP'">
          <xsl:value-of select ="'true'"/>
        </xsl:when>
        <xsl:when test="AccountName='ST-Main-MS-SWAP1'">
          <xsl:value-of select ="'true'"/>
        </xsl:when>
        <xsl:when test="AccountName='ST-Main-MS-SWAP2'">
          <xsl:value-of select ="'true'"/>
        </xsl:when>
        <xsl:when test="AccountName='ST-MONT-MS-SWAP'">
          <xsl:value-of select ="'true'"/>
        </xsl:when>
        <xsl:when test="AccountName='ST-PUFF-MS-SWAP'">
          <xsl:value-of select ="'true'"/>
        </xsl:when>
		<xsl:when test="AccountName='CRPA-MS-SWAP'">
          <xsl:value-of select ="'true'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="'false'"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <Groups>
      <!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
      <!-- let's build a Group node for each different EntityID by   -->
      <!-- looping trough all the records...                         -->
      <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotState!='Sent'][contains(AccountName,'SWAP')='true']
	  [ CounterParty!='PRVT' or CounterParty!='Transfer' or CounterParty!='BOX Collapse' or CounterParty!='CorpAction']">
        <!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
        <xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
          <!-- <xsl:if test="CounterParty = 'MSCO-S'"> -->
          <!-- ...buid a Group for this node_id -->
          <xsl:call-template name="TaxLotIDBuilder">
            <xsl:with-param name="I_GroupID">
              <xsl:value-of select="PBUniqueID" />
            </xsl:with-param>
          </xsl:call-template>
          <!-- </xsl:if> -->
        </xsl:if>
      </xsl:for-each>
    </Groups>
  </xsl:template>

  <xsl:template name="Conversion">
    <xsl:param name="Value"/>
    <xsl:param name="Curr"/>
    <xsl:choose>
      <xsl:when test="Asset='Equity' and IsSwapped='true'">
        <xsl:choose>
          <xsl:when test="FXConversionMethodOperator_Taxlot='M'">
            <xsl:choose>
              <xsl:when test="number(FXRate_Taxlot)">
                <xsl:value-of select="$Value * FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD'">
                    <xsl:value-of select="$Value * ForexRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$Value div ForexRate"/>
                  </xsl:otherwise>
                </xsl:choose>

              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:choose>
              <xsl:when test="number(FXRate_Taxlot)">
                <xsl:value-of select="$Value div FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="$Curr='GBP' or $Curr='EUR' or $Curr='AUD'">
                    <xsl:value-of select="$Value * ForexRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$Value div ForexRate"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$Value"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template  name="TestTaxlotId">
    <xsl:param name="I_GroupID" />
    <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">
      <xsl:value-of select ="TaxLotState"/>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="TaxLotIDBuilder">
    <xsl:param name="I_GroupID" />

    <xsl:variable name="varR">
      <xsl:call-template name="TestTaxlotId">
        <xsl:with-param name="I_GroupID">
          <xsl:value-of select="PBUniqueID" />
        </xsl:with-param>
      </xsl:call-template>
    </xsl:variable>

    <xsl:if test="not(contains($varR,'Amended')) and not(contains($varR,'Deleted'))">

      <xsl:variable name="AllocatedQty" />
      <!-- Building a Group with the EntityID $I_GroupID... -->

      <xsl:variable name="QtySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
      </xsl:variable>

      <xsl:variable name="CommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
      </xsl:variable>
      <xsl:variable name="SoftCommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/SoftCommissionCharged)"/>
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
      <xsl:variable name="ClearingBrokerFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/ClearingBrokerFee)"/>
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
      <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TaxLotState"/>
    </xsl:variable>

      <xsl:variable name="varTaxlotStateTx">
        <xsl:choose>
          <xsl:when test="contains($varR,'Amended') ">
            <xsl:value-of select ="'COR'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Deleted'))">
            <xsl:value-of select ="'NEW'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Allocated'))">
            <xsl:value-of select ="'CAN'"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select ="'COR'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>


    <xsl:variable name="varSymbol">

      <xsl:choose>
        <xsl:when test="Asset='EquityOption'">
          <xsl:value-of select="OSIOptionSymbol"/>
        </xsl:when>
        <xsl:when test="Asset='FixedIncome'">
          <xsl:value-of select="ISIN"/>
        </xsl:when>
        <xsl:when test="contains(Symbol,'-')">
          <xsl:value-of select="SEDOL"/>
        </xsl:when>
        <xsl:when test="CUSIP != ''">
          <xsl:value-of select="CUSIP"/>
        </xsl:when>
        <xsl:when test="BBCode != ''">
          <xsl:value-of select="substring-before(BBCode,' ')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select ="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varSecType">
      <xsl:choose>
        <xsl:when test="Asset='EquityOption'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="contains(Symbol,'-')">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="CUSIP != ''">
          <xsl:value-of select="'C'"/>
        </xsl:when>

        <xsl:when test="BBCode != ''">
          <xsl:value-of select="'B'"/>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select ="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="GroupGrsAmt">
      <!-- <xsl:value-of select="$QtySum * format-number(AveragePrice,'###.0000') * AssetMultiplier"/> -->
      <xsl:value-of  select="$QtySum * AveragePrice * AssetMultiplier"/>
    </xsl:variable>

    <xsl:variable name="GroupSideMultiplier">
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

    <xsl:variable name="GroupNetAmt">
      <xsl:value-of select="$GroupGrsAmt + (($CommissionSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum) * $GroupSideMultiplier)"/>
    </xsl:variable>


 <xsl:variable name="varGroupCommiossionAmnt">
      <xsl:value-of select="$CommissionSum + $SoftCommissionSum"/>
    </xsl:variable>
	
    <xsl:variable name="varGroupNetAmnt">
      <xsl:choose>
        <xsl:when test="Asset='Equity' and IsSwapped='true'">
          <xsl:choose>
            <xsl:when test="Side='Buy' or Side='Buy to Close'">
              <xsl:value-of select="$GroupGrsAmt + $ClearingBrokerFeeSum + $varGroupCommiossionAmnt"/>
            </xsl:when>
            <xsl:when test="Side='Sell' or Side='Sell short'">
              <xsl:value-of select="($GroupGrsAmt - $ClearingBrokerFeeSum)-$varGroupCommiossionAmnt"/>
            </xsl:when>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$NetAmountSum"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varEXCODE">
      <xsl:choose>
        <xsl:when test="Symbol = 'TKMR'">
          <xsl:value-of select="'TOR'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name ="TCounterparty" select="document('../ReconMappingXML/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='Morgan Stanley']/BrokerData[@PranaBroker=$CPVar]/@PBBroker"/>
    <xsl:variable name ="ThirdPartyCounterparty">
      <xsl:choose>
        <xsl:when test="$TCounterparty!=''">
          <xsl:value-of select="$TCounterparty"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="CounterParty"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varPBUniqueIDAndDatePart">
      <xsl:value-of select="concat(PBUniqueID,substring(TradeDate,4,2))"/>
    </xsl:variable>

    <xsl:variable name="PB_NAME" select="'Morgan Stanley'"/>
    <xsl:variable name = "PRANA_FUND_NAME">
      <xsl:value-of select="AccountName"/>
    </xsl:variable>

    <xsl:variable name ="varGroupEection_Mapping">
      <xsl:value-of select ="document('../ReconMappingXml/EOD_ExecutionMapping.xml')/ExecutionMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@ExecutionNO"/>
    </xsl:variable>

    <xsl:variable name="varEection">
      <xsl:choose>
        <xsl:when test="$varGroupEection_Mapping!=''">
          <xsl:value-of select="$varGroupEection_Mapping"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="038214755"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varOtherFeesAndCharges">
      <xsl:value-of select="$ClearingBrokerFeeSum"/>
    </xsl:variable>

    <xsl:variable name="varSettlCurrency">
      <!-- <xsl:choose> -->
        <!-- <xsl:when test="CurrencySymbol='BRL' or CurrencySymbol='RUB' or CurrencySymbol='ZAR' or CurrencySymbol='CNY'"> -->
          <!-- <xsl:value-of select="'USD'"/> -->
        <!-- </xsl:when> -->
        <!-- <xsl:otherwise> -->
          <!-- <xsl:value-of select="SettlCurrency"/> -->
        <!-- </xsl:otherwise> -->
      <!-- </xsl:choose> -->
	   <xsl:value-of select="SettlCurrency"/>
    </xsl:variable>

    <xsl:variable name="varPriceFXRate">
      <xsl:value-of select="1"/>
    </xsl:variable>

      <xsl:variable name="TaxlotStateTx">
        <xsl:choose>
          <xsl:when test="contains($varR,'Amended') ">
            <xsl:value-of select ="'Amended'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Deleted'))">
            <xsl:value-of select ="'Allocated'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Allocated'))">
            <xsl:value-of select ="'Deleted'"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select ="'Amended'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
    
    <Group
				TransactionType = "SW002" TransactionStatus="{$varTaxlotStateTx}" BuySellIndicator="" LongShortIndicator="" PositionType="{$Sidevar}"
				transactionlevel="B" ClientRefNo="{$varPBUniqueIDAndDatePart}" AssociatedTradeId="{$varPBUniqueIDAndDatePart}" ExecutionAccount="{$varEection}" AccountID="{$varEection}"
				BrokerCounterparty="{$ThirdPartyCounterparty}" SecurityIdentifierType="{$varSecType}" SecurityIdentifier="{$varSymbol}" SecurityDescription="{FullSecurityName}" TradeDate="{TradeDate}" SettlementDate="{SettlementDate}"
				SettlementCCY="{$varSettlCurrency}"	ExchangeCode="{$varEXCODE}" Quantity="{$QtySum}" 	PriceAmount="{format-number(AveragePrice,'###.######')}"	PriceIndicator="G" Principal="{format-number($GroupGrsAmt,'###.######')}"
				ExecutingBrokerCommission="{format-number($CommissionSum + $SoftCommissionSum + $TaxOnCommissionSum,'###.######')}" ExecutingBrokerCommissionIndicator="F"
				MSFees="{format-number($varOtherFeesAndCharges,'###.######')}"
				NotRequired=""
				MSFeesIndicator="F" DividendEntitlementPercent="" Spread="" NetAmount="{format-number($varGroupNetAmnt,'###.######')}" HearsayInd="N" Custodian="MSCO" MoneyManager="" Bookid=""
				Dealid="" Taxlotid="" Originaltaxlottransactiondate="" OriginalTaxlottransactionprice="" TaxLotCloseoutMethod="" PriceFXRate="{$varPriceFXRate}" AcquitionDate="" Comments="" SwapReferenceNo="" BasketId="" PriceCurrency="{CurrencySymbol}" ResetIndicator=""
				SNSreference="" ResearchFee=""   ResearchFeeIndicator="F"  LEI=""
				EntityID="{EntityID}" TaxLotState="{$TaxlotStateTx}" TaxLotState1="">


      <!-- ...selecting all the records for this Group... -->
      <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID and TaxLotState!='Sent']">
        <!-- ...and building a ThirdPartyFlatFileDetail for each -->
        <xsl:variable name="taxLotIDVar" select="EntityID"/>

        <xsl:variable name="varTaxlotState">
          <xsl:choose>
            <xsl:when test="TaxLotState='Allocated'">
              <xsl:value-of select ="'NEW'"/>
            </xsl:when>
            <xsl:when test="TaxLotState='Amended'">
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

        <xsl:variable name="TaxlotGrsAmt">
          <xsl:value-of select="AllocatedQty * AveragePrice * AssetMultiplier"/>
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

        <xsl:variable name ="THIRDPARTY_FUND_CODE">
          <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
        </xsl:variable>

        <xsl:variable name="FundName_ThirdParty">
          <xsl:choose>
            <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
              <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$PRANA_FUND_NAME"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name="varClearingFee">
          <xsl:call-template name="Conversion">
            <xsl:with-param name="Value" select="ClearingBrokerFee"/>
            <xsl:with-param name="Curr" select="CurrencySymbol"/>
          </xsl:call-template>

        </xsl:variable>

        <!-- <xsl:variable name="varAllocatinNetAmnt"> -->
          <!-- <xsl:choose> -->
            <!-- <xsl:when test="Asset='Equity' and IsSwapped='true'"> -->
              <!-- <xsl:choose> -->
                <!-- <xsl:when test="Side='Buy' or Side='Buy to Close'"> -->
                  <!-- <xsl:value-of select="NetAmount - SecFee"/> -->
                <!-- </xsl:when> -->
                <!-- <xsl:when test="Side='Sell' or Side='Sell short'"> -->
                  <!-- <xsl:value-of select="NetAmount + SecFee"/> -->
                <!-- </xsl:when> -->
              <!-- </xsl:choose> -->
            <!-- </xsl:when> -->
            <!-- <xsl:otherwise> -->
              <!-- <xsl:value-of select="NetAmount"/> -->
            <!-- </xsl:otherwise> -->
          <!-- </xsl:choose> -->
        <!-- </xsl:variable> -->
		
		
		 <xsl:variable name="varTotalCommission">
          <xsl:value-of select="(CommissionCharged + SoftCommissionCharged)"/>
        </xsl:variable>
		
		
		<xsl:variable name="varAllocatinNetAmnt">
          <xsl:choose>
            <xsl:when test="Asset='Equity' and IsSwapped='true'">
              <xsl:choose>
                <xsl:when test="Side='Buy' or Side='Buy to Close'">
                  <xsl:value-of select="$TaxlotGrsAmt + ClearingBrokerFee + $varTotalCommission"/>
                </xsl:when>
                <xsl:when test="Side='Sell' or Side='Sell short'">
                  <xsl:value-of select="($TaxlotGrsAmt - ClearingBrokerFee) -$varTotalCommission"/>
                </xsl:when>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="NetAmount"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:variable name = "TaxlotPRANA_FUND_NAME">
          <xsl:value-of select="AccountName"/>
        </xsl:variable>

        <xsl:variable name ="varExecution_Mapping">
          <xsl:value-of select ="document('../ReconMappingXml/EOD_ExecutionMapping.xml')/ExecutionMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$TaxlotPRANA_FUND_NAME]/@AccountNO"/>
        </xsl:variable>

        <xsl:variable name="varAccountNO">
          <xsl:choose>
            <xsl:when test="$varExecution_Mapping!=''">
              <xsl:value-of select="$varExecution_Mapping"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$TaxlotPRANA_FUND_NAME"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <ThirdPartyFlatFileDetail
					
							Group_Id="" TransactionType = "SW002" TransactionStatus="{$varTaxlotState}" BuySellIndicator=""
							LongShortIndicator="" PositionType="{$Sidevar}" transactionlevel="A" ClientRefNo="{concat('A',substring(EntityID,2))}"
							AssociatedTradeId="{$varPBUniqueIDAndDatePart}" ExecutionAccount="{$varEection}" AccountID="{$varAccountNO}"
							BrokerCounterparty="{$ThirdPartyCounterparty}" SecurityIdentifierType="{$varSecType}" SecurityIdentifier="{$varSymbol}" SecurityDescription="{FullSecurityName}"
							TradeDate="{TradeDate}" SettlementDate="{SettlementDate}" SettlementCCY="{$varSettlCurrency}"	ExchangeCode="{$varEXCODE}"
							Quantity="{AllocatedQty}" PriceAmount="{format-number(AveragePrice,'###.######')}" PriceIndicator="G"
							Principal="{format-number($TaxlotGrsAmt,'###.######')}" ExecutingBrokerCommission="{format-number(CommissionCharged + SoftCommissionCharged + TaxOnCommissions,'###.######')}"
							ExecutingBrokerCommissionIndicator="F" MSFees="{format-number(ClearingBrokerFee,'###.######')}"
							NotRequired=""
							MSFeesIndicator="F" DividendEntitlementPercent="" Spread="" NetAmount="{format-number($varAllocatinNetAmnt,'###.######')}" HearsayInd="N" Custodian="MSCO"
							MoneyManager="" Bookid="" Dealid="" Taxlotid="" Originaltaxlottransactiondate="" OriginalTaxlottransactionprice="" TaxLotCloseoutMethod="" PriceFXRate="{$varPriceFXRate}"
							AcquitionDate="" Comments="" SwapReferenceNo="" BasketId="" PriceCurrency="{CurrencySymbol}" ResetIndicator=""
							SNSreference="" ResearchFee=""   ResearchFeeIndicator="F"  LEI=""
					        EntityID="{EntityID}" TaxLotState="{TaxLotState}"/>
      </xsl:for-each>
    </Group>
   </xsl:if>

    <xsl:if test="not(contains($varR,'Amended')) and not(contains($varR,'Allocated'))">

      <xsl:variable name="AllocatedQty" />
      <!-- Building a Group with the EntityID $I_GroupID... -->

      <xsl:variable name="QtySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/AllocatedQty)"/>
      </xsl:variable>

      <xsl:variable name="CommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/CommissionCharged)"/>
      </xsl:variable>
      <xsl:variable name="SoftCommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SoftCommissionCharged)"/>
      </xsl:variable>
      <xsl:variable name="TaxOnCommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TaxOnCommissions)"/>
      </xsl:variable>
      <xsl:variable name="SecFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/SecFee)"/>
      </xsl:variable>
      <xsl:variable name="StampDutySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/StampDuty)"/>
      </xsl:variable>
      <xsl:variable name="ClearingBrokerFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/ClearingBrokerFee)"/>
      </xsl:variable>
      <xsl:variable name="TransactionLevySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TransactionLevy)"/>
      </xsl:variable>
      <xsl:variable name="ClearingFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/ClearingFee)"/>
      </xsl:variable>
      <xsl:variable name="MiscFeesSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/MiscFees)"/>
      </xsl:variable>
      <xsl:variable name="OrfFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/OrfFee)"/>
      </xsl:variable>
      <xsl:variable name="OtherBrokerFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/OtherBrokerFee)"/>
      </xsl:variable>
      <xsl:variable name="GrossAmountSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/GrossAmount)"/>
      </xsl:variable>
      <xsl:variable name="NetAmountSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/NetAmount)"/>
      </xsl:variable>

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
        <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TaxLotState"/>
      </xsl:variable>

      <xsl:variable name="varTaxlotStateTx">
        <xsl:choose>
          <xsl:when test="contains($varR,'Amended') ">
            <xsl:value-of select ="'COR'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Deleted'))">
            <xsl:value-of select ="'NEW'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Allocated'))">
            <xsl:value-of select ="'CAN'"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select ="'COR'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>


      <xsl:variable name="varSymbol">

        <xsl:choose>
          <xsl:when test="Asset='EquityOption'">
            <xsl:value-of select="OSIOptionSymbol"/>
          </xsl:when>
          <xsl:when test="Asset='FixedIncome'">
            <xsl:value-of select="ISIN"/>
          </xsl:when>
          <xsl:when test="contains(Symbol,'-')">
            <xsl:value-of select="SEDOL"/>
          </xsl:when>
          <xsl:when test="CUSIP != ''">
            <xsl:value-of select="CUSIP"/>
          </xsl:when>
          <xsl:when test="BBCode != ''">
            <xsl:value-of select="substring-before(BBCode,' ')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select ="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varSecType">
        <xsl:choose>
          <xsl:when test="Asset='EquityOption'">
            <xsl:value-of select="'T'"/>
          </xsl:when>
          <xsl:when test="contains(Symbol,'-')">
            <xsl:value-of select="'S'"/>
          </xsl:when>
          <xsl:when test="CUSIP != ''">
            <xsl:value-of select="'C'"/>
          </xsl:when>

          <xsl:when test="BBCode != ''">
            <xsl:value-of select="'B'"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select ="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="GroupGrsAmt">
        <!-- <xsl:value-of select="$QtySum * format-number(AveragePrice,'###.0000') * AssetMultiplier"/> -->
        <xsl:value-of  select="$QtySum * AveragePrice * AssetMultiplier"/>
      </xsl:variable>

      <xsl:variable name="GroupSideMultiplier">
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

      <xsl:variable name="GroupNetAmt">
        <xsl:value-of select="$GroupGrsAmt + (($CommissionSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum) * $GroupSideMultiplier)"/>
      </xsl:variable>


      <xsl:variable name="varGroupCommiossionAmnt">
        <xsl:value-of select="$CommissionSum + $SoftCommissionSum"/>
      </xsl:variable>

      <xsl:variable name="varGroupNetAmnt">
        <xsl:choose>
          <xsl:when test="Asset='Equity' and IsSwapped='true'">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Close'">
                <xsl:value-of select="$GroupGrsAmt + $ClearingBrokerFeeSum + $varGroupCommiossionAmnt"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell short'">
                <xsl:value-of select="($GroupGrsAmt - $ClearingBrokerFeeSum)-$varGroupCommiossionAmnt"/>
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$NetAmountSum"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varEXCODE">
        <xsl:choose>
          <xsl:when test="Symbol = 'TKMR'">
            <xsl:value-of select="'TOR'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name ="TCounterparty" select="document('../ReconMappingXML/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='Morgan Stanley']/BrokerData[@PranaBroker=$CPVar]/@PBBroker"/>
      <xsl:variable name ="ThirdPartyCounterparty">
        <xsl:choose>
          <xsl:when test="$TCounterparty!=''">
            <xsl:value-of select="$TCounterparty"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="CounterParty"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varPBUniqueIDAndDatePart">
        <xsl:value-of select="concat(PBUniqueID,substring(TradeDate,4,2))"/>
      </xsl:variable>

      <xsl:variable name="PB_NAME" select="'Morgan Stanley'"/>
      <xsl:variable name = "PRANA_FUND_NAME">
        <xsl:value-of select="AccountName"/>
      </xsl:variable>

      <xsl:variable name ="varGroupEection_Mapping">
        <xsl:value-of select ="document('../ReconMappingXml/EOD_ExecutionMapping.xml')/ExecutionMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@ExecutionNO"/>
      </xsl:variable>

      <xsl:variable name="varEection">
        <xsl:choose>
          <xsl:when test="$varGroupEection_Mapping!=''">
            <xsl:value-of select="$varGroupEection_Mapping"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="038214755"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varOtherFeesAndCharges">
        <xsl:value-of select="$ClearingBrokerFeeSum"/>
      </xsl:variable>

      <xsl:variable name="varSettlCurrency">
        <!-- <xsl:choose> -->
        <!-- <xsl:when test="CurrencySymbol='BRL' or CurrencySymbol='RUB' or CurrencySymbol='ZAR' or CurrencySymbol='CNY'"> -->
        <!-- <xsl:value-of select="'USD'"/> -->
        <!-- </xsl:when> -->
        <!-- <xsl:otherwise> -->
        <!-- <xsl:value-of select="SettlCurrency"/> -->
        <!-- </xsl:otherwise> -->
        <!-- </xsl:choose> -->
        <xsl:value-of select="SettlCurrency"/>
      </xsl:variable>

      <xsl:variable name="varPriceFXRate">
        <xsl:value-of select="1"/>
      </xsl:variable>

      <xsl:variable name="TaxlotStateTx">
        <xsl:choose>
          <xsl:when test="contains($varR,'Amended') ">
            <xsl:value-of select ="'Amended'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Deleted'))">
            <xsl:value-of select ="'Allocated'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Allocated'))">
            <xsl:value-of select ="'Deleted'"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select ="'Amended'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <Group
          TransactionType = "SW002" TransactionStatus="{$varTaxlotStateTx}" BuySellIndicator="" LongShortIndicator="" PositionType="{$Sidevar}"
          transactionlevel="B" ClientRefNo="{$varPBUniqueIDAndDatePart}" AssociatedTradeId="{$varPBUniqueIDAndDatePart}" ExecutionAccount="{$varEection}" AccountID="{$varEection}"
          BrokerCounterparty="{$ThirdPartyCounterparty}" SecurityIdentifierType="{$varSecType}" SecurityIdentifier="{$varSymbol}" SecurityDescription="{FullSecurityName}" TradeDate="{TradeDate}" SettlementDate="{SettlementDate}"
          SettlementCCY="{$varSettlCurrency}"	ExchangeCode="{$varEXCODE}" Quantity="{$QtySum}" 	PriceAmount="{format-number(AveragePrice,'###.######')}"	PriceIndicator="G" Principal="{format-number($GroupGrsAmt,'###.######')}"
          ExecutingBrokerCommission="{format-number($CommissionSum + $SoftCommissionSum + $TaxOnCommissionSum,'###.######')}" ExecutingBrokerCommissionIndicator="F"
          MSFees="{format-number($varOtherFeesAndCharges,'###.######')}"
          NotRequired=""
          MSFeesIndicator="F" DividendEntitlementPercent="" Spread="" NetAmount="{format-number($varGroupNetAmnt,'###.######')}" HearsayInd="N" Custodian="MSCO" MoneyManager="" Bookid=""
          Dealid="" Taxlotid="" Originaltaxlottransactiondate="" OriginalTaxlottransactionprice="" TaxLotCloseoutMethod="" PriceFXRate="{$varPriceFXRate}" AcquitionDate="" Comments="" SwapReferenceNo="" BasketId="" PriceCurrency="{CurrencySymbol}" ResetIndicator=""
          SNSreference="" ResearchFee=""   ResearchFeeIndicator="F"  LEI=""
          EntityID="{EntityID}" TaxLotState="{$TaxlotStateTx}" TaxLotState1="">


        <!-- ...selecting all the records for this Group... -->
        <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID and TaxLotState!='Sent']">
          <!-- ...and building a ThirdPartyFlatFileDetail for each -->
          <xsl:variable name="taxLotIDVar" select="EntityID"/>

          <xsl:variable name="varTaxlotState">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
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

          <xsl:variable name="TaxlotGrsAmt">
            <xsl:value-of select="AllocatedQty * AveragePrice * AssetMultiplier"/>
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

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>

          <xsl:variable name="FundName_ThirdParty">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varClearingFee">
            <xsl:call-template name="Conversion">
              <xsl:with-param name="Value" select="ClearingBrokerFee"/>
              <xsl:with-param name="Curr" select="CurrencySymbol"/>
            </xsl:call-template>

          </xsl:variable>

          <!-- <xsl:variable name="varAllocatinNetAmnt"> -->
          <!-- <xsl:choose> -->
          <!-- <xsl:when test="Asset='Equity' and IsSwapped='true'"> -->
          <!-- <xsl:choose> -->
          <!-- <xsl:when test="Side='Buy' or Side='Buy to Close'"> -->
          <!-- <xsl:value-of select="NetAmount - SecFee"/> -->
          <!-- </xsl:when> -->
          <!-- <xsl:when test="Side='Sell' or Side='Sell short'"> -->
          <!-- <xsl:value-of select="NetAmount + SecFee"/> -->
          <!-- </xsl:when> -->
          <!-- </xsl:choose> -->
          <!-- </xsl:when> -->
          <!-- <xsl:otherwise> -->
          <!-- <xsl:value-of select="NetAmount"/> -->
          <!-- </xsl:otherwise> -->
          <!-- </xsl:choose> -->
          <!-- </xsl:variable> -->


          <xsl:variable name="varTotalCommission">
            <xsl:value-of select="(CommissionCharged + SoftCommissionCharged)"/>
          </xsl:variable>


          <xsl:variable name="varAllocatinNetAmnt">
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Close'">
                    <xsl:value-of select="$TaxlotGrsAmt + ClearingBrokerFee + $varTotalCommission"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell short'">
                    <xsl:value-of select="($TaxlotGrsAmt - ClearingBrokerFee) -$varTotalCommission"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="NetAmount"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name = "TaxlotPRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="varExecution_Mapping">
            <xsl:value-of select ="document('../ReconMappingXml/EOD_ExecutionMapping.xml')/ExecutionMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$TaxlotPRANA_FUND_NAME]/@AccountNO"/>
          </xsl:variable>

          <xsl:variable name="varAccountNO">
            <xsl:choose>
              <xsl:when test="$varExecution_Mapping!=''">
                <xsl:value-of select="$varExecution_Mapping"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$TaxlotPRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <ThirdPartyFlatFileDetail
					
                Group_Id="" TransactionType = "SW002" TransactionStatus="{$varTaxlotState}" BuySellIndicator=""
                LongShortIndicator="" PositionType="{$Sidevar}" transactionlevel="A" ClientRefNo="{concat('A',substring(EntityID,2))}"
                AssociatedTradeId="{$varPBUniqueIDAndDatePart}" ExecutionAccount="{$varEection}" AccountID="{$varAccountNO}"
                BrokerCounterparty="{$ThirdPartyCounterparty}" SecurityIdentifierType="{$varSecType}" SecurityIdentifier="{$varSymbol}" SecurityDescription="{FullSecurityName}"
                TradeDate="{TradeDate}" SettlementDate="{SettlementDate}" SettlementCCY="{$varSettlCurrency}"	ExchangeCode="{$varEXCODE}"
                Quantity="{AllocatedQty}" PriceAmount="{format-number(AveragePrice,'###.######')}" PriceIndicator="G"
                Principal="{format-number($TaxlotGrsAmt,'###.######')}" ExecutingBrokerCommission="{format-number(CommissionCharged + SoftCommissionCharged + TaxOnCommissions,'###.######')}"
                ExecutingBrokerCommissionIndicator="F" MSFees="{format-number(ClearingBrokerFee,'###.######')}"
                NotRequired=""
                MSFeesIndicator="F" DividendEntitlementPercent="" Spread="" NetAmount="{format-number($varAllocatinNetAmnt,'###.######')}" HearsayInd="N" Custodian="MSCO"
                MoneyManager="" Bookid="" Dealid="" Taxlotid="" Originaltaxlottransactiondate="" OriginalTaxlottransactionprice="" TaxLotCloseoutMethod="" PriceFXRate="{$varPriceFXRate}"
                AcquitionDate="" Comments="" SwapReferenceNo="" BasketId="" PriceCurrency="{CurrencySymbol}" ResetIndicator=""
                SNSreference="" ResearchFee=""   ResearchFeeIndicator="F"  LEI=""
                    EntityID="{EntityID}" TaxLotState="{TaxLotState}"/>
        </xsl:for-each>
      </Group>
    </xsl:if>

    <xsl:if test="contains($varR,'Amended')">

      <xsl:variable name="AllocatedQty" />
      <!-- Building a Group with the EntityID $I_GroupID... -->

      <xsl:variable name="QtySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
      </xsl:variable>

      <xsl:variable name="CommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
      </xsl:variable>
      <xsl:variable name="SoftCommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/SoftCommissionCharged)"/>
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
      <xsl:variable name="ClearingBrokerFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/ClearingBrokerFee)"/>
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
        <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/TaxLotState"/>
      </xsl:variable>

      <xsl:variable name="varTaxlotStateTx">
        <xsl:choose>
          <xsl:when test="contains($varR,'Amended') ">
            <xsl:value-of select ="'COR'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Deleted'))">
            <xsl:value-of select ="'NEW'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Allocated'))">
            <xsl:value-of select ="'CAN'"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select ="'COR'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>


      <xsl:variable name="varSymbol">

        <xsl:choose>
          <xsl:when test="Asset='EquityOption'">
            <xsl:value-of select="OSIOptionSymbol"/>
          </xsl:when>
          <xsl:when test="Asset='FixedIncome'">
            <xsl:value-of select="ISIN"/>
          </xsl:when>
          <xsl:when test="contains(Symbol,'-')">
            <xsl:value-of select="SEDOL"/>
          </xsl:when>
          <xsl:when test="CUSIP != ''">
            <xsl:value-of select="CUSIP"/>
          </xsl:when>
          <xsl:when test="BBCode != ''">
            <xsl:value-of select="substring-before(BBCode,' ')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select ="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varSecType">
        <xsl:choose>
          <xsl:when test="Asset='EquityOption'">
            <xsl:value-of select="'T'"/>
          </xsl:when>
          <xsl:when test="contains(Symbol,'-')">
            <xsl:value-of select="'S'"/>
          </xsl:when>
          <xsl:when test="CUSIP != ''">
            <xsl:value-of select="'C'"/>
          </xsl:when>

          <xsl:when test="BBCode != ''">
            <xsl:value-of select="'B'"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select ="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="GroupGrsAmt">
        <xsl:value-of  select="$QtySum * AveragePrice * AssetMultiplier"/>
      </xsl:variable>

      <xsl:variable name="GroupSideMultiplier">
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

      <xsl:variable name="GroupNetAmt">
        <xsl:value-of select="$GroupGrsAmt + (($CommissionSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum) * $GroupSideMultiplier)"/>
      </xsl:variable>


      <xsl:variable name="varGroupCommiossionAmnt">
        <xsl:value-of select="$CommissionSum + $SoftCommissionSum"/>
      </xsl:variable>

      <xsl:variable name="varGroupNetAmnt">
        <xsl:choose>
          <xsl:when test="Asset='Equity' and IsSwapped='true'">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Close'">
                <xsl:value-of select="$GroupGrsAmt + $ClearingBrokerFeeSum + $varGroupCommiossionAmnt"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell short'">
                <xsl:value-of select="($GroupGrsAmt - $ClearingBrokerFeeSum)-$varGroupCommiossionAmnt"/>
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$NetAmountSum"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varEXCODE">
        <xsl:choose>
          <xsl:when test="Symbol = 'TKMR'">
            <xsl:value-of select="'TOR'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name ="TCounterparty" select="document('../ReconMappingXML/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='Morgan Stanley']/BrokerData[@PranaBroker=$CPVar]/@PBBroker"/>
      <xsl:variable name ="ThirdPartyCounterparty">
        <xsl:choose>
          <xsl:when test="$TCounterparty!=''">
            <xsl:value-of select="$TCounterparty"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="CounterParty"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varPBUniqueIDAndDatePart">
        <xsl:value-of select="concat(PBUniqueID,substring(TradeDate,4,2))"/>
      </xsl:variable>

      <xsl:variable name="PB_NAME" select="'Morgan Stanley'"/>
      <xsl:variable name = "PRANA_FUND_NAME">
        <xsl:value-of select="AccountName"/>
      </xsl:variable>

      <xsl:variable name ="varGroupEection_Mapping">
        <xsl:value-of select ="document('../ReconMappingXml/EOD_ExecutionMapping.xml')/ExecutionMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@ExecutionNO"/>
      </xsl:variable>

      <xsl:variable name="varEection">
        <xsl:choose>
          <xsl:when test="$varGroupEection_Mapping!=''">
            <xsl:value-of select="$varGroupEection_Mapping"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="038214755"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varOtherFeesAndCharges">
        <xsl:value-of select="$ClearingBrokerFeeSum"/>
      </xsl:variable>

      <xsl:variable name="varSettlCurrency">
        <xsl:value-of select="SettlCurrency"/>
      </xsl:variable>

      <xsl:variable name="varPriceFXRate">
        <xsl:value-of select="1"/>
      </xsl:variable>

      <xsl:variable name="TaxlotStateTx">
        <xsl:choose>
          <xsl:when test="contains($varR,'Amended') ">
            <xsl:value-of select ="'Amended'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Deleted'))">
            <xsl:value-of select ="'Allocated'"/>
          </xsl:when>

          <xsl:when test="not(contains($varR,'Amended')) and not(contains($varR,'Allocated'))">
            <xsl:value-of select ="'Deleted'"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select ="'Amended'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <Group
          TransactionType = "SW002" TransactionStatus="{$varTaxlotStateTx}" BuySellIndicator="" LongShortIndicator="" PositionType="{$Sidevar}"
          transactionlevel="B" ClientRefNo="{$varPBUniqueIDAndDatePart}" AssociatedTradeId="{$varPBUniqueIDAndDatePart}" ExecutionAccount="{$varEection}" AccountID="{$varEection}"
          BrokerCounterparty="{$ThirdPartyCounterparty}" SecurityIdentifierType="{$varSecType}" SecurityIdentifier="{$varSymbol}" SecurityDescription="{FullSecurityName}" TradeDate="{TradeDate}" SettlementDate="{SettlementDate}"
          SettlementCCY="{$varSettlCurrency}"	ExchangeCode="{$varEXCODE}" Quantity="{$QtySum}" 	PriceAmount="{format-number(AveragePrice,'###.######')}"	PriceIndicator="G" Principal="{format-number($GroupGrsAmt,'###.######')}"
          ExecutingBrokerCommission="{format-number($CommissionSum + $SoftCommissionSum + $TaxOnCommissionSum,'###.######')}" ExecutingBrokerCommissionIndicator="F"
          MSFees="{format-number($varOtherFeesAndCharges,'###.######')}"
          NotRequired=""
          MSFeesIndicator="F" DividendEntitlementPercent="" Spread="" NetAmount="{format-number($varGroupNetAmnt,'###.######')}" HearsayInd="N" Custodian="MSCO" MoneyManager="" Bookid=""
          Dealid="" Taxlotid="" Originaltaxlottransactiondate="" OriginalTaxlottransactionprice="" TaxLotCloseoutMethod="" PriceFXRate="{$varPriceFXRate}" AcquitionDate="" Comments="" SwapReferenceNo="" BasketId="" PriceCurrency="{CurrencySymbol}" ResetIndicator=""
          SNSreference="" ResearchFee=""   ResearchFeeIndicator="F"  LEI=""
          EntityID="{EntityID}" TaxLotState="{$TaxlotStateTx}" TaxLotState1="">


        <!-- ...selecting all the records for this Group... -->
        <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID and TaxLotState!='Sent']">
          <!-- ...and building a ThirdPartyFlatFileDetail for each -->
          <xsl:variable name="taxLotIDVar" select="EntityID"/>

          <xsl:variable name="varTaxlotState">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
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

          <xsl:variable name="TaxlotGrsAmt">
            <xsl:value-of select="AllocatedQty * AveragePrice * AssetMultiplier"/>
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

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>

          <xsl:variable name="FundName_ThirdParty">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varClearingFee">
            <xsl:call-template name="Conversion">
              <xsl:with-param name="Value" select="ClearingBrokerFee"/>
              <xsl:with-param name="Curr" select="CurrencySymbol"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="varTotalCommission">
            <xsl:value-of select="(CommissionCharged + SoftCommissionCharged)"/>
          </xsl:variable>

          <xsl:variable name="varAllocatinNetAmnt">
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Close'">
                    <xsl:value-of select="$TaxlotGrsAmt + ClearingBrokerFee + $varTotalCommission"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell short'">
                    <xsl:value-of select="($TaxlotGrsAmt - ClearingBrokerFee) -$varTotalCommission"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="NetAmount"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name = "TaxlotPRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="varExecution_Mapping">
            <xsl:value-of select ="document('../ReconMappingXml/EOD_ExecutionMapping.xml')/ExecutionMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$TaxlotPRANA_FUND_NAME]/@AccountNO"/>
          </xsl:variable>

          <xsl:variable name="varAccountNO">
            <xsl:choose>
              <xsl:when test="$varExecution_Mapping!=''">
                <xsl:value-of select="$varExecution_Mapping"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$TaxlotPRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <ThirdPartyFlatFileDetail
					
                Group_Id="" TransactionType = "SW002" TransactionStatus="{$varTaxlotState}" BuySellIndicator=""
                LongShortIndicator="" PositionType="{$Sidevar}" transactionlevel="A" ClientRefNo="{concat('A',substring(EntityID,2))}"
                AssociatedTradeId="{$varPBUniqueIDAndDatePart}" ExecutionAccount="{$varEection}" AccountID="{$varAccountNO}"
                BrokerCounterparty="{$ThirdPartyCounterparty}" SecurityIdentifierType="{$varSecType}" SecurityIdentifier="{$varSymbol}" SecurityDescription="{FullSecurityName}"
                TradeDate="{TradeDate}" SettlementDate="{SettlementDate}" SettlementCCY="{$varSettlCurrency}"	ExchangeCode="{$varEXCODE}"
                Quantity="{AllocatedQty}" PriceAmount="{format-number(AveragePrice,'###.######')}" PriceIndicator="G"
                Principal="{format-number($TaxlotGrsAmt,'###.######')}" ExecutingBrokerCommission="{format-number(CommissionCharged + SoftCommissionCharged + TaxOnCommissions,'###.######')}"
                ExecutingBrokerCommissionIndicator="F" MSFees="{format-number(ClearingBrokerFee,'###.######')}"
                NotRequired=""
                MSFeesIndicator="F" DividendEntitlementPercent="" Spread="" NetAmount="{format-number($varAllocatinNetAmnt,'###.######')}" HearsayInd="N" Custodian="MSCO"
                MoneyManager="" Bookid="" Dealid="" Taxlotid="" Originaltaxlottransactiondate="" OriginalTaxlottransactionprice="" TaxLotCloseoutMethod="" PriceFXRate="{$varPriceFXRate}"
                AcquitionDate="" Comments="" SwapReferenceNo="" BasketId="" PriceCurrency="{CurrencySymbol}" ResetIndicator=""
                SNSreference="" ResearchFee=""   ResearchFeeIndicator="F"  LEI=""
                EntityID="{EntityID}" TaxLotState="{TaxLotState}"/>
        </xsl:for-each>
      </Group>
    </xsl:if>
    
  </xsl:template>
</xsl:stylesheet>

