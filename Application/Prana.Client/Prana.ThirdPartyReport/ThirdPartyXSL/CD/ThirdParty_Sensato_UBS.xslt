<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>

          <!-- System internal use only-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <OriginalTransactionType>
            <xsl:value-of select ="'SWAP'"/>
          </OriginalTransactionType>

          <Account>
            <xsl:value-of select ="AccountMappedName"/>
          </Account>

          <Trade_Reference>
            <xsl:value-of select="TradeRefID"/>
          </Trade_Reference>
          <xsl:variable name = "varTradeMth" >
            <xsl:value-of select="substring(TradeDate,1,2)"/>
          </xsl:variable>
          <xsl:variable name = "varTradeDay" >
            <xsl:value-of select="substring(TradeDate,4,2)"/>
          </xsl:variable>
          <xsl:variable name = "varTradeYR" >
            <xsl:value-of select="substring(TradeDate,7,4)"/>
          </xsl:variable>
          <TradeDate>
            <xsl:value-of select="concat($varTradeYR,'',$varTradeMth,'',$varTradeDay)"/>
          </TradeDate>

          <xsl:variable name = "varSettleMth" >
            <xsl:value-of select="substring(SettlementDate,1,2)"/>
          </xsl:variable>
          <xsl:variable name = "varSettleDay" >
            <xsl:value-of select="substring(SettlementDate,4,2)"/>
          </xsl:variable>
          <xsl:variable name = "varSettleYR" >
            <xsl:value-of select="substring(SettlementDate,7,4)"/>
          </xsl:variable>
          <SettlementDate>
            <xsl:value-of select="concat($varSettleYR,'',$varSettleMth,'',$varSettleDay)"/>
          </SettlementDate>

          <!--   Side     -->

          <xsl:choose>
            <xsl:when test ="TaxLotState='Amemded'">
              <xsl:choose>
                <xsl:when test="Side='Buy' or Side='Buy to Open'">
                  <ActionCode>
                    <xsl:value-of select="'AB'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
                  <ActionCode>
                    <xsl:value-of select="'ABC'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:when test="Side='Sell' or Side='Sell to Close'">
                  <ActionCode>
                    <xsl:value-of select="'AS'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                  <ActionCode>
                    <xsl:value-of select="'ASHS'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:otherwise>
                  <ActionCode>
                    <xsl:value-of select="''"/>
                  </ActionCode>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:when test ="TaxLotState='Deleted'">
              <xsl:choose>
                <xsl:when test="Side='Buy' or Side='Buy to Open'">
                  <ActionCode>
                    <xsl:value-of select="'XB'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
                  <ActionCode>
                    <xsl:value-of select="'XBC'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:when test="Side='Sell' or Side='Sell to Close'">
                  <ActionCode>
                    <xsl:value-of select="'XS'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                  <ActionCode>
                    <xsl:value-of select="'XSS'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:otherwise>
                  <ActionCode>
                    <xsl:value-of select="''"/>
                  </ActionCode>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>
              <xsl:choose>
                <xsl:when test="Side='Buy' or Side='Buy to Open'">
                  <ActionCode>
                    <xsl:value-of select="'BUY'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
                  <ActionCode>
                    <xsl:value-of select="'BC'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:when test="Side='Sell' or Side='Sell to Close'">
                  <ActionCode>
                    <xsl:value-of select="'SELL'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                  <ActionCode>
                    <xsl:value-of select="'SS'"/>
                  </ActionCode>
                </xsl:when>
                <xsl:otherwise>
                  <ActionCode>
                    <xsl:value-of select="''"/>
                  </ActionCode>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:otherwise>
          </xsl:choose>



          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <!--<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
							<NetAmmount>
								<xsl:value-of select="NetAmount - (StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions)"/>
							</NetAmmount>
						</xsl:when>
						<xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
							<NetAmmount>
								<xsl:value-of select="NetAmount + (StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions)"/>
							</NetAmmount>
						</xsl:when>
						<xsl:otherwise>
							<NetAmmount>
								<xsl:value-of select="NetAmount"/>
							</NetAmmount>
						</xsl:otherwise>
					</xsl:choose>-->

          <NetAmmount>
            <xsl:value-of select="NetAmount"/>
          </NetAmmount>

          <SettlementCcy>
            <xsl:value-of select="'USD'"/>
          </SettlementCcy>

          <xsl:choose>
            <xsl:when test="Asset = 'Equity'">
              <SecurityID>
                <xsl:value-of select ="SEDOL"/>
              </SecurityID>
            </xsl:when>
            <xsl:when test ="Asset = 'EquityOption'">
              <!--<xsl:variable name ="varSymbolBef" select ="substring-before(Symbol,' ')"/>
							<xsl:variable name ="varSymbolAft" select ="substring-after(Symbol,' ')"/>-->
              <SecurityID>
                <!--<xsl:value-of select ="concat($varSymbolBef,'+',$varSymbolAft)"/>-->
                <xsl:value-of select ="OSIOptionSymbol"/>
              </SecurityID>
            </xsl:when>
            <xsl:otherwise>
              <SecurityID>
                <xsl:value-of select ="SEDOL"/>
              </SecurityID>
            </xsl:otherwise>
          </xsl:choose>

          <!--<xsl:choose>
						<xsl:when test ="CounterParty='CUTTONE' or CounterParty='CUTN'">
							<ExecBrokerCode>
								<xsl:value-of select="'CUTE'"/>
							</ExecBrokerCode>
						</xsl:when>
						<xsl:otherwise>
							<ExecBrokerCode>
								<xsl:value-of select="CounterParty"/>
							</ExecBrokerCode>
						</xsl:otherwise>
					</xsl:choose>-->

		<xsl:choose>
			<xsl:when test ="CounterParty='GSPrg' or CounterParty='GSElec'">
				<ExecBrokerCode>
					<xsl:value-of select="'GSCO'"/>
				</ExecBrokerCode>
			</xsl:when>
			<xsl:when test ="CounterParty='UBS Program' or CounterParty='UBS Electronic'">
				<ExecBrokerCode>
					<xsl:value-of select="'UBSW'"/>
				</ExecBrokerCode>
			</xsl:when>
			<xsl:when test ="CounterParty='DBPrg' or CounterParty='DBElec'">
				<ExecBrokerCode>
					<xsl:value-of select="'DBAB'"/>
				</ExecBrokerCode>
			</xsl:when>
			<xsl:when test ="CounterParty='INSTElec' or CounterParty='DBElec'">
				<ExecBrokerCode>
					<xsl:value-of select="'INST'"/>
				</ExecBrokerCode>
			</xsl:when>
			<xsl:otherwise >
				<ExecBrokerCode>
					<xsl:value-of select="CounterParty"/>
				</ExecBrokerCode>
			</xsl:otherwise>
		</xsl:choose >
					
		  <Commission>
            <xsl:value-of select="CommissionCharged"/>
          </Commission>

          <BuyCurrency>
            <xsl:value-of select ="''"/>
          </BuyCurrency>


          <SellCurrency>
            <xsl:value-of select ="''"/>
          </SellCurrency>

          <BuyAmount>
            <xsl:value-of select ="''"/>
          </BuyAmount>

          <SellAmount>
            <xsl:value-of select ="''"/>
          </SellAmount>

          <Rate>
            <xsl:value-of select ="''"/>
          </Rate>


          <!--<xsl:choose>
            <xsl:when test ="TaxLotState='Amemded'">
              <OriginalTradeReferenceID>
                <xsl:value-of select="TradeRefID"/>
              </OriginalTradeReferenceID>
            </xsl:when>
            <xsl:otherwise>
              <OriginalTradeReferenceID>
                <xsl:value-of select="''"/>
              </OriginalTradeReferenceID>
            </xsl:otherwise>
          </xsl:choose>-->

          <!-- System internal use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
