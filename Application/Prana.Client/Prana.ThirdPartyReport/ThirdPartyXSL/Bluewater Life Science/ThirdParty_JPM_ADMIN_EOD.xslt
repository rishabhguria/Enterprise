<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>


        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>


        <RecordType>
          <xsl:value-of select="'RecordType'"/>
        </RecordType>


      
        <RecordAction>
          <xsl:value-of select="'RecordAction'"/>
        </RecordAction>

        <Portfolio>
          <xsl:value-of select="'Portfolio'"/>
        </Portfolio>

        <LocationAccount>
          <xsl:value-of select="'LocationAccount'"/>
        </LocationAccount>

        <Broker>
          <xsl:value-of select="'Broker'"/>
        </Broker>

        <Strategy>
          <xsl:value-of select="'Strategy'"/>
        </Strategy>

      
        <Investment>
          <xsl:value-of select="'Investment'"/>
        </Investment>

        <EventDate>
          <xsl:value-of select="'EventDate'"/>
        </EventDate>

        <SettleDate>
          <xsl:value-of select="'SettleDate'"/>
        </SettleDate>

        <ActualSettleDate>
          <xsl:value-of select="'ActualSettleDate'"/>
        </ActualSettleDate>

        <UserTranId1>
          <xsl:value-of select="'UserTranId1'"/>
        </UserTranId1>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <NetCounterAmount>
          <xsl:value-of select="'NetCounterAmount'"/>
        </NetCounterAmount>

        <TotCommission>
          <xsl:value-of select="'TotCommission'"/>
        </TotCommission>

        <SecFeeAmount>
          <xsl:value-of select="'SecFeeAmount'"/>
        </SecFeeAmount>

        <PriceDenomination>
          <xsl:value-of select="'PriceDenomination'"/>
        </PriceDenomination>

        <CounterInvestment>
          <xsl:value-of select="'CounterInvestment'"/>
        </CounterInvestment>

        <AccruedInterest>
          <xsl:value-of select="'AccruedInterest'"/>
        </AccruedInterest>

        <tradeFx>
          <xsl:value-of select="'tradeFx'"/>
        </tradeFx>

        <NetInvestmentAmount>
          <xsl:value-of select="'NetInvestmentAmount'"/>
        </NetInvestmentAmount>

        <Assetclass>
          <xsl:value-of select="'Asset class'"/>
        </Assetclass>

        <ExchangeDetails>
          <xsl:value-of select="'Exchange Details'"/>
        </ExchangeDetails>
        
        <RepoClearingDetails>
          <xsl:value-of select="'RepoClearing Details'"/>
        </RepoClearingDetails>

        <FXDetails>
          <xsl:value-of select="'FX Details'"/>
        </FXDetails>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      
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
            <xsl:value-of select="'JPM'"/>
          </Portfolio>

          <LocationAccount>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'JPMSE31740293'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="'JPMSE10200196'"/>
              </xsl:otherwise>
            </xsl:choose>
          </LocationAccount>

          <Broker>
            <xsl:value-of select="CounterParty"/>
          </Broker>

        
          <Strategy>
            <xsl:value-of select="''"/>
          </Strategy>
          
          <xsl:variable name="varSymbol">
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true' and SEDOL !=''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>

              <xsl:when test="Asset='Equity' and SEDOL !=''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>

              <xsl:when test="Symbol !=''">
                <xsl:value-of select="Symbol"/>
              </xsl:when>              
              <!--<xsl:when test="SEDOL !=''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>-->
              
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
            <xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/>
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
          
          <Assetclass>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'TRS'"/>
              </xsl:when>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'Equity'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Assetclass>

          <ExchangeDetails>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'OTC'"/>
              </xsl:when>
             
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ExchangeDetails>

          <RepoClearingDetails>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'Bilateral'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </RepoClearingDetails>

          <FXDetails>
            <xsl:value-of select="''"/>
          </FXDetails>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
