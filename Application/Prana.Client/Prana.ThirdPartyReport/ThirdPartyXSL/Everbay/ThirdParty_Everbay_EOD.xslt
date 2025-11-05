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
                <xsl:value-of select ="'N'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'COR'"/>
              </xsl:when>
              <xsl:when test="TaxLotState = 'Deleted'">
                <xsl:value-of select="'C'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'NO'"/>
              </xsl:otherwise>
            </xsl:choose>
          </TradeCancel>

          <Side>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>

              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>

              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>

              <xsl:when test="Side='Buy to Cover'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Side>

          <Quantity>
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="AllocatedQty"/>
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
              
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="CUSIP"/>
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
            <xsl:value-of select="AccountName"/>
          </Account>

          <CommissionType>
            <xsl:value-of select="'1'"/>
          </CommissionType>
          
          <xsl:variable name="varBrokerrate">
            <xsl:value-of select="CommissionCharged div AllocatedQty"/>
          </xsl:variable>
          <CommissionValue>
            <xsl:value-of select="$varBrokerrate"/>
          </CommissionValue>

          <TradePrice>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </TradePrice>        

          <NetMoney>
            <xsl:value-of select="NetAmount"/>
          </NetMoney>

          <ContraBroker>
            <xsl:value-of select="CounterParty"/>
          </ContraBroker>

          <OpenClose>
          <xsl:value-of select="''"/>
          </OpenClose>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>
        


          <TradeCcyUSD>
            <xsl:value-of select="CurrencySymbol"/>
          </TradeCcyUSD>


          <TradeFXRate>
            <xsl:value-of select="FXRate_Taxlot"/>
          </TradeFXRate>

          <AccruedInterest>
            <xsl:value-of select="AccruedInterest"/>
          </AccruedInterest>

          <ClientTrade>
            <xsl:value-of select="EntityID"/>
          </ClientTrade>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>