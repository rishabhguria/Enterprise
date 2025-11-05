<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
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

        <Trade_ID>
          <xsl:value-of select="'Trade_ID'"/>
        </Trade_ID>

      <Portfolio>
        <xsl:value-of select="'Portfolio'"/>
      </Portfolio>

      <Broker>
        <xsl:value-of select="'Broker'"/>
      </Broker>

      <Custodian>
        <xsl:value-of select ="'Custodian'"/>
      </Custodian>

      <Strategy>
        <xsl:value-of select ="'Strategy'"/>
      </Strategy>

      <Fund>
        <xsl:value-of select ="'Fund Class'"/>
      </Fund>

      <Transaction>
        <xsl:value-of select ="'Transaction'"/>
      </Transaction>

      <Status>
        <xsl:value-of select ="'Status'"/>
      </Status>

      <Security_Type>
        <xsl:value-of select ="'Security_Type'"/>
      </Security_Type>

      <Security>
        <xsl:value-of select ="'Security'"/>
      </Security>

      <Exchange>
        <xsl:value-of select="'Exchange'"/>
      </Exchange>

      <Sedol>
        <xsl:value-of select ="'Sedol'"/>
      </Sedol>

      <ISIN>
        <xsl:value-of select ="'ISIN'"/>
      </ISIN>


      <Cusip>
        <xsl:value-of select ="'Cusip'"/>
      </Cusip>

      <BBCode>
        <xsl:value-of select ="'Bberg Code'"/>
      </BBCode>

      <Description>
        <xsl:value-of select ="'Description'"/>
      </Description>

      <Underlying>
        <xsl:value-of select ="'Underlying'"/>
      </Underlying>

      <UnderlyingDescription>
        <xsl:value-of select ="'Underlying Description'"/>
      </UnderlyingDescription>

      <Order_Quantity>
        <xsl:value-of select ="'Order_Quantity'"/>
      </Order_Quantity>

      <Order_Price>
        <xsl:value-of select ="'Order_Price'"/>
      </Order_Price>

      <Yield_Price>
        <xsl:value-of select ="'Yield_Price'"/>
      </Yield_Price>

      <Trade_Currency>
        <xsl:value-of select ="'Trade_Currency'"/>
      </Trade_Currency>

      <Settlement_Currency>
        <xsl:value-of select ="'Settlement_Currency'"/>
      </Settlement_Currency>

      <Strike_Price>
        <xsl:value-of select ="'Strike_Price'"/>
      </Strike_Price>

      <Option_Indicator>
        <xsl:value-of select ="'Option_Indicator'"/>
      </Option_Indicator>

      <Exercise_Type>
        <xsl:value-of select ="'Exercise_Type'"/>
      </Exercise_Type>

      <Buy_Currency>
        <xsl:value-of select ="'Buy_Currency'"/>

      </Buy_Currency>

      <Buy_Quantity>
        <xsl:value-of select ="'Buy_Quantity'"/>
      </Buy_Quantity>

      <Sell_Currency>
        <xsl:value-of select ="'Sell_Currency'"/>
      </Sell_Currency>

      <Sell_Quantity>
        <xsl:value-of select ="'Sell_Quantity'"/>
      </Sell_Quantity>

      <Trade_Date>
        <xsl:value-of select ="'Trade_Date'"/>
      </Trade_Date>

      <Settle_Date>
        <xsl:value-of select ="'Settle_Date'"/>
      </Settle_Date>

      <Principal>
        <xsl:value-of select ="'Principal'"/>
      </Principal>

      <Total_Fees>
        <xsl:value-of select="'Total_Fees'"/>
      </Total_Fees>

      <Total_Commission>
        <xsl:value-of select ="'Total_Commission'"/>
      </Total_Commission>

      <Net_Amount_Trade>
        <xsl:value-of select="'Net_Amount_Trade'"/>
      </Net_Amount_Trade>


      <Net_Amount_Settle>
        <xsl:value-of select ="'Net_Amount_Settle'"/>
      </Net_Amount_Settle>

      <Accrued_Interest>
        <xsl:value-of select ="'Accrued_Interest'"/>
      </Accrued_Interest>

      <Expense>
        <xsl:value-of select="'Expense'"/>
      </Expense>

      <Expense_Type>
        <xsl:value-of select ="'Expense_Type'"/>
      </Expense_Type>

      <Maturity_Date>
        <xsl:value-of select ="'Maturity_Date'"/>
      </Maturity_Date>


      <FX_Rate>
        <xsl:value-of select= "'FX_Rate'"/>
      </FX_Rate>

      <Tax_Lot_ID>
        <xsl:value-of select ="'Tax_Lot_ID'"/>
      </Tax_Lot_ID>

      <!--column 20 -->

      <Versus_Date>
        <xsl:value-of select ="'Versus_Date'"/>
      </Versus_Date>

      <Original_Face>
        <xsl:value-of select ="'Original_Face'"/>
      </Original_Face>

      <ContractSize>
        <xsl:value-of select ="'Contract size'"/>
      </ContractSize>

      <IssueCountry>
        <xsl:value-of select ="'Issue Country'"/>
      </IssueCountry>


      <InstrumentCurrency>
        <xsl:value-of select ="'Instrument Currency'"/>
      </InstrumentCurrency>

      <AccrualStartDate>
        <xsl:value-of select ="'Accrual Start Date'"/>
      </AccrualStartDate>

      <FirstCouponDate>
        <xsl:value-of select ="'First Coupon Date'"/>
      </FirstCouponDate>

      <CouponRate>
        <xsl:value-of select ="'Coupon Rate'"/>
      </CouponRate>

      <CouponFrequency>
        <xsl:value-of select ="'Coupon Frequency'"/>
      </CouponFrequency>

      <FirstCouponDateFloat>
        <xsl:value-of select ="'First Coupon Date (Float)'"/>
      </FirstCouponDateFloat>

      <CouponRateFloat>
        <xsl:value-of select ="'Coupon Rate (Float)'"/>
      </CouponRateFloat>

      <CouponFrequencyFloat>
        <xsl:value-of select ="'Coupon Frequency (Float)'"/>
      </CouponFrequencyFloat>

      <FirstResetDateFloat>
        <xsl:value-of select ="'First Reset Date (Float)'"/>
      </FirstResetDateFloat>

      <FirstResetFreqFloat>
        <xsl:value-of select ="'Rate Reset Frequency (Float)'"/>
      </FirstResetFreqFloat>

      <LXID>
        <xsl:value-of select ="'LX ID'"/>
      </LXID>

      <FacilityCouponFrequency>
        <xsl:value-of select="'Facility Coupon Frequency'"/>
      </FacilityCouponFrequency>

      <FaciltyCouponRate>
        <xsl:value-of select="'Facilty Coupon Rate'"/>
      </FaciltyCouponRate>

      <Issuer>
        <xsl:value-of select ="'Issuer'"/>
      </Issuer>

      <Tranche>
        <xsl:value-of select ="'Tranche'"/>
      </Tranche>

      <GlobalAmount>
        <xsl:value-of select="'Global Amount'"/>
      </GlobalAmount>

      <ContractAmount>
        <xsl:value-of select ="'Contract Amount'"/>
      </ContractAmount>

      <ContractFrequency>
        <xsl:value-of select ="'Contract Frequency'"/>
      </ContractFrequency>

      <ContractRate>
        <xsl:value-of select ="'Contract Rate'"/>
      </ContractRate>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
        
      </ThirdPartyFlatFileDetail>

        <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          
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
            
            <Trade_ID>
              <xsl:value-of select="EntityID"/>
            </Trade_ID>

            <Portfolio>
              <xsl:value-of select="''"/>
            </Portfolio>

            <Broker>
              <xsl:value-of select="CounterParty"/>
            </Broker>

            <Custodian>
              <xsl:value-of select="ThirdParty"/>
            </Custodian>

            <Strategy>
              <xsl:value-of select="Strategy"/>
            </Strategy>

            <Fund>
              <xsl:value-of select="AccountName"/>
            </Fund>

            <Transaction>
              <xsl:value-of select="Side"/>
            </Transaction>

            <Status>
              <xsl:value-of select="TaxLotState"/>
            </Status>

            <Security_Type>
              <xsl:value-of select="Asset"/>
            </Security_Type>

            <Security>
              <xsl:value-of select="Symbol"/>
            </Security>
            
            <Exchange>
              <xsl:value-of select="Exchange"/>
            </Exchange>

            <Sedol>
              <xsl:value-of select ="SEDOL"/>
            </Sedol>

            <ISIN>
              <xsl:value-of select ="ISIN"/>
            </ISIN>


            <Cusip>
              <xsl:value-of select ="CUSIP"/>
            </Cusip>

            <BBCode>
              <xsl:value-of select ="BBCode"/>
            </BBCode>

            <Description>
              <xsl:value-of select ="FullSecurityName"/>
            </Description>

            <Underlying>
              <xsl:value-of select ="UnderlyingSymbol"/>
            </Underlying>

            <UnderlyingDescription>
              <xsl:value-of select ="''"/>
            </UnderlyingDescription>

            <Order_Quantity>
              <xsl:value-of select ="AllocatedQty"/>
            </Order_Quantity>

            <Order_Price>
              <xsl:value-of select ="AveragePrice"/>
            </Order_Price>

            <Yield_Price>
              <xsl:value-of select ="''"/>
            </Yield_Price>

            <Trade_Currency>
              <xsl:value-of select ="CurrencySymbol"/>
            </Trade_Currency>

            <Settlement_Currency>
              <xsl:value-of select ="'USD'"/>
            </Settlement_Currency>

            <Strike_Price>
              <xsl:value-of select ="StrikePrice"/>
            </Strike_Price>

            <Option_Indicator>
              <xsl:value-of select ="PutOrCall"/>
            </Option_Indicator>

            <Exercise_Type>
              <xsl:value-of select ="''"/>
            </Exercise_Type>

            <Buy_Currency>
              <xsl:value-of select ="''"/>
            </Buy_Currency>

            <Buy_Quantity>
              <xsl:value-of select ="''"/>
            </Buy_Quantity>

            <Sell_Currency>
              <xsl:value-of select ="''"/>
            </Sell_Currency>

            <Sell_Quantity>
              <xsl:value-of select ="''"/>
            </Sell_Quantity>

            <Trade_Date>
              <xsl:value-of select ="TradeDate"/>
            </Trade_Date>

            <!--column 7 -->

            <Settle_Date>
              <xsl:value-of select ="SettlementDate"/>
            </Settle_Date>

            <Principal>
              <xsl:value-of select ="GrossAmount"/>
            </Principal>

            <Total_Fees>
              <xsl:value-of select="StampDuty+TransactionLevy+ClearingFee+TaxOnCommissions+MiscFees"/>
            </Total_Fees>

            <Total_Commission>
              <xsl:value-of select ="CommissionCharged"/>
            </Total_Commission>

            <Net_Amount_Trade>
              <xsl:value-of select="NetAmount"/>
            </Net_Amount_Trade>

            <!-- Column 13-->
            <Net_Amount_Settle>
              <xsl:value-of select ="''"/>
            </Net_Amount_Settle>

            <Accrued_Interest>
              <xsl:value-of select ="AccruedInterest"/>
            </Accrued_Interest>

            <Expense >
              <xsl:value-of select="''"/>
            </Expense >

            <Expense_Type>
              <xsl:value-of select ="''"/>
            </Expense_Type>

            <Maturity_Date>
              <xsl:value-of select ="''"/>
            </Maturity_Date>

            <FX_Rate>
              <xsl:value-of select= 'ForexRate'/>
            </FX_Rate>

            <Tax_Lot_ID>
              <xsl:value-of select ="''"/>
            </Tax_Lot_ID>


            <Versus_Date>
              <xsl:value-of select ="''"/>
            </Versus_Date>

            <Original_Face>
              <xsl:value-of select ="''"/>
            </Original_Face>

            <ContractSize>
              <xsl:value-of select ="''"/>
            </ContractSize>

            <IssueCountry>
              <xsl:value-of select ="0"/>
            </IssueCountry>

            <InstrumentCurrency>
              <xsl:value-of select ="''"/>
            </InstrumentCurrency>

            <AccrualStartDate>
              <xsl:value-of select ="0"/>
            </AccrualStartDate>

            <FirstCouponDate>
              <xsl:value-of select ="FirstCouponDate"/>
            </FirstCouponDate>


            <CouponRate>
              <xsl:value-of select ="Coupon"/>
            </CouponRate>

            <CouponFrequency>
              <xsl:value-of select ="Frequency"/>
            </CouponFrequency>

            <FirstCouponDateFloat>
              <xsl:value-of select ="''"/>
            </FirstCouponDateFloat>

            <CouponRateFloat>
              <xsl:value-of select ="''"/>
            </CouponRateFloat>

            <CouponFrequencyFloat>
              <xsl:value-of select ="''"/>
            </CouponFrequencyFloat>

            <FirstResetDateFloat>
              <xsl:value-of select ="''"/>
            </FirstResetDateFloat>


            <FirstResetFreqFloat>
              <xsl:value-of select ="''"/>
            </FirstResetFreqFloat>

            <LXID>
              <xsl:value-of select ="''"/>
            </LXID>

            <FacilityCouponFrequency>
              <xsl:value-of select="''"/>
            </FacilityCouponFrequency>

            <FaciltyCouponRate>
              <xsl:value-of select="''"/>
            </FaciltyCouponRate>

            <Issuer>
              <xsl:value-of select ="''"/>
            </Issuer>

            <Tranche>
              <xsl:value-of select ="''"/>
            </Tranche>

            <GlobalAmount>
              <xsl:value-of select="''"/>
            </GlobalAmount>


            <ContractAmount>
              <xsl:value-of select ="''"/>
            </ContractAmount>

            <ContractFrequency>
              <xsl:value-of select ="''"/>
            </ContractFrequency>

            <ContractRate>
              <xsl:value-of select ="''"/>
            </ContractRate>


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
