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

      
      
      <xsl:for-each select="ThirdPartyFlatFileDetail [CounterParty = 'GSCO']">
        <ThirdPartyFlatFileDetail>
          
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


        <Side>
         <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:choose>
                  <xsl:when test="Side='Buy to Open'">
                    <xsl:value-of select="'B'"/>
                  </xsl:when>
                  <xsl:when test="Side='sell to Open' or Side='Sell to Open'">
                    <xsl:value-of select="'SS'"/>
                  </xsl:when>
                  <xsl:when test="Side='sell to Close' or Side='Sell to Close'">
                    <xsl:value-of select="'S'"/>
                  </xsl:when>
                  <xsl:when test="Side='buy to Close' or Side='Buy to Close'">
                    <xsl:value-of select="'BTC'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
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
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'BTC'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
        </Side>
        <Ticker>
         <xsl:value-of select ="Symbol"/>
        </Ticker>	
        <CUSIP>
         <xsl:choose>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
        </CUSIP>
          
        <RIC>
          <xsl:value-of select="''"/>
        </RIC>
          
        <BBCode>
          <xsl:value-of select="BBCode"/>
        </BBCode>
        <ISIN>
         <xsl:choose>
              <xsl:when test="ISIN != ''">
                <xsl:value-of select="ISIN"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
        </ISIN>
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
        <OrderID>
          <xsl:value-of select="EntityID"/>
        </OrderID>
        <OrderQuantity>
         <xsl:value-of select="AllocatedQty"/>
        </OrderQuantity>
          
        <TradeDate>
          <xsl:value-of select="TradeDate"/>
        </TradeDate>
          
        <SettlementDate>
          <xsl:value-of select="SettlementDate"/>
        </SettlementDate>
          
        <ExecutionPrice>
          <xsl:value-of select="AveragePrice"/>
        </ExecutionPrice>
          
        <ExecutingBrokerCode>
          <xsl:value-of select="CounterParty"/>
        </ExecutingBrokerCode>
          
          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountNo"/>
          </xsl:variable>

          <!--<xsl:variable name = "PB_NAME">
            <xsl:value-of select="'BTIG'"/>
          </xsl:variable>-->

          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>
        <Account>
           <xsl:choose>
              <xsl:when test ="$PRANA_FUND_NAME!=''">
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:otherwise>

            </xsl:choose>
        </Account>
          
          <xsl:variable name="varCommission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>
        <TradeCommission>
            <xsl:value-of select="$varCommission"/>
        </TradeCommission>
          
          
         
        <SecFees>
          <xsl:value-of select="SecFees"/>
        </SecFees>
          
        <OtherFees>
          <xsl:value-of select="OtherBrokerFee"/>
        </OtherFees>
          
         <xsl:variable name="varStrikePrice">
            <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="StrikePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'0'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <StrikePrice>
            <xsl:value-of select="$varStrikePrice"/>
          </StrikePrice>
          
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
          <xsl:value-of select="ExpirationDate"/>
        </ExpirationDate>
          
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
        <PutOrCall>
         <xsl:value-of select="$varPutCall"/>
        </PutOrCall>
          
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
        <UnderlyingSymbol>
           <xsl:value-of select="UnderlyingSymbol"/>
        </UnderlyingSymbol>
          
        <Exchange>
          <xsl:value-of select="Exchange"/>
        </Exchange>
          
            
        <TradedCurrency>
          <xsl:value-of select="CurrencySymbol"/>
        </TradedCurrency>

          <xsl:variable name="varBroker">
            <xsl:choose>
              <xsl:when test="AccountName='WF ACK I 2MA00122'">
                <xsl:value-of select ="'WFPB'"/>
              </xsl:when>
              <xsl:when test="AccountName='WF ACK II 2MA00123'">
                <xsl:value-of select ="'WFPB'"/>
              </xsl:when>
              <xsl:when test="AccountName='GS ACK I 065442956'">
                <xsl:value-of select ="'GSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='GS ACK I 066135344'">
                <xsl:value-of select ="'GSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='GS ACK II 065442758'">
                <xsl:value-of select ="'GSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='GS ACK II 066135351'">
                <xsl:value-of select ="'GSCO'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
        <Custodian>
         <xsl:value-of select="$varBroker"/>
        </Custodian>

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

