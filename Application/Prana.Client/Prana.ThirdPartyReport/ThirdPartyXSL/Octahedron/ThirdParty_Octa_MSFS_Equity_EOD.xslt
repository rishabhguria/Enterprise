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

        <TransactionType>
          <xsl:value-of select="'Transaction Type'"/>
        </TransactionType>

        <RecordType>
          <xsl:value-of select="'Record Type'"/>
        </RecordType>

        <BuySell>
          <xsl:value-of select="'Buy/Sell'"/>
        </BuySell>

        <LongShort>
          <xsl:value-of select="'Long/Short'"/>
        </LongShort>

        <BuySellLongShort>
          <xsl:value-of select="'Buy/Sell Long/Short'"/>
        </BuySellLongShort>

        <TransactionLevel>
          <xsl:value-of select="'Transaction Level'"/>
        </TransactionLevel>

        <ReferenceNo>
          <xsl:value-of select="'Reference No.'"/>
        </ReferenceNo>

        <Block>
          <xsl:value-of select="'Block'"/>
        </Block>

        <ExecutionAccount>
          <xsl:value-of select="'Execution Account'"/>
        </ExecutionAccount>

        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <ExecutingBroker>
          <xsl:value-of select="'Executing Broker'"/>
        </ExecutingBroker>

        <SecurityIDType>
          <xsl:value-of select="'Security ID Type'"/>
        </SecurityIDType>

        <SecurityID>
          <xsl:value-of select="'Security ID'"/>
        </SecurityID>

        <SecurityDesc>
          <xsl:value-of select="'Security Desc.'"/>
        </SecurityDesc>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select="'Settlement Date'"/>
        </SettlementDate>

        <SettlementCCY>
          <xsl:value-of select="'Settlement CCY'"/>
        </SettlementCCY>

        <ExchangeCode>
          <xsl:value-of select="'Exchange Code'"/>
        </ExchangeCode>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <PriceType>
          <xsl:value-of select="'Price Type'"/>
        </PriceType>

        <Principal>
          <xsl:value-of select="'Principal'"/>
        </Principal>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>
        
        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>
        
        <CommissionType>
          <xsl:value-of select="'Commission Type'"/>
        </CommissionType>

        <OtherCharges>
          <xsl:value-of select="'Other Charges'"/>
        </OtherCharges>

        <GovtSalesTax>
          <xsl:value-of select="'Govt Sales Tax'"/>
        </GovtSalesTax>

        <OtherChargesType>
          <xsl:value-of select="'Other Charges Type'"/>
        </OtherChargesType>

        <Interest>
          <xsl:value-of select="'Interest'"/>
        </Interest>

        <WaiveInterestGross>
          <xsl:value-of select="'Waive Interest Gross'"/>
        </WaiveInterestGross>

        <NetAmount>
          <xsl:value-of select="'Net Amount'"/>
        </NetAmount>

        <PortfolioOnly>
          <xsl:value-of select="'Portfolio Only'"/>
        </PortfolioOnly>

        <CustodianBroker>
          <xsl:value-of select="'Custodian Broker'"/>
        </CustodianBroker>

        <MoneyMngr>
          <xsl:value-of select="'Money Mngr.'"/>
        </MoneyMngr>

        <BookStrategy>
          <xsl:value-of select="'Book Strategy'"/>
        </BookStrategy>

        <DealId>
          <xsl:value-of select="'Deal Id'"/>
        </DealId>

        <TaxlotId>
          <xsl:value-of select="'Taxlot Id'"/>
        </TaxlotId>

        <PurchaseDate>
          <xsl:value-of select="'Vs. Purchase Date'"/>
        </PurchaseDate>

        <PurchasePrice>
          <xsl:value-of select="'Vs. Purchase Price'"/>
        </PurchasePrice>

        <CloseoutMethod>
          <xsl:value-of select="'Closeout Method'"/>
        </CloseoutMethod>

        <ExchangeRate>
          <xsl:value-of select="'Exchange Rate'"/>
        </ExchangeRate>

        <AcquireDate>
          <xsl:value-of select="'Acquire Date'"/>
        </AcquireDate>

        <Instruction>
          <xsl:value-of select="'Instruction 1'"/>
        </Instruction>

        <SinglePaymentDirection>
          <xsl:value-of select="'Single Payment Direction'"/>
        </SinglePaymentDirection>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select="'False'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="TransactionType">
            <xsl:choose>
              <xsl:when test="(AccountName='Octahedron Fund'
      or AccountName='MSPA In House' or AccountName='Octahedron Fund IPO' or AccountName='Octahedron Fund GS' 
      or AccountName='OCTAHEDRON LONG ONLY MASTER FUND LP- Hearsay' or AccountName='OCTAHEDRON LONG ONLY MASTER FUND LP IPO- Hearsay') 
      and CounterParty != 'CorpAction' and CounterParty != 'Washsale' and CounterParty != 'BOXcollapse' and CounterParty!='SwapReset' and TaxLotState != 'Deleted'">
                <xsl:choose>
                  <xsl:when test="Asset='Equity' and IsSwapped='true'">
                    <xsl:value-of select="'SW002'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'TR001'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>

              <xsl:when test="TaxLotState = 'Deleted' 
      and (AccountName='Octahedron Fund' or AccountName='MSPA In House' or AccountName='Octahedron Fund IPO' 
      or AccountName='Octahedron Fund GS') and CounterParty != 'CorpAction' and CounterParty !='Transfer' 
      and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset'">
                <xsl:choose>
                  <xsl:when test="Asset='Equity' and IsSwapped='true'">
                    <xsl:value-of select="'SW002'"/>
                  </xsl:when>
                  <xsl:when test="Asset='FX'">
                    <xsl:value-of select="'FX002'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="'TR001'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <TransactionType>
            <xsl:value-of select="$TransactionType"/>
          </TransactionType>

          <xsl:variable name="varTaxlotState">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'COR'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'CAN'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <RecordType>
            <xsl:value-of select="$varTaxlotState"/>
          </RecordType>

          <BuySell>
            <xsl:value-of select="''"/>
          </BuySell>

          <LongShort>
            <xsl:value-of select="''"/>
          </LongShort>

          <xsl:variable name="Sidevar">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">BL</xsl:when>
              <xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">BC</xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close'">SL</xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">SS</xsl:when>
              <xsl:otherwise></xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <BuySellLongShort>
            <xsl:value-of select="$Sidevar"/>
          </BuySellLongShort>

          <TransactionLevel>
            <xsl:value-of select="'B'"/>
          </TransactionLevel>

          <ReferenceNo>
            <xsl:value-of select="PBUniqueID"/>
          </ReferenceNo>

          <Block>
            <xsl:value-of select="PBUniqueID"/>
          </Block>

          <ExecutionAccount>
            <xsl:value-of select="'038211165'"/>
          </ExecutionAccount>

          <Account>
            <xsl:value-of select="'038211165'"/>
          </Account>

          <xsl:variable name="PB_NAME" select="'MS'"/>

          <xsl:variable name="PB_COUNTERPARTY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=CounterParty]/@ThirdPartyBrokerID"/>
          </xsl:variable>

          <xsl:variable name="CPVar">
            <xsl:choose>
              <xsl:when test="$PB_COUNTERPARTY_NAME!=''">
                <xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
              </xsl:when>
              <xsl:when test="CounterParty='RBCZ' or CounterParty='RBCB'">
                <xsl:value-of select="'RBCM'"/>
              </xsl:when>
              <xsl:when test="CounterParty='Transfer'">
                <xsl:value-of select="'TRFR'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CounterParty"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <ExecutingBroker>
            <xsl:value-of select="$CPVar"/>
          </ExecutingBroker>

          <xsl:variable name="varSecType">
            <xsl:choose>
              <xsl:when test ="Asset='EquityOption'">
                <xsl:value-of select="'T'"/>
              </xsl:when>
              <xsl:when test ="Asset='FixedIncome'">
                <xsl:value-of select="'I'"/>
              </xsl:when>

              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="'C'"/>
              </xsl:when>
              <xsl:when test="ISIN != ''">
                <xsl:value-of select="'I'"/>
              </xsl:when>


              <xsl:when test="Symbol != ''">
                <xsl:value-of select="'T'"/>
              </xsl:when>
              <xsl:when test="RIC != ''">
                <xsl:value-of select="'R'"/>
              </xsl:when>
              <xsl:when test="BBCode != ''">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="'T'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <SecurityIDType>
            <xsl:value-of select="$varSecType"/>
          </SecurityIDType>

          <xsl:variable name="varSymbol">
            <xsl:choose>
              <xsl:when test ="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:when test ="Asset='FixedIncome'">
                <xsl:value-of select="ISIN"/>
              </xsl:when>

              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:when test="ISIN != ''">
                <xsl:value-of select="ISIN"/>
              </xsl:when>


              <xsl:when test="Symbol != ''">
                <xsl:value-of select="Symbol"/>
              </xsl:when>
              <xsl:when test="RIC != ''">
                <xsl:value-of select="RIC"/>
              </xsl:when>
              <xsl:when test="BBCode != ''">
                <xsl:value-of select="BBCode"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <SecurityID>
            <xsl:value-of select="$varSymbol"/>
          </SecurityID>

          <SecurityDesc>
            <xsl:value-of select="FullSecurityName"/>
          </SecurityDesc>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <SettlementCCY>
            <xsl:value-of select="SettlCurrency"/>
          </SettlementCCY>

          <ExchangeCode>
            <xsl:value-of select="''"/>
          </ExchangeCode>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <xsl:variable name="varSettFxAmt">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Trade ='M'">
                    <xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="AveragePrice"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varAveragePriceSettl">
            <xsl:choose>
              <xsl:when test="SettlCurrency = CurrencySymbol">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varSettFxAmt"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <Price>
            <xsl:value-of select="format-number($varAveragePriceSettl,'###.00000')"/>
          </Price>

          <PriceType>
            <xsl:value-of select="'G'"/>
          </PriceType>

          <xsl:variable name="varFXRate">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="GroupGrsAmt">
            <xsl:value-of select="AllocatedQty * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
          </xsl:variable>

          <xsl:variable name="varGroupGrsAmtSettl">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$GroupGrsAmt"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="format-number($GroupGrsAmt * $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="format-number($GroupGrsAmt div $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Principal>
            <xsl:value-of select="format-number($varGroupGrsAmtSettl,'###.00')"/>
          </Principal>

          <Commission>
            <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'###.00')"/>
          </Commission>

          <CommissionType>
            <xsl:value-of select="'F'"/>
          </CommissionType>

          <OtherCharges>
            <xsl:value-of select="0"/>
          </OtherCharges>

          <GovtSalesTax>
            <xsl:value-of select="0"/>
          </GovtSalesTax>

          <OtherChargesType>
            <xsl:value-of select="'F'"/>
          </OtherChargesType>

          <xsl:variable name="varAccruedInt">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="AccruedInterest"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="format-number(AccruedInterest * $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="format-number(AccruedInterest div $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varAccruedInterest">
            <xsl:choose>
              <xsl:when test ="contains(Asset,'FixedIncome')">
                <xsl:value-of select="format-number($varAccruedInt,'#.##')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Interest>
            <xsl:value-of select="$varAccruedInterest"/>
          </Interest>

          <WaiveInterestGross>
            <xsl:value-of select="''"/>
          </WaiveInterestGross>

          <xsl:variable name="GroupNetAmtFX">
            <xsl:choose>
              <xsl:when test ="contains(Asset,'FixedIncome')">
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Close'">
                    <xsl:value-of select="$GroupGrsAmt + AccruedInterest"/>
                  </xsl:when>

                  <xsl:when test="Side='Sell' or Side='Sell short'">
                    <xsl:value-of select="$GroupGrsAmt - AccruedInterest"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="NetAmount"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varNetamountSettl">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$GroupNetAmtFX"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="format-number($GroupNetAmtFX * $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="format-number($GroupNetAmtFX div $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <NetAmount>
            <xsl:value-of select="format-number($varNetamountSettl,'###.00')"/>
          </NetAmount>

          <PortfolioOnly>
            <xsl:value-of select="'Y'"/>
          </PortfolioOnly>

          <xsl:variable name="varCustodian">
            <xsl:choose>
              <xsl:when test="AccountMappedName='038CADJ92' and Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'MSSW'"/>
              </xsl:when>

              <xsl:when test="AccountName='Octahedron Fund IPO'">
                <xsl:value-of select="'MSCO'"/>
              </xsl:when>
              <xsl:when test="AccountMappedName='038CADJ92'and IsSwapped='false'">
                <xsl:value-of select="'MSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='MSPA In House'">
                <xsl:value-of select="'MSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='Octahedron Fund GS'">
                <xsl:value-of select="'GSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='Octahedron Fund GS'">
                <xsl:value-of select="'GSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='OCTAHEDRON LONG ONLY MASTER FUND LP- Hearsay'">
                <xsl:value-of select="'MSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='OCTAHEDRON LONG ONLY MASTER FUND LP IPO- Hearsay'">
                <xsl:value-of select="'MSCO'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <CustodianBroker>
            <xsl:value-of select="$varCustodian"/>
          </CustodianBroker>

          <MoneyMngr>
            <xsl:value-of select="''"/>
          </MoneyMngr>

          <BookStrategy>
            <xsl:value-of select="''"/>
          </BookStrategy>

          <DealId>
            <xsl:value-of select="''"/>
          </DealId>

          <TaxlotId>
            <xsl:value-of select="''"/>
          </TaxlotId>

          <PurchaseDate>
            <xsl:value-of select="''"/>
          </PurchaseDate>

          <PurchasePrice>
            <xsl:value-of select="''"/>
          </PurchasePrice>

          <CloseoutMethod>
            <xsl:value-of select="''"/>
          </CloseoutMethod>

          <ExchangeRate>
            <xsl:value-of select="''"/>
          </ExchangeRate>

          <AcquireDate>
            <xsl:value-of select="''"/>
          </AcquireDate>

          <Instruction>
            <xsl:value-of select="''"/>
          </Instruction>

          <SinglePaymentDirection>
            <xsl:value-of select="''"/>
          </SinglePaymentDirection>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select="'False'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="TransactionType">
            <xsl:choose>
              <xsl:when test="(AccountName='Octahedron Fund'
      or AccountName='MSPA In House' or AccountName='Octahedron Fund IPO' or AccountName='Octahedron Fund GS' 
      or AccountName='OCTAHEDRON LONG ONLY MASTER FUND LP- Hearsay' or AccountName='OCTAHEDRON LONG ONLY MASTER FUND LP IPO- Hearsay') 
      and CounterParty != 'CorpAction' and CounterParty != 'Washsale' and CounterParty != 'BOXcollapse' and CounterParty!='SwapReset' and TaxLotState != 'Deleted'">
                <xsl:choose>
                  <xsl:when test="Asset='Equity' and IsSwapped='true'">
                    <xsl:value-of select="'SW002'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'TR001'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>

              <xsl:when test="TaxLotState = 'Deleted' 
      and (AccountName='Octahedron Fund' or AccountName='MSPA In House' or AccountName='Octahedron Fund IPO' 
      or AccountName='Octahedron Fund GS') and CounterParty != 'CorpAction' and CounterParty !='Transfer' 
      and CounterParty!='Washsale' and CounterParty!='BOXcollapse' and CounterParty!='SwapReset'">
                <xsl:choose>
                  <xsl:when test="Asset='Equity' and IsSwapped='true'">
                    <xsl:value-of select="'SW002'"/>
                  </xsl:when>
                  <xsl:when test="Asset='FX'">
                    <xsl:value-of select="'FX002'"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="'TR001'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <TransactionType>
            <xsl:value-of select="$TransactionType"/>
          </TransactionType>

          <xsl:variable name="varTaxlotState">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'COR'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'CAN'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <RecordType>
            <xsl:value-of select="$varTaxlotState"/>
          </RecordType>

          <BuySell>
            <xsl:value-of select="''"/>
          </BuySell>

          <LongShort>
            <xsl:value-of select="''"/>
          </LongShort>

          <xsl:variable name="Sidevar">
            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">BL</xsl:when>
              <xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">BC</xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close'">SL</xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">SS</xsl:when>
              <xsl:otherwise></xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <BuySellLongShort>
            <xsl:value-of select="$Sidevar"/>
          </BuySellLongShort>

          <TransactionLevel>
            <xsl:value-of select="'B'"/>
          </TransactionLevel>

          <xsl:variable name="varAccountMapping">
            <xsl:choose>
              <xsl:when test="AccountName = 'Octahedron Fund'">
                <xsl:value-of select="'038QAH5I7'"/>
              </xsl:when>
              <xsl:when test="AccountName = 'Octahedron Fund IPO'">
                <xsl:value-of select="'038QAH5J5'"/>
              </xsl:when>
              <xsl:when test="AccountName = 'MSPA In House'">
                <xsl:value-of select="'038QAH5I7'"/>
              </xsl:when>
              <xsl:when test="AccountName = 'Octahedron Fund GS'">
                <xsl:value-of select="'038QAH5I7'"/>
              </xsl:when>
              <xsl:when test="AccountName = 'OCTAHEDRON LONG ONLY MASTER FUND LP- Hearsay'">
                <xsl:value-of select="'038QLAHD8'"/>
              </xsl:when>
              <xsl:when test="AccountName = 'OCTAHEDRON LONG ONLY MASTER FUND LP IPO- Hearsay'">
                <xsl:value-of select="'038QLAHE6'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <ReferenceNo>
            <xsl:value-of select="concat(PBUniqueID,$varAccountMapping)"/>
          </ReferenceNo>

          <Block>
            <xsl:value-of select="PBUniqueID"/>
          </Block>

          <ExecutionAccount>
            <xsl:value-of select="'038211165'"/>
          </ExecutionAccount>

          <Account>
            <xsl:value-of select="$varAccountMapping"/>
          </Account>

          <xsl:variable name="PB_NAME" select="'MS'"/>

          <xsl:variable name="PB_COUNTERPARTY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=CounterParty]/@ThirdPartyBrokerID"/>
          </xsl:variable>

          <xsl:variable name="CPVar">
            <xsl:choose>
              <xsl:when test="$PB_COUNTERPARTY_NAME!=''">
                <xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
              </xsl:when>
              <xsl:when test="CounterParty='RBCZ' or CounterParty='RBCB'">
                <xsl:value-of select="'RBCM'"/>
              </xsl:when>
              <xsl:when test="CounterParty='Transfer'">
                <xsl:value-of select="'TRFR'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CounterParty"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <ExecutingBroker>
            <xsl:value-of select="$CPVar"/>
          </ExecutingBroker>

          <xsl:variable name="varSecType">
            <xsl:choose>
              <xsl:when test ="Asset='EquityOption'">
                <xsl:value-of select="'T'"/>
              </xsl:when>
              <xsl:when test ="Asset='FixedIncome'">
                <xsl:value-of select="'I'"/>
              </xsl:when>

              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="'C'"/>
              </xsl:when>
              <xsl:when test="ISIN != ''">
                <xsl:value-of select="'I'"/>
              </xsl:when>


              <xsl:when test="Symbol != ''">
                <xsl:value-of select="'T'"/>
              </xsl:when>
              <xsl:when test="RIC != ''">
                <xsl:value-of select="'R'"/>
              </xsl:when>
              <xsl:when test="BBCode != ''">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="'T'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <SecurityIDType>
            <xsl:value-of select="$varSecType"/>
          </SecurityIDType>

          <xsl:variable name="varSymbol">
            <xsl:choose>
              <xsl:when test ="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:when test ="Asset='FixedIncome'">
                <xsl:value-of select="ISIN"/>
              </xsl:when>

              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:when test="ISIN != ''">
                <xsl:value-of select="ISIN"/>
              </xsl:when>


              <xsl:when test="Symbol != ''">
                <xsl:value-of select="Symbol"/>
              </xsl:when>
              <xsl:when test="RIC != ''">
                <xsl:value-of select="RIC"/>
              </xsl:when>
              <xsl:when test="BBCode != ''">
                <xsl:value-of select="BBCode"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <SecurityID>
            <xsl:value-of select="$varSymbol"/>
          </SecurityID>

          <SecurityDesc>
            <xsl:value-of select="FullSecurityName"/>
          </SecurityDesc>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <SettlementCCY>
            <xsl:value-of select="SettlCurrency"/>
          </SettlementCCY>

          <ExchangeCode>
            <xsl:value-of select="''"/>
          </ExchangeCode>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <xsl:variable name="varSettFxAmt">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Trade ='M'">
                    <xsl:value-of select="AveragePrice * FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AveragePrice div FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="AveragePrice"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varAveragePriceSettl">
            <xsl:choose>
              <xsl:when test="SettlCurrency = CurrencySymbol">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varSettFxAmt"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Price>
            <xsl:value-of select="format-number($varAveragePriceSettl,'###.00000')"/>
          </Price>

          <PriceType>
            <xsl:value-of select="'G'"/>
          </PriceType>

          <xsl:variable name="varFXRate">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="GroupGrsAmt">
            <xsl:value-of select="AllocatedQty * format-number(AveragePrice,'###.0000') * AssetMultiplier"/>
          </xsl:variable>

          <xsl:variable name="varGroupGrsAmtSettl">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$GroupGrsAmt"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="format-number($GroupGrsAmt * $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="format-number($GroupGrsAmt div $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Principal>
            <xsl:value-of select="format-number($varGroupGrsAmtSettl,'###.00')"/>
          </Principal>

          <Commission>
            <xsl:value-of select="format-number(CommissionCharged + SoftCommissionCharged,'###.00')"/>
          </Commission>

          <CommissionType>
            <xsl:value-of select="'F'"/>
          </CommissionType>

          <OtherCharges>
            <xsl:value-of select="0"/>
          </OtherCharges>

          <GovtSalesTax>
            <xsl:value-of select="0"/>
          </GovtSalesTax>

          <OtherChargesType>
            <xsl:value-of select="'F'"/>
          </OtherChargesType>

          <xsl:variable name="varAccruedInt">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="AccruedInterest"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="format-number(AccruedInterest * $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="format-number(AccruedInterest div $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varAccruedInterest">
            <xsl:choose>
              <xsl:when test ="contains(Asset,'FixedIncome')">
                <xsl:value-of select="format-number($varAccruedInt,'#.##')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <Interest>
            <xsl:value-of select="$varAccruedInterest"/>
          </Interest>

          <WaiveInterestGross>
            <xsl:value-of select="''"/>
          </WaiveInterestGross>

          <xsl:variable name="GroupNetAmtFX">
            <xsl:choose>
              <xsl:when test ="contains(Asset,'FixedIncome')">
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Close'">
                    <xsl:value-of select="$GroupGrsAmt + AccruedInterest"/>
                  </xsl:when>

                  <xsl:when test="Side='Sell' or Side='Sell short'">
                    <xsl:value-of select="$GroupGrsAmt - AccruedInterest"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="NetAmount"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varNetamountSettl">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$GroupNetAmtFX"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="format-number($GroupNetAmtFX * $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="format-number($GroupNetAmtFX div $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <NetAmount>
            <xsl:value-of select="format-number($varNetamountSettl,'###.00')"/>
          </NetAmount>

          <PortfolioOnly>
            <xsl:value-of select="'Y'"/>
          </PortfolioOnly>

          <xsl:variable name="varCustodian">
            <xsl:choose>
              <xsl:when test="AccountMappedName='038CADJ92' and Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'MSSW'"/>
              </xsl:when>
              <xsl:when test="AccountName='Octahedron Fund IPO'">
                <xsl:value-of select="'MSCO'"/>
              </xsl:when>
              <xsl:when test="AccountMappedName='038CADJ92'and IsSwapped='false'">
                <xsl:value-of select="'MSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='MSPA In House'">
                <xsl:value-of select="'MSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='Octahedron Fund GS'">
                <xsl:value-of select="'GSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='Octahedron Fund GS'">
                <xsl:value-of select="'GSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='OCTAHEDRON LONG ONLY MASTER FUND LP- Hearsay'">
                <xsl:value-of select="'MSCO'"/>
              </xsl:when>
              <xsl:when test="AccountName='OCTAHEDRON LONG ONLY MASTER FUND LP IPO- Hearsay'">
                <xsl:value-of select="'MSCO'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <CustodianBroker>
            <xsl:value-of select="$varCustodian"/>
          </CustodianBroker>

          <MoneyMngr>
            <xsl:value-of select="''"/>
          </MoneyMngr>

          <BookStrategy>
            <xsl:value-of select="''"/>
          </BookStrategy>

          <DealId>
            <xsl:value-of select="''"/>
          </DealId>

          <TaxlotId>
            <xsl:value-of select="''"/>
          </TaxlotId>

          <PurchaseDate>
            <xsl:value-of select="''"/>
          </PurchaseDate>

          <PurchasePrice>
            <xsl:value-of select="''"/>
          </PurchasePrice>

          <CloseoutMethod>
            <xsl:value-of select="''"/>
          </CloseoutMethod>

          <ExchangeRate>
            <xsl:value-of select="''"/>
          </ExchangeRate>

          <AcquireDate>
            <xsl:value-of select="''"/>
          </AcquireDate>

          <Instruction>
            <xsl:value-of select="''"/>
          </Instruction>

          <SinglePaymentDirection>
            <xsl:value-of select="''"/>
          </SinglePaymentDirection>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>