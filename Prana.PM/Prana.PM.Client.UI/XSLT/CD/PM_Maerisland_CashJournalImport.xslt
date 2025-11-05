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
        <xsl:if test="normalize-space(COL1) != 'Account Name'">
          <PositionMaster>

            <!--Start Of mandatory columns-->

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL1"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Jefferies']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name = "PRANA_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="COL7='Withdraw'">
                  <xsl:value-of select="'CASH-WDL'"/>
                </xsl:when>
                <xsl:when test="COL7='Deposit'">
                  <xsl:value-of select="'CASH-DEP'"/>
                </xsl:when>
                <xsl:when test="COL7='Interest'">
                  <xsl:value-of select="'Interest_Income'"/>
                </xsl:when>
                <xsl:when test="COL7='Management Fee'">
                  <xsl:value-of select="'MGMT-FEE'"/>
                </xsl:when>
                <xsl:when test="COL7='Spot Fx'">
                  <xsl:value-of select="'Spot_FX'"/>
                </xsl:when>
                <xsl:when test="COL7='Return of Capital' or COL7='SL Rebate/Fee'">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Misc_Fees'"/>
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


            <FundID>
              <xsl:value-of select="0"/>
            </FundID>

            <xsl:variable name = "amount" >
              <xsl:choose>
                <xsl:when test="COL7='Withdraw' or COL7='Deposit' or COL7='Spot FX'">
                  <xsl:choose>
                    <xsl:when test="COL8 &gt; 0">
                      <xsl:value-of select="COL8"/>
                    </xsl:when>
                    <xsl:when test="COL8 &lt; 0">
                      <xsl:value-of select="COL8*(-1)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="COL32 &gt; 0">
                      <xsl:value-of select="COL32"/>
                    </xsl:when>
                    <xsl:when test="COL32 &lt; 0">
                      <xsl:value-of select="COL32*(-1)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <Date>
              <xsl:value-of select="COL2"/>
            </Date>

            <Description>
              <xsl:value-of select ="COL7"/>
            </Description>

            <CurrencyName>
              <xsl:choose>
                <xsl:when test="COL24=''">
                  <xsl:value-of select="'USD'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL24"/>
                </xsl:otherwise>
              </xsl:choose>
            </CurrencyName>

            <CurrencyID>
              <xsl:value-of select ="0"/>
            </CurrencyID>
			  

						<JournalEntries>

              <!-- Note
							  
							  * Sub account acronyms being used, must exists in db. New sub account may be added through cash management's account setup UI. 
							  * Multiple account will be seperated by separetor ; i.e- 
								concat('Cash:' , $amount , ';Transaction_Levy:' , $amount , '|Interest_Income:' , $amount, ';Transaction_Levy:' , $amount).
							  * Separator | is used to separate out the Dr entries from cr entries, Initially Dr entries and then Cr enties.
							  
							  -->
              <!-- Amount positive-->
              <xsl:choose>
                <xsl:when test="COL7='Withdraw' or COL7='Deposit' or COL7='Spot FX'">
                  <xsl:choose>
                  <xsl:when test="COL8 &lt; 0">
                      <xsl:value-of select="concat('Cash:' , $amount , '|', $PRANA_ACRONYM_NAME, ':' , $amount)"/>
                    </xsl:when>
                  <!-- Amount negative-->
                  <xsl:when  test="COL8 &gt; 0">
                    <xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $amount , '|Cash:' , $amount)"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="COL32 &lt; 0">
                      <xsl:value-of select="concat('Cash:' , $amount , '|', $PRANA_ACRONYM_NAME, ':' , $amount)"/>
                    </xsl:when>
                    <!-- Amount negative-->
                    <xsl:when  test="COL32 &gt; 0">
                      <xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $amount , '|Cash:' , $amount)"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:otherwise>
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
