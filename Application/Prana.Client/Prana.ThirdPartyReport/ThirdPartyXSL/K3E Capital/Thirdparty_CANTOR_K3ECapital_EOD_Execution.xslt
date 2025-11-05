<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <msxsl:script language="C#" implements-prefix="my"> public int RoundOff(double Qty) { return (int)Math.Round(Qty,0); } </msxsl:script>
  
  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
        
        <RowHeader>
          <xsl:value-of select="'False'"/>
        </RowHeader>
        
        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>
        
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
        
        <Action>
          <xsl:value-of select="'Action'"/>
        </Action>
        
        <TradeReference>
          <xsl:value-of select="'TradeReference'"/>
        </TradeReference>
        
        <OriginalTradeReference>
          <xsl:value-of select="'OriginalTradeReference'"/>
        </OriginalTradeReference>
        
        <Flow>
          <xsl:value-of select="'Flow'"/>
        </Flow>
        
        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>
        
        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>
        
        <TradeTime>
          <xsl:value-of select="'TradeTime'"/>
        </TradeTime>
        
        <SettleDate>
          <xsl:value-of select="'SettleDate'"/>
        </SettleDate>
        
        <Side>
          <xsl:value-of select="'Side'"/>
        </Side>
        
        <InstrumentId>
          <xsl:value-of select="'InstrumentId'"/>
        </InstrumentId>
        
        <InstrumentIdType>
          <xsl:value-of select="'InstrumentIdType'"/>
        </InstrumentIdType>
        
        
        
        <CountryOfTrading>
          <xsl:value-of select="'CountryOfTrading'"/>
        </CountryOfTrading>
        
        <ExchangeId>
          <xsl:value-of select="'ExchangeId'"/>
        </ExchangeId>
        
        <Depot>
          <xsl:value-of select="'Depot'"/>
        </Depot>
        
        <OptCloseOpen>
          <xsl:value-of select="'OptCloseOpen'"/>
        </OptCloseOpen>
        
        <OptUnderlyingSym>
          <xsl:value-of select="'OptUnderlyingSym'"/>
        </OptUnderlyingSym>
        
        <OptCallPut>
          <xsl:value-of select="'OptCallPut'"/>
        </OptCallPut>
        
        <OptStrikePrice>
          <xsl:value-of select="'OptStrikePrice'"/>
        </OptStrikePrice>
        
        <OptExpDate>
          <xsl:value-of select="'OptExpDate'"/>
        </OptExpDate>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>
               
        
        <Yield>
          <xsl:value-of select="'Yield'"/>
        </Yield>
        
        <PrincipalAmount>
          <xsl:value-of select="'PrincipalAmount'"/>
        </PrincipalAmount>
        
        <NetSettleAmount>
          <xsl:value-of select="'NetSettleAmount'"/>
        </NetSettleAmount>
        
        <TradeCurrency>
          <xsl:value-of select="'TradeCurrency'"/>
        </TradeCurrency>

        <SettleCurrency>
          <xsl:value-of select="'SettleCurrency'"/>
        </SettleCurrency>
        
        <Interest>
          <xsl:value-of select="'Interest'"/>
        </Interest>
        
        <FxRate>
          <xsl:value-of select="'FxRate'"/>
        </FxRate>
        
        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>
        
        <CommissionType>
          <xsl:value-of select="'CommissionType'"/>
        </CommissionType>
        
        <Broker>
          <xsl:value-of select="'Broker'"/>
        </Broker>
        
        <Strategy>
          <xsl:value-of select="'Strategy'"/>
        </Strategy>
        
        <TermDate>
          <xsl:value-of select="'Term Date'"/>
        </TermDate>
        
        <Fee1>
          <xsl:value-of select="'Fee1'"/>
        </Fee1>
        
        <Fee1Type>
          <xsl:value-of select="'Fee1Type'"/>
        </Fee1Type>
        
        <Fee2>
          <xsl:value-of select="'Fee2'"/>
        </Fee2>
        
        <Fee2Type>
          <xsl:value-of select="'Fee2Type'"/>
        </Fee2Type>
        
        <Fee3>
          <xsl:value-of select="'Fee3'"/>
        </Fee3>
        
        <Fee3Type>
          <xsl:value-of select="'Fee3Type'"/>
        </Fee3Type>
        
        <Fee4>
          <xsl:value-of select="'Fee4'"/>
        </Fee4>
        
        <Fee4Type>
          <xsl:value-of select="'Fee4Type'"/>
        </Fee4Type>
          
        <Fee5>
          <xsl:value-of select="'Fee5'"/>
        </Fee5>
        
        <Fee5Type>
          <xsl:value-of select="'Fee5Type'"/>
        </Fee5Type>
        
        <SettlementInstruction>
          <xsl:value-of select="'SettlementInstruction'"/>
        </SettlementInstruction>
        
        <RepoRate>
          <xsl:value-of select="'RepoRate'"/>
        </RepoRate>
        
        <AccountTypeOverride>
          <xsl:value-of select="'AccountTypeOverride'"/>
        </AccountTypeOverride>
        
        <RepoPrincipal>
          <xsl:value-of select="'RepoPrincipal'"/>
        </RepoPrincipal>
        
        <RepoHaircut>
          <xsl:value-of select="'RepoHaircut'"/>
        </RepoHaircut>
        
        <RepoHaircutDirection>
          <xsl:value-of select="'RepoHaircutDirection'"/>
        </RepoHaircutDirection>
        
        <RepoPartialPositionFlag>
          <xsl:value-of select="'Repo Partial Position Flag'"/>
        </RepoPartialPositionFlag>
        
      </ThirdPartyFlatFileDetail>
      
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <xsl:if test ="CounterParty='COWN'">

          <xsl:if test ="contains(AccountName,'Cantor Class C')">
            <ThirdPartyFlatFileDetail>

            <RowHeader>
              <xsl:value-of select="'False'"/>
            </RowHeader>

            <TaxLotState>
              <xsl:value-of select="TaxLotState"/>
            </TaxLotState>

            <EntityID>
              <xsl:value-of select="EntityID"/>
            </EntityID>

            <Action>
              <xsl:value-of select="'NEW'"/>
            </Action>

            <TradeReference>
              <xsl:value-of select="TradeRefID"/>
            </TradeReference>

            <OriginalTradeReference>
              <xsl:value-of select="''"/>
            </OriginalTradeReference>

            <Flow>
              <xsl:value-of select="'PB'"/>
            </Flow>

            <Account>
              <xsl:choose>
                <xsl:when test="AccountNo = '025-00231' or AccountNo='2002088'">
                  <xsl:value-of select="'TD'"/>
                </xsl:when>
                <xsl:when test="AccountNo = '32500154'">
                  <xsl:value-of select="'CANT'"/>
                </xsl:when>
   
            <xsl:otherwise>
              <xsl:value-of select="''"/>
            </xsl:otherwise>
          </xsl:choose>
        </Account>

            <TradeDate>
              <xsl:value-of select="TradeDate"/>
            </TradeDate>

            <TradeTime>
              <xsl:value-of select="''"/>
            </TradeTime>

            <SettleDate>
              <xsl:value-of select="SettlementDate"/>
            </SettleDate>

            <Side>
              <xsl:choose>
                <xsl:when test="Side='Sell short'">
                  <xsl:value-of select="'SHORT'"/>
                </xsl:when>
                <xsl:when test="Side='Buy to Close'">
                  <xsl:value-of select="'COVER'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate(Side,$varSmall,$varCapital)"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <InstrumentId>
              <xsl:value-of select="Symbol"/>
            </InstrumentId>

            <InstrumentIdType>
              <xsl:value-of select="'TICKER'"/>
            </InstrumentIdType>

            <CountryOfTrading>
              <xsl:value-of select="''"/>
            </CountryOfTrading>

            <ExchangeId>
              <xsl:value-of select="''"/>
            </ExchangeId>

            <Depot>
              <xsl:value-of select="''"/>
            </Depot>

            <OptCloseOpen>
              <xsl:value-of select="''"/>
            </OptCloseOpen>

            <OptUnderlyingSym>
              <xsl:value-of select="''"/>
            </OptUnderlyingSym>

            <OptCallPut>
              <xsl:value-of select="PutOrCall"/>
            </OptCallPut>

            <OptStrikePrice>
              <xsl:value-of select="''"/>
            </OptStrikePrice>

            <OptExpDate>
              <xsl:value-of select="''"/>
            </OptExpDate>

            <Quantity>
              <xsl:value-of select="AllocatedQty"/>
            </Quantity>

            <Price>
              <xsl:value-of select="AveragePrice"/>
            </Price>

            <Yield>
              <xsl:value-of select="''"/>
            </Yield>

            <PrincipalAmount>
              <xsl:value-of select="''"/>
            </PrincipalAmount>

            <NetSettleAmount>
              <xsl:value-of select="''"/>
            </NetSettleAmount>

            <TradeCurrency>
              <xsl:value-of select="''"/>
            </TradeCurrency>

            <SettleCurrency>
              <xsl:value-of select="'USD'"/>
            </SettleCurrency>

            <Interest>
              <xsl:value-of select="''"/>
            </Interest>

            <FxRate>
              <xsl:value-of select="''"/>
            </FxRate>

            <Commission>
              <xsl:value-of select="CommissionCharged"/>
            </Commission>

            <CommissionType>
              <xsl:value-of select="'ABSOLUTE'"/>
            </CommissionType>

            <Broker>
              <xsl:value-of select="CounterParty"/>
            </Broker>

            <Strategy>
              <xsl:value-of select="''"/>
            </Strategy>

            <TermDate>
              <xsl:value-of select="''"/>
            </TermDate>

            <Fee1>
              <xsl:value-of select="''"/>
            </Fee1>

            <Fee1Type>
              <xsl:value-of select="''"/>
            </Fee1Type>

            <Fee2>
              <xsl:value-of select="''"/>
            </Fee2>

            <Fee2Type>
              <xsl:value-of select="''"/>
            </Fee2Type>

            <Fee3>
              <xsl:value-of select="''"/>
            </Fee3>

            <Fee3Type>
              <xsl:value-of select="''"/>
            </Fee3Type>

            <Fee4>
              <xsl:value-of select="''"/>
            </Fee4>

            <Fee4Type>
              <xsl:value-of select="''"/>
            </Fee4Type>

            <Fee5>
              <xsl:value-of select="''"/>
            </Fee5>

            <Fee5Type>
              <xsl:value-of select="''"/>
            </Fee5Type>

            <SettlementInstruction>
              <xsl:value-of select="''"/>
            </SettlementInstruction>

            <RepoRate>
              <xsl:value-of select="''"/>
            </RepoRate>

            <AccountTypeOverride>
              <xsl:value-of select="''"/>
            </AccountTypeOverride>

            <RepoPrincipal>
              <xsl:value-of select="''"/>
            </RepoPrincipal>

            <RepoHaircut>
              <xsl:value-of select="''"/>
            </RepoHaircut>

            <RepoHaircutDirection>
              <xsl:value-of select="''"/>
            </RepoHaircutDirection>

            <RepoPartialPositionFlag>
              <xsl:value-of select="''"/>
            </RepoPartialPositionFlag>

          </ThirdPartyFlatFileDetail>
          </xsl:if>
            
        </xsl:if>
        
        <xsl:if test ="not(contains(CounterParty,'COWN'))">
          <ThirdPartyFlatFileDetail>

            <RowHeader>
              <xsl:value-of select="'False'"/>
            </RowHeader>

            <TaxLotState>
              <xsl:value-of select="TaxLotState"/>
            </TaxLotState>

            <EntityID>
              <xsl:value-of select="EntityID"/>
            </EntityID>

            <Action>
              <xsl:value-of select="'NEW'"/>
            </Action>

            <TradeReference>
              <xsl:value-of select="TradeRefID"/>
            </TradeReference>

            <OriginalTradeReference>
              <xsl:value-of select="''"/>
            </OriginalTradeReference>

            <Flow>
              <xsl:value-of select="'PB'"/>
            </Flow>

            <Account>
              <xsl:choose>
                <xsl:when test="AccountNo = '025-00231' or AccountNo='2002088'">
                  <xsl:value-of select="'TD'"/>
                </xsl:when>
                <xsl:when test="AccountNo = '32500154'">
                  <xsl:value-of select="'CANT'"/>
                </xsl:when>
   
            <xsl:otherwise>
              <xsl:value-of select="''"/>
            </xsl:otherwise>
          </xsl:choose>
        </Account>

            <TradeDate>
              <xsl:value-of select="TradeDate"/>
            </TradeDate>

            <TradeTime>
              <xsl:value-of select="''"/>
            </TradeTime>

            <SettleDate>
              <xsl:value-of select="SettlementDate"/>
            </SettleDate>

            <Side>
              <xsl:choose>
                <xsl:when test="Side='Sell short'">
                  <xsl:value-of select="'SHORT'"/>
                </xsl:when>
                <xsl:when test="Side='Buy to Close'">
                  <xsl:value-of select="'COVER'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate(Side,$varSmall,$varCapital)"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <InstrumentId>
              <xsl:value-of select="Symbol"/>
            </InstrumentId>

            <InstrumentIdType>
              <xsl:value-of select="'TICKER'"/>
            </InstrumentIdType>

            <CountryOfTrading>
              <xsl:value-of select="''"/>
            </CountryOfTrading>

            <ExchangeId>
              <xsl:value-of select="''"/>
            </ExchangeId>

            <Depot>
              <xsl:value-of select="''"/>
            </Depot>

            <OptCloseOpen>
              <xsl:value-of select="''"/>
            </OptCloseOpen>

            <OptUnderlyingSym>
              <xsl:value-of select="''"/>
            </OptUnderlyingSym>

            <OptCallPut>
              <xsl:value-of select="PutOrCall"/>
            </OptCallPut>

            <OptStrikePrice>
              <xsl:value-of select="''"/>
            </OptStrikePrice>

            <OptExpDate>
              <xsl:value-of select="''"/>
            </OptExpDate>

            <Quantity>
              <xsl:value-of select="AllocatedQty"/>
            </Quantity>

            <Price>
              <xsl:value-of select="AveragePrice"/>
            </Price>

            <Yield>
              <xsl:value-of select="''"/>
            </Yield>

            <PrincipalAmount>
              <xsl:value-of select="''"/>
            </PrincipalAmount>

            <NetSettleAmount>
              <xsl:value-of select="''"/>
            </NetSettleAmount>

            <TradeCurrency>
              <xsl:value-of select="''"/>
            </TradeCurrency>

            <SettleCurrency>
              <xsl:value-of select="'USD'"/>
            </SettleCurrency>

            <Interest>
              <xsl:value-of select="''"/>
            </Interest>

            <FxRate>
              <xsl:value-of select="''"/>
            </FxRate>

            <Commission>
              <xsl:value-of select="CommissionCharged"/>
            </Commission>

            <CommissionType>
              <xsl:value-of select="'ABSOLUTE'"/>
            </CommissionType>

            <Broker>
              <xsl:value-of select="CounterParty"/>
            </Broker>

            <Strategy>
              <xsl:value-of select="''"/>
            </Strategy>

            <TermDate>
              <xsl:value-of select="''"/>
            </TermDate>

            <Fee1>
              <xsl:value-of select="''"/>
            </Fee1>

            <Fee1Type>
              <xsl:value-of select="''"/>
            </Fee1Type>

            <Fee2>
              <xsl:value-of select="''"/>
            </Fee2>

            <Fee2Type>
              <xsl:value-of select="''"/>
            </Fee2Type>

            <Fee3>
              <xsl:value-of select="''"/>
            </Fee3>

            <Fee3Type>
              <xsl:value-of select="''"/>
            </Fee3Type>

            <Fee4>
              <xsl:value-of select="''"/>
            </Fee4>

            <Fee4Type>
              <xsl:value-of select="''"/>
            </Fee4Type>

            <Fee5>
              <xsl:value-of select="''"/>
            </Fee5>

            <Fee5Type>
              <xsl:value-of select="''"/>
            </Fee5Type>

            <SettlementInstruction>
              <xsl:value-of select="''"/>
            </SettlementInstruction>

            <RepoRate>
              <xsl:value-of select="''"/>
            </RepoRate>

            <AccountTypeOverride>
              <xsl:value-of select="''"/>
            </AccountTypeOverride>

            <RepoPrincipal>
              <xsl:value-of select="''"/>
            </RepoPrincipal>

            <RepoHaircut>
              <xsl:value-of select="''"/>
            </RepoHaircut>

            <RepoHaircutDirection>
              <xsl:value-of select="''"/>
            </RepoHaircutDirection>

            <RepoPartialPositionFlag>
              <xsl:value-of select="''"/>
            </RepoPartialPositionFlag>

          </ThirdPartyFlatFileDetail>  
        </xsl:if>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>