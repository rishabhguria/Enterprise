<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month=1">
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month=2">
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month=3">
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month=4">
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month=5">
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month=6">
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month=7">
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month=8">
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month=9">
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail[CounterParty!= 'Transfer' and CounterParty!= 'Corporate action' and CounterParty!= 'Cost Adjustment']">

        <xsl:variable name="varNetamount">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test ="TaxLotState!='Amemded'">
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'false'"/>
              </RowHeader>
              <FileHeader>
                <xsl:value-of select="'true'"/>
              </FileHeader>

              <FileFooter>
                <xsl:value-of select="'true'"/>
              </FileFooter>

              <TaxLotState>
                <xsl:value-of select ="TaxLotState"/>
              </TaxLotState>

              <RecordType>
                <xsl:value-of select="'DET'"/>
              </RecordType>

              <GFFVersion>
                <xsl:value-of select="'1'"/>
              </GFFVersion>

              <GeneralInfo>
                <xsl:value-of select="concat('&lt;','&lt;','GeneralInfo','>','>')"/>
              </GeneralInfo>

              <SenderId>
                <!-- <xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/> -->
			          	<xsl:value-of select="'3365602'"/>
              </SenderId>

              <Destination>
                <xsl:value-of select="'NTC'"/>
              </Destination>

              <SendersReference>
                <xsl:choose>
				<xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of select="concat(PBUniqueID,position())"/>
                  </xsl:when>
                  <!-- <xsl:when test ="TaxLotState = 'Allocated'">
                    <xsl:value-of  select="PBUniqueID"/>
                  </xsl:when> -->		  
			      	   

                  <xsl:otherwise>
                    <xsl:value-of  select="PBUniqueID"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SendersReference>

              <xsl:variable name ="varAllocationState">
                <xsl:choose>
                  <xsl:when test ="TaxLotState = 'Allocated'">
                    <xsl:value-of  select="'N'"/>
                  </xsl:when>
				  
			      	   <xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of  select="'C'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of  select="'SENT'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <Functionofthemessage>
                <xsl:value-of select="$varAllocationState"/>
              </Functionofthemessage>

              <ReferenceInfo>
			       <xsl:value-of select="concat('&lt;','&lt;','ReferenceInfo','>','>')"/>
        
              </ReferenceInfo>

              <CountCurrentinstruction>
                <xsl:value-of select="''"/>
              </CountCurrentinstruction>

              <CountTotalNoInstructions>
                <xsl:value-of select="''"/>
              </CountTotalNoInstructions>

              <PreviousReference1>
                 <xsl:choose>
                  <xsl:when test ="TaxLotState = 'Allocated'">
                    <xsl:value-of  select="''"/>
                  </xsl:when>
				  
			      	   <xsl:when test ="TaxLotState = 'Deleted'">
                    <xsl:value-of  select="PBUniqueID"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of  select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </PreviousReference1>

              <PreviousReference2>
                <xsl:value-of select="''"/>
              </PreviousReference2>

              <PreviousReference3>
                <xsl:value-of select="''"/>
              </PreviousReference3>

              <PreviousReference4>
                <xsl:value-of select="''"/>
              </PreviousReference4>

              <PreviousReference5>
                <xsl:value-of select="''"/>
              </PreviousReference5>

              <PreadviseReference>
                <xsl:value-of select="''"/>
              </PreadviseReference>

              <PoolReference>
                <xsl:value-of select="''"/>
              </PoolReference>

              <DealReference>
                <xsl:value-of select="''"/>
              </DealReference>

              <TradeInfo>
                <xsl:value-of select="concat('&lt;','&lt;','TradeInfo','>','>')"/>
              </TradeInfo>

              <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
              </TradeDate>

              <PlaceofTrade>
                <xsl:value-of select="''"/>
              </PlaceofTrade>

              <InstructionType>
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'P'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'S'"/>
                  </xsl:when>
                
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </InstructionType>

              <xsl:variable name="varSecurityID">
                <xsl:choose>
                  <xsl:when test="Asset = 'EquityOption'">
                    <xsl:value-of select="'OP'"/>
                  </xsl:when>
                  <xsl:when test="Asset = 'Equity'">
                    <xsl:value-of select="'EQ'"/>
                  </xsl:when>

                  <xsl:when test="Asset='FixedIncome'">
                    <xsl:value-of select="'DB'"/>
                  </xsl:when>


                  <xsl:when test="Asset = 'Future'">
                    <xsl:value-of select="FT"/>
                  </xsl:when>


                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <ProductType>
                <xsl:value-of select="$varSecurityID"/>
              </ProductType>

              <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettlementDate>
                <xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
              </SettlementDate>

              <QuantityCVOriginalFace>
                <xsl:value-of select="OrderQty"/>
              </QuantityCVOriginalFace>
              <ALLOCQTY>
                <xsl:value-of select="OrderQty"/>
              </ALLOCQTY>
              <xsl:variable name="PB_NAME" select="'Northern Trust'"/>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>


              <xsl:variable name="varAccountName">
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <SafekeepingAccount>
                <xsl:value-of select="$varAccountName"/>
              </SafekeepingAccount>

              <TradeType>
                <xsl:value-of select="'TR'"/>
              </TradeType>


              <xsl:variable name ="varCurrencySymbol">
                <xsl:value-of select ="CurrencySymbol"/>
              </xsl:variable>

              <xsl:variable name ="varBICCodeMapping">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name= $PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol]/@BICCode"/>
              </xsl:variable>
              <PlaceofSettlementBIC>
            
								<xsl:choose>
		   
		   <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'DAKVDEFFXXX'"/>
              </xsl:when>
			  <xsl:when test="Exchange ='Euronext'">
                <xsl:value-of select="'NECINL2AXXX'"/>
              </xsl:when>
             
			 
              <xsl:otherwise>
                <xsl:value-of select="$varBICCodeMapping"/>
              </xsl:otherwise>
            </xsl:choose>
							
						
              </PlaceofSettlementBIC>

              <PlaceofSettlementISO>
                <xsl:value-of select="''"/>
              </PlaceofSettlementISO>

              <PlaceofSafekeeping>
                <xsl:value-of select="''"/>
              </PlaceofSafekeeping>

              <SecurityInfo>
                <xsl:value-of select="concat('&lt;','&lt;','SecurityInfo','>','>')"/>
              </SecurityInfo>


              <xsl:variable name="varSecurityIDType">
                <xsl:choose>
                  <xsl:when test="CUSIP!='' and SettlCurrency ='USD'">
                    <xsl:value-of select="'CU'"/>
                  </xsl:when>
                  
                  <xsl:when test="SEDOL!='' and  SettlCurrency !='USD'">
                    <xsl:value-of select="'SD'"/>
                  </xsl:when>
				  
				  <xsl:when test="ISIN!=''">
                    <xsl:value-of select="'IS'"/>
                  </xsl:when>
                  <xsl:when test="Symbol!=''">
                    <xsl:value-of select="'TC'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <SecurityIDType>
                <xsl:value-of select="$varSecurityIDType"/>
              </SecurityIDType>

              <xsl:variable name="varSecurity">
                <xsl:choose>

                 <xsl:when test="CUSIP != '' and SettlCurrency ='USD'">
                    <xsl:value-of select ="CUSIP"/>
                  </xsl:when>
                 

                  <xsl:when test="SEDOL != '' and SettlCurrency !='USD'">
                    <xsl:value-of select ="SEDOL"/>
                  </xsl:when>
				  <xsl:when test="ISIN != ''">
                    <xsl:value-of select ="ISIN"/>
                  </xsl:when>

                  <xsl:when test="Symbol!='' ">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <SecurityID >
                <xsl:value-of select="$varSecurity"/>
              </SecurityID>

              <SecurityDescription>
                <xsl:value-of select="CompanyName"/>
              </SecurityDescription>

              <SecurityType>
                <xsl:value-of select="''"/>
              </SecurityType>

              <PlaceofListing>
                <xsl:value-of select="''"/>
              </PlaceofListing>

              <BrokerInfo>
                <xsl:value-of select="concat('&lt;','&lt;','BrokerInfo','>','>')"/>
              </BrokerInfo>

			     <xsl:variable name="PRANA_CURRENCY_NAME" select="CurrencySymbol"/>
              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBIC"/>
              </xsl:variable>

              <ExecutingBrokerBIC>
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC"/>
              </ExecutingBrokerBIC>
              
			  <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
              <xsl:variable name="THIRDPARTY_BROKER">
                <xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
              </xsl:variable>
			  
			      <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerLocalCode"/>
              </xsl:variable>
			  
              <ExecutingBrokerLocalCode>
                 <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode"/>
              </ExecutingBrokerLocalCode>


              <ExecutingBrokerName>
                <xsl:value-of select="'JONES TRADING'"/>
              </ExecutingBrokerName>

			       <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerBIC"/>
              </xsl:variable>

			   <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBICType">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBICType"/>
							</xsl:variable>
              <ClearingBrokerBIC>
                <xsl:choose>
		   
		   <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'PARBDEFFXXX'"/>
              </xsl:when>
              <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType = 'BIC'">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
              </xsl:when>
			   <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
              </xsl:when>
			 
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
              </ClearingBrokerBIC>

			       <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerLocalCode">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerLocalCode"/>
              </xsl:variable>

              <ClearingBrokerLocalCode>
            
              <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                <xsl:value-of select="'0161'"/>
              </xsl:when>
              <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
              </xsl:when>
			 
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
              </ClearingBrokerLocalCode>

              <ClearingBrokerName>
               <xsl:value-of select="'JONES TRADING'"/>
              </ClearingBrokerName>

              <ExecBrokersAccount>
                <xsl:value-of select="''"/>
              </ExecBrokersAccount>

              <ClearingBrokersAccountFedThirdParty>
                <xsl:value-of select="''"/>
              </ClearingBrokersAccountFedThirdParty>

              <IntermediateBrokerBIC>
                <xsl:value-of select="''"/>
              </IntermediateBrokerBIC>

              <IntermediateBrokerLocalCode>
                <xsl:value-of select="''"/>
              </IntermediateBrokerLocalCode>

              <IntermediateBrokerName>
                <xsl:value-of select="''"/>
              </IntermediateBrokerName>

              <IntermediateBrokerAcct>
                <xsl:value-of select="''"/>
              </IntermediateBrokerAcct>

              <SettlementInfo>
              <xsl:value-of select="concat('&lt;', '&lt;','SettlementInfo','>','>')"/>
              </SettlementInfo>


              <xsl:variable name="varSettFxAmt">
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:choose>
                      <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                        <xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varPrice">
                <xsl:choose>
                  <xsl:when test="SettlCurrency = CurrencySymbol">
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$varSettFxAmt"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <Price>
                <xsl:value-of select="format-number($varPrice,'0.######')"/>
              </Price>

              <TradingCurrency>
                  <xsl:value-of select="SettlCurrency"/>
              </TradingCurrency>

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

              <xsl:variable name="varSettlementAmount">
                <xsl:choose>
                  <xsl:when test="$varFXRate=0">
                    <xsl:value-of select="$varNetamount"/>
                  </xsl:when>
                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
                    <xsl:value-of select="$varNetamount * $varFXRate"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
                    <xsl:value-of select="$varNetamount div $varFXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <TradeAmount>
               <xsl:choose>
              <xsl:when test ="Side='Buy'">
                <xsl:value-of select ="format-number((OrderQty * $varPrice),'#.##')"/>
              </xsl:when>
              <xsl:when test ="Side='Sell'">
                <xsl:value-of select ="format-number((OrderQty * $varPrice),'#.##')"/>
              </xsl:when>
            </xsl:choose>             
              </TradeAmount>


              <SettlementCurrency>
                <xsl:value-of select="SettlCurrency"/>
              </SettlementCurrency>

             
             <SettlementAmount>
                 <xsl:value-of select="format-number($varSettlementAmount,'0.####')"/>             
               </SettlementAmount> 
               <InternalNetNotional>
                <xsl:value-of select="format-number($varSettlementAmount,'0.####')"/>
              </InternalNetNotional> 
              <ExecutingBrokersAmount>
                <xsl:value-of select="''"/>
              </ExecutingBrokersAmount>

              <TaxCost>
                <xsl:value-of select="''"/>
              </TaxCost>

              <NetGainLossAmount >
                <xsl:value-of select="''"/>
              </NetGainLossAmount>


              <ChargesFees>
			   <xsl:value-of select="format-number((OtherBrokerFees + ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee),'0.######')"/>
                    <!-- <xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/> -->
              </ChargesFees>

              <ConsumptionTax>
                <xsl:value-of select="''"/>
              </ConsumptionTax>

              <CountryNationalFederalTax >
                <xsl:value-of select="''"/>
              </CountryNationalFederalTax>

              <IssueDiscountAllowance >
                <xsl:value-of select="''"/>
              </IssueDiscountAllowance>

              <PaymentLevyTax >
                <xsl:value-of select="''"/>
              </PaymentLevyTax>

              <LocalTax >
                <xsl:value-of select="''"/>
              </LocalTax>

              <LocalBrokersCommission >
			  <xsl:value-of select="format-number((CommissionCharged + SoftCommissionCharged),'0.######')"/>
             <!--    <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/> -->
              </LocalBrokersCommission>

              <Margin>
                <xsl:value-of select="''"/>
              </Margin>

              <OtherAmount>
                <xsl:value-of select="''"/>
              </OtherAmount>

              <PostageAmount>
                <xsl:value-of select="''"/>
              </PostageAmount>

              <RegulatoryAmount>
                <xsl:value-of select="''"/>
              </RegulatoryAmount>

              <ShippingAmount>
                <xsl:value-of select="''"/>
              </ShippingAmount>

              <SpecialConcessionsAmount >
                <xsl:value-of select="''"/>
              </SpecialConcessionsAmount>

              <StampDuty>
			  <xsl:value-of select="format-number(StampDuty,'0.######')"/>
             <!--    <xsl:value-of select="StampDuty"/> -->
              </StampDuty>

              <StockExchangeTax >
                <xsl:value-of select="''"/>
              </StockExchangeTax>

              <TransferTax>
                <xsl:value-of select="''"/>
              </TransferTax>

              <TransactionTax>
                <xsl:value-of select="''"/>
              </TransactionTax>

              <Value-AddedTax>
                <xsl:value-of select="''"/>
              </Value-AddedTax>

              <WithholdingTax>
                <xsl:value-of select="''"/>
              </WithholdingTax>

              <ResultingAmount>
                <xsl:value-of select="''"/>
              </ResultingAmount>

              <WireAccountwithInstitution-BIC>
                <xsl:value-of select="''"/>
              </WireAccountwithInstitution-BIC>

              <WireAccountwithInstitution-LocalCode>
                <xsl:value-of select="''"/>
              </WireAccountwithInstitution-LocalCode>

              <WireAccountwithInstitution-Name>
                <xsl:value-of select="''"/>
              </WireAccountwithInstitution-Name>

              <xsl:variable name="THIRDPARTY_CURRENCY_WireBeneficiaryBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@WireBeneficiaryBIC"/>
              </xsl:variable>

              <WireBeneficiary-BIC>
                <xsl:value-of select="''"/>
              </WireBeneficiary-BIC>

             

              <WireBeneficiary-LocalCode>
                  <xsl:value-of select="''"/>
              </WireBeneficiary-LocalCode>

              <WireBeneficiary-Name>
                <xsl:value-of select="''"/>
              </WireBeneficiary-Name>

              <WireBeneficiaryAccount>
                 <xsl:value-of select="''"/>
              </WireBeneficiaryAccount>

              <MiscellaneousInfo>
                   <xsl:value-of select="concat('&lt;','&lt;','MiscellaneousInfo','>','>')"/>
              </MiscellaneousInfo>


              <Narrative>
                <xsl:value-of select="''"/>
              </Narrative>

              <ShortSaleBuytoCover>
                <xsl:value-of select="''"/>
              </ShortSaleBuytoCover>

              <Taxable>
                <xsl:value-of select="''"/>
              </Taxable>

              <FreeCleanSettlement>
                <xsl:value-of select="''"/>
              </FreeCleanSettlement>

              <Physical>
                <xsl:value-of select="''"/>
              </Physical>

              <SpecialDelivery>
                <xsl:value-of select="''"/>
              </SpecialDelivery>

              <SplitSettlement>
                <xsl:value-of select="''"/>
              </SplitSettlement>

              <StampDutyCode>
                <xsl:value-of select="''"/>
              </StampDutyCode>

              <RealTimeGrossSettlement>
                <xsl:value-of select="''"/>
              </RealTimeGrossSettlement>

              <Beneficiary>
                <xsl:value-of select="''"/>
              </Beneficiary>

              <FXSSI>
                <xsl:value-of select="''"/>
              </FXSSI>

              <BlockSettlement>
                <xsl:value-of select="''"/>
              </BlockSettlement>

              <CollateralTypeIndicator>
                <xsl:value-of select="''"/>
              </CollateralTypeIndicator>

              <Tracking>
                <xsl:value-of select="''"/>
              </Tracking>

              <FXInstruction>
                <xsl:value-of select="'USD'"/>
              </FXInstruction>

              <DebtFields>
                <xsl:value-of select="concat('&lt;','&lt;','DebtFields','>','>')"/>
              </DebtFields>

              <LateDeliveryDate>
                <xsl:value-of select="''"/>
              </LateDeliveryDate>

              <CurrentFace>
                <xsl:value-of select="''"/>
              </CurrentFace>

              <MaturityDate>
                <xsl:value-of select="''"/>
              </MaturityDate>

              <InterestRate>
                <xsl:value-of select="''"/>
              </InterestRate>

              <AccruedInterestAmount>
                <xsl:value-of select="AccruedInterest"/>
              </AccruedInterestAmount>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>
          </xsl:when>

          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">
              <ThirdPartyFlatFileDetail>

                <RowHeader>
                  <xsl:value-of select ="'false'"/>
                </RowHeader>
                <FileHeader>
                  <xsl:value-of select="'true'"/>
                </FileHeader>

                <FileFooter>
                  <xsl:value-of select="'true'"/>
                </FileFooter>

                <TaxLotState>
                  <xsl:value-of select="'Deleted'"/>
                </TaxLotState>

                <RecordType>
                  <xsl:value-of select="'DET'"/>
                </RecordType>

                <GFFVersion>
                  <xsl:value-of select="'1'"/>
                </GFFVersion>

                <GeneralInfo>
                  <xsl:value-of select="concat('&lt;','&lt;','GeneralInfo','>','>')"/>
                </GeneralInfo>

              <SenderId>
                <!-- <xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/> -->
				<xsl:value-of select="'3365602'"/>
              </SenderId>

                <Destination>
                  <xsl:value-of select="'NTC'"/>
                </Destination>

                <SendersReference>
                  <xsl:value-of select="substring(AmendTaxLotId1,string-length(AmendTaxLotId1)-7,string-length(AmendTaxLotId1))"/>
                </SendersReference>

               

                <Functionofthemessage>
                  <xsl:value-of  select="'C'"/>
                </Functionofthemessage>

                <ReferenceInfo>
                  <xsl:value-of select="concat('&lt;','&lt;','ReferenceInfo','>','>')"/>
                </ReferenceInfo>

                <CountCurrentinstruction>
                  <xsl:value-of select="''"/>
                </CountCurrentinstruction>

                <CountTotalNoInstructions>
                  <xsl:value-of select="''"/>
                </CountTotalNoInstructions>

                <PreviousReference1>
                  <xsl:value-of select="PBUniqueID"/>
                </PreviousReference1>

                <PreviousReference2>
                  <xsl:value-of select="''"/>
                </PreviousReference2>

                <PreviousReference3>
                  <xsl:value-of select="''"/>
                </PreviousReference3>

                <PreviousReference4>
                  <xsl:value-of select="''"/>
                </PreviousReference4>

                <PreviousReference5>
                  <xsl:value-of select="''"/>
                </PreviousReference5>

                <PreadviseReference>
                  <xsl:value-of select="''"/>
                </PreadviseReference>

                <PoolReference>
                  <xsl:value-of select="''"/>
                </PoolReference>

                <DealReference>
                  <xsl:value-of select="''"/>
                </DealReference>

                <TradeInfo>
                    <xsl:value-of select="concat('&lt;','&lt;','TradeInfo','>','>')"/>
                </TradeInfo>

                <xsl:variable name="varTradeDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldTradeDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <TradeDate>
                  <xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
                </TradeDate>

                <PlaceofTrade>
                  <xsl:value-of select="''"/>
                </PlaceofTrade>

                <InstructionType>
                  <xsl:choose>
                    <xsl:when test="OldSide='Buy'">
                      <xsl:value-of select="'P'"/>
                    </xsl:when>
                    <xsl:when test="OldSide='Sell'">
                      <xsl:value-of select="'S'"/>
                    </xsl:when>
                    <!--<xsl:when test="Side='Sell short'">
                <xsl:value-of select="'Sell Short'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'Buy to Close'"/>
              </xsl:when>-->
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </InstructionType>

                <xsl:variable name="varSecurityID">
                  <xsl:choose>
                    <xsl:when test="Asset = 'EquityOption'">
                      <xsl:value-of select="'OP'"/>
                    </xsl:when>
                    <xsl:when test="Asset = 'Equity'">
                      <xsl:value-of select="'EQ'"/>
                    </xsl:when>

                    <xsl:when test="Asset='FixedIncome'">
                      <xsl:value-of select="'DB'"/>
                    </xsl:when>


                    <xsl:when test="Asset = 'Future'">
                      <xsl:value-of select="FT"/>
                    </xsl:when>


                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <ProductType>
                  <xsl:value-of select="$varSecurityID"/>
                </ProductType>

                <xsl:variable name="varSettlementDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldSettlementDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <SettlementDate>
                  <xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
                </SettlementDate>

                <QuantityCVOriginalFace>
                  <xsl:value-of select="OldExecutedQuantity"/>
                </QuantityCVOriginalFace>

                <ALLOCQTY>
                  <xsl:value-of select="OldExecutedQuantity"/>
                </ALLOCQTY>
                <xsl:variable name="PB_NAME" select="'Northern Trust'"/>

                <xsl:variable name = "PRANA_FUND_NAME">
                  <xsl:value-of select="AccountName"/>
                </xsl:variable>

                <xsl:variable name ="THIRDPARTY_FUND_CODE">
                  <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
                </xsl:variable>


                <xsl:variable name="varAccountName">
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                      <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_FUND_NAME"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <SafekeepingAccount>
                   <xsl:value-of select="$varAccountName"/>
                </SafekeepingAccount>

                <TradeType>
                  <xsl:value-of select="'TR'"/>
                </TradeType>

                <xsl:variable name ="varCurrencySymbol">
                  <xsl:value-of select ="CurrencySymbol"/>
                </xsl:variable>

                <xsl:variable name ="varBICCodeMapping">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name= $PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol]/@BICCode"/>
              </xsl:variable>
                <PlaceofSettlementBIC>
                 
								<xsl:choose>
		   
		   <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'DAKVDEFFXXX'"/>
              </xsl:when>
			  <xsl:when test="Exchange ='Euronext'">
                <xsl:value-of select="'NECINL2AXXX'"/>
              </xsl:when>
             
			 
              <xsl:otherwise>
                <xsl:value-of select="$varBICCodeMapping"/>
              </xsl:otherwise>
            </xsl:choose>
							
						
                </PlaceofSettlementBIC>

                <PlaceofSettlementISO>
                  <xsl:value-of select="''"/>
                </PlaceofSettlementISO>

                <PlaceofSafekeeping>
                  <xsl:value-of select="''"/>
                </PlaceofSafekeeping>

                <SecurityInfo>
         <xsl:value-of select="concat('&lt;','&lt;','SecurityInfo','>','>')"/>
                </SecurityInfo>


                <xsl:variable name="varSecurityIDType">
                   <xsl:choose>
                  <xsl:when test="CUSIP!='' and SettlCurrency ='USD'">
                    <xsl:value-of select="'CU'"/>
                  </xsl:when>
                 
                  <xsl:when test="SEDOL!='' and SettlCurrency !='USD'">
                    <xsl:value-of select="'SD'"/>
                  </xsl:when>
				  
				   <xsl:when test="ISIN!=''">
                    <xsl:value-of select="'IS'"/>
                  </xsl:when>
                  <xsl:when test="Symbol!=''">
                    <xsl:value-of select="'TC'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
                </xsl:variable>
                <SecurityIDType>
                  <xsl:value-of select="$varSecurityIDType"/>
                </SecurityIDType>

                <xsl:variable name="varSecurity">
                  <xsl:choose>

                   <xsl:when test="CUSIP != '' and SettlCurrency ='USD'">
                    <xsl:value-of select ="CUSIP"/>
                  </xsl:when>				 

                  <xsl:when test="SEDOL != '' and SettlCurrency !='USD'">
                    <xsl:value-of select ="SEDOL"/>
                  </xsl:when>
				  
				   <xsl:when test="ISIN != ''">
                    <xsl:value-of select ="ISIN"/>
                  </xsl:when>

                  <xsl:when test="Symbol!='' ">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select ="Symbol"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <SecurityID >
                  <xsl:value-of select="$varSecurity"/>
                </SecurityID>

                <SecurityDescription>
                  <xsl:value-of select="CompanyName"/>
                </SecurityDescription>

                <SecurityType>
                  <xsl:value-of select="''"/>
                </SecurityType>

                <PlaceofListing>
                  <xsl:value-of select="''"/>
                </PlaceofListing>

                <BrokerInfo>
                  <xsl:value-of select="concat('&lt;','&lt;','BrokerInfo','>','>')"/>
                </BrokerInfo>

                <xsl:variable name="PRANA_CURRENCY_NAME" select="CurrencySymbol"/>
                <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBIC">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBIC"/>
                </xsl:variable>
                <ExecutingBrokerBIC>
                  <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC"/>
                </ExecutingBrokerBIC>


                <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="OldCounterParty"/>
                <xsl:variable name="THIRDPARTY_BROKER">
                  <xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
                </xsl:variable>

                <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerLocalCode"/>
                </xsl:variable>
                <ExecutingBrokerLocalCode>
                  <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode"/>
                </ExecutingBrokerLocalCode>


               <ExecutingBrokerName>
                <xsl:value-of select="'JONES TRADING'"/>
              </ExecutingBrokerName>

			       <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerBIC"/>
              </xsl:variable>

			   <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBICType">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBICType"/>
							</xsl:variable>
              <ClearingBrokerBIC>
                 <xsl:choose>
		   
		   <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'PARBDEFFXXX'"/>
              </xsl:when>
              <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType = 'BIC'">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
              </xsl:when>
			   <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
              </xsl:when>
			 
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
              </ClearingBrokerBIC>

			       <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerLocalCode">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerLocalCode"/>
              </xsl:variable>

              <ClearingBrokerLocalCode>
            
              <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                <xsl:value-of select="'0161'"/>
              </xsl:when>
              <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
              </xsl:when>
			 
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
              </ClearingBrokerLocalCode>

              <ClearingBrokerName>
              <xsl:value-of select="'JONES TRADING'"/>
              </ClearingBrokerName>

                <ExecBrokersAccount>
                  <xsl:value-of select="''"/>
                </ExecBrokersAccount>

                <ClearingBrokersAccountFedThirdParty>
                  <xsl:value-of select="''"/>
                </ClearingBrokersAccountFedThirdParty>

                <IntermediateBrokerBIC>
                  <xsl:value-of select="''"/>
                </IntermediateBrokerBIC>

                <IntermediateBrokerLocalCode>
                  <xsl:value-of select="''"/>
                </IntermediateBrokerLocalCode>

                <IntermediateBrokerName>
                  <xsl:value-of select="''"/>
                </IntermediateBrokerName>

                <IntermediateBrokerAcct>
                  <xsl:value-of select="''"/>
                </IntermediateBrokerAcct>

                <SettlementInfo>
         <xsl:value-of select="concat('&lt;', '&lt;','SettlementInfo','>','>')"/>
                </SettlementInfo>

                <xsl:variable name="varSettFxAmt">
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency != CurrencySymbol">
                      <xsl:choose>
                        <xsl:when test="OldFXConversionMethodOperator ='M'">
                          <xsl:value-of select="OldAvgPrice * OldFXRate"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="OldAvgPrice div OldFXRate"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="OldAvgPrice"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="varPrice">
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency = CurrencySymbol">
                      <xsl:value-of select="OldAvgPrice"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$varSettFxAmt"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <Price>
				
                  <xsl:choose>
                    <xsl:when test="number($varPrice)">
                      <xsl:value-of select="format-number($varPrice,'##.######')"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>

                </Price>


                <TradingCurrency>
                  <xsl:value-of select="SettlCurrency"/>
                </TradingCurrency>

                <xsl:variable name="varOldNetAmount">
                  <xsl:choose>
                    <xsl:when test="contains(OldSide,'Buy')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
                    </xsl:when>
                    <xsl:when test="contains(OldSide,'Sell')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>



                <xsl:variable name="varFXRate">
                  <xsl:choose>
                    <xsl:when test="OldSettlCurrency != CurrencySymbol">
                      <xsl:value-of select="OldFXRate"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="varSettlementAmount">
                  <xsl:choose>
                    <xsl:when test="$varFXRate=0">
                      <xsl:value-of select="$varOldNetAmount"/>
                    </xsl:when>
                    <xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='M'">
                      <xsl:value-of select="$varOldNetAmount * $varFXRate"/>
                    </xsl:when>

                    <xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='D'">
                      <xsl:value-of select="$varOldNetAmount div $varFXRate"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
               <TradeAmount>
                  <xsl:choose>
              <xsl:when test ="Side='Buy'">
                <xsl:value-of select ="format-number((OrderQty * $varPrice),'#.##')"/>
              </xsl:when>
              <xsl:when test ="Side='Sell'">
                <xsl:value-of select ="format-number((OrderQty * $varPrice),'#.##')"/>
              </xsl:when>
            </xsl:choose>                 
                </TradeAmount>


                <SettlementCurrency>
                  <xsl:value-of select="SettlCurrency"/>
                </SettlementCurrency>

              
                 <SettlementAmount>
                  <xsl:value-of select="format-number($varSettlementAmount,'0.####')"/>                  
                 </SettlementAmount> 
             <InternalNetNotional>
                <xsl:value-of select="format-number($varSettlementAmount,'0.####')"/>                
              </InternalNetNotional> 
                <xsl:variable name="varOldOtherFees">
                  <xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + TaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
                </xsl:variable>

                <xsl:variable name="varOtherFees1">
                  <xsl:choose>
                    <xsl:when test="$varFXRate=0">
                      <xsl:value-of select="$varOldOtherFees"/>
                    </xsl:when>
                    <xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='M'">
                      <xsl:value-of select="$varOldOtherFees * $varFXRate"/>
                    </xsl:when>

                    <xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='D'">
                      <xsl:value-of select="$varOldOtherFees div $varFXRate"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <ExecutingBrokersAmount>
                  <xsl:value-of select="''"/>
                </ExecutingBrokersAmount>

                <TaxCost>
                  <xsl:value-of select="''"/>
                </TaxCost>

                <NetGainLossAmount >
                  <xsl:value-of select="''"/>
                </NetGainLossAmount>

                <ChargesFees>
				<xsl:value-of select="format-number((OldOtherBrokerFees + OldClearingBrokerFee + OldTransactionLevy + OldClearingFee + TaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee),'0.######')"/>
     <!--                <xsl:value-of select="OldOtherBrokerFees + OldClearingBrokerFee + OldTransactionLevy + OldClearingFee + TaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/> -->
                </ChargesFees>

                <ConsumptionTax>
                  <xsl:value-of select="''"/>
                </ConsumptionTax>

                <CountryNationalFederalTax >
                  <xsl:value-of select="''"/>
                </CountryNationalFederalTax>

                <IssueDiscountAllowance >
                  <xsl:value-of select="''"/>
                </IssueDiscountAllowance>

                <PaymentLevyTax >
                  <xsl:value-of select="''"/>
                </PaymentLevyTax>

                <LocalTax >
                  <xsl:value-of select="''"/>
                </LocalTax>

                <LocalBrokersCommission >
				 <xsl:value-of select="format-number((OldCommission + OldSoftCommission),'0.######')"/>
                 <!--  <xsl:value-of select="OldCommission + OldSoftCommission"/> -->
                </LocalBrokersCommission>

                <Margin>
                  <xsl:value-of select="''"/>
                </Margin>

                <OtherAmount>
                  <xsl:value-of select="''"/>
                </OtherAmount>

                <PostageAmount>
                  <xsl:value-of select="''"/>
                </PostageAmount>

                <RegulatoryAmount>
                  <xsl:value-of select="''"/>
                </RegulatoryAmount>

                <ShippingAmount>
                  <xsl:value-of select="''"/>
                </ShippingAmount>

                <SpecialConcessionsAmount >
                  <xsl:value-of select="''"/>
                </SpecialConcessionsAmount>

                <StampDuty>
				<xsl:value-of select="format-number(OldStampDuty,'0.######')"/>
               <!--    <xsl:value-of select="OldStampDuty"/> -->
                </StampDuty>

                <StockExchangeTax >
                   <xsl:value-of select="''"/>
                </StockExchangeTax>

                <TransferTax>
                  <xsl:value-of select="''"/>
                </TransferTax>

                <TransactionTax>
                  <xsl:value-of select="''"/>
                </TransactionTax>

                <Value-AddedTax>
                  <xsl:value-of select="''"/>
                </Value-AddedTax>

                <WithholdingTax>
                  <xsl:value-of select="''"/>
                </WithholdingTax>

                <ResultingAmount>
                <xsl:value-of select="''"/>
                </ResultingAmount>

                <WireAccountwithInstitution-BIC>
                  <xsl:value-of select="''"/>
                </WireAccountwithInstitution-BIC>

                <WireAccountwithInstitution-LocalCode>
                  <xsl:value-of select="''"/>
                </WireAccountwithInstitution-LocalCode>

                <WireAccountwithInstitution-Name>
                  <xsl:value-of select="''"/>
                </WireAccountwithInstitution-Name>

                <xsl:variable name="THIRDPARTY_CURRENCY_WireBeneficiaryBIC">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@WireBeneficiaryBIC"/>
                </xsl:variable>

                <WireBeneficiary-BIC>
                  <xsl:value-of select="''"/>
                </WireBeneficiary-BIC>

                <WireBeneficiary-LocalCode>
                  <xsl:value-of select="''"/>
                </WireBeneficiary-LocalCode>

                <WireBeneficiary-Name>
                  <xsl:value-of select="''"/>
                </WireBeneficiary-Name>

                <WireBeneficiaryAccount>
                  <xsl:value-of select="''"/>
                </WireBeneficiaryAccount>

                <MiscellaneousInfo>
                    <xsl:value-of select="concat('&lt;','&lt;','MiscellaneousInfo','>','>')"/>
                </MiscellaneousInfo>


                <Narrative>
                  <xsl:value-of select="''"/>
                </Narrative>

                <ShortSaleBuytoCover>
                  <xsl:value-of select="''"/>
                </ShortSaleBuytoCover>

                <Taxable>
                  <xsl:value-of select="''"/>
                </Taxable>

                <FreeCleanSettlement>
                  <xsl:value-of select="''"/>
                </FreeCleanSettlement>

                <Physical>
                  <xsl:value-of select="''"/>
                </Physical>

                <SpecialDelivery>
                  <xsl:value-of select="''"/>
                </SpecialDelivery>

                <SplitSettlement>
                  <xsl:value-of select="''"/>
                </SplitSettlement>

                <StampDutyCode>
                  <xsl:value-of select="''"/>
                </StampDutyCode>

                <RealTimeGrossSettlement>
                  <xsl:value-of select="''"/>
                </RealTimeGrossSettlement>

                <Beneficiary>
                  <xsl:value-of select="''"/>
                </Beneficiary>

                <FXSSI>
                  <xsl:value-of select="''"/>
                </FXSSI>

                <BlockSettlement>
                  <xsl:value-of select="''"/>
                </BlockSettlement>

                <CollateralTypeIndicator>
                  <xsl:value-of select="''"/>
                </CollateralTypeIndicator>

                <Tracking>
                  <xsl:value-of select="''"/>
                </Tracking>

                <FXInstruction>
                  <xsl:value-of select="'USD'"/>
                </FXInstruction>

                <DebtFields>
                        <xsl:value-of select="concat('&lt;','&lt;','DebtFields','>','>')"/>
                </DebtFields>

                <LateDeliveryDate>
                  <xsl:value-of select="''"/>
                </LateDeliveryDate>

                <CurrentFace>
                  <xsl:value-of select="''"/>
                </CurrentFace>

                <MaturityDate>
                  <xsl:value-of select="''"/>
                </MaturityDate>

                <InterestRate>
                  <xsl:value-of select="''"/>
                </InterestRate>

                <AccruedInterestAmount>
                  <xsl:value-of select="OldAccruedInterest"/>
                </AccruedInterestAmount>



                <EntityID>
                  <xsl:value-of select="EntityID"/>
                </EntityID>

              </ThirdPartyFlatFileDetail>
            </xsl:if>
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'false'"/>
              </RowHeader>
              <FileHeader>
                <xsl:value-of select="'true'"/>
              </FileHeader>

              <FileFooter>
                <xsl:value-of select="'true'"/>
              </FileFooter>

              <TaxLotState>
                <xsl:value-of select="'Allocated'"/>
              </TaxLotState>

              <RecordType>
                <xsl:value-of select="'DET'"/>
              </RecordType>

              <GFFVersion>
                <xsl:value-of select="'1'"/>
              </GFFVersion>

              <GeneralInfo>
                <xsl:value-of select="concat('&lt;','&lt;','GeneralInfo','>','>')"/>
              </GeneralInfo>

              <SenderId>
                <!-- <xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/> -->
			        	<xsl:value-of select="'3365602'"/>
              </SenderId>

              <Destination>
                <xsl:value-of select="'NTC'"/>
              </Destination>

              <SendersReference>
                <xsl:value-of select="concat(PBUniqueID,position())"/>
              </SendersReference>

              <xsl:variable name ="varAllocationState">
                <xsl:value-of  select="'N'"/>
              </xsl:variable>

              <Functionofthemessage>
                <xsl:value-of select="$varAllocationState"/>
              </Functionofthemessage>

              <ReferenceInfo>
                     <xsl:value-of select="concat('&lt;','&lt;','ReferenceInfo','>','>')"/>
              </ReferenceInfo>

              <CountCurrentinstruction>
                <xsl:value-of select="''"/>
              </CountCurrentinstruction>

              <CountTotalNoInstructions>
                <xsl:value-of select="''"/>
              </CountTotalNoInstructions>

              <PreviousReference1>
                <xsl:value-of select="''"/>
              </PreviousReference1>

              <PreviousReference2>
                <xsl:value-of select="''"/>
              </PreviousReference2>

              <PreviousReference3>
                <xsl:value-of select="''"/>
              </PreviousReference3>

              <PreviousReference4>
                <xsl:value-of select="''"/>
              </PreviousReference4>

              <PreviousReference5>
                <xsl:value-of select="''"/>
              </PreviousReference5>

              <PreadviseReference>
                <xsl:value-of select="''"/>
              </PreadviseReference>

              <PoolReference>
                <xsl:value-of select="''"/>
              </PoolReference>

              <DealReference>
                <xsl:value-of select="''"/>
              </DealReference>

              <TradeInfo>
                     <xsl:value-of select="concat('&lt;','&lt;','TradeInfo','>','>')"/>
              </TradeInfo>

              <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
              </TradeDate>

              <PlaceofTrade>
                <xsl:value-of select="''"/>
              </PlaceofTrade>

              <InstructionType>
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'P'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'S'"/>
                  </xsl:when>
                 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </InstructionType>

              <xsl:variable name="varSecurityID">
                <xsl:choose>
                  <xsl:when test="Asset = 'EquityOption'">
                    <xsl:value-of select="'OP'"/>
                  </xsl:when>
                  <xsl:when test="Asset = 'Equity'">
                    <xsl:value-of select="'EQ'"/>
                  </xsl:when>

                  <xsl:when test="Asset='FixedIncome'">
                    <xsl:value-of select="'DB'"/>
                  </xsl:when>


                  <xsl:when test="Asset = 'Future'">
                    <xsl:value-of select="FT"/>
                  </xsl:when>


                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <ProductType>
                <xsl:value-of select="$varSecurityID"/>
              </ProductType>

              <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettlementDate>
                <xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
              </SettlementDate>

              <QuantityCVOriginalFace>
                <xsl:value-of select="OrderQty"/>
              </QuantityCVOriginalFace>

              <ALLOCQTY>
                <xsl:value-of select="OrderQty"/>
              </ALLOCQTY>
              <xsl:variable name="PB_NAME" select="'Northern Trust'"/>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>


              <xsl:variable name="varAccountName">
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <SafekeepingAccount>
                <xsl:value-of select="$varAccountName"/>
              </SafekeepingAccount>

              <TradeType>
                <xsl:value-of select="'TR'"/>
              </TradeType>

              <xsl:variable name ="varCurrencySymbol">
                <xsl:value-of select ="CurrencySymbol"/>
              </xsl:variable>

              <xsl:variable name ="varBICCodeMapping">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BICCodeMapping.xml')/BICCodeMapping/PB[@Name= $PB_NAME]/BICData[@CurrencyName=$varCurrencySymbol]/@BICCode"/>
              </xsl:variable>
              <PlaceofSettlementBIC>
             
								<xsl:choose>
		   
		   <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'DAKVDEFFXXX'"/>
              </xsl:when>
			  <xsl:when test="Exchange ='Euronext'">
                <xsl:value-of select="'NECINL2AXXX'"/>
              </xsl:when>
             
			 
              <xsl:otherwise>
                <xsl:value-of select="$varBICCodeMapping"/>
              </xsl:otherwise>
            </xsl:choose>
							
						
              </PlaceofSettlementBIC>

              <PlaceofSettlementISO>
                <xsl:value-of select="''"/>
              </PlaceofSettlementISO>

              <PlaceofSafekeeping>
                <xsl:value-of select="''"/>
              </PlaceofSafekeeping>

              <SecurityInfo>
                <xsl:value-of select="concat('&lt;','&lt;','SecurityInfo','>','>')"/>
              </SecurityInfo>


              <xsl:variable name="varSecurityIDType">
                 <xsl:choose>
                  <xsl:when test="CUSIP!='' and SettlCurrency ='USD'">
                    <xsl:value-of select="'CU'"/>
                  </xsl:when>
                 
                  <xsl:when test="SEDOL!='' and SettlCurrency !='USD'">
                    <xsl:value-of select="'SD'"/>
                  </xsl:when>
				  
				   <xsl:when test="ISIN!=''">
                    <xsl:value-of select="'IS'"/>
                  </xsl:when>
                  <xsl:when test="Symbol!=''">
                    <xsl:value-of select="'TC'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
              <SecurityIDType>
                <xsl:value-of select="$varSecurityIDType"/>
              </SecurityIDType>

              <xsl:variable name="varSecurity">
                <xsl:choose>

                  <xsl:when test="CUSIP != '' and SettlCurrency ='USD'">
                    <xsl:value-of select ="CUSIP"/>
                  </xsl:when>
				  
                  <xsl:when test="SEDOL != '' and SettlCurrency !='USD'">
                    <xsl:value-of select ="SEDOL"/>
                  </xsl:when>
				  
				   <xsl:when test="ISIN != ''">
                    <xsl:value-of select ="ISIN"/>
                  </xsl:when>

                  <xsl:when test="Symbol!='' ">
                    <xsl:value-of select="Symbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <SecurityID >
                <xsl:value-of select="$varSecurity"/>
              </SecurityID>

              <SecurityDescription>
                <xsl:value-of select="CompanyName"/>
              </SecurityDescription>

              <SecurityType>
                <xsl:value-of select="''"/>
              </SecurityType>

              <PlaceofListing>
                <xsl:value-of select="''"/>
              </PlaceofListing>

              <BrokerInfo>
                <xsl:value-of select="concat('&lt;','&lt;','BrokerInfo','>','>')"/>
              </BrokerInfo>

              <xsl:variable name="PRANA_CURRENCY_NAME" select="CurrencySymbol"/>
              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBIC"/>
              </xsl:variable>
              <ExecutingBrokerBIC>
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBIC"/>
              </ExecutingBrokerBIC>


              <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
              <xsl:variable name="THIRDPARTY_BROKER">
                <xsl:value-of select="document('../ReconMappingXml/ExecBrokerDTCMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@DTCCode"/>
              </xsl:variable>

              <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerLocalCode"/>
              </xsl:variable>
              <ExecutingBrokerLocalCode>
                 <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerLocalCode"/>
              </ExecutingBrokerLocalCode>


                <ExecutingBrokerName>
                <xsl:value-of select="'JONES TRADING'"/>
              </ExecutingBrokerName>

			       <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerBIC"/>
              </xsl:variable>

			   <xsl:variable name="THIRDPARTY_CURRENCY_ExecutingBrokerBICType">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ExecutingBrokerBICType"/>
							</xsl:variable>
              <ClearingBrokerBIC>
                <xsl:choose>
		   
		   <xsl:when test="Exchange ='FRA'">
                <xsl:value-of select="'PARBDEFFXXX'"/>
              </xsl:when>
              <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType = 'BIC'">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
              </xsl:when>
			   <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType"/>
              </xsl:when>
			 
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
              </ClearingBrokerBIC>

			       <xsl:variable name="THIRDPARTY_CURRENCY_ClearingBrokerLocalCode">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@ClearingBrokerLocalCode"/>
              </xsl:variable>

              <ClearingBrokerLocalCode>
            
              <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                <xsl:value-of select="'0161'"/>
              </xsl:when>
              <xsl:when test="$THIRDPARTY_CURRENCY_ExecutingBrokerBICType != 'BIC'">
                <xsl:value-of select="$THIRDPARTY_CURRENCY_ClearingBrokerBIC"/>
              </xsl:when>
			 
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
              </ClearingBrokerLocalCode>

              <ClearingBrokerName>
               <xsl:value-of select="'JONES TRADING'"/>
              </ClearingBrokerName>

              <ExecBrokersAccount>
                <xsl:value-of select="''"/>
              </ExecBrokersAccount>

              <ClearingBrokersAccountFedThirdParty>
                <xsl:value-of select="''"/>
              </ClearingBrokersAccountFedThirdParty>

              <IntermediateBrokerBIC>
                <xsl:value-of select="''"/>
              </IntermediateBrokerBIC>

              <IntermediateBrokerLocalCode>
                <xsl:value-of select="''"/>
              </IntermediateBrokerLocalCode>

              <IntermediateBrokerName>
                <xsl:value-of select="''"/>
              </IntermediateBrokerName>

              <IntermediateBrokerAcct>
                <xsl:value-of select="''"/>
              </IntermediateBrokerAcct>

              <SettlementInfo>
               <xsl:value-of select="concat('&lt;', '&lt;','SettlementInfo','>','>')"/>
              </SettlementInfo>

              <xsl:variable name="varSettFxAmt">
                <xsl:choose>
                  <xsl:when test="SettlCurrency != CurrencySymbol">
                    <xsl:choose>
                      <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                        <xsl:value-of select="AvgPrice * FXRate_Taxlot"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="AvgPrice div FXRate_Taxlot"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varPrice">
                <xsl:choose>
                  <xsl:when test="SettlCurrency = CurrencySymbol">
                    <xsl:value-of select="AvgPrice"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$varSettFxAmt"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <Price>
                <xsl:value-of select="format-number($varPrice,'0.####')"/>
              </Price>

              <TradingCurrency>
                  <xsl:value-of select="SettlCurrency"/>
              </TradingCurrency>

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

              <xsl:variable name="varSettlementAmount">
                <xsl:choose>
                  <xsl:when test="$varFXRate=0">
                    <xsl:value-of select="$varNetamount"/>
                  </xsl:when>
                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='M'">
                    <xsl:value-of select="$varNetamount * $varFXRate"/>
                  </xsl:when>

                  <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
                    <xsl:value-of select="$varNetamount div $varFXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


               <TradeAmount>
                <xsl:choose>
              <xsl:when test ="Side='Buy'">
                <xsl:value-of select ="format-number((OrderQty * $varPrice),'#.##')"/>
              </xsl:when>
              <xsl:when test ="Side='Sell'">
                <xsl:value-of select ="format-number((OrderQty * $varPrice),'#.##')"/>
              </xsl:when>
            </xsl:choose> 
              </TradeAmount>

              <SettlementCurrency>
                <xsl:value-of select="SettlCurrency"/>
              </SettlementCurrency>

            

             <SettlementAmount> 
                <xsl:value-of select="format-number($varSettlementAmount,'0.####')"/>              
              </SettlementAmount> 
             <InternalNetNotional>
                <xsl:value-of select="format-number($varSettlementAmount,'0.####')"/>
              </InternalNetNotional>
              
              <ExecutingBrokersAmount>
                <xsl:value-of select="''"/>
              </ExecutingBrokersAmount>

              <TaxCost>
               <xsl:value-of select="''"/>
              </TaxCost>

              <NetGainLossAmount >
                <xsl:value-of select="''"/>
              </NetGainLossAmount>
			  
			

               <ChargesFees>
			   <xsl:value-of select="format-number((OtherBrokerFees + ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee),'0.######')"/>
                <!-- <xsl:value-of select="OtherBrokerFees + ClearingBrokerFee + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/> -->
              </ChargesFees>

              <ConsumptionTax>
                <xsl:value-of select="''"/>
              </ConsumptionTax>

              <CountryNationalFederalTax >
                <xsl:value-of select="''"/>
              </CountryNationalFederalTax>

              <IssueDiscountAllowance >
                <xsl:value-of select="''"/>
              </IssueDiscountAllowance>

              <PaymentLevyTax >
                <xsl:value-of select="''"/>
              </PaymentLevyTax>

              <LocalTax >
                <xsl:value-of select="''"/>
              </LocalTax>

              <LocalBrokersCommission>
			  <xsl:value-of select="format-number((CommissionCharged + SoftCommissionCharged),'0.######')"/>
               <!-- <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/> -->
              </LocalBrokersCommission>

              <Margin>
                <xsl:value-of select="''"/>
              </Margin>

              <OtherAmount>
                <xsl:value-of select="''"/>
              </OtherAmount>

              <PostageAmount>
                <xsl:value-of select="''"/>
              </PostageAmount>

              <RegulatoryAmount>
                <xsl:value-of select="''"/>
              </RegulatoryAmount>

              <ShippingAmount>
                <xsl:value-of select="''"/>
              </ShippingAmount>

              <SpecialConcessionsAmount >
                <xsl:value-of select="''"/>
              </SpecialConcessionsAmount>

              <StampDuty>
			  <xsl:value-of select="format-number(StampDuty,'0.######')"/>
               <!--  <xsl:value-of select="StampDuty"/> -->
              </StampDuty>

              <StockExchangeTax >
                <xsl:value-of select="''"/>
              </StockExchangeTax>

              <TransferTax>
                <xsl:value-of select="''"/>
              </TransferTax>

              <TransactionTax>
                <xsl:value-of select="''"/>
              </TransactionTax>

              <Value-AddedTax>
                <xsl:value-of select="''"/>
              </Value-AddedTax>

              <WithholdingTax>
                <xsl:value-of select="''"/>
              </WithholdingTax>

              <ResultingAmount>
                <xsl:value-of select="''"/>
              </ResultingAmount>

              <WireAccountwithInstitution-BIC>
                <xsl:value-of select="''"/>
              </WireAccountwithInstitution-BIC>

              <WireAccountwithInstitution-LocalCode>
                <xsl:value-of select="''"/>
              </WireAccountwithInstitution-LocalCode>

              <WireAccountwithInstitution-Name>
                <xsl:value-of select="''"/>
              </WireAccountwithInstitution-Name>

              <xsl:variable name="THIRDPARTY_CURRENCY_WireBeneficiaryBIC">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Currency_BrokerCodeMapping.xml')/ExecutingBrokerBICMapping/PB[@Name=$PB_NAME]/BICData[@CurrencyName=$PRANA_CURRENCY_NAME]/@WireBeneficiaryBIC"/>
              </xsl:variable>

              <WireBeneficiary-BIC>
                <xsl:value-of select="''"/>
              </WireBeneficiary-BIC>

              <WireBeneficiary-LocalCode>
                <xsl:value-of select="''"/>
              </WireBeneficiary-LocalCode>

              <WireBeneficiary-Name>
                <xsl:value-of select="''"/>
              </WireBeneficiary-Name>

              <WireBeneficiaryAccount>
                <xsl:value-of select="''"/>
              </WireBeneficiaryAccount>

              <MiscellaneousInfo>
                <xsl:value-of select="concat('&lt;','&lt;','MiscellaneousInfo','>','>')"/>
              </MiscellaneousInfo>


              <Narrative>
                <xsl:value-of select="''"/>
              </Narrative>

              <ShortSaleBuytoCover>
                <xsl:value-of select="''"/>
              </ShortSaleBuytoCover>

              <Taxable>
                <xsl:value-of select="''"/>
              </Taxable>

              <FreeCleanSettlement>
                <xsl:value-of select="''"/>
              </FreeCleanSettlement>

              <Physical>
                <xsl:value-of select="''"/>
              </Physical>

              <SpecialDelivery>
                <xsl:value-of select="''"/>
              </SpecialDelivery>

              <SplitSettlement>
                <xsl:value-of select="''"/>
              </SplitSettlement>

              <StampDutyCode>
                <xsl:value-of select="''"/>
              </StampDutyCode>

              <RealTimeGrossSettlement>
                <xsl:value-of select="''"/>
              </RealTimeGrossSettlement>

              <Beneficiary>
                <xsl:value-of select="''"/>
              </Beneficiary>

              <FXSSI>
                <xsl:value-of select="''"/>
              </FXSSI>

              <BlockSettlement>
                <xsl:value-of select="''"/>
              </BlockSettlement>

              <CollateralTypeIndicator>
                <xsl:value-of select="''"/>
              </CollateralTypeIndicator>

              <Tracking>
                <xsl:value-of select="''"/>
              </Tracking>

              <FXInstruction>
                <xsl:value-of select="'USD'"/>
              </FXInstruction>

              <DebtFields>
                     <xsl:value-of select="concat('&lt;','&lt;','DebtFields','>','>')"/>
              </DebtFields>

              <LateDeliveryDate>
                <xsl:value-of select="''"/>
              </LateDeliveryDate>

              <CurrentFace>
                <xsl:value-of select="''"/>
              </CurrentFace>

              <MaturityDate>
                <xsl:value-of select="''"/>
              </MaturityDate>

              <InterestRate>
                <xsl:value-of select="''"/>
              </InterestRate>

              <AccruedInterestAmount>
                <xsl:value-of select="AccruedInterest"/>
              </AccruedInterestAmount>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
