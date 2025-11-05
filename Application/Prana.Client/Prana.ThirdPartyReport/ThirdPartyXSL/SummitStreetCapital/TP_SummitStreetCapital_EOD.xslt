<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <Account>
            <xsl:value-of select="AccountNo"/>
          </Account>
          
          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>
          
          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>
          
          <Transaction>
             <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'BY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'SL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Transaction>
          
          <InstrumentID>
            <xsl:value-of select="''"/>
          </InstrumentID>
          
          <Ticker>
            <xsl:value-of select="Symbol"/>
          </Ticker>
          
          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>
          
          <SEDOL>
            <xsl:value-of select="SEDOL"/>
          </SEDOL>
          
          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>
          
          <AssetType>
            <xsl:value-of select="Asset"/>
          </AssetType>
          
          <SecurityDescription>
            <xsl:value-of select="FullSecurityName"/>
          </SecurityDescription>
          
          <Shares>
            <xsl:value-of select="AllocatedQty"/>
          </Shares>
          
          <AvgPrice>
            <xsl:value-of select="AveragePrice"/>
          </AvgPrice>
          
          <PriceCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </PriceCurrency>
          
          <Broker>
            <xsl:value-of select="CounterParty"/>
          </Broker>
          
          <CommissionType>
            <xsl:value-of select="'T'"/>
          </CommissionType>
          
          <xsl:variable name="varCommission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>
          <Commission>
            <xsl:value-of select="$varCommission"/>
          </Commission>
          
         
          <TotalCommission>
            <xsl:value-of select="$varCommission"/>
          </TotalCommission>
          
          <NetAmount>
            <xsl:value-of select="NetAmount"/>
          </NetAmount>
          
          <OtherFeesAmount>  
            <xsl:value-of select="OtherBrokerFee + OrfFee"/>
          </OtherFeesAmount>
          
          <SECFeeAmount>
            <xsl:value-of select="SecFees"/>
          </SECFeeAmount>
          
          <SettlementCurrency>
            <xsl:value-of select="SettlCurrency"/>
          </SettlementCurrency>
          
          <SettlementExchangeRate>
            <xsl:value-of select="FXRate_Taxlot"/>
          </SettlementExchangeRate>
          
          <TradeId>
		  <xsl:value-of select="PBUniqueID"/>
            <!-- <xsl:value-of select="concat(PBUniqueID, position())"/> -->
          </TradeId>
          
          <AllocationId>
            <xsl:value-of select="''"/>
          </AllocationId>
          
          <Comments>
            <xsl:value-of select="''"/>
          </Comments>
          
          <Interest>
            <xsl:value-of select="''"/>
          </Interest>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>

