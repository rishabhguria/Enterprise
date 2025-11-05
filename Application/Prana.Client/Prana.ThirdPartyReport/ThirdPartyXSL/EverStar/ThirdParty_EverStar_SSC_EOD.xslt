<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[(Asset='Equity' or Asset='EquityOption')]">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>


          <DealType>
            <xsl:value-of select="'EquityDeal'"/>
          </DealType>

          <DealId>
            <xsl:value-of select="EntityID"/>
          </DealId>

          <xsl:variable name="varTaxlotStateTx">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'New'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'Updated'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'Cancel'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Action>
            <xsl:value-of select="$varTaxlotStateTx"/>
          </Action>

          <Client>
            <xsl:value-of select="'Everstar Master Fund'"/>
          </Client>

          <Fund>
            <xsl:value-of select="AccountName"/>
          </Fund>
       
          <PortfolioBusinessUnit>
            <xsl:value-of select="''"/>
          </PortfolioBusinessUnit>

          <Strategy>
            <xsl:value-of select="Strategy"/>
          </Strategy>

          <Custodian>
            <xsl:value-of select="CounterParty"/>
          </Custodian>
          
          <CashAccount>
            <xsl:value-of select="AccountNo"/>
          </CashAccount>

          <Counterparty>
            <xsl:value-of select="CounterParty"/>
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

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <Reserved>
            <xsl:value-of select="''"/>
          </Reserved>

          <GlobeOpSecurityIdentifier>
            <xsl:value-of select="CUSIP"/>
          </GlobeOpSecurityIdentifier>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>

          <Sedol>
            <xsl:value-of select="SEDOL"/>
          </Sedol>

          <Bloombergticker>
            <xsl:value-of select="BBCode"/>
          </Bloombergticker>

          <RIC>
            <xsl:value-of select="RIC"/>
          </RIC>

           <SecurityDescription>
            <xsl:value-of select="FullSecurityName"/>
          </SecurityDescription>

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

          <TransactionIndicator>
            <xsl:value-of select="$varSide"/>
          </TransactionIndicator>


          <xsl:variable name="varSubSide">
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'Buy Long'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buy Cover'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'Sell Long'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'Sell Short'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>

              <xsl:when test="Asset='EquityOption'">
                <xsl:choose>
                  <xsl:when test="Side='Buy to Open'">
                    <xsl:value-of select="'Buy Long'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buy Cover'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Open'">
                    <xsl:value-of select="'Sell Long'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Close'">
                    <xsl:value-of select="'Sell Short'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <SubTransaction>
            <xsl:value-of select="$varSubSide"/>
          </SubTransaction>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>
          <xsl:variable name="varCommission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

          <Commission>
            <xsl:value-of select="$varCommission"/>
          </Commission>

          <SECFee>
            <xsl:value-of select="SecFees"/>
          </SECFee>

          <VAT>
            <xsl:value-of select="''"/>
          </VAT>


           <TradeCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </TradeCurrency>

          <ExchangeRate>
            <xsl:value-of select="''"/>
          </ExchangeRate>

          <Reserved1>
            <xsl:value-of select="''"/>
          </Reserved1>

          <BrokerShortName>
            <xsl:value-of select="''"/>
          </BrokerShortName>

          <ClearingMode>
            <xsl:value-of select="''"/>
          </ClearingMode>

          <Exchange>
            <xsl:value-of select="Exchange"/>
          </Exchange>

          <ClientReference>
            <xsl:value-of select="''"/>
          </ClientReference>

          <Reserved2>
            <xsl:value-of select="''"/>
          </Reserved2>

          <SecurityCurrency>
            <xsl:value-of select="''"/>
          </SecurityCurrency>

          <BlockId>
            <xsl:value-of select="''"/>
          </BlockId>

          <BlockAmount>
            <xsl:value-of select="''"/>
          </BlockAmount>

          <ExecutionDateTimeStamp>
            <xsl:value-of select="''"/>
          </ExecutionDateTimeStamp>

          <ClearingFee>
            <xsl:value-of select="AUECFee1"/>
          </ClearingFee>

          <SettlementCurrencyHedge>
            <xsl:value-of select="''"/>
          </SettlementCurrencyHedge>

          <TradeDateFx>
            <xsl:value-of select="FXRate_Taxlot"/>
          </TradeDateFx>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        
      </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>