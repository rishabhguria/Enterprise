<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='VeriStar Jefferies 43001842']">

        <xsl:choose>
          <xsl:when test ="TaxLotState!='Amemded'">
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'false'"/>
              </RowHeader>

              <TaxLotState>
                <xsl:value-of select="TaxLotState"/>
              </TaxLotState>

              <CancelIndicator>
                <xsl:choose>
                  <xsl:when test="TaxLotState='Allocated'">
                    <xsl:value-of select ="''"/>
                  </xsl:when>
                  
                  <xsl:when test="TaxLotState='Amemded'">
                    <xsl:value-of select ="''"/>
                  </xsl:when>

                  <xsl:when test="TaxLotState='Deleted'">
                    <xsl:value-of select ="'X'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CancelIndicator>

              <JefferiesTradeId>
                <xsl:value-of select ="''"/>
              </JefferiesTradeId>

              <ClientTradeId>
                <xsl:value-of select ="''"/>
              </ClientTradeId>

              <Moniker>
                <xsl:value-of select ="'P1125'"/>
              </Moniker>

              <!--Side Identifier-->

              <xsl:choose>
                <xsl:when test="Side='Buy to Open' or Side='Buy' ">
                  <TransactionType>
                    <xsl:value-of select ="'BY'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Buy to Cover' or Side='Buy to Close' ">
                  <TransactionType>
                    <xsl:value-of select ="'CS'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Sell' or Side='Sell to Close' ">
                  <TransactionType>
                    <xsl:value-of select ="'SL'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
                  <TransactionType>
                    <xsl:value-of select ="'SS'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:otherwise>
                  <TransactionType>
                    <xsl:value-of select="Side"/>
                  </TransactionType>
                </xsl:otherwise>
              </xsl:choose>

              <Quantity>
                <xsl:value-of select="CumQty"/>
              </Quantity>

              <xsl:variable name ="varCheckSymbolUnderlying">
                <xsl:value-of select ="substring-before(Symbol,'-')"/>
              </xsl:variable>
              
              <xsl:choose>
                <xsl:when test="$varCheckSymbolUnderlying != '' and SEDOL != '' and Asset != 'FX'">
                  <InstrumentId>
                    <xsl:value-of select="SEDOL"/>
                  </InstrumentId>
                </xsl:when>
                
                <xsl:when test ="Asset ='EquityOption' ">
                  <InstrumentId>
                    <xsl:value-of select="OSIOptionSymbol"/>
                  </InstrumentId>
                </xsl:when>
                
                <xsl:otherwise>
                  <InstrumentId>
                    <xsl:value-of select="Symbol"/>
                  </InstrumentId>
                </xsl:otherwise>
              </xsl:choose>

              <Price>
                <xsl:value-of select="AvgPrice"/>
              </Price>

              <AccountId>
                <xsl:value-of select="FundAccntNo"/>
              </AccountId>

              <xsl:variable name ="varCounterParty">
                <xsl:choose>
                  <xsl:when test="CounterParty ='BOMC'">
                    <xsl:value-of select="'BMOC'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring-before(CounterParty,'-')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <ExecutingBroker>
                <xsl:value-of select ="$varCounterParty"/>
              </ExecutingBroker>
              <!-- <ExecutingBroker> -->

              <!-- <xsl:value-of select ="CounterParty"/> -->
              <!-- </ExecutingBroker> -->

              <TradeDate>
                <xsl:value-of select="substring-before(TradeDate,'T')"/>
              </TradeDate>

              <SettleDate>
                <xsl:value-of select ="substring-before(SettlementDate,'T')"/>
              </SettleDate>

              <CommissionType>
                <xsl:value-of select="'T'"/>
              </CommissionType>

              <!-- only commission and taxes on commission-->
              <Commission>
                <xsl:value-of select="CommissionCharged  + SoftCommissionCharged"/>
              </Commission>

              <SellingMethod>
                <xsl:value-of select ="''"/>
              </SellingMethod>

              <Vs_purchases_Date>
                <xsl:value-of select="''"/>
              </Vs_purchases_Date>

              <SettlementCurrency>
                <xsl:value-of select="SettlCurrency"/>
              </SettlementCurrency>

              <SettlementExchangeRate>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD' and SettlCurrency='USD'">
                    <xsl:value-of select="1"/>
                  </xsl:when>
                  <xsl:when test="SettlCurrency = CurrencySymbol">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="SettlCurrency!= CurrencySymbol and number(FXRate_Taxlot)">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>

                </xsl:choose>
              </SettlementExchangeRate>

              <Exchange>
                <xsl:value-of select="''"/>
              </Exchange>

              <OtherFee>
                <xsl:value-of select="OrfFee + SecFee + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFees + TaxOnCommissions"/>
              </OtherFee>

              <Strategy>
                <xsl:value-of select="''"/>
              </Strategy>

              <LotNumber>
                <xsl:value-of select="0"/>
              </LotNumber>

              <LotQuantity>
                <xsl:value-of select="0"/>
              </LotQuantity>

              <Trader>
                <xsl:value-of select="''"/>
              </Trader>

              <Interest>
                <xsl:value-of select="0"/>
              </Interest>

              <Custodian>
                <xsl:value-of select="''"/>
              </Custodian>

              <WhenIssued>
                <xsl:value-of select="'N'"/>
              </WhenIssued>

              <!-- this is also for internal purpose-->
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

                <TaxLotState>
                  <xsl:value-of select="TaxLotState"/>
                </TaxLotState>

                <CancelIndicator>
                  <xsl:value-of select="'X'"/>
                </CancelIndicator>

                <JefferiesTradeId>
                  <xsl:value-of select ="''"/>
                </JefferiesTradeId>

                <ClientTradeId>
                  <xsl:value-of select ="''"/>
                </ClientTradeId>

                <Moniker>
                  <xsl:value-of select ="'P1125'"/>
                </Moniker>

                <!--Side Identifier-->

                <xsl:choose>
                  <xsl:when test="OldSide='Buy to Open' or OldSide='Buy' ">
                    <TransactionType>
                      <xsl:value-of select ="'BY'"/>
                    </TransactionType>
                  </xsl:when>
                  <xsl:when test="OldSide='Buy to Cover' or OldSide='Buy to Close' ">
                    <TransactionType>
                      <xsl:value-of select ="'CS'"/>
                    </TransactionType>
                  </xsl:when>
                  <xsl:when test="OldSide='Sell' or OldSide='Sell to Close' ">
                    <TransactionType>
                      <xsl:value-of select ="'SL'"/>
                    </TransactionType>
                  </xsl:when>
                  <xsl:when test="OldSide='Sell short' or OldSide='Sell to Open' ">
                    <TransactionType>
                      <xsl:value-of select ="'SS'"/>
                    </TransactionType>
                  </xsl:when>
                  <xsl:otherwise>
                    <TransactionType>
                      <xsl:value-of select="OldSide"/>
                    </TransactionType>
                  </xsl:otherwise>
                </xsl:choose>

                <Quantity>
                  <xsl:value-of select="OldExecutedQuantity"/>
                </Quantity>

                <xsl:variable name ="varCheckSymbolUnderlying">
                  <xsl:value-of select ="substring-before(Symbol,'-')"/>
                </xsl:variable>
                <xsl:choose>
                  <xsl:when test="$varCheckSymbolUnderlying != '' and SEDOL != '' and Asset != 'FX'">
                    <InstrumentId>
                      <xsl:value-of select="SEDOL"/>
                    </InstrumentId>
                  </xsl:when>
                  <xsl:when test ="Asset ='EquityOption' ">
                    <InstrumentId>
                      <xsl:value-of select="OSIOptionSymbol"/>
                    </InstrumentId>

                  </xsl:when>
                  <xsl:otherwise>
                    <InstrumentId>
                      <xsl:value-of select="Symbol"/>
                    </InstrumentId>
                  </xsl:otherwise>
                </xsl:choose>

                <Price>
                  <xsl:value-of select="OldAvgPrice"/>
                </Price>

                <AccountId>
                  <xsl:value-of select="FundAccntNo"/>
                </AccountId>

                <xsl:variable name ="varCounterParty">
                  <xsl:choose>
                    <xsl:when test="OldCounterparty ='BOMC'">
                      <xsl:value-of select="'BMOC'"/>
                    </xsl:when>
                    <xsl:otherwise>
					  <xsl:value-of select="substring-before(OldCounterparty,'-')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <ExecutingBroker>
                  <xsl:value-of select ="$varCounterParty"/>
                </ExecutingBroker>
                <!-- <ExecutingBroker> -->

                <!-- <xsl:value-of select ="CounterParty"/> -->
                <!-- </ExecutingBroker> -->

                <TradeDate>
                  <xsl:value-of select="substring-before(OldTradeDate,'T')"/>
                </TradeDate>

                <SettleDate>
                  <xsl:value-of select ="substring-before(OldSettlementDate,'T')"/>
                </SettleDate>

                <CommissionType>
                  <xsl:value-of select="'T'"/>
                </CommissionType>

                <!-- only commission and taxes on commission-->
                <Commission>
                  <xsl:value-of select="OldCommission  + OldSoftCommission"/>
                </Commission>

                <SellingMethod>
                  <xsl:value-of select ="''"/>
                </SellingMethod>

                <Vs_purchases_Date>
                  <xsl:value-of select="''"/>
                </Vs_purchases_Date>

                <SettlementCurrency>
                  <xsl:value-of select="OldSettlCurrency"/>
                </SettlementCurrency>

                <SettlementExchangeRate>
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol='USD' and OldSettlCurrency='USD'">
                      <xsl:value-of select="1"/>
                    </xsl:when>
                    <xsl:when test="OldSettlCurrency = CurrencySymbol">
                      <xsl:value-of select="''"/>
                    </xsl:when>
                    <xsl:when test="OldSettlCurrency!= CurrencySymbol and number(FXRate_Taxlot)">
                      <xsl:value-of select="FXRate_Taxlot"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </SettlementExchangeRate>

                <Exchange>
                  <xsl:value-of select="''"/>
                </Exchange>

                <OtherFee>
                  <xsl:value-of select="OldOrfFee + OldSecFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldMiscFees + OldOtherBrokerFees + OldTaxOnCommissions"/>
                </OtherFee>

                <Strategy>
                  <xsl:value-of select="''"/>
                </Strategy>

                <LotNumber>
                  <xsl:value-of select="0"/>
                </LotNumber>

                <LotQuantity>
                  <xsl:value-of select="0"/>
                </LotQuantity>

                <Trader>
                  <xsl:value-of select="''"/>
                </Trader>

                <Interest>
                  <xsl:value-of select="0"/>
                </Interest>

                <Custodian>
                  <xsl:value-of select="''"/>
                </Custodian>

                <WhenIssued>
                  <xsl:value-of select="'N'"/>
                </WhenIssued>

                <!-- this is also for internal purpose-->
                <EntityID>
                  <xsl:value-of select="EntityID"/>
                </EntityID>

              </ThirdPartyFlatFileDetail> 
            </xsl:if>

            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'false'"/>
              </RowHeader>

              <TaxLotState>
                <xsl:value-of select="TaxLotState"/>
              </TaxLotState>

              <CancelIndicator>
                <xsl:value-of select="''"/>
              </CancelIndicator>

              <JefferiesTradeId>
                <xsl:value-of select ="''"/>
              </JefferiesTradeId>

              <ClientTradeId>
                <xsl:value-of select ="''"/>
              </ClientTradeId>

              <Moniker>
                <xsl:value-of select ="'P1125'"/>
              </Moniker>

              <!--Side Identifier-->

              <xsl:choose>
                <xsl:when test="Side='Buy to Open' or Side='Buy' ">
                  <TransactionType>
                    <xsl:value-of select ="'BY'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Buy to Cover' or Side='Buy to Close' ">
                  <TransactionType>
                    <xsl:value-of select ="'CS'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Sell' or Side='Sell to Close' ">
                  <TransactionType>
                    <xsl:value-of select ="'SL'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
                  <TransactionType>
                    <xsl:value-of select ="'SS'"/>
                  </TransactionType>
                </xsl:when>
                <xsl:otherwise>
                  <TransactionType>
                    <xsl:value-of select="Side"/>
                  </TransactionType>
                </xsl:otherwise>
              </xsl:choose>

              <Quantity>
                <xsl:value-of select="CumQty"/>
              </Quantity>

              <xsl:variable name ="varCheckSymbolUnderlying">
                <xsl:value-of select ="substring-before(Symbol,'-')"/>
              </xsl:variable>

              <xsl:choose>
                <xsl:when test="$varCheckSymbolUnderlying != '' and SEDOL != '' and Asset != 'FX'">
                  <InstrumentId>
                    <xsl:value-of select="SEDOL"/>
                  </InstrumentId>
                </xsl:when>

                <xsl:when test ="Asset ='EquityOption' ">
                  <InstrumentId>
                    <xsl:value-of select="OSIOptionSymbol"/>
                  </InstrumentId>
                </xsl:when>

                <xsl:otherwise>
                  <InstrumentId>
                    <xsl:value-of select="Symbol"/>
                  </InstrumentId>
                </xsl:otherwise>
              </xsl:choose>

              <Price>
                <xsl:value-of select="AvgPrice"/>
              </Price>

              <AccountId>
                <xsl:value-of select="FundAccntNo"/>
              </AccountId>

              <xsl:variable name ="varCounterParty">
                <xsl:choose>
                  <xsl:when test="CounterParty ='BOMC'">
                    <xsl:value-of select="'BMOC'"/>
                  </xsl:when>
                  <xsl:otherwise>
                        <xsl:value-of select="substring-before(CounterParty,'-')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <ExecutingBroker>
                <xsl:value-of select ="$varCounterParty"/>
              </ExecutingBroker>
              <!-- <ExecutingBroker> -->

              <!-- <xsl:value-of select ="CounterParty"/> -->
              <!-- </ExecutingBroker> -->

              <TradeDate>
                <xsl:value-of select="substring-before(TradeDate,'T')"/>
              </TradeDate>

              <SettleDate>
                <xsl:value-of select ="substring-before(SettlementDate,'T')"/>
              </SettleDate>

              <CommissionType>
                <xsl:value-of select="'T'"/>
              </CommissionType>

              <!-- only commission and taxes on commission-->
              <Commission>
                <xsl:value-of select="CommissionCharged  + SoftCommissionCharged"/>
              </Commission>

              <SellingMethod>
                <xsl:value-of select ="''"/>
              </SellingMethod>

              <Vs_purchases_Date>
                <xsl:value-of select="''"/>
              </Vs_purchases_Date>

              <SettlementCurrency>
                <xsl:value-of select="SettlCurrency"/>
              </SettlementCurrency>

              <SettlementExchangeRate>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD' and SettlCurrency='USD'">
                    <xsl:value-of select="1"/>
                  </xsl:when>
                  <xsl:when test="SettlCurrency = CurrencySymbol">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="SettlCurrency!= CurrencySymbol and number(FXRate_Taxlot)">
                    <xsl:value-of select="FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>

                </xsl:choose>
              </SettlementExchangeRate>

              <Exchange>
                <xsl:value-of select="''"/>
              </Exchange>

              <OtherFee>
                <xsl:value-of select="OrfFee + SecFee + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFees + TaxOnCommissions"/>
              </OtherFee>

              <Strategy>
                <xsl:value-of select="''"/>
              </Strategy>

              <LotNumber>
                <xsl:value-of select="0"/>
              </LotNumber>

              <LotQuantity>
                <xsl:value-of select="0"/>
              </LotQuantity>

              <Trader>
                <xsl:value-of select="''"/>
              </Trader>

              <Interest>
                <xsl:value-of select="0"/>
              </Interest>

              <Custodian>
                <xsl:value-of select="''"/>
              </Custodian>

              <WhenIssued>
                <xsl:value-of select="'N'"/>
              </WhenIssued>

              <!-- this is also for internal purpose-->
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
