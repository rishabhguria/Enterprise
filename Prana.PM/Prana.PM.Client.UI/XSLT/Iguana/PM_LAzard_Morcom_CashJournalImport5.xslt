<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template name="tempMetalSymbolCode">
    <xsl:param name="paramMetalSymbol"/>
    <!-- 1 characters for metal code -->
    <!--  e.g. A represents A = aluminium-->
    <xsl:choose>
      <xsl:when test ="$paramMetalSymbol='U S DOLLAR'">
        <xsl:value-of select ="'USD'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='UK POUND STERLING'">
        <xsl:value-of select ="'GBP'"/>
      </xsl:when>

      <xsl:when test ="$paramMetalSymbol='EURO'">
        <xsl:value-of select ="'EUR'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='AUSTRALIAN DOLLAR'">
        <xsl:value-of select ="'AUD'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='HONG KONG DOLLAR'">
        <xsl:value-of select ="'HKD'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='INDONESIAN RUPIAH'">
        <xsl:value-of select ="'IDR'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='JAPANESE YEN'">
        <xsl:value-of select ="'JPY'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='MALAYSIAN RINGGIT'">
        <xsl:value-of select ="'MYR'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='NEW ZEALAND DOLLAR'">
        <xsl:value-of select ="'NZD'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='PHILIPPINO PESO'">
        <xsl:value-of select ="'PBP'"/>
      </xsl:when>
      <xsl:when test ="$paramMetalSymbol='SINGAPORE DOLLAR'">
        <xsl:value-of select ="'SGD'"/>
      </xsl:when>

      <xsl:when test ="$paramMetalSymbol='THAI BAHT'">
        <xsl:value-of select ="'THB'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="  COL17 = 'CEQU'">
          <PositionMaster>

            <!--Start Of mandatory columns-->

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL15"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Morcom']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name = "PB_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="number(COL10)">
                  <xsl:value-of select="COL12"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL10"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="COL40 = 'INT' and COL36 &gt; 0">
                  <xsl:value-of select="'Interest_Income'"/>
                </xsl:when>
                <xsl:when test="COL40 = 'INT' and COL36 &lt; 0">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>
                <xsl:when test="(COL40 = 'JRL' or COL40 = 'DBT') and COL36 &gt; 0">
                  <xsl:value-of select="'CASH-DEP'"/>
                </xsl:when>
                <xsl:when test="(COL40 = 'JRL' or COL40 = 'DBT') and COL36 &lt; 0">
                  <xsl:value-of select="'CASH-WDL'"/>
                </xsl:when>
                <xsl:when test="COL40 = 'REB' or COL40 = 'BME'or COL40='MGN' or COL40 = 'FEE'">
                  <xsl:value-of select="'MISC-EXP'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME = ''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <!--<FundID>
              <xsl:value-of select="COL2"/>
            </FundID>-->

            <xsl:variable name = "amount" >
              <xsl:choose>
                <xsl:when test="COL36 &gt; 0">
                  <xsl:value-of select="COL36"/>
                </xsl:when>
                <xsl:when test="COL36 &lt; 0">
                  <xsl:value-of select="COL36*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <Date>
              <xsl:value-of select="COL11"/>
            </Date>

            <Description>
              <xsl:choose>
                <xsl:when test="COL40 = 0">
                  <xsl:value-of select="COL28"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL40"/>
                </xsl:otherwise>
              </xsl:choose>
            </Description>

            <CurrencyName>
              <xsl:value-of select ="COL2"/>
            </CurrencyName>

            <CurrencyID>
              <xsl:value-of select ="0"/>
            </CurrencyID>

            <JournalEntries>
              <xsl:choose>
                <!-- Note
							  
							  * Sub account acronyms being used, must exists in db. New sub account may be added through cash management's account setup UI. 
							  * Multiple account will be seperated by separetor ; i.e- 
								concat('Cash:' , $amount , ';Transaction_Levy:' , $amount , '|Interest_Income:' , $amount, ';Transaction_Levy:' , $amount).
							  * Separator | is used to separate out the Dr entries from cr entries, Initially Dr entries and then Cr enties.
							  
							  -->
                <!-- Amount negative-->
                <xsl:when test="COL36 &lt; 0">
                  <xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $amount, '|Cash:', $amount )"/>
                </xsl:when>
                <!-- Amount positive-->
                <xsl:when  test="COL36 &gt; 0">
                  <xsl:value-of select="concat( 'Cash:' , $amount, '|', $PRANA_ACRONYM_NAME,':' , $amount)"/>
                </xsl:when>

              </xsl:choose>
            </JournalEntries>

            <!--End Of mandatory columns-->

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
