<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[(Asset='Equity' or Asset='EquityOption')][(AccountName='VeriStar Jefferies 43001842')]">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>


          <DealType>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'EquityDeal'"/>
              </xsl:when>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'OptionDeal'"/>
              </xsl:when>
            </xsl:choose>
          </DealType>

          <!-- <DealId> -->
            <!-- <xsl:value-of select="substring(EntityID,string-length(EntityID)-7,string-length(EntityID))"/> -->
          <!-- </DealId> -->
		       <DealId>
            <xsl:value-of select="EntityID"/>
          </DealId>

          <xsl:variable name="varTaxlotStateTx">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'New'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'Update'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'Cancel'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Action>
            <xsl:value-of select="$varTaxlotStateTx"/>
          </Action>

          <Client>
            <xsl:value-of select="'AEADMGM'"/>
          </Client>

          <Fund>
            <xsl:value-of select="'AEAEQTY'"/>
          </Fund>

          <PortfolioBusinessUnit>
            <xsl:value-of select="''"/>
          </PortfolioBusinessUnit>

          <Strategy>
            <xsl:value-of select="'AEAEQTY'"/>
          </Strategy>

          <Custodian>
            <xsl:value-of select="'JEFF'"/>
          </Custodian>

          <CashAccount>
            <xsl:value-of select="'JFNAECAEPB'"/>
          </CashAccount>

          <Counterparty>
            <xsl:value-of select="CounterParty"/>
          </Counterparty>

          <Comments>
            <xsl:value-of select="''"/>
          </Comments>

          <State>
            <xsl:value-of select="'Valid'"/>
          </State>

          <TradeDate>
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),'-',substring-before(SettlementDate,'/'),'-',substring-before(substring-after(SettlementDate,'/'),'/'))"/>
          </SettlementDate>

          <Reserved>
            <xsl:value-of select="''"/>
          </Reserved>

          <GlobeOpSecurityIdentifier>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and CurrencySymbol !='USD'">
                <xsl:value-of select="ISIN"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CUSIP"/>
              </xsl:otherwise>
            </xsl:choose>
          </GlobeOpSecurityIdentifier>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>

          <Sedol>
            <xsl:value-of select="SEDOL"/>
          </Sedol>

          <xsl:variable name="varBloombergticker">
            <xsl:value-of select="substring-before(BBCode,' EQUITY')"/>
          </xsl:variable>

          <Bloombergticker>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and CurrencySymbol !='USD'">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='CAD'">
                    <xsl:value-of select="concat(substring-before($varBloombergticker,' '),' CT')"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol='JPY'">
                    <xsl:value-of select="concat(substring-before($varBloombergticker,' '),' JT')"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol='GBP'">
                    <xsl:value-of select="concat(substring-before($varBloombergticker,' '),' LN')"/>
                  </xsl:when>
				          <xsl:when test="CurrencySymbol='AUD'">
                    <xsl:value-of select="concat(substring-before($varBloombergticker,' '),' AT')"/>
                  </xsl:when>
				         <xsl:when test="CurrencySymbol='SGD'">
                    <xsl:value-of select="concat(substring-before($varBloombergticker,' '),' SP')"/>
                  </xsl:when>				  
                  <xsl:when test="CurrencySymbol='EUR'">
                    <xsl:value-of select="concat(substring-before($varBloombergticker,' '),' GY')"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varBloombergticker"/>
              </xsl:otherwise>
            </xsl:choose>
            
          </Bloombergticker>

          <RIC>
            <xsl:value-of select="RIC"/>
          </RIC>

          <SecurityDescription>
            <xsl:value-of select="FullSecurityName"/>
          </SecurityDescription>

          <xsl:variable name="varSide">
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:choose>
                  <xsl:when test="Side='Buy' or Side='Buy to Close'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell' or Side='Sell short'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>

              <xsl:when test="Asset='EquityOption'">
                <xsl:choose>
                  <xsl:when test="Side='Buy to Open'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buy'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Open'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Close'">
                    <xsl:value-of select="'Sell'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <TransactionIndicator>
            <xsl:value-of select="$varSide"/>
          </TransactionIndicator>


          <xsl:variable name="varSubSide">
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:choose>
                  <xsl:when test="Side='Buy'">
                    <xsl:value-of select="'Buy Long'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buy Cover'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell'">
                    <xsl:value-of select="'Sell Long'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell short'">
                    <xsl:value-of select="'Sell Short'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>

              <xsl:when test="Asset='EquityOption'">
                <xsl:choose>
                  <xsl:when test="Side='Buy to Open'">
                    <xsl:value-of select="'Buy Long'"/>
                  </xsl:when>
                  <xsl:when test="Side='Buy to Close'">
                    <xsl:value-of select="'Buy Cover'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Open'">
                    <xsl:value-of select="'Sell Long'"/>
                  </xsl:when>
                  <xsl:when test="Side='Sell to Close'">
                    <xsl:value-of select="'Sell Short'"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <SubTransactionIndicator>
            <xsl:value-of select="$varSubSide"/>
          </SubTransactionIndicator>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <xsl:variable name="varSettCurrAmt">
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

          <xsl:variable name="varPrice">
            <xsl:choose>
              <xsl:when test="SettlCurrency = CurrencySymbol">
                <xsl:value-of select="AveragePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varSettCurrAmt"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Price>
            <xsl:value-of select="$varPrice"/>
          </Price>
        

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

          <xsl:variable name="varCommission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

          <xsl:variable name="varCommissionAmount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$varCommission"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="$varCommission * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:choose>
                  <xsl:when test="$varCommission !=0">
                    <xsl:value-of select="$varCommission div $varFXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>               
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <Commission>
            <xsl:value-of select="$varCommissionAmount"/>
          </Commission>

          <xsl:variable name="varSecFee">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="SecFee"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="SecFee * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:choose>
                  <xsl:when test="SecFee !=0">
                    <xsl:value-of select="SecFee div $varFXRate"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>                
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varTotalFee">
            <xsl:value-of select="$varSecFee + OtherBrokerFee"/>
          </xsl:variable>

          <SECFee>
            <xsl:value-of select="$varTotalFee"/>
          </SECFee>

          <VAT>
            <xsl:value-of select="''"/>
          </VAT>


          <TradeCurrency>
            <xsl:value-of select="SettlCurrency"/>
          </TradeCurrency>

          <ExchangeRate>
            <xsl:value-of select="''"/>
          </ExchangeRate>

          <Reserved1>
            <xsl:value-of select="''"/>
          </Reserved1>

          <BrokerShortName>
            <xsl:value-of select="''"/>
          </BrokerShortName>

          <ClearingMode>
            <xsl:value-of select="''"/>
          </ClearingMode>

          <Exchange>
            <xsl:value-of select="''"/>
          </Exchange>

          <ClientReference>
            <xsl:value-of select="''"/>
          </ClientReference>

          <Reserved2>
            <xsl:value-of select="''"/>
          </Reserved2>

          <SecurityCurrency>
            <xsl:value-of select="''"/>
          </SecurityCurrency>

          <BlockId>
            <xsl:value-of select="''"/>
          </BlockId>

          <BlockAmount>
            <xsl:value-of select="''"/>
          </BlockAmount>

          <ExecutionDateTimeStamp>
            <xsl:value-of select="''"/>
          </ExecutionDateTimeStamp>

          <ClearingFee>
            <xsl:value-of select="AUECFee1"/>
          </ClearingFee>

          <SettlementCurrencyHedge>
            <xsl:value-of select="SettlCurrency"/>
          </SettlementCurrencyHedge>

          <TradeDateFx>
            <xsl:value-of select="''"/>
          </TradeDateFx>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>