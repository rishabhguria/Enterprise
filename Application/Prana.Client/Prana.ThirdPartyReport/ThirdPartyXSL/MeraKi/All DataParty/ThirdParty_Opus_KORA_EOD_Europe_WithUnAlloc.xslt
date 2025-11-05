<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  
  <xsl:template name="Count">
    <xsl:param name="Symbol"/>
    <xsl:value-of select="count(//ThirdPartyFlatFileDetail[Symbol=$Symbol])"/>
  </xsl:template>

  <xsl:template name="SumPrice">
    <xsl:param name="Symbol"/>
    <xsl:value-of select="sum(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/AveragePrice)"/>
  </xsl:template>

  <xsl:template name="BLK">
    <xsl:param name="ID"/>
    <xsl:param name="Symbol"/>
    <xsl:choose>
      <xsl:when test="$ID = (//ThirdPartyFlatFileDetail[Symbol=$Symbol]/PBUniqueID)">
        <xsl:value-of select="(//ThirdPartyFlatFileDetail[Symbol=$Symbol]/PBUniqueID)"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset !='FX' and Asset !='FXForward' and  contains(AccountName,'Kora') 
	  and (UnderLyingName = 'EU')]">

        <ThirdPartyFlatFileDetail>

          <xsl:variable name="Count">
            <xsl:call-template name="Count">
              <xsl:with-param name="Symbol" select="Symbol"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="SumPrice">
            <xsl:call-template name ="SumPrice">
              <xsl:with-param name="Symbol" select="Symbol"/>
            </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="AvgofPrice">
            <xsl:value-of select="AveragePrice"/>
          </xsl:variable>

          <xsl:variable name="BLK">
            <xsl:call-template name="BLK">
              <xsl:with-param name="ID" select="PBUniqueID"/>
              <xsl:with-param name="Symbol" select="Symbol"/>
            </xsl:call-template>
          </xsl:variable>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>


         
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


          <Side>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'BY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'SL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Open'">
                <xsl:value-of select="'BO'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'CS'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Open'">
                <xsl:value-of select="'SO'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close'">
                <xsl:value-of select="'SC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>           
          </Side>

          <Complete>
			<xsl:value-of select="format-number(AllocatedQty,'###,###.####')"/>
          </Complete>

          <Symbol>
		   <xsl:choose>
		    <xsl:when test="contains(substring-after(substring-after(BBCode,' '),' '),'Equity') ">
                <xsl:value-of select ="substring-before(BBCode,' Equity')"/>
              </xsl:when>
              <xsl:when test="contains(substring-after(substring-after(BBCode,' '),' '),'EQUITY') ">
                <xsl:value-of select ="substring-before(BBCode,' EQUITY')"/>
              </xsl:when>
			  <xsl:when test="contains(substring-after(substring-after(BBCode,' '),' '),'equity') ">
                <xsl:value-of select ="substring-before(BBCode,' equity')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
              <!-- <xsl:value-of select="BBCode"/> -->
          </Symbol>
          
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

          <xsl:variable name ="varAvgPrice">
            <xsl:choose>
              <xsl:when test="SettlCurrency = CurrencySymbol">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varSettFxAmt"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <ExPrice>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="format-number($varAvgPrice,'###,##0.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>           
          </ExPrice>
		  
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
		  
          <xsl:variable name="SideMultiplier">
            <xsl:choose>
              <xsl:when test="SideTag = '1'  or SideTag = 'A' or SideID = 'B' or SideTag = '3' ">
                <xsl:value-of select ="1"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="-1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varCommission" select="(CommissionCharged + SoftCommissionCharged)"/>

          <xsl:variable name="varOtherFees" select="(OtherBrokerFees + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions+ MiscFees + SecFee + OccFee + OrfFee + ClearingBrokerFee)"/>

          <xsl:variable name="CalcNetAmt">
            <xsl:value-of select="(AllocatedQty * AveragePrice * AssetMultiplier) + (($varCommission + $varOtherFees) * $SideMultiplier)"/>
          </xsl:variable>

          <xsl:variable name="NetPrice">
            <xsl:value-of select="$CalcNetAmt div (AllocatedQty * AssetMultiplier)"/>
          </xsl:variable>

          <xsl:variable name="varVWAP">
            <xsl:value-of select="$SumPrice"/>
          </xsl:variable>
          <VWAP>
            <xsl:value-of select="''"/>
          </VWAP>
          
          
          <!--<xsl:variable name="varLeaves">
            <xsl:value-of select="((AllocatedQty div ExecutedQty) * TotalQty) - AllocatedQty"/>
          </xsl:variable>
          <Leaves>
			        <xsl:value-of select="format-number($varLeaves,'###,##0.####')"/>
          </Leaves>-->          
         
          <Leaves>
			<xsl:value-of select="format-number(RemainingQty,'###,##0.####')"/>
          </Leaves>

          <I>
            <xsl:value-of select="'|'"/>            
          </I>

		    <xsl:variable name="varGrossAmount">
            <xsl:value-of select="GrossAmount"/>
          </xsl:variable>

          <xsl:variable name="Grossamount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$varGrossAmount"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$varGrossAmount * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="$varGrossAmount div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
        
           <GrossVal>
            <xsl:choose>
              <xsl:when test="number($Grossamount)">
                <xsl:value-of select="format-number($Grossamount,'###,##0.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>            
          </GrossVal>

          <xsl:variable name="varNetAmount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="NetAmount"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="NetAmount * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="NetAmount div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <NetVal>
            <xsl:choose>
              <xsl:when test="number($varNetAmount)">
                <xsl:value-of select="format-number($varNetAmount,'###,##0.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>            
          </NetVal>
		  
		   <xsl:variable name="Fees">
           
			<xsl:value-of select="ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
			 
          </xsl:variable>
			
			
		   <xsl:variable name="Fees1">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$Fees"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$Fees * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="$Fees div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Fees>
            <xsl:value-of select="format-number(($Fees1),'###,##0.####')"/>
          </Fees>
		  
		  <xsl:variable name="Commission">
          
			<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
			 
          </xsl:variable>
			
			
		   <xsl:variable name="Commission1">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$Commission"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$Commission * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="$Commission div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <BrokerComm>
            <xsl:value-of select="format-number(($Commission1),'###,##0.####')"/>
          </BrokerComm>
          
      
		  
		    <!--<xsl:variable name="varBrokerrate">
		  <xsl:choose>
              <xsl:when test="CurrencySymbol='USD' ">
                <xsl:value-of select ="format-number(((CommissionCharged + SoftCommissionCharged) div AllocatedQty),'0.####')"/>
              </xsl:when>
              <xsl:otherwise>
             <xsl:value-of select="format-number((((CommissionCharged + SoftCommissionCharged) div GrossAmount) * 10000),'0.####')"/>
              </xsl:otherwise>
            </xsl:choose>
			</xsl:variable>-->
          
          <xsl:variable name="varBrokerrate">
		  <xsl:choose>
              <xsl:when test="GrossAmount != 0 ">
                <xsl:value-of select="format-number((((CommissionCharged + SoftCommissionCharged) div GrossAmount) * 10000),'0.####')"/>
              </xsl:when>
              <xsl:otherwise>
             <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
			</xsl:variable>
			
			
          <Brokerrate>
			 <xsl:value-of select="format-number(($varBrokerrate),'###,##0.####')"/>
          </Brokerrate>
		  
		  <xsl:variable name="MerakiComm">
          
			<xsl:value-of select="OtherBrokerFee"/>
		
          </xsl:variable>
			
			
		   <xsl:variable name="MerakiComm1">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$MerakiComm"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$MerakiComm * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="$MerakiComm div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <MerakiComm>
            <xsl:value-of select="format-number(($MerakiComm1),'###,##0.####')"/>
          </MerakiComm>
		  
		  		    <!--<xsl:variable name="varMerakirate">
		  <xsl:choose>
              <xsl:when test="CurrencySymbol='USD' ">
                <xsl:value-of select ="format-number(((OtherBrokerFee) div AllocatedQty),'0.####')"/>
              </xsl:when>
              <xsl:otherwise>
             <xsl:value-of select="format-number((((OtherBrokerFee) div GrossAmount) * 10000),'0.####')"/>
              </xsl:otherwise>
            </xsl:choose>
			</xsl:variable>-->
          
          <xsl:variable name="varMerakirate">
		  <xsl:choose>
              <xsl:when test="GrossAmount != 0 ">
                <xsl:value-of select="format-number((((OtherBrokerFee) div GrossAmount) * 10000),'0.####')"/>
              </xsl:when>
              <xsl:otherwise>
             <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
			</xsl:variable>

          <Merakirate>
		   <xsl:value-of select="format-number(($varMerakirate),'###,##0.####')"/>
          </Merakirate>
		  
		   <xsl:variable name="TotalComm">
          
			<xsl:value-of select="CommissionCharged + SoftCommissionCharged + OtherBrokerFee"/>

          </xsl:variable>
			
			
		   <xsl:variable name="TotalComm1">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$TotalComm"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$TotalComm * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="$TotalComm div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <TotalComm>
            <xsl:value-of select="format-number(($TotalComm1),'###,##0.####')"/>
          </TotalComm>

		   <xsl:variable name="VarFxrate">
		  <xsl:choose>
              <xsl:when test="FXRate_Taxlot='0'">
                <xsl:value-of select ="1"/>
              </xsl:when>
              <xsl:otherwise>
             <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:otherwise>
            </xsl:choose>
			</xsl:variable>
		  
          <FXRate>
            <xsl:value-of select="format-number($VarFxrate,'###,##0.####')"/>
          </FXRate>

          <SettleCcy>
            <xsl:value-of select="SettlCurrency"/>
          </SettleCcy>

          <Broker>
            <xsl:value-of select="CounterParty"/>
          </Broker>

          <PB>
            <xsl:value-of select="''"/>
          </PB>

          <TD>
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </TD>

          <SD>
            <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),'-',substring-before(SettlementDate,'/'),'-',substring-before(substring-after(SettlementDate,'/'),'/'))"/>
          </SD>

         <Acct>
		   <xsl:choose>
		     <xsl:when test="contains(AccountName,'Mult')">
                <xsl:value-of select="'Mult'"/>
              </xsl:when>		
              <xsl:otherwise>
                <xsl:value-of select="AccountName"/>
              </xsl:otherwise>
            </xsl:choose> 
        </Acct>

         
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
