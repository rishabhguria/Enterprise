<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
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
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name="varDividend">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL22"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($varDividend)">
          <PositionMaster>
            <xsl:variable name="PB_Name">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL2"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <xsl:variable name="PB_Symbol" select="COL8"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_Name]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="Symbol" select="normalize-space(COL5)"/>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test ="$Symbol!=''">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_Symbol"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="varAmount">
               <xsl:value-of select="COL22"/>
            </xsl:variable>
            <Amount>
              <xsl:choose>
                <xsl:when test="$varAmount &gt; 0">
                  <xsl:value-of select="$varAmount"/>
                </xsl:when>

                <xsl:when test="$varAmount &lt; 0">
                  <xsl:value-of select="$varAmount * -1"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Amount>


            <xsl:variable name="varEDate">
              <xsl:value-of select="COL12"/>
            </xsl:variable>
            <xsl:variable name="varEMonth">
              <xsl:value-of select="substring($varEDate,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varEDay">
              <xsl:value-of select="substring($varEDate,1,4)"/>
            </xsl:variable>
            <xsl:variable name="varEYear">
              <xsl:value-of select="substring($varEDate,7,2)"/>
            </xsl:variable>
            <ExDate>
              <xsl:value-of select="concat($varEMonth,'/',$varEDay,'/',$varEYear)"/>
            </ExDate>

            <Description>
              <xsl:value-of select="'Dividend'"/>
            </Description>
            <ActivityType>
              <xsl:choose>
                <xsl:when test="$varAmount &gt; 0">
                  <xsl:value-of select="'DividendIncome'"/>
                </xsl:when>

                <xsl:when test="$varAmount &lt; 0">
                  <xsl:value-of select="'DividendExpenses'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </ActivityType>

            <xsl:variable name="varCurrency">
              <xsl:value-of select="COL10"/>
            </xsl:variable>
            
            <CurrencyID>
              <xsl:choose>
                <xsl:when test="normalize-space($varCurrency) ='USD'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='HKD'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='JPY'">
                  <xsl:value-of select="'3'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='GBP'">
                  <xsl:value-of select="'4'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='AED'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='BRL'">
                  <xsl:value-of select="'6'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='CAD'">
                  <xsl:value-of select="'7'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='EUR'">
                  <xsl:value-of select="'8'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='NOK'">
                  <xsl:value-of select="'9'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='SGD'">
                  <xsl:value-of select="'10'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='MUL'">
                  <xsl:value-of select="'11'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='ZAR'">
                  <xsl:value-of select="'12'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='SEK'">
                  <xsl:value-of select="'13'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='AUD'">
                  <xsl:value-of select="'14'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='CNY'">
                  <xsl:value-of select="'15'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='KRW'">
                  <xsl:value-of select="'16'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='BDT'">
                  <xsl:value-of select="'17'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='THB'">
                  <xsl:value-of select="'18'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='dong'">
                  <xsl:value-of select="'19'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='GBX'">
                  <xsl:value-of select="'20'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='INR'">
                  <xsl:value-of select="'21'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='CHF'">
                  <xsl:value-of select="'23'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='CLP'">
                  <xsl:value-of select="'24'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='COP'">
                  <xsl:value-of select="'25'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='CZK'">
                  <xsl:value-of select="'26'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='DKK'">
                  <xsl:value-of select="'27'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='GHS'">
                  <xsl:value-of select="'28'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='HUF'">
                  <xsl:value-of select="'29'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='IDR'">
                  <xsl:value-of select="'30'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='ILS'">
                  <xsl:value-of select="'31'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='ISK'">
                  <xsl:value-of select="'32'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='KZT'">
                  <xsl:value-of select="'33'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='LVL'">
                  <xsl:value-of select="'34'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='MXN'">
                  <xsl:value-of select="'35'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='NZD'">
                  <xsl:value-of select="'36'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='PEN'">
                  <xsl:value-of select="'37'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='PLN'">
                  <xsl:value-of select="'38'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='RON'">
                  <xsl:value-of select="'40'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='RUB'">
                  <xsl:value-of select="'41'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='SKK'">
                  <xsl:value-of select="'42'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='TRY'">
                  <xsl:value-of select="'43'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='ARS'">
                  <xsl:value-of select="'44'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='UYU'">
                  <xsl:value-of select="'45'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='TWD'">
                  <xsl:value-of select="'46'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='BMD'">
                  <xsl:value-of select="'47'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='EEK'">
                  <xsl:value-of select="'48'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='GEL'">
                  <xsl:value-of select="'49'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='MYR'">
                  <xsl:value-of select="'51'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='SIT'">
                  <xsl:value-of select="'52'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='XAF'">
                  <xsl:value-of select="'53'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='XOF'">
                  <xsl:value-of select="'54'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='AZN'">
                  <xsl:value-of select="'55'"/>
                </xsl:when>
                <xsl:when test="normalize-space($varCurrency) ='PKR'">
                  <xsl:value-of select="'56'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </CurrencyID>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>