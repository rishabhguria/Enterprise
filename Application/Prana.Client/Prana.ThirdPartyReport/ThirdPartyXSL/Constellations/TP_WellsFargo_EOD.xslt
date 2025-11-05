<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>


      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>
          
            <WFID>
              <xsl:value-of select="EntityID"/>
            </WFID>
          
            <ClientTradeReference>
              <xsl:value-of select="''"/>
            </ClientTradeReference>
          
          
          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select ="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close'">
                <xsl:value-of select ="'SL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
                <xsl:value-of select ="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close' or Side='Buy to Cover' ">
                <xsl:value-of select ="'BC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
            <TransactionCode>
              <xsl:value-of select="$varSide"/>
            </TransactionCode>

          <xsl:variable name="varOpenCloseIndicatorEquity">
            <xsl:choose>
              <xsl:when test= "Side='Buy' or Side ='Sell short'">
                <xsl:value-of select="'O'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side ='Buy to Close'">
                <xsl:value-of select="'C'"/>
              </xsl:when>
             
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varOpenCloseIndicatorOption">
            <xsl:choose>
              <xsl:when test="Side='Buy to Open' or Side ='Sell to Open'">
                <xsl:value-of select="'OPEN'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close' or Side ='Sell to Close'">
                <xsl:value-of select="'CLOSE'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
            <OpenCloseindicator>
              <xsl:choose>
                <xsl:when test="Asset='Equity'">
                  <xsl:value-of select="$varOpenCloseIndicatorEquity"/>
                </xsl:when>
                <xsl:when test="Asset='EquityOption'">
                  <xsl:value-of select="$varOpenCloseIndicatorEquity"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </OpenCloseindicator>

          <xsl:variable name="varSecurityIDType">
            <xsl:choose>
              <xsl:when test="BBCode!=''">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="CUSIP!=''">
                <xsl:value-of select="'C'"/>
              </xsl:when>
              <xsl:when test="ISIN!=''">
                <xsl:value-of select="'I'"/>
              </xsl:when>
              <xsl:when test="SEDOL!=''">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Symbol!=''">
                <xsl:value-of select="'T'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

             <SecurityIDTypeIndicator>
              <xsl:value-of select="'T'"/>
            </SecurityIDTypeIndicator>
          
            <SecurityIdentifier>
              <xsl:value-of select="Symbol"/>
            </SecurityIdentifier>
          
            <ISIN>
              <xsl:value-of select="''"/>
            </ISIN>
          
            <CUSIP>
              <xsl:value-of select="''"/>
            </CUSIP>
          
            <Ticker>
              <xsl:value-of select="''"/>
            </Ticker>
          
            <Sedol>
              <xsl:value-of select="SEDOL"/>
            </Sedol>
          
            <BBGID>
              <xsl:value-of select="''"/>
            </BBGID>
          
            <SecurityDescription>
              <xsl:value-of select="''"/>
            </SecurityDescription>
          
            <Quantity>
              <xsl:value-of select="AllocatedQty"/>
            </Quantity>
          
            <WhenIssuedIndicator>
              <xsl:value-of select="''"/>
            </WhenIssuedIndicator>
          
          
          <xsl:variable name="Trade_Day" select="substring-before(substring-after(TradeDate,'/'),'/')"/>
          <xsl:variable name="Trade_Month" select="substring-before(TradeDate,'/')"/>
          <xsl:variable name="Trade_Year" select="substring-after(substring-after(TradeDate,'/'),'/')"/>
          
          <TradeDate>
            <xsl:value-of select="concat($Trade_Year,'/',$Trade_Month,'/',$Trade_Day)"/>
          </TradeDate>
          
            <ContractualSettlementDate>
              <xsl:value-of select="''"/>
            </ContractualSettlementDate>


          <xsl:variable name="STrade_Day" select="substring-before(substring-after(SettlementDate,'/'),'/')"/>
          <xsl:variable name="STrade_Month" select="substring-before(SettlementDate,'/')"/>
          <xsl:variable name="STrade_Year" select="substring-after(substring-after(SettlementDate,'/'),'/')"/>

          <SettlementDate>
            <xsl:value-of select="concat($STrade_Year,'/',$STrade_Month,'/',$STrade_Day)"/>
          </SettlementDate>
          <Price>
            <xsl:value-of select="AveragePrice"/>
          </Price>  
         
          
            <CommissionAmount>
              <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'0.###')"/>
            </CommissionAmount>
          
            <CommissionType>
              <xsl:value-of select="'G'"/>
            </CommissionType>
          
          <xsl:variable name="PB_NAME">
            <xsl:value-of select="'WellsFargo'"/>
          </xsl:variable>
          <xsl:variable name="PRANA_COUNTERPARTY">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name="PB_COUNTERPARTY">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name = $PB_NAME]/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@PBBroker"/>
          </xsl:variable>

          <xsl:variable name="varCounterParty">
            <xsl:choose>
              <xsl:when test="$PB_COUNTERPARTY = ''">
                <xsl:value-of select="$PRANA_COUNTERPARTY"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PB_COUNTERPARTY"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
            <Broker>
              <xsl:value-of select="$varCounterParty"/>
            </Broker>
          
            <Market>
              <xsl:value-of select="''"/>
            </Market>
          
            <SettlementLocation>
              <xsl:value-of select="''"/>
            </SettlementLocation>
          
            <TradeCurrency>
              <xsl:value-of select="CurrencySymbol"/>
            </TradeCurrency>
          
            <SettCurrency>
              <xsl:value-of select="SettlCurrency"/>
            </SettCurrency>
          
            <FXRate>
              <xsl:value-of select="FXRate_Taxlot"/>
            </FXRate>


          <xsl:variable name="varTaxlotStateGrp">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'N'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amend'">
                <xsl:value-of select ="'A'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'X'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="'N'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
            <TradeStatus>
              <xsl:value-of select="$varTaxlotStateGrp"/>
            </TradeStatus>
          
            <AccuredInterest>
              <xsl:value-of select="AccruedInterest"/>
            </AccuredInterest>
          
            <Tax>
              <xsl:value-of select="TaxOnCommissions"/>
            </Tax>
          
            <OtherFees>
              <xsl:value-of select="OtherBrokerFee"/>
            </OtherFees>
          
            <SettlementAmountNetmoney>
            <xsl:choose>
              <xsl:when test="CurrencySymbol !='USD'">
                  <xsl:value-of select="NetAmount"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>            
            </SettlementAmountNetmoney>
          
            <CostBasis>
              <xsl:value-of select="AveragePrice"/>
            </CostBasis>
            <PurchaseDate>
              <xsl:value-of select="''"/>
            </PurchaseDate>
          
            <SpecificIDCloseOutIndicatorS>
              <xsl:value-of select="''"/>
            </SpecificIDCloseOutIndicatorS>

          <SpecificIDTaxLotID>
              <xsl:value-of select="LotId"/>
            </SpecificIDTaxLotID>
   
        
          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountMappedName"/>
          </xsl:variable>
          <xsl:variable name ="PB_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PRANA_FUND_NAME]/@PBFundName"/>
          </xsl:variable>
          
            <AccountNumber>
              <xsl:choose>               
                <xsl:when test ="$PB_FUND_NAME != ''">
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountNumber>
          
            <Strategy>
              <xsl:value-of select="''"/>
            </Strategy>
          
            <SECFeeindicator>
              <xsl:value-of select="'Y'"/>
            </SECFeeindicator>
          
            <ORFFeeindicator>
              <xsl:value-of select="''"/>
            </ORFFeeindicator>
          
            <SecFeeAmt>
              <xsl:value-of select="SecFees"/>
            </SecFeeAmt>
          
            <ORFFeeAmt>
              <xsl:value-of select="OrfFee"/>
            </ORFFeeAmt>
          
            <BookFXRate>
              <xsl:value-of select="''"/>
            </BookFXRate>
          
            <BlockID>
              <xsl:value-of select="''"/>
            </BlockID>
          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>