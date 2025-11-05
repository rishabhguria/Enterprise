<?xml version="1.0" encoding="UTF-8"?>
<!--Description: Citco EOD file, Created Date: 02-13-2012(mm-DD-YY)-->

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

  <xsl:template name="substring-after-last">
    <xsl:param name="string" />
    <xsl:param name="delimiter" />
    <xsl:choose>
      <xsl:when test="contains($string, $delimiter)">
        <xsl:call-template name="substring-after-last">
          <xsl:with-param name="string"
            select="substring-after($string, $delimiter)" />
          <xsl:with-param name="delimiter" select="$delimiter" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$string" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system use only-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select ="'TaxLotState'"/>
        </TaxLotState>

        <OrdStatus>
          <xsl:value-of select ="'OrdStatus'"/>
        </OrdStatus>

        <ExecTransType>
          <xsl:value-of select="'ExecTransType'"/>
        </ExecTransType>

        <ClientOrderID>
          <xsl:value-of select="'ClientOrderID'"/>
        </ClientOrderID>

        <FillID>
          <xsl:value-of select="'Fill ID'"/>
        </FillID>

        <OrderID>
          <xsl:value-of select="'ID of Order Or Fill for Action'"/>
        </OrderID>

        <LotNumber>
          <xsl:value-of select="'LotNumber'"/>
        </LotNumber>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <SecurityType>
          <xsl:value-of select="'SecurityType'"/>
        </SecurityType>

        <SecurityCurrency>
          <xsl:value-of select="'Security Currency'"/>
        </SecurityCurrency>

        <SecurityDescription>
          <xsl:value-of select="'Security Description'"/>
        </SecurityDescription>

        <BuySellShortCover>
          <xsl:value-of select="'BuySellShortCover'"/>
        </BuySellShortCover>

        <OpenClose>
          <xsl:value-of select="'OpenClose'"/>
        </OpenClose>

        <IDSource>
          <xsl:value-of select="'IDSource'"/>
        </IDSource>

        <SecurityID>
          <xsl:value-of select="'SecurityID'"/>
        </SecurityID>

        <ISIN>
          <xsl:value-of select="'ISIN'"/>
        </ISIN>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <SEDOL>
          <xsl:value-of select="'SEDOL'"/>
        </SEDOL>

        <Bloomberg>
          <xsl:value-of select="'Bloomberg'"/>
        </Bloomberg>

        <CINS>
          <xsl:value-of select="'CINS'"/>
        </CINS>

        <WhenIssued>
          <xsl:value-of select="'WhenIssued'"/>
        </WhenIssued>

        <IssueDate>
          <xsl:value-of select="'IssueDate'"/>
        </IssueDate>

        <Maturity>
          <xsl:value-of select="'Maturity'"/>
        </Maturity>

        <Coupon>
          <xsl:value-of select="'Coupon %'"/>
        </Coupon>

        <ExecutionInterestDays>
          <xsl:value-of select="'ExecutionInterestDays'"/>
        </ExecutionInterestDays>

        <AccruedInterest>
          <xsl:value-of select="'AccruedInterest'"/>
        </AccruedInterest>

        <FaceValue>
          <xsl:value-of select="'FaceValue'"/>
        </FaceValue>

        <RollableType>
          <xsl:value-of select="'RollableType'"/>
        </RollableType>

        <RepoLoanCurrency>
          <xsl:value-of select="'RepoLoanCurrency'"/>
        </RepoLoanCurrency>

        <DayCountBase>
          <xsl:value-of select="'DayCountBase'"/>
        </DayCountBase>

        <RepoLoanAmount>
          <xsl:value-of select="'RepoLoanAmount'"/>
        </RepoLoanAmount>

        <Trader>
          <xsl:value-of select="'Trader'"/>
        </Trader>

        <OrderQty>
          <xsl:value-of select="'OrderQty'"/>
        </OrderQty>

        <FillQty>
          <xsl:value-of select="'FillQty'"/>
        </FillQty>

        <CumQty>
          <xsl:value-of select="'CumQty'"/>
        </CumQty>

        <HairCut>
          <xsl:value-of select="'HairCut'"/>
        </HairCut>

        <AvgPx>
          <xsl:value-of select="'AvgPx'"/>
        </AvgPx>

        <FillPrice>
          <xsl:value-of select="'FillPrice'"/>
        </FillPrice>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <TradeTime>
          <xsl:value-of select="'TradeTime'"/>
        </TradeTime>

        <OrigDate>
          <xsl:value-of select="'OrigDate'"/>
        </OrigDate>

        <Unused>
          <xsl:value-of select="'Unused'"/>
        </Unused>

        <SettlementDate>
          <xsl:value-of select="'SettlementDate'"/>
        </SettlementDate>

        <ExecutingUser>
          <xsl:value-of select="'Executing User'"/>
        </ExecutingUser>

        <Comment>
          <xsl:value-of select="'Comment'"/>
        </Comment>

        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <Fund>
          <xsl:value-of select="'Fund'"/>
        </Fund>

        <SubFund>
          <xsl:value-of select="'SubFund'"/>
        </SubFund>

        <AllocationCode>
          <xsl:value-of select="'AllocationCode'"/>
        </AllocationCode>

        <StrategyCode>
          <xsl:value-of select="'StrategyCode'"/>
        </StrategyCode>

        <ExecutionBroker>
          <xsl:value-of select="'Execution Broker'"/>
        </ExecutionBroker>

        <ClearingBroker>
          <xsl:value-of select="'ClearingBroker'"/>
        </ClearingBroker>

        <ContractSize>
          <xsl:value-of select="'ContractSize'"/>
        </ContractSize>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <SpotFXRate>
          <xsl:value-of select="'Spot FX Rate'"/>
        </SpotFXRate>

        <FWDFXpoints>
          <xsl:value-of select="'FWD FX points'"/>
        </FWDFXpoints>

        <Fee>
          <xsl:value-of select="'Fee'"/>
        </Fee>

        <CurrencyTraded>
          <xsl:value-of select="'CurrencyTraded'"/>
        </CurrencyTraded>

        <SettleCurrency>
          <xsl:value-of select="'SettleCurrency'"/>
        </SettleCurrency>

        <FXBASErate>
          <xsl:value-of select="'FX/BASE rate'"/>
        </FXBASErate>

        <BASEFXrate>
          <xsl:value-of select="'BASE/FX rate'"/>
        </BASEFXrate>

        <StrikePrice>
          <xsl:value-of select="'StrikePrice'"/>
        </StrikePrice>

        <PutOrCall>
          <xsl:value-of select="'PutOrCall'"/>
        </PutOrCall>

        <DerivativeExpiry>
          <xsl:value-of select="'Derivative Expiry'"/>
        </DerivativeExpiry>

        <SubStrategy>
          <xsl:value-of select="'SubStrategy'"/>
        </SubStrategy>

        <OrderGroup>
          <xsl:value-of select="'OrderGroup'"/>
        </OrderGroup>

        <RepoPenalty>
          <xsl:value-of select="'RepoPenalty'"/>
        </RepoPenalty>

        <CommissionTurn>
          <xsl:value-of select="'CommissionTurn'"/>
        </CommissionTurn>

        <AllocRule>
          <xsl:value-of select="'AllocRule'"/>
        </AllocRule>

        <PaymentFreq>
          <xsl:value-of select="'PaymentFreq'"/>
        </PaymentFreq>

        <RateSource>
          <xsl:value-of select="'RateSource'"/>
        </RateSource>

        <Spread>
          <xsl:value-of select="'Spread'"/>
        </Spread>

        <CurrentFace>
          <xsl:value-of select="'CurrentFace'"/>
        </CurrentFace>

        <CurrentPrincipalFactor>
          <xsl:value-of select="'CurrentPrincipalFactor'"/>
        </CurrentPrincipalFactor>

        <AccrualFactor>
          <xsl:value-of select="'AccrualFactor'"/>
        </AccrualFactor>

        <TaxRate>
          <xsl:value-of select="'Tax Rate'"/>
        </TaxRate>

        <Expenses>
          <xsl:value-of select="'Expenses'"/>
        </Expenses>

        <Fees>
          <xsl:value-of select="'Fees'"/>
        </Fees>

        <NetConsideration>
          <xsl:value-of select="'Net Consideration'"/>
        </NetConsideration>

        <ImpliedCommissionFlag>
          <xsl:value-of select="'Implied Commission Flag'"/>
        </ImpliedCommissionFlag>

        <TransactionType>
          <xsl:value-of select="'Transaction Type'"/>
        </TransactionType>

        <MasterConfrimType>
          <xsl:value-of select="'Master Confrim Type'"/>
        </MasterConfrimType>

        <MatrixTerm>
          <xsl:value-of select="'Matrix Term'"/>
        </MatrixTerm>

        <EMInternalSeqNo>
          <xsl:value-of select="'EMInternalSeqNo.'"/>
        </EMInternalSeqNo>

        <ObjectivePrice>
          <xsl:value-of select="'ObjectivePrice'"/>
        </ObjectivePrice>

        <MarketPrice>
          <xsl:value-of select="'MarketPrice'"/>
        </MarketPrice>

        <StopPrice>
          <xsl:value-of select="'StopPrice'"/>
        </StopPrice>

        <NetConsdieration>
          <xsl:value-of select="'NetConsdieration'"/>
        </NetConsdieration>

        <FixingDate>
          <xsl:value-of select="'Fixing Date'"/>
        </FixingDate>

        <DeliveryInstructions>
          <xsl:value-of select="'Delivery Instructions'"/>
        </DeliveryInstructions>

        <ForceMatchID>
          <xsl:value-of select="'Force Match ID'"/>
        </ForceMatchID>

        <ForceMatchType>
          <xsl:value-of select="'Force Match Type'"/>
        </ForceMatchType>

        <ForceMatchNotes>
          <xsl:value-of select="'Force Match Notes'"/>
        </ForceMatchNotes>

        <CommissionRateAllocation>
          <xsl:value-of select="'Commission Rate for Allocation'"/>
        </CommissionRateAllocation>

        <CommissionAmountFill>
          <xsl:value-of select="'Commission Amount for Fill'"/>
        </CommissionAmountFill>

        <ExpenseAmountforFill>
          <xsl:value-of select="'Expense Amount for Fill'"/>
        </ExpenseAmountforFill>

        <FeeAmountforFill>
          <xsl:value-of select="'Fee Amount for Fill'"/>
        </FeeAmountforFill>

        <StandardStrategy>
          <xsl:value-of select="'Standard Strategy'"/>
        </StandardStrategy>

        <StrategyLinkName>
          <xsl:value-of select="'Strategy Link Name'"/>
        </StrategyLinkName>

        <StrategyGroup>
          <xsl:value-of select="'Strategy Group'"/>
        </StrategyGroup>

        <FillFXSettleAmount>
          <xsl:value-of select="'Fill FX Settle Amount'"/>
        </FillFXSettleAmount>


        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName = '99726']">
          <ThirdPartyFlatFileDetail>
            <!--for system internal use-->
            <RowHeader>
              <xsl:value-of select ="true"/>
            </RowHeader>

            <!--for system use only-->
            <IsCaptionChangeRequired>
              <xsl:value-of select ="true"/>
            </IsCaptionChangeRequired>

            <!--for system internal use-->
            <TaxLotState>
              <xsl:value-of select ="TaxLotState"/>
            </TaxLotState>

            <OrdStatus>
              <xsl:choose>
                <xsl:when test="TaxLotState = 'Allocated'">
                  <xsl:value-of select="'N'"/>
                </xsl:when>
                <xsl:when test="TaxLotState = 'Amemded'">
                  <xsl:value-of select="'R'"/>
                </xsl:when>
                <xsl:when test="TaxLotState = 'Deleted'">
                  <xsl:value-of select="'D'"/>
                </xsl:when>
              </xsl:choose>
            </OrdStatus>

            <ExecTransType>
              <xsl:choose>
                <xsl:when test="TaxLotState = 'Allocated'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'0'"/>
                </xsl:otherwise>
              </xsl:choose>
            </ExecTransType>

            <ClientOrderID>
              <xsl:value-of select="concat('CIT',TradeRefID)"/>
            </ClientOrderID>

            <FillID>
				<xsl:value-of select="concat('CIT',TradeRefID)"/>
			</FillID>

            <OrderID>
              <xsl:value-of select="''"/>
            </OrderID>

            <LotNumber>
              <xsl:value-of select="''"/>
            </LotNumber>

            <Symbol>
              <xsl:value-of select="''"/>
            </Symbol>

            <xsl:variable name="varKey">
              <xsl:call-template name="substring-after-last">
                <xsl:with-param name="string" select="BBCode"/>
                <xsl:with-param name="delimiter" select="' '"/>
              </xsl:call-template>
            </xsl:variable>


            <SecurityType>
              <xsl:choose>
                <xsl:when test="($varKey = 'Index' or $varKey = 'INDEX') and Asset = 'Future'">
                  <xsl:value-of select="'IDXFUT'"/>
                </xsl:when>
                <xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY' or $varKey = 'Curncy' or $varKey = 'CURNCY') and Asset = 'Future' and Exchange != 'LME'">
                  <xsl:value-of select="'CMDFUT'"/>
                </xsl:when>
                <xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY' or $varKey = 'Curncy' or $varKey = 'CURNCY') and Asset = 'FutureOption'">
                  <xsl:value-of select="'CMDFUTOPT'"/>
                </xsl:when>
                <xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY') and Exchange = 'LME' ">
                  <xsl:value-of select="'MTLFWD'"/>
                </xsl:when>
              </xsl:choose>
            </SecurityType>

            <SecurityCurrency>
              <xsl:value-of select="CurrencySymbol"/>
            </SecurityCurrency>

            <SecurityDescription>
              <xsl:value-of select="''"/>
            </SecurityDescription>

            <BuySellShortCover>
              <xsl:choose>
                <xsl:when test="Side = 'Buy'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="Side = 'Sell'">
                  <xsl:value-of select="'S'"/>
                </xsl:when>
                <xsl:when test="Side = 'Sell short'">
                  <xsl:value-of select="'SS'"/>
                </xsl:when>
                <xsl:when test="Side = 'Buy to Close'">
                  <xsl:value-of select="'BC'"/>
                </xsl:when>
              </xsl:choose>
            </BuySellShortCover>

            <OpenClose>
              <xsl:value-of select="''"/>
            </OpenClose>

            <IDSource>
              <!--<xsl:choose>
                <xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY') and Exchange = 'LME' ">
                  <xsl:value-of select="'TID'"/>
                </xsl:when>
                <xsl:otherwise>-->
                  <xsl:value-of select="'BLOOMBERG'"/>
                <!--</xsl:otherwise>
              </xsl:choose>-->
            </IDSource>

            <SecurityID>
              <!--<xsl:choose>
                <xsl:when test="($varKey = 'Comdty' or $varKey = 'COMDTY') and Exchange = 'LME' ">
                  <xsl:value-of select="Symbol"/>
                </xsl:when>
                <xsl:otherwise>-->
                  <xsl:value-of select="BBCode"/>
                <!--</xsl:otherwise>
              </xsl:choose>-->
            </SecurityID>

            <ISIN>
              <xsl:value-of select="''"/>
            </ISIN>

            <CUSIP>
              <xsl:value-of select="''"/>
            </CUSIP>

            <SEDOL>
              <xsl:value-of select="''"/>
            </SEDOL>

            <Bloomberg>
              <xsl:value-of select="''"/>
            </Bloomberg>

            <CINS>
              <xsl:value-of select="''"/>
            </CINS>

            <WhenIssued>
              <xsl:value-of select="''"/>
            </WhenIssued>

            <IssueDate>
              <xsl:value-of select="''"/>
            </IssueDate>

            <Maturity>
              <xsl:value-of select="''"/>
            </Maturity>

            <Coupon>
              <xsl:value-of select="''"/>
            </Coupon>

            <ExecutionInterestDays>
              <xsl:value-of select="''"/>
            </ExecutionInterestDays>

            <AccruedInterest>
              <xsl:value-of select="''"/>
            </AccruedInterest>

            <FaceValue>
              <xsl:value-of select="''"/>
            </FaceValue>

            <RollableType>
              <xsl:value-of select="''"/>
            </RollableType>

            <RepoLoanCurrency>
              <xsl:value-of select="''"/>
            </RepoLoanCurrency>

            <DayCountBase>
              <xsl:value-of select="''"/>
            </DayCountBase>

            <RepoLoanAmount>
              <xsl:value-of select="''"/>
            </RepoLoanAmount>

            <Trader>
              <xsl:value-of select="'Citrine Trader'"/>
            </Trader>

            <OrderQty>
              <xsl:value-of select="AllocatedQty"/>
            </OrderQty>

            <FillQty>
              <xsl:value-of select="AllocatedQty"/>
            </FillQty>

            <CumQty>
              <xsl:value-of select="''"/>
            </CumQty>

            <HairCut>
              <xsl:value-of select="''"/>
            </HairCut>

            <AvgPx>
              <xsl:value-of select="format-number(AveragePrice,'#.000000')"/>
            </AvgPx>

            <FillPrice>
              <xsl:value-of select="format-number(AveragePrice,'#.000000')"/>
            </FillPrice>

            <TradeDate>
              <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
            </TradeDate>

            <TradeTime>
              <xsl:value-of select="''"/>
            </TradeTime>

            <OrigDate>
              <xsl:value-of select="''"/>
            </OrigDate>

            <Unused>
              <xsl:value-of select="''"/>
            </Unused>

            <SettlementDate>
              <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
            </SettlementDate>

            <ExecutingUser>
              <xsl:value-of select="''"/>
            </ExecutingUser>

            <Comment>
              <xsl:value-of select="''"/>
            </Comment>

            <Account>
              <xsl:value-of select="''"/>
            </Account>

            <Fund>
              <xsl:value-of select="AccountName"/>
            </Fund>

            <SubFund>
              <xsl:value-of select="''"/>
            </SubFund>

            <AllocationCode>
              <xsl:value-of select="''"/>
            </AllocationCode>

            <StrategyCode>
              <xsl:value-of select="''"/>
            </StrategyCode>

            <ExecutionBroker>
              <xsl:value-of select="CounterParty"/>
            </ExecutionBroker>

            <ClearingBroker>
              <xsl:value-of select="'JPM'"/>
            </ClearingBroker>

            <ContractSize>
              <xsl:value-of select="''"/>
            </ContractSize>

            <Commission>
              <xsl:value-of select="''"/>
            </Commission>

            <SpotFXRate>
              <xsl:value-of select="''"/>
            </SpotFXRate>

            <FWDFXpoints>
              <xsl:value-of select="''"/>
            </FWDFXpoints>

            <Fee>
              <xsl:value-of select="''"/>
            </Fee>

            <CurrencyTraded>
              <xsl:value-of select="''"/>
            </CurrencyTraded>

            <SettleCurrency>
              <xsl:value-of select="'USD'"/>
            </SettleCurrency>

            <FXBASErate>
              <xsl:value-of select="''"/>
            </FXBASErate>

            <BASEFXrate>
              <xsl:value-of select="''"/>
            </BASEFXrate>

            <StrikePrice>
              <xsl:value-of select="''"/>
            </StrikePrice>

            <PutOrCall>
              <xsl:value-of select="''"/>
            </PutOrCall>

            <DerivativeExpiry>
              <xsl:value-of select="''"/>
            </DerivativeExpiry>

            <SubStrategy>
              <xsl:value-of select="''"/>
            </SubStrategy>

            <OrderGroup>
              <xsl:value-of select="''"/>
            </OrderGroup>

            <RepoPenalty>
              <xsl:value-of select="''"/>
            </RepoPenalty>

            <CommissionTurn>
              <xsl:value-of select="''"/>
            </CommissionTurn>

            <AllocRule>
              <xsl:value-of select="''"/>
            </AllocRule>

            <PaymentFreq>
              <xsl:value-of select="''"/>
            </PaymentFreq>

            <RateSource>
              <xsl:value-of select="''"/>
            </RateSource>

            <Spread>
              <xsl:value-of select="''"/>
            </Spread>

            <CurrentFace>
              <xsl:value-of select="''"/>
            </CurrentFace>

            <CurrentPrincipalFactor>
              <xsl:value-of select="''"/>
            </CurrentPrincipalFactor>

            <AccrualFactor>
              <xsl:value-of select="''"/>
            </AccrualFactor>

            <TaxRate>
              <xsl:value-of select="''"/>
            </TaxRate>

            <Expenses>
              <xsl:value-of select="''"/>
            </Expenses>

            <Fees>
              <xsl:value-of select="''"/>
            </Fees>

            <NetConsideration>
              <xsl:value-of select="''"/>
            </NetConsideration>

            <ImpliedCommissionFlag>
              <xsl:value-of select="''"/>
            </ImpliedCommissionFlag>

            <TransactionType>
              <xsl:value-of select="''"/>
            </TransactionType>

            <MasterConfrimType>
              <xsl:value-of select="''"/>
            </MasterConfrimType>

            <MatrixTerm>
              <xsl:value-of select="''"/>
            </MatrixTerm>

            <EMInternalSeqNo>
              <xsl:value-of select="''"/>
            </EMInternalSeqNo>

            <ObjectivePrice>
              <xsl:value-of select="''"/>
            </ObjectivePrice>

            <MarketPrice>
              <xsl:value-of select="''"/>
            </MarketPrice>

            <StopPrice>
              <xsl:value-of select="''"/>
            </StopPrice>

            <NetConsdieration>
              <xsl:value-of select="''"/>
            </NetConsdieration>

            <FixingDate>
              <xsl:value-of select="''"/>
            </FixingDate>

            <DeliveryInstructions>
              <xsl:value-of select="''"/>
            </DeliveryInstructions>

            <ForceMatchID>
              <xsl:value-of select="''"/>
            </ForceMatchID>

            <ForceMatchType>
              <xsl:value-of select="''"/>
            </ForceMatchType>

            <ForceMatchNotes>
              <xsl:value-of select="''"/>
            </ForceMatchNotes>

            <CommissionRateAllocation>
              <xsl:value-of select="''"/>
            </CommissionRateAllocation>

            <CommissionAmountFill>
              <xsl:value-of select="''"/>
            </CommissionAmountFill>

            <ExpenseAmountforFill>
              <xsl:value-of select="''"/>
            </ExpenseAmountforFill>

            <FeeAmountforFill>
              <xsl:value-of select="''"/>
            </FeeAmountforFill>

            <StandardStrategy>
              <xsl:value-of select="''"/>
            </StandardStrategy>

            <StrategyLinkName>
              <xsl:value-of select="''"/>
            </StrategyLinkName>

            <StrategyGroup>
              <xsl:value-of select="''"/>
            </StrategyGroup>

            <FillFXSettleAmount>
              <xsl:value-of select="''"/>
            </FillFXSettleAmount>


            <!-- system use only-->
            <EntityID>
              <xsl:value-of select="EntityID"/>
            </EntityID>

          </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>