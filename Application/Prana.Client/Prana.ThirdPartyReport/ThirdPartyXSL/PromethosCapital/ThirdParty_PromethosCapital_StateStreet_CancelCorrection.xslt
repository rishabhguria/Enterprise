<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month=1">
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month=2">
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month=3">
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month=4">
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month=5">
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month=6">
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month=7">
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month=8">
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month=9">
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template name="DateFormatTrans">
    <xsl:param name="TDate"/>
    <xsl:value-of select="concat(substring-before($TDate,'-'),substring-before(substring-after($TDate,'-'),'-'),substring-after(substring-after($TDate,'-'),'-'))"/>
  </xsl:template>
  
  
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>


      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <xsl:variable name="varNetamount">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test ="TaxLotState!='Amemded'">
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'True'"/>
              </RowHeader>

              <TaxLotState>
                <xsl:value-of select ="TaxLotState"/>
              </TaxLotState>



              <TransactionType>
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Open' or Side = 'Buy to Close'">
                    <xsl:value-of select="'BUY'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell to Close' or Side = 'Sell short' or Side = 'Sell to Open'">
                    <xsl:value-of select="'SELL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TransactionType>

              <MessageFunction>              
                    <xsl:value-of select="'NEWM'"/>                
              </MessageFunction>

              <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              

              <xsl:variable name="varTransactionReferenceDate">
                <xsl:value-of select="concat(substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'),substring-after(substring-after($varTradeDate,'/'),'/'))"/>
              </xsl:variable>
              <TransactionReference>
                <xsl:value-of select="concat(translate($varTransactionReferenceDate,'/',''),'-', position())"/>
              </TransactionReference>

              <RelatedReferenceNumber>
                <xsl:value-of select="''"/>
              </RelatedReferenceNumber>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'StateStreet'"/>
              </xsl:variable>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>

              <FundID>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE != ''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="FundAccntNo"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundID>
            
          
              <TradeDate>
                <xsl:value-of select="$varTradeDate"/>
              </TradeDate>
              
              <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettlementDate>
                <xsl:value-of select="$varSettlementDate"/>
              </SettlementDate>

              <LateDeliveryDate>
                <xsl:value-of select="''"/>
              </LateDeliveryDate>

              <!--<SecurityIDType>
            <xsl:choose>
              <xsl:when test="CurrencySymbol ='USD'">
                <xsl:value-of select="'US'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol !='USD'">
                <xsl:value-of select="'ISIN'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
           
          </SecurityIDType>-->

              <SecurityIDType>
                <xsl:value-of select="'GB'"/>
              </SecurityIDType>

              <SecurityID>
                <xsl:choose>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SecurityID>

              <SecurityDescription>
                <xsl:value-of select="CompanyName"/>
              </SecurityDescription>

              <SecurityType>
                <xsl:value-of select="'CS'"/>
              </SecurityType>

              <CurrencyOfDenomination>
                <xsl:value-of select="''"/>
              </CurrencyOfDenomination>

              <OptionStyle>
                <xsl:value-of select="''"/>
              </OptionStyle>

              <OptionType>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="PutOrCall"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OptionType>

              <ContractSize>
                <xsl:value-of select="''"/>
              </ContractSize>

              <StrikePrice>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="format-number(StrikePrice,'#.000000')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </StrikePrice>


              <ExpirationDate>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="ExpirationDate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ExpirationDate>

              <UnderlyingSecurityIDType>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityIDType>

              <UnderlyingSecurityID>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityID>

              <UnderlyingSecurityDesc>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityDesc>

              <MaturityDate>
                <xsl:value-of select="''"/>
              </MaturityDate>

              <IssueDate>
                <xsl:value-of select="''"/>
              </IssueDate>

              <InterestRate>
                <xsl:value-of select="''"/>
              </InterestRate>

              <OriginalFace>
                <xsl:value-of select="''"/>
              </OriginalFace>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="number(OrderQty)">
                    <xsl:value-of select="format-number(OrderQty,'#.00')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <TradeCurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </TradeCurrency>

              <DealPriceCode>
                <xsl:value-of select="'ACTU'"/>
              </DealPriceCode>


              <DealPrice>
                <xsl:value-of select="format-number(AvgPrice,'#.000000')"/>
              </DealPrice>

              <xsl:variable name="varAssetMultiplier">
                <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier)"/>
              </xsl:variable>

              <PrincipalAmount>
                <xsl:value-of select="format-number($varAssetMultiplier,'#.00')"/>
              </PrincipalAmount>

              <CommissionsAmount>
                <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'#.##')"/>
              </CommissionsAmount>

              <ChargesFeesAmount>
                <xsl:value-of select="''"/>
              </ChargesFeesAmount>

              <OtherAmount>
                <xsl:value-of select="''"/>
              </OtherAmount>

              <AccruedInterestAmount>
                <xsl:value-of select="''"/>
              </AccruedInterestAmount>

              <TaxesAmount>
                <xsl:value-of select="format-number(TaxOnCommissions,'#.##')"/>
              </TaxesAmount>

              <StampDutyExemptionAmount>
                <xsl:value-of select="''"/>
              </StampDutyExemptionAmount>

              <SettlementCurrency>
                <xsl:value-of select="SettlCurrency"/>
              </SettlementCurrency>

              <SettlementAmount>
                <xsl:value-of select="format-number($varNetamount,'#.00')"/>
              </SettlementAmount>

              <TransactionSubType>
                <xsl:value-of select="'TRAD'"/>
              </TransactionSubType>

              <SettlementTransactionConditionIndicator>
                <xsl:value-of select="''"/>
              </SettlementTransactionConditionIndicator>

              <SettlementTransactionConditionIndicator2>
                <xsl:value-of select="''"/>
              </SettlementTransactionConditionIndicator2>

              <ProcessingIndicator>
                <xsl:value-of select="''"/>
              </ProcessingIndicator>

              <TrackingIndicator>
                <xsl:value-of select="''"/>
              </TrackingIndicator>

              <xsl:variable name="PRANA_UDACountryName" select="UDACountryName"/>
              <xsl:variable name="PRANA_COUNTERPARTY_Name" select="CounterParty"/>


              <xsl:variable name="THIRDPARTY_PSET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@PSET"/>
              </xsl:variable>

              <SettlementLocation>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementLocation>


              <xsl:variable name="THIRDPARTY_MARKET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@Market"/>
              </xsl:variable>


              <PlaceOfTrade>
                
                    <xsl:value-of select="''"/>
                 
              </PlaceOfTrade>

              <PlaceOfSafekeeping>
                <xsl:value-of select="''"/>
              </PlaceOfSafekeeping>

              <FXContraCurrency>
				<xsl:choose>
                  <xsl:when test="FundAccntNo = 'HIBI'">
                    <xsl:value-of select="'USD'"/>
                  </xsl:when>
				  <xsl:when test="FundAccntNo = 'M4QZ - SS'">
                    <xsl:value-of select="'USD'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FXContraCurrency>

              <FXOrderCXLIndicator>
                <xsl:value-of select="''"/>
              </FXOrderCXLIndicator>

              <xsl:variable name="PRANA_SettlementInstruction_NAME" select="UDACountryName"/>


              <xsl:variable name="THIRDPARTY_COUNTERPARTY_TYPE">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerIDType"/>
              </xsl:variable>
              <ExecutingBrokerIDType>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="'DTCYID'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_COUNTERPARTY_TYPE != ''">
                    <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_TYPE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ExecutingBrokerIDType>

              <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerID"/>
              </xsl:variable>

              <ExecutingBrokerID>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ExecutingBrokerID>
              <xsl:variable name="THIRDPARTY_EXECUTINGBROKERAC_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ExecutingBrokerAccount"/>
              </xsl:variable>
              <ExecutingBrokerAcct>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="'DTCYID'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_EXECUTINGBROKERAC_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_EXECUTINGBROKERAC_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ExecutingBrokerAcct>

              <xsl:variable name="THIRDPARTY_ClearerIDType_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ClearerIDType"/>
              </xsl:variable>
              <ClearingBrokerAgentIDType>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_ClearerIDType_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_ClearerIDType_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ClearingBrokerAgentIDType>

              <xsl:variable name="THIRDPARTY_ClearerID_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ClearerID"/>
              </xsl:variable>
              <ClearingBrokerAgentID>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_ClearerID_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_ClearerID_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ClearingBrokerAgentID>

              <ExposureTypeIndicator>
                <xsl:value-of select="''"/>
              </ExposureTypeIndicator>

              <NetMovementIndicator>
                <xsl:value-of select="''"/>
              </NetMovementIndicator>

              <NetMovementAmount>
                <xsl:value-of select="''"/>
              </NetMovementAmount>

              <IntermediaryIDType>
                <xsl:value-of select="''"/>
              </IntermediaryIDType>

              <IntermediaryID>
                <xsl:value-of select="''"/>
              </IntermediaryID>

              <AcctWithInstitutionIDType>
                <xsl:value-of select="''"/>
              </AcctWithInstitutionIDType>

              <AcctWithInstitutionID>
                <xsl:value-of select="''"/>
              </AcctWithInstitutionID>

              <PayingInstitution>
                <xsl:value-of select="''"/>
              </PayingInstitution>

              <BeneficiaryOfMoney>
                <xsl:value-of select="''"/>
              </BeneficiaryOfMoney>

              <CashAcct>
                <xsl:value-of select="''"/>
              </CashAcct>

              <CBO>
                <xsl:value-of select="''"/>
              </CBO>

              <StampDutyExemption>
                <xsl:value-of select="''"/>
              </StampDutyExemption>

              <StampCode>
                <xsl:value-of select="''"/>
              </StampCode>

              <TRADDETNarrative>
                <xsl:value-of select="''"/>
              </TRADDETNarrative>

              <FIANarrative>
                <xsl:value-of select="''"/>
              </FIANarrative>

              <Processing>
                <xsl:value-of select="''"/>
              </Processing>

              <Reference>
                <xsl:value-of select="''"/>
              </Reference>

              <ClearingBrokerAccount>
                <xsl:value-of select="''"/>
              </ClearingBrokerAccount>

              <Restrictions>
                <xsl:value-of select="''"/>
              </Restrictions>

              <RepoTermOpenInd>
                <xsl:value-of select="''"/>
              </RepoTermOpenInd>

              <RepoTermDate>
                <xsl:value-of select="''"/>
              </RepoTermDate>

              <RepoRateType>
                <xsl:value-of select="''"/>
              </RepoRateType>

              <RepoRate>
                <xsl:value-of select="''"/>
              </RepoRate>

              <RepoReference>
                <xsl:value-of select="''"/>
              </RepoReference>

              <RepoTotalTermAmt>
                <xsl:value-of select="''"/>
              </RepoTotalTermAmt>

              <RepoAccrueAmt>
                <xsl:value-of select="''"/>
              </RepoAccrueAmt>

              <RepoTotalCollCnt>
                <xsl:value-of select="''"/>
              </RepoTotalCollCnt>

              <RepoCollNumb>
                <xsl:value-of select="''"/>
              </RepoCollNumb>

              <RepoTypeInd>
                <xsl:value-of select="''"/>
              </RepoTypeInd>


              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>
            </ThirdPartyFlatFileDetail>
          </xsl:when>

          <xsl:otherwise>
            <xsl:if test ="number(OldExecutedQuantity)">
              <ThirdPartyFlatFileDetail>

                <RowHeader>
                  <xsl:value-of select ="'True'"/>
                </RowHeader>

                <TaxLotState>
                  <xsl:value-of select="'Deleted'"/>
                </TaxLotState>



                <TransactionType>
                  <xsl:choose>
                    <xsl:when test="OldSide='Buy' or OldSide='Buy to Open' or OldSide = 'Buy to Close'">
                      <xsl:value-of select="'BUY'"/>
                    </xsl:when>
                    <xsl:when test="OldSide='Sell' or OldSide='Sell to Close' or OldSide = 'Sell short' or OldSide = 'Sell to Open'">
                      <xsl:value-of select="'SELL'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </TransactionType>

                <MessageFunction>
                  <xsl:value-of select="'CANC'"/>
                </MessageFunction>

                <xsl:variable name="varOldTradeDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldTradeDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>


                <xsl:variable name="varTransactionReferenceDate">
                  <xsl:value-of select="concat(substring-before($varOldTradeDate,'/'),substring-before(substring-after($varOldTradeDate,'/'),'/'),substring-after(substring-after($varOldTradeDate,'/'),'/'))"/>
                </xsl:variable>
                <TransactionReference>
                  <xsl:value-of select="concat(translate($varTransactionReferenceDate,'/',''),'-', position(),'C')"/>
                </TransactionReference>

                <RelatedReferenceNumber>
                  <xsl:value-of select="concat(translate($varTransactionReferenceDate,'/',''),'-', position())"/>
                </RelatedReferenceNumber>

                <xsl:variable name="PB_NAME">
                  <xsl:value-of select="'StateStreet'"/>
                </xsl:variable>

                <xsl:variable name = "PRANA_FUND_NAME">
                  <xsl:value-of select="AccountName"/>
                </xsl:variable>

                <xsl:variable name ="THIRDPARTY_FUND_CODE">
                  <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
                </xsl:variable>

                <FundID>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_FUND_CODE != ''">
                      <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="FundAccntNo"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </FundID>
               
                <TradeDate>
                  <xsl:value-of select="$varOldTradeDate"/>
                </TradeDate>
                
                <xsl:variable name="varOldSettlementDate">
                  <xsl:call-template name="DateFormat">
                    <xsl:with-param name="Date" select="OldSettlementDate">
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:variable>
                <SettlementDate>
                  <xsl:value-of select="$varOldSettlementDate"/>
                </SettlementDate>

                <LateDeliveryDate>
                  <xsl:value-of select="''"/>
                </LateDeliveryDate>

                <!--<SecurityIDType>
            <xsl:choose>
              <xsl:when test="CurrencySymbol ='USD'">
                <xsl:value-of select="'US'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol !='USD'">
                <xsl:value-of select="'ISIN'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
           
          </SecurityIDType>-->

                <SecurityIDType>
                  <xsl:value-of select="'GB'"/>
                </SecurityIDType>

                <SecurityID>
                  <xsl:choose>
                    <xsl:when test="SEDOL != ''">
                      <xsl:value-of select="SEDOL"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="Symbol"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SecurityID>

                <SecurityDescription>
                  <xsl:value-of select="CompanyName"/>
                </SecurityDescription>

                <SecurityType>
                  <xsl:value-of select="'CS'"/>
                </SecurityType>

                <CurrencyOfDenomination>
                  <xsl:value-of select="''"/>
                </CurrencyOfDenomination>

                <OptionStyle>
                  <xsl:value-of select="''"/>
                </OptionStyle>

                <OptionType>
                  <xsl:choose>
                    <xsl:when test="Asset='EquityOption'">
                      <xsl:value-of select="PutOrCall"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </OptionType>

                <ContractSize>
                  <xsl:value-of select="''"/>
                </ContractSize>

                <StrikePrice>
                  <xsl:choose>
                    <xsl:when test="Asset='EquityOption'">
                      <xsl:value-of select="format-number(StrikePrice,'#.000000')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </StrikePrice>


                <ExpirationDate>
                  <xsl:choose>
                    <xsl:when test="Asset='EquityOption'">
                      <xsl:value-of select="ExpirationDate"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExpirationDate>

                <UnderlyingSecurityIDType>
                  <xsl:value-of select="''"/>
                </UnderlyingSecurityIDType>

                <UnderlyingSecurityID>
                  <xsl:value-of select="''"/>
                </UnderlyingSecurityID>

                <UnderlyingSecurityDesc>
                  <xsl:value-of select="''"/>
                </UnderlyingSecurityDesc>

                <MaturityDate>
                  <xsl:value-of select="''"/>
                </MaturityDate>

                <IssueDate>
                  <xsl:value-of select="''"/>
                </IssueDate>

                <InterestRate>
                  <xsl:value-of select="''"/>
                </InterestRate>

                <OriginalFace>
                  <xsl:value-of select="''"/>
                </OriginalFace>

                <Quantity>
                  <xsl:choose>
                    <xsl:when test="number(OldExecutedQuantity)">
                      <xsl:value-of select="format-number(OldExecutedQuantity,'#.00')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Quantity>

                <TradeCurrency>
                  <xsl:value-of select="CurrencySymbol"/>
                </TradeCurrency>

                <DealPriceCode>
                  <xsl:value-of select="'ACTU'"/>
                </DealPriceCode>


                <DealPrice>
                  <xsl:value-of select="format-number(OldAvgPrice,'#.000000')"/>
                </DealPrice>

                <xsl:variable name="varAssetMultiplier">
                  <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier)"/>
                </xsl:variable>

                <PrincipalAmount>
                  <xsl:value-of select="format-number($varAssetMultiplier,'#.00')"/>
                </PrincipalAmount>

                <CommissionsAmount>
                  <xsl:value-of select="format-number(OldCommission + OldSoftCommission,'#.##')"/>
                </CommissionsAmount>

                <ChargesFeesAmount>
                  <xsl:value-of select="''"/>
                </ChargesFeesAmount>

                <OtherAmount>
                  <xsl:value-of select="''"/>
                </OtherAmount>

                <AccruedInterestAmount>
                  <xsl:value-of select="''"/>
                </AccruedInterestAmount>

                <TaxesAmount>
                  <xsl:value-of select="format-number(OldTaxOnCommissions,'#.##')"/>
                </TaxesAmount>

                <StampDutyExemptionAmount>
                  <xsl:value-of select="''"/>
                </StampDutyExemptionAmount>

                <SettlementCurrency>
                  <xsl:value-of select="OldSettlCurrency"/>
                </SettlementCurrency>
                
                <xsl:variable name="varOldNetAmount">
                  <xsl:choose>
                    <xsl:when test="contains(OldSide,'Buy')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee"/>
                    </xsl:when>
                    <xsl:when test="contains(OldSide,'Sell')">
                      <xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>
                <SettlementAmount>
                  <xsl:value-of select="format-number($varOldNetAmount,'#.00')"/>
                </SettlementAmount>

                <TransactionSubType>
                  <xsl:value-of select="'TRAD'"/>
                </TransactionSubType>

                <SettlementTransactionConditionIndicator>
                  <xsl:value-of select="''"/>
                </SettlementTransactionConditionIndicator>

                <SettlementTransactionConditionIndicator2>
                  <xsl:value-of select="''"/>
                </SettlementTransactionConditionIndicator2>

                <ProcessingIndicator>
                  <xsl:value-of select="''"/>
                </ProcessingIndicator>

                <TrackingIndicator>
                  <xsl:value-of select="''"/>
                </TrackingIndicator>

                <xsl:variable name="PRANA_UDACountryName" select="UDACountryName"/>
                <xsl:variable name="PRANA_COUNTERPARTY_Name" select="CounterParty"/>


                <xsl:variable name="THIRDPARTY_PSET_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@PSET"/>
                </xsl:variable>

                <SettlementLocation>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </SettlementLocation>


                <xsl:variable name="THIRDPARTY_MARKET_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@Market"/>
                </xsl:variable>


                <PlaceOfTrade>
                  <xsl:value-of select="''"/>
                </PlaceOfTrade>

                <PlaceOfSafekeeping>
                  <xsl:value-of select="''"/>
                </PlaceOfSafekeeping>

                <FXContraCurrency>
				<xsl:choose>
                  <xsl:when test="FundAccntNo = 'HIBI'">
                    <xsl:value-of select="'USD'"/>
                  </xsl:when>
				  <xsl:when test="FundAccntNo = 'M4QZ - SS'">
                    <xsl:value-of select="'USD'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FXContraCurrency>

                <FXOrderCXLIndicator>
                  <xsl:value-of select="''"/>
                </FXOrderCXLIndicator>

                <xsl:variable name="PRANA_SettlementInstruction_NAME" select="UDACountryName"/>


                <xsl:variable name="THIRDPARTY_COUNTERPARTY_TYPE">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerIDType"/>
                </xsl:variable>
                <ExecutingBrokerIDType>
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol='USD'">
                      <xsl:value-of select="'DTCYID'"/>
                    </xsl:when>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_TYPE != ''">
                      <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_TYPE"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExecutingBrokerIDType>

                <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerID"/>
                </xsl:variable>

                <ExecutingBrokerID>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExecutingBrokerID>
                <xsl:variable name="THIRDPARTY_EXECUTINGBROKERAC_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ExecutingBrokerAccount"/>
                </xsl:variable>
                <ExecutingBrokerAcct>
                  <xsl:choose>
                    <xsl:when test="CurrencySymbol='USD'">
                      <xsl:value-of select="'DTCYID'"/>
                    </xsl:when>
                    <xsl:when test="$THIRDPARTY_EXECUTINGBROKERAC_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_EXECUTINGBROKERAC_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExecutingBrokerAcct>

                <xsl:variable name="THIRDPARTY_ClearerIDType_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ClearerIDType"/>
                </xsl:variable>
                <ClearingBrokerAgentIDType>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_ClearerIDType_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_ClearerIDType_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ClearingBrokerAgentIDType>

                <xsl:variable name="THIRDPARTY_ClearerID_NAME">
                  <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ClearerID"/>
                </xsl:variable>
                <ClearingBrokerAgentID>
                  <xsl:choose>
                    <xsl:when test="$THIRDPARTY_ClearerID_NAME != ''">
                      <xsl:value-of select="$THIRDPARTY_ClearerID_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </ClearingBrokerAgentID>

                <ExposureTypeIndicator>
                  <xsl:value-of select="''"/>
                </ExposureTypeIndicator>

                <NetMovementIndicator>
                  <xsl:value-of select="''"/>
                </NetMovementIndicator>

                <NetMovementAmount>
                  <xsl:value-of select="''"/>
                </NetMovementAmount>

                <IntermediaryIDType>
                  <xsl:value-of select="''"/>
                </IntermediaryIDType>

                <IntermediaryID>
                  <xsl:value-of select="''"/>
                </IntermediaryID>

                <AcctWithInstitutionIDType>
                  <xsl:value-of select="''"/>
                </AcctWithInstitutionIDType>

                <AcctWithInstitutionID>
                  <xsl:value-of select="''"/>
                </AcctWithInstitutionID>

                <PayingInstitution>
                  <xsl:value-of select="''"/>
                </PayingInstitution>

                <BeneficiaryOfMoney>
                  <xsl:value-of select="''"/>
                </BeneficiaryOfMoney>

                <CashAcct>
                  <xsl:value-of select="''"/>
                </CashAcct>

                <CBO>
                  <xsl:value-of select="''"/>
                </CBO>

                <StampDutyExemption>
                  <xsl:value-of select="''"/>
                </StampDutyExemption>

                <StampCode>
                  <xsl:value-of select="''"/>
                </StampCode>

                <TRADDETNarrative>
                  <xsl:value-of select="''"/>
                </TRADDETNarrative>

                <FIANarrative>
                  <xsl:value-of select="''"/>
                </FIANarrative>

                <Processing>
                  <xsl:value-of select="''"/>
                </Processing>

                <Reference>
                  <xsl:value-of select="''"/>
                </Reference>

                <ClearingBrokerAccount>
                  <xsl:value-of select="''"/>
                </ClearingBrokerAccount>

                <Restrictions>
                  <xsl:value-of select="''"/>
                </Restrictions>

                <RepoTermOpenInd>
                  <xsl:value-of select="''"/>
                </RepoTermOpenInd>

                <RepoTermDate>
                  <xsl:value-of select="''"/>
                </RepoTermDate>

                <RepoRateType>
                  <xsl:value-of select="''"/>
                </RepoRateType>

                <RepoRate>
                  <xsl:value-of select="''"/>
                </RepoRate>

                <RepoReference>
                  <xsl:value-of select="''"/>
                </RepoReference>

                <RepoTotalTermAmt>
                  <xsl:value-of select="''"/>
                </RepoTotalTermAmt>

                <RepoAccrueAmt>
                  <xsl:value-of select="''"/>
                </RepoAccrueAmt>

                <RepoTotalCollCnt>
                  <xsl:value-of select="''"/>
                </RepoTotalCollCnt>

                <RepoCollNumb>
                  <xsl:value-of select="''"/>
                </RepoCollNumb>

                <RepoTypeInd>
                  <xsl:value-of select="''"/>
                </RepoTypeInd>


              </ThirdPartyFlatFileDetail>
            </xsl:if>
            <ThirdPartyFlatFileDetail>

              <RowHeader>
                <xsl:value-of select ="'True'"/>
              </RowHeader>
              <TaxLotState>
                <xsl:value-of select="'Allocated'"/>
              </TaxLotState>



              <TransactionType>
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Open' or Side = 'Buy to Close'">
                    <xsl:value-of select="'BUY'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell to Close' or Side = 'Sell short' or Side = 'Sell to Open'">
                    <xsl:value-of select="'SELL'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </TransactionType>

              <MessageFunction>
                <xsl:value-of select="'NEWM'"/>
              </MessageFunction>

              <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>


              <xsl:variable name="varTransactionReferenceDate">
                <xsl:value-of select="concat(substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'),substring-after(substring-after($varTradeDate,'/'),'/'))"/>
              </xsl:variable>
              <TransactionReference>
                <xsl:value-of select="concat(translate($varTransactionReferenceDate,'/',''),'-', position())"/>
              </TransactionReference>

              <RelatedReferenceNumber>
                <xsl:value-of select="''"/>
              </RelatedReferenceNumber>

              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'StateStreet'"/>
              </xsl:variable>

              <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
              </xsl:variable>

              <xsl:variable name ="THIRDPARTY_FUND_CODE">
                <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
              </xsl:variable>

              <FundID>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_FUND_CODE != ''">
                    <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="FundAccntNo"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundID>
              
             
              <TradeDate>
                <xsl:value-of select="$varTradeDate"/>
              </TradeDate>

              <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettlementDate>
                <xsl:value-of select="$varSettlementDate"/>
              </SettlementDate>

              <LateDeliveryDate>
                <xsl:value-of select="''"/>
              </LateDeliveryDate>

              <!--<SecurityIDType>
            <xsl:choose>
              <xsl:when test="CurrencySymbol ='USD'">
                <xsl:value-of select="'US'"/>
              </xsl:when>
              <xsl:when test="CurrencySymbol !='USD'">
                <xsl:value-of select="'ISIN'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
           
          </SecurityIDType>-->

              <SecurityIDType>
                <xsl:value-of select="'GB'"/>
              </SecurityIDType>

              <SecurityID>
                <xsl:choose>
                  <xsl:when test="SEDOL != ''">
                    <xsl:value-of select="SEDOL"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SecurityID>

              <SecurityDescription>
                <xsl:value-of select="CompanyName"/>
              </SecurityDescription>

              <SecurityType>
                <xsl:value-of select="'CS'"/>
              </SecurityType>

              <CurrencyOfDenomination>
                <xsl:value-of select="''"/>
              </CurrencyOfDenomination>

              <OptionStyle>
                <xsl:value-of select="''"/>
              </OptionStyle>

              <OptionType>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="PutOrCall"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </OptionType>

              <ContractSize>
                <xsl:value-of select="''"/>
              </ContractSize>

              <StrikePrice>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="format-number(StrikePrice,'#.000000')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </StrikePrice>


              <ExpirationDate>
                <xsl:choose>
                  <xsl:when test="Asset='EquityOption'">
                    <xsl:value-of select="ExpirationDate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ExpirationDate>

              <UnderlyingSecurityIDType>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityIDType>

              <UnderlyingSecurityID>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityID>

              <UnderlyingSecurityDesc>
                <xsl:value-of select="''"/>
              </UnderlyingSecurityDesc>

              <MaturityDate>
                <xsl:value-of select="''"/>
              </MaturityDate>

              <IssueDate>
                <xsl:value-of select="''"/>
              </IssueDate>

              <InterestRate>
                <xsl:value-of select="''"/>
              </InterestRate>

              <OriginalFace>
                <xsl:value-of select="''"/>
              </OriginalFace>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="number(OrderQty)">
                    <xsl:value-of select="format-number(OrderQty,'#.00')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

              <TradeCurrency>
                <xsl:value-of select="CurrencySymbol"/>
              </TradeCurrency>

              <DealPriceCode>
                <xsl:value-of select="'ACTU'"/>
              </DealPriceCode>


              <DealPrice>
                <xsl:value-of select="format-number(AvgPrice,'#.000000')"/>
              </DealPrice>
              
              <xsl:variable name="varAssetMultiplier">
                <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier)"/>
              </xsl:variable>


              <PrincipalAmount>
                <xsl:value-of select="format-number($varAssetMultiplier,'#.00')"/>
              </PrincipalAmount>

              <CommissionsAmount>
                <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'#.##')"/>
              </CommissionsAmount>

              <ChargesFeesAmount>
                <xsl:value-of select="''"/>
              </ChargesFeesAmount>

              <OtherAmount>
                <xsl:value-of select="''"/>
              </OtherAmount>

              <AccruedInterestAmount>
                <xsl:value-of select="''"/>
              </AccruedInterestAmount>

              <TaxesAmount>
                <xsl:value-of select="format-number(TaxOnCommissions,'#.##')"/>
              </TaxesAmount>

              <StampDutyExemptionAmount>
                <xsl:value-of select="''"/>
              </StampDutyExemptionAmount>

              <SettlementCurrency>
                <xsl:value-of select="SettlCurrency"/>
              </SettlementCurrency>

              <SettlementAmount>
                <xsl:value-of select="format-number($varNetamount,'#.00')"/>
              </SettlementAmount>

              <TransactionSubType>
                <xsl:value-of select="'TRAD'"/>
              </TransactionSubType>

              <SettlementTransactionConditionIndicator>
                <xsl:value-of select="''"/>
              </SettlementTransactionConditionIndicator>

              <SettlementTransactionConditionIndicator2>
                <xsl:value-of select="''"/>
              </SettlementTransactionConditionIndicator2>

              <ProcessingIndicator>
                <xsl:value-of select="''"/>
              </ProcessingIndicator>

              <TrackingIndicator>
                <xsl:value-of select="''"/>
              </TrackingIndicator>

              <xsl:variable name="PRANA_UDACountryName" select="UDACountryName"/>
              <xsl:variable name="PRANA_COUNTERPARTY_Name" select="CounterParty"/>


              <xsl:variable name="THIRDPARTY_PSET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@PSET"/>
              </xsl:variable>

              <SettlementLocation>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_PSET_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_PSET_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </SettlementLocation>


              <xsl:variable name="THIRDPARTY_MARKET_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@Market"/>
              </xsl:variable>


              <PlaceOfTrade>
                <xsl:value-of select="''"/>
              </PlaceOfTrade>

              <PlaceOfSafekeeping>
                <xsl:value-of select="''"/>
              </PlaceOfSafekeeping>

              <FXContraCurrency>
				<xsl:choose>
                  <xsl:when test="FundAccntNo = 'HIBI'">
                    <xsl:value-of select="'USD'"/>
                  </xsl:when>
				  <xsl:when test="FundAccntNo = 'M4QZ - SS'">
                    <xsl:value-of select="'USD'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FXContraCurrency>

              <FXOrderCXLIndicator>
                <xsl:value-of select="''"/>
              </FXOrderCXLIndicator>

              <xsl:variable name="PRANA_SettlementInstruction_NAME" select="UDACountryName"/>


              <xsl:variable name="THIRDPARTY_COUNTERPARTY_TYPE">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerIDType"/>
              </xsl:variable>
              <ExecutingBrokerIDType>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="'DTCYID'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_COUNTERPARTY_TYPE != ''">
                    <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_TYPE"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ExecutingBrokerIDType>

              <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@BrokerID"/>
              </xsl:variable>

              <ExecutingBrokerID>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ExecutingBrokerID>
              <xsl:variable name="THIRDPARTY_EXECUTINGBROKERAC_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ExecutingBrokerAccount"/>
              </xsl:variable>
              <ExecutingBrokerAcct>
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='USD'">
                    <xsl:value-of select="'DTCYID'"/>
                  </xsl:when>
                  <xsl:when test="$THIRDPARTY_EXECUTINGBROKERAC_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_EXECUTINGBROKERAC_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ExecutingBrokerAcct>

              <xsl:variable name="THIRDPARTY_ClearerIDType_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ClearerIDType"/>
              </xsl:variable>
              <ClearingBrokerAgentIDType>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_ClearerIDType_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_ClearerIDType_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ClearingBrokerAgentIDType>

              <xsl:variable name="THIRDPARTY_ClearerID_NAME">
                <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Promethis_Broker_Mapping.xml')/InstructionMapping/PB[@Name=$PB_NAME]/InstructionData[@UDACountryName=$PRANA_UDACountryName and @CounterPartyName=$PRANA_COUNTERPARTY_Name]/@ClearerID"/>
              </xsl:variable>
              <ClearingBrokerAgentID>
                <xsl:choose>
                  <xsl:when test="$THIRDPARTY_ClearerID_NAME != ''">
                    <xsl:value-of select="$THIRDPARTY_ClearerID_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </ClearingBrokerAgentID>

              <ExposureTypeIndicator>
                <xsl:value-of select="''"/>
              </ExposureTypeIndicator>

              <NetMovementIndicator>
                <xsl:value-of select="''"/>
              </NetMovementIndicator>

              <NetMovementAmount>
                <xsl:value-of select="''"/>
              </NetMovementAmount>

              <IntermediaryIDType>
                <xsl:value-of select="''"/>
              </IntermediaryIDType>

              <IntermediaryID>
                <xsl:value-of select="''"/>
              </IntermediaryID>

              <AcctWithInstitutionIDType>
                <xsl:value-of select="''"/>
              </AcctWithInstitutionIDType>

              <AcctWithInstitutionID>
                <xsl:value-of select="''"/>
              </AcctWithInstitutionID>

              <PayingInstitution>
                <xsl:value-of select="''"/>
              </PayingInstitution>

              <BeneficiaryOfMoney>
                <xsl:value-of select="''"/>
              </BeneficiaryOfMoney>

              <CashAcct>
                <xsl:value-of select="''"/>
              </CashAcct>

              <CBO>
                <xsl:value-of select="''"/>
              </CBO>

              <StampDutyExemption>
                <xsl:value-of select="''"/>
              </StampDutyExemption>

              <StampCode>
                <xsl:value-of select="''"/>
              </StampCode>

              <TRADDETNarrative>
                <xsl:value-of select="''"/>
              </TRADDETNarrative>

              <FIANarrative>
                <xsl:value-of select="''"/>
              </FIANarrative>

              <Processing>
                <xsl:value-of select="''"/>
              </Processing>

              <Reference>
                <xsl:value-of select="''"/>
              </Reference>

              <ClearingBrokerAccount>
                <xsl:value-of select="''"/>
              </ClearingBrokerAccount>

              <Restrictions>
                <xsl:value-of select="''"/>
              </Restrictions>

              <RepoTermOpenInd>
                <xsl:value-of select="''"/>
              </RepoTermOpenInd>

              <RepoTermDate>
                <xsl:value-of select="''"/>
              </RepoTermDate>

              <RepoRateType>
                <xsl:value-of select="''"/>
              </RepoRateType>

              <RepoRate>
                <xsl:value-of select="''"/>
              </RepoRate>

              <RepoReference>
                <xsl:value-of select="''"/>
              </RepoReference>

              <RepoTotalTermAmt>
                <xsl:value-of select="''"/>
              </RepoTotalTermAmt>

              <RepoAccrueAmt>
                <xsl:value-of select="''"/>
              </RepoAccrueAmt>

              <RepoTotalCollCnt>
                <xsl:value-of select="''"/>
              </RepoTotalCollCnt>

              <RepoCollNumb>
                <xsl:value-of select="''"/>
              </RepoCollNumb>

              <RepoTypeInd>
                <xsl:value-of select="''"/>
              </RepoTypeInd>

              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>

            </ThirdPartyFlatFileDetail>

          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
