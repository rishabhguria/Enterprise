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
  <xsl:template name="formatdate">
    <xsl:param name="DateTimeStr" />

    <xsl:variable name="datestr">
      <xsl:value-of select="substring-before($DateTimeStr,'T')" />
    </xsl:variable>

    <xsl:variable name="mm">
      <xsl:value-of select="substring($datestr,6,2)" />
    </xsl:variable>

    <xsl:variable name="dd">
      <xsl:value-of select="substring($datestr,9,2)" />
    </xsl:variable>

    <xsl:variable name="yyyy">
      <xsl:value-of select="substring($datestr,1,4)" />
    </xsl:variable>

    <xsl:value-of select="concat($yyyy,$mm, $dd)" />
  </xsl:template>
  
<xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before($Date,'-'),substring-before(substring-after($Date,'-'),'-'),substring-before(substring-after(substring-after($Date,'-'),'-'),'T'))"/>
  </xsl:template>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
 <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <Account>
            <xsl:value-of select ="'F1163'"/>
          </Account>

          <TransactionType>
            <xsl:value-of select ="'F55'"/>
          </TransactionType>

          <SecurityID>
            <xsl:value-of select ="'F14'"/>
          </SecurityID>

          <SecurityIDType>
            <xsl:value-of select ="'F1432'"/>
          </SecurityIDType>

          <InvestmentType>
            <xsl:value-of select ="'F11'"/>
          </InvestmentType>

          <LongShortIndicator>
            <xsl:value-of select ="'F15'"/>
          </LongShortIndicator>

          <TradeDate>
            <xsl:value-of select ="'F35'"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select ="'F37'"/>
          </SettlementDate>

          <Quantity>
            <xsl:value-of select ="'F40'"/>
          </Quantity>

          <UnitPrice_local>
            <xsl:value-of select ="'F45'"/>
          </UnitPrice_local>

          <CommissionAmountLocal>
            <xsl:value-of select ="'F47'"/>
          </CommissionAmountLocal>

          <Sec_Fee_Local>
            <xsl:value-of select ="'F48'"/>
          </Sec_Fee_Local>

          <Tax_Amount_Local>
            <xsl:value-of select ="'F46'"/>
          </Tax_Amount_Local>

          <Stamp_Duty_Local>
            <xsl:value-of select ="'F51'"/>
          </Stamp_Duty_Local>

          <Other_Fee_Local>
            <xsl:value-of select ="'F3752'"/>
          </Other_Fee_Local>

          <BrokerCode>
            <xsl:value-of select ="'F88'"/>
          </BrokerCode>

          <Traded_interest_local>
            <xsl:value-of select ="'F49'"/>
          </Traded_interest_local>

          <NetAmount_Local>
            <xsl:value-of select ="'F50'"/>
          </NetAmount_Local>

          <FX_Rate>
            <xsl:value-of select ="'F87'"/>
          </FX_Rate>

          <EagleSpecificHardCoded>
            <xsl:value-of select ="'F7000'"/>
          </EagleSpecificHardCoded>

          <OriginalFaceForFISecurities>
            <xsl:value-of select ="'F41'"/>
          </OriginalFaceForFISecurities>

          <ILB_Index_Ratio>
            <xsl:value-of select ="'F4483'"/>
          </ILB_Index_Ratio>

          <FactorForFISecurities>
            <xsl:value-of select ="'F91'"/>
          </FactorForFISecurities>

          <Principal_Amount_Local>
            <xsl:value-of select ="'F165'"/>
          </Principal_Amount_Local>

          <NetAmountBase>
            <xsl:value-of select ="'F478'"/>
          </NetAmountBase>

          <UniqueTradeID>
            <xsl:value-of select ="'F761'"/>
          </UniqueTradeID>

          <Settlement_Currency>
            <xsl:value-of select ="'F63'"/>
          </Settlement_Currency>

          <Asset_Currency>
            <xsl:value-of select ="'F85'"/>
          </Asset_Currency>

          <Auto_Settle_Indicator>
            <xsl:value-of select ="'F58'"/>
          </Auto_Settle_Indicator>

          <UniqueTradeID_CancelAmend>
            <xsl:value-of select ="'F762'"/>
          </UniqueTradeID_CancelAmend>

          <Eagle_Event_ID>
            <xsl:value-of select ="'F25'"/>
          </Eagle_Event_ID>

          <Orig_Face_Amount>
            <xsl:value-of select ="'F1215'"/>
          </Orig_Face_Amount>


          <EntityID>
            <xsl:value-of select="'EntityID'"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail[TaxLotState != 'Sent']">

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
          <xsl:when test ="TaxLotState != 'Amemded'">
            <ThirdPartyFlatFileDetail>

              <TaxLotState>
                <xsl:value-of select ="TaxLotState"/>
              </TaxLotState>

               <Account>
                <xsl:choose>
                  <xsl:when test="AccountID = '1'">
                    <xsl:value-of select="'SOWF'"/>
                  </xsl:when>
				  <xsl:otherwise>
                    <xsl:value-of select="AccountID"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Account>

              <TransactionType>			  
			    <xsl:choose>
                         <xsl:when test ="TaxLotState = 'Deleted'">
                        <xsl:value-of select="'CANCEL'"/>
                      </xsl:when>                     
					   <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="Side='Buy' or Side='Buy to Open'">
                        <xsl:value-of select="'BUY'"/>
                      </xsl:when>
                      <xsl:when test="Side='Sell' or Side='Sell to Close'">
                        <xsl:value-of select="'SELL'"/>
                      </xsl:when>
                      <xsl:when test="Side='Buy to Close'">
                        <xsl:value-of select="'BUYCVR'"/>
                      </xsl:when>
                      <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                        <xsl:value-of select="'SHORTSELL'"/>
                      </xsl:when>
					   <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                    </xsl:choose>
					
                
              </TransactionType>

              <xsl:variable name="varSecurityID">
			   <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                     <xsl:value-of select="Symbol"/>
                  </xsl:when>
				  <xsl:otherwise>
                   <xsl:choose>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>
                  <xsl:when test="CUSIP != ''">
                    <xsl:value-of select="CUSIP"/>
                  </xsl:when>                 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>				
              </xsl:variable>
			  
              <SecurityID>
                <xsl:value-of select="$varSecurityID"/>
              </SecurityID>

              <xsl:variable name="varSecurityIDType">
			  <xsl:choose>
				<xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="'TICKER'"/>
                </xsl:when>                  
                <xsl:otherwise>
                  <xsl:choose>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select="'SEDOL'"/>
                  </xsl:when>
                  <xsl:when test="CUSIP != ''">
                    <xsl:value-of select="'CUSIP'"/>
                  </xsl:when>                 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
               </xsl:otherwise>
            </xsl:choose>               
              </xsl:variable>

              <SecurityIDType>
                <xsl:value-of select ="$varSecurityIDType"/>
              </SecurityIDType>

              <xsl:variable name="varAsset">
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="'OP'"/>
                  </xsl:when>
                  <xsl:when test="Asset='FixedIncome'">
                    <xsl:value-of select="'FI'"/>
                  </xsl:when>
				  <xsl:when test="Asset='Equity'">
                    <xsl:value-of select="'EQ'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
			  
              <InvestmentType>
                <xsl:value-of select ="''"/>
              </InvestmentType>

              <LongShortIndicator>
                <xsl:value-of select ="''"/>
              </LongShortIndicator>
			  
              <xsl:variable name="varYear">
                <xsl:value-of select ="substring-after(substring-after(TradeDate,'/'),'/')"/>
              </xsl:variable>
              <xsl:variable name="varMM">
                <xsl:value-of select ="substring-before(TradeDate,'/')"/>
              </xsl:variable>
              <xsl:variable name="varDD">
                <xsl:value-of select ="substring-before(substring-after(TradeDate,'/'),'/')"/>
              </xsl:variable>
			  
              <TradeDate>
                <xsl:call-template name="formatdate">
                  <xsl:with-param name="DateTimeStr" select="TradeDate"/>
                </xsl:call-template>
              </TradeDate>
			  
              <xsl:variable name="varSYear">
                <xsl:value-of select ="substring-before(substring-before(SettlementDate,'-'),'-')"/>
              </xsl:variable>
              <xsl:variable name="varSMM">
                <xsl:value-of select ="substring-after(substring-before(SettlementDate,'-'),'-')"/>
              </xsl:variable>
              <xsl:variable name="varSDD">
                <xsl:value-of select ="substring-before(substring-after(SettlementDate,'/'),'/')"/>
              </xsl:variable>
			  
              <SettlementDate>
                <xsl:call-template name="formatdate">
                  <xsl:with-param name="DateTimeStr" select="SettlementDate"/>
                </xsl:call-template>
              </SettlementDate>

              <Quantity>
                <xsl:value-of select ="OrderQty"/>
              </Quantity>

              <UnitPrice_local>
                <xsl:value-of select ="AvgPrice"/>
              </UnitPrice_local>

              <xsl:variable name="varTCommission">
                <xsl:value-of select ="SoftCommission + CommissionCharged"/>
              </xsl:variable>
			  
              <CommissionAmountLocal>
                <xsl:value-of select ="''"/>
              </CommissionAmountLocal>

              <Sec_Fee_Local>
                <xsl:value-of select ="''"/>
              </Sec_Fee_Local>

              <Tax_Amount_Local>
                <xsl:value-of select ="''"/>
              </Tax_Amount_Local>

              <Stamp_Duty_Local>
                <xsl:value-of select ="''"/>
              </Stamp_Duty_Local>

              <Other_Fee_Local>
                <xsl:value-of select ="''"/>
              </Other_Fee_Local>

              <BrokerCode>
                <xsl:value-of select ="CounterParty"/>
              </BrokerCode>

              <Traded_interest_local>
                <xsl:value-of select ="''"/>
              </Traded_interest_local>

              <NetAmount_Local>
                <xsl:value-of select ="format-number($varNetamount,'#0.00')"/>
              </NetAmount_Local>

              <FX_Rate>
                <xsl:value-of select ="''"/>
              </FX_Rate>

              <EagleSpecificHardCoded>
                <xsl:value-of select ="'N'"/>
              </EagleSpecificHardCoded>

              <OriginalFaceForFISecurities>
                <xsl:value-of select ="''"/>
              </OriginalFaceForFISecurities>

              <ILB_Index_Ratio>
                <xsl:value-of select ="''"/>
              </ILB_Index_Ratio>

              <FactorForFISecurities>
                <xsl:value-of select ="''"/>
              </FactorForFISecurities>

              <Principal_Amount_Local>
                <xsl:value-of select ="OrderQty * AvgPrice * AssetMultiplier"/>
              </Principal_Amount_Local>

              <NetAmountBase>
                <xsl:value-of select ="''"/>
              </NetAmountBase>

              <UniqueTradeID>
                <xsl:value-of select ="EntityID"/>
              </UniqueTradeID>

              <Settlement_Currency>
                <xsl:value-of select ="''"/>
              </Settlement_Currency>

              <Asset_Currency>
                <xsl:value-of select ="''"/>
              </Asset_Currency>

              <Auto_Settle_Indicator>
                <xsl:value-of select ="''"/>
              </Auto_Settle_Indicator>

              <UniqueTradeID_CancelAmend>
                <xsl:value-of select ="EntityID"/>
              </UniqueTradeID_CancelAmend>

              <Eagle_Event_ID>
                <xsl:value-of select ="''"/>
              </Eagle_Event_ID>

              <Orig_Face_Amount>
                <xsl:value-of select="''"/>
              </Orig_Face_Amount>
			  
              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>
          </xsl:when>
		  
          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">
              <ThirdPartyFlatFileDetail>

		<xsl:variable name="varOldNetAmount">
          <xsl:choose>
            <xsl:when test="contains(OldSide,'Buy')">
              <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
            </xsl:when>
            <xsl:when test="contains(OldSide,'Sell')">
              <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
		
                <TaxLotState>
                  <xsl:value-of select ="TaxLotState"/>
                </TaxLotState>
				
				<Account>
                <xsl:choose>
                  <xsl:when test="AccountID='1'">
                    <xsl:value-of select="'SOWF'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AccountID"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Account>

                <!-- <TransactionType> -->
                 <!-- <xsl:choose> -->
                      <!-- <xsl:when test="OldSide = 'Buy' or OldSide ='Buy to Open'"> -->
                        <!-- <xsl:value-of select="'BUY'"/> -->
                      <!-- </xsl:when> -->
                      <!-- <xsl:when test="OldSide ='Sell' or OldSide ='Sell to Close'"> -->
                        <!-- <xsl:value-of select="'SELL'"/> -->
                      <!-- </xsl:when> -->
                      <!-- <xsl:when test="OldSide ='Buy to Close'"> -->
                        <!-- <xsl:value-of select="'BUYCVR'"/> -->
                      <!-- </xsl:when> -->
                      <!-- <xsl:when test="OldSide='Sell short' or OldSide ='Sell to Open'"> -->
                        <!-- <xsl:value-of select="'SHORTSELL'"/> -->
                      <!-- </xsl:when> -->
                    <!-- </xsl:choose> -->
              <!-- </TransactionType> -->
			  
			  <TransactionType>
				<xsl:value-of select="'CANCEL'"/>
			</TransactionType>

               <xsl:variable name="varSecurityID">
			   <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                     <xsl:value-of select="Symbol"/>
                  </xsl:when>
				  <xsl:otherwise>
                   <xsl:choose>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>
                  <xsl:when test="CUSIP != ''">
                    <xsl:value-of select="CUSIP"/>
                  </xsl:when>                 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>				
              </xsl:variable>
				
                <SecurityID>
                  <xsl:value-of select="$varSecurityID"/>
                </SecurityID>

                <xsl:variable name="varSecurityIDType">
			  <xsl:choose>
				<xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="'TICKER'"/>
                </xsl:when>                  
                <xsl:otherwise>
                  <xsl:choose>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select="'SEDOL'"/>
                  </xsl:when>
                  <xsl:when test="CUSIP != ''">
                    <xsl:value-of select="'CUSIP'"/>
                  </xsl:when>                 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
               </xsl:otherwise>
            </xsl:choose>               
              </xsl:variable>

                <SecurityIDType>
                  <xsl:value-of select ="$varSecurityIDType"/>
                </SecurityIDType>

                <xsl:variable name="varAsset">
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="'OP'"/>
                  </xsl:when>
                  <xsl:when test="Asset='FixedIncome'">
                    <xsl:value-of select="'FI'"/>
                  </xsl:when>
				  <xsl:when test="Asset='Equity'">
                    <xsl:value-of select="'EQ'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
				
                <InvestmentType>
                  <xsl:value-of select ="''"/>
                </InvestmentType>

                <LongShortIndicator>
                  <xsl:value-of select ="''"/>
                </LongShortIndicator>
				
                
				
				<xsl:variable name="varTradeDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldTradeDate"/>
                  </xsl:call-template>
                </xsl:variable>
				
                <TradeDate>
                  <xsl:value-of select ="$varTradeDate"/>
                </TradeDate>
				
                <xsl:variable name="varSettleDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldSettlementDate"/>
                  </xsl:call-template>
                </xsl:variable>
				
                <SettlementDate>
                  <xsl:value-of select ="$varSettleDate"/>
                </SettlementDate>

                <Quantity>
                  <xsl:value-of select ="OldExecutedQuantity"/>
                </Quantity>

                <UnitPrice_local>
                  <xsl:value-of select ="OldAvgPrice"/>
                </UnitPrice_local>

                <xsl:variable name="varTCommission">
                  <xsl:value-of select ="OldSoftCommission + OldCommission"/>
                </xsl:variable>
				
                <CommissionAmountLocal>
                  <xsl:value-of select ="''"/>
                </CommissionAmountLocal>

                <Sec_Fee_Local>
                  <xsl:value-of select ="''"/>
                </Sec_Fee_Local>

                <Tax_Amount_Local>
                  <xsl:value-of select ="''"/>
                </Tax_Amount_Local>

                <Stamp_Duty_Local>
                  <xsl:value-of select ="''"/>
                </Stamp_Duty_Local>

                <Other_Fee_Local>
                  <xsl:value-of select ="''"/>
                </Other_Fee_Local>


                <BrokerCode>
                  <xsl:value-of select ="OldCounterparty"/>
                </BrokerCode>

                <Traded_interest_local>
                  <xsl:value-of select ="''"/>
                </Traded_interest_local>

                <NetAmount_Local>
                  <xsl:value-of select ="format-number($varOldNetAmount,'#0.00')"/>
                </NetAmount_Local>

                <FX_Rate>
                  <xsl:value-of select ="''"/>
                </FX_Rate>

                <EagleSpecificHardCoded>
                  <xsl:value-of select ="'N'"/>
                </EagleSpecificHardCoded>

                <OriginalFaceForFISecurities>
                  <xsl:value-of select ="''"/>
                </OriginalFaceForFISecurities>

                <ILB_Index_Ratio>
                  <xsl:value-of select ="''"/>
                </ILB_Index_Ratio>

                <FactorForFISecurities>
                  <xsl:value-of select ="''"/>
                </FactorForFISecurities>

                <Principal_Amount_Local>
                  <xsl:value-of select ="OldExecutedQuantity * OldAvgPrice * AssetMultiplier"/>
                </Principal_Amount_Local>

                <NetAmountBase>
                  <xsl:value-of select ="''"/>
                </NetAmountBase>

                <UniqueTradeID>
                  <xsl:value-of select ="EntityID"/>
                </UniqueTradeID>

                <Settlement_Currency>
                  <xsl:value-of select ="''"/>
                </Settlement_Currency>

                <Asset_Currency>
                  <xsl:value-of select ="''"/>
                </Asset_Currency>

                <Auto_Settle_Indicator>
                  <xsl:value-of select ="''"/>
                </Auto_Settle_Indicator>

                <UniqueTradeID_CancelAmend>
                  <xsl:value-of select ="EntityID"/>
                </UniqueTradeID_CancelAmend>

                <Eagle_Event_ID>
                  <xsl:value-of select ="''"/>
                </Eagle_Event_ID>
				
                <Orig_Face_Amount>
                  <xsl:value-of select="''"/>
                </Orig_Face_Amount>



                <EntityID>
                  <xsl:value-of select="EntityID"/>
                </EntityID>

              </ThirdPartyFlatFileDetail>
            </xsl:if>
			
            <ThirdPartyFlatFileDetail>

              <TaxLotState>
                <xsl:value-of select ="TaxLotState"/>
              </TaxLotState>

              <Account>
                <xsl:choose>
                  <xsl:when test="AccountID='1'">
                    <xsl:value-of select="'SOWF'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AccountID"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Account>

              <TransactionType>
                 <xsl:choose>
                      <xsl:when test="Side='Buy' or Side='Buy to Open'">
                        <xsl:value-of select="'BUY'"/>
                      </xsl:when>
                      <xsl:when test="Side='Sell' or Side='Sell to Close'">
                        <xsl:value-of select="'SELL'"/>
                      </xsl:when>
                      <xsl:when test="Side='Buy to Close'">
                        <xsl:value-of select="'BUYCVR'"/>
                      </xsl:when>
                      <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                        <xsl:value-of select="'SHORTSELL'"/>
                      </xsl:when>
					   <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                    </xsl:choose>
              </TransactionType>

              <xsl:variable name="varSecurityID">
			   <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                     <xsl:value-of select="Symbol"/>
                  </xsl:when>
				  <xsl:otherwise>
                   <xsl:choose>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>
                  <xsl:when test="CUSIP != ''">
                    <xsl:value-of select="CUSIP"/>
                  </xsl:when>                 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>				
              </xsl:variable>
			  
              <SecurityID>
                <xsl:value-of select="$varSecurityID"/>
              </SecurityID>

               <xsl:variable name="varSecurityIDType">
			  <xsl:choose>
				<xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="'TICKER'"/>
                </xsl:when>                  
                <xsl:otherwise>
                  <xsl:choose>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select="'SEDOL'"/>
                  </xsl:when>
                  <xsl:when test="CUSIP != ''">
                    <xsl:value-of select="'CUSIP'"/>
                  </xsl:when>                 
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
               </xsl:otherwise>
            </xsl:choose>               
              </xsl:variable>

              <SecurityIDType>
                <xsl:value-of select ="$varSecurityIDType"/>
              </SecurityIDType>

              <xsl:variable name="varAsset">
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="'OP'"/>
                  </xsl:when>
                  <xsl:when test="Asset='FixedIncome'">
                    <xsl:value-of select="'FI'"/>
                  </xsl:when>
				  <xsl:when test="Asset='Equity'">
                    <xsl:value-of select="'EQ'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>
			  
              <InvestmentType>
                <xsl:value-of select ="''"/>
              </InvestmentType>

              <LongShortIndicator>
                <xsl:value-of select ="''"/>
              </LongShortIndicator>
			  
              <xsl:variable name="varYear">
                <xsl:value-of select ="substring-after(substring-after(TradeDate,'/'),'/')"/>
              </xsl:variable>
              <xsl:variable name="varMM">
                <xsl:value-of select ="substring-before(TradeDate,'/')"/>
              </xsl:variable>
              <xsl:variable name="varDD">
                <xsl:value-of select ="substring-before(substring-after(TradeDate,'/'),'/')"/>
              </xsl:variable>
			  
              <TradeDate>
                <xsl:call-template name="formatdate">
                  <xsl:with-param name="DateTimeStr" select="TradeDate"/>
                </xsl:call-template>
              </TradeDate>
			  
              <xsl:variable name="varSYear">
                <xsl:value-of select ="substring-before(substring-before(SettlementDate,'-'),'-')"/>
              </xsl:variable>
              <xsl:variable name="varSMM">
                <xsl:value-of select ="substring-after(substring-before(SettlementDate,'-'),'-')"/>
              </xsl:variable>
              <xsl:variable name="varSDD">
                <xsl:value-of select ="substring-before(substring-after(SettlementDate,'/'),'/')"/>
              </xsl:variable>
			  
              <SettlementDate>
                <xsl:call-template name="formatdate">
                  <xsl:with-param name="DateTimeStr" select="SettlementDate"/>
                </xsl:call-template>
              </SettlementDate>

              <Quantity>
                <xsl:value-of select ="OrderQty"/>
              </Quantity>

              <UnitPrice_local>
                <xsl:value-of select ="AvgPrice"/>
              </UnitPrice_local>

              <xsl:variable name="varTCommission">
                <xsl:value-of select ="SoftCommission + CommissionCharged"/>
              </xsl:variable>
              <CommissionAmountLocal>
                <xsl:value-of select ="''"/>
              </CommissionAmountLocal>

              <Sec_Fee_Local>
                <xsl:value-of select ="''"/>
              </Sec_Fee_Local>

              <Tax_Amount_Local>
                <xsl:value-of select ="''"/>
              </Tax_Amount_Local>

              <Stamp_Duty_Local>
                <xsl:value-of select ="''"/>
              </Stamp_Duty_Local>

              <Other_Fee_Local>
                <xsl:value-of select ="''"/>
              </Other_Fee_Local>

              <BrokerCode>
                <xsl:value-of select ="CounterParty"/>
              </BrokerCode>

              <Traded_interest_local>
                <xsl:value-of select ="''"/>
              </Traded_interest_local>

              <NetAmount_Local>
                <xsl:value-of select ="format-number($varNetamount,'#0.00')"/>
              </NetAmount_Local>

              <FX_Rate>
                <xsl:value-of select ="''"/>
              </FX_Rate>

              <EagleSpecificHardCoded>
                <xsl:value-of select ="'N'"/>
              </EagleSpecificHardCoded>

              <OriginalFaceForFISecurities>
                <xsl:value-of select ="''"/>
              </OriginalFaceForFISecurities>

              <ILB_Index_Ratio>
                <xsl:value-of select ="''"/>
              </ILB_Index_Ratio>

              <FactorForFISecurities>
                <xsl:value-of select ="''"/>
              </FactorForFISecurities>

              <Principal_Amount_Local>
                <xsl:value-of select ="OrderQty * AvgPrice * AssetMultiplier"/>
              </Principal_Amount_Local>

              <NetAmountBase>
                <xsl:value-of select ="''"/>
              </NetAmountBase>

              <UniqueTradeID>
                <xsl:value-of select ="concat('N',EntityID)"/>
              </UniqueTradeID>

              <Settlement_Currency>
                <xsl:value-of select ="''"/>
              </Settlement_Currency>

              <Asset_Currency>
                <xsl:value-of select ="''"/>
              </Asset_Currency>

              <Auto_Settle_Indicator>
                <xsl:value-of select ="''"/>
              </Auto_Settle_Indicator>

              <UniqueTradeID_CancelAmend>
                 <xsl:value-of select ="concat('N',EntityID)"/>
              </UniqueTradeID_CancelAmend>

              <Eagle_Event_ID>
                <xsl:value-of select ="''"/>
              </Eagle_Event_ID>

              <Orig_Face_Amount>
                <xsl:value-of select="''"/>
              </Orig_Face_Amount>

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
