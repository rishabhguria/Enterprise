<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template name="Translate">
    <xsl:param name="Number" />
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="FormatDate">
    <xsl:param name="varFullDate" />
    <xsl:variable name="varYear">
      <xsl:value-of select="substring($varFullDate, string-length($varFullDate) - 3 , 4)"/>
    </xsl:variable>
    <xsl:variable name="varWithoutYear">
      <xsl:value-of select="substring($varFullDate, 1, string-length($varFullDate) - 4)"/>
    </xsl:variable>
    <xsl:variable name="varDay">
      <xsl:choose>
        <xsl:when test="$varWithoutYear &lt; 100">
          <xsl:value-of select="concat('0',substring($varWithoutYear, string-length($varWithoutYear) - 0, 1))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($varWithoutYear, string-length($varWithoutYear) - 1, string-length($varWithoutYear))"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="varMonth">
      <xsl:choose>
        <xsl:when test="$varWithoutYear &lt; 999">
          <xsl:value-of select="concat('0',substring($varWithoutYear, 1, 1))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($varWithoutYear, 1, 2)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varDrAmount">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:if test="number($varDrAmount)" >
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''" />
            </xsl:variable>
            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name="PB_FUND_NAME" select ="COL1"/>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

            <xsl:variable name = "Dramount" >
              <xsl:choose>
                <xsl:when test="$varDrAmount &gt; 0">
                  <xsl:value-of select="$varDrAmount"/>
                </xsl:when>
                <xsl:when test="$varDrAmount &lt; 0">
                  <xsl:value-of select="$varDrAmount*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varCrAmount">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL8"/>
              </xsl:call-template>
            </xsl:variable>


            <xsl:variable name = "Cramount" >
              <xsl:choose>
                <xsl:when test="varCrAmount &gt; 0">
                  <xsl:value-of select="varCrAmount"/>
                </xsl:when>
                <xsl:when test="varCrAmount &lt; 0">
                  <xsl:value-of select="varCrAmount*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varDebitCurrency">
              <xsl:value-of select="COL4"/>
            </xsl:variable>
            <xsl:variable name="varCreditCurrency">
              <xsl:value-of select="COL7"/>
            </xsl:variable>
            <DRCurrencyName>
              <xsl:choose>
                <xsl:when test="COL5!=''">
                  <xsl:value-of select="$varDebitCurrency"/>
                </xsl:when>
              </xsl:choose>
            </DRCurrencyName>


            <CRCurrencyName>
              <xsl:choose>
                <xsl:when test="COL8!=''">
                  <xsl:value-of select="$varCreditCurrency"/>
                </xsl:when>
              </xsl:choose>
            </CRCurrencyName>
            <!--<CurrencyName>
              <xsl:choose>
                <xsl:when test="COL5!=''">
                  <xsl:value-of select="$varDebitCurrency"/>
                </xsl:when>
                <xsl:when test="COL8!=''">
                  <xsl:value-of select="$varCreditCurrency"/>
                </xsl:when>
              </xsl:choose>
            </CurrencyName>-->
            <JournalEntries>
              <xsl:value-of select="concat('Cash', ':' , $varDrAmount , '|', 'Cash', ':' , $varCrAmount)"/>
            </JournalEntries>


            <xsl:variable name="varPositionStartDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="varFullDate" select="COL3"/>
              </xsl:call-template>
            </xsl:variable>
            <Date>
              <xsl:value-of select="$varPositionStartDate"/>
            </Date>


            <xsl:variable name="varCurrency">
              <xsl:choose>
                <xsl:when test="$varDebitCurrency='USD'">
                  <xsl:value-of select="($varDrAmount div $varCrAmount)" />
                </xsl:when>

                <xsl:when test="$varDebitCurrency='EUR'">
                  <xsl:value-of select="($varCrAmount div $varDrAmount)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>



            <!--<FXRate>
              <xsl:choose>
                <xsl:when test="$varCurrency &gt; 0">
                  <xsl:value-of select="$varCurrency" />
                </xsl:when>
                <xsl:when test="$varCurrency &lt; 0">
                  <xsl:value-of select="$varCurrency * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>-->

            <DRFXRate>
              <xsl:choose>
                <xsl:when test="$varCurrency &gt; 0">
                  <xsl:value-of select="$varCurrency" />
                </xsl:when>
                <xsl:when test="$varCurrency &lt; 0">
                  <xsl:value-of select="$varCurrency * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </DRFXRate>
            <CRFXRate>
              <xsl:choose>
                <xsl:when test="$varCurrency &gt; 0">
                  <xsl:value-of select="$varCurrency" />
                </xsl:when>
                <xsl:when test="$varCurrency &lt; 0">
                  <xsl:value-of select="$varCurrency * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </CRFXRate>
            

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>