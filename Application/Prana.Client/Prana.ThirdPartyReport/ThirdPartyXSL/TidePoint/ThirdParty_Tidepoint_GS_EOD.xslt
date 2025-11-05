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
      <xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[AccountName!='Rebal Risk Analysis' and CounterParty!='Transfer']">
        <!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
        <!--<xsl:if test="(1=position()) or(preceding-sibling::*[1]/TaxLotID != TaxLotID)">-->
          <!-- ...buid a Group for this node_id -->
          <xsl:call-template name="TaxLotIDBuilder">
            <xsl:with-param name="I_TaxLotID">
              <xsl:value-of select="TaxLotID" />
            </xsl:with-param>
          </xsl:call-template>
        <!--</xsl:if>-->
      </xsl:for-each>
    </Groups>
  </xsl:template>


  <xsl:template name="TaxLotIDBuilder">
    <xsl:param name="I_TaxLotID" />


    <xsl:variable name="varTransactionType">
      <xsl:choose>
	 <!--As requested by Alex on May 7, 2014-->
 	<xsl:when test="Side = 'Sell to Open' and Asset = 'EquityOption'">
          <xsl:value-of select="'SS'"/>
        </xsl:when>
        <xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="Side = 'Sell' or Side = 'Sell to Open' or Side = 'Sell to Close'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="Side = 'Buy to Close'">
          <xsl:value-of select="'BC'"/>
        </xsl:when>
        <xsl:when test="Side = 'Sell short'">
          <xsl:value-of select="'SS'"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varClientAccount">
      <xsl:choose>
        <xsl:when test ="AccountName = 'Newtyn Partners LP:522-91K57'">
          <xsl:value-of select ="'52291K57'"/>
        </xsl:when>
        <xsl:when test ="AccountName = 'Newtyn TE Partners LP:522-91K58'">
          <xsl:value-of select ="'52291K58'"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

	  <xsl:variable name ="varAccountNo">
		  <xsl:choose>
			  <xsl:when test ="AccountName= 'GS Newtyn Partners LP:002486561'">
				  <xsl:value-of select ="'002486561'"/>
			  </xsl:when>
			  <xsl:when test ="AccountName= 'GS Newtyn TE Partners LP:002486553'">
				  <xsl:value-of select ="'002486553'"/>
			  </xsl:when>
		  </xsl:choose>
	  </xsl:variable>


    <xsl:variable name="varSecurityID">
      <xsl:choose>
        <xsl:when test="Asset = 'EquityOption'">
          <xsl:value-of select="OSIOptionSymbol"/>
        </xsl:when>
		  <xsl:when test="Asset = 'FixedIncome'">
			  <xsl:value-of select="CUSIP"/>
		  </xsl:when>
        <xsl:when test="ISIN != ''">
          <xsl:value-of select="ISIN"/>
        </xsl:when>
        <xsl:when test="CUSIP != ''">
          <xsl:value-of select="CUSIP"/>
        </xsl:when>
        <xsl:when test="SEDOL != ''">
          <xsl:value-of select="SEDOL"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="Symbol"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varFeeType1">
      <xsl:choose>
        <xsl:when test="number(StampDuty)">
          <xsl:value-of select="'SEC'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varFeeValue1">
      <xsl:choose>
        <xsl:when test="number(StampDuty)">
          <xsl:value-of select="StampDuty"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varFeeType2">
      <xsl:choose>
        <xsl:when test="number(MiscFees)">
          <xsl:value-of select="'MSCF'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varFeeValue2">
      <xsl:choose>
        <xsl:when test="number(MiscFees)">
          <xsl:value-of select="MiscFees"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

	  <xsl:variable name ="varUnderlyingSymbol">
		  <xsl:choose>
			  <xsl:when test ="contains(Asset, 'Option')!= false">
				  <xsl:value-of select ="UnderlyingSymbol"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select ="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name ="varStrikePrice">
		  <xsl:choose>
			  <xsl:when test ="contains(Asset, 'Option')!= false">
				  <xsl:value-of select ="StrikePrice"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select ="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	

		  <xsl:variable name="varSymbol">
			  <xsl:choose>
				  <xsl:when test="contains(Asset,'EquityOption') and SettlCurrency!='USD'">
					  <xsl:value-of select="BBCode"/>
				  </xsl:when>
				  <xsl:when test="contains(Asset,'EquityOption')">
					  <xsl:value-of select="OSIOptionSymbol"/>
				  </xsl:when>
				  
				  <xsl:when test="SEDOL != '*' and SEDOL != ''">
					  <xsl:value-of select="SEDOL"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select ="Symbol"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </xsl:variable>


	  

	  <xsl:variable name ="varTradeTax">
		  <xsl:choose>
			  <xsl:when test ="CurrencySymbol != 'USD'">
				  <xsl:value-of select ="StampDuty"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select ="0"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="varSettFxAmt">
		  <xsl:choose>
			  <xsl:when test="SettlCurrency != CurrencySymbol">
				  <xsl:choose>
					  <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
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

	  <xsl:variable name="Price">
		  <xsl:choose>
			  <xsl:when test="SettlCurrency = CurrencySymbol">
				  <xsl:value-of select="format-number(AveragePrice,'#.0000')"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="format-number($varSettFxAmt,'#.0000')"/>
			  </xsl:otherwise>
		  </xsl:choose>

	  </xsl:variable>

 	<xsl:variable name ="varAllocationState">
		  <xsl:choose>
			  <xsl:when test ="TaxLotState = 'Amended'">
				  <xsl:value-of  select="'A'"/>
			  </xsl:when>
			  <xsl:when test ="TaxLotState = 'Deleted'">
				  <xsl:value-of  select="'C'"/>
			  </xsl:when>
			  <xsl:when test ="TaxLotState = 'Allocated'">
				  <xsl:value-of  select="'N'"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of  select="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="varSecurityName">
		  <xsl:choose>
			  <xsl:when test="Asset='Equity' and IsSwapped='true'">
				  <xsl:value-of select="'CFD'"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="varExpiration">
		  <xsl:choose>
			  <xsl:when test="contains(Asset,'EquityOption')">
				  <xsl:value-of select="ExpirationDate"/>
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
				  <xsl:value-of select="0"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>
	  
	  <xsl:variable name="Commission">
		  <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
	  </xsl:variable>

	  <xsl:variable name="VarComm">
		  <xsl:choose>
			  <xsl:when test="$varFXRate=0">
				  <xsl:value-of select="format-number($Commission,'##.00')"/>
			  </xsl:when>
			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
				  <xsl:value-of select="format-number($Commission * $varFXRate,'##.00')"/>
			  </xsl:when>

			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
				  <xsl:value-of select="format-number($Commission div $varFXRate,'##.00')"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="NetAmount">
		  <xsl:choose>
			  <xsl:when test="$varFXRate=0">
				  <xsl:value-of select="NetAmount"/>
			  </xsl:when>
			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
				  <xsl:value-of select="NetAmount * $varFXRate"/>
			  </xsl:when>

			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
				  <xsl:value-of select="NetAmount div $varFXRate"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>
	  
	  <xsl:variable name="varNetAmount">
		  <xsl:value-of select="format-number($NetAmount,'#.00')"/>
	  </xsl:variable>

	  <xsl:variable name="GrossAmount">
		  <xsl:choose>
			  <xsl:when test="$varFXRate=0">
				  <xsl:value-of select="GrossAmount"/>
			  </xsl:when>
			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
				  <xsl:value-of select="GrossAmount * $varFXRate"/>
			  </xsl:when>

			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
				  <xsl:value-of select="GrossAmount div $varFXRate"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>

	  <xsl:variable name="varGrossAmount">
		  <xsl:value-of select="format-number($GrossAmount,'#.00')"/>
	  </xsl:variable>

	 


	  <xsl:variable name="Broker">
		  <xsl:choose>
			  <xsl:when test="AccountName='MIO Onshore' and CounterParty='GSCO'">
				  <xsl:value-of select="'GOLD'"/>
			  </xsl:when>
			  <xsl:when test="AccountName='MIO Offshore' and CounterParty='GSCO'">
				  <xsl:value-of select="'GOLD'"/>
			  </xsl:when>
			  <xsl:when test="AccountName='Stevens Capital' and CounterParty='GSCO'">
				  <xsl:value-of select="'GOLD'"/>
			  </xsl:when>

				<xsl:when test="AccountName='Diamond Growth GS' and CounterParty='GSCO'">
				  <xsl:value-of select="'GOLD'"/>
			  </xsl:when>
			  
			  <xsl:when test="AccountName='Diamond Neutral GS' and CounterParty='GSCO'">
				  <xsl:value-of select="'GOLD'"/>
			  </xsl:when>

			  <xsl:when test="AccountName='Walleye WMO' and CounterParty='GSCO'">
				  <xsl:value-of select="'GOLD'"/>
			  </xsl:when>
			  
			  <xsl:when test="AccountName='Walleye WMO Systematic' and CounterParty='GSCO'">
				  <xsl:value-of select="'GOLD'"/>
			  </xsl:when>

			  <xsl:when test="Asset ='Equity' and CounterParty='JONES'">
				  <xsl:value-of select="'JONESE'"/>
			  </xsl:when>
			  <xsl:when test="Asset ='EquityOption' and CounterParty='JONES'">
				  <xsl:value-of select="'JONESO'"/>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select="CounterParty"/>
			  </xsl:otherwise>
		  </xsl:choose>
	  </xsl:variable>
	  <Group 
			  OrderNumber ="{EntityID}" Cancelcorrectindicator ="{$varAllocationState}" AccountNumber="{AccountNo}" SecurityIdentifier="{$varSymbol}"
			  Broker="{$Broker}" Custodian="GSCO" TransactionType="{$varTransactionType}" CurrencyCode="{SettlCurrency}"
			  TradeDate = "{TradeDate}" SettleDate = "{SettlementDate}"
			  Quantity="{AllocatedQty}" Commission="{format-number($VarComm,'#.00')}" Price="{$Price}" AccruedInterest="{format-number(AccruedInterest,'#.00')}" 
			  TradeTax="{$varTradeTax}" MiscMoney="{MiscFees}" NetAmount="{$varNetAmount}" Principal="{$varGrossAmount}" Description=""
			  SecurityType="{$varSecurityName}" CountrySettlementCode="" ClearingAgent="" SECFee="" RepoOpenSettleDate="" RepoMaturityDate=""
        RepoRate="" RepoInterest="" OptionUnderlyer="{$varUnderlyingSymbol}" OptionExpiryDate="{$varExpiration}" OptionCallPutIndicator="{substring(PutOrCall,1,1)}"
        OptionStrikePrice="{$varStrikePrice}" Trailer=""
		GenevaLotNumber1="" GainsKeeperLotNumber1="" LotDate1="" LotQty1="" LotPrice1=""
		GenevaLotNumber2 ="" GainsKeeperLotNumber2="" LotDate2="" LotQty2="" LotPrice2=""
		GenevaLotNumber3="" GainsKeeperLotNumber3="" LotDate3="" LotQty3="" LotPrice3=""
		GenevaLotNumber4 ="" GainsKeeperLotNumber4="" LotDate4="" LotQty4="" LotPrice4=""
		GenevaLotNumber5="" GainsKeeperLotNumber5="" LotDate5="" LotQty5="" LotPrice5=""
       
        EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="" FileHeader="FALSE" FileFooter="FALSE">

    </Group>
  </xsl:template>
</xsl:stylesheet>
