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
        <xsl:if test="normalize-space(COL3) = '' or COL3 = '*' or COL3 = 'USDCCY' or COL3 = 'CADCCY'">
          <PositionMaster>

            <!--Start Of mandatory columns-->

            <AccountName>
              <xsl:value-of select="substring(COL1,1,4)"/>
            </AccountName>


            <FundID>
              <xsl:value-of select="''"/>
            </FundID>

            <!--<Symbol>
				<xsl:value-of select="''"/>
			</Symbol>-->

            <xsl:variable name = "amount" >
              <xsl:choose>
                <xsl:when test="COL12 &gt; 0">
                  <xsl:value-of select="COL12"/>
                </xsl:when>
                <xsl:when test="COL12 &lt; 0">
                  <xsl:value-of select="(-1)*COL12"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <Date>
              <xsl:value-of select="COL4"/>
            </Date>

            <xsl:variable name = "varUnderlying_LME" >
              <xsl:call-template name="tempMetalSymbolCode">
                <xsl:with-param name="paramMetalSymbol" select="COL5" />
              </xsl:call-template>
            </xsl:variable>

            <Description>
              <xsl:value-of select ="COL20"/>
            </Description>

            <CurrencyName>
              <xsl:value-of select ="COL14"/>
            </CurrencyName>

            <CurrencyID>
              <xsl:value-of select ="''"/>
            </CurrencyID>

            <JournalEntries>
              <xsl:choose>
                <!-- Note
							  
							  * Sub account acronyms being used, must exists in db. New sub account may be added through cash management's account setup UI. 
							  * Multiple account will be seperated by separetor ; i.e- 
								concat('Cash:' , $amount , ';Transaction_Levy:' , $amount , '|Interest_Income:' , $amount, ';Transaction_Levy:' , $amount).
							  * Separator | is used to separate out the Dr entries from cr entries, Initially Dr entries and then Cr enties.
							  
							  -->
                <!-- Amount positive-->
                <xsl:when test="COL12 &gt; 0">
                  <xsl:value-of select="concat('Cash:' , $amount , '|', COL2, ':' , $amount)"/>
                </xsl:when>
                <!-- Amount negative-->
                <xsl:when  test="COL12 &lt; 0">
                  <xsl:value-of select="concat(COL2, ':' , $amount , '|Cash:' , $amount)"/>
                </xsl:when>
                <!--<xsl:otherwise>
							  <xsl:value-of select="concat('Interest_Expense:' , $amount , '|Cash:' , $amount)"/>
						  </xsl:otherwise>-->
              </xsl:choose>
            </JournalEntries>

            <!--End Of mandatory columns-->

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
