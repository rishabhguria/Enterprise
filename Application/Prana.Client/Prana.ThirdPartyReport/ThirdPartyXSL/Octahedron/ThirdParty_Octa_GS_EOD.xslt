<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public int RoundOff(double Qty)
    {

    return (int)Math.Round(Qty,0);
    }
  </msxsl:script>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[IsSwapped='false']">
        <ThirdPartyFlatFileDetail>
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>


          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="Sidevar">
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
                <xsl:value-of select="'BuyToClose'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Open'">
                <xsl:value-of select="'SellToOpen'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close'">
                <xsl:value-of select="'SellToClose'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Open'">
                <xsl:value-of select="'BuyToOpen'"/>
              </xsl:when>
              <xsl:otherwise> </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <xsl:variable name="varTransactionType">
            <xsl:choose>
              <xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell' or Side='Sell to Close'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side = 'Buy to Close' or Side='Buy to Cover'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varClientAccount">
            <xsl:choose>
              <xsl:when test ="AccountName = 'Octahedron Fund GS'">
                <xsl:value-of select ="'065333031'"/>
              </xsl:when>
              <xsl:when test ="AccountName = 'Octahedron Fund GS Swap'">
                <xsl:value-of select ="'064390842'"/>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name ="varAccountNo">
            <xsl:choose>
              <xsl:when test ="AccountName= 'Octahedron Fund GS'">
                <xsl:value-of select ="'065333031'"/>
              </xsl:when>
              <xsl:when test ="AccountName= 'Octahedron Fund GS Swap'">
                <xsl:value-of select ="'064390842'"/>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varInstruction">
            <xsl:choose>
              <xsl:when test="TaxLotState = 'Allocated'">
                <xsl:value-of select="'N'"/>
              </xsl:when>
              <xsl:when test="TaxLotState = 'Amended'">
                <xsl:value-of select="'A'"/>
              </xsl:when>
              <xsl:when test="TaxLotState = 'Deleted'">
                <xsl:value-of select="'C'"/>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varSecurityID">
            <xsl:choose>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:when test="Asset = 'FixedIncome'">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
			    <xsl:when test="Asset='Equity' and IsSwapped='true' ">
                <xsl:value-of select="RIC"/>
              </xsl:when>
              <xsl:when test="ISIN != ''">
                <xsl:value-of select="ISIN"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varFeeType1">
            <xsl:choose>
              <xsl:when test="number(StampDuty)">
                <xsl:value-of select="'SEC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varFeeValue1">
            <xsl:choose>
              <xsl:when test="number(StampDuty)">
                <xsl:value-of select="StampDuty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varFeeType2">
            <xsl:choose>
              <xsl:when test="number(MiscFees)">
                <xsl:value-of select="'MSCF'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varFeeValue2">
            <xsl:choose>
              <xsl:when test="number(MiscFees)">
                <xsl:value-of select="MiscFees"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name ="varUnderlyingSymbol">
            <xsl:choose>
              <xsl:when test ="contains(Asset, 'Option')!= false">
                <xsl:value-of select ="UnderlyingSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name ="varStrikePrice">
            <xsl:choose>
              <xsl:when test ="contains(Asset, 'Option')!= false">
                <xsl:value-of select ="StrikePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable  name="PB_NAME">
            <xsl:value-of select="'GS'"/>
          </xsl:variable>

          <xsl:variable name = "PB_SYMBOL_NAME">
            <xsl:value-of select="Symbol"/>
          </xsl:variable>
          <xsl:variable name="PRANA_SYMBOL_NAME">
            <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
          </xsl:variable>

          <xsl:variable name="varSymbol">
            <xsl:choose>
              <xsl:when test ="$PRANA_SYMBOL_NAME!=''">
                <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
              </xsl:when>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
			    <xsl:when test="Asset='Equity' and IsSwapped='true' ">
                <xsl:value-of select="RIC"/>
              </xsl:when>
			    <xsl:when test="Asset = 'FixedIncome'">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:when test="SEDOL != '*' and SEDOL != ''">
                <xsl:value-of select="SEDOL"/>
              </xsl:when>

              <xsl:when test="ISIN != '*' and ISIN != ''">
                <xsl:value-of select="ISIN"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select ="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name ="SwapCheck">
            <xsl:if test="Asset='Equity' and IsSwapped='true'">
              <xsl:value-of select="'SWAP'"/>
            </xsl:if>
          </xsl:variable>




          <xsl:variable name ="UDA_Country">
            <xsl:value-of select ="UDACountryName"/>
          </xsl:variable>


          <xsl:variable name ="varTradeTax">
            <xsl:choose>
              <xsl:when test ="CurrencySymbol != 'USD'">
                <xsl:value-of select ="StampDuty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>



          <xsl:variable name="varSettFxAmt">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
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
                <xsl:value-of select="format-number(AveragePrice,'###.######')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="format-number($varSettFxAmt,'###.######')"/>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:variable>

          <xsl:variable name="varAccrued">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="AccruedInterest * FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="AccruedInterest div FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="AccruedInterest"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varAccruedInterest">
            <xsl:choose>
              <xsl:when test="SettlCurrency = CurrencySymbol">
                <xsl:value-of select="AccruedInterest"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varAccrued"/>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:variable>



          <xsl:variable name="varSettCommission">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="(CommissionCharged + SoftCommissionCharged) * FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="(CommissionCharged + SoftCommissionCharged) div FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="(CommissionCharged + SoftCommissionCharged)"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name ="varCommission">
            <xsl:choose>
              <xsl:when test ="number($varSettCommission)">
                <xsl:value-of select="format-number($varSettCommission,'###.######')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>



          <xsl:variable name="varSettFxNetAmount">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="NetAmount * FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="NetAmount div FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="NetAmount"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varNetAmount">
            <xsl:choose>
              <xsl:when test="SettlCurrency = CurrencySymbol">
                <xsl:value-of select="NetAmount"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varSettFxNetAmount"/>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:variable>

          <xsl:variable name="varCpty">
            <xsl:choose>
              <xsl:when test="CounterParty= 'JEFF' or CounterParty= 'ZJEFF'">
                <xsl:value-of select="'JEFF'"/>
              </xsl:when>
              <xsl:when test="CounterParty= 'CITI' or CounterParty= 'ZCITI'">
                <xsl:value-of select="'CITI'"/>
              </xsl:when>
              <xsl:when test="CounterParty= 'JPMS' or CounterParty= 'ZJPMS'">
                <xsl:value-of select="'JPMS'"/>
              </xsl:when>
              <xsl:when test="CounterParty= 'GS' or CounterParty= 'ZGS'">
                <xsl:value-of select="'GS'"/>
              </xsl:when>
              <xsl:when test="CounterParty= 'BERN' or CounterParty= 'ZBERN'">
                <xsl:value-of select="'BERN'"/>
              </xsl:when>
              <xsl:when test="CounterParty= 'CS' or CounterParty= 'ZCS'">
                <xsl:value-of select="'CS'"/>
              </xsl:when>
              <xsl:when test="CounterParty= 'ITGI' or CounterParty= 'ZITGI'">
                <xsl:value-of select="'ITGI'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CounterParty"/>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:variable>


          <xsl:variable name="varCurr">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="SettlCurrency"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CurrencySymbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varProductCode">
            <xsl:choose>
			<xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'SWAP'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <xsl:variable name="ExpiryDate">
            <xsl:choose>
              <xsl:when test="contains(Asset,'Option')">
                <xsl:value-of select="ExpirationDate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name ="varTaxlotState">
            <xsl:value-of select="'Allocated'"/>
          </xsl:variable>

          <xsl:variable name="Stamp">
            <xsl:choose>
              <xsl:when test="CurrencySymbol = 'USD' or CurrencySymbol = 'JPY' or CurrencySymbol = 'GBP' or CurrencySymbol = 'EUR'">
                <xsl:value-of select="StampDuty + TransactionLevy + OrfFee + ClearingFee + MiscFees + TaxOnCommissions"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varStamp">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Taxlot ='M'">
                    <xsl:value-of select="$Stamp * FXRate_Taxlot"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$Stamp div FXRate_Taxlot"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$Stamp"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name ="varATradeTax">
            <xsl:choose>
              <xsl:when test ="number($varStamp)">
                <xsl:value-of select="format-number($varStamp,'###.######')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <OrderNumber>
            <xsl:value-of select="PBUniqueID"/>
          </OrderNumber>


          <Cancelcorrectindicator>
            <xsl:value-of select="$varInstruction"/>
          </Cancelcorrectindicator>

          <AccountNumber>
            <xsl:value-of select="$varClientAccount"/>
          </AccountNumber>

          <SecurityIdentifier>
            <xsl:value-of select="$varSymbol"/>
          </SecurityIdentifier>

          <Broker>
            <xsl:value-of select="$varCpty"/>
          </Broker>

          <Custodian>
            <xsl:value-of select="'GSCO'"/>
          </Custodian>

          <TransactionType>
            <xsl:value-of select="$varTransactionType"/>
          </TransactionType>

          <CurrencyCode>
            <xsl:value-of select="$varCurr"/>
          </CurrencyCode>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select="SettlementDate"/>
          </SettleDate>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <Commission>
            <xsl:value-of select="$varCommission"/>
          </Commission>

          <Price>
            <xsl:value-of select="$varPrice"/>
          </Price>

          <AccruedInterest>
            <xsl:value-of select="$varAccruedInterest"/>
          </AccruedInterest>

          <TradeTax>
            <xsl:value-of select="''"/>
          </TradeTax>


          <MiscMoney>
            <xsl:value-of select="''"/>
          </MiscMoney>


          <NetAmount>
            <xsl:value-of select="$varNetAmount"/>
          </NetAmount>


          <Principal>
            <xsl:value-of select="GrossAmount"/>
          </Principal>


          <Description>
            <xsl:value-of select="''"/>
          </Description>


          <SecurityType>
            <xsl:value-of select="$varProductCode"/>
          </SecurityType>


          <CountrySettlementCode>
            <xsl:value-of select="''"/>
          </CountrySettlementCode>


          <ClearingAgent>
            <xsl:value-of select="''"/>
          </ClearingAgent>


          <SECFee>
            <xsl:value-of select="format-number(SecFee,'###.######')"/>
          </SECFee>


          <RepoOpenSettleDate>
            <xsl:value-of select="''"/>
          </RepoOpenSettleDate>


          <RepoMaturityDate>
            <xsl:value-of select="''"/>
          </RepoMaturityDate>


          <RepoRate>
            <xsl:value-of select="''"/>
          </RepoRate>


          <RepoInterest>
            <xsl:value-of select="''"/>
          </RepoInterest>


          <OptionUnderlyer>
            <xsl:value-of select="$varUnderlyingSymbol"/>
          </OptionUnderlyer>


          <OptionExpiryDate>
            <xsl:value-of select="$ExpiryDate"/>
          </OptionExpiryDate>


          <OptionCallPutIndicator>
            <xsl:value-of select="substring(PutOrCall,1,1)"/>
          </OptionCallPutIndicator>

          <OptionStrikePrice>
            <xsl:value-of select="$varStrikePrice"/>
          </OptionStrikePrice>

          <Trailer>
            <xsl:value-of select="''"/>
          </Trailer>

          <GenevaLotNumber1>
            <xsl:value-of select="''"/>
          </GenevaLotNumber1>

          <GainsKeeperLotNumber1>
            <xsl:value-of select="''"/>
          </GainsKeeperLotNumber1>

          <LotDate1>
            <xsl:value-of select="''"/>
          </LotDate1>

          <LotQty1>
            <xsl:value-of select="''"/>
          </LotQty1>

          <LotPrice1>
            <xsl:value-of select="''"/>
          </LotPrice1>

          <GenevaLotNumber2>
            <xsl:value-of select="''"/>
          </GenevaLotNumber2>

          <GainsKeeperLotNumber2>
            <xsl:value-of select="''"/>
          </GainsKeeperLotNumber2>

          <LotDate2>
            <xsl:value-of select="''"/>
          </LotDate2>

          <LotQty2>
            <xsl:value-of select="''"/>
          </LotQty2>

          <LotPrice2>
            <xsl:value-of select="''"/>
          </LotPrice2>

          <GenevaLotNumber3>
            <xsl:value-of select="''"/>
          </GenevaLotNumber3>

          <GainsKeeperLotNumber3>
            <xsl:value-of select="''"/>
          </GainsKeeperLotNumber3>

          <LotDate3>
            <xsl:value-of select="''"/>
          </LotDate3>

          <LotQty3>
            <xsl:value-of select="''"/>
          </LotQty3>


          <LotPrice3>
            <xsl:value-of select="''"/>
          </LotPrice3>


          <GenevaLotNumber4>
            <xsl:value-of select="''"/>
          </GenevaLotNumber4>


          <GainsKeeperLotNumber4>
            <xsl:value-of select="''"/>
          </GainsKeeperLotNumber4>


          <LotDate4>
            <xsl:value-of select="''"/>
          </LotDate4>


          <LotQty4>
            <xsl:value-of select="''"/>
          </LotQty4>


          <LotPrice4>
            <xsl:value-of select="''"/>
          </LotPrice4>


          <GenevaLotNumber5>
            <xsl:value-of select="''"/>
          </GenevaLotNumber5>

          <GainsKeeperLotNumber5>
            <xsl:value-of select="''"/>
          </GainsKeeperLotNumber5>

          <LotDate5>
            <xsl:value-of select="''"/>
          </LotDate5>

          <LotQty5>
            <xsl:value-of select="''"/>
          </LotQty5>

          <LotPrice5>
            <xsl:value-of select="''"/>
          </LotPrice5>



          <EntityID>
           <xsl:value-of select="EntityID"/>
         </EntityID>


        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
