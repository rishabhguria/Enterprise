<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="ThirdPartyFlatFileDetail[(contains(AccountName,'JEFF')) and CounterParty!='CorpAction' and CounterParty!='Transfer' and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <!-- this field use internal purpose-->

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

		<CancelIndicator>
           <xsl:choose>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select="'X'"/>
              <!-- </xsl:when> -->
			  <!-- <xsl:when test="TaxLotState='Amended'"> -->
                <!-- <xsl:value-of select="'C'"/> -->
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
            <xsl:value-of select ="TradeRefID"/>
          </ClientTradeId>

          <Moniker>
            <xsl:value-of select ="'P1415'"/>
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
            <xsl:value-of select="AllocatedQty"/>
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
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <AccountId>
            <xsl:value-of select="AccountNo"/>
          </AccountId>

          <ExecutingBroker>
            <xsl:value-of select ="CounterParty"/>
          </ExecutingBroker>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select ="SettlementDate"/>
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
            <xsl:value-of select="OrfFee + SecFee + StampDuty + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee + TaxOnCommissions"/>
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
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
