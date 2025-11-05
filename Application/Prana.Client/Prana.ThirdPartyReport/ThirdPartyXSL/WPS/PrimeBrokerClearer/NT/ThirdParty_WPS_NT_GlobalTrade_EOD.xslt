<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>


          <xsl:variable name="varTaxlotStateTx">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'New'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'Update'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'Cancel'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <NewInputAmendmentCancellation>
            <xsl:value-of select="$varTaxlotStateTx"/>
          </NewInputAmendmentCancellation>

          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Close'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell short'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>

              <xsl:when test="Asset='EquityOption'">
                <xsl:choose>
                  <xsl:when test="Side='Buy to Open'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Open'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Close'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <BuySell>
            <xsl:value-of select="Side"/>
          </BuySell>

          <ClientPortfolioCode>
            <xsl:value-of select="''"/>
          </ClientPortfolioCode>



          <SecurityIdentifier>
            <xsl:value-of select="SEDOL"/>
          </SecurityIdentifier>

          <SecurityDiscription>
            <xsl:value-of select="FullSecurityName"/>
          </SecurityDiscription>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>


          <ExecutingBrokerBIC>
            <xsl:value-of select="CounterParty"/>
          </ExecutingBrokerBIC>
          
          <ClearingBrokerBICAlternativeBrokerID>
            <xsl:value-of select="''"/>
          </ClearingBrokerBICAlternativeBrokerID>

          <FEDThirdPartyBroker>
            <xsl:value-of select="''"/>
          </FEDThirdPartyBroker>
          
          <SpecialInstructions>
            <xsl:value-of select="''"/>
          </SpecialInstructions>


          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>
          
          <OriginalFace>
            <xsl:value-of select="''"/>
          </OriginalFace>

          <xsl:variable name="varSettCurrAmt">
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
                <xsl:value-of select="$varSettCurrAmt"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Price>
            <xsl:value-of select="$varPrice"/>
          </Price>

          <TradeCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </TradeCurrency>

          <PlaceofSettlementPSET>
            <xsl:value-of select="''"/>
          </PlaceofSettlementPSET>

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


          <xsl:variable name="Principal">
            <xsl:value-of select="AllocatedQty * AveragePrice * AssetMultiplier"/>
          </xsl:variable>

          <xsl:variable name="varPRINCIPAL">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$Principal"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$Principal * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:choose>
                  <xsl:when test="$Principal!=0">
                    <xsl:value-of select="$Principal div $varFXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
                
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <GrossTradeAmount>
            <xsl:value-of select="$varPRINCIPAL"/>
          </GrossTradeAmount>


       
          <xsl:variable name="varCommission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

          <xsl:variable name="varCommissionAmount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$varCommission"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$varCommission * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:choose>
                  <xsl:when test="$varCommission!=0">
                    <xsl:value-of select="$varCommission div $varFXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
                
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <BrokerageFee>
            <xsl:value-of select="$varCommissionAmount"/>
          </BrokerageFee>

          <xsl:variable name = "varOthFees">
            <xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee + SecFee)"/>
          </xsl:variable>

          <xsl:variable name="varSubCustodiancharge">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$varOthFees"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$varOthFees * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:choose>
                  <xsl:when test="$varOthFees!=0">
                    <xsl:value-of select="$varOthFees div $varFXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
                
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <SubCustodiancharge>
            <xsl:value-of select="''"/>
          </SubCustodiancharge>

          <AccuredInterest>
            <xsl:value-of select="''"/>
          </AccuredInterest>



          <StampDuty>
            <xsl:value-of select="''"/>
          </StampDuty>

          <Tax>
            <xsl:value-of select="''"/>
          </Tax>

          <MiscCommission>
            <xsl:value-of select="''"/>
          </MiscCommission>

          <ActualSettlementAmountNet>
            <xsl:value-of select="''"/>
          </ActualSettlementAmountNet>

          <ActualSettlementCurrency>
            <xsl:value-of select="''"/>
          </ActualSettlementCurrency>

         
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>