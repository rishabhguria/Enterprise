<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          <!-- system inetrnal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
          <!-- system inetrnal use-->
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <xsl:choose>
            <xsl:when test ="FundName = 'JEFF Offshore'">
              <PortfolioCode>
                <xsl:value-of select ="'43300037'"/>
              </PortfolioCode>
            </xsl:when>
            <xsl:when test ="FundName = 'JEFF SCLP'">
              <PortfolioCode>
                <xsl:value-of select ="'43300005'"/>
              </PortfolioCode>
            </xsl:when>
            <xsl:when test ="FundName = 'IB Offshore'">
              <PortfolioCode>
                <xsl:value-of select ="'U743966'"/>
              </PortfolioCode>
            </xsl:when>
            <xsl:when test ="FundName = 'IB SCLP'">
              <PortfolioCode>
                <xsl:value-of select ="'U743965'"/>
              </PortfolioCode>
            </xsl:when>
            <xsl:when test ="FundName = 'PERS Offshore'">
              <PortfolioCode>
                <xsl:value-of select ="'JMP001813'"/>
              </PortfolioCode>
            </xsl:when>
            <xsl:when test ="FundName = 'PERS SCLP'">
              <PortfolioCode>
                <xsl:value-of select ="'JMP001797'"/>
              </PortfolioCode>
            </xsl:when>
            <xsl:when test ="FundName = 'HSBC SCLP'">
              <PortfolioCode>
                <xsl:value-of select ="'10385'"/>
              </PortfolioCode>
            </xsl:when>
            <xsl:when test ="FundName = 'HSBC Offshore'">
              <PortfolioCode>
                <xsl:value-of select ="'10443'"/>
              </PortfolioCode>
            </xsl:when>
            <xsl:when test ="FundName = 'Schwab SCLP'">
              <PortfolioCode>
                <xsl:value-of select ="'26004389'"/>
              </PortfolioCode>
            </xsl:when>
            <xsl:when test ="FundName = 'AMTD SCLP'">
              <PortfolioCode>
                <xsl:value-of select ="'amtdsclp'"/>
              </PortfolioCode>
            </xsl:when>
            <xsl:otherwise>
              <PortfolioCode>
                <xsl:value-of select ="''"/>
              </PortfolioCode>
            </xsl:otherwise>
          </xsl:choose>


          <!--<PortfolioCode>
						<xsl:value-of select="FundName"/>
					</PortfolioCode>-->


          <!--   Side     -->
          <xsl:choose>
            <xsl:when test="Side='Buy' or Side='Buy to Open'">
              <TranCode>
                <xsl:value-of select="'by'"/>
              </TranCode>
            </xsl:when>
            <xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
              <TranCode>
                <xsl:value-of select="'cs'"/>
              </TranCode>
            </xsl:when>
            <xsl:when test="Side='Sell' or Side='Sell to Close'">
              <TranCode>
                <xsl:value-of select="'sl'"/>
              </TranCode>
            </xsl:when>
            <xsl:when test="Side='Sell short' or Side='Sell to Open'">
              <TranCode>
                <xsl:value-of select="'ss'"/>
              </TranCode>
            </xsl:when>
            <xsl:otherwise>
              <TranCode>
                <xsl:value-of select="''"/>
              </TranCode>
            </xsl:otherwise>
          </xsl:choose>

          <!--   Side End    -->
          <Comment>
            <xsl:value-of select ="''"/>
          </Comment>

          <xsl:variable name ="varCheckSymbolUnderlying">
            <xsl:value-of select ="substring-before(Symbol,'-')"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test="$varCheckSymbolUnderlying = '' and Asset = 'Equity'">
              <SecType>
                <xsl:value-of select ="'csus'"/>
              </SecType>
            </xsl:when >
            <xsl:when test="Asset='EquityOption' and PutOrCall = 'CALL'">
              <SecType>
                <xsl:value-of select ="'clus'"/>
              </SecType>
            </xsl:when >
            <xsl:when test="Asset='EquityOption' and PutOrCall = 'PUT'">
              <SecType>
                <xsl:value-of select ="'ptus'"/>
              </SecType>
            </xsl:when >
            <xsl:otherwise>
              <SecType>
                <xsl:value-of select ="''"/>
              </SecType>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:variable name ="varEqtSymbol">
            <xsl:choose>
              <xsl:when test ="Asset='Equity' and substring-before(Symbol,'/W') != ''">
                <xsl:value-of select ="concat(translate(Symbol,'.','/'),'S')"/>
              </xsl:when >
              <xsl:when test ="Asset='Equity' and substring-before(Symbol,'/W') = ''">
                <xsl:value-of select ="Symbol"/>
              </xsl:when >
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:choose>
            <!--For US Equities use ticker symbol and add ' us' to it. For example MSFT is msft us-->
            <!--varCheckSymbolUnderlying is used to check whether Symbol is US Equity Symbol or international symbol-->
            <!--<xsl:when test ="Asset = 'Equity' and CurrencySymbol = 'USD'">-->
            <xsl:when test ="Asset = 'Equity' and $varCheckSymbolUnderlying = ''">
              <SecuritySymbol>
                <!--<xsl:value-of select="concat(translate(Symbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST), ' us')"/>-->
                <xsl:value-of select="translate($varEqtSymbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
              </SecuritySymbol>
            </xsl:when>
            <!--For US Equity Options use ticker symbol and add '+' to it. For example MSFT is msft us-->
            <xsl:when test ="Asset = 'EquityOption'">
              <SecuritySymbol>
                <xsl:value-of select="translate(OSIOptionSymbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
                <!--<xsl:value-of select="Symbol"/>-->
              </SecuritySymbol>
            </xsl:when>
            <!--Otherwise use ticker symbol and translate to lower case. For example MSFT is msft us-->
            <xsl:otherwise>
              <SecuritySymbol>
                <xsl:value-of select="translate(Symbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
              </SecuritySymbol>
            </xsl:otherwise>
          </xsl:choose>

          <TradeDate>
            <xsl:value-of select ="translate(TradeDate,'/','')"/>
          </TradeDate>

          <!--column 7 -->

          <SettleDate>
            <xsl:value-of select ="translate(SettlementDate,'/','')"/>
          </SettleDate>

          <OriginalCostDate>
            <xsl:value-of select ="''"/>
          </OriginalCostDate>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <CloseMath>
            <xsl:value-of select ="''"/>
          </CloseMath>

          <VersusDate>
            <xsl:value-of select="''"/>
          </VersusDate>

          <SourceType>
            <xsl:value-of select ="'caus'"/>
          </SourceType>

          <!-- Column 13-->
          <SourceSymbol>
            <xsl:value-of select ="'cash'"/>
          </SourceSymbol>

          <TradeDateFXRate>
            <xsl:value-of select ="''"/>
          </TradeDateFXRate>

          <SettleDateFXRate>
            <xsl:value-of select="''"/>
          </SettleDateFXRate>

          <OriginalFXRate>
            <xsl:value-of select ="''"/>
          </OriginalFXRate>

          <MarkToMarket>
            <xsl:value-of select ="''"/>
          </MarkToMarket>

          <xsl:variable name ="varAvgPrice">
            <xsl:choose>
              <xsl:when test ="AveragePrice != 0">
                <xsl:value-of select = 'format-number(AveragePrice, "###.0000000")'/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <TradeAmount>
            <xsl:value-of select="concat('@',$varAvgPrice)"/>
          </TradeAmount>

          <OriginalCost>
            <xsl:value-of select ="''"/>
          </OriginalCost>

          <!--column 20 -->

          <Comment1>
            <xsl:value-of select ="''"/>
          </Comment1>

          <WithholdingTax>
            <xsl:value-of select ="''"/>
          </WithholdingTax>

          <Exchange>
            <xsl:value-of select ="'5'"/>
          </Exchange>

          <!--<ExchangeFee>
						<xsl:value-of select ="MiscFees"/>
					</ExchangeFee>-->

          <xsl:choose>
            <xsl:when test ="MiscFees != 0">
              <ExchangeFee>
                <xsl:value-of select = 'format-number(MiscFees, "###.0000000")'/>
              </ExchangeFee>
            </xsl:when>
            <xsl:otherwise>
              <ExchangeFee>
                <xsl:value-of select ="0"/>
              </ExchangeFee>
            </xsl:otherwise>
          </xsl:choose>

          <!--<commission>
						<xsl:value-of select="CommissionCharged"/>
					</commission>-->

          <xsl:choose>
            <xsl:when test ="CommissionCharged != 0">
              <commission>
                <xsl:value-of select = 'format-number(CommissionCharged, "###.0000000")'/>
              </commission>
            </xsl:when>
            <xsl:otherwise>
              <commission>
                <xsl:value-of select ="0"/>
              </commission>
            </xsl:otherwise>
          </xsl:choose>

          <xsl:choose>
            <xsl:when test ="string-length(CounterParty) &lt; 4">
              <Broker>
                <xsl:value-of select="concat(translate(CounterParty,$vUppercaseChars_CONST,$vLowercaseChars_CONST),'kr')"/>
              </Broker>
            </xsl:when>
            <xsl:otherwise>
              <Broker>
                <xsl:value-of select="translate(CounterParty,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
              </Broker>
            </xsl:otherwise>
          </xsl:choose>

          <ImpliedComm>
            <xsl:value-of select ="'n'"/>
          </ImpliedComm>

          <!--<OtherFees>
						<xsl:value-of select ="OtherBrokerFee"/>
					</OtherFees>-->

          <xsl:choose>
            <xsl:when test ="OtherBrokerFee != 0">
              <OtherFees>
                <xsl:value-of select = 'format-number(OtherBrokerFee, "###.0000000")'/>
              </OtherFees>
            </xsl:when>
            <xsl:otherwise>
              <OtherFees>
                <xsl:value-of select ="0"/>
              </OtherFees>
            </xsl:otherwise>
          </xsl:choose>

          <CommPurpose>
            <xsl:value-of select ="''"/>
          </CommPurpose>

          <!-- Column 29-->

          <Pledge>
            <xsl:value-of select ="'n'"/>
          </Pledge>

          <LotLocation>
            <xsl:value-of select ="'253'"/>
          </LotLocation>

          <DestPledge>
            <xsl:value-of select ="''"/>
          </DestPledge>

          <DestLotLocation>
            <xsl:value-of select ="''"/>
          </DestLotLocation>

          <OriginalFace>
            <xsl:value-of select ="''"/>
          </OriginalFace>

          <YieldOnCost>
            <xsl:value-of select ="''"/>
          </YieldOnCost>

          <!-- column 35-->

          <DurationOnCost>
            <xsl:value-of select ="''"/>
          </DurationOnCost>

          <UserDef1>
            <xsl:value-of select ="''"/>
          </UserDef1>

          <UserDef2>
            <xsl:value-of select="''"/>
          </UserDef2>

          <UserDef3>
            <xsl:value-of select="''"/>
          </UserDef3>

          <TranID>
            <xsl:value-of select ="''"/>
          </TranID>

          <IPCounter>
            <xsl:value-of select ="''"/>
          </IPCounter>

          <Repl>
            <xsl:value-of select="''"/>
          </Repl>

          <!-- column 42-->

          <Source>
            <xsl:value-of select ="''"/>
          </Source>

          <Comment2>
            <xsl:value-of select ="''"/>
          </Comment2>

          <OmniAcct>
            <xsl:value-of select ="''"/>
          </OmniAcct>

          <Recon>
            <xsl:value-of select="''"/>
          </Recon>

          <Post>
            <xsl:value-of select ="'y'"/>
          </Post>

          <LabelName>
            <xsl:value-of select="''"/>
          </LabelName>

          <LabelDefinition>
            <xsl:value-of select ="''"/>
          </LabelDefinition>

          <LabelDefinition_Date>
            <xsl:value-of select="''"/>
          </LabelDefinition_Date>

          <!-- column 50-->

          <LabelDefinition_String>
            <xsl:value-of select="''"/>
          </LabelDefinition_String>

          <Comment3>
            <xsl:value-of select ="''"/>
          </Comment3>

          <RecordDate>
            <xsl:value-of select ="''"/>
          </RecordDate>

          <ReclaimAmount>
            <xsl:value-of select ="''"/>
          </ReclaimAmount>

          <Strategy>
            <xsl:value-of select ="''"/>
          </Strategy>

          <Comment4>
            <xsl:value-of select ="''"/>
          </Comment4>

          <IncomeAccount>
            <xsl:value-of select ="''"/>
          </IncomeAccount>

          <AccrualAccount>
            <xsl:value-of select ="''"/>
          </AccrualAccount>

          <DivAccrualMethod>
            <xsl:value-of select ="''"/>
          </DivAccrualMethod>

          <PerfContributionOrWithdrawal>
            <xsl:value-of select ="''"/>
          </PerfContributionOrWithdrawal>

          <!-- system inetrnal use-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
