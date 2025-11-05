<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

       

        <DealType>
          <xsl:value-of select="'DealType'"/>
        </DealType>

        <DealId>
          <xsl:value-of select="'DealId'"/>
        </DealId>


        <Action>
          <xsl:value-of select="'Action'"/>
        </Action>


        <Client>
          <xsl:value-of select="'Client'"/>
        </Client>
        <Reserved1>
            <xsl:value-of select="'Reserved1'"/>
          </Reserved1>
          
          <Reserved2>
              <xsl:value-of select="'Reserved2'"/>
          </Reserved2>
          <Folder>
             <xsl:value-of select="'Folder'"/>        
          </Folder>


        <Custodian>
          <xsl:value-of select="'Custodian'"/>
        </Custodian>

        <CashAccount>
          <xsl:value-of select="'CashAccount'"/>
        </CashAccount>

        
        <Counterparty>
          <xsl:value-of select="'Counterparty'"/>
        </Counterparty>

        <Comments>
          <xsl:value-of select="'Comments'"/>
        </Comments>

        <State>
          <xsl:value-of select="'State'"/>
        </State>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>
        

        <Reserved3>
          <xsl:value-of select="'Reserved3'"/>
        </Reserved3>
        
        <Reserved>
          <xsl:value-of select="'Reserved'"/>
        </Reserved>

         <InitialMargin>
             <xsl:value-of select="'InitialMargin'"/>   
          </InitialMargin>	
          
          <InitialMarginPercentage>
             <xsl:value-of select="'InitialMarginPercentage'"/>            
          </InitialMarginPercentage>
          
          
          <InitialMarginCurrency>
             <xsl:value-of select="'InitialMarginCurrency'"/>            
          </InitialMarginCurrency>
          
          <RegenerateCashFlow>
             <xsl:value-of select="'RegenerateCashFlow'"/>            
          </RegenerateCashFlow>
          
          <ReceiveUnderlyingType>
             <xsl:value-of select="'ReceiveUnderlyingType'"/>                   
          </ReceiveUnderlyingType>
          
          <ReceiveLegType>
             <xsl:value-of select="'ReceiveLegType'"/>                   
          </ReceiveLegType>
          
          <ReceiveFloatRate>
             <xsl:value-of select="'ReceiveFloatRate'"/>                   
          </ReceiveFloatRate>
          
          <ReceiveCusip>
             <xsl:value-of select="'ReceiveCusip'"/>                
          </ReceiveCusip>
          
          <ReceiveIsin> 
          <xsl:value-of select="'ReceiveIsin'"/>         
          </ReceiveIsin>
          
          <ReceiveSedol>
            <xsl:value-of select="'ReceiveSedol'"/>  
          
          </ReceiveSedol>	
          
          
          <ReceiveBloomberg>
            <xsl:value-of select="'ReceiveBloomberg'"/>  
          
          </ReceiveBloomberg>
          
          <ReceiveRIC>
            <xsl:value-of select="'ReceiveRIC'"/>            
         </ReceiveRIC>
          
          <ReceiveBasketIndex>
            <xsl:value-of select="'ReceiveBasketIndex'"/>           
          </ReceiveBasketIndex>
          
          <ReceiveAdditiveMargin>
            <xsl:value-of select="'ReceiveAdditiveMargin'"/>          
          </ReceiveAdditiveMargin>
          
          <ReceiveFirstCouponDate>
            <xsl:value-of select="'ReceiveFirstCouponDate'"/>           
          </ReceiveFirstCouponDate>
          
          <ReceiveFirstCouponRate>
            <xsl:value-of select="'ReceiveFirstCouponRate'"/>           
          </ReceiveFirstCouponRate>
          
          <ReceiveFixedRate>
            <xsl:value-of select="'ReceiveFixedRate'"/>            
          </ReceiveFixedRate>
          
          <ReceiveDaycount>
            <xsl:value-of select="'ReceiveDaycount'"/>          
          </ReceiveDaycount>	
          
          <ReceiveFrequency>
            <xsl:value-of select="'ReceiveFrequency'"/>  
          
          </ReceiveFrequency>	
          
          <ReceiveFixingFrequency>
            <xsl:value-of select="'ReceiveFixingFrequency'"/>  
          
          </ReceiveFixingFrequency>
          
          <ReceivePaymentRollConvention>
            <xsl:value-of select="'ReceivePaymentRollConvention'"/>           
          </ReceivePaymentRollConvention>	
          
          <ReceiveEffectiveDate>
            <xsl:value-of select="'ReceiveEffectiveDate'"/>          
          </ReceiveEffectiveDate>
          
          <ReceiveMaturityDate>
            <xsl:value-of select="'ReceiveMaturityDate'"/>           
          </ReceiveMaturityDate>	
          
          <ReceiveNotional>
            <xsl:value-of select="'ReceiveNotional'"/>  
          </ReceiveNotional>	
          
          <ReceiveCurrency>
            <xsl:value-of select="'ReceiveCurrency'"/>          
          </ReceiveCurrency>	
          
          <ReceiveCurrencyPair>
            <xsl:value-of select="'ReceiveCurrencyPair'"/>           
          </ReceiveCurrencyPair>
          
          <ReceiveSpotRate>
            <xsl:value-of select="'ReceiveSpotRate'"/>          
          </ReceiveSpotRate>
          
          <ReceiveResetLag>
            <xsl:value-of select="'ReceiveResetLag'"/>       
          </ReceiveResetLag>
          
          <ReceiveInArrears>
            <xsl:value-of select="'ReceiveInArrears'"/>       
          </ReceiveInArrears>
          
          <ReceiveDiscountCurve>
            <xsl:value-of select="'ReceiveDiscountCurve'"/>        
          </ReceiveDiscountCurve>
          
          <ReceiveSwapFee>
            <xsl:value-of select="'ReceiveSwapFee'"/>          
          </ReceiveSwapFee>
          
          <ReceiveQuantity>
            <xsl:value-of select="'ReceiveQuantity'"/>          
          </ReceiveQuantity>
          
          <ReceivePrice>
            <xsl:value-of select="'ReceivePrice'"/>         
          </ReceivePrice>
          
          <PayUnderlyingType>
            <xsl:value-of select="'PayUnderlyingType'"/>         
          </PayUnderlyingType>
          
          <PayLegRateType>
            <xsl:value-of select="'PayLegRateType'"/>  
          </PayLegRateType>
          
          <PayFloatRate>
            <xsl:value-of select="'PayFloatRate'"/>        
          </PayFloatRate>
          
          <PayCusip>
            <xsl:value-of select="'PayCusip'"/>            
          </PayCusip>	
          
          <PayIsin>
            <xsl:value-of select="'PayIsin'"/>         
          </PayIsin>	
          
          <PaySedol>
            <xsl:value-of select="'PaySedol'"/>         
          </PaySedol>
          
          <PayBloomberg>
            <xsl:value-of select="'PayBloomberg'"/>        
          </PayBloomberg>
          
          <PayRIC>
            <xsl:value-of select="'PayRIC'"/>           
          </PayRIC>	
          
          <PayBasketIndex>
            <xsl:value-of select="'PayBasketIndex'"/>      
          </PayBasketIndex>	
          
          <PayAdditiveMargin>
            <xsl:value-of select="'PayAdditiveMargin'"/>        
          </PayAdditiveMargin>	
          
          <PayFirstCouponDate>
            <xsl:value-of select="'PayFirstCouponDate'"/>         
          </PayFirstCouponDate>
          
          <PayFirstCouponRate>
            <xsl:value-of select="'PayFirstCouponRate'"/>       
          </PayFirstCouponRate>	
          
          <PayFixedRate>
            <xsl:value-of select="'PayFixedRate'"/>    
          </PayFixedRate>	
          
          <PayDaycount>
            <xsl:value-of select="'PayDaycount'"/>         
          </PayDaycount>
          
          <PayFrequency>
            <xsl:value-of select="'PayFrequency'"/>           
          </PayFrequency>	
          
          <PayFixingFrequency>
            <xsl:value-of select="'PayFixingFrequency'"/>     
          </PayFixingFrequency>
          
          <PayPaymentRollConvention>
            <xsl:value-of select="'PayPaymentRollConvention'"/>          
          </PayPaymentRollConvention>	
          
          
          <PayEffectiveDate>
            <xsl:value-of select="'PayEffectiveDate'"/>           
          </PayEffectiveDate>
          
          <PayMaturityDate>
            <xsl:value-of select="'PayMaturityDate'"/>       
          </PayMaturityDate>
          
          <PayNotional>
            <xsl:value-of select="'PayNotional'"/>    
          </PayNotional>	
          
          <PayCurrency>
            <xsl:value-of select="'PayCurrency'"/>       
          </PayCurrency>	
          
          <PayCurrencyPair>
            <xsl:value-of select="'PayCurrencyPair'"/>  
          </PayCurrencyPair>	
          
          <PaySpotRate>
            <xsl:value-of select="'PaySpotRate'"/>         
          </PaySpotRate>	
          
          <PayResetLag>
            <xsl:value-of select="'PayResetLag'"/>   
          </PayResetLag>	
          
          <PayResetInArrears>
            <xsl:value-of select="'PayResetInArrears'"/>   
          </PayResetInArrears>
          
          <PayDiscountCurve>
            <xsl:value-of select="'PayDiscountCurve'"/>         
          </PayDiscountCurve>	
          
          <PaySwapFee>
            <xsl:value-of select="'PaySwapFee'"/>        
          </PaySwapFee>
          
          <PayQuantity>
            <xsl:value-of select="'PayQuantity'"/>    
          </PayQuantity>
          
          <PayPrice>
            <xsl:value-of select="'PayPrice'"/>  
          </PayPrice>	
          
          <CalendarPay>
            <xsl:value-of select="'CalendarPay'"/>      
          </CalendarPay>	
          
          <CalendarReceive>
            <xsl:value-of select="'CalendarReceive'"/>   
          </CalendarReceive>
          
          <EquitySwapType>
            <xsl:value-of select="'EquitySwapType'"/>  
          </EquitySwapType>
          
          <VolatilityStrike>
            <xsl:value-of select="'VolatilityStrike'"/>  
          </VolatilityStrike>
          
          <ReceiveForwardCurve>
            <xsl:value-of select="'ReceiveForwardCurve'"/>     
          </ReceiveForwardCurve>	
          
          <PayForwardCurve>
            <xsl:value-of select="'PayForwardCurve'"/>   
          </PayForwardCurve>
          
          <ReceiveAssetCurve>
            <xsl:value-of select="'ReceiveAssetCurve'"/>  
          </ReceiveAssetCurve>
          
          <PayAssetCurve>
            <xsl:value-of select="'PayAssetCurve'"/>  
          </PayAssetCurve>
          
          <AnnualizationFactor>
            <xsl:value-of select="'AnnualizationFactor'"/>  
          
          </AnnualizationFactor>
          
          <VolatilityCap>
            <xsl:value-of select="'VolatilityCap'"/>  
          
          </VolatilityCap>	
          
          <ClientReference>
            <xsl:value-of select="'ClientReference'"/>  
          
          </ClientReference>	
          
          <ReceiveReturnType>
            <xsl:value-of select="'ReceiveReturnType'"/>  
          
          </ReceiveReturnType>
          
          <PayReturnType>
            <xsl:value-of select="'PayReturnType'"/>  
          
          </PayReturnType>	
          
          <GiveUpBroker>
            <xsl:value-of select="'GiveUpBroker'"/>      
          </GiveUpBroker>	
          
          
           <ClearingFacility>
              <xsl:value-of select="'ClearingFacility'"/>           
          </ClearingFacility>
          
          <CcpTradeRef>
          <xsl:value-of select="'CcpTradeRef'"/>  
          </CcpTradeRef>	
          
          <BlockId>
            <xsl:value-of select="'BlockId'"/>  
          
          </BlockId>
          
          <BlockAmount>
            <xsl:value-of select="'BlockAmount'"/>  
          
          </BlockAmount>
          
          <UpfrontFee>
            <xsl:value-of select="'UpfrontFee'"/>  
          
          </UpfrontFee>	
          
          <UpfrontFeePaydate>
            <xsl:value-of select="'UpfrontFeePaydate'"/>  
          </UpfrontFeePaydate>
          
          <UpFrontFeeComments>
            <xsl:value-of select="'UpFrontFeeComments'"/>  
          </UpFrontFeeComments>	
          
          <UpfrontFeeCurrency>
            <xsl:value-of select="'UpfrontFeeCurrency'"/>         
          </UpfrontFeeCurrency>
          
          <ReceiveEOMA>
            <xsl:value-of select="'ReceiveEOMA'"/>        
          </ReceiveEOMA>
          
          <PayEOMA>
            <xsl:value-of select="'PayEOMA'"/>  
          </PayEOMA>	
          
          <ReceiveIMMPeriod>
            <xsl:value-of select="'ReceiveIMMPeriod'"/>     
          </ReceiveIMMPeriod>
          
          <PayIMMPeriod>
            <xsl:value-of select="'PayIMMPeriod'"/>  
          </PayIMMPeriod>
          
          <ReceiveAdjusted>
          <xsl:value-of select="'ReceiveAdjusted'"/>  
          </ReceiveAdjusted>	
          
          <PayAdjusted>
            <xsl:value-of select="'PayAdjusted'"/>  
          
          </PayAdjusted>
          
          <NettingId>
            <xsl:value-of select="'NettingId'"/>  
          
          </NettingId>	
          
          <ReceiveGlobeOpIdentifier>
            <xsl:value-of select="'ReceiveGlobeOpIdentifier'"/>  
          
          </ReceiveGlobeOpIdentifier>	
          
          <PayGlobeOpIdentifier>
            <xsl:value-of select="'PayGlobeOpIdentifier'"/>  
          
          </PayGlobeOpIdentifier>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
	  
      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='VeriStar Jefferies Swap']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
         
          
          <DealType>
            <xsl:value-of select="'EquitySwapDeal'"/>
          </DealType>

          <DealId>
            <xsl:value-of select="EntityID"/>
          </DealId>

          <xsl:variable name="varTaxlotStateTx">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'Update'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'CANCEL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Action>
            <xsl:value-of select="$varTaxlotStateTx"/>
          </Action>

          <xsl:variable name="PB_NAME" select="''"/>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>

          <Client>
            <xsl:value-of select="'AEADMGM'"/>
          </Client>

         <Reserved1>
            <xsl:value-of select="''"/>
          </Reserved1>
          
          <Reserved2>
              <xsl:value-of select="''"/>
          
          </Reserved2>
          <Folder>
             <xsl:value-of select="'AEAEQTY'"/>
          
          </Folder>

          <Custodian>
            <xsl:value-of select="'JEFF'"/>
          </Custodian>

          <CashAccount>
            <xsl:value-of select="'JFNAECAEPB'"/>
          </CashAccount>
          
          <!--<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
          
          <xsl:variable name="PB_COUNTERPARTY_NAME_A">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
          </xsl:variable>

          <xsl:variable name="varCpty">
            <xsl:choose>
              <xsl:when test="$PB_COUNTERPARTY_NAME_A!=''">
                <xsl:value-of select="$PB_COUNTERPARTY_NAME_A"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CounterParty"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>-->
          <Counterparty>
            <xsl:value-of select="'JEFCNY'"/>
          </Counterparty>

          <Comments>
            <xsl:value-of select="''"/>
          </Comments>

          <State>
            <xsl:value-of select="''"/>
          </State>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>
          
        <Reserved3>
          <xsl:value-of select="''"/>
        </Reserved3>
        
        <Reserved>
          <xsl:value-of select="''"/>
        </Reserved>
       
          
          <InitialMargin>
             <xsl:value-of select="''"/>   
          </InitialMargin>	
          
          <InitialMarginPercentage>
             <xsl:value-of select="''"/>            
          </InitialMarginPercentage>
          
          
          <InitialMarginCurrency>
             <xsl:value-of select="''"/>            
          </InitialMarginCurrency>
          
          <RegenerateCashFlow>
             <xsl:value-of select="''"/>            
          </RegenerateCashFlow>
          
          <ReceiveUnderlyingType>
             <xsl:value-of select="'Equity'"/>                   
          </ReceiveUnderlyingType>
          
          <ReceiveLegType>
             <xsl:value-of select="''"/>                   
          </ReceiveLegType>
          
          <ReceiveFloatRate>
             <xsl:value-of select="''"/>                   
          </ReceiveFloatRate>
          
          <ReceiveCusip>
             <xsl:value-of select="''"/>                
          </ReceiveCusip>
          
          <ReceiveIsin> 
          <xsl:value-of select="''"/>         
          </ReceiveIsin>
          
          <ReceiveSedol>
            <xsl:value-of select="SEDOL"/>  
          
          </ReceiveSedol>	
          
          
          <ReceiveBloomberg>
            <xsl:value-of select="''"/>  
          
          </ReceiveBloomberg>
          
          <ReceiveRIC>
            <xsl:value-of select="''"/>            
         </ReceiveRIC>
          
          <ReceiveBasketIndex>
            <xsl:value-of select="''"/>           
          </ReceiveBasketIndex>
          
          <ReceiveAdditiveMargin>
            <xsl:value-of select="''"/>          
          </ReceiveAdditiveMargin>
          
          <ReceiveFirstCouponDate>
            <xsl:value-of select="''"/>           
          </ReceiveFirstCouponDate>
          
          <ReceiveFirstCouponRate>
            <xsl:value-of select="''"/>           
          </ReceiveFirstCouponRate>
          
          <ReceiveFixedRate>
            <xsl:value-of select="''"/>            
          </ReceiveFixedRate>
          
          <ReceiveDaycount>
             <xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped= 'true'">
								       <xsl:value-of select="TradeAttribute5"/>     
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>                  
          </ReceiveDaycount>	
          
          <ReceiveFrequency>
            
            <xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped= 'true'">
								     <xsl:value-of select="TradeAttribute1"/>       
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>                       
          </ReceiveFrequency>	
          
          <ReceiveFixingFrequency>
            <xsl:value-of select="''"/>  
          
          </ReceiveFixingFrequency>
          
          <ReceivePaymentRollConvention>
            <xsl:value-of select="''"/>           
          </ReceivePaymentRollConvention>	
          
          <ReceiveEffectiveDate>
            <xsl:value-of select="TradeDate"/>
          </ReceiveEffectiveDate>
          
          <ReceiveMaturityDate>
            <xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped= 'true'">
								    <xsl:value-of select="TradeAttribute2"/>    
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>               
          </ReceiveMaturityDate>
          <xsl:variable name="varReceiveNotional">
            <xsl:choose>
              <xsl:when test="FXConversionMethodOperator_Taxlot='M' and FXRate_Taxlot !=0">
                <xsl:value-of select="NetAmount * FXRate_Taxlot"/>
              </xsl:when>
              <xsl:when test="FXConversionMethodOperator_Taxlot='D' and FXRate_Taxlot !=0">
                <xsl:value-of select="NetAmount div FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <ReceiveNotional>
            <xsl:value-of select="$varReceiveNotional"/>         
          </ReceiveNotional>	
          
          <ReceiveCurrency>
            <xsl:value-of select="SettlCurrency"/>          
          </ReceiveCurrency>	
          
          <ReceiveCurrencyPair>
            <xsl:value-of select="''"/>           
          </ReceiveCurrencyPair>
          
          <ReceiveSpotRate>
            <xsl:value-of select="''"/>          
          </ReceiveSpotRate>
          
          <ReceiveResetLag>
            <xsl:value-of select="''"/>       
          </ReceiveResetLag>
          
          <ReceiveInArrears>
            <xsl:value-of select="''"/>       
          </ReceiveInArrears>
          
          <ReceiveDiscountCurve>
            <xsl:value-of select="''"/>        
          </ReceiveDiscountCurve>
          
          <ReceiveSwapFee>
            <xsl:value-of select="''"/>          
          </ReceiveSwapFee>
          
          <ReceiveQuantity>
            <xsl:value-of select="AllocatedQty"/>          
          </ReceiveQuantity>
          
          <ReceivePrice>
            <xsl:value-of select="$varReceiveNotional div AllocatedQty"/>         
          </ReceivePrice>
          
          <PayUnderlyingType>
            <xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped= 'true'">
								 <xsl:value-of select="TradeAttribute3"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
                    
          </PayUnderlyingType>
          
          <PayLegRateType>
             <xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped= 'true'">
								  <xsl:value-of select="TradeAttribute4"/> 
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>           
          </PayLegRateType>
          
          <PayFloatRate>
            <xsl:value-of select="''"/>        
          </PayFloatRate>
          
          <PayCusip>
            <xsl:value-of select="''"/>            
          </PayCusip>	
          
          <PayIsin>
            <xsl:value-of select="''"/>         
          </PayIsin>	
          
          <PaySedol>
            <xsl:value-of select="''"/>         
          </PaySedol>
          
          <PayBloomberg>
            <xsl:value-of select="''"/>        
          </PayBloomberg>
          
          <PayRIC>
            <xsl:value-of select="''"/>           
          </PayRIC>	
          
          <PayBasketIndex>
            <xsl:value-of select="''"/>      
          </PayBasketIndex>	
          
          <PayAdditiveMargin>
            <xsl:value-of select="''"/>        
          </PayAdditiveMargin>	
          
          <PayFirstCouponDate>
            <xsl:value-of select="''"/>         
          </PayFirstCouponDate>
          
          <PayFirstCouponRate>
            <xsl:value-of select="''"/>       
          </PayFirstCouponRate>	
          
          <PayFixedRate>
             <xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped= 'true'">
								 <xsl:value-of select="TradeAttribute6"/>   
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>	
               
          </PayFixedRate>	
          
          <PayDaycount>         				
							<xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped= 'true'">
									<xsl:value-of select="TradeAttribute5"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>		        
          </PayDaycount>
          
          <PayFrequency>
            <xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped= 'true'">
									<xsl:value-of select="TradeAttribute1"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>	                     
          </PayFrequency>	
          
          <PayFixingFrequency>
            <xsl:value-of select="''"/>     
          </PayFixingFrequency>
          
          <PayPaymentRollConvention>
            <xsl:value-of select="''"/>          
          </PayPaymentRollConvention>	
          
          
          <PayEffectiveDate>
            <xsl:value-of select="TradeDate"/>           
          </PayEffectiveDate>
          
          <PayMaturityDate>
            <xsl:choose>
								<xsl:when test="Asset='Equity' and IsSwapped= 'true'">
								 <xsl:value-of select="TradeAttribute2"/>   
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>	              
          </PayMaturityDate>
          
          <PayNotional>
            <xsl:choose>
              <xsl:when test="FXConversionMethodOperator_Taxlot='M' and FXRate_Taxlot !=0">
                <xsl:value-of select="NetAmount * FXRate_Taxlot"/>
              </xsl:when>
              <xsl:when test="FXConversionMethodOperator_Taxlot='D' and FXRate_Taxlot !=0">
                <xsl:value-of select="NetAmount div FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </PayNotional>	
          
          <PayCurrency>
            <xsl:value-of select="SettlCurrency"/>       
          </PayCurrency>	
          
          <PayCurrencyPair>
            <xsl:value-of select="''"/>  
          </PayCurrencyPair>	
          
          <PaySpotRate>
            <xsl:value-of select="''"/>         
          </PaySpotRate>	
       
          <PayResetLag>
            <xsl:value-of select="''"/>   
          </PayResetLag>	
          
          <PayResetInArrears>
            <xsl:value-of select="''"/>   
          </PayResetInArrears>
          
          <PayDiscountCurve>
            <xsl:value-of select="''"/>         
          </PayDiscountCurve>	
          
          <PaySwapFee>
            <xsl:value-of select="''"/>        
          </PaySwapFee>
          
          <PayQuantity>
            <xsl:value-of select="AllocatedQty"/>    
          </PayQuantity>
          
          <PayPrice>
            <xsl:value-of select="$varReceiveNotional div AllocatedQty"/>  
          </PayPrice>	
          
          <CalendarPay>
            <xsl:value-of select="''"/>      
          </CalendarPay>	
          
          <CalendarReceive>
            <xsl:value-of select="''"/>   
          </CalendarReceive>
          
          <EquitySwapType>
            <xsl:value-of select="'EQITYSWP'"/>  
          </EquitySwapType>
          
          <VolatilityStrike>
            <xsl:value-of select="''"/>  
          </VolatilityStrike>
          
          <ReceiveForwardCurve>
            <xsl:value-of select="''"/>     
          </ReceiveForwardCurve>	
          
          <PayForwardCurve>
            <xsl:value-of select="''"/>   
          </PayForwardCurve>
          
          <ReceiveAssetCurve>
            <xsl:value-of select="''"/>  
          </ReceiveAssetCurve>
          
          <PayAssetCurve>
            <xsl:value-of select="''"/>  
          </PayAssetCurve>
          
          <AnnualizationFactor>
            <xsl:value-of select="''"/>           
          </AnnualizationFactor>
          
          <VolatilityCap>
            <xsl:value-of select="''"/>  
          
          </VolatilityCap>	
          
          <ClientReference>
            <xsl:value-of select="''"/>  
          
          </ClientReference>	
          
          <ReceiveReturnType>
            <xsl:value-of select="''"/>  
          
          </ReceiveReturnType>
          
          <PayReturnType>
            <xsl:value-of select="''"/>          
          </PayReturnType>	
          
          <GiveUpBroker>
            <xsl:value-of select="''"/>      
          </GiveUpBroker>	
          
          
           <ClearingFacility>
              <xsl:value-of select="''"/>           
          </ClearingFacility>
          
          <CcpTradeRef>
          <xsl:value-of select="''"/>  
          </CcpTradeRef>	
          
          <BlockId>
            <xsl:value-of select="''"/>  
          
          </BlockId>
          
          <BlockAmount>
            <xsl:value-of select="''"/>  
          
          </BlockAmount>
          
          <UpfrontFee>
            <xsl:value-of select="''"/>  
          
          </UpfrontFee>	
          
          <UpfrontFeePaydate>
            <xsl:value-of select="''"/>  
          </UpfrontFeePaydate>
          
          <UpFrontFeeComments>
            <xsl:value-of select="''"/>  
          </UpFrontFeeComments>	
          
          <UpfrontFeeCurrency>
            <xsl:value-of select="''"/>         
          </UpfrontFeeCurrency>
          
          <ReceiveEOMA>
            <xsl:value-of select="''"/>        
          </ReceiveEOMA>
          
          <PayEOMA>
            <xsl:value-of select="''"/>  
          </PayEOMA>	
          
          <ReceiveIMMPeriod>
            <xsl:value-of select="''"/>     
          </ReceiveIMMPeriod>
          
          <PayIMMPeriod>
            <xsl:value-of select="''"/>  
          </PayIMMPeriod>
          
          <ReceiveAdjusted>
          <xsl:value-of select="''"/>  
          </ReceiveAdjusted>	
          
          <PayAdjusted>
            <xsl:value-of select="''"/>         
          </PayAdjusted>
          
          <NettingId>
            <xsl:value-of select="''"/>           
          </NettingId>	
          
          <ReceiveGlobeOpIdentifier>
            <xsl:value-of select="''"/>           
          </ReceiveGlobeOpIdentifier>	
          
          <PayGlobeOpIdentifier>
            <xsl:value-of select="''"/>          
          </PayGlobeOpIdentifier>

          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>