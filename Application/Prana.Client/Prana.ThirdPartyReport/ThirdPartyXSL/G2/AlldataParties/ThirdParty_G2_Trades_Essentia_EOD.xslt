<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name = "PB_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>


          <xsl:variable name ="PRANA_FUND_NAME">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
          </xsl:variable>
          <PortfolioCode>
            <xsl:value-of select ="AccountNo"/>
          </PortfolioCode>


          <PortfolioName>
            <xsl:choose>
              <xsl:when test ="$PRANA_FUND_NAME!=''">
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select ="$PB_FUND_NAME"/>
              </xsl:otherwise>

            </xsl:choose>
          </PortfolioName>

          <TradeDate>
            <xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),'-',substring-before(TradeDate,'/'),'-',substring-before(substring-after(TradeDate,'/'),'/'))"/>
          </TradeDate>


          <TradeIdentifier>
            <xsl:value-of select="PBUniqueID"/>
          </TradeIdentifier>


          <TradeQuantity>
            <xsl:choose>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="(AllocatedQty *(-1))"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="AllocatedQty"/>
              </xsl:otherwise>
            </xsl:choose>

          </TradeQuantity>


          <TradePrice>
            <xsl:value-of select="AveragePrice"/>
          </TradePrice>

          <TradeCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </TradeCurrency>

          <xsl:variable name="varCommission" select="(CommissionCharged)+(SoftCommissionCharged)"/>
          <Commission>
            <xsl:value-of select="$varCommission"/>
          </Commission>

          <CommissionCurrency>
            <xsl:value-of select="''"/>
          </CommissionCurrency>

          <xsl:variable name = "varOthFees">
            <xsl:value-of select="(StampDuty + TaxOnCommissions + ClearingFee + OtherBrokerFee + MiscFees + TransactionLevy + OrfFee + SecFee)"/>
          </xsl:variable>
          <OtherFees>
            <xsl:value-of select="''"/>
          </OtherFees>

          <OtherFeesCurrency>
            <xsl:value-of select="''"/>
          </OtherFeesCurrency>

          <AssetClass>
            <xsl:value-of select="Asset"/>
          </AssetClass>

          <xsl:variable name="varSecType">
            <xsl:choose>
              <xsl:when test ="Asset='Equity'">
                <xsl:value-of select="'Stock'"/>
              </xsl:when>

              <xsl:when test ="Asset='EquityOption'">
                <xsl:value-of select="'Stock'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <InstrumentType>
            <xsl:value-of select="$varSecType"/>
          </InstrumentType>

          <TradeDescription>
            <xsl:value-of select="FullSecurityName"/>
          </TradeDescription>

          <TradeSymbol>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="SEDOL"/>
              </xsl:otherwise>
            </xsl:choose>

          </TradeSymbol>

          <TradeSymbolType>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'OSI'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'SEDOL'"/>
              </xsl:otherwise>
            </xsl:choose>
          </TradeSymbolType>

          <DisplaySymbol>
            <xsl:value-of select="''"/>
          </DisplaySymbol>

          <TradeFlag>
            <xsl:value-of select="''"/>
          </TradeFlag>


          
 <CorporateActionType>
           <xsl:choose>              
               <xsl:when test="TradeAttribute2='IPO'">
                 <xsl:value-of select="'IPO'"/>
               </xsl:when>
               <xsl:when test="CounterParty='CorpAction'">
                 <xsl:value-of select="'CorpAction'"/>
               </xsl:when>        
             <xsl:otherwise>
               <xsl:choose>
                 <xsl:when test="Asset='EquityOption' or Asset='Equity'">
                   <xsl:choose>
                     <xsl:when test="TransactionType='Exercise'">
                       <xsl:value-of select="'Exercise'"/>
                     </xsl:when>
                     <xsl:when test="TransactionType='Assignment'">
                       <xsl:value-of select="'Assignment'"/>
                     </xsl:when>
                     <xsl:when test="TransactionType='Expire'">
                       <xsl:value-of select="'Expire'"/>
                     </xsl:when>
                     <xsl:otherwise>
                       <xsl:value-of select="''"/>
                     </xsl:otherwise>
                   </xsl:choose>
                 </xsl:when>
               </xsl:choose>
             </xsl:otherwise>
           </xsl:choose>
         </CorporateActionType>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>