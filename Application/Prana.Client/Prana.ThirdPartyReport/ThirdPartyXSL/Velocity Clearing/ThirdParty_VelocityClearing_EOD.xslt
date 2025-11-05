<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <AccountNo>
          <xsl:value-of select="'Account Number'"/>
        </AccountNo>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select="'SettleDate'"/>
        </SettlementDate>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <SEDOL>
          <xsl:value-of select="'Sedol'"/>
        </SEDOL>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <BuyOrSell>
          <xsl:value-of select="'BuyOrSell'"/>
        </BuyOrSell>

        <OpenClose>
          <xsl:value-of select="'OpenClose Indicator'"/>
        </OpenClose>

        <AllocatedQty>
          <xsl:value-of select="'Quantity'"/>
        </AllocatedQty>

        <AveragePrice>
          <xsl:value-of select="'Avg Price'"/>
        </AveragePrice>

        <CommissionCharged>
          <xsl:value-of select="'Commission'"/>
        </CommissionCharged>

        <SecFees>
          <xsl:value-of select="'sec fees'"/>
        </SecFees>

        <OtherBrokerFee>
          <xsl:value-of select="'other fees'"/>
        </OtherBrokerFee>

        <AccruedInterest>
          <xsl:value-of select="'Accrued Interest'"/>
        </AccruedInterest>

        <NetAmount>
          <xsl:value-of select="'Net Amount'"/>
        </NetAmount>

        <CounterParty>
          <xsl:value-of select="'Broker'"/>
        </CounterParty>

        <UnderlyingSymbol>
          <xsl:value-of select="'Underlier'"/>
        </UnderlyingSymbol>

        <StrikePrice>
          <xsl:value-of select="'Strike'"/>
        </StrikePrice>

        <PutOrCall>
          <xsl:value-of select="'C/P'"/>
        </PutOrCall>

        <ExpirationDate>
          <xsl:value-of select="'Expiry'"/>
        </ExpirationDate>

        <CurrencySymbol>
          <xsl:value-of select="'Trade Currency'"/>
        </CurrencySymbol>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">


        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <AccountNo>
            <xsl:value-of select="AccountNo"/>
          </AccountNo>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <SEDOL>
            <xsl:value-of select="SEDOL"/>
          </SEDOL>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

          <BuyOrSell>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </BuyOrSell>

          <OpenClose>
            <xsl:choose>
              <xsl:when test="Side='Buy to Close' or Side='Sell' or Side='Sell to Close'">
                <xsl:value-of select="'C'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </OpenClose>

          <AllocatedQty>
            <xsl:value-of select="AllocatedQty"/>
          </AllocatedQty>

          <AveragePrice>
            <xsl:value-of select="AveragePrice"/>
          </AveragePrice>

          <CommissionCharged>
            <xsl:value-of select="CommissionCharged"/>
          </CommissionCharged>

          <SecFees>
            <xsl:value-of select="SecFees"/>
          </SecFees>

          <OtherBrokerFee>
            <xsl:value-of select="OtherBrokerFee"/>
          </OtherBrokerFee>

          <AccruedInterest>
            <xsl:value-of select="AccruedInterest"/>
          </AccruedInterest>

          <NetAmount>
            <xsl:value-of select="NetAmount"/>
          </NetAmount>

          <CounterParty>
            <xsl:value-of select="CounterParty"/>
          </CounterParty>

          <UnderlyingSymbol>
            <xsl:value-of select="UnderlyingSymbol"/>
          </UnderlyingSymbol>

          <StrikePrice>
            <xsl:value-of select="StrikePrice"/>
          </StrikePrice>

          <PutOrCall>
            <xsl:value-of select="PutOrCall"/>
          </PutOrCall>

          <ExpirationDate>
            <xsl:value-of select="ExpirationDate"/>
          </ExpirationDate>

          <CurrencySymbol>
            <xsl:value-of select="CurrencySymbol"/>
          </CurrencySymbol>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

</xsl:stylesheet>