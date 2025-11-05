<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>

       

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
          <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

          <xsl:variable name="PB_NAME" select="'WellsFargo'"/>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountNo"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>


          <Symbol>
            <xsl:choose>
              <xsl:when test="CurrencySymbol ='USD'">
                <xsl:value-of select="Symbol"/>
              </xsl:when>
             
              <xsl:otherwise>
                <xsl:value-of select="SEDOL"/>
              </xsl:otherwise>
            </xsl:choose>
          </Symbol>

          <Account>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </Account>

          <Side>
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">
                <xsl:value-of select="'Buy'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'Buy_cover'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'Sell_Short'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close' or Side='Sell'">
                <xsl:value-of select="'Sell'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Side>

          <Quantity>
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>

          </Quantity>

          <Price>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </Price>

          <NetMoney>
            <xsl:value-of select="NetAmount"/>
          </NetMoney>

          <Currency>
            <xsl:value-of select="CurrencySymbol"/>
          </Currency>

          <ExecutionTime>
            <xsl:value-of select="''"/>
          </ExecutionTime>

          <Underlying>
                     <xsl:choose>
              <xsl:when test="(CurrencySymbol ='USD') and (Asset='EquityOption')">
                <xsl:value-of select="Symbol"/>
              </xsl:when>
			       <xsl:when test="CurrencySymbol!='USD' and (Asset='EquityOption')">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
             
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Underlying>

          <Expiration>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="ExpirationDate"/>
              </xsl:when>
             
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Expiration>

          <StrikePrice>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="StrikePrice"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </StrikePrice>

          <CallPutIndicator>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="PutOrCall"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </CallPutIndicator>

          <RootCode>
            <xsl:value-of select="''"/>
          </RootCode>

          <SecurityType>
            <xsl:choose>

              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'Swap'"/>
              </xsl:when>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'Equity'"/>
              </xsl:when>
              <xsl:when test="Asset='Future'">
                <xsl:value-of select="'Future'"/>
              </xsl:when>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'Option'"/>
              </xsl:when>
              <xsl:when test="Asset='FXForward'">
                <xsl:value-of select="'FX Forward'"/>
              </xsl:when>
              <xsl:when test="contains(Asset,'FX')">
                <xsl:value-of select="'Currency'"/>
              </xsl:when>

              <xsl:when test="contains(Asset,'FixedIncome')">
                <xsl:value-of select="'Bond'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Asset"/>
              </xsl:otherwise>

            </xsl:choose>
          </SecurityType>

          <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

          <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
          </xsl:variable>

          <xsl:variable name="Broker">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
                <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <ExecBroker>
            <xsl:value-of select="$Broker"/>
          </ExecBroker>

          <BookingCategory>
            <xsl:value-of select="$Broker"/>
          </BookingCategory>


          <xsl:variable name="varBrokerrate">
            <xsl:choose>
              <xsl:when test="CurrencySymbol='USD' ">
                <xsl:value-of select ="format-number(((CommissionCharged + SoftCommissionCharged) div AllocatedQty),'0.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <CommissionCentsPerShare>
            <xsl:value-of select="$varBrokerrate"/>
          </CommissionCentsPerShare>
		  
 <xsl:variable name="varCommissionFee">
            <xsl:choose>
              <xsl:when test="CurrencySymbol='USD' ">
                <xsl:value-of select ="format-number(((SecFee + OtherBrokerFee + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee) ),'0.####')"/>
              </xsl:when>
			  <xsl:when test="CurrencySymbol!='USD' ">
                <xsl:value-of select ="format-number(((SecFee + OtherBrokerFee + CommissionCharged + SoftCommissionCharged + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee) ),'0.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <CommissionFlatFee>
            <xsl:value-of select="$varCommissionFee"/>
          </CommissionFlatFee>

          <TradeDate>
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),'-',substring-before(SettlementDate,'/'),'-',substring-before(substring-after(SettlementDate,'/'),'/'))"/>
          </SettlementDate>

          <SecuritySubType>
            <xsl:value-of select="''"/>
          </SecuritySubType>

          <LocateID>
            <xsl:value-of select="LotId"/>
          </LocateID>

          <PositionEffect>
            <xsl:value-of select="''"/>
          </PositionEffect>

          <AllocID>
            <xsl:value-of select="EntityID"/>
          </AllocID>

          <ClOrdID>
            <xsl:value-of select="EntityID"/>
          </ClOrdID>

          <CoveredOrUncovered>
            <xsl:value-of select="''"/>
          </CoveredOrUncovered>

          <LastMarket>
            <xsl:value-of select="''"/>
          </LastMarket>

          <Exchange>
            <xsl:value-of select="Exchange"/>
          </Exchange>

          <ExecutionState>
            <xsl:value-of select="''"/>
          </ExecutionState>

          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>

          <SEDOL>
            <xsl:value-of select="SEDOL"/>
          </SEDOL>

          <GroupExecutionID>
            <xsl:value-of select="''"/>
          </GroupExecutionID>

          <ClientID>
            <xsl:value-of select="AccountNo"/>
          </ClientID>

          <Ticker>
            <xsl:value-of select="''"/>
          </Ticker>

          <TickerType>
            <xsl:value-of select="''"/>
          </TickerType>

          <Interest>
            <xsl:value-of select="''"/>
          </Interest>

          <Country>
            <xsl:value-of select="Country"/>
          </Country>

          <Fee_SEC>
            <xsl:value-of select="''"/>
          </Fee_SEC>

          <Fee_Stamp>
            <xsl:value-of select="''"/>
          </Fee_Stamp>

          <Fee_Other>
            <xsl:value-of select="''"/>
          </Fee_Other>

          <Yield>
            <xsl:value-of select="''"/>
          </Yield>

          <Principal>
            <xsl:value-of select="''"/>
          </Principal>

          <PSET>
            <xsl:value-of select="''"/>
          </PSET>


          <SettlInstID>
            <xsl:value-of select="''"/>
          </SettlInstID>

          <Fee_PTM>
            <xsl:value-of select="''"/>
          </Fee_PTM>

          <Fee_Tax>
            <xsl:value-of select="''"/>
          </Fee_Tax>

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

          <xsl:variable name="varPrice">
            <xsl:choose>
              <xsl:when test="SettlCurrency = CurrencySymbol">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varSettFxAmt"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
         
          <SettlementPrice>
            <xsl:choose>
           <xsl:when test="SettlCurrency!= CurrencySymbol and CurrencySymbol!='USD'">
                <xsl:value-of select="format-number($varPrice,'##.######')"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SettlementPrice>

          <SettlementCcy>
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">             
                     <xsl:value-of select="SettlCurrency"/>
               </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
          </SettlementCcy>

        
          <SettlementFxRate>
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SettlementFxRate>
		  
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
          
  <xsl:variable name="NetAmount">
            <xsl:value-of select="NetAmount"/>
          </xsl:variable>
		  
		    <xsl:variable name="varNetAmount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$NetAmount"/>
              </xsl:when>
			  
			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade ='M'">
                <xsl:value-of select="$NetAmount * $varFXRate"/>
              </xsl:when>
			  
				  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade ='D'">
                <xsl:value-of select="$NetAmount div $varFXRate"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
		  
		  
          <SettlementNetAmount>
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="format-number($varNetAmount,'##.######')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SettlementNetAmount>
          
        

          <xsl:variable name="Commission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

          <xsl:variable name="varCommission">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$Commission"/>
              </xsl:when>
			  
			  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade ='M'">
                <xsl:value-of select="$Commission * $varFXRate"/>
              </xsl:when>
			  
				  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade ='D'">
                <xsl:value-of select="$Commission div $varFXRate"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
		  
          <SettlementCommission>
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="format-number($varCommission,'##.######')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SettlementCommission>
		  
		   <xsl:variable name="varFeee23">
		  
		  <xsl:value-of select="SecFee + OtherBrokerFee + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
      </xsl:variable>
	  
          <SettlementFee>
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="format-number($varFeee23,'##.######')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SettlementFee>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>