<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="false"/>
          </RowHeader>


          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>


          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">
                <xsl:value-of select="'Buy'"/>
              </xsl:when>              
              <xsl:when test="Side='Sell' or Side='Sell to Close'">
                <xsl:value-of select="'Sell'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'SellShort'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'CoverShort'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <RecordType>
            <xsl:value-of select="$varSide"/>
          </RecordType>


          <xsl:variable name="varLifeCycle">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select="'New'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amemded'">
                <xsl:value-of select="'COR'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select="'CAN'"/>
              </xsl:when>
             
              <xsl:otherwise>
                <xsl:value-of select="'SENT'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <RecordAction>
            <xsl:value-of select="$varLifeCycle"/>
          </RecordAction>

          <Portfolio>
            <xsl:value-of select="'HCG'"/>
          </Portfolio>

          <LocationAccount>
            <xsl:value-of select="AccountName"/>
          </LocationAccount>

          <Broker>
            <xsl:value-of select="CounterParty"/>
          </Broker>

          <Strategy>
            <xsl:value-of select="''"/>
          </Strategy>
          
          <xsl:variable name="varSymbol">
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>

              <xsl:when test="Symbol !=''">
                <xsl:value-of select="Symbol"/>
              </xsl:when>              
              <xsl:when test="SEDOL !=''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              <xsl:when test="BBCode != ''">
                <xsl:value-of select="BBCode"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Investment>
            <xsl:value-of select="$varSymbol"/>
          </Investment>

          <EventDate>
            <xsl:value-of select="concat(substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'),'-',substring-after(substring-after(TradeDate,'/'),'/'))"/>
          </EventDate>

          <SettleDate>
            <xsl:value-of select="concat(substring-before(SettlementDate,'/'),'-',substring-before(substring-after(SettlementDate,'/'),'/'),'-',substring-after(substring-after(SettlementDate,'/'),'/'))"/>
          </SettleDate>

          <ActualSettleDate>
            <xsl:value-of select="concat(substring-before(SettlementDate,'/'),'-',substring-before(substring-after(SettlementDate,'/'),'/'),'-',substring-after(substring-after(SettlementDate,'/'),'/'))"/>
          </ActualSettleDate>

          <UserTranId1>
            <xsl:value-of select="''"/>
          </UserTranId1>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <xsl:variable name="varPrencipaleAmt">
            <xsl:value-of select="(AllocatedQty * AveragePrice * AssetMultiplier)"/>
          </xsl:variable>
          <NetCounterAmount>
            <xsl:value-of select="$varPrencipaleAmt"/>
          </NetCounterAmount>
          
          <xsl:variable name="varCommission" select="(CommissionCharged + SoftCommissionCharged)"/>
          <TotCommission>
            <xsl:value-of select="$varCommission"/>
          </TotCommission>

          <SecFeeAmount>
            <xsl:choose>
              <xsl:when test="number(SecFee)">
                <xsl:value-of select="SecFee"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>           
          </SecFeeAmount>

          <PriceDenomination>
            <xsl:value-of select="CurrencySymbol"/>
          </PriceDenomination>

          <CounterInvestment>
            <xsl:value-of select="SettlCurrency"/>
          </CounterInvestment>

          <AccruedInterest>
            <xsl:value-of select="''"/>
          </AccruedInterest>

          <tradeFx>
            <xsl:value-of select="FXRate_Taxlot"/>
          </tradeFx>

          <NetInvestmentAmount>
            <xsl:value-of select="NetAmount"/>
          </NetInvestmentAmount>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
