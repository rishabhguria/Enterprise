<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template name="noofzeros">
    <xsl:param name="count"/>
    <xsl:if test="$count > 0">
      <xsl:value-of select ="'0'"/>
      <xsl:call-template name="noofzeros">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
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


          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountNo"/>
          </xsl:variable>

          <!--<xsl:variable name = "PB_NAME">
            <xsl:value-of select="'BTIG'"/>
          </xsl:variable>-->

          <!--<xsl:variable name = "Portfolio">
            <xsl:value-of select="AccountNo"/>
          </xsl:variable>-->
          
          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>

          <Portfolio>
            <xsl:choose>
              <xsl:when test ="$PRANA_FUND_NAME!=''">
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:otherwise>

            </xsl:choose>
          </Portfolio>


          <Side>
            <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:choose>
                  <xsl:when test="Side='Buy to open'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='sell to open' or Side='Sell to Open'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:when test="Side='sell to close' or Side='Sell to Close'">
                    <xsl:value-of select="'Short'"/>
                  </xsl:when>
                  <xsl:when test="Side='buy to close' or Side='Buy to Close'">
                    <xsl:value-of select="'Cover'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'Short'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Cover'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </Side>
          <xsl:variable name="varSymbol">
            <xsl:choose>
              <xsl:when test="SEDOL != ''">               
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
             
                <xsl:when test="Asset = 'EquityOption'">
                  <xsl:value-of select="OSIOptionSymbol"/>
                </xsl:when>
             
                <xsl:otherwise>
                  <xsl:value-of select ="Symbol"/>
                </xsl:otherwise>                
            </xsl:choose>
          </xsl:variable>

          <Investment>
            <xsl:value-of select="$varSymbol"/>
          </Investment>

          <Cusip>          
            <xsl:choose>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Cusip>

          <Sedol>
            <xsl:choose>
              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Sedol>

          <xsl:variable name="varTradeDate">
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </xsl:variable>
          <EventDate>
            <xsl:value-of select="$varTradeDate"/>
          </EventDate>

          <xsl:variable name="varSettlementDate">
            <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),'-',substring-before(SettlementDate,'/'),'-',substring-before(substring-after(SettlementDate,'/'),'/'))"/>
          </xsl:variable>

          <SettleDate>
            <xsl:value-of select="$varSettlementDate"/>
          </SettleDate>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <ClosingMethod>
            <xsl:value-of select="''"/>
          </ClosingMethod>

          <TradeFX>
            <xsl:value-of select="FXRate_Taxlot"/>
          </TradeFX>

          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>

          <TradeCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </TradeCurrency>

          <SettleCurrency>
            <xsl:value-of select="SettlCurrency"/>
          </SettleCurrency>

          <xsl:variable name="varCommission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>
          <Commission>
            <xsl:value-of select="$varCommission"/>
          </Commission>

          <CommissionType>
            <xsl:value-of select="'absolute'"/>
          </CommissionType>
          
       
          <Broker>
            <xsl:value-of select="CounterParty"/>
          </Broker>


          <xsl:variable name="varMultiplier">
            <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="AssetMultiplier"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <OptionsMultiplier>
            <xsl:value-of select="$varMultiplier"/>
          </OptionsMultiplier>
          
          <xsl:variable name="varPutCall">
            <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="PutOrCall"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <PutCall>
            <xsl:value-of select="$varPutCall"/>
          </PutCall>

          <xsl:variable name="varExpirationDate">
            <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="ExpirationDate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <ExpirationDate>
            <xsl:value-of select="$varExpirationDate"/>
          </ExpirationDate>

          <xsl:variable name="varStrikePrice">
            <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="StrikePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <StrikePrice>
            <xsl:value-of select="$varStrikePrice"/>
          </StrikePrice>
          
          <xsl:variable name="varUnderlyingSymbol">
            <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="UnderlyingSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Underlyer>
            <xsl:value-of select="$varUnderlyingSymbol"/>
          </Underlyer>

          <xsl:variable name="varTaxlotStateTx">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select="'AMEND'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select="'CANCEL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <State>
            <xsl:value-of select="$varTaxlotStateTx"/>
          </State>
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

