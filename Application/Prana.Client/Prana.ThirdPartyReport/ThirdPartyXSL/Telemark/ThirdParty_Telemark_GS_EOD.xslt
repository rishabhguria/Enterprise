<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

         
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
          
          <OrderNumber>
            <xsl:value-of select="EntityID"/>
          </OrderNumber>

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
          <Cancelcorrectindicator>            
                <xsl:value-of select="$varAllocationState"/>              
          </Cancelcorrectindicator>

          <Accountnumberoracronym>
            <xsl:value-of select ="AccountNo"/>
          </Accountnumberoracronym>

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
          <SecurityIdentifier>
            <xsl:value-of select ="$varSymbol"/>
          </SecurityIdentifier>

          <Broker>
            <xsl:value-of select ="CounterParty"/>
          </Broker>

          <Custodian>
            <xsl:value-of select="''"/>
          </Custodian>

          <xsl:variable name="varTransactionType">
            <xsl:choose>
              
              <xsl:when test="Side = 'Sell to Open'">
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
          <TransactionType>
            <xsl:value-of select="$varTransactionType"/>
          </TransactionType>

          <CurrencyCode>
            <xsl:value-of select="SettlCurrency"/>
          </CurrencyCode>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

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
          <Commission>
            <xsl:value-of select="format-number($VarComm,'#.00')"/>
          </Commission>

          <xsl:variable name="varSettFxAmt">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="FXRate_Taxlot !=0">
                        <xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="AveragePrice"/>
                      </xsl:otherwise>
                    </xsl:choose>                   
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
                <xsl:value-of select="format-number(AveragePrice,'#.0000')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="format-number($varSettFxAmt,'#.0000')"/>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:variable>
          <Price>
            <xsl:value-of select="$varPrice"/>
          </Price>

          <AccruedInterest>
            <xsl:value-of select="format-number(AccruedInterest,'#.00')"/>
          </AccruedInterest>
          
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
          <TradeTax>
            <xsl:value-of select="$varTradeTax"/>
          </TradeTax>

          <MiscMoney>
            <xsl:value-of select="MiscFees"/>
          </MiscMoney>

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
          <NetAmount>
            <xsl:value-of select="$varNetAmount"/>
          </NetAmount>

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
          <Principal>
            <xsl:value-of select="$varGrossAmount"/>
          </Principal>

          <Description>
            <xsl:value-of select="''"/>
          </Description>
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
          <SecurityType>
            <xsl:value-of select="$varSecurityName"/>
          </SecurityType>

          <CountrySettlementCode>
            <xsl:value-of select="''"/>
          </CountrySettlementCode>

          <ClearingAgent>
            <xsl:value-of select="''"/>
          </ClearingAgent>

          <SECFee>
            <xsl:value-of select="''"/>
          </SECFee>


          <OptionUnderlyer>
            <xsl:value-of select="''"/>
          </OptionUnderlyer>

          <OptionExpiryDate>
            <xsl:value-of select="''"/>
          </OptionExpiryDate>


          <OptionCallPutIndicator>
            <xsl:value-of select="''"/>
          </OptionCallPutIndicator>


          <OptionStrikePrice>
            <xsl:value-of select="''"/>
          </OptionStrikePrice>


          <Trailer>
            <xsl:value-of select="''"/>
          </Trailer>

          <GenevaLotNumber1>
            <xsl:value-of select="''"/>
          </GenevaLotNumber1>

          <GainsKeeperLotNumber1>
            <xsl:value-of select="''"/>
          </GainsKeeperLotNumber1>

          <LotDate1>
            <xsl:value-of select="''"/>
          </LotDate1>

          <LotQty1>
            <xsl:value-of select="''"/>
          </LotQty1>

          <LotPrice1>
            <xsl:value-of select="''"/>
          </LotPrice1>

          <GenevaLotNumber2>
            <xsl:value-of select="''"/>
          </GenevaLotNumber2>

          <GainsKeeperLotNumber2>
            <xsl:value-of select="''"/>
          </GainsKeeperLotNumber2>

          <LotDate2>
            <xsl:value-of select="''"/>
          </LotDate2>

          <LotQty2>
            <xsl:value-of select="''"/>
          </LotQty2>

          <LotPrice2>
            <xsl:value-of select="''"/>
          </LotPrice2>

          <GenevaLotNumber3>
            <xsl:value-of select="''"/>
          </GenevaLotNumber3>

          <GainsKeeperLotNumber3>
            <xsl:value-of select="''"/>
          </GainsKeeperLotNumber3>

          <LotDate3>
            <xsl:value-of select="''"/>
          </LotDate3>

          <LotQty3>
            <xsl:value-of select="''"/>
          </LotQty3>

          <LotPrice3>
            <xsl:value-of select="''"/>
          </LotPrice3>

          <GenevaLotNumber4>
            <xsl:value-of select="''"/>
          </GenevaLotNumber4>

          <GainsKeeperLotNumber4>
            <xsl:value-of select="''"/>
          </GainsKeeperLotNumber4>

          <LotDate4>
            <xsl:value-of select="''"/>
          </LotDate4>

          <LotQty4>
            <xsl:value-of select="''"/>
          </LotQty4>

          <LotPrice4>
            <xsl:value-of select="''"/>
          </LotPrice4>

          <GenevaLotNumber5>
            <xsl:value-of select="''"/>
          </GenevaLotNumber5>

          <GainsKeeperLotNumber5>
            <xsl:value-of select="''"/>
          </GainsKeeperLotNumber5>

          <LotDate5>
            <xsl:value-of select="''"/>
          </LotDate5>

          <LotQty5>
            <xsl:value-of select="''"/>
          </LotQty5>

          <LotPrice5>
            <xsl:value-of select="''"/>
          </LotPrice5>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
