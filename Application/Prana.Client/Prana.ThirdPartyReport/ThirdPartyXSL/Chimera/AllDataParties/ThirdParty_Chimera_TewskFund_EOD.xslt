<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
         <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='Stevens Capital' and CounterParty != 'CORP' and CounterParty != 'Transfer' and CounterParty != 'Transfer1' and CounterParty != 'Undefined']">
        <ThirdPartyFlatFileDetail>
          
          <RowHeader>
            <xsl:value-of select ="true"/>
          </RowHeader>

         
          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>


          <xsl:variable name="varOrdStatus">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated' or TaxLotState='Sent'">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select="'R'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select="'D'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="substring(TaxLotState,1,1)"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <OrdStatus>
            <xsl:value-of select="$varOrdStatus"/>
          </OrdStatus>

          <xsl:variable name="varExecTransType">
            <xsl:choose>
              <xsl:when test="TaxLotState='Amended' or TaxLotState='Deleted'">
                <xsl:value-of select="0"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select="2"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="2"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <ExecTransType>
            <xsl:value-of select="$varExecTransType"/>
          </ExecTransType>

          <ClientOrderID>
            <xsl:value-of select="concat(TradeRefID,'CHIM')"/>
          </ClientOrderID>


          <FillID>
            <xsl:value-of select="concat(TradeRefID,'CHIM')"/>
          </FillID>

          <IDofOrderOrFillforAction>
            <xsl:value-of select="''"/>
          </IDofOrderOrFillforAction>

          <LotNumber>
            <xsl:value-of select="''"/>
          </LotNumber>

          <Symbol>
			  <xsl:choose>
				  <xsl:when test="Asset='EquityOption'">
					  <xsl:value-of select="OSIOptionSymbol"/>				  
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="Symbol"/>
				  </xsl:otherwise>
			  </xsl:choose>

		  </Symbol>

          <xsl:variable name="varSecurityType">
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'CS'"/>
              </xsl:when>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'OPT'"/>
              </xsl:when>
              <xsl:when test="Asset='Future' ">
                <xsl:value-of select="'FUT'"/>
              </xsl:when>
              <xsl:when test="Asset='FutureOption' ">
                <xsl:value-of select="'IDXFUTOPT'"/>
              </xsl:when>
              <xsl:when test="Asset='FXForward'">
                <xsl:value-of select="'FWDFX'"/>
              </xsl:when>
              <xsl:when test="Asset='FixedIncome'">
                <xsl:value-of select="'COP'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Asset"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <SecurityType>
            <xsl:value-of select="$varSecurityType"/>
          </SecurityType>

          <SecurityCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </SecurityCurrency>

          <SecurityDescription>
            <xsl:value-of select="FullSecurityName"/>
          </SecurityDescription>


         
          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <BuySellShortCover>
            <xsl:value-of select="$varSide"/>
          </BuySellShortCover>

          <OpenClose>
            <xsl:value-of select="''"/>
          </OpenClose>
          <xsl:choose>
            
            <xsl:when test ="Asset='EquityOption'">
              <IDSource>
                <xsl:value-of select="'OCS'"/>
              </IDSource>
            </xsl:when>
			  <xsl:when test ="SEDOL !=''">
				  <IDSource>
					  <xsl:value-of select="'SEDOL'"/>
				  </IDSource>
			  </xsl:when>
			  <xsl:when test ="CUSIP != ''">
				  <IDSource>
					  <xsl:value-of select="'CUSIP'"/>
				  </IDSource>
			  </xsl:when>
            <xsl:when test ="ISIN != ''">
              <IDSource>
                <xsl:value-of select="'ISIN'"/>
              </IDSource>
            </xsl:when>
			  <xsl:when test ="BBCode != ''">
				  <IDSource>
					  <xsl:value-of select="'BLOOMBERG'"/>
				  </IDSource>
			  </xsl:when>
            <xsl:when test ="Asset = 'FutureOption' or Asset = 'Future'">
              <IDSource>
                <xsl:value-of select="'BLOOMBERG'"/>
              </IDSource>
            </xsl:when>
            <xsl:otherwise>
              <IDSource>
                <xsl:value-of select="'TICKER'"/>
              </IDSource>
            </xsl:otherwise>
          </xsl:choose>

          <SecurityID>
			  <xsl:choose>
				  <xsl:when test="Asset='EquityOption'">
					  <xsl:value-of select="OSIOptionSymbol"/>
				  </xsl:when>

				  <xsl:when test="SEDOL !=''">
					  <xsl:value-of select="SEDOL"/>
				  </xsl:when>

				  <xsl:when test="CUSIP!=''">
					  <xsl:value-of select="CUSIP"/>
				  </xsl:when>

				  <xsl:when test="ISIN!=''">
					  <xsl:value-of select="ISIN"/>
				  </xsl:when>

				  <xsl:when test="BBCode !=''">
					  <xsl:value-of select="BBCode"/>
				  </xsl:when>
				  
				  <xsl:otherwise>
					  <xsl:value-of select="Symbol"/>
				  </xsl:otherwise>
			  </xsl:choose>
          </SecurityID>

          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

          <SEDOL>
            <xsl:value-of select="SEDOL"/>
          </SEDOL>

          <Bloomberg>
            <xsl:value-of select="BBCode"/>
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

          <xsl:variable name="varMaturity">
            <xsl:choose>
              <xsl:when test="ExpirationDate='01/01/1800'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="concat(substring-after(substring-after(ExpirationDate,'/'),'/'),substring-before(ExpirationDate,'/'),substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
               
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Maturity>
            <xsl:value-of select="$varMaturity"/>
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
            <xsl:value-of select="'CHIM'"/>
          </Trader>

          <OrderQty>
            <xsl:value-of select="AllocatedQty"/>
          </OrderQty>


          <FillQty>
            <xsl:value-of select="''"/>
          </FillQty>


          <CumQty>
            <xsl:value-of select="''"/>
          </CumQty>

          <HairCut>
            <xsl:value-of select="''"/>
          </HairCut>


          <AvgPx>
            <xsl:value-of select="format-number(AveragePrice,'##.########')"/>
          </AvgPx>


          <FillPrice>
			  <xsl:value-of select="format-number(AveragePrice,'##.########')"/>
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
            <xsl:value-of select="'TIF'"/>
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

			<xsl:variable name="PB_NAME" select="'GSEC'"/>
			<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterPartyID"/>

			<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
				<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBrokerCode=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
			</xsl:variable>

			<xsl:variable name="varBroker">
				<xsl:choose>
					<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
						<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
          <ExecutionBroker>
            <xsl:value-of select="$varBroker"/>
          </ExecutionBroker>

          <ClearingBroker>
            <xsl:value-of select="'GS'"/>
          </ClearingBroker>

          <ContractSize>
            <xsl:value-of select="''"/>
          </ContractSize>

          <xsl:variable name="varCommision">
            <xsl:choose>
              <xsl:when test="number(CommissionCharged)">
                <xsl:value-of select="CommissionCharged"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Commission>
			  <xsl:value-of select="format-number($varCommision,'##.##')"/>
          </Commission>

          <SpotFXRate>
            <xsl:value-of select="ForexRate_Trade"/>
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
            <xsl:value-of select="CurrencySymbol"/>
          </SettleCurrency>

          <FXBASErate>
            <xsl:value-of select="''"/>
          </FXBASErate>

          <BASEFXrate>
            <xsl:value-of select="''"/>
          </BASEFXrate>

          <StrikePrice>
            <xsl:value-of select="StrikePrice"/>
          </StrikePrice>

          <PutOrCall>
            <xsl:value-of select="PutOrCall"/>
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

          <xsl:variable name="varFees">
            <xsl:value-of select="StampDuty + MiscFees + ClearingFee + OtherBrokerFee + TaxOnCommissions + TransactionLevy"/>
          </xsl:variable>

          <Fees>
            <xsl:value-of select="$varFees"/>
          </Fees>

          <PostCommAndFeesOnInit>
            <xsl:value-of select="''"/>
          </PostCommAndFeesOnInit>


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

          <InitialMargin>
            <xsl:value-of select="''"/>
          </InitialMargin>

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


          <CommissionRateforAllocation>
            <xsl:value-of select="''"/>
          </CommissionRateforAllocation>


          <CommissionAmountforFill>
            <xsl:value-of select="''"/>
          </CommissionAmountforFill>


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

			<Reserved>
				<xsl:value-of select="''"/>
			</Reserved>

			<Reserved1>
				<xsl:value-of select="''"/>
			</Reserved1>

			<DealAttributes>
				<xsl:value-of select="''"/>
			</DealAttributes>

			<FinanceLeg>
				<xsl:value-of select="''"/>
			</FinanceLeg>

			<PerformanceLeg>
				<xsl:value-of select="''"/>
			</PerformanceLeg>

			<Attributes>
				<xsl:value-of select="''"/>
			</Attributes>

			<DealSymbol>
				<xsl:value-of select="''"/>
			</DealSymbol>
			<Initialmargintype>
				<xsl:value-of select="''"/>
			</Initialmargintype>

			<InitialMarginAmount>
				<xsl:value-of select="''"/>
			</InitialMarginAmount>
			<InitialmarginCCY>
				<xsl:value-of select="''"/>
			</InitialmarginCCY>

			<ConfirmStatus>
				<xsl:value-of select="''"/>
			</ConfirmStatus>

			<Counterparty>
				<xsl:value-of select="''"/>
			</Counterparty>

			<TraderNotes>
				<xsl:value-of select="''"/>
			</TraderNotes>

			<ConvertPricetoSettleCcy>
				<xsl:value-of select="''"/>
			</ConvertPricetoSettleCcy>

			<BondCouponType>
				<xsl:value-of select="''"/>
			</BondCouponType>

			<GenericFeesEnabled>
				<xsl:value-of select="''"/>
			</GenericFeesEnabled>

			<GenericFeesListing>
				<xsl:value-of select="''"/>
			</GenericFeesListing>

			<OrderLevelAttributes>
				<xsl:value-of select="''"/>
			</OrderLevelAttributes>

			<SettlingSub>
				<xsl:value-of select="''"/>
			</SettlingSub>

			<ConfirmationTime>
				<xsl:value-of select="''"/>
			</ConfirmationTime>
			<ConfirmationMeans>
				<xsl:value-of select="''"/>
			</ConfirmationMeans>
			<PaymentDate>
				<xsl:value-of select="''"/>
			</PaymentDate>


			<EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
