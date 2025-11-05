<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />
  <xsl:template match="/">
    <Groups>
      <!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
      <!-- let's build a Group node for each different EntityID by   -->
      <!-- looping trough all the records...                         -->
      <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset']">
        <!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
        <xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
          <!--<xsl:if test="CounterParty != 'Undefined' and AccountName !='Quantum Partners LP' and contains(AccountName,'MS Investment Partners')!='true' and Asset !='FX'">-->
          <!--<xsl:if test="CounterParty != 'Undefined' and AccountName !='Quantum Partners LP' and Asset !='FX'">-->
          <!--<xsl:if test="CounterParty != 'Undefined' and Asset !='FX' and AccountName != 'Quantum Partners LP'">-->
          <xsl:if test="CounterParty != 'Undefined' and not(contains(Asset, 'FX'))">
            <!-- ...buid a Group for this node_id -->
            <xsl:call-template name="TaxLotIDBuilder">
              <xsl:with-param name="I_GroupID">
                <xsl:value-of select="PBUniqueID" />
              </xsl:with-param>
            </xsl:call-template>
          </xsl:if>
        </xsl:if>
      </xsl:for-each>
    </Groups>
  </xsl:template>


  <xsl:template name="TaxLotIDBuilder">
    <xsl:param name="I_GroupID" />

    <xsl:if test="(AccountName='Napean MS Swap 0617853O7') and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset' and TaxLotState != 'Deleted'">

      <xsl:variable name="AllocatedQty" />
      <!-- Building a Group with the EntityID $I_GroupID... -->
      <xsl:variable name="QtySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/AllocatedQty)"/>
      </xsl:variable>
      <xsl:variable name="GroupNetAmt">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/NetAmount)"/>
      </xsl:variable>
      <xsl:variable name="CommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/CommissionCharged)"/>
      </xsl:variable>
      <xsl:variable name="TaxOnCommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/TaxOnCommissions)"/>
      </xsl:variable>
      <xsl:variable name="SecFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/SecFee)"/>
      </xsl:variable>
      <xsl:variable name="StampDutySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/StampDuty)"/>
      </xsl:variable>
      <xsl:variable name="TransactionLevySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/TransactionLevy)"/>
      </xsl:variable>
      <xsl:variable name="ClearingFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/ClearingFee)"/>
      </xsl:variable>
      <xsl:variable name="MiscFeesSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/MiscFees)"/>
      </xsl:variable>
      <xsl:variable name="OrfFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/OrfFee)"/>
      </xsl:variable>
      <xsl:variable name="OtherBrokerFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/OtherBrokerFee)"/>
      </xsl:variable>
      <xsl:variable name="GrossAmountSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/GrossAmount)"/>
      </xsl:variable>
      <xsl:variable name="NetAmountSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted'][AccountName !='Quantum Partners LP']/NetAmount)"/>
      </xsl:variable>
      <!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
      <xsl:variable name="tempSideVar">
        <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/Side"/>
      </xsl:variable>

      <xsl:variable name="tempCPVar">
        <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/CounterParty"/>
      </xsl:variable>

      <xsl:variable name="PB_NAME" select="'DB'"/>

      <xsl:variable name="PB_COUNTERPARTY_NAME">
        <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$tempCPVar]/@ThirdPartyBrokerID"/>
      </xsl:variable>

      <xsl:variable name="CPVar">
        <!--<xsl:choose>
				<xsl:when test="$tempCPVar='CUTTONE' or $tempCPVar='CUTN'">CUTE</xsl:when>
				<xsl:when test="$tempCPVar='SAND'">SDLR</xsl:when>
				<xsl:when test="$tempCPVar='ISGI'">INTS</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="$tempCPVar"/>
				</xsl:otherwise>
			</xsl:choose>-->
        <xsl:choose>
          <xsl:when test="$PB_COUNTERPARTY_NAME!=''">
            <xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
          </xsl:when>
          <xsl:when test="CounterParty='RBCZ' or CounterParty='RBCB'">
            <xsl:value-of select="'RBCM'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$tempCPVar"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="Sidevar">
        <xsl:choose>
          <xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">Buy</xsl:when>
          <xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">Buytocover</xsl:when>
          <xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">Sell</xsl:when>
          <xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">Sellshort</xsl:when>
          <xsl:otherwise></xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
	  <xsl:variable name="varLongShort">
        <xsl:choose>
          <xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">Long</xsl:when>
          <xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">Short</xsl:when>
          <xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">Long</xsl:when>
          <xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">Short</xsl:when>
          <xsl:otherwise></xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
	  
      <xsl:variable name="tempTaxlotStateVar">
        <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotStateID>1]/TaxLotState"/>
      </xsl:variable>

      <xsl:variable name="varTaxlotStateGrp">
        <xsl:choose>
          <xsl:when test="$tempTaxlotStateVar != ''">COR</xsl:when>
          <xsl:otherwise>NEW</xsl:otherwise>
        </xsl:choose>
      </xsl:variable>



      <xsl:variable name="varSymbol">
        <xsl:choose>
          <xsl:when test ="Asset='EquityOption'">
            <xsl:value-of select="OSIOptionSymbol"/>
          </xsl:when>
          <xsl:when test ="Asset='FixedIncome'">
            <xsl:value-of select="ISIN"/>
          </xsl:when>

          <xsl:when test="SEDOL != ''">
            <xsl:value-of select="SEDOL"/>
          </xsl:when>
          <xsl:when test="CUSIP != ''">
            <xsl:value-of select="CUSIP"/>
          </xsl:when>
          <xsl:when test="ISIN != ''">
            <xsl:value-of select="ISIN"/>
          </xsl:when>


          <xsl:when test="Symbol != ''">
            <xsl:value-of select="Symbol"/>
          </xsl:when>
          <xsl:when test="RIC != ''">
            <xsl:value-of select="RIC"/>
          </xsl:when>
          <xsl:when test="BBCode != ''">
            <xsl:value-of select="BBCode"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select ="Symbol"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varSecType">
        <xsl:choose>
          <xsl:when test ="Asset='EquityOption'">
            <xsl:value-of select="'T'"/>
          </xsl:when>
          <xsl:when test ="Asset='FixedIncome'">
            <xsl:value-of select="'I'"/>
          </xsl:when>

          <xsl:when test="SEDOL != ''">
            <xsl:value-of select="'S'"/>
          </xsl:when>
          <xsl:when test="CUSIP != ''">
            <xsl:value-of select="'C'"/>
          </xsl:when>
          <xsl:when test="ISIN != ''">
            <xsl:value-of select="'I'"/>
          </xsl:when>


          <xsl:when test="Symbol != ''">
            <xsl:value-of select="'T'"/>
          </xsl:when>
          <xsl:when test="RIC != ''">
            <xsl:value-of select="'R'"/>
          </xsl:when>
          <xsl:when test="BBCode != ''">
            <xsl:value-of select="'B'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select ="'T'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="GroupGrsAmt">
        <xsl:value-of select="$QtySum * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
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

      <xsl:variable name="GroupNetAmt1">
        <xsl:value-of select="$GroupGrsAmt + (($CommissionSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum) * $GroupSideMultiplier)"/>
      </xsl:variable>

      <xsl:variable name="PRANA_EXCHANGE" select="Exchange"/>

      <xsl:variable name="PB_EXCHANGE">
        <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name=$PB_NAME]/ExchangeData[@PranaExchange=$PRANA_EXCHANGE]/@PBExchangeName"/>
      </xsl:variable>

      <xsl:variable name="varEXCODE">
        <xsl:choose>
          <xsl:when test ="$PB_EXCHANGE!=''">
            <xsl:value-of select="$PB_EXCHANGE"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="Exchange"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varTaxlotStateTx">
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
            <xsl:value-of select ="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="GroupStamp">
        <xsl:value-of select="$GroupGrsAmt * 0.0000184"/>
      </xsl:variable>

      <xsl:variable name="GroupOrf">
        <xsl:value-of select="$QtySum * 0.04"/>
      </xsl:variable>

      <xsl:variable name="GroupTotalFees">
        <xsl:choose>
          <xsl:when test="number($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum +$SecFeeSum) &gt; 0">
            <xsl:value-of select="($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum +$SecFeeSum)"/>
          </xsl:when>
          <xsl:when test="number($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum +$SecFeeSum) &lt; 0">
            <xsl:value-of select="($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum +$SecFeeSum) * -1"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="0"/>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:variable>

      <xsl:variable name="TransactionType">
        <xsl:choose>
          <!--<xsl:when test="Asset='Equity' and IsSwapped='true'">-->
		  <xsl:when test="AccountName='Octahedron Fund GS Swap' or AccountName='Octahedron MS Swap'"> 
            <xsl:value-of select="'SW002'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="'TR001'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="varCustodian1">
        <xsl:choose>
          <xsl:when test="AccountMappedName='038CADJ92' and Asset='Equity' and IsSwapped='true'">
            <xsl:value-of select="'MSSW'"/>
          </xsl:when>

          <xsl:when test="AccountName='Octahedron MS Swap'">
            <xsl:value-of select="'MSSW'"/>
          </xsl:when>
          <xsl:when test="AccountMappedName='038CADJ92'and IsSwapped='false'">
            <xsl:value-of select="'MSCO'"/>
          </xsl:when>
		  <xsl:when test="AccountName='MSPA In House'">
            <xsl:value-of select="'MSCO'"/>
          </xsl:when>
		  <xsl:when test="AccountName='Octahedron Fund GS Swap'">
            <xsl:value-of select="'GSSW'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varFXRate">
        <xsl:choose>
          <xsl:when test="SettlCurrency != CurrencySymbol">
            <xsl:value-of select="FXRate_Taxlot"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="1"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>





      <xsl:variable name="varGroupGrsAmtSettl">
        <xsl:choose>
          <xsl:when test="$varFXRate=0">
            <xsl:value-of select="$GroupGrsAmt"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
            <xsl:value-of select="format-number($GroupGrsAmt * $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
            <xsl:value-of select="format-number($GroupGrsAmt div $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>


      <xsl:variable name="GroupNetAmtFX">
        <xsl:choose>
          <xsl:when test ="contains(Asset,'FixedIncome')">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Close'">
                <xsl:value-of select="$GroupGrsAmt + AccruedInterest"/>
              </xsl:when>

              <xsl:when test="Side='Sell' or Side='Sell short'">
                <xsl:value-of select="$GroupGrsAmt - AccruedInterest"/>
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$GroupNetAmt"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varNetamountSettl">
        <xsl:choose>
          <xsl:when test="$varFXRate=0">
            <xsl:value-of select="$GroupNetAmtFX"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
            <xsl:value-of select="format-number($GroupNetAmtFX * $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
            <xsl:value-of select="format-number($GroupNetAmtFX div $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varCOMM">
        <xsl:choose>
          <xsl:when test="$varFXRate=0">
            <xsl:value-of select="$CommissionSum"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
            <xsl:value-of select="format-number($CommissionSum * $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
            <xsl:value-of select="format-number($CommissionSum div $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="''"/>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>



      <xsl:variable name="varSettFxAmt">
        <xsl:choose>
          <xsl:when test="SettlCurrency != CurrencySymbol">
            <xsl:choose>
              <xsl:when test="FXConversionMethodOperator_Trade ='M'">
                <xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="AveragePrice"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varAveragePriceSettl">
        <xsl:choose>
          <xsl:when test="SettlCurrency = CurrencySymbol">
            <xsl:value-of select="AveragePrice"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$varSettFxAmt"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varTaxFees">
        <xsl:choose>
          <xsl:when test="$varFXRate=0">
            <xsl:value-of select="$GroupTotalFees"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
            <xsl:value-of select="format-number($varFXRate * $GroupTotalFees,'0.##')"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
            <xsl:value-of select="format-number($GroupTotalFees div $varFXRate,'0.##')"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="''"/>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varAccruedInt">
        <xsl:choose>
          <xsl:when test="$varFXRate=0">
            <xsl:value-of select="AccruedInterest"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
            <xsl:value-of select="format-number(AccruedInterest * $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
            <xsl:value-of select="format-number(AccruedInterest div $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varAccruedInterest">
        <xsl:choose>
          <xsl:when test ="contains(Asset,'FixedIncome')">
            <xsl:value-of select="format-number($varAccruedInt,'#.##')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varInterestindicator">
        <xsl:choose>
          <xsl:when test ="contains(Asset,'FixedIncome')">
            <xsl:value-of select="'F'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>


      <xsl:variable name="varFundMapping">
        <xsl:choose>
          <xsl:when test="AccountName = 'Octahedron Fund'">
            <xsl:value-of select="'038CADJ92'"/>
          </xsl:when>
          <xsl:when test="AccountName = 'Octahedron MS Swap'">
            <xsl:value-of select="'06178JIB9'"/>
          </xsl:when>
		  <xsl:when test="AccountName = 'MSPA In House'">
            <xsl:value-of select="'MSPA In House'"/>
          </xsl:when>
		  <xsl:when test="AccountName = 'Octahedron Fund GS Swap'">
            <xsl:value-of select="'038QAH5I7'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>



      <Group
					TransStatus="{$varTaxlotStateTx}" BuySell="{$Sidevar}" LongShort="{$varLongShort}" 
					 ClientRef="{PBUniqueID}"  AccountID="038211165"
					ExecBkr="{$CPVar}" SecType="{'Swap'}" SecID="{BBCode}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
					SCCY="{SettlCurrency}"	 qty="{$QtySum}" 	Price="{format-number($varAveragePriceSettl,'###.00000')}"	 prin="{format-number($varGroupGrsAmtSettl,'###.00')}"
					comm="{format-number($varCOMM,'###.00')}" 
					Taxfees="{$varTaxFees}"
					Tax2="0"
					interest="{$varAccruedInterest}" interestindicator="{$varInterestindicator}" netamount="{format-number($varNetamountSettl,'###.00')}" 
				
					EntityID="{EntityID}" TaxLotState="{TaxLotState}">


        <!-- ...selecting all the records for this Group... -->
        <!--<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID and CounterParty != 'Undefined' and contains(AccountName,'Quantum Partners LP')!='true' and contains(AccountName,'MS Investment Partners')!='true']">-->
        <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset' and AccountName != 'Quantum Partners LP']">
          <!-- ...and building a ThirdPartyFlatFileDetail for each -->
          <!--<xsl:if test="TaxLotState!='Deleted'">-->

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

          <xsl:variable name="PRANA_FUND_NAME" select="AccountMappedName"/>

          <xsl:variable name="THIRDPARTY_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
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
              <xsl:when test="number(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee +SecFee) &gt; 0">
                <xsl:value-of select="StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee +SecFee"/>
              </xsl:when>
              <xsl:when test="number(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee + SecFee) &lt; 0">
                <xsl:value-of select="(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee +SecFee) * -1"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <xsl:variable name="amp">N</xsl:variable>

          <ThirdPartyFlatFileDetail
								TaxLotState="{TaxLotState}" TransStatus="{$varTaxlotState}" BuySell="{$Sidevar}" LongShort="{$varLongShort}" 
							 ClientRef="{concat(PBUniqueID,'038Q51978')}"   AccountID="038QAH5I7"
								ExecBkr="{$CPVar}" SecType="{'Swap'}" SecID="{BBCode}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
								SCCY="{SettlCurrency}"	 qty="{AllocatedQty}" Price="{format-number($varAveragePriceSettl,'###.00000')}"  prin="{format-number($varGroupGrsAmtSettl,'###.00')}"
								comm="{format-number($varCOMM,'###.00')}" 
								Taxfees="{$varTaxFees}"
								Tax2="0"
								interest="{$varAccruedInterest}" interestindicator="{$varInterestindicator}" netamount="{format-number($varNetamountSettl,'###.00')}" 
							 
								EntityID="{EntityID}" />
          <!--</xsl:if>-->
        </xsl:for-each>

      </Group>
    </xsl:if>




    <xsl:if test="TaxLotState = 'Deleted' and (AccountName='Napean MS Swap 0617853O7') and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset'">

      <xsl:variable name="AllocatedQty" />
      <!-- Building a Group with the EntityID $I_GroupID... -->
      <xsl:variable name="QtySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/AllocatedQty)"/>
      </xsl:variable>
      <xsl:variable name="GroupNetAmt">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/NetAmount)"/>
      </xsl:variable>
      <xsl:variable name="CommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/CommissionCharged)"/>
      </xsl:variable>
      <xsl:variable name="TaxOnCommissionSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/TaxOnCommissions)"/>
      </xsl:variable>
      <xsl:variable name="SecFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/SecFee)"/>
      </xsl:variable>
      <xsl:variable name="StampDutySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/StampDuty)"/>
      </xsl:variable>
      <xsl:variable name="TransactionLevySum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/TransactionLevy)"/>
      </xsl:variable>
      <xsl:variable name="ClearingFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/ClearingFee)"/>
      </xsl:variable>
      <xsl:variable name="MiscFeesSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/MiscFees)"/>
      </xsl:variable>
      <xsl:variable name="OrfFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/OrfFee)"/>
      </xsl:variable>
      <xsl:variable name="OtherBrokerFeeSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/OtherBrokerFee)"/>
      </xsl:variable>
      <xsl:variable name="GrossAmountSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/GrossAmount)"/>
      </xsl:variable>
      <xsl:variable name="NetAmountSum">
        <xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/NetAmount)"/>
      </xsl:variable>
      <!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
      <xsl:variable name="tempSideVar">
        <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/Side"/>
      </xsl:variable>

      <xsl:variable name="tempCPVar">
        <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][AccountName !='Quantum Partners LP']/CounterParty"/>
      </xsl:variable>

      <xsl:variable name="PB_NAME" select="'MS'"/>

      <xsl:variable name="PB_COUNTERPARTY_NAME">
        <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$tempCPVar]/@ThirdPartyBrokerID"/>
      </xsl:variable>

      <xsl:variable name="CPVar">
        <!--<xsl:choose>
				<xsl:when test="$tempCPVar='CUTTONE' or $tempCPVar='CUTN'">CUTE</xsl:when>
				<xsl:when test="$tempCPVar='SAND'">SDLR</xsl:when>
				<xsl:when test="$tempCPVar='ISGI'">INTS</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="$tempCPVar"/>
				</xsl:otherwise>
			</xsl:choose>-->
        <xsl:choose>
          <xsl:when test="$PB_COUNTERPARTY_NAME!=''">
            <xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$tempCPVar"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>


      <xsl:variable name="Sidevar">
        <xsl:choose>
          <xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">Buy</xsl:when>
          <xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">Buytocover</xsl:when>
          <xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">Sell</xsl:when>
          <xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">Sellshort</xsl:when>
          <xsl:otherwise></xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
	  <xsl:variable name="varLongShort">
        <xsl:choose>
          <xsl:when test="$tempSideVar='Buy' or $tempSideVar='Buy to Open'">Long</xsl:when>
          <xsl:when test="$tempSideVar='Buy to Close' or $tempSideVar='Buy to Cover'">Short</xsl:when>
          <xsl:when test="$tempSideVar='Sell' or $tempSideVar='Sell to Close'">Long</xsl:when>
          <xsl:when test="$tempSideVar='Sell short' or $tempSideVar='Sell to Open'">Short</xsl:when>
          <xsl:otherwise></xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
	  
      <xsl:variable name="tempTaxlotStateVar">
        <xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotStateID>1]/TaxLotState"/>
      </xsl:variable>

      <xsl:variable name="varTaxlotStateGrp">
        <xsl:choose>
          <xsl:when test="$tempTaxlotStateVar != ''">COR</xsl:when>
          <xsl:otherwise>NEW</xsl:otherwise>
        </xsl:choose>
      </xsl:variable>


      <!--<xsl:variable name ="varSymbolBef" select ="substring-before(Symbol,' ')"/>
					<xsl:variable name ="varSymbolAft" select ="substring-after(Symbol,' ')"/>
					<xsl:value-of select="concat($varSymbolBef,'/',$varSymbolAft)"/>-->

      <!--<xsl:variable name="varSymbol">
			<xsl:choose>
				<xsl:when test="Asset='EquityOption'">
					
					<xsl:value-of select="translate(OSIOptionSymbol,' ','')"/>					
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>-->

      <xsl:variable name="varSymbol">
        <xsl:choose>
          <xsl:when test ="Asset='EquityOption'">
            <xsl:value-of select="OSIOptionSymbol"/>
          </xsl:when>

          <xsl:when test="SEDOL != ''">
            <xsl:value-of select="SEDOL"/>
          </xsl:when>
          <xsl:when test="CUSIP != ''">
            <xsl:value-of select="CUSIP"/>
          </xsl:when>
          <xsl:when test="ISIN != ''">
            <xsl:value-of select="ISIN"/>
          </xsl:when>


          <xsl:when test="Symbol != ''">
            <xsl:value-of select="Symbol"/>
          </xsl:when>
          <xsl:when test="RIC != ''">
            <xsl:value-of select="RIC"/>
          </xsl:when>
          <xsl:when test="BBCode != ''">
            <xsl:value-of select="BBCode"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select ="Symbol"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varSecType">
        <xsl:choose>
          <xsl:when test ="Asset='EquityOption'">
            <xsl:value-of select="'T'"/>
          </xsl:when>

          <xsl:when test="SEDOL != ''">
            <xsl:value-of select="'S'"/>
          </xsl:when>
          <xsl:when test="CUSIP != ''">
            <xsl:value-of select="'C'"/>
          </xsl:when>
          <xsl:when test="ISIN != ''">
            <xsl:value-of select="'I'"/>
          </xsl:when>


          <xsl:when test="Symbol != ''">
            <xsl:value-of select="'T'"/>
          </xsl:when>
          <xsl:when test="RIC != ''">
            <xsl:value-of select="'R'"/>
          </xsl:when>
          <xsl:when test="BBCode != ''">
            <xsl:value-of select="'B'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select ="'T'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="GroupGrsAmt">
        <xsl:value-of select="$QtySum * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
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

      <xsl:variable name="GroupNetAmt1">
        <xsl:value-of select="$GroupGrsAmt + (($CommissionSum + $StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum) * $GroupSideMultiplier)"/>
      </xsl:variable>

      <xsl:variable name="PRANA_EXCHANGE" select="Exchange"/>

      <xsl:variable name="PB_EXCHANGE">
        <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name=$PB_NAME]/ExchangeData[@PranaExchange=$PRANA_EXCHANGE]/@PBExchangeName"/>
      </xsl:variable>

      <xsl:variable name="varEXCODE">
        <xsl:choose>
          <xsl:when test ="$PB_EXCHANGE!=''">
            <xsl:value-of select="$PB_EXCHANGE"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="Exchange"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varTaxlotStateTx">
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
            <xsl:value-of select ="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <!--<xsl:variable name="PRANA_FUND_NAME_Group" select="FundName"/>

		<xsl:variable name="THIRDPARTY_FUND_NAME_Group">
			<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME_Group]/@PBFundCode"/>
		</xsl:variable>

		<xsl:variable name="AccountId_Group">
			<xsl:choose>
				<xsl:when test="$THIRDPARTY_FUND_NAME_Group!=''">
					<xsl:value-of select="$THIRDPARTY_FUND_NAME_Group"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$PRANA_FUND_NAME_Group"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>-->

      <!--<xsl:variable name="varCustodian">
				<xsl:choose>
					<xsl:when test="AccountName='Sheffield Partners LP-MS'">
						<xsl:value-of select="'MSCO'"/>
					</xsl:when>
					<xsl:when test="AccountName='Sheffield Partners LP-JPM'">
						<xsl:value-of select="'JPMS'"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>-->

      <xsl:variable name="GroupStamp">
        <xsl:value-of select="$GroupGrsAmt * 0.0000184"/>
      </xsl:variable>

      <xsl:variable name="GroupOrf">
        <xsl:value-of select="$QtySum * 0.04"/>
      </xsl:variable>

      <xsl:variable name="GroupTotalFees">
        <xsl:choose>
          <xsl:when test="number($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum + $SecFeeSum) &gt; 0">
            <xsl:value-of select="($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum + $SecFeeSum)"/>
          </xsl:when>
          <xsl:when test="number($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum +$SecFeeSum) &lt; 0">
            <xsl:value-of select="($StampDutySum + $TransactionLevySum + $ClearingFeeSum + $MiscFeesSum + $OtherBrokerFeeSum + $TaxOnCommissionSum + $OrfFeeSum +$SecFeeSum) * -1"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="0"/>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:variable>
      <xsl:variable name="TransactionType2">
        <xsl:choose>
          <!--<xsl:when test="Asset='Equity' and IsSwapped='true'">-->
		   <xsl:when test="AccountName='Octahedron Fund GS Swap' or AccountName='Octahedron MS Swap'">
            <xsl:value-of select="'SW002'"/>
          </xsl:when>
          <xsl:when test="Asset='FX'">
            <xsl:value-of select="'FX002'"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="'TR001'"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varCustodian1">
       <xsl:choose>
          <xsl:when test="AccountMappedName='038CADJ92' and Asset='Equity' and IsSwapped='true'">
            <xsl:value-of select="'MSSW'"/>
          </xsl:when>

          <xsl:when test="AccountName='Octahedron MS Swap'">
            <xsl:value-of select="'MSSW'"/>
          </xsl:when>
          <xsl:when test="AccountMappedName='038CADJ92'and IsSwapped='false'">
            <xsl:value-of select="'MSCO'"/>
          </xsl:when>
		  <xsl:when test="AccountName='MSPA In House'">
            <xsl:value-of select="'MSCO'"/>
          </xsl:when>
		  <xsl:when test="AccountName='MSPA In House'">
            <xsl:value-of select="'MSCO'"/>
          </xsl:when>
		  <xsl:when test="AccountName='Octahedron Fund GS Swap'">
            <xsl:value-of select="'GSSW'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varFXRate">
        <xsl:choose>
          <xsl:when test="SettlCurrency != CurrencySymbol">
            <xsl:value-of select="FXRate_Taxlot"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="1"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>



      <xsl:variable name="varGroupGrsAmtSettl">
        <xsl:choose>
          <xsl:when test="$varFXRate=0">
            <xsl:value-of select="$GroupGrsAmt"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
            <xsl:value-of select="format-number($GroupGrsAmt * $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
            <xsl:value-of select="format-number($GroupGrsAmt div $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>


      <xsl:variable name="GroupNetAmtFX">
        <xsl:choose>
          <xsl:when test ="contains(Asset,'FixedIncome')">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Close'">
                <xsl:value-of select="$GroupGrsAmt + AccruedInterest"/>
              </xsl:when>

              <xsl:when test="Side='Sell' or Side='Sell short'">
                <xsl:value-of select="$GroupGrsAmt - AccruedInterest"/>
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$GroupNetAmt"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varNetamountSettl">
        <xsl:choose>
          <xsl:when test="$varFXRate=0">
            <xsl:value-of select="$GroupNetAmtFX"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
            <xsl:value-of select="format-number($GroupNetAmtFX * $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
            <xsl:value-of select="format-number($GroupNetAmtFX div $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>


      <xsl:variable name="varCOMM">
        <xsl:choose>
          <xsl:when test="$varFXRate=0">
            <xsl:value-of select="$CommissionSum"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
            <xsl:value-of select="format-number($CommissionSum * $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
            <xsl:value-of select="format-number($CommissionSum div $varFXRate,'##.00')"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="''"/>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>



      <xsl:variable name="varSettFxAmt">
        <xsl:choose>
          <xsl:when test="SettlCurrency != CurrencySymbol">
            <xsl:choose>
              <xsl:when test="FXConversionMethodOperator_Trade ='M'">
                <xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="AveragePrice"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varAveragePriceSettl">
        <xsl:choose>
          <xsl:when test="SettlCurrency = CurrencySymbol">
            <xsl:value-of select="AveragePrice"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$varSettFxAmt"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:variable name="varTaxFees">
        <xsl:choose>
          <xsl:when test="$varFXRate=0">
            <xsl:value-of select="$GroupTotalFees"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
            <xsl:value-of select="format-number($varFXRate * $GroupTotalFees,'0.##')"/>
          </xsl:when>

          <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
            <xsl:value-of select="format-number($GroupTotalFees div $varFXRate,'0.##')"/>
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="''"/>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="varFundMapping">
        <xsl:choose>
          <xsl:when test="AccountName = 'Octahedron Fund'">
            <xsl:value-of select="'038CADJ92'"/>
          </xsl:when>
          <xsl:when test="AccountName = 'Octahedron MS Swap'">
            <xsl:value-of select="'06178JIB9'"/>
          </xsl:when>
		  <xsl:when test="AccountName = 'MSPA In House'">
            <xsl:value-of select="'MSPA In House'"/>
          </xsl:when>
		   <xsl:when test="AccountName = 'Octahedron Fund GS Swap'">
            <xsl:value-of select="'038QAH5I7'"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <Group
					 BuySell="{$Sidevar}" LongShort="{$varLongShort}" 
				 ClientRef="{PBUniqueID}" ExecAccount="038211165" AccountID="038211165"
					ExecBkr="{$CPVar}" SecType="{'Swap'}" SecID="{BBCode}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
					SCCY="{SettlCurrency}"	qty="{$QtySum}" 	Price="{format-number($varAveragePriceSettl,'###.00000')}"	 prin="{format-number($varGroupGrsAmtSettl,'###.00')}"
					comm="{format-number($varCOMM,'###.00')}" 
					Taxfees="{$varTaxFees}"
					Tax2="0"
				 interest="0" interestindicator="" netamount="{format-number($varNetamountSettl,'###.00')}" 
					EntityID="{EntityID}" TaxLotState="{TaxLotState}">


        <!-- ...selecting all the records for this Group... -->
        <!--<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID and CounterParty != 'Undefined' and contains(AccountName,'Quantum Partners LP')!='true' and contains(AccountName,'MS Investment Partners')!='true']">-->
        <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID and CounterParty != 'Undefined' and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset' and contains(AccountName,'Quantum Partners LP')!= 'true']">
          <!-- ...and building a ThirdPartyFlatFileDetail for each -->
          <!--<xsl:if test="TaxLotState!='Deleted'">-->

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

          <xsl:variable name="PRANA_FUND_NAME" select="AccountMappedName"/>

          <xsl:variable name="THIRDPARTY_FUND_NAME">
            <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
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
              <xsl:when test="number(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee + SecFee) &gt; 0">
                <xsl:value-of select="StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee + SecFee"/>
              </xsl:when>
              <xsl:when test="number(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee + SecFee) &lt; 0">
                <xsl:value-of select="(StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions + OrfFee + SecFee) * -1"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>




          <xsl:variable name="amp">N</xsl:variable>

          <ThirdPartyFlatFileDetail
								TaxLotState="{TaxLotState}"  TransStatus="{$varTaxlotState}" BuySell="{$Sidevar}" LongShort="{$varLongShort}"
							 ClientRef="{concat(PBUniqueID,'038Q51978')}"  AccountID="{'038QAH5I7'}"
								ExecBkr="{$CPVar}" SecType="{'Swap'}" SecID="{BBCode}" desc="{FullSecurityName}" TDate="{TradeDate}" SDate="{SettlementDate}"
								SCCY="{SettlCurrency}"	qty="{AllocatedQty}" Price="{format-number($varAveragePriceSettl,'###.00000')}" prin="{format-number($varGroupGrsAmtSettl,'###.00')}"
								comm="{format-number($varCOMM,'###.00')}" 
								Taxfees="{$varTaxFees}"
								Tax2="0"
								 interest="0" interestindicator="" netamount="{format-number($varNetamountSettl,'###.00')}"
							
								EntityID="{EntityID}" />
          <!--</xsl:if>-->
        </xsl:for-each>

      </Group>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>