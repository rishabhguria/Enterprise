<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>

    <xsl:variable name="varYear">
      <xsl:value-of select="substring-before($Date,'-')"/>
    </xsl:variable>
	 <xsl:variable name="varMonth">
      <xsl:value-of select="substring-before(substring-after($Date,'-'),'-')"/>
    </xsl:variable>
	 <xsl:variable name="varDay">
      <xsl:value-of select="substring-after(substring-after($Date,'-'),'-')"/>
    </xsl:variable>
	<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
	 </xsl:template>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>


        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <TradeCancel>
          <xsl:value-of select="'TradeCancel'"/>
        </TradeCancel>

        <Side>
          <xsl:value-of select="'Side'"/>
        </Side>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Security>
          <xsl:value-of select="'Security'"/>
        </Security>

        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <CommissionType>
          <xsl:value-of select="'CommissionType'"/>
        </CommissionType>

        <CommissionValue>
          <xsl:value-of select="'CommissionValue'"/>
        </CommissionValue>

        <TradePrice>
          <xsl:value-of select="'TradePrice'"/>
        </TradePrice>

        <NetMoney>
          <xsl:value-of select="'NetMoney'"/>
        </NetMoney>

        <ContraBroker>
          <xsl:value-of select="'ContraBroker'"/>
        </ContraBroker>

        <OpenClose>
          <xsl:value-of select="'Open/Close'"/>
        </OpenClose>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'SettleDate'"/>
        </SettleDate>



        <TradeCcyUSD>
          <xsl:value-of select="'TradeCcy(USD)'"/>
        </TradeCcyUSD>


        <TradeFXRate>
          <xsl:value-of select="'TradeFXRate'"/>
        </TradeFXRate>

        <AccruedInterest>
          <xsl:value-of select="'AccruedInterest'"/>
        </AccruedInterest>

        <ClientTrade>
          <xsl:value-of select="'ClientTrade'"/>
        </ClientTrade>


        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      
          <xsl:for-each select="ThirdPartyFlatFileDetail">

            <xsl:choose>
              <xsl:when test ="TaxLotState!='Amemded'">
                <ThirdPartyFlatFileDetail>

                  <RowHeader>
                    <xsl:value-of select ="'false'"/>
                  </RowHeader>

                  <TaxLotState>
                    <xsl:value-of select ="TaxLotState"/>
                  </TaxLotState>

                  <TradeCancel>
                    <xsl:choose>
                      <xsl:when test="TaxLotState='Allocated'">
                        <xsl:value-of select ="'New'"/>
                      </xsl:when>
                      <xsl:when test="TaxLotState='Amemded'">
                        <xsl:value-of select ="'Correct'"/>
                      </xsl:when>

                      <xsl:when test="TaxLotState='Deleted'">
                        <xsl:value-of select ="'Cancel'"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="'New'"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </TradeCancel>

                  <Side>
                    <xsl:choose>
                      <xsl:when test="Side='Buy' or Side='Buy to Open'">
                        <xsl:value-of select="'B'"/>
                      </xsl:when>

                      <xsl:when test="Side='Sell'  or Side ='Sell to Close' or Side='Sell to Open'">
                        <xsl:value-of select="'S'"/>
                      </xsl:when>

                      <xsl:when test="Side='Sell short' ">
                        <xsl:value-of select="'SS'"/>
                      </xsl:when>

                      <xsl:when test="Side='Buy to Close'">
                        <xsl:value-of select="'BC'"/>
                      </xsl:when>



                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Side>


                  <Quantity>
                    <xsl:choose>
                      <xsl:when test="number(CumQty)">
                        <xsl:value-of select="CumQty"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Quantity>

                  <xsl:variable name="varSecurityID">
                    <xsl:choose>
                      <xsl:when test="Asset = 'EquityOption'">
                        <xsl:value-of select="OSIOptionSymbol"/>
                      </xsl:when>

                      <xsl:when test="CurrencySymbol='USD' and CUSIP != '*'">
                        <xsl:value-of select="CUSIP"/>
                      </xsl:when>



                      <xsl:when test="SEDOL != '*'">
                        <xsl:value-of select="SEDOL"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="Symbol"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:variable>

                  <Security>
                    <xsl:value-of select="$varSecurityID"/>
                  </Security>

                  <Account>
				   <xsl:choose>
					<xsl:when test="AccountName = 'TD : 02500225'">
                        <xsl:value-of select="'025-00225'"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="AccountName"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Account>

                  <CommissionType>
                    <xsl:value-of select="'3'"/>
                  </CommissionType>

                  <xsl:variable name="varBrokerrate">
                    <xsl:value-of select="CommissionCharged"/>
                  </xsl:variable>
                  <CommissionValue>
                    <xsl:value-of select="format-number($varBrokerrate,'#.####')"/>
                  </CommissionValue>

                  <TradePrice>
                    <xsl:choose>
                      <xsl:when test="number(AvgPrice)">
                        <xsl:value-of select="format-number(AvgPrice,'#.######')"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </TradePrice>

                  <xsl:variable name="varTotalCommissionAndFees">
                    <xsl:value-of select="CommissionCharged+OtherBrokerFees+StampDuty+TransactionLevy+ClearingFee+TaxOnCommissions+MiscFees+AccruedInterest"/>
                  </xsl:variable>

                  <xsl:variable name="varNetMoney">
                    <xsl:choose>
                      <xsl:when test="contains(Side,'Buy')">
                        <xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier) + $varTotalCommissionAndFees"/>
                      </xsl:when>
                      <xsl:when test="contains(Side,'Sell')">
                        <xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier) - $varTotalCommissionAndFees"/>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:variable>
                  
                  <NetMoney>
                    <xsl:value-of select="format-number(($varNetMoney),'#.00')"/>
                  </NetMoney>

                  <ContraBroker>
                    <xsl:choose>
                      <xsl:when test="CounterParty='TDSU'">
                        <xsl:value-of select="'TDSI'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="CounterParty"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </ContraBroker>

                  <OpenClose>
                    <xsl:choose>


                      <xsl:when test="Side='Sell'  or Side ='Buy to Close' or Side='Sell to Close'">
                        <xsl:value-of select="'F'"/>
                      </xsl:when>


                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </OpenClose>
				 <xsl:variable name="varTradeDate">
						  <xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="substring-before(TradeDate,'T')"/>
						  </xsl:call-template>
						</xsl:variable>
                  <TradeDate>
                    <xsl:value-of select="$varTradeDate"/>
                  </TradeDate>
				<xsl:variable name="varSettlementDate">
						  <xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="substring-before(SettlementDate,'T')"/>
						  </xsl:call-template>
						</xsl:variable>
                  <SettleDate>
                    <xsl:value-of select="$varSettlementDate"/>
                  </SettleDate>



                  <TradeCcyUSD>
                    <xsl:value-of select="CurrencySymbol"/>
                  </TradeCcyUSD>


                  <TradeFXRate>
                    <xsl:value-of select="format-number(FXRate_Taxlot,'#.####')"/>
                  </TradeFXRate>

                  <AccruedInterest>
                    <xsl:choose>
                      <xsl:when test="number(AccruedInterest)">
                        <xsl:value-of select="format-number(AccruedInterest,'#.####')"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="'0'"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </AccruedInterest>



                  <ClientTrade>
                    <xsl:value-of select="EntityID"/>
                  </ClientTrade>

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
                        <xsl:value-of select ="TaxLotState"/>
                      </TaxLotState>

                      <TradeCancel>
                        <xsl:choose>
                          <xsl:when test="TaxLotState='Allocated'">
                            <xsl:value-of select ="'New'"/>
                          </xsl:when>
                          <xsl:when test="TaxLotState='Amemded'">
                            <xsl:value-of select ="'Cancel'"/>
                          </xsl:when>

                          <xsl:when test="TaxLotState='Deleted'">
                            <xsl:value-of select ="'Cancel'"/>
                          </xsl:when>

                          <xsl:otherwise>
                            <xsl:value-of select="'New'"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </TradeCancel>

                      <Side>
                        <xsl:choose>
                          <xsl:when test="OldSide='Buy' or OldSide='Buy to Open'">
                            <xsl:value-of select="'B'"/>
                          </xsl:when>

                          <xsl:when test="OldSide='Sell'  or OldSide ='Sell to Close' or OldSide='Sell to Open'">
                            <xsl:value-of select="'S'"/>
                          </xsl:when>

                          <xsl:when test="OldSide='Sell short' ">
                            <xsl:value-of select="'SS'"/>
                          </xsl:when>

                          <xsl:when test="OldSide='Buy to Close'">
                            <xsl:value-of select="'BC'"/>
                          </xsl:when>



                          <xsl:otherwise>
                            <xsl:value-of select="''"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </Side>

                      <Quantity>
                        <xsl:choose>
                          <xsl:when test="number(OldExecutedQuantity)">
                            <xsl:value-of select="OldExecutedQuantity"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="''"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </Quantity>

                      <xsl:variable name="varSecurityID">
                        <xsl:choose>
                          <xsl:when test="Asset = 'EquityOption'">
                            <xsl:value-of select="OSIOptionSymbol"/>
                          </xsl:when>

                          <xsl:when test="CurrencySymbol='USD' and CUSIP != '*'">
                            <xsl:value-of select="CUSIP"/>
                          </xsl:when>



                          <xsl:when test="SEDOL != '*'">
                            <xsl:value-of select="SEDOL"/>
                          </xsl:when>

                          <xsl:otherwise>
                            <xsl:value-of select="Symbol"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>

                      <Security>
                        <xsl:value-of select="$varSecurityID"/>
                      </Security>

                      <Account>
                        <xsl:choose>
					<xsl:when test="AccountName = 'TD : 02500225'">
                        <xsl:value-of select="'025-00225'"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="AccountName"/>
                      </xsl:otherwise>
                    </xsl:choose>
                      </Account>

                      <CommissionType>
                        <xsl:value-of select="'3'"/>
                      </CommissionType>

                      <xsl:variable name="varBrokerrate">
                        <xsl:value-of select="OldCommission"/>
                      </xsl:variable>
                      <CommissionValue>
                        <xsl:value-of select="format-number($varBrokerrate,'#.####')"/>
                      </CommissionValue>

                      <TradePrice>
                        <xsl:choose>
                          <xsl:when test="number(OldAvgPrice)">
                            <xsl:value-of select="format-number(OldAvgPrice,'#.######')"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="''"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </TradePrice>

                    <xsl:variable name="varTotalCommissionAndFees">
                      <xsl:value-of select="OldCommission+OldOtherBrokerFees+OldStampDuty+OldTransactionLevy+OldClearingFee+OldMiscFees+OldAccruedInterest"/>
                    </xsl:variable>

                    <xsl:variable name="varNetMoney">
                      <xsl:choose>
                        <xsl:when test="contains(Side,'Buy')">
                          <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + $varTotalCommissionAndFees"/>
                        </xsl:when>
                        <xsl:when test="contains(Side,'Sell')">
                          <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - $varTotalCommissionAndFees"/>
                        </xsl:when>
                      </xsl:choose>
                    </xsl:variable>

                    <NetMoney>
                      <xsl:value-of select="format-number(($varNetMoney),'#.00')"/>
                    </NetMoney>

                      <ContraBroker>
                        <xsl:choose>
                          <xsl:when test="OldCounterparty='TDSU'">
                            <xsl:value-of select="'TDSI'"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="OldCounterparty"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </ContraBroker>

                      <OpenClose>
                        <xsl:choose>


                          <xsl:when test="OldSide='Sell'  or OldSide ='Buy to Close' or OldSide='Sell to Close'">
                            <xsl:value-of select="'F'"/>
                          </xsl:when>


                          <xsl:otherwise>
                            <xsl:value-of select="''"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </OpenClose>
						<xsl:variable name="varOldTradeDate">
						  <xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="substring-before(OldTradeDate,'T')"/>
						  </xsl:call-template>
						</xsl:variable>
                      <TradeDate>
                        <xsl:value-of select="$varOldTradeDate"/>
                      </TradeDate>
						<xsl:variable name="varOldSettlementDate">
						  <xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="substring-before(OldSettlementDate,'T')"/>
						  </xsl:call-template>
						</xsl:variable>
                      <SettleDate>
                        <xsl:value-of select="$varOldSettlementDate"/>
                      </SettleDate>



                      <TradeCcyUSD>
                        <xsl:value-of select="CurrencySymbol"/>
                      </TradeCcyUSD>


                      <TradeFXRate>
                        <xsl:value-of select="format-number(OldFXRate,'#.####')"/>
                      </TradeFXRate>

                      <AccruedInterest>
                        <xsl:choose>
                          <xsl:when test="number(OldAccruedInterest)">
                            <xsl:value-of select="format-number(OldAccruedInterest,'#.####')"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="'0'"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </AccruedInterest>



                      <ClientTrade>
                        <xsl:value-of select="EntityID"/>
                      </ClientTrade>

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
                    <xsl:value-of select ="TaxLotState"/>
                  </TaxLotState>

                  <TradeCancel>
                    <xsl:choose>
                      <xsl:when test="TaxLotState='Allocated'">
                        <xsl:value-of select ="'New'"/>
                      </xsl:when>
                      <xsl:when test="TaxLotState='Amemded'">
                        <xsl:value-of select ="'New'"/>
                      </xsl:when>

                      <xsl:when test="TaxLotState='Deleted'">
                        <xsl:value-of select ="'Cancel'"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="'New'"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </TradeCancel>

                  <Side>
                    <xsl:choose>
                      <xsl:when test="Side='Buy' or Side='Buy to Open'">
                        <xsl:value-of select="'B'"/>
                      </xsl:when>

                      <xsl:when test="Side='Sell'  or Side ='Sell to Close' or Side='Sell to Open'">
                        <xsl:value-of select="'S'"/>
                      </xsl:when>

                      <xsl:when test="Side='Sell short' ">
                        <xsl:value-of select="'SS'"/>
                      </xsl:when>

                      <xsl:when test="Side='Buy to Close'">
                        <xsl:value-of select="'BC'"/>
                      </xsl:when>



                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Side>

                  <Quantity>
                    <xsl:choose>
                      <xsl:when test="number(CumQty)">
                        <xsl:value-of select="CumQty"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Quantity>

                  <xsl:variable name="varSecurityID">
                    <xsl:choose>
                      <xsl:when test="Asset = 'EquityOption'">
                        <xsl:value-of select="OSIOptionSymbol"/>
                      </xsl:when>

                      <xsl:when test="CurrencySymbol='USD' and CUSIP != '*'">
                        <xsl:value-of select="CUSIP"/>
                      </xsl:when>



                      <xsl:when test="SEDOL != '*'">
                        <xsl:value-of select="SEDOL"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="Symbol"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:variable>

                  <Security>
                    <xsl:value-of select="$varSecurityID"/>
                  </Security>

                  <Account>
                    <xsl:choose>
					<xsl:when test="AccountName = 'TD : 02500225'">
                        <xsl:value-of select="'025-00225'"/>
                      </xsl:when>

                      <xsl:otherwise>
                        <xsl:value-of select="AccountName"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Account>

                  <CommissionType>
                    <xsl:value-of select="'3'"/>
                  </CommissionType>

                  <xsl:variable name="varBrokerrate">
                    <xsl:value-of select="CommissionCharged"/>
                  </xsl:variable>
                  <CommissionValue>
                    <xsl:value-of select="format-number($varBrokerrate,'#.####')"/>
                  </CommissionValue>

                  <TradePrice>
                    <xsl:choose>
                      <xsl:when test="number(AvgPrice)">
                        <xsl:value-of select="format-number(AvgPrice,'#.######')"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </TradePrice>

                  <xsl:variable name="varTotalCommissionAndFees">
                    <xsl:value-of select="CommissionCharged+OtherBrokerFees+StampDuty+TransactionLevy+ClearingFee+TaxOnCommissions+MiscFees+AccruedInterest"/>
                  </xsl:variable>

                  <xsl:variable name="varNetMoney">
                    <xsl:choose>
                      <xsl:when test="contains(Side,'Buy')">
                        <xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier) + $varTotalCommissionAndFees"/>
                      </xsl:when>
                      <xsl:when test="contains(Side,'Sell')">
                        <xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier) - $varTotalCommissionAndFees"/>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:variable>

                  <NetMoney>
                    <xsl:value-of select="format-number(($varNetMoney),'#.00')"/>
                  </NetMoney>

                  <ContraBroker>
                    <xsl:choose>
                      <xsl:when test="CounterParty='TDSU'">
                        <xsl:value-of select="'TDSI'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="CounterParty"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </ContraBroker>

                  <OpenClose>
                    <xsl:choose>


                      <xsl:when test="Side='Sell'  or Side ='Buy to Close' or Side='Sell to Close'">
                        <xsl:value-of select="'F'"/>
                      </xsl:when>


                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </OpenClose>
				<xsl:variable name="varTradeDate">
						  <xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="substring-before(TradeDate,'T')"/>
						  </xsl:call-template>
						</xsl:variable>
                  <TradeDate>
                    <xsl:value-of select="$varTradeDate"/>
                  </TradeDate>
						<xsl:variable name="varSettlementDate">
						  <xsl:call-template name="DateFormat">
							<xsl:with-param name="Date" select="substring-before(SettlementDate,'T')"/>
						  </xsl:call-template>
						</xsl:variable>
                  <SettleDate>
                    <xsl:value-of select="$varSettlementDate"/>
                  </SettleDate>



                  <TradeCcyUSD>
                    <xsl:value-of select="CurrencySymbol"/>
                  </TradeCcyUSD>


                  <TradeFXRate>
                    <xsl:value-of select="format-number(FXRate_Taxlot,'#.####')"/>
                  </TradeFXRate>

                  <AccruedInterest>
                    <xsl:choose>
                      <xsl:when test="number(AccruedInterest)">
                        <xsl:value-of select="format-number(AccruedInterest,'#.####')"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="'0'"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </AccruedInterest>



                  <ClientTrade>
                    <xsl:value-of select="EntityID"/>
                  </ClientTrade>

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