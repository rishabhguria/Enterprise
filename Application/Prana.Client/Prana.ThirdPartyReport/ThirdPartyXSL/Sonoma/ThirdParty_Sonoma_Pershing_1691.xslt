<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>
    <xsl:choose>
      <xsl:when test ="$varMonth=01">
        <xsl:value-of select ="'JAN'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=02">
        <xsl:value-of select ="'FEB'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=03">
        <xsl:value-of select ="'MAR'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=04">
        <xsl:value-of select ="'APR'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=05">
        <xsl:value-of select ="'MAY'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=06">
        <xsl:value-of select ="'JUN'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=07">
        <xsl:value-of select ="'JUL'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=08">
        <xsl:value-of select ="'AUG'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=09">
        <xsl:value-of select ="'SEP'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=10">
        <xsl:value-of select ="'OCT'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=11">
        <xsl:value-of select ="'NOV'"/>
      </xsl:when>
      <xsl:when test ="$varMonth=12">
        <xsl:value-of select ="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          <!--for system use only-->
          <FileHeader>
            <xsl:value-of select ="'true'"/>
          </FileHeader>
          <!--for system use only-->
          <FileFooter>
            <xsl:value-of select ="'true'"/>
          </FileFooter>
          <!--for system use only-->
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>
          <!-- system inetrnal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <LOCALREF>
            <xsl:value-of select ="concat(substring(TradeDate,1,2),substring(TradeDate,4,2),substring(TradeDate,9,2),position())"/>
          </LOCALREF>

          <CFID>
            <xsl:value-of select ="concat(substring(TradeDate,1,2),substring(TradeDate,4,2),substring(TradeDate,9,2),position())"/>
          </CFID>

          <ROUTECD>
            <xsl:value-of select ="'PSHG'"/>
          </ROUTECD>

          <TIRORDERID>
            <xsl:value-of select ="concat(PBUniqueID,position())"/>
          </TIRORDERID>

          <TIRPIECE>
            <xsl:value-of select ="''"/>
          </TIRPIECE>

          <TIRSEQ>
            <xsl:value-of select ="''"/>
          </TIRSEQ>

          <xsl:variable name ="varCheckSymbolUnderlying">
            <xsl:value-of select ="substring-before(Symbol,'-')"/>
          </xsl:variable>

          <xsl:choose>
            <xsl:when test="ISIN != '' and $varCheckSymbolUnderlying != '' and Asset != 'FX'">
              <SECIDTYPE>
                <xsl:value-of select ="'N'"/>
              </SECIDTYPE>
              <SECURITYID>
                <xsl:value-of select ="ISIN"/>
              </SECURITYID>
            </xsl:when>
            <xsl:when test="Asset = 'EquityOption'">
              <SECIDTYPE>
                <xsl:value-of select ="'S'"/>
              </SECIDTYPE>
              <SECURITYID>
                <xsl:value-of select ="OSIOptionSymbol"/>
              </SECURITYID>
            </xsl:when>
            <xsl:otherwise>
              <SECIDTYPE>
                <xsl:value-of select ="'S'"/>
              </SECIDTYPE>
              <SECURITYID>
                <xsl:value-of select ="Symbol"/>
              </SECURITYID>
            </xsl:otherwise>
          </xsl:choose>

          <DESCRIPTION1>
            <xsl:value-of select ="concat('PB ',CounterParty)"/>
          </DESCRIPTION1>

          <DESCRIPTION2>
            <xsl:value-of select ="''"/>
          </DESCRIPTION2>

          <DESCRIPTION3>
            <xsl:value-of select ="''"/>
          </DESCRIPTION3>

          <DESCRIPTION4>
            <xsl:value-of select ="''"/>
          </DESCRIPTION4>

          <xsl:variable name = "varMonthCodeTrade" >
            <xsl:call-template name="MonthCode">
              <xsl:with-param name="varMonth" select="substring(TradeDate,1,2)" />
            </xsl:call-template>
          </xsl:variable>

          <TRADEDATE>
            <xsl:value-of select ="concat(substring(TradeDate,4,2),'-',$varMonthCodeTrade,'-',substring(TradeDate,9,2))"/>
          </TRADEDATE>

          <xsl:variable name = "varMonthCodeSettle" >
            <xsl:call-template name="MonthCode">
              <xsl:with-param name="varMonth" select="substring(SettlementDate,1,2)" />
            </xsl:call-template>
          </xsl:variable>

          <SETLDATE>
            <xsl:value-of select ="concat(substring(SettlementDate,4,2),'-',$varMonthCodeSettle,'-',substring(SettlementDate,9,2))"/>
          </SETLDATE>

          <QUANTITY>
            <xsl:value-of select ="AllocatedQty"/>
          </QUANTITY>

          <QUANTITYDESC>
            <xsl:value-of select ="''"/>
          </QUANTITYDESC>

          <NETMONEY>
            <xsl:value-of select ="''"/>
          </NETMONEY>

					<!--<xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell' or Side='Sell to Close'">
							<CASHACCOUNT>
								<xsl:value-of select="concat(FundAccountNo,'2')"/>
							</CASHACCOUNT>
						</xsl:when>
            <xsl:when test="Side='Buy to Close' or Side='Buy to Cover' or Side='Sell short' or Side='Sell to Open'">
              <CASHACCOUNT>
								<xsl:value-of select="concat(FundAccountNo,'3')"/>
              </CASHACCOUNT>
            </xsl:when>           
            <xsl:otherwise>
              <CASHACCOUNT>
                <xsl:value-of select="FundAccountNo"/>
              </CASHACCOUNT>
            </xsl:otherwise>
					</xsl:choose>

          <xsl:choose>
						<xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Sell' or Side='Sell to Close'">
							<SECACCOUNT>
								<xsl:value-of select="concat(FundAccountNo,'2')"/>
							</SECACCOUNT>
						</xsl:when>
						<xsl:when test="Side='Buy to Close' or Side='Buy to Cover' or Side='Sell short' or Side='Sell to Open'">
							<SECACCOUNT>
								<xsl:value-of select="concat(FundAccountNo,'3')"/>
							</SECACCOUNT>
						</xsl:when>
            <xsl:otherwise>
              <SECACCOUNT>
								<xsl:value-of select="FundAccountNo"/>
              </SECACCOUNT>
            </xsl:otherwise>
          </xsl:choose>-->

          <CASHACCOUNT>
            <xsl:value-of select="FundMappedName"/>
          </CASHACCOUNT>


          <SECACCOUNT>
            <xsl:value-of select="FundAccountNo"/>
          </SECACCOUNT>

          <TRADECURRID>
            <xsl:value-of select ="CurrencySymbol"/>
          </TRADECURRID>

          <SETLCURRID>
            <xsl:value-of select ="CurrencySymbol"/>
          </SETLCURRID>

          <!--   Side     -->
          <xsl:choose>
            <xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
              <BSIND>
                <xsl:value-of select="'B'"/>
              </BSIND>
            </xsl:when>
            <xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
              <BSIND>
                <xsl:value-of select="'S'"/>
              </BSIND>
            </xsl:when>
            <xsl:otherwise>
              <BSIND>
                <xsl:value-of select="''"/>
              </BSIND>
            </xsl:otherwise>
          </xsl:choose>

          <!--   Side End    -->

          <xsl:choose>
            <xsl:when test="TaxLotState='Deleted'">
              <INSTTYP>
                <xsl:value-of select ="'Y'"/>
              </INSTTYP>
            </xsl:when>
            <xsl:otherwise>
              <INSTTYP>
                <xsl:value-of select ="'N'"/>
              </INSTTYP>
            </xsl:otherwise>
          </xsl:choose>

          <PRICE>
            <xsl:value-of select ="AveragePrice"/>
          </PRICE>

          <COMMISSION>
            <xsl:value-of select ="CommissionCharged"/>
          </COMMISSION>

          <STAMPTAX>
            <xsl:value-of select ="''"/>
          </STAMPTAX>

          <LOCALCHGS>
            <xsl:value-of select ="''"/>
          </LOCALCHGS>

          <INTEREST>
            <xsl:value-of select ="''"/>
          </INTEREST>

          <PRINCIPAL>
            <xsl:value-of select ="''"/>
          </PRINCIPAL>

          <SECFEE>
            <xsl:value-of select ="''"/>
          </SECFEE>

          <EXECBROKER>
            <xsl:value-of select ="''"/>
          </EXECBROKER>

          <BROKEROS>
            <xsl:value-of select ="''"/>
          </BROKEROS>

          <TRAILERCD1>
            <xsl:value-of select ="''"/>
          </TRAILERCD1>

          <TRAILERCD2>
            <xsl:value-of select ="''"/>
          </TRAILERCD2>

          <TRAILERCD3>
            <xsl:value-of select ="''"/>
          </TRAILERCD3>

          <xsl:choose>
            <xsl:when test ="$varCheckSymbolUnderlying != ''">
              <BLOTTERCD>
                <xsl:value-of select ="'40'"/>
              </BLOTTERCD>
            </xsl:when>
            <xsl:otherwise>
              <BLOTTERCD>
                <xsl:value-of select ="'49'"/>
              </BLOTTERCD>
            </xsl:otherwise>
          </xsl:choose>


          <CLRNGHSE>
            <xsl:value-of select ="'Y'"/>
          </CLRNGHSE>

          <CLRAGNTCD>
            <xsl:value-of select ="CounterParty"/>
          </CLRAGNTCD>

          <CLRAGNT1>
            <xsl:value-of select ="''"/>
          </CLRAGNT1>

          <CLRAGNT2>
            <xsl:value-of select ="''"/>
          </CLRAGNT2>

          <CLRAGNT3>
            <xsl:value-of select ="''"/>
          </CLRAGNT3>

          <CLRAGNT4>
            <xsl:value-of select ="''"/>
          </CLRAGNT4>

          <CNTRPRTYCD>
            <xsl:value-of select ="''"/>
          </CNTRPRTYCD>

          <CNTRPTY1>
            <xsl:value-of select ="''"/>
          </CNTRPTY1>

          <CNTRPTY2>
            <xsl:value-of select ="''"/>
          </CNTRPTY2>

          <CNTRPTY3>
            <xsl:value-of select ="''"/>
          </CNTRPTY3>

          <CNTRPTY4>
            <xsl:value-of select ="''"/>
          </CNTRPTY4>

          <INSTRUCT>
            <xsl:value-of select ="''"/>
          </INSTRUCT>

          <CEDELAKV>
            <xsl:value-of select ="''"/>
          </CEDELAKV>

          <ORIGLOCALREF>
            <xsl:value-of select ="''"/>
          </ORIGLOCALREF>

          <NOTES>
            <xsl:value-of select ="''"/>
          </NOTES>

          <FILLER>
            <xsl:value-of select ="''"/>
          </FILLER>

          <FILLER1>
            <xsl:value-of select ="''"/>
          </FILLER1>

          <RR>
            <xsl:value-of select ="''"/>
          </RR>

          <SETLCOUNTRYCD>
            <xsl:value-of select ="'US'"/>
          </SETLCOUNTRYCD>

          <INSTRUMENTTYPE>
            <xsl:value-of select ="''"/>
          </INSTRUMENTTYPE>

          <COMMISSIONRATE>
            <xsl:value-of select ="''"/>
          </COMMISSIONRATE>

          <COMPANYNO>
            <xsl:value-of select ="''"/>
          </COMPANYNO>

          <Filler2>
            <xsl:value-of select ="''"/>
          </Filler2>

          <Filler3>
            <xsl:value-of select ="''"/>
          </Filler3>

          <Filler4>
            <xsl:value-of select ="''"/>
          </Filler4>

          <Filler5>
            <xsl:value-of select ="''"/>
          </Filler5>

          <Filler6>
            <xsl:value-of select ="''"/>
          </Filler6>

          <Filler7>
            <xsl:value-of select ="''"/>
          </Filler7>

          <GPF2IDCode>
            <xsl:value-of select ="''"/>
          </GPF2IDCode>

          <GPF2Amount>
            <xsl:value-of select ="''"/>
          </GPF2Amount>

          <GPF2CurrencyCode>
            <xsl:value-of select ="''"/>
          </GPF2CurrencyCode>

          <GPF2AddSubtract>
            <xsl:value-of select ="''"/>
          </GPF2AddSubtract>

          <GPF3IDCode>
            <xsl:value-of select ="''"/>
          </GPF3IDCode>

          <GPF3Amount>
            <xsl:value-of select ="''"/>
          </GPF3Amount>

          <GPF3CurrencyCode>
            <xsl:value-of select ="''"/>
          </GPF3CurrencyCode>

          <GPF4IDCode>
            <xsl:value-of select ="''"/>
          </GPF4IDCode>

          <GPF4Amount>
            <xsl:value-of select ="''"/>
          </GPF4Amount>

          <GPF4CurrencyCode>
            <xsl:value-of select ="''"/>
          </GPF4CurrencyCode>

          <GPF4AddSubtract>
            <xsl:value-of select ="''"/>
          </GPF4AddSubtract>

          <GPF5IDCode>
            <xsl:value-of select ="''"/>
          </GPF5IDCode>

          <GPF5Amount>
            <xsl:value-of select ="''"/>
          </GPF5Amount>

          <GPF5CurrencyCode>
            <xsl:value-of select ="''"/>
          </GPF5CurrencyCode>

          <GPF5AddSubtract>
            <xsl:value-of select ="''"/>
          </GPF5AddSubtract>

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
