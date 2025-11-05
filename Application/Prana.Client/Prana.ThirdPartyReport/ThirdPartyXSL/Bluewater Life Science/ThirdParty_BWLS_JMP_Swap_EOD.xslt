<?xml version="1.0" encoding="UTF-8"?>

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
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
     
      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset='Equity' and IsSwapped='true']">
        <ThirdPartyFlatFileDetail>
          <RowHeader>
            <xsl:value-of select ="'True'"/>
          </RowHeader>
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <FileHeader>
            <xsl:value-of select ="'true'"/>
          </FileHeader>

          <FileFooter>
            <xsl:value-of select ="'true'"/>
          </FileFooter>


          <xsl:variable name="varMethod">
            <xsl:choose>
              <xsl:when test="IsSwapped='true' and Asset = 'Equity'">
                <xsl:value-of select ="'SWAP'"/>
              </xsl:when>
              <xsl:when test="contains(Symbol,'-') != true and Asset = 'Equity'">
                <xsl:value-of select ="'INTL'"/>
              </xsl:when>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="'PRIME'"/>
              </xsl:when>
              <xsl:when test="Asset = 'EquityOption'">
                <xsl:value-of select="'CMTA'"/>
              </xsl:when>
              <xsl:when test="(Asset = 'PrivateEquity' or Asset = 'FixedIncome') and CurrencySymbol!='USD'">
                <xsl:value-of select="'INTL'"/>
              </xsl:when>

              <xsl:when test="(Asset = 'PrivateEquity' or Asset = 'FixedIncome') and CurrencySymbol ='USD'">
                <xsl:value-of select="'PRIME'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <TRADEID>
            <xsl:value-of select="concat($varMethod,PBUniqueID)"/>
          </TRADEID>

          <BLOCKID>
            <xsl:value-of select="''"/>
          </BLOCKID>


          <xsl:variable name="varTaxlotStateTx">
            <xsl:choose>
              <xsl:when test="TaxLotState='Allocated'">
                <xsl:value-of select ="'NEW'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Amended'">
                <xsl:value-of select ="'MODIFY'"/>
              </xsl:when>
              <xsl:when test="TaxLotState='Deleted'">
                <xsl:value-of select ="'CANCEL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <ACTION>
            <xsl:value-of select="$varTaxlotStateTx"/>
          </ACTION>

          <TRADEDATE>
            <xsl:value-of select="TradeDate"/>
          </TRADEDATE>

          <SETTLEDATE>
            <xsl:value-of select="SettlementDate"/>
          </SETTLEDATE>


          <xsl:variable name="varAccountName">
            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="'31750468'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="'10238924'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <ACCOUNT>           
            <xsl:value-of select="AccountNo"/>
          </ACCOUNT>

          <METHOD>
            <xsl:value-of select="'SWAP'"/>
          </METHOD>


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
              <xsl:when test="(Side='Buy to Cover' or Side='Buy to Close') and Asset = 'Equity'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>             
              <xsl:when test="Side='Sell to Open'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Open'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <SIDE>
            <xsl:value-of select="$Sidevar"/>
          </SIDE>



          <xsl:variable name="varSymbol">
            <xsl:choose>
              <xsl:when test="SEDOL != ''">               
                <xsl:value-of select="SEDOL"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:when test="BBCode != ''">               
                <xsl:value-of select="BBCode"/>
              </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select ="Symbol"/>
                  </xsl:otherwise>
                </xsl:choose>
         
          </xsl:variable>
          <SECURITY>
            <xsl:value-of select="$varSymbol"/>
          </SECURITY>


          <SECID>
            <xsl:choose>
              <xsl:when test="SEDOL != ''">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:when test="CUSIP != ''">
                <xsl:value-of select="''"/>
              </xsl:when>

              <xsl:when test="BBCode = ''">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SECID>


          <QUANTITY>
            <xsl:value-of select="AllocatedQty"/>
          </QUANTITY>

          <PRICE>
            <xsl:value-of select="format-number((NetAmount div AllocatedQty),'0.######')"/>
          </PRICE>

          <xsl:variable name="Commission">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

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

          <xsl:variable name="Commission1">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="format-number($Commission,'##.00')"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='M'">
                <xsl:value-of select="format-number($Commission * $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Trade='D'">
                <xsl:value-of select="format-number($Commission div $varFXRate,'##.00')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>

              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varCommissionAmount">
            <xsl:choose>
              <xsl:when test="Asset = 'Equity' or Asset = 'EquityOption'">
                <xsl:value-of select="$Commission1"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <COMMTYPE>
            <xsl:choose>
              <xsl:when test="CommissionCharged !='' and CommissionCharged !=0">
                <xsl:value-of select ="'TOTAL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>

          </COMMTYPE>

          <xsl:variable name="varTotalCom">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>
          <COMM>

            <xsl:choose>
              <xsl:when test="Asset='Equity' and IsSwapped='true'">
                <xsl:value-of select="''"/>
              </xsl:when>
              
              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
           
          </COMM>

          <INTEREST>
            <xsl:value-of select="format-number(AccruedInterest,'#.##')"/>
          </INTEREST>


          <xsl:variable name="PRANA_COUNTERPARTY">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name="PB_COUNTERPARTY">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name = 'GS']/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@MLPBroker"/>
          </xsl:variable>

          <xsl:variable name="varCounterParty">
            <xsl:choose>
              <xsl:when test="$PB_COUNTERPARTY = ''">
                <xsl:value-of select="$PRANA_COUNTERPARTY"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PB_COUNTERPARTY"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <EXECBRKR>
            <xsl:value-of select="$varCounterParty"/>
          </EXECBRKR>

          <CURRENCY>
            <xsl:value-of select="SettlCurrency"/>
          </CURRENCY>

          <CONVCURR>
            <xsl:value-of select="''"/>
          </CONVCURR>

          <PUTCALL>
            <xsl:value-of select="''"/>
          </PUTCALL>

          <STRIKE>
            <xsl:value-of select="''"/>
          </STRIKE>

          <EXPDATE>
            <xsl:value-of select="''"/>
          </EXPDATE>

          <PORTFOLIOSWAP>
            <xsl:value-of select="concat('NS8_',SettlCurrency)"/>
          </PORTFOLIOSWAP>

          <EXCHNGRATE>
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </EXCHNGRATE>

          <STRATEGY>
            <xsl:value-of select="''"/>
          </STRATEGY>

          <SECCLEARANCECODE>
            <xsl:value-of select="''"/>
          </SECCLEARANCECODE>

          <IMPACTNET>
            <xsl:value-of select="''"/>
          </IMPACTNET>
          
          <PREFIGUREDPRINCIPAL>
            <xsl:value-of select="''"/>
          </PREFIGUREDPRINCIPAL>

          <COUNTRY>
            <xsl:value-of select="''"/>
          </COUNTRY>

          <TAXAMOUNT>
            <xsl:value-of select="''"/>
          </TAXAMOUNT>

          <TCODE1>
            <xsl:value-of select="''"/>
          </TCODE1>

          <DESC1>
            <xsl:value-of select="''"/>
          </DESC1>
         

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>

